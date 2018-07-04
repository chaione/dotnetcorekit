// -----------------------------------------------------------------------
// <copyright file="ApplicationUser.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.Models.Domain
{
    using Microsoft.AspNetCore.Identity;

    /// <inheritdoc />
    public class ApplicationUser : IdentityUser<int>
    {
    }
}