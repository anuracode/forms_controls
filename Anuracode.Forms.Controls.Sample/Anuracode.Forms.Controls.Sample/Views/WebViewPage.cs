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
            WebView webView = new WebView()
            {
                Source = ViewModel == null ? null : ViewModel.StartUrl
            };

            webView.Navigating += WebView_Navigating;

            return webView;
        }

        /// <summary>
        /// Event when navigating.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WebView_Navigating(object sender, WebNavigatingEventArgs e)
        {
            var taskAlert = this.DisplayAlert("URL", e.Url, "Cancel");
        }
    }
}