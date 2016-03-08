// <copyright file="IStoreListPage.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Sample.Model;
using Anuracode.Forms.Controls.Sample.ViewModels;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls.Sample.Interfaces
{
    /// <summary>
    /// Interface for the pages of the store.
    /// </summary>
    public interface IStoreListPage
    {
        /// <summary>
        /// Command for showing the options.
        /// </summary>
        Command HideItemOptionsCommand { get; }

        /// <summary>
        /// Navigate to store level.
        /// </summary>
        Command<StoreItemLevel> NavigateToStoreLevelCommand { get; }

        /// <summary>
        /// Navigate to store level.
        /// </summary>
        Command<StoreItemLevel> NavigateToStoreLevelProductsCommand { get; }

        /// <summary>
        /// Selected item viewmodel.
        /// </summary>
        StoreItemViewModel SelectedItemViewModel { get; set; }

        /// <summary>
        /// Showitem options.
        /// </summary>
        Command<StoreItemViewModel> ShowItemOptionsCommand { get; }

        /// <summary>
        /// Store list viewmodel.
        /// </summary>
        IStoreListViewModel StoreListViewModel { get; }

        /// <summary>
        /// Command to change the view to production list mode.
        /// </summary>
        Command<bool?> SwitchToProductionListModeCommand { get; }
    }
}