// -----------------------------------------------------------------------
// <copyright file="EmailSender.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.Webservices.Services
{
    using System.Threading.Tasks;

    using DotNetCoreKit.Abstractions.Interfaces;

    /// <inheritdoc />
    public abstract class EmailSender : IEmailSender
    {
        /// <inheritdoc/>
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Task.CompletedTask;
        }
    }
}