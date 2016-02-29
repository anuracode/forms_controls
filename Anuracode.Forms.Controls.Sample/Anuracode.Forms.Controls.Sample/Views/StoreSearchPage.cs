// <copyright file="IntroPage.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Sample.ViewModels;

namespace Anuracode.Forms.Controls.Sample.Views
{
    /// <summary>
    /// Store search view.
    /// </summary>
    public class StoreSearchPage : BaseView<StoreSearchViewModel>
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="viewModel">View model to use.</param>
        public StoreSearchPage(StoreSearchViewModel viewModel)
            : base(viewModel)
        {
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StoreSearchPage()
            : this(null)
        {
        }
    }
}