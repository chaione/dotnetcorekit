﻿// -----------------------------------------------------------------------
// <copyright file="AuthenticateController.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.Apis.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;

    using DotNetCoreKit.Apis.Models;
    using DotNetCoreKit.Apis.Models.AccountViewModels;
    using DotNetCoreKit.Models.Domain;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;

    /// <inheritdoc />
    [Route("api/[controller]")]
    public class AuthenticateController : Controller
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticateController"/> class.
        /// </summary>
        /// <param name="userManager">Used to manage user in a persistence store.</param>
        /// <param name="signInManager">Used to manage user sign ins.</param>
        /// <param name="logger">Used to log exceptions or messages.</param>
        /// <param name="settingsOptions">Used to read the custom web settings file for parsing.</param>
        public AuthenticateController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AuthenticateController> logger,
            IOptions<AppConfigurationSettings> settingsOptions)
        {
            UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            SignInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            SettingsOptions = settingsOptions.Value ?? throw new ArgumentNullException(nameof(settingsOptions));
        }

        /// <summary>
        /// Gets private user manager reference.
        /// </summary>
        private UserManager<ApplicationUser> UserManager { get; }

        /// <summary>
        /// Gets private sign in manager reference.
        /// </summary>
        private SignInManager<ApplicationUser> SignInManager { get; }

        /// <summary>
        /// Gets private logger reference.
        /// </summary>
        private ILogger Logger { get; }

        /// <summary>
        /// Gets the settings options reference.
        /// </summary>
        private AppConfigurationSettings SettingsOptions { get; }

        /// <summary>
        /// Method used to authenticate a user registered in the app.
        /// </summary>
        /// <param name="model">Model used to validate user credentials.</param>
        /// <returns>Token generated by request to authenticate.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Token([FromBody] LoginViewModel model)
        {
            // Clear the existing external data to ensure a clean login process
            await SignInManager.SignOutAsync();

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = await UserManager.FindByEmailAsync(model.Email);

            // add mock user for testing for in memory testing.
            if (user == null)
            {
                var appuser = new ApplicationUser { Email = "test@test.com", UserName = "test@test.com" };
                var res = await UserManager.CreateAsync(appuser, "Hello2!");
                if (res.Succeeded)
                {
                    user = await UserManager.FindByEmailAsync(model.Email);
                }
                else
                {
                    return BadRequest();
                }
            }

            var result = await SignInManager.PasswordSignInAsync(
                    userName: model.Email,
                    password: model.Password,
                    isPersistent: model.RememberMe,
                    lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var token = await GetJwtSecurityToken(user);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                });
            }

            if (result.IsLockedOut)
            {
                Logger.LogWarning("User account locked out.");
                return Unauthorized();
            }

            // If the user information is invalid this is a bad request.
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return BadRequest();
        }

        /// <summary>
        /// Used to log user out and invalidate their token.
        /// </summary>
        /// <returns>Returns OK</returns>
        [HttpPost]
        public IActionResult Logout()
        {
            var signOutAsync = SignInManager.SignOutAsync();

            if (signOutAsync.IsCompletedSuccessfully)
            {
                Logger.LogInformation("User logged out.");
                return new NoContentResult();
            }
            else
            {
                return BadRequest();
            }
        }

        private static IEnumerable<Claim> GetTokenClaims(ApplicationUser user)
        {
            return new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            };
        }

        private async Task<JwtSecurityToken> GetJwtSecurityToken(ApplicationUser user)
        {
            var userClaims = await UserManager.GetClaimsAsync(user);

            return new JwtSecurityToken(
                issuer: SettingsOptions.SiteUrl,
                audience: SettingsOptions.SiteUrl,
                claims: GetTokenClaims(user).Union(userClaims),
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SettingsOptions.Key)),
                    SecurityAlgorithms.HmacSha256));
        }
    }
}