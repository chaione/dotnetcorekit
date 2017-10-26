// -----------------------------------------------------------------------
// <copyright file="PeopleContext.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.Models
{
    using DomainModels;
    using Microsoft.EntityFrameworkCore;

    /// <inheritdoc />
    /// <summary>
    /// The people context.
    /// </summary>
    public class PeopleContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PeopleContext"/> class.
        /// </summary>
        /// <param name="options">
        /// The options.
        /// </param>
        public PeopleContext(DbContextOptions<PeopleContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the people.
        /// </summary>
        public DbSet<People> People { get; set; }
    }
}
