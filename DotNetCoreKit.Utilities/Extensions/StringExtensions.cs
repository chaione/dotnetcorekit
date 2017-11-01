// -----------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.Utilities.Extensions
{
    /// <summary>
    /// Contains a list of extensions that can be used for common purposes.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Determines whether a specified string is not null, empty, or consists only of white-space characters.
        /// </summary>
        /// <param name="source">The source string that we should check validity of.</param>
        /// <returns>Returns true if the string is valid and false if is invalid</returns>
        public static bool HasValue(this string source) => !string.IsNullOrWhiteSpace(source) && source?.Trim().Length != 0;
    }
}