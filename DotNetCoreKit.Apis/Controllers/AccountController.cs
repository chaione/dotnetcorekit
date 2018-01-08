// -----------------------------------------------------------------------
// <copyright file="AccountController.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.Apis.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using DotNetCoreKit.Apis.Extensions;
    using DotNetCoreKit.Apis.Models.AccountViewModels;
    using DotNetCoreKit.Models.Domain;
    using DotNetCoreKit.Utilities.Extensions;
    using DotNetCoreKit.Utilities.Interfaces;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    /// <inheritdoc />
    /// <summary>
    /// The Account controller.
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="userManager">Used to manage user in a persistence store.</param>
        /// <param name="signInManager">Used to manage user sign ins.</param>
        /// <param name="emailSender">Used to send email for account confirmation and password reset.</param>
        /// <param name="logger">Used to log exceptions or messages.</param>
        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<AccountController> logger)
        {
            UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            SignInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            EmailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets or sets test
        /// </summary>
        [TempData]
        private string ErrorMessage { get; set; }

        /// <summary>
        /// Gets private user manager reference.
        /// </summary>
        private UserManager<ApplicationUser> UserManager { get; }

        /// <summary>
        /// Gets private sign in manager reference.
        /// </summary>
        private SignInManager<ApplicationUser> SignInManager { get; }

        /// <summary>
        /// Gets private email sender reference.
        /// </summary>
        private IEmailSender EmailSender { get; }

        /// <summary>
        /// Gets private logger reference.
        /// </summary>
        private ILogger Logger { get; }

        /// <summary>
        /// Provides basic login page.
        /// Forces logout of any existing user that is logged into current context as well.
        /// </summary>
        /// <param name="returnUrl">The URL that should be returned if the action fails or when it completes.</param>
        /// <returns>Returns empty view</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// Used to authenticate a user.
        /// </summary>
        /// <param name="model">Input view model for this action.</param>
        /// <param name="returnUrl">The URL that should be returned if the action fails or when it completes.</param>
        /// <returns>Returns the authenticated user or an error.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                // If we entered this block, something failed, redisplay form
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                Logger.LogInformation("User logged in.");
                return RedirectToLocal(returnUrl);
            }

            if (result.RequiresTwoFactor)
            {
                return RedirectToAction(nameof(LoginWith2Fa), new { returnUrl, model.RememberMe });
            }

            if (result.IsLockedOut)
            {
                Logger.LogWarning("User account locked out.");
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }
        }

        /// <summary>
        /// Returns the two-factor authentication page.
        /// </summary>
        /// <param name="rememberMe">Flag to remember the logged in user, so cookie can be persisted between sessions.</param>
        /// <param name="returnUrl">The URL that should be returned if the action fails or when it completes.</param>
        /// <returns>Returns the two-factor authentication page to authenticate with.</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWith2Fa(bool rememberMe, string returnUrl = null)
        {
            // Ensure the user has gone through the user name & password screen first
            var user = await SignInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            var model = new LoginWith2FaViewModel { RememberMe = rememberMe };
            ViewData["ReturnUrl"] = returnUrl;

            return View(model);
        }

        /// <summary>
        /// Performs the two-factor authentication action.
        /// </summary>
        /// <param name="model">Input view model for this action.</param>
        /// <param name="rememberMe">Flag to remember the logged in user, so cookie can be persisted between sessions.</param>
        /// <param name="returnUrl">The URL that should be returned if the action fails or when it completes.</param>
        /// <returns>Returns appropriate response based on validation of authenticating user.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWith2Fa(LoginWith2FaViewModel model, bool rememberMe, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await SignInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }

            var authenticatorCode = model.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result = await SignInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, model.RememberMachine);

            if (result.Succeeded)
            {
                Logger.LogInformation("User with ID {UserId} logged in with 2fa.", user.Id);
                return RedirectToLocal(returnUrl);
            }
            else if (result.IsLockedOut)
            {
                Logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                Logger.LogWarning("Invalid authenticator code entered for user with ID {UserId}.", user.Id);
                ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
                return View();
            }
        }

        /// <summary>
        /// Gets the recovery page.
        /// </summary>
        /// <param name="returnUrl">The URL that should be returned if the action fails or when it completes.</param>
        /// <returns>Returns view for recovery page.</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWithRecoveryCode(string returnUrl = null)
        {
            // Ensure the user has gone through the user name & password screen first
            var user = await SignInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        /// <summary>
        /// Performs the account recovery action.
        /// </summary>
        /// <param name="model">Input view model for this action.</param>
        /// <param name="returnUrl">The URL that should be returned if the action fails or when it completes.</param>
        /// <returns>Outcome of recovery attempt, with appropriate error messages.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWithRecoveryCode(LoginWithRecoveryCodeViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await SignInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            var recoveryCode = model.RecoveryCode.Replace(" ", string.Empty);

            var result = await SignInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);

            if (result.Succeeded)
            {
                Logger.LogInformation("User with ID {UserId} logged in with a recovery code.", user.Id);
                return RedirectToLocal(returnUrl);
            }

            if (result.IsLockedOut)
            {
                Logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                Logger.LogWarning("Invalid recovery code entered for user with ID {UserId}", user.Id);
                ModelState.AddModelError(string.Empty, "Invalid recovery code entered.");
                return View();
            }
        }

        /// <summary>
        /// Returns the account lock out page.
        /// </summary>
        /// <returns>Returns the resulting view for the executed action.</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout() => View();

        /// <summary>
        /// Returns the account registration page.
        /// </summary>
        /// <param name="returnUrl">The URL that should be returned if the action fails or when it completes.</param>
        /// <returns>Returns the resulting view for the executed action.</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// Used for registering a new user account.
        /// </summary>
        /// <param name="model">Input view model for this action.</param>
        /// <param name="returnUrl">The URL that should be returned if the action fails or when it completes.</param>
        /// <returns>Returns the resulting view for the executed action.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await UserManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                Logger.LogInformation("User created a new account with password.");

                var code = await UserManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                await EmailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);

                await SignInManager.SignInAsync(user, isPersistent: false);
                Logger.LogInformation("User created a new account with password.");
                return RedirectToLocal(returnUrl);
            }

            AddErrors(result);

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        /// <summary>
        /// Used to log user out and invaliate their cookie.
        /// </summary>
        /// <returns>User back to home page.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();
            Logger.LogInformation("User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        /// <summary>
        /// Used to authenticate with external login provider.
        /// </summary>
        /// <param name="provider">External third party provider for authentication.</param>
        /// <param name="returnUrl">The URL that should be returned if the action fails or when it completes.</param>
        /// <returns>Returns the resulting view for the executed action.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        /// <summary>
        /// Provides external login page.
        /// </summary>
        /// <param name="returnUrl">The URL that should be returned if the action fails or when it completes.</param>
        /// <param name="remoteError">Error page for the external third party authentication provider.</param>
        /// <returns>Returns the resulting view for the executed action.</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToAction(nameof(Login));
            }

            var info = await SignInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await SignInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                Logger.LogInformation("User logged in with {Name} provider.", info.LoginProvider);
                return RedirectToLocal(returnUrl);
            }

            if (result.IsLockedOut)
            {
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return View("ExternalLogin", new ExternalLoginViewModel { Email = email });
            }
        }

        /// <summary>
        /// Performs the external login and registers the user into our system for future use.
        /// </summary>
        /// <param name="model">Input view model for this action.</param>
        /// <param name="returnUrl">The URL that should be returned if the action fails or when it completes.</param>
        /// <returns>Returns the resulting view for the executed action.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await SignInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    throw new ApplicationException("Error loading external login information during confirmation.");
                }

                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false);
                        Logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                        return RedirectToLocal(returnUrl);
                    }
                }

                AddErrors(result);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(nameof(ExternalLogin), model);
        }

        /// <summary>
        /// Provides action to confirm the user's email with provided code.
        /// </summary>
        /// <param name="userId">Database id used to identify target user.</param>
        /// <param name="code">One time use confirmation code provided for email confirmation.</param>
        /// <returns>Returns the resulting view for the executed action.</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }

            var result = await UserManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        /// <summary>
        /// Returns forgot password page.
        /// </summary>
        /// <returns>Returns the resulting view for the executed action.</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        /// <summary>
        /// Performs the forgot password actions and allows user to reset their password.
        /// </summary>
        /// <param name="model">Input view model for this action.</param>
        /// <returns>Returns the resulting view for the executed action.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Something failed, redisplay form
                return View(model);
            }

            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null || !(await UserManager.IsEmailConfirmedAsync(user)))
            {
                // Don't reveal that the user does not exist or is not confirmed
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            // For more information on how to enable account confirmation and password reset please
            // visit https://go.microsoft.com/fwlink/?LinkID=532713
            var code = await UserManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);
            var emailLink = $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>";

            await EmailSender.SendEmailAsync(model.Email, "Reset Password", emailLink);
            return RedirectToAction(nameof(ForgotPasswordConfirmation));
        }

        /// <summary>
        /// Provides the confirmation page for forgot password reset.
        /// </summary>
        /// <returns>Returns the resulting view for the executed action.</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation() => View();

        /// <summary>
        /// Reset password page.
        /// </summary>
        /// <param name="code">One time use confirmation code provided for password reset request validation.</param>
        /// <returns>Returns the resulting view for the executed action.</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                throw new ApplicationException("A code must be supplied for password reset.");
            }

            var model = new ResetPasswordViewModel { Code = code };
            return View(model);
        }

        /// <summary>
        /// Resets a user password or returns appropriate error.
        /// </summary>
        /// <param name="model">Input view model for this action.</param>
        /// <returns>Returns the resulting view for the executed action.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }

            var result = await UserManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }

            AddErrors(result);
            return View();
        }

        /// <summary>
        /// Reset password confirmation page.
        /// </summary>
        /// <returns>Returns the resulting view for the executed action.</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation() => View();

        /// <summary>
        /// Access denied page.
        /// </summary>
        /// <returns>Returns the resulting view for the executed action.</returns>
        [HttpGet]
        public IActionResult AccessDenied() => View();

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}