// <copyright file="BaseView.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Xamarin.Forms;

namespace Anuracode.Forms.Controls.Sample.Views
{
    /// <summary>
    /// Base view.
    /// </summary>
    public class BaseView : ContentPage
    {
        /// <summary>
        /// Command for navigating back.
        /// </summary>
        private Command navigateBackCommand;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public BaseView()
        {
            this.Style = Theme.ApplicationStyles.ViewPageStyle;
        }

        /// <summary>
        /// Command for navigating back.
        /// </summary>
        public Command NavigateBackCommand
        {
            get
            {
                if (navigateBackCommand == null)
                {
                    navigateBackCommand = new Command(NavigateBack);
                }

                return navigateBackCommand;
            }
        }

        /// <summary>
        /// Navigate back.
        /// </summary>
        protected virtual void NavigateBack()
        {
            Navigation.PopAsync();
        }
    }
}