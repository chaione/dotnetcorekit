// -----------------------------------------------------------------------
// <copyright file="IndexViewModel.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.Apis.Models.ManageViewModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Gets or sets test
    /// </summary>
    public class IndexViewModel
    {
        /// <summary>
        /// Gets or sets test
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether test
        /// </summary>
        public bool IsEmailConfirmed { get; set; }

        /// <summary>
        /// Gets or sets test
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets test
        /// </summary>
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets test
        /// </summary>
        public string StatusMessage { get; set; }
    }
}