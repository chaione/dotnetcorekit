// -----------------------------------------------------------------------
// <copyright file="LoginWith2FaViewModel.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.Apis.Models.AccountViewModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// test
    /// </summary>
    public class LoginWith2FaViewModel
    {
        /// <summary>
        /// Gets or sets test
        /// </summary>
        [Required]
        [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Authenticator code")]
        public string TwoFactorCode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether test
        /// </summary>
        [Display(Name = "Remember this machine")]
        public bool RememberMachine { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether test
        /// </summary>
        public bool RememberMe { get; set; }
    }
}