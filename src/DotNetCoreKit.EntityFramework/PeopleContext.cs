// -----------------------------------------------------------------------
// <copyright file="PeopleContext.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.EntityFramework
{
    using DotNetCoreKit.Abstractions.Domain;
    using Microsoft.EntityFrameworkCore;

    /// <inheritdoc />
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

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<People>().HasOne(x => x.CreatedBy);
            modelBuilder.Entity<People>().HasOne(x => x.UpdatedBy);
        }
    }
}