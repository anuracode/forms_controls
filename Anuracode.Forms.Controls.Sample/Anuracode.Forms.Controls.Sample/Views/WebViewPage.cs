// <copyright file="WebViewPage.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Sample.ViewModels;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls.Sample.Views
{
    /// <summary>
    /// Page for the view.
    /// </summary>
    public class WebViewPage : ContentBaseView<WebViewViewModel>
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="viewModel">View model to use.</param>
        public WebViewPage(WebViewViewModel viewModel)
            : base(viewModel)
        {
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public WebViewPage()
            : this(null)
        {
        }

        /// <summary>
        /// Content of the page.
        /// </summary>
        protected override View RenderContent()
        {
            CookieWebView webView = new CookieWebView()
            {
                Source = ViewModel == null ? null : ViewModel.StartUrl
            };

            webView.AddLoginWebsites();

            return webView;
        }
    }
}