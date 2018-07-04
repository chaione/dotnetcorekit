// -----------------------------------------------------------------------
// <copyright file="DefaultModule.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.Webservices
{
    using Autofac;
    using DotNetCoreKit.Utilities.Extensions;
    using DotNetCoreKit.Utilities.Interfaces;
    using DotNetCoreKit.Webservices.Services;

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