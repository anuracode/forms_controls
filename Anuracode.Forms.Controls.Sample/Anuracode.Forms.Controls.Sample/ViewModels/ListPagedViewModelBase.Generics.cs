// <copyright file="ListPagedViewModelBase.Generics.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Sample.Interfaces;
using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using Anuracode.Forms.Controls.Extensions;

namespace Anuracode.Forms.Controls.Sample.ViewModels
{
    /// <summary>
    /// View model for the list of items.
    /// </summary>    
    public abstract class ListPagedViewModelBase<TItem> : BaseViewModel, IListPagedViewModelBase<TItem>
        where TItem : class
    {
        /// <summary>
        /// Add item.
        /// </summary>
        private Command addItemCommand;

        /// <summary>
        /// Delete article.
        /// </summary>
        private Command<TItem> deleteItemCommand;

        /// <summary>
        /// Edit article.
        /// </summary>
        private Command<TItem> editItemCommand;

        /// <summary>
        /// Token to use in the filters.
        /// </summary>
        private CancellationTokenSource filterCancellationTokenSource;

        /// <summary>
        /// Search term.
        /// </summary>
        private string filterTerm;

        /// <summary>
        /// Subject for the search tearm.
        /// </summary>
        private Subject<string> filterTermSubject;

        /// <summary>
        /// View model has been initilized.
        /// </summary>
        private bool initilized;

        /// <summary>
        /// Elemets to display.
        /// </summary>
        private ObservableCollectionFast<TItem> items = new ObservableCollectionFast<TItem>();

        /// <summary>
        /// Name of the entity to view, used for logging.
        /// </summary>
        private string itemTypeName;

        /// <summary>
        /// Command for loading the items.
        /// </summary>
        private Command loadItemsCommand;

        /// <summary>
        /// Command for loading the more items if the page results has more items.
        /// </summary>
        private Command loadMoreItemsCommand;

        /// <summary>
        /// Semaphore for the repository file.
        /// </summary>
        private SemaphoreSlim lockLoadItems;

        /// <summary>
        /// Do not use cache when quering the data.
        /// </summary>
        private bool refresh = false;

        /// <summary>
        /// Command for refreshing items.
        /// </summary>
        private Command refreshCommand;

        /// <summary>
        /// Count of the total items in the result.
        /// </summary>
        private int totalItemsCount;

        /// <summary>
        /// View article.
        /// </summary>
        private Command<TItem> viewItemDetailCommand;

        /// <summary>
        /// Default constuctor.
        /// </summary>
        public ListPagedViewModelBase()
            : base()
        {
            // Autocomplete.
            var observableFilterTerm = FilterTermSubject.ObserveOn(NewThreadScheduler.Default);

            var observableFilterTermThrottle = observableFilterTerm
            .DistinctUntilChanged()
            .Where(x => (string.IsNullOrWhiteSpace(x) ||
                (!string.IsNullOrWhiteSpace(x) && (x.Length > 1))))
            .Throttle(TimeSpan.FromSeconds(0.5));

            var lastTerm = (from osearchTerm in observableFilterTermThrottle
                            select Observable.Return(osearchTerm).TakeUntil(observableFilterTermThrottle)).Switch();

            IDisposable termDisposible = lastTerm.Subscribe(
                term =>
                {
                    if (LoadItemsCommand != null && LoadItemsCommand.CanExecute(null))
                    {
                        LoadItemsCommand.Execute(null);
                    }
                },
                HandlerCommandException);
        }

        /// <summary>
        /// Edit article.
        /// </summary>
        public virtual Command AddItemCommand
        {
            get
            {
                if (addItemCommand == null)
                {
                    addItemCommand = new Command(
                        AddItem,
                        CanAdd);
                }

                return addItemCommand;
            }
        }

        /// <summary>
        /// Can load more items.
        /// </summary>
        public bool CanLoadMoreItems
        {
            get
            {
                int itemsCount = Items.Count;
                return (Items != null) && (itemsCount > 0) && (itemsCount < TotalItemsCount);
            }
        }

        /// <summary>
        /// Delete item.
        /// </summary>
        public virtual Command<TItem> DeleteItemCommand
        {
            get
            {
                if (deleteItemCommand == null)
                {
                    deleteItemCommand = new Command<TItem>(
                        DeleteItem,
                        CanDelete);
                }

                return deleteItemCommand;
            }
        }

        /// <summary>
        /// Edit item.
        /// </summary>
        public virtual Command<TItem> EditItemCommand
        {
            get
            {
                if (editItemCommand == null)
                {
                    editItemCommand = new Command<TItem>(
                        EditItemDetail,
                        CanEditItem);
                }

                return editItemCommand;
            }
        }

        /// <summary>
        /// Search term.
        /// </summary>
        public string FilterTerm
        {
            get
            {
                return filterTerm;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref filterTerm, value);

                RefreshCommand.ChangeCanExecute();
                LoadItemsCommand.ChangeCanExecute();

                OnPropertyChanged(nameof(IsFiltered));

                FilterTermSubject.OnNext(value);
            }
        }

        /// <summary>
        /// Subject for the search tearm.
        /// </summary>
        public Subject<string> FilterTermSubject
        {
            get
            {
                if (filterTermSubject == null)
                {
                    filterTermSubject = new Subject<string>();
                }

                return filterTermSubject;
            }
        }

        /// <summary>
        /// View model has been initilized.
        /// </summary>
        public virtual bool Initialized
        {
            get
            {
                return initilized;
            }

            set
            {
                initilized = value;
                OnPropertyChanged(nameof(Initialized));
            }
        }

        /// <summary>
        /// Is filtered.
        /// </summary>
        public bool IsFiltered
        {
            get
            {
                return !string.IsNullOrWhiteSpace(FilterTerm) && (FilterTerm.Trim().Length > 0);
            }
        }

        /// <summary>
        /// Flag for when the system is loading.
        /// </summary>
        public virtual bool IsLoading
        {
            get
            {
                return !Initialized || !LoadItemsCommand.CanExecute(null) || !RefreshCommand.CanExecute(null);
            }
        }

        /// <summary>
        /// Elemets to display.
        /// </summary>
        public ObservableCollectionFast<TItem> Items
        {
            get { return items; }
        }

        /// <summary>
        /// Name of the entity to view, used for logging.
        /// </summary>
        public string ItemTypeName
        {
            get
            {
                if (itemTypeName == null)
                {
                    itemTypeName = typeof(TItem).Name;
                }

                return itemTypeName;
            }
        }

        /// <summary>
        /// Load elements.
        /// </summary>
        public virtual Command LoadItemsCommand
        {
            get
            {
                if (this.loadItemsCommand == null)
                {
                    var tmpCommand = new Command(
                        async () =>
                        {
                            IEnumerable<TItem> newElements = null;

                            try
                            {
                                CancellationToken ct = CancellationToken.None;

                                if (FilterCancellationTokenSource != null)
                                {
                                    ct = FilterCancellationTokenSource.Token;
                                }

                                await LockLoadItems.WaitAsync(ct);

                                OnPropertyChanged(nameof(IsLoading));
                                OnPropertyChanged(nameof(CanLoadMoreItems));
                                LoadMoreItemsCommand.ChangeCanExecute();

                                ct.ThrowIfCancellationRequested();

                                var pageResult = await LoadItemsAsync(FilterTerm, 0, PageSize, !Refresh, ct);

                                ct.ThrowIfCancellationRequested();

                                if (pageResult == null)
                                {
                                    TotalItemsCount = 0;
                                }
                                else
                                {
                                    TotalItemsCount = pageResult.TotalItemsCount;

                                    if (pageResult.PagedItems == null)
                                    {
                                        newElements = null;
                                    }
                                    else
                                    {
                                        newElements = await pageResult.PagedItems.ToListAsync();
                                    }
                                }
                            }
                            finally
                            {
                                LockLoadItems.Release();
                            }

                            AC.ScheduleManaged(
                                async () =>
                            {
                                await Task.FromResult(0);

                                LoadItemsCommand_CommandCompleted(this, newElements);
                            });
                        },
                        CanLoadItems);

                    this.loadItemsCommand = tmpCommand;
                }

                return this.loadItemsCommand;
            }
        }

        /// <summary>
        /// Load elements.
        /// </summary>
        public virtual Command LoadMoreItemsCommand
        {
            get
            {
                if (this.loadMoreItemsCommand == null)
                {
                    var tmpCommand = new Command(
                        async () =>
                        {
                            try
                            {
                                CancellationToken ct = CancellationToken.None;

                                if (FilterCancellationTokenSource != null)
                                {
                                    ct = FilterCancellationTokenSource.Token;
                                }

                                await LockLoadItems.WaitAsync(ct);

                                OnPropertyChanged(nameof(IsLoading));
                                OnPropertyChanged(nameof(CanLoadMoreItems));
                                LoadMoreItemsCommand.ChangeCanExecute();

                                ct.ThrowIfCancellationRequested();

                                var pageResult = await LoadItemsAsync(FilterTerm, Items.Count, PageSize, false, ct);

                                ct.ThrowIfCancellationRequested();

                                if ((pageResult != null) && (pageResult.PagedItems != null) && (await pageResult.PagedItems.FirstOrDefaultAsync() != null))
                                {
                                    try
                                    {
                                        var newItems = await pageResult.PagedItems.ToListAsync();

                                        AC.ScheduleManaged(
                                            async () =>
                                            {
                                                try
                                                {
                                                    await LockUI.WaitAsync();
                                                    Items.Merge(newItems);

                                                    OnPropertyChanged(nameof(IsLoading));
                                                    OnPropertyChanged(nameof(CanLoadMoreItems));
                                                    LoadMoreItemsCommand.ChangeCanExecute();
                                                }
                                                finally
                                                {
                                                    LockUI.Release();
                                                }
                                            });
                                    }
                                    catch (Exception ex)
                                    {
                                        throw ex;
                                    }
                                }
                            }
                            finally
                            {
                                LockLoadItems.Release();
                            }
                        },
                        () =>
                        {
                            return CanLoadMoreItems;
                        });

                    this.loadMoreItemsCommand = tmpCommand;
                }

                return this.loadMoreItemsCommand;
            }
        }

        /// <summary>
        /// Semaphore for the repository file.
        /// </summary>
        public SemaphoreSlim LockLoadItems
        {
            get
            {
                if (lockLoadItems == null)
                {
                    lockLoadItems = new SemaphoreSlim(1);
                }

                return lockLoadItems;
            }
        }

        /// <summary>
        /// Page title.
        /// </summary>
        public abstract string PageTitle { get; }

        /// <summary>
        /// Command for refresh.
        /// </summary>
        public virtual Command RefreshCommand
        {
            get
            {
                if (refreshCommand == null)
                {
                    var tmpCommand = new Command(
                        RefreshAction);

                    this.refreshCommand = tmpCommand;
                }

                return refreshCommand;
            }
        }

        /// <summary>
        /// Count of the total items in the result.
        /// </summary>
        public int TotalItemsCount
        {
            get
            {
                return totalItemsCount;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref totalItemsCount, value);
            }
        }

        /// <summary>
        /// View item detail.
        /// </summary>
        public Command<TItem> ViewItemDetailCommand
        {
            get
            {
                if (viewItemDetailCommand == null)
                {
                    viewItemDetailCommand = new Command<TItem>(
                        ViewItemDetail,
                        CanViewItem);
                }

                return viewItemDetailCommand;
            }
        }

        /// <summary>
        /// Token to use in the searches.
        /// </summary>
        protected CancellationTokenSource FilterCancellationTokenSource
        {
            get
            {
                return filterCancellationTokenSource;
            }

            set
            {
                filterCancellationTokenSource = value;
            }
        }

        /// <summary>
        /// Page size to initially display.
        /// </summary>
        public virtual int PageSize
        {
            get
            {
                return Device.OS.OnPlatform(15, 15, 15, 15, 30);
            }
        }

        /// <summary>
        /// Do not use cache when quering the data.
        /// </summary>
        protected virtual bool Refresh
        {
            get
            {
                return refresh;
            }

            set
            {
                refresh = value;
            }
        }

        /// <summary>
        /// Flag to refresh on first load.
        /// </summary>
        protected virtual bool RefreshOnFirstLoad
        {
            get
            {
                return true;
            }
        }     

        /// <summary>
        /// Add item.
        /// </summary>
        /// <returns></returns>
        protected virtual void AddItem()
        {
        }

        /// <summary>
        /// Can add item.
        /// </summary>
        /// <returns>True when can add.</returns>
        protected virtual bool CanAdd()
        {
            return true;
        }

        /// <summary>
        /// Check if an item can be deleted.
        /// </summary>
        /// <param name="item">Item to delete.</param>
        /// <returns>True if can delete.</returns>
        protected virtual bool CanDelete(TItem item)
        {
            return item != null;
        }

        /// <summary>
        /// Can edit item.
        /// </summary>
        /// <param name="item">Item to edit.</param>
        /// <returns>True if can do.</returns>
        protected virtual bool CanEditItem(TItem item)
        {
            return item != null;
        }

        /// <summary>
        /// Can load items.
        /// </summary>
        /// <returns>True when it can.</returns>
        protected virtual bool CanLoadItems()
        {
            return true;
        }

        /// <summary>
        /// Can view item.
        /// </summary>
        /// <param name="item">Item to view.</param>
        /// <returns>True when it can.</returns>
        protected virtual bool CanViewItem(TItem item)
        {
            return item != null;
        }

        /// <summary>
        /// Delete the element.
        /// </summary>
        /// <param name="item">Item to use.</param>
        /// <returns>Task to await.</returns>
        protected virtual void DeleteItem(TItem item)
        {
        }

        /// <summary>
        /// Edit item detail.
        /// </summary>
        /// <param name="item">Item to edit.</param>
        /// <returns>Task to await.</returns>
        protected virtual void EditItemDetail(TItem item)
        {
        }

        /// <summary>
        /// Load the items to show.
        /// </summary>
        /// <param name="filterTerm">Term to filter the elements with.</param>
        /// <param name="skip">The number of elements to skip from the result.</param>
        /// <param name="pageSize">The number of elements to take from the result.</param>
        /// <param name="cacheData">True when the data should be reloaded without cache.</param>
        /// <param name="cancellationToken">Cancellation token for when the operation is cancel.</param>
        /// <returns>Results to use.</returns>
        protected abstract Task<PagedResult<TItem>> LoadItemsAsync(
            string filterTerm,
            int skip,
            int pageSize,
            bool cacheData,
            CancellationToken cancellationToken);

        /// <summary>
        /// Event when the load items command is complete.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="newElements">New elements to use.</param>
        protected virtual void LoadItemsCommand_CommandCompleted(object sender, IEnumerable<TItem> newElements)
        {
            Refresh = false;

            AC.ScheduleManaged(
                async () =>
                {
                    try
                    {
                        await LockUI.WaitAsync();

                        if ((newElements != null) && (await newElements.FirstOrDefaultAsync() != null))
                        {
                            try
                            {
                                Items.Reset(newElements);
                            }
                            catch (Exception ex)
                            {
                                AC.TraceError("Items.Reset", ex);
                                throw ex;
                            }
                            finally
                            {
                                Title = string.Format("{0} ({1:N0})", PageTitle, TotalItemsCount);

                                OnPropertyChanged(nameof(IsLoading));
                                OnPropertyChanged(nameof(CanLoadMoreItems));
                                LoadMoreItemsCommand.ChangeCanExecute();
                                Initialized = true;
                            }
                        }
                        else
                        {
                            try
                            {
                                Items.Clear();
                            }
                            catch (ArgumentOutOfRangeException)
                            {
                                // Skip becasue there is a bug in Xamarin.Forms.
                            }
                            catch (Exception ex)
                            {
                                AC.TraceError("Items.Clear", ex);
                                throw ex;
                            }
                            finally
                            {
                                Title = string.Format("{0} ({1:N0})", PageTitle, 0);

                                OnPropertyChanged(nameof(IsLoading));
                                OnPropertyChanged(nameof(CanLoadMoreItems));
                                LoadMoreItemsCommand.ChangeCanExecute();
                                Initialized = true;
                            }
                        }
                    }
                    finally
                    {
                        LockUI.Release();
                    }
                });
        }

        /// <summary>
        /// Method when the viewmodel is refreshed.
        /// </summary>
        /// <returns></returns>
        protected virtual void RefreshAction()
        {
            Refresh = true;

            if (LoadItemsCommand.CanExecute(null))
            {
                LoadItemsCommand.Execute(null);
            }
        }

        /// <summary>
        /// View item detail.
        /// </summary>
        /// <param name="item">Item to use.</param>
        /// <returns>Task to await.</returns>
        protected virtual void ViewItemDetail(TItem item)
        {
        }
    }
}
