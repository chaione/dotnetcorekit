﻿// -----------------------------------------------------------------------
// <copyright file="ForgotPasswordViewModel.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.Apis.Models.AccountViewModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// test
    /// </summary>
    public class ForgotPasswordViewModel
    {
        /// <summary>
        /// Gets or sets email address
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}