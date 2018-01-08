// -----------------------------------------------------------------------
// <copyright file="CustomWebSettings.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.Apis.Models
{
    /// <summary>
    /// Custom Web Settings configuration file that contains settings that we want to set per env where app is deployed.
    /// </summary>
    public class CustomWebSettings
    {
        /// <summary>
        /// Gets or sets Title value
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets Updates value
        /// </summary>
        public int Updates { get; set; }
    }
}