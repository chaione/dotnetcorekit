// -----------------------------------------------------------------------
// <copyright file="EnableAuthenticatorViewModel.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.Apis.Models.ManageViewModels
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// test
    /// </summary>
    public class EnableAuthenticatorViewModel
    {
        /// <summary>
        /// Gets or sets test
        /// </summary>
        [Required]
        [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Verification Code")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets test
        /// </summary>
        [ReadOnly(true)]
        public string SharedKey { get; set; }

        /// <summary>
        /// Gets or sets test
        /// </summary>
        public string AuthenticatorUri { get; set; }
    }
}