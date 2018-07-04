// -----------------------------------------------------------------------
// <copyright file="ManageController.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.Apis.Controllers
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using DotNetCoreKit.Apis.Extensions;
    using DotNetCoreKit.Apis.Models.ManageViewModels;
    using DotNetCoreKit.Models.Domain;
    using DotNetCoreKit.Utilities.Extensions;
    using DotNetCoreKit.Utilities.Interfaces;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    /// <inheritdoc />
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    [Route("[controller]/[action]")]
    public class ManageController : Controller
    {
        private const string AuthenicatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

        /// <summary>
        /// Initializes a new instance of the <see cref="ManageController"/> class.
        /// </summary>
        /// <param name="userManager">Used to manage user in a persistence store.</param>
        /// <param name="signInManager">Used to manage user sign ins.</param>
        /// <param name="emailSender">Used to send email for account confirmation and password reset.</param>
        /// <param name="logger">Used to log exceptions or messages.</param>
        /// <param name="urlEncoder">encoder for urls</param>
        public ManageController(
          UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager,
          IEmailSender emailSender,
          ILogger<ManageController> logger,
          UrlEncoder urlEncoder)
        {
            UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            SignInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            UrlEncoder = urlEncoder ?? throw new ArgumentNullException(nameof(urlEncoder));
            EmailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        }

        /// <summary>
        /// Gets or sets message
        /// </summary>
        [TempData]
        private string StatusMessage { get; set; }

        private UserManager<ApplicationUser> UserManager { get; }

        private SignInManager<ApplicationUser> SignInManager { get; }

        private ILogger Logger { get; }

        private UrlEncoder UrlEncoder { get; }

        private IEmailSender EmailSender { get; }

        /// <summary>
        /// Used by api page to show current logged in user.
        /// </summary>
        /// <returns>Returns current user details page..</returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await UserManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }

            var model = new IndexViewModel
            {
                Username = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsEmailConfirmed = user.EmailConfirmed,
                StatusMessage = StatusMessage,
            };

            return View(model);
        }

        /// <summary>
        /// Identity user (application) detail update method.
        /// </summary>
        /// <param name="model">Model that holds the user details.</param>
        /// <returns>Either success or error message when attempting to update user details.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(IndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }

            var email = user.Email;
            if (model.Email != email)
            {
                var setEmailResult = await UserManager.SetEmailAsync(user, model.Email);
                if (!setEmailResult.Succeeded)
                {
                    throw new ApplicationException($"Unexpected error occurred setting email for user with ID '{user.Id}'.");
                }
            }

            var phoneNumber = user.PhoneNumber;
            if (model.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await UserManager.SetPhoneNumberAsync(user, model.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    throw new ApplicationException($"Unexpected error occurred setting phone number for user with ID '{user.Id}'.");
                }
            }

            StatusMessage = "Your profile has been updated";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Load the sends verification email page.
        /// </summary>
        /// <param name="model">hello</param>
        /// <returns>Returns success or failure message for sending verification email.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendVerificationEmail(IndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }

            var code = await UserManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
            var email = user.Email;
            await EmailSender.SendEmailConfirmationAsync(email, callbackUrl);

            StatusMessage = "Verification email sent. Please check your email.";
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Change current user password page.
        /// </summary>
        /// <returns>Returns page to change password or error message with user not found.</returns>
        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await UserManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }

            var hasPassword = await UserManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                return RedirectToAction(nameof(SetPassword));
            }

            var model = new ChangePasswordViewModel { StatusMessage = StatusMessage };
            return View(model);
        }

        /// <summary>
        /// Change current user password.
        /// </summary>
        /// <param name="model">hello</param>
        /// <returns>Changes the user password success or failure message.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await UserManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                AddErrors(changePasswordResult);
                return View(model);
            }

            await SignInManager.SignInAsync(user, isPersistent: false);
            Logger.LogInformation("User changed their password successfully.");
            StatusMessage = "Your password has been changed.";

            return RedirectToAction(nameof(ChangePassword));
        }

        /// <summary>
        /// Sets the users hashed password.
        /// </summary>
        /// <returns>Page with hashed password or error message.</returns>
        [HttpGet]
        public async Task<IActionResult> SetPassword()
        {
            var user = await UserManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }

            var hasPassword = await UserManager.HasPasswordAsync(user);

            if (hasPassword)
            {
                return RedirectToAction(nameof(ChangePassword));
            }

            var model = new SetPasswordViewModel { StatusMessage = StatusMessage };
            return View(model);
        }

        /// <summary>
        /// Sets the password provided on the current user.
        /// </summary>
        /// <param name="model">Set password view model</param>
        /// <returns>Success or failure messages returned.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }

            var addPasswordResult = await UserManager.AddPasswordAsync(user, model.NewPassword);
            if (!addPasswordResult.Succeeded)
            {
                AddErrors(addPasswordResult);
                return View(model);
            }

            await SignInManager.SignInAsync(user, isPersistent: false);
            StatusMessage = "Your password has been set.";

            return RedirectToAction(nameof(SetPassword));
        }

        /// <summary>
        /// Show external log in page.
        /// </summary>
        /// <returns>Either external login page can be user or not.</returns>
        [HttpGet]
        public async Task<IActionResult> ExternalLogins()
        {
            var user = await UserManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }

            var model = new ExternalLoginsViewModel { CurrentLogins = await UserManager.GetLoginsAsync(user) };
            model.OtherLogins = (await SignInManager.GetExternalAuthenticationSchemesAsync())
                .Where(auth => model.CurrentLogins.All(ul => auth.Name != ul.LoginProvider))
                .ToList();
            model.ShowRemoveButton = await UserManager.HasPasswordAsync(user) || model.CurrentLogins.Count > 1;
            model.StatusMessage = StatusMessage;

            return View(model);
        }

        /// <summary>
        /// External login provider is invoked.
        /// </summary>
        /// <param name="provider">hello</param>
        /// <returns>Results from external provider's login attempt.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LinkLogin(string provider)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            // Request a redirect to the external login provider to link a login for the current user
            var redirectUrl = Url.Action(nameof(LinkLoginCallback));
            var properties = SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, UserManager.GetUserId(User));
            return new ChallengeResult(provider, properties);
        }

        /// <summary>
        /// Register external login info for a user and test it.
        /// </summary>
        /// <returns>Returns success or failure message after external login is registered.</returns>
        [HttpGet]
        public async Task<IActionResult> LinkLoginCallback()
        {
            var user = await UserManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }

            var info = await SignInManager.GetExternalLoginInfoAsync(user.Id.ToString());
            if (info == null)
            {
                throw new ApplicationException($"Unexpected error occurred loading external login info for user with ID '{user.Id}'.");
            }

            var result = await UserManager.AddLoginAsync(user, info);
            if (!result.Succeeded)
            {
                throw new ApplicationException($"Unexpected error occurred adding external login for user with ID '{user.Id}'.");
            }

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            StatusMessage = "The external login was added.";
            return RedirectToAction(nameof(ExternalLogins));
        }

        /// <summary>
        /// Removes the external third party authentication provider from current user.
        /// </summary>
        /// <param name="model">Remove external login view model.</param>
        /// <returns>Returns user back to the external login registration page.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveLogin(RemoveLoginViewModel model)
        {
            var user = await UserManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }

            var result = await UserManager.RemoveLoginAsync(user, model.LoginProvider, model.ProviderKey);
            if (!result.Succeeded)
            {
                throw new ApplicationException($"Unexpected error occurred removing external login for user with ID '{user.Id}'.");
            }

            await SignInManager.SignInAsync(user, isPersistent: false);
            StatusMessage = "The external login was removed.";
            return RedirectToAction(nameof(ExternalLogins));
        }

        /// <summary>
        /// Shows the two factor authenticator page for current user.
        /// </summary>
        /// <returns>Returns page with current user's two factor authentication details.</returns>
        [HttpGet]
        public async Task<IActionResult> TwoFactorAuthentication()
        {
            var user = await UserManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }

            var model = new TwoFactorAuthenticationViewModel
            {
                HasAuthenticator = await UserManager.GetAuthenticatorKeyAsync(user) != null,
                Is2FaEnabled = user.TwoFactorEnabled,
                RecoveryCodesLeft = await UserManager.CountRecoveryCodesAsync(user),
            };

            return View(model);
        }

        /// <summary>
        /// Provides disabling of two factor authentication warning.
        /// </summary>
        /// <returns>Redirects user to disabling two factor authentication page when successful or error mess when it fails.</returns>
        [HttpGet]
        public async Task<IActionResult> Disable2FaWarning()
        {
            var user = await UserManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }

            if (!user.TwoFactorEnabled)
            {
                throw new ApplicationException($"Unexpected error occurred disabling 2FA for user with ID '{user.Id}'.");
            }

            return View(nameof(Disable2Fa));
        }

        /// <summary>
        /// Provides disabling of two factor authentication.
        /// </summary>
        /// <returns>Redirects user to two factor authentication page when successful or error message if it fails.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Disable2Fa()
        {
            var user = await UserManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }

            var disable2FaResult = await UserManager.SetTwoFactorEnabledAsync(user, false);
            if (!disable2FaResult.Succeeded)
            {
                throw new ApplicationException($"Unexpected error occurred disabling 2FA for user with ID '{user.Id}'.");
            }

            Logger.LogInformation("User with ID {UserId} has disabled 2fa.", user.Id);
            return RedirectToAction(nameof(TwoFactorAuthentication));
        }

        /// <summary>
        /// Gets the page to enable authentication for current user.
        /// </summary>
        /// <returns>Enable authenticator page.</returns>
        [HttpGet]
        public async Task<IActionResult> EnableAuthenticator()
        {
            var user = await UserManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }

            var unformattedKey = await UserManager.GetAuthenticatorKeyAsync(user);
            if (string.IsNullOrEmpty(unformattedKey))
            {
                await UserManager.ResetAuthenticatorKeyAsync(user);
                unformattedKey = await UserManager.GetAuthenticatorKeyAsync(user);
            }

            var model = new EnableAuthenticatorViewModel
            {
                SharedKey = FormatKey(unformattedKey),
                AuthenticatorUri = GenerateQrCodeUri(user.Email, unformattedKey),
            };

            return View(model);
        }

        /// <summary>
        /// Enable the two factor authentication.
        /// </summary>
        /// <param name="model">Enable authenticator view model.</param>
        /// <returns>Returns generated recovery code.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnableAuthenticator(EnableAuthenticatorViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }

            // Strip spaces and hypens
            var verificationCode = model.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

            var is2FaTokenValid = await UserManager.VerifyTwoFactorTokenAsync(
                user, UserManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

            if (!is2FaTokenValid)
            {
                ModelState.AddModelError("model.Code", "Verification code is invalid.");
                return View(model);
            }

            await UserManager.SetTwoFactorEnabledAsync(user, true);
            Logger.LogInformation("User with ID {UserId} has enabled 2FA with an authenticator app.", user.Id);
            return RedirectToAction(nameof(GenerateRecoveryCodes));
        }

        /// <summary>
        /// Show reset two-factor authentication page.
        /// </summary>
        /// <returns>View returned for authentication.</returns>
        [HttpGet]
        public IActionResult ResetAuthenticatorWarning() => View(nameof(ResetAuthenticator));

        /// <summary>
        /// Disable the two factor authentications for current user.
        /// </summary>
        /// <returns>Resets the authenticator app key for current user.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetAuthenticator()
        {
            var user = await UserManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }

            await UserManager.SetTwoFactorEnabledAsync(user, false);
            await UserManager.ResetAuthenticatorKeyAsync(user);
            Logger.LogInformation("User with id '{UserId}' has reset their authentication app key.", user.Id);

            return RedirectToAction(nameof(EnableAuthenticator));
        }

        /// <summary>
        /// Request recovery codes for current user.
        /// </summary>
        /// <returns>Returns the generated recovery code after attaching it to current user.</returns>
        [HttpGet]
        public async Task<IActionResult> GenerateRecoveryCodes()
        {
            var user = await UserManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }

            if (!user.TwoFactorEnabled)
            {
                throw new ApplicationException($"Cannot generate recovery codes for user with ID '{user.Id}' as they do not have 2FA enabled.");
            }

            var recoveryCodes = await UserManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
            var model = new GenerateRecoveryCodesViewModel { RecoveryCodes = recoveryCodes.ToArray() };

            Logger.LogInformation("User with ID {UserId} has generated new 2FA recovery codes.", user.Id);

            return View(model);
        }

        private static string FormatKey(string unformattedKey)
        {
            var result = new StringBuilder();
            var currentPosition = 0;

            while (currentPosition + 4 < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
                currentPosition += 4;
            }

            if (currentPosition < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition));
            }

            return result.ToString().ToLowerInvariant();
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private string GenerateQrCodeUri(string email, string unformattedKey)
        {
            return string.Format(
                AuthenicatorUriFormat,
                UrlEncoder.Encode("DotNetCoreKit.Apis"),
                UrlEncoder.Encode(email),
                unformattedKey);
        }
    }
}