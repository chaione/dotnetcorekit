// -----------------------------------------------------------------------
// <copyright file="CustomizedApiControllerBaseController.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.Webservices.Controllers
{
    using System;

    using DotNetCoreKit.Webservices.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    /// <inheritdoc />
    /// <summary>
    /// Base controller class used to set up generic repeated configuration application context wide.
    /// </summary>
    [Authorize]
    [ProducesResponseType(typeof(BadRequestResult), statusCode: 400)]
    [ProducesResponseType(typeof(UnauthorizedResult), statusCode: 401)]
    [ProducesResponseType(typeof(NotFoundResult), statusCode: 404)]
    [ApiController]
    public class CustomizedApiControllerBaseController : ControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomizedApiControllerBaseController"/> class.
        /// </summary>
        /// <param name="logger">Used to log exceptions or messages.</param>
        /// <param name="settingsOptions">Used to read the custom web settings file for parsing.</param>
        protected CustomizedApiControllerBaseController(
            ILogger<AuthenticateController> logger,
            IOptions<AppConfigurationSettings> settingsOptions)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            SettingsOptions = settingsOptions.Value ?? throw new ArgumentNullException(nameof(settingsOptions));
        }

        /// <summary>
        /// Gets private logger reference.
        /// </summary>
        protected ILogger Logger { get; }

        /// <summary>
        /// Gets the settings options reference.
        /// </summary>
        protected AppConfigurationSettings SettingsOptions { get; }
    }
}