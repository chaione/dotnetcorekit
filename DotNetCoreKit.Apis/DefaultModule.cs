// -----------------------------------------------------------------------
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
            // Register as a single server instance value to be referenced in any methods
            builder.RegisterType<MachineClockDateTime>().As<IDateTime>().InstancePerLifetimeScope();

            // Registering a new lifetime/transient instances of application services.
            builder.RegisterType<EmailSender>().As<IEmailSender>().InstancePerLifetimeScope();
        }
    }
}