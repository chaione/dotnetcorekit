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
    [Authorize]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IEmailSender emailSender;
        private readonly ILogger logger;

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
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
            this.logger = logger;
        }

        /// <summary>
        /// Gets or sets test
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// tes
        /// </summary>
        /// <param name="returnUrl">tt</param>
        /// <returns>te</returns>
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
        /// test
        /// </summary>
        /// <param name="model">te</param>
        /// <param name="returnUrl">ts</param>
        /// <returns>tsa</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    logger.LogInformation("User logged in.");
                    return RedirectToLocal(returnUrl);
                }

                if (result.RequiresTwoFactor)
                {
                    return RedirectToAction(nameof(LoginWith2Fa), new { returnUrl, model.RememberMe });
                }

                if (result.IsLockedOut)
                {
                    logger.LogWarning("User account locked out.");
                    return RedirectToAction(nameof(Lockout));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        /// <summary>
        /// test
        /// </summary>
        /// <param name="rememberMe">tt</param>
        /// <param name="returnUrl">as</param>
        /// <returns>td</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWith2Fa(bool rememberMe, string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            var model = new LoginWith2FaViewModel { RememberMe = rememberMe };
            ViewData["ReturnUrl"] = returnUrl;

            return View(model);
        }

        /// <summary>
        /// tses
        /// </summary>
        /// <param name="model">rr</param>
        /// <param name="rememberMe">dd</param>
        /// <param name="returnUrl">sd</param>
        /// <returns>dfasd</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWith2Fa(LoginWith2FaViewModel model, bool rememberMe, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            var authenticatorCode = model.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result = await signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, model.RememberMachine);

            if (result.Succeeded)
            {
                logger.LogInformation("User with ID {UserId} logged in with 2fa.", user.Id);
                return RedirectToLocal(returnUrl);
            }
            else if (result.IsLockedOut)
            {
                logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                logger.LogWarning("Invalid authenticator code entered for user with ID {UserId}.", user.Id);
                ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
                return View();
            }
        }

        /// <summary>
        /// adf
        /// </summary>
        /// <param name="returnUrl">re</param>
        /// <returns>dd</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWithRecoveryCode(string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        /// <summary>
        /// aer
        /// </summary>
        /// <param name="model">re</param>
        /// <param name="returnUrl">er</param>
        /// <returns>asdf</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWithRecoveryCode(LoginWithRecoveryCodeViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            var recoveryCode = model.RecoveryCode.Replace(" ", string.Empty);

            var result = await signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);

            if (result.Succeeded)
            {
                logger.LogInformation("User with ID {UserId} logged in with a recovery code.", user.Id);
                return RedirectToLocal(returnUrl);
            }

            if (result.IsLockedOut)
            {
                logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                logger.LogWarning("Invalid recovery code entered for user with ID {UserId}", user.Id);
                ModelState.AddModelError(string.Empty, "Invalid recovery code entered.");
                return View();
            }
        }

        /// <summary>
        /// asdf
        /// </summary>
        /// <returns>result</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }

        /// <summary>
        /// re
        /// </summary>
        /// <param name="returnUrl">dd</param>
        /// <returns>result</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// twet
        /// </summary>
        /// <param name="model">dd</param>
        /// <param name="returnUrl">ss</param>
        /// <returns>result</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    logger.LogInformation("User created a new account with password.");

                    var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                    await emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);

                    await signInManager.SignInAsync(user, isPersistent: false);
                    logger.LogInformation("User created a new account with password.");
                    return RedirectToLocal(returnUrl);
                }

                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        /// <summary>
        /// dfd
        /// </summary>
        /// <returns>result</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            logger.LogInformation("User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        /// <summary>
        /// asdf
        /// </summary>
        /// <param name="provider">dd</param>
        /// <param name="returnUrl">df</param>
        /// <returns>result</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        /// <summary>
        /// asdf
        /// </summary>
        /// <param name="returnUrl">ed</param>
        /// <param name="remoteError">fs</param>
        /// <returns>result</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToAction(nameof(Login));
            }

            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                logger.LogInformation("User logged in with {Name} provider.", info.LoginProvider);
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
        /// erta
        /// </summary>
        /// <param name="model">de</param>
        /// <param name="returnUrl">re</param>
        /// <returns>result</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    throw new ApplicationException("Error loading external login information during confirmation.");
                }

                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await signInManager.SignInAsync(user, isPersistent: false);
                        logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                        return RedirectToLocal(returnUrl);
                    }
                }

                AddErrors(result);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(nameof(ExternalLogin), model);
        }

        /// <summary>
        /// tes
        /// </summary>
        /// <param name="userId">de</param>
        /// <param name="code">df</param>
        /// <returns>result</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }

            var result = await userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        /// <summary>
        /// df
        /// </summary>
        /// <returns>result</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        /// <summary>
        /// sd
        /// </summary>
        /// <param name="model">fd</param>
        /// <returns>result</returns>
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

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await userManager.IsEmailConfirmedAsync(user)))
            {
                // Don't reveal that the user does not exist or is not confirmed
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            // For more information on how to enable account confirmation and password reset please
            // visit https://go.microsoft.com/fwlink/?LinkID=532713
            var code = await userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);
            var emailLink = $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>";

            await emailSender.SendEmailAsync(model.Email, "Reset Password", emailLink);
            return RedirectToAction(nameof(ForgotPasswordConfirmation));
        }

        /// <summary>
        /// dfd
        /// </summary>
        /// <returns>result</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        /// <summary>
        /// dsf
        /// </summary>
        /// <param name="code">fd</param>
        /// <returns>result</returns>
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
        /// ds
        /// </summary>
        /// <param name="model">de</param>
        /// <returns>result</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }

            var result = await userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }

            AddErrors(result);
            return View();
        }

        /// <summary>
        /// sdsd
        /// </summary>
        /// <returns>result</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        /// <summary>
        /// asdfa
        /// </summary>
        /// <returns>result</returns>
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

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
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
    }
}