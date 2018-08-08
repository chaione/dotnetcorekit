// -----------------------------------------------------------------------
// <copyright file="EntityBase.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.Models.Domain
{
    using System;

    /// <summary>
    /// The base entity being used to create all domain tables that require tracking of last modified and by whom.
    /// </summary>
    public class EntityBase
    {
        /// <summary>
        /// Gets or sets database Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Date time stamp for when the record was updated
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets Date time stamp for when the record was updated
        /// </summary>
        public DateTime UpdatedOn { get; set; }

        /// <summary>
        /// Gets or sets Created by Application User Id
        /// </summary>
        public int CreatedById { get; set; }

        /// <summary>
        /// Gets or sets Created by Application User
        /// </summary>
        public virtual ApplicationUser CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets Updated by Application User Id
        /// </summary>
        public int UpdatedById { get; set; }

        /// <summary>
        /// Gets or sets Updated by Application User
        /// </summary>
        public virtual ApplicationUser UpdatedBy { get; set; }
    }
}