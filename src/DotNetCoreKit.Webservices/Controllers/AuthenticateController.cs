﻿// -----------------------------------------------------------------------
// <copyright file="AuthenticateController.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.Webservices.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;

    using DotNetCoreKit.Abstractions.Domain;
    using DotNetCoreKit.Abstractions.Dto;
    using DotNetCoreKit.Abstractions.InputDtos;
    using DotNetCoreKit.Webservices.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;

    /// <inheritdoc />
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : CustomizedApiControllerBaseController
    {
        /// <inheritdoc cref="CustomizedApiControllerBaseController"/>
        /// <param name="userManager">Used to manage user in a persistence store.</param>
        /// <param name="signInManager">Used to manage user sign ins.</param>
        /// <param name="logger">Used to log exceptions or messages.</param>
        /// <param name="settingsOptions">Used to read the custom web settings file for parsing.</param>
        public AuthenticateController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AuthenticateController> logger,
            IOptions<AppConfigurationSettings> settingsOptions)
            : base(logger, settingsOptions)
        {
            UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            SignInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

        /// <summary>
        /// Gets private user manager reference.
        /// </summary>
        private UserManager<ApplicationUser> UserManager { get; }

        /// <summary>
        /// Gets private sign in manager reference.
        /// </summary>
        private SignInManager<ApplicationUser> SignInManager { get; }

#pragma warning disable SA1629 // Documentation text should end with a period
        /// <summary>
        /// Method used to authenticate a user registered in the app.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /login
        ///     {
        ///        "email": "test@test.com",
        ///        "password": "Hello2!",
        ///        "rememberMe": true
        ///     }
        ///
        /// </remarks>
        /// <param name="model">Model used to validate user credentials.</param>
        /// <returns>Token generated by request to authenticate.</returns>
        [HttpPost("authenticate")]
#pragma warning restore SA1629 // Documentation text should end with a period
        [AllowAnonymous]
        [ProducesResponseType(typeof(JwtDto), statusCode: 200)]
        public async Task<IActionResult> Authenticate([FromBody] LoginViewModel model)
        {
            // Clear the existing external data to ensure a clean login process
            await SignInManager.SignOutAsync();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await UserManager.FindByEmailAsync(model.UserName);

            // add mock user for testing for in memory testing.
            if (user == null)
            {
                var appuser = new ApplicationUser { Email = "test@test.com", UserName = "test@test.com" };
                var res = await UserManager.CreateAsync(appuser, "Hello2!");
                if (res.Succeeded)
                {
                    user = await UserManager.FindByEmailAsync(model.UserName);
                }
                else
                {
                    return BadRequest(res);
                }
            }

            var result = await SignInManager.PasswordSignInAsync(
                    userName: model.UserName,
                    password: model.Password,
                    isPersistent: false,
                    lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var token = await GetJwtSecurityToken(user);

                return Ok(new JwtDto
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo,
                });
            }

            if (result.IsLockedOut)
            {
                Logger.LogWarning("User account locked out.");
                return Unauthorized();
            }

            // If the user information is invalid this is a bad request.
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Used to log user out and invalidate their token.
        /// </summary>
        /// <returns>Returns okay response with no content if successful.</returns>
        [HttpPost("logout")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(NoContentResult), statusCode: 204)]
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
                expires: DateTime.UtcNow.AddMinutes(SettingsOptions.ExpirationInMinutes),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SettingsOptions.Key)),
                    SecurityAlgorithms.HmacSha256));
        }
    }
}