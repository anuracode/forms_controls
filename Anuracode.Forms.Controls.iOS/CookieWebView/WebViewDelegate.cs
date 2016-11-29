// <copyright file="CookieWebViewRenderer.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Foundation;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UIKit;

namespace Anuracode.Forms.Controls.Renderers
{
    /// <summary>
    /// Web view client.
    /// </summary>
    internal class WebViewDelegate : UIWebViewDelegate
    {
        /// <summary>
        /// Web view.
        /// </summary>
        private CookieWebView cookieWebView;

        /// <summary>
        /// Is first page.
        /// </summary>
        private bool firstPage = true;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="cookieWebView">View to use.</param>
        public WebViewDelegate(CookieWebView cookieWebView)
        {
            this.cookieWebView = cookieWebView;
        }

        /// <summary>
        /// On faill.
        /// </summary>
        /// <param name="webView">View to use.</param>
        /// <param name="error">Error to use.</param>
        public override void LoadFailed(UIWebView webView, NSError error)
        {
            AC.TraceError(error.ToString(), new System.Exception(error.ToString()));
        }

        /// <summary>
        /// Loading finished.
        /// </summary>
        /// <param name="webView">Web view to use.</param>
        public override void LoadingFinished(UIWebView webView)
        {
        }

        /// <summary>
        /// Load started.
        /// </summary>
        /// <param name="webView">View to use.</param>
        public override void LoadStarted(UIWebView webView)
        {
        }

        /// <summary>
        /// Should start loading.
        /// </summary>
        /// <param name="webView">View to use.</param>
        /// <param name="request">Request to use.</param>
        /// <param name="navigationType">Navigation type.</param>
        /// <returns></returns>
        public override bool ShouldStartLoad(UIWebView webView, NSUrlRequest request, UIWebViewNavigationType navigationType)
        {
            if (firstPage)
            {
                // Set cookies here
                var cookieJar = NSHttpCookieStorage.SharedStorage;
                cookieJar.AcceptPolicy = NSHttpCookieAcceptPolicy.Always;

                // Clean up old cookies
                foreach (var aCookie in cookieJar.Cookies)
                {
                    cookieJar.DeleteCookie(aCookie);
                }

                // Set up the new cookies
                var jCookies = cookieWebView.Cookies.GetCookies(request.Url);
                IList<NSHttpCookie> eCookies =
                    (from object jCookie in jCookies
                     where jCookie != null
                     select (Cookie)jCookie
                     into netCookie
                     select new NSHttpCookie(netCookie)).ToList();

                cookieJar.SetCookies(eCookies.ToArray(), request.Url, request.Url);

                firstPage = false;
            }

            return true;
        }
    }
}