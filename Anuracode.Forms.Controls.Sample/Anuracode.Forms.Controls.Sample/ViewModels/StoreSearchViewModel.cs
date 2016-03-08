// <copyright file="StoreSearchViewModel.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

namespace Anuracode.Forms.Controls.Sample.ViewModels
{
    /// <summary>
    /// ViewModel for the listing of all the store elements.
    /// </summary>
    public class StoreSearchViewModel : StoreListViewModel
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public StoreSearchViewModel()
            : base()
        {
            IsStartViewSearchMode = true;
            IsProductListMode = true;
            IsFullSearchMode = true;
        }
    }
}