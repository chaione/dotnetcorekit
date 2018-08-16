// -----------------------------------------------------------------------
// <copyright file="PeopleDto.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.Abstractions.Dto
{
    using System.Collections.Generic;

    /// <summary>
    /// The people dto.
    /// </summary>
    public class PeopleDto
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the age.
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Gets or sets the pets.
        /// </summary>
        public List<int> Pets { get; set; }
    }
}