// -----------------------------------------------------------------------
// <copyright file="AppConfigurationSettings.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.Webservices.Models
{
    /// <summary>
    /// Custom Web Settings configuration file that contains settings that we want to set per env where app is deployed.
    /// </summary>
    public class AppConfigurationSettings
    {
        /// <summary>
        /// Gets or sets Title value
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets Updates value
        /// </summary>
        public int Updates { get; set; }

        /// <summary>
        /// Gets or sets the site URL used to indicate issuer of the token.
        /// </summary>
        public string SiteUrl { get; set; }

        /// <summary>
        /// Gets or sets the Application Key used for generation of JWT tokens
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the application's token life time
        /// </summary>
        public int ExpirationInMinutes { get; set; }
    }
}