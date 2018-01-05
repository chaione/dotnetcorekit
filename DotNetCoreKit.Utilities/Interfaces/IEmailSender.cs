// -----------------------------------------------------------------------
// <copyright file="IEmailSender.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.Utilities.Interfaces
{
    using System.Threading.Tasks;

    /// <summary>
    /// EmailSender interface.
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Send email asynchronously method
        /// </summary>
        /// <param name="email">Email address to send message to.</param>
        /// <param name="subject">Message subject.</param>
        /// <param name="message">Message body.</param>
        /// <returns>Returns a task that can be consumed where necessary.</returns>
        Task SendEmailAsync(string email, string subject, string message);
    }
}