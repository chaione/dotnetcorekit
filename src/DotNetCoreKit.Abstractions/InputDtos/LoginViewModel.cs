// -----------------------------------------------------------------------
// <copyright file="LoginViewModel.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.Abstractions.InputDtos
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// View model used to log a user in.
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// Gets or sets user name.
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets password.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}