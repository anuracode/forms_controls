// <copyright file="CookieWebView.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System.Collections.Generic;
using System.Net;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls
{
    /// <summary>
    /// Web view with cookie support.
    /// </summary>
    public class CookieWebView : WebView
    {
        /// <summary>
        /// Cookies bindable property.
        /// </summary>
        public static readonly BindableProperty CookiesProperty = BindableProperty.Create(
            propertyName: nameof(Cookies),
            returnType: typeof(CookieContainer),
            declaringType: typeof(CookieWebView),
            defaultValue: default(string));

        /// <summary>
        /// List of website that should be clean before navigating.
        /// </summary>
        private List<string> cleanWebsites;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public CookieWebView()
        {
            Cookies = new CookieContainer();
        }

        /// <summary>
        /// List of website that should be clean before navigating.
        /// </summary>
        public List<string> CleanWebsites
        {
            get
            {
                if (cleanWebsites == null)
                {
                    cleanWebsites = new List<string>();
                }

                return cleanWebsites;
            }
        }

        /// <summary>
        /// Cookies container.
        /// </summary>
        public CookieContainer Cookies
        {
            get
            {
                return (CookieContainer)GetValue(CookiesProperty);
            }

            set
            {
                SetValue(CookiesProperty, value);
            }
        }

        /// <summary>
        /// Add facebook, google and windows live urls to the clean cookie list.
        /// </summary>
        public void AddLoginWebsites()
        {
            CleanWebsites.Add("https://www.facebook.com");
            CleanWebsites.Add("https://accounts.google.com");
            CleanWebsites.Add("https://login.live.com");
        }
    }
}