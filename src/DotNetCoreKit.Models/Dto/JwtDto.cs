// -----------------------------------------------------------------------
// <copyright file="JwtDto.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.Models.Dto
{
    using System;

    /// <summary>
    /// Model describing the Jwt Token response returned on authentication
    /// </summary>
    public class JwtDto
    {
        /// <summary>
        /// Gets or sets token value
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the life time of the token.
        /// </summary>
        public DateTime Expiration { get; set; }
    }
}