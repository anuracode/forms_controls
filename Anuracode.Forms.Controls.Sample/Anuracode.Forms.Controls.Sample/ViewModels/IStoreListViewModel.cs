// <copyright file="IStoreListViewModel.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Sample.Model;

namespace Anuracode.Forms.Controls.Sample.ViewModels
{
    /// <summary>
    /// Interface for the store list viewmodel.
    /// </summary>
    public interface IStoreListViewModel
    {
        /// <summary>
        /// Level for the elements.
        /// </summary>
        StoreItemLevel Level { get; }

        /// <summary>
        /// Sublevel viewmodel.
        /// </summary>
        ObservableCollectionFast<StoreItemLevelViewModel> SublevelsViewModels { get; }
    }
}