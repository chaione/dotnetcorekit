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
        /// Gets test
        /// </summary>
        public static string ActivePageKey => "ActivePage";

        /// <summary>
        /// Gets test
        /// </summary>
        public static string Index => "Index";

        /// <summary>
        /// Gets test
        /// </summary>
        public static string ChangePassword => "ChangePassword";

        /// <summary>
        /// Gets test
        /// </summary>
        public static string ExternalLogins => "ExternalLogins";

        /// <summary>
        /// Gets test
        /// </summary>
        public static string TwoFactorAuthentication => "TwoFactorAuthentication";

        /// <summary>
        /// Gets test
        /// </summary>
        /// <param name="viewContext">y</param>
        /// <returns>tets</returns>
        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        /// <summary>
        /// Gets test
        /// </summary>
        /// <param name="viewContext">y</param>
        /// <returns>tets</returns>
        public static string ChangePasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangePassword);

        /// <summary>
        /// Gets test
        /// </summary>
        /// <param name="viewContext">y</param>
        /// <returns>tets</returns>
        public static string ExternalLoginsNavClass(ViewContext viewContext) => PageNavClass(viewContext, ExternalLogins);

        /// <summary>
        /// Gets test
        /// </summary>
        /// <param name="viewContext">y</param>
        /// <returns>tets</returns>
        public static string TwoFactorAuthenticationNavClass(ViewContext viewContext) => PageNavClass(viewContext, TwoFactorAuthentication);

        /// <summary>
        /// test
        /// </summary>
        /// <param name="viewData">tt</param>
        /// <param name="activePage">te</param>
        public static void AddActivePage(this ViewDataDictionary viewData, string activePage) => viewData[ActivePageKey] = activePage;

        /// <summary>
        /// test
        /// </summary>
        /// <param name="viewContext">y</param>
        /// <param name="page">t</param>
        /// <returns>tets</returns>
        private static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string;
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}