// -----------------------------------------------------------------------
// <copyright file="EmailSender.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.Apis.Services
{
    using System.Threading.Tasks;

    using DotNetCoreKit.Utilities.Interfaces;

    /// <inheritdoc />
    public class EmailSender : IEmailSender
    {
        /// <inheritdoc/>
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Task.CompletedTask;
        }
    }
}