// -----------------------------------------------------------------------
// <copyright file="HomeController.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.Apis.Controllers
{
    using System.Diagnostics;

    using DotNetCoreKit.Models;
    using Microsoft.AspNetCore.Mvc;

    /// <inheritdoc />
    /// <summary>
    /// The Home controller used only for the api server when hosted to get documentation and generic info about api.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Provides the main view page on server.
        /// </summary>
        /// <returns>View</returns>
        public IActionResult Index() => View();

        /// <summary>
        /// Provides the action view for about page on server.
        /// </summary>
        /// <returns>View</returns>
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        /// <summary>
        /// Provides the action view for contact page on server.
        /// </summary>
        /// <returns>View</returns>
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        /// <summary>
        /// Provides the action view for errors on server.
        /// </summary>
        /// <returns>View</returns>
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}