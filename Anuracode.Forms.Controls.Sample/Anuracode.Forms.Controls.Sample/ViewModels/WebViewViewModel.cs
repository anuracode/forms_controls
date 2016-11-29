// <copyright file="AddressListViewModel.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

namespace Anuracode.Forms.Controls.Sample.ViewModels
{
    /// <summary>
    /// View model for the list of address.
    /// </summary>
    public class WebViewViewModel : BaseViewModel
    {
        /// <summary>
        /// Default constuctor.
        /// </summary>
        public WebViewViewModel()
            : base()
        {
            Title = App.LocalizationResources.WebViewLabel;
        }

        /// <summary>
        /// Start url for the authentication.
        /// </summary>
        public string StartUrl
        {
            get
            {
                return "http://www.facebook.com";
            }
        }
    }
}