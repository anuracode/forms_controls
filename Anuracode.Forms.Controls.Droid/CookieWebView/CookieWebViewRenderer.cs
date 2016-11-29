// <copyright file="CookieWebViewRenderer.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Xamarin.Forms;

[assembly: ExportRenderer(typeof(Anuracode.Forms.Controls.CookieWebView), typeof(Anuracode.Forms.Controls.Renderers.CookieWebViewRenderer))]

namespace Anuracode.Forms.Controls.Renderers
{
    /// <summary>
    /// Web view renderer.
    /// </summary>
    public class CookieWebViewRenderer : Xamarin.Forms.Platform.Android.WebViewRenderer
    {
        /// <summary>
        /// Web view.
        /// </summary>
        public CookieWebView CookieWebView
        {
            get { return Element as CookieWebView; }
        }

        /// <summary>
        /// On element changed.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                Control.SetWebViewClient(new CookieWebViewClient(CookieWebView));
            }
        }
    }
}