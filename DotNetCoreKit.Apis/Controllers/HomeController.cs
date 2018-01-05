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
    /// The Home controller.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// test
        /// </summary>
        /// <returns>View</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// test
        /// </summary>
        /// <returns>View</returns>
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        /// <summary>
        /// test
        /// </summary>
        /// <returns>View</returns>
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        /// <summary>
        /// test
        /// </summary>
        /// <returns>View</returns>
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}