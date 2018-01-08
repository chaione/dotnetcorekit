﻿// -----------------------------------------------------------------------
// <copyright file="DefaultModule.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.Apis
{
    using Autofac;
    using DotNetCoreKit.Apis.Services;
    using Utilities.Extensions;
    using Utilities.Interfaces;

    /// <inheritdoc />
    public class DefaultModule : Module
    {
        /// <inheritdoc/>
        protected override void Load(ContainerBuilder builder)
        {
            // Registering a new lifetime/transient instances of application services.
            builder.RegisterType<MachineClockDateTime>().As<IDateTime>().InstancePerLifetimeScope();
            builder.RegisterType<EmailSender>().As<IEmailSender>().InstancePerLifetimeScope();
        }
    }
}