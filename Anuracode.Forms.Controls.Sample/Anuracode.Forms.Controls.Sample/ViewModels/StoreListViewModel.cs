// <copyright file="ListStoreViewModel.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Extensions;
using Anuracode.Forms.Controls.Sample.Model;
using Anuracode.Forms.Controls.Sample.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls.Sample.ViewModels
{
    /// <summary>
    /// ViewModel for the listing of all the store elements.
    /// </summary>
    public class StoreListViewModel : ListPagedViewModelBase<StoreItemViewModel>, IStoreListViewModel
    {
        /// <summary>
        /// Key to store the settings.
        /// </summary>
        public const string RECENT_SEARCH_SETTINGS_KEY = "RecentSearchCache";

        /// <summary>
        /// Maximum of recent searches.
        /// </summary>
        private const int MAX_RECENT_SEARCHES = 5;

        /// <summary>
        /// Lock for the images.
        /// </summary>
        private static SemaphoreSlim lockQuery = new SemaphoreSlim(1);

        /// <summary>
        /// Elemets to display.
        /// </summary>
        private ObservableCollectionFast<StoreItemViewModel> featuredItems = new ObservableCollectionFast<StoreItemViewModel>();

        /// <summary>
        /// Group to use.
        /// </summary>
        private int groupType;

        /// <summary>
        /// Flag when is in full search mode.
        /// </summary>
        private bool isFullSearchMode;

        /// <summary>
        /// True when the list is a list of product not of categories.
        /// </summary>
        private bool isProductListMode;

        /// <summary>
        /// Date of the last search.
        /// </summary>
        private DateTime? lastSearchDate;

        /// <summary>
        /// Level for the elements.
        /// </summary>
        private StoreItemLevel level;

        /// <summary>
        /// Load elements.
        /// </summary>
        private Command loadFeaturedItemsCommand;

        /// <summary>
        /// counter of load more.
        /// </summary>
        private int loadMoreCount = 0;

        /// <summary>
        /// Load elements.
        /// </summary>
        private Command loadSublevelsCommand;

        /// <summary>
        /// Load elements.
        /// </summary>
        private Command loadSuggestedItemsCommand;

        /// <summary>
        /// Elemets to display.
        /// </summary>
        private ObservableCollectionFast<string> recentSearches = new ObservableCollectionFast<string>();

        /// <summary>
        /// Sublevels to display.
        /// </summary>
        private ObservableCollectionFast<StoreItemLevel> sublevels = new ObservableCollectionFast<StoreItemLevel>();

        /// <summary>
        /// Sublevels viewmodel.
        /// </summary>
        private ObservableCollectionFast<StoreItemLevelViewModel> sublevelsViewModels = new ObservableCollectionFast<StoreItemLevelViewModel>();

        /// <summary>
        /// Elemets to display.
        /// </summary>
        private ObservableCollectionFast<StoreItemViewModel> suggestedItems = new ObservableCollectionFast<StoreItemViewModel>();

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StoreListViewModel()
            : base()
        {
            Title = LocalizationResources.StoreItemLabel;
        }

        /// <summary>
        /// Command completed.
        /// </summary>
        public event EventHandler<IEnumerable<StoreItemViewModel>> LoadFeaturedItemsCommandCompleted;

        /// <summary>
        /// Command completed.
        /// </summary>
        public event EventHandler<IEnumerable<StoreItemViewModel>> LoadItemsCommandCompleted;

        /// <summary>
        /// Command completed.
        /// </summary>
        public event EventHandler<IEnumerable<StoreItemLevel>> LoadSublevelsCommandCompleted;

        /// <summary>
        /// Lock for the images.
        /// </summary>
        public static SemaphoreSlim LockQuery
        {
            get
            {
                return lockQuery;
            }
        }

        /// <summary>
        /// App image path.
        /// </summary>
        public string AppImagePath
        {
            get
            {
                return Theme.CommonResources.PathImageAppLogoLarge;
            }
        }

        /// <summary>
        /// Elemets to display.
        /// </summary>
        public ObservableCollectionFast<StoreItemViewModel> FeaturedItems
        {
            get
            {
                return featuredItems;
            }
        }

        /// <summary>
        /// Group to use.
        /// </summary>
        public int GroupType
        {
            get
            {
                return groupType;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref groupType, value);

                if (LoadItemsCommand.CanExecute(null))
                {
                    LoadItemsCommand.Execute(null);
                }
            }
        }

        /// <summary>
        /// Flag when is in full search mode.
        /// </summary>
        public bool IsFullSearchMode
        {
            get
            {
                return isFullSearchMode;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref isFullSearchMode, value);

                OnPropertyChanged(nameof(IsProductListModeNoFullSearch));
            }
        }

        /// <summary>
        /// True when the list is a list of product not of categories.
        /// </summary>
        public bool IsProductListMode
        {
            get
            {
                return isProductListMode;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref isProductListMode, value);

                OnPropertyChanged(nameof(IsProductListModeNoFullSearch));
            }
        }

        /// <summary>
        /// Flag when is product list and not full search.
        /// </summary>
        public bool IsProductListModeNoFullSearch
        {
            get
            {
                return IsProductListMode && !IsFullSearchMode;
            }
        }

        /// <summary>
        /// Flag for the view to start on search mode.
        /// </summary>
        public bool IsStartViewSearchMode { get; set; }

        /// <summary>
        /// Date of the last search.
        /// </summary>
        public DateTime? LastSearchDate
        {
            get
            {
                return lastSearchDate;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref lastSearchDate, value);
            }
        }

        /// <summary>
        /// Level for the elements.
        /// </summary>
        public StoreItemLevel Level
        {
            get
            {
                if (level == null)
                {
                    level = new StoreItemLevel();
                }

                return level;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref level, value);
            }
        }

        /// <summary>
        /// Load elements.
        /// </summary>
        public virtual Command LoadFeaturedItemsCommand
        {
            get
            {
                if (this.loadFeaturedItemsCommand == null)
                {
                    var tmpCommand = new Command(
                        async () =>
                        {
                            IEnumerable<StoreItemViewModel> newElements = null;

                            if (!IsFullSearchMode)
                            {
                                try
                                {
                                    await LockQuery.WaitAsync();

                                    var pageResult = await RepositoryStoreItem.GetPagedAsync(Level, string.Empty, 0, FeaturedPageSize, true, CancellationToken.None, 0, true);
                                    newElements = pageResult.PagedItems;
                                }
                                finally
                                {
                                    LockQuery.Release();
                                }
                            }

                            if (LoadFeaturedItemsCommandCompleted != null)
                            {
                                AC.ScheduleManaged(
                                   () =>
                                   {
                                       if (LoadFeaturedItemsCommandCompleted != null)
                                       {
                                           LoadFeaturedItemsCommandCompleted(this, newElements);
                                       }
                                   });
                            }

                            if ((newElements != null) && (await newElements.FirstOrDefaultAsync() != null))
                            {
                                AC.ScheduleManaged(
                                () =>
                                {
                                    FeaturedItems.Reset(newElements);

                                    return Task.FromResult(0);
                                });
                            }
                            else
                            {
                                AC.ScheduleManaged(
                                () =>
                                {
                                    try
                                    {
                                        FeaturedItems.Clear();
                                    }
                                    catch (ArgumentOutOfRangeException)
                                    {
                                        // Skip becasue there is a bug in Xamarin.Forms.
                                    }

                                    return Task.FromResult(0);
                                });
                            }
                        },
                        () =>
                        {
                            return !IsFullSearchMode && (Level != null);
                        });

                    this.loadFeaturedItemsCommand = tmpCommand;
                }

                return this.loadFeaturedItemsCommand;
            }
        }

        /// <summary>
        /// Load elements.
        /// </summary>
        public virtual Command LoadSublevelsCommand
        {
            get
            {
                if (this.loadSublevelsCommand == null)
                {
                    var tmpCommand = new Command(
                        async () =>
                        {
                            IEnumerable<StoreItemLevel> newElements = null;

                            if (!IsFullSearchMode)
                            {
                                try
                                {
                                    await LockQuery.WaitAsync();

                                    newElements = await RepositoryStoreItem.GetSublevelsAsync(Level, true, CancellationToken.None);
                                }
                                finally
                                {
                                    LockQuery.Release();
                                }
                            }

                            if ((newElements != null) && (await newElements.FirstOrDefaultAsync() != null))
                            {
                                AC.ScheduleManaged(
                                () =>
                                {
                                    Sublevels.Reset(newElements);

                                    return Task.FromResult(0);
                                });
                            }
                            else
                            {
                                AC.ScheduleManaged(
                                () =>
                                {
                                    try
                                    {
                                        Sublevels.Clear();
                                    }
                                    catch (ArgumentOutOfRangeException)
                                    {
                                        // Skip becasue there is a bug in Xamarin.Forms.
                                    }

                                    return Task.FromResult(0);
                                });
                            }

                            if (LoadSublevelsCommandCompleted != null)
                            {
                                AC.ScheduleManaged(
                                   () =>
                                   {
                                       if (LoadSublevelsCommandCompleted != null)
                                       {
                                           LoadSublevelsCommandCompleted(this, newElements);
                                       }
                                   });
                            }

                            if (!IsProductListMode)
                            {
                                if ((newElements != null) && (await newElements.FirstOrDefaultAsync() != null))
                                {
                                    var newSublevelsViewmodel = from element in newElements
                                                                select new StoreItemLevelViewModel(element, HandlerCommandException, CancellationToken.None);

                                    AC.ScheduleManaged(
                                    () =>
                                    {
                                        SublevelsViewModels.Reset(newSublevelsViewmodel);

                                        return Task.FromResult(0);
                                    });
                                }
                                else
                                {
                                    AC.ScheduleManaged(
                                    () =>
                                    {
                                        try
                                        {
                                            SublevelsViewModels.Clear();
                                        }
                                        catch (ArgumentOutOfRangeException)
                                        {
                                            // Skip becasue there is a bug in Xamarin.Forms.
                                        }

                                        return Task.FromResult(0);
                                    });
                                }
                            }
                        },
                        () =>
                        {
                            return !IsFullSearchMode && (Level != null);
                        });

                    this.loadSublevelsCommand = tmpCommand;
                }

                return this.loadSublevelsCommand;
            }
        }

        /// <summary>
        /// Load elements.
        /// </summary>
        public virtual Command LoadSuggestedItemsCommand
        {
            get
            {
                if (this.loadSuggestedItemsCommand == null)
                {
                    var tmpCommand = new Command(
                        async () =>
                        {
                            IEnumerable<StoreItemViewModel> newElements = null;

                            if (IsFullSearchMode)
                            {
                                try
                                {
                                    await LockQuery.WaitAsync(CancellationToken.None);

                                    var pageResult = await RepositoryStoreItem.GetPagedAsync(Level, string.Empty, 0, FeaturedPageSize, true, CancellationToken.None, 0, true);
                                    newElements = pageResult.PagedItems;
                                }
                                finally
                                {
                                    LockQuery.Release();
                                }
                            }

                            if ((newElements != null) && (await newElements.FirstOrDefaultAsync() != null))
                            {
                                AC.ScheduleManaged(
                                () =>
                                {
                                    lock (SuggestedItems)
                                    {
                                        SuggestedItems.Reset(newElements);
                                    }

                                    return Task.FromResult(0);
                                });
                            }
                            else
                            {
                                AC.ScheduleManaged(
                                () =>
                                {
                                    try
                                    {
                                        lock (SuggestedItems)
                                        {
                                            SuggestedItems.Clear();
                                        }
                                    }
                                    catch (ArgumentOutOfRangeException)
                                    {
                                        // Skip becasue there is a bug in Xamarin.Forms.
                                    }

                                    return Task.FromResult(0);
                                });
                            }
                        },
                        () =>
                        {
                            return IsFullSearchMode && string.IsNullOrWhiteSpace(FilterTerm);
                        });

                    this.loadSuggestedItemsCommand = tmpCommand;
                }

                return this.loadSuggestedItemsCommand;
            }
        }

        /// <summary>
        /// Page title.
        /// </summary>
        public override string PageTitle
        {
            get
            {
                string title = Level.ToString();
                return string.IsNullOrWhiteSpace(title) ? LocalizationResources.StoreItemsLabel : title;
            }
        }

        /// <summary>
        /// Elemets to display.
        /// </summary>
        public ObservableCollectionFast<string> RecentSearches
        {
            get
            {
                return recentSearches;
            }
        }

        /// <summary>
        /// Repository for store items.
        /// </summary>
        public RepositoryStoreItem RepositoryStoreItem
        {
            get
            {
                return App.RepositoryStoreItem;
            }
        }

        /// <summary>
        /// Sublevels to display.
        /// </summary>
        public ObservableCollectionFast<StoreItemLevel> Sublevels
        {
            get
            {
                return sublevels;
            }
        }

        /// <summary>
        /// Sublevel viewmodel.
        /// </summary>
        public ObservableCollectionFast<StoreItemLevelViewModel> SublevelsViewModels
        {
            get
            {
                return sublevelsViewModels;
            }
        }

        /// <summary>
        /// Elemets to display.
        /// </summary>
        public ObservableCollectionFast<StoreItemViewModel> SuggestedItems
        {
            get
            {
                return suggestedItems;
            }
        }

        /// <summary>
        /// Featured page size.
        /// </summary>
        protected virtual int FeaturedPageSize
        {
            get
            {
                return 5;
            }
        }

        /// <summary>
        /// Loading not finished flag.
        /// </summary>
        protected bool LoadingNotFinished { get; set; }

        /// <summary>
        /// Page size.
        /// </summary>
        public override int PageSize
        {
            get
            {
                return Device.RuntimePlatform.OnPlatform(8, 8, 8, 8, 8);
            }
        }

        /// <summary>
        /// Flag to refresh on first load.
        /// </summary>
        protected override bool RefreshOnFirstLoad
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Add recent search.
        /// </summary>
        /// <param name="searchTerm">Search term.</param>
        public void AddRecentSearch(string searchTerm)
        {
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                AC.ScheduleManaged(
                    () =>
                    {
                        lock (RecentSearches)
                        {
                            var humanizedTerm = searchTerm.LetterCasingSentence();

                            int itemIndex = RecentSearches.IndexOf(humanizedTerm);

                            if (itemIndex > 0)
                            {
                                RecentSearches.RemoveAt(itemIndex);
                            }

                            if (itemIndex != 0)
                            {
                                RecentSearches.Insert(0, humanizedTerm);
                            }

                            while ((RecentSearches.Count > MAX_RECENT_SEARCHES) && (RecentSearches.Count > 0))
                            {
                                RecentSearches.RemoveAt(RecentSearches.Count - 1);
                            }

                            SaveRecentSearch();

                            return Task.FromResult(0);
                        }
                    });
            }
        }

        /// <summary>
        /// Handle exception from commands.
        /// </summary>
        /// <param name="ex">Exception to handle.</param>
        public override void HandlerCommandException(Exception ex)
        {
            if (ex is System.Threading.Tasks.TaskCanceledException)
            {
                LoadingNotFinished = true;
                return;
            }
            else if (ex is System.OperationCanceledException)
            {
                LoadingNotFinished = true;
                return;
            }
            else
            {
                base.HandlerCommandException(ex);
            }
        }

        /// <summary>
        /// Load recent search.
        /// </summary>
        public void LoadRecentSearch()
        {
            lock (RecentSearches)
            {
                // Implement.
            }
        }

        /// <summary>
        /// Can load items.
        /// </summary>
        /// <returns>True when it can.</returns>
        protected override bool CanLoadItems()
        {
            return base.CanLoadItems() && IsProductListMode;
        }

        /// <summary>
        /// Can share content.
        /// </summary>
        /// <returns>True when can share content.</returns>
        protected override bool CanShareContent()
        {
            return true;
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
        protected override async Task<PagedResult<StoreItemViewModel>> LoadItemsAsync(string filterTerm, int skip, int pageSize, bool cacheData, CancellationToken cancellationToken)
        {
            try
            {
                PagedResult<StoreItemViewModel> pagedResults = null;

                int pageMultiplayer = 1;

                if (skip > 0)
                {
                    loadMoreCount++;

                    if (loadMoreCount > 2)
                    {
                        pageMultiplayer = 2;

                        if (loadMoreCount > 6)
                        {
                            pageMultiplayer = 3;

                            if (loadMoreCount > 10)
                            {
                                pageMultiplayer = 4;
                            }
                        }
                    }
                }

                if ((IsFullSearchMode && !string.IsNullOrWhiteSpace(filterTerm)) || (!IsFullSearchMode))
                {
                    await Task.Delay(TimeSpan.FromSeconds(0.1));
                    await LockQuery.WaitAsync(cancellationToken);
                    await Task.Delay(TimeSpan.FromSeconds(0.1));

                    pagedResults = await RepositoryStoreItem.GetPagedAsync(Level, filterTerm, skip, (pageSize * pageMultiplayer), true, CancellationToken.None, GroupType);
                }
                else
                {
                    pagedResults = new PagedResult<StoreItemViewModel>()
                    {
                        PagedItems = new List<StoreItemViewModel>(),
                        TotalItemsCount = 0
                    };
                }

                LastSearchDate = DateTime.UtcNow;

                return pagedResults;
            }
            finally
            {
                LockQuery.Release();
            }
        }

        /// <summary>
        /// Event when the load items command is complete.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="newElements">New elements to use.</param>
        protected override async void LoadItemsCommand_CommandCompleted(object sender, IEnumerable<StoreItemViewModel> newElements)
        {
            base.LoadItemsCommand_CommandCompleted(sender, newElements);

            if (IsFullSearchMode)
            {
                if ((newElements != null) && (await newElements.FirstOrDefaultAsync() != null))
                {
                    AC.ScheduleManaged(
                    () =>
                    {
                        lock (SuggestedItems)
                        {
                            SuggestedItems.Reset(newElements);
                        }

                        return Task.FromResult(0);
                    });
                }
                else
                {
                    AC.ScheduleManaged(
                    () =>
                    {
                        try
                        {
                            lock (SuggestedItems)
                            {
                                SuggestedItems.Clear();
                            }
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            // Skip becasue there is a bug in Xamarin.Forms.
                        }

                        if (LoadSuggestedItemsCommand.CanExecute(null))
                        {
                            LoadSuggestedItemsCommand.Execute(null);
                        }

                        return Task.FromResult(0);
                    });
                }
            }

            if (LoadItemsCommandCompleted != null)
            {
                AC.ScheduleManaged(
                   () =>
                   {
                       if (LoadItemsCommandCompleted != null)
                       {
                           LoadItemsCommandCompleted(sender, newElements);
                       }
                   });
            }
        }

        /// <summary>
        /// Refresh the view.
        /// </summary>
        /// <returns>Task to await.</returns>
        protected override void RefreshAction()
        {
            OnPropertyChanged(nameof(IsLoading));

            if (LoadSublevelsCommand.CanExecute(null))
            {
                LoadSublevelsCommand.Execute(null);
            }

            if (LoadFeaturedItemsCommand.CanExecute(null))
            {
                LoadFeaturedItemsCommand.Execute(null);
            }

            base.RefreshAction();
        }

        /// <summary>
        /// Load recent search.
        /// </summary>
        protected void SaveRecentSearch()
        {
            // TODO: Implement.
        }
    }
}