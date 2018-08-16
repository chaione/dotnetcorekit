// -----------------------------------------------------------------------
// <copyright file="Person.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.Abstractions.InputDtos
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The person.
    /// </summary>
    public class Person
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        [DefaultValue("")]
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