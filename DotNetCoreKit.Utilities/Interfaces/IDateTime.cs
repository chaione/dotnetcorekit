// -----------------------------------------------------------------------
// <copyright file="IDateTime.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.Utilities.Interfaces
{
    using System;

    /// <summary>
    /// Global referable interface for date time.
    /// </summary>
    public interface IDateTime
    {
        /// <summary>
        /// Gets replacement for regular DateTime.Now.
        /// </summary>
        DateTime Now { get; }
    }
}