// -----------------------------------------------------------------------
// <copyright file="MachineClockDateTime.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.Abstractions.Extensions
{
    using System;

    using DotNetCoreKit.Abstractions.Interfaces;

    /// <inheritdoc />
    public class MachineClockDateTime : IDateTime
    {
        /// <inheritdoc />
        public DateTime Now => DateTime.Now;
    }
}