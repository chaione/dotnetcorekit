// -----------------------------------------------------------------------
// <copyright file="ManageNavPages.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.Apis.Views.Manage
{
    using System;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;

    /// <summary>
    /// test
    /// </summary>
    public static class ManageNavPages
    {
        /// <summary>
        /// Gets active page string.
        /// </summary>
        private static string ActivePageKey => "ActivePage";

        /// <summary>
        /// Gets index string.
        /// </summary>
        private static string Index => "Index";

        /// <summary>
        /// Gets change password string.
        /// </summary>
        private static string ChangePassword => "ChangePassword";

        /// <summary>
        /// Gets external login string.
        /// </summary>
        private static string ExternalLogins => "ExternalLogins";

        /// <summary>
        /// Gets two factor authentication string.
        /// </summary>
        private static string TwoFactorAuthentication => "TwoFactorAuthentication";

        /// <summary>
        /// Gets index page
        /// </summary>
        /// <param name="viewContext">Current view context</param>
        /// <returns>If the current page is index or not.</returns>
        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        /// <summary>
        /// Gets change password page
        /// </summary>
        /// <param name="viewContext">Current view context</param>
        /// <returns>If the current page is change password or not.</returns>
        public static string ChangePasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangePassword);

        /// <summary>
        /// Gets the external login page.
        /// </summary>
        /// <param name="viewContext">Current view context</param>
        /// <returns>If the current page is external login page or not.</returns>
        public static string ExternalLoginsNavClass(ViewContext viewContext) => PageNavClass(viewContext, ExternalLogins);

        /// <summary>
        /// Gets two factor authentication page.
        /// </summary>
        /// <param name="viewContext">Current view context</param>
        /// <returns>If the current page is two factor authentication or not.</returns>
        public static string TwoFactorAuthenticationNavClass(ViewContext viewContext) => PageNavClass(viewContext, TwoFactorAuthentication);

        /// <summary>
        /// Internal dictionary used to keep track of active pages.
        /// </summary>
        /// <param name="viewData">View data dictionary.</param>
        /// <param name="activePage">Current active page.</param>
        public static void AddActivePage(this ViewDataDictionary viewData, string activePage) => viewData[ActivePageKey] = activePage;

        /// <summary>
        /// Provides internal page navigation class.
        /// </summary>
        /// <param name="viewContext">Current page context.</param>
        /// <param name="page">Page to navigate to.</param>
        /// <returns>Returns if the active page is the same as page navigating to.</returns>
        private static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string;
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}