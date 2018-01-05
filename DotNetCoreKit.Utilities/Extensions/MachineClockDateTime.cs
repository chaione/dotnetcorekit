// -----------------------------------------------------------------------
// <copyright file="MachineClockDateTime.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.Utilities.Extensions
{
    using System;

    using Interfaces;

    /// <inheritdoc />
    public class MachineClockDateTime : IDateTime
    {
        /// <inheritdoc />
        public DateTime Now => DateTime.Now;
    }
}