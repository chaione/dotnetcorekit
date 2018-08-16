// -----------------------------------------------------------------------
// <copyright file="IntegerConstants.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.Abstractions.Constants
{
    /// <summary>
    /// Used to provide application wide integer constant values.
    /// </summary>
    public static class IntegerConstants
    {
        /// <summary>
        /// Minimum length for a password to be referenced anywhere in the app.
        /// </summary>
        public const int MinimumPasswordLength = 10;

        /// <summary>
        /// Maximum length to be supported in app for a property, for column in db and validation for inputs.
        /// </summary>
        public const int MaxLength = 256;
    }
}