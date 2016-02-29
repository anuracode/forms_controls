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
    public class BaseView<TViewModel> : BaseView
        where TViewModel : class
    {
        /// <summary>
        /// View model from the AppNavigation.
        /// </summary>
        protected TViewModel ViewModel { get; private set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="viewModel">View model to use.</param>
        public BaseView(TViewModel viewModel)
            : base()
        {
            ViewModel = viewModel;

            if (ViewModel != null)
            {
                this.BindingContext = ViewModel;
            }

            this.Title = string.Empty;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public BaseView()
            : this(null)
        {
        }
    }
}