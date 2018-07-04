// -----------------------------------------------------------------------
// <copyright file="ExternalLoginViewModel.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.Apis.Models.AccountViewModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// test
    /// </summary>
    public class ExternalLoginViewModel
    {
        /// <summary>
        /// Gets or sets Email address
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}