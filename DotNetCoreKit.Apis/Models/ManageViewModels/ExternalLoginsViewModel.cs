// -----------------------------------------------------------------------
// <copyright file="ExternalLoginsViewModel.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.Apis.Models.ManageViewModels
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Identity;

    /// <summary>
    /// Gets or sets test
    /// </summary>
    public class ExternalLoginsViewModel
    {
        /// <summary>
        /// Gets or sets test
        /// </summary>
        public IList<UserLoginInfo> CurrentLogins { get; set; }

        /// <summary>
        /// Gets or sets test
        /// </summary>
        public IList<AuthenticationScheme> OtherLogins { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether test
        /// </summary>
        public bool ShowRemoveButton { get; set; }

        /// <summary>
        /// Gets or sets test
        /// </summary>
        public string StatusMessage { get; set; }
    }
}