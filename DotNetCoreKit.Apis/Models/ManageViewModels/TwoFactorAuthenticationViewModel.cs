// -----------------------------------------------------------------------
// <copyright file="TwoFactorAuthenticationViewModel.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.Apis.Models.ManageViewModels
{
    /// <summary>
    /// Gets or sets test
    /// </summary>
    public class TwoFactorAuthenticationViewModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether test
        /// </summary>
        public bool HasAuthenticator { get; set; }

        /// <summary>
        /// Gets or sets test
        /// </summary>
        public int RecoveryCodesLeft { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether test
        /// </summary>
        public bool Is2FaEnabled { get; set; }
    }
}