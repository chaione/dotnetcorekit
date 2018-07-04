// -----------------------------------------------------------------------
// <copyright file="EmailSenderExtensions.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.Utilities.Extensions
{
    using System;
    using System.Threading.Tasks;

    using DotNetCoreKit.Utilities.Interfaces;

    /// <summary>
    /// Used to proved helper methods for emailing services.
    /// </summary>
    public static class EmailSenderExtensions
    {
        /// <summary>
        /// Asynchronous method to send confirmation messages.
        /// </summary>
        /// <param name="emailSender">Email Sender interface</param>
        /// <param name="email">Email message</param>
        /// <param name="encodedLink">HTML Encoded email link to provide confirmation from.</param>
        /// <returns>Task</returns>
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string encodedLink)
        {
            var link = $"Please confirm your account by clicking this link: <a href='{encodedLink}'>link</a>";

            return encodedLink.Contains(" ")
                ? Task.FromException<InvalidOperationException>(
                    new InvalidOperationException("Provided link was not HtmlEncoder.Default.Encode format text."))
                : emailSender.SendEmailAsync(email, "Confirm your email", link);
        }
    }
}