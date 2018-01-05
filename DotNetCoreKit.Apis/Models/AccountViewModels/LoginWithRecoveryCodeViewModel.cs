// -----------------------------------------------------------------------
// <copyright file="LoginWithRecoveryCodeViewModel.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.Apis.Models.AccountViewModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// test
    /// </summary>
    public class LoginWithRecoveryCodeViewModel
    {
        /// <summary>
        /// Gets or sets test
        /// </summary>
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Recovery Code")]
        public string RecoveryCode { get; set; }
    }
}