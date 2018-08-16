// <copyright file="People.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>

namespace DotNetCoreKit.Abstractions.Domain
{
    /// <inheritdoc />
    /// <summary>
    /// Defines the People instance of a user.
    /// </summary>
    public class People : EntityBase
    {
        /// <summary>
        /// Gets or sets name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets age.
        /// </summary>
        public int Age { get; set; }
    }
}