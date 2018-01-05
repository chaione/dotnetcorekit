// -----------------------------------------------------------------------
// <copyright file="UrlHelperExtensions.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.Apis.Extensions
{
    using DotNetCoreKit.Apis.Controllers;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Helper extensions that could not be extracted to common utility library.
    /// </summary>
    public static class UrlHelperExtensions
    {
        /// <summary>
        /// Email conformation link helper method
        /// </summary>
        /// <param name="urlHelper">URL helper interface</param>
        /// <param name="userId">User's database id</param>
        /// <param name="code">HTTP Status code</param>
        /// <param name="scheme">Protocol used for sending email.</param>
        /// <returns>Returns URL helper string.</returns>
        public static string EmailConfirmationLink(this IUrlHelper urlHelper, int userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(AccountController.ConfirmEmail),
                controller: "Account",
                values: new { userId, code },
                protocol: scheme);
        }

        /// <summary>
        /// Email conformation link helper method
        /// </summary>
        /// <param name="urlHelper">URL helper interface</param>
        /// <param name="userId">User's database id</param>
        /// <param name="code">HTTP Status code</param>
        /// <param name="scheme">Protocol used for sending email.</param>
        /// <returns>Returns URL helper string.</returns>
        public static string ResetPasswordCallbackLink(this IUrlHelper urlHelper, int userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(AccountController.ResetPassword),
                controller: "Account",
                values: new { userId, code },
                protocol: scheme);
        }
    }
}