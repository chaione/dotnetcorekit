// -----------------------------------------------------------------------
// <copyright file="ErrorViewModel.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.Models
{
    /// <summary>
    /// test
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Gets or sets request id
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// Gets a value indicating whether returns boolean value if RequestId is not null.
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}