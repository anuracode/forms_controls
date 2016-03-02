// <copyright file="IListPagedViewModelBase.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Xamarin.Forms;

namespace Anuracode.Forms.Controls.Sample.Interfaces
{
    /// <summary>
    /// Interface for a view that lists items.
    /// </summary>
    /// <typeparam name="TItem">Type for the item.</typeparam>
    public interface IListPagedViewModelBase<TItem>
    {
        /// <summary>
        /// Add item.
        /// </summary>
        Command AddItemCommand { get; }

        /// <summary>
        /// Can load more items.
        /// </summary>
        bool CanLoadMoreItems { get; }

        /// <summary>
        /// Delete article.
        /// </summary>
        Command<TItem> DeleteItemCommand { get; }

        /// <summary>
        /// Search term.
        /// </summary>
        string FilterTerm { get; set; }

        /// <summary>
        /// View model has been initilized.
        /// </summary>
        bool Initialized { get; }

        /// <summary>
        /// Is filtered.
        /// </summary>
        bool IsFiltered { get; }

        /// <summary>
        /// Flag for when the system is loading.
        /// </summary>
        bool IsLoading { get; }

        /// <summary>
        /// Elemets to display.
        /// </summary>
        ObservableCollectionFast<TItem> Items { get; }

        /// <summary>
        /// Load elements.
        /// </summary>
        Command LoadItemsCommand { get; }

        /// <summary>
        /// Load more elements.
        /// </summary>
        Command LoadMoreItemsCommand { get; }

        /// <summary>
        /// Command for refresh.
        /// </summary>
        Command RefreshCommand { get; }

        /// <summary>
        /// Count of the total items in the result.
        /// </summary>
        int TotalItemsCount { get; }

        /// <summary>
        /// View item detail.
        /// </summary>
        Command<TItem> ViewItemDetailCommand { get; }
    }
}