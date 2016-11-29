// <copyright file="CookieWebViewRenderer.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System;
using Windows.Web.Http;
using Xamarin.Forms;

#if WINDOWS_UWP

using Xamarin.Forms.Platform.UWP;

#else

using Xamarin.Forms.Platform.WinRT;

#endif

[assembly: ExportRenderer(typeof(Anuracode.Forms.Controls.CookieWebView), typeof(Anuracode.Forms.Controls.Renderers.CookieWebViewRenderer))]

namespace Anuracode.Forms.Controls.Renderers
{
    /// <summary>
    /// Web view renderer.
    /// </summary>
    public class CookieWebViewRenderer : WebViewRenderer
    {
        /// <summary>
        /// Event when element property changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if ((this.Element == null) || (this.Control == null))
            {
                return;
            }
        }

        /// <summary>
        /// On element changed.
        /// </summary>
        /// <param name="e">Arguments to use.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                Control.NavigationStarting -= Control_NavigationStarting;
                Control.NavigationCompleted -= Control_NavigationCompleted;
                Control.NavigationStarting += Control_NavigationStarting;
                Control.NavigationCompleted += Control_NavigationCompleted;
            }
        }

        /// <summary>
        /// Event when completed.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="args">Arguments of the event.</param>
        private void Control_NavigationCompleted(Windows.UI.Xaml.Controls.WebView sender, Windows.UI.Xaml.Controls.WebViewNavigationCompletedEventArgs args)
        {
        }

        /// <summary>
        /// Flag if is first page.
        /// </summary>
        private bool firstPage = true;

        /// <summary>
        /// Navigate starting.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="args">Arguments of the event.</param>
        private void Control_NavigationStarting(Windows.UI.Xaml.Controls.WebView sender, Windows.UI.Xaml.Controls.WebViewNavigationStartingEventArgs args)
        {
            // Clear cookies.
            if ((args != null) && (args.Uri != null))
            {
                if (firstPage)
                {
                    Windows.Web.Http.Filters.HttpBaseProtocolFilter myFilter = new Windows.Web.Http.Filters.HttpBaseProtocolFilter();
                    var cookieManager = myFilter.CookieManager;
                    HttpCookieCollection myCookieJar = cookieManager.GetCookies(args.Uri);
                    foreach (HttpCookie cookie in myCookieJar)
                    {
                        cookieManager.DeleteCookie(cookie);
                    }

                    if (Element != null)
                    {
                        CookieWebView cookieView = Element as CookieWebView;

                        if ((cookieView != null) && (cookieView.CleanWebsites.Count > 0))
                        {
                            foreach (var webSitePath in cookieView.CleanWebsites)
                            {
                                if (!string.IsNullOrEmpty(webSitePath))
                                {
                                    HttpCookieCollection innerCookieJar = cookieManager.GetCookies(new Uri(webSitePath));
                                    foreach (HttpCookie cookie in innerCookieJar)
                                    {
                                        cookieManager.DeleteCookie(cookie);
                                    }
                                }
                            }
                        }
                    }

                    firstPage = false;
                }
            }
        }
    }
}