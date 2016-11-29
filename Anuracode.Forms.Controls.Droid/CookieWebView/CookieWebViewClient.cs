// <copyright file="CookieWebViewRenderer.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Android.Graphics;
using Android.Webkit;

namespace Anuracode.Forms.Controls.Renderers
{
    /// <summary>
    /// Web view client.
    /// </summary>
    internal class CookieWebViewClient : WebViewClient
    {
        /// <summary>
        /// Cached webview.
        /// </summary>
        private readonly CookieWebView cookieWebView;

        /// <summary>
        /// Is first page.
        /// </summary>
        private bool firstPage = true;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="cookieWebView"></param>
        internal CookieWebViewClient(CookieWebView cookieWebView)
        {
            this.cookieWebView = cookieWebView;
        }

        /// <summary>
        /// When finished.
        /// </summary>
        /// <param name="view">View to use.</param>
        /// <param name="url">Url to use.</param>
        public override void OnPageFinished(global::Android.Webkit.WebView view, string url)
        {
        }

        /// <summary>
        /// On paged started.
        /// </summary>
        /// <param name="view">View to use.</param>
        /// <param name="url">Url to use.</param>
        /// <param name="favicon">Icon to use.</param>
        public override void OnPageStarted(global::Android.Webkit.WebView view, string url, Bitmap favicon)
        {
            if (firstPage)
            {
                var cookieManager = CookieManager.Instance;
                cookieManager.SetAcceptCookie(true);
                cookieManager.RemoveAllCookie();
                var cookies = cookieWebView.Cookies.GetCookies(new System.Uri(url));
                for (var i = 0; i < cookies.Count; i++)
                {
                    string cookieValue = cookies[i].Value;
                    string cookieDomain = cookies[i].Domain;
                    string cookieName = cookies[i].Name;
                    cookieManager.SetCookie(cookieDomain, cookieName + "=" + cookieValue);
                }

                firstPage = false;
            }

            base.OnPageStarted(view, url, favicon);
        }
    }
}