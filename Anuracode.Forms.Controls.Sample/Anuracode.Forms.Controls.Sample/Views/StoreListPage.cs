// <copyright file="StoreListPage.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Extensions;
using Anuracode.Forms.Controls.Sample.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reactive.Concurrency;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls.Sample.Views
{
    /// <summary>
    /// Page for the store items.
    /// </summary>
    public class StoreListPage : ListBasePagedView<StoreListViewModel, StoreItemViewModel> //, IStoreListPage
    {
        /// <summary>
        /// Counter for the autoscroll.
        /// </summary>
        private long autoScrollCounter = 0;

        /// <summary>
        /// Interval for auto scroll.
        /// </summary>
        private TimeSpan autoScrollInterval = TimeSpan.FromSeconds(15);

        /// <summary>
        /// View model for the store detail.
        /// </summary>
        private StoreDetailViewModel detailViewModel;

        /// <summary>
        /// Command for the filter bar.
        /// </summary>
        private Command<string> executeSearchCommand;

        /// <summary>
        /// Flag for idle.
        /// </summary>
        private bool featuredScrollIdle = false;

        /// <summary>
        /// Hide cart command.
        /// </summary>
        private Command hideCartCommand;

        /// <summary>
        /// Hide item group detail.
        /// </summary>
        private Command hideItemGroupDetailCommand;

        /// <summary>
        /// Hide item large detail command.
        /// </summary>
        private Command hideItemLargeDetailCommand;

        /// <summary>
        /// Hide items options command.
        /// </summary>
        private Command hideItemOptionsCommand;

        /// <summary>
        /// Flag for when the cart view has been added.
        /// </summary>
        private bool isCartViewSmallAdded = false;

        /// <summary>
        /// Flag for the cart.
        /// </summary>
        private bool isCartVisible;

        /// <summary>
        /// Flag for the group detail.
        /// </summary>
        private bool isGroupDetailVisible;

        /// <summary>
        /// Flag for the large detail.
        /// </summary>
        private bool isLargeDetailVisible;

        /// <summary>
        /// Level stack.
        /// </summary>
        private Stack<NavigationStep> levelStack;

        /// <summary>
        /// Navigate to store level.
        /// </summary>
        private DelegateCommandAsync<StoreItemLevel> navigateToStoreLevelCommand;

        /// <summary>
        /// Navigate to store level.
        /// </summary>
        private DelegateCommandAsync<StoreItemLevel> navigateToStoreLevelProductsCommand;

        /// <summary>
        /// Navigate to store level in search mode.
        /// </summary>
        private DelegateCommandAsync<StoreItemLevel> navigateToStoreLevelProductsSearchModeCommand;

        /// <summary>
        /// Command for refreshing items.
        /// </summary>
        private DelegateCommandAsync refreshCommand;

        /// <summary>
        /// navigate back depending on the view.
        /// </summary>
        private DelegateCommandAsync relativeBackCommand;

        /// <summary>
        /// Selected item viewmodel.
        /// </summary>
        private StoreItemViewModel selectedItemViewModel;

        /// <summary>
        /// Select payment type command.
        /// </summary>
        private DelegateCommandAsync selectGroupTypeCommand;

        /// <summary>
        /// Share content.
        /// </summary>
        private DelegateCommandAsync shareContentCommand;

        /// <summary>
        /// Show cart command.
        /// </summary>
        private DelegateCommandAsync showCartCommand;

        /// <summary>
        /// Show the item detail.
        /// </summary>
        private DelegateCommandAsync<object> showItemLargeDetailCommand;

        /// <summary>
        /// Show the items options.
        /// </summary>
        private DelegateCommandAsync<StoreItemViewModel> showItemOptionsCommand;

        /// <summary>
        /// Show search.
        /// </summary>
        private DelegateCommandAsync showSearchCommand;

        /// <summary>
        /// Command to change the view to category mode.
        /// </summary>
        private DelegateCommandAsync<bool?> switchToCategoryModeCommand;

        /// <summary>
        /// Command to change the view to production list mode.
        /// </summary>
        private DelegateCommandAsync<bool?> switchToProductionListModeCommand;

        /// <summary>
        /// Timer subscription.
        /// </summary>
        private IDisposable timerSubscription;

        /// <summary>
        /// Update the view to reflect a specifig navigation step.
        /// </summary>
        private DelegateCommandAsync<NavigationStep> updateViewToStepCommand;

        /// <summary>
        /// Complete the search with the suggestion.
        /// </summary>
        private DelegateCommandAsync<StoreItemViewModel> useSuggestionCommand;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="viewModel">View model to use.</param>
        public StoreListPage(StoreListViewModel viewModel)
            : base(viewModel)
        {
            ViewModel.LoadItemsCommand.CommandCompleted += LoadItemsCommand_CommandCompleted;
            ViewModel.LoadSublevelsCommand.CommandCompleted += LoadSublevelsCommand_CommandCompleted;
            ViewModel.LoadFeaturedItemsCommand.CommandCompleted += LoadFeaturedItemsCommand_CommandCompleted;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StoreListPage()
            : this(null)
        {
        }

        /// <summary>
        /// Total animation time.
        /// </summary>
        public static uint TotalShowDetailAnimationTime
        {
            get
            {
                return 450;
            }
        }

        /// <summary>
        /// Command that moves the scroll automatically.
        /// </summary>
        public DelegateCommandAsync AutoScrollFeaturedItemsCommand
        {
            get
            {
                if (autoScrollFeaturedItemsCommand == null)
                {
                    autoScrollFeaturedItemsCommand = new DelegateCommandAsync(
                        async () =>
                        {
                            await Task.FromResult(0);

                            Parcero.Core.App.ThreadManager.ScheduleManaged(
                                async () =>
                                {
                                    if ((this.Content != null) && (timerSubscription != null))
                                    {
                                        if (featuredScrollIdle)
                                        {
                                            if (FeaturedScrollView != null)
                                            {
                                                RepeaterRecycleView recycler = FeaturedScrollView as RepeaterRecycleView;

                                                if (recycler != null)
                                                {
                                                    if ((recycler.ItemsSource != null) && !recycler.IsLoading)
                                                    {
                                                        int totalItems = recycler.ItemsSource.Count;

                                                        if (totalItems > 0)
                                                        {
                                                            autoScrollCounter++;
                                                            long division = autoScrollCounter / totalItems;
                                                            int newIndex = Convert.ToInt32(autoScrollCounter - (totalItems * division));

                                                            await recycler.ScrollToIndexAsync(newIndex.Clamp(0, totalItems - 1), ScrollToPosition.Center, true);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    RepeaterView<StoreItemViewModel> stackScroll = FeaturedScrollView.Content as RepeaterView<StoreItemViewModel>;

                                                    if ((stackScroll != null) && (stackScroll.Children != null) && !stackScroll.IsLoading)
                                                    {
                                                        int totalItems = stackScroll.Children.Count;

                                                        if (totalItems > 0)
                                                        {
                                                            autoScrollCounter++;
                                                            long division = autoScrollCounter / totalItems;
                                                            int newIndex = Convert.ToInt32(autoScrollCounter - (totalItems * division));
                                                            Element newElement = stackScroll.Children[newIndex.Clamp(0, totalItems - 1)];

                                                            await FeaturedScrollView.ScrollToAsync(newElement, ScrollToPosition.Start, true);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            featuredScrollIdle = true;
                                        }
                                    }
                                });
                        },
                        () => AutoScrollFeaturedItemsCommand,
                        this,
                        ViewModel.HandlerCommandException);
                }

                return autoScrollFeaturedItemsCommand;
            }
        }

        /// <summary>
        /// View model for the store detail.
        /// </summary>
        public StoreDetailViewModel DetailViewModel
        {
            get
            {
                if (detailViewModel == null)
                {
                    detailViewModel = AC.Resolve<StoreDetailViewModel>();

                    detailViewModel.UpdateNavigation(this.Navigation);
                }

                return detailViewModel;
            }
        }

        /// <summary>
        /// Command for the filter bar.
        /// </summary>
        public override Command<string> ExecuteSearchCommand
        {
            get
            {
                if (executeSearchCommand == null)
                {
                    executeSearchCommand = new Command<string>(
                        async (newFilterTerm) =>
                        {
                            await Task.FromResult(0);

                            if (!string.IsNullOrWhiteSpace(newFilterTerm) && string.Compare(ViewModel.FilterTerm, newFilterTerm, StringComparison.CurrentCultureIgnoreCase) != 0)
                            {
                                ViewModel.FilterTerm = newFilterTerm;
                            }

                            ViewModel.AddRecentSearch(newFilterTerm);

                            HideSearchCommand.ExecuteIfCan();

                            FilterBar_Completed(this, null);
                        });
                }

                return executeSearchCommand;
            }
        }

        /// <summary>
        /// Show store.
        /// </summary>
        public DelegateCommandAsync HideCartCommand
        {
            get
            {
                if (hideCartCommand == null)
                {
                    hideCartCommand = new DelegateCommandAsync(
                        async () =>
                        {
                            await Task.FromResult(0);

                            if (IsCartVisible)
                            {
                                IsCartVisible = false;

                                if ((CartView != null) && CartView.HideOverlayCommand.CanExecute())
                                {
                                    await CartView.HideOverlayCommand.ExecuteAsync();
                                }
                            }
                        },
                        () =>
                        {
                            return IsCartVisible;
                        },
                        () => HideCartCommand,
                        this,
                        ViewModel.HandlerCommandException);
                }

                return hideCartCommand;
            }
        }

        /// <summary>
        /// Hide view.
        /// </summary>
        public DelegateCommandAsync HideItemGroupDetailCommand
        {
            get
            {
                if (hideItemGroupDetailCommand == null)
                {
                    hideItemGroupDetailCommand = new DelegateCommandAsync(
                        async () =>
                        {
                            await Task.FromResult(0);

                            if (IsGroupDetailVisible)
                            {
                                if (GroupItemsView.BindingContext != DetailViewModel)
                                {
                                    GroupItemsView.BindingContext = DetailViewModel;
                                }

                                if (GroupItemsView != null && GroupItemsView.HideOverlayCommand.CanExecute())
                                {
                                    await GroupItemsView.HideOverlayCommand.ExecuteAsync();
                                }

                                DetailViewModel.CurrentLoadedGroup = null;
                                IsGroupDetailVisible = false;
                            }
                        },
                        () =>
                        {
                            return IsGroupDetailVisible;
                        },
                        () => HideItemGroupDetailCommand,
                        this,
                        ViewModel.HandlerCommandException);
                }

                return hideItemGroupDetailCommand;
            }
        }

        /// <summary>
        /// Show store.
        /// </summary>
        public DelegateCommandAsync HideItemLargeDetailCommand
        {
            get
            {
                if (hideItemLargeDetailCommand == null)
                {
                    hideItemLargeDetailCommand = new DelegateCommandAsync(
                        async () =>
                        {
                            await Task.FromResult(0);

                            if (IsLargeDetailVisible)
                            {
                                IsLargeDetailVisible = false;

                                if ((DetailLargeView != null) && DetailLargeView.HideOverlayCommand.CanExecute())
                                {
                                    await DetailLargeView.HideOverlayCommand.ExecuteAsync();
                                }
                            }
                        },
                        () =>
                        {
                            return IsLargeDetailVisible;
                        },
                        () => HideItemLargeDetailCommand,
                        this,
                        ViewModel.HandlerCommandException);
                }

                return hideItemLargeDetailCommand;
            }
        }

        /// <summary>
        /// Hide items options command.
        /// </summary>
        public DelegateCommandAsync HideItemOptionsCommand
        {
            get
            {
                if (hideItemOptionsCommand == null)
                {
                    hideItemOptionsCommand = new DelegateCommandAsync(
                        async () =>
                        {
                            await Task.FromResult(0);

                            if (ItemDetailView != null && ItemDetailView.HideOverlayCommand.CanExecute())
                            {
                                await ItemDetailView.HideOverlayCommand.ExecuteAsync();
                            }

                            SelectedItemViewModel = null;
                        },
                        () =>
                        {
                            return SelectedItemViewModel != null;
                        },
                        () => HideItemOptionsCommand,
                        this,
                        ViewModel.HandlerCommandException);
                }

                return hideItemOptionsCommand;
            }
        }

        /// <summary>
        /// Flag for the cart.
        /// </summary>
        public bool IsCartVisible
        {
            get
            {
                return isCartVisible;
            }

            set
            {
                if (isCartVisible != value)
                {
                    isCartVisible = value;
                    OnPropertyChanged("IsCartVisible");
                    ShowCartCommand.RaiseCanExecuteChanged();
                    HideCartCommand.RaiseCanExecuteChanged();
                    ShowSearchCommand.RaiseCanExecuteChanged();
                    ShareContentCommand.RaiseCanExecuteChanged();
                    RefreshCommand.RaiseCanExecuteChanged();
                    RelativeBackCommand.RaiseCanExecuteChanged();
                    SwitchToCategoryModeCommand.RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        /// Flag for the large detail.
        /// </summary>
        public bool IsGroupDetailVisible
        {
            get
            {
                return isGroupDetailVisible;
            }

            set
            {
                if (isGroupDetailVisible != value)
                {
                    isGroupDetailVisible = value;
                    OnPropertyChanged("IsGroupDetailVisible");
                    HideItemGroupDetailCommand.RaiseCanExecuteChanged();
                    ShowSearchCommand.RaiseCanExecuteChanged();
                    RefreshCommand.RaiseCanExecuteChanged();
                    RelativeBackCommand.RaiseCanExecuteChanged();
                    ShareContentCommand.RaiseCanExecuteChanged();
                    SwitchToCategoryModeCommand.RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        /// Flag for the large detail.
        /// </summary>
        public bool IsLargeDetailVisible
        {
            get
            {
                return isLargeDetailVisible;
            }

            set
            {
                if (isLargeDetailVisible != value)
                {
                    isLargeDetailVisible = value;
                    OnPropertyChanged("IsLargeDetailVisible");
                    ShowItemLargeDetailCommand.RaiseCanExecuteChanged();
                    HideItemLargeDetailCommand.RaiseCanExecuteChanged();
                    ShowSearchCommand.RaiseCanExecuteChanged();
                    RelativeBackCommand.RaiseCanExecuteChanged();
                    SwitchToCategoryModeCommand.RaiseCanExecuteChanged();
                    ShareContentCommand.RaiseCanExecuteChanged();
                    RefreshCommand.RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        /// Level stack.
        /// </summary>
        public Stack<NavigationStep> LevelStack
        {
            get
            {
                if (levelStack == null)
                {
                    levelStack = new Stack<NavigationStep>();
                }

                return levelStack;
            }
        }

        /// <summary>
        /// Navigate to store level.
        /// </summary>
        public DelegateCommandAsync<StoreItemLevel> NavigateToStoreLevelCommand
        {
            get
            {
                if (navigateToStoreLevelCommand == null)
                {
                    navigateToStoreLevelCommand = new DelegateCommandAsync<StoreItemLevel>(
                        async (subLevel) =>
                        {
                            bool calculatedProductMode = (subLevel != null) && !string.IsNullOrWhiteSpace(subLevel.Department) && !string.IsNullOrWhiteSpace(subLevel.Category) && !string.IsNullOrWhiteSpace(subLevel.Subcategory);

                            if (UseSingleViewSublevels)
                            {
                                LevelStack.Push(new NavigationStep()
                                {
                                    Level = ViewModel.Level,
                                    IsProductListMode = ViewModel.IsProductListMode,
                                    IsFullSearchMode = ViewModel.IsFullSearchMode,
                                    IsSearchMode = IsSearchVisible,
                                    FilterTerm = ViewModel.FilterTerm,
                                    SelectedItemViewModel = SelectedItemViewModel,
                                    IsCartVisible = IsCartVisible,
                                    IsGroupDetailVisible = IsGroupDetailVisible,
                                    IsLargeDetailVisible = IsLargeDetailVisible,
                                    CurrentLoadedGroup = DetailViewModel.CurrentLoadedGroup,
                                    SelectedDetailItemViewModel = DetailViewModel != null ? DetailViewModel.CurrentItemViewModel : null
                                });

                                NavigationStep newStep = new NavigationStep()
                                {
                                    Level = subLevel,
                                    IsProductListMode = false,
                                    IsFullSearchMode = false
                                };

                                if (UpdateViewToStepCommand.CanExecute(newStep))
                                {
                                    await UpdateViewToStepCommand.ExecuteAsync(newStep);
                                }
                            }
                            else
                            {
                                await NavigationHelper.ShowStoreLevel(subLevel, ViewModel, ViewModel.NavigationCancellationToken, calculatedProductMode);
                                ViewModel.NavigationCancellationToken = new CancellationTokenSource();
                            }
                        },
                        (sublevel) =>
                        {
                            return sublevel != null;
                        },
                        () => NavigateToStoreLevelCommand,
                        this,
                        ViewModel.HandlerCommandException);
                }

                return navigateToStoreLevelCommand;
            }
        }

        /// <summary>
        /// Navigate to store level.
        /// </summary>
        public DelegateCommandAsync<StoreItemLevel> NavigateToStoreLevelProductsCommand
        {
            get
            {
                if (navigateToStoreLevelProductsCommand == null)
                {
                    navigateToStoreLevelProductsCommand = new DelegateCommandAsync<StoreItemLevel>(
                        async (subLevel) =>
                        {
                            if (UseSingleViewSublevels)
                            {
                                LevelStack.Push(new NavigationStep()
                                {
                                    Level = ViewModel.Level,
                                    IsProductListMode = ViewModel.IsProductListMode,
                                    IsFullSearchMode = ViewModel.IsFullSearchMode,
                                    IsSearchMode = IsSearchVisible,
                                    FilterTerm = ViewModel.FilterTerm,
                                    SelectedItemViewModel = SelectedItemViewModel,
                                    IsCartVisible = IsCartVisible,
                                    IsGroupDetailVisible = IsGroupDetailVisible,
                                    IsLargeDetailVisible = IsLargeDetailVisible,
                                    CurrentLoadedGroup = DetailViewModel.CurrentLoadedGroup,
                                    SelectedDetailItemViewModel = DetailViewModel != null ? DetailViewModel.CurrentItemViewModel : null
                                });

                                NavigationStep newStep = new NavigationStep()
                                {
                                    Level = subLevel,
                                    IsProductListMode = true
                                };

                                if (UpdateViewToStepCommand.CanExecute(newStep))
                                {
                                    await UpdateViewToStepCommand.ExecuteAsync(newStep);
                                }
                            }
                            else
                            {
                                await NavigationHelper.ShowStoreLevel(subLevel, ViewModel, ViewModel.NavigationCancellationToken, true);
                                ViewModel.NavigationCancellationToken = new CancellationTokenSource();
                            }
                        },
                        (sublevel) =>
                        {
                            return sublevel != null;
                        },
                        () => NavigateToStoreLevelProductsCommand,
                        this,
                        ViewModel.HandlerCommandException);
                }

                return navigateToStoreLevelProductsCommand;
            }
        }

        /// <summary>
        /// Navigate to store level.
        /// </summary>
        public DelegateCommandAsync<StoreItemLevel> NavigateToStoreLevelProductsSearchModeCommand
        {
            get
            {
                if (navigateToStoreLevelProductsSearchModeCommand == null)
                {
                    navigateToStoreLevelProductsSearchModeCommand = new DelegateCommandAsync<StoreItemLevel>(
                        async (subLevel) =>
                        {
                            if (UseSingleViewSublevels)
                            {
                                LevelStack.Push(new NavigationStep()
                                {
                                    Level = ViewModel.Level,
                                    IsProductListMode = ViewModel.IsProductListMode,
                                    IsFullSearchMode = ViewModel.IsFullSearchMode,
                                    IsSearchMode = IsSearchVisible,
                                    FilterTerm = ViewModel.FilterTerm,
                                    SelectedItemViewModel = SelectedItemViewModel,
                                    IsCartVisible = IsCartVisible,
                                    IsGroupDetailVisible = IsGroupDetailVisible,
                                    IsLargeDetailVisible = IsLargeDetailVisible,
                                    CurrentLoadedGroup = DetailViewModel.CurrentLoadedGroup,
                                    SelectedDetailItemViewModel = DetailViewModel != null ? DetailViewModel.CurrentItemViewModel : null
                                });

                                NavigationStep newStep = new NavigationStep()
                                {
                                    Level = subLevel,
                                    IsProductListMode = true,
                                    IsSearchMode = true
                                };

                                if (UpdateViewToStepCommand.CanExecute(newStep))
                                {
                                    await UpdateViewToStepCommand.ExecuteAsync(newStep);
                                }
                            }
                            else
                            {
                                await NavigationHelper.ShowStoreLevel(subLevel, ViewModel, ViewModel.NavigationCancellationToken, true, true);
                                ViewModel.NavigationCancellationToken = new CancellationTokenSource();
                            }
                        },
                        (sublevel) =>
                        {
                            return sublevel != null;
                        },
                        () => NavigateToStoreLevelProductsSearchModeCommand,
                        this,
                        ViewModel.HandlerCommandException);
                }

                return navigateToStoreLevelProductsSearchModeCommand;
            }
        }

        /// <summary>
        /// Command for refresh.
        /// </summary>
        public virtual DelegateCommandAsync RefreshCommand
        {
            get
            {
                if (refreshCommand == null)
                {
                    var tmpCommand = new DelegateCommandAsync(
                        async () =>
                        {
                            await Task.FromResult(0);

                            if (IsCartVisible)
                            {
                                if (CartView != null)
                                {
                                    CartView.ViewModel.RefreshCommand.ExecuteIfCan();
                                }
                            }
                            else
                            {
                                ViewModel.RefreshCommand.ExecuteIfCan();
                            }
                        },
                        () =>
                        {
                            return IsCartVisible || (!IsCartVisible && (SelectedItemViewModel == null) && !IsGroupDetailVisible && !IsLargeDetailVisible);
                        },
                        () => RefreshCommand,
                        this,
                        ViewModel.HandlerCommandException);

                    this.refreshCommand = tmpCommand;
                }

                return refreshCommand;
            }
        }

        /// <summary>
        /// Show search.
        /// </summary>
        public DelegateCommandAsync RelativeBackCommand
        {
            get
            {
                if (relativeBackCommand == null)
                {
                    relativeBackCommand = new DelegateCommandAsync(
                        async () =>
                        {
                            await Task.FromResult(0);

                            if (IsCartVisible)
                            {
                                HideCartCommand.ExecuteIfCan();
                            }
                            else if (IsLargeDetailVisible)
                            {
                                if (LastSelectedCartItem == null)
                                {
                                    HideItemLargeDetailCommand.ExecuteIfCan();
                                }
                                else
                                {
                                    LastSelectedCartItem = null;

                                    if (ShowCartCommand.CanExecute())
                                    {
                                        await ShowCartCommand.ExecuteAsync();
                                    }

                                    if (CacheItemDetailCart == null)
                                    {
                                        HideItemLargeDetailCommand.ExecuteIfCan();
                                    }
                                    else
                                    {
                                        DetailViewModel.CurrentItemViewModel = CacheItemDetailCart;

                                        if (DetailLargeView.BindingContext != DetailViewModel)
                                        {
                                            DetailLargeView.BindingContext = DetailViewModel;
                                        }

                                        AC.ThreadManager.ScheduleManaged(
                                            () =>
                                            {
                                                DetailLargeView.UpdateProductDetail();
                                            });

                                        CacheItemDetailCart = null;
                                    }
                                }
                            }
                            else if (SelectedItemViewModel != null)
                            {
                                HideItemOptionsCommand.ExecuteIfCan();
                            }
                            else if (IsGroupDetailVisible)
                            {
                                HideItemGroupDetailCommand.ExecuteIfCan();
                            }
                            else if (LevelStack.Count > 0)
                            {
                                var oldStep = LevelStack.Pop();

                                if (UpdateViewToStepCommand.CanExecute(oldStep))
                                {
                                    await UpdateViewToStepCommand.ExecuteAsync(oldStep);
                                }
                            }
                            else
                            {
                                ViewModel.NavigateBackManagedCommand.ExecuteIfCan();
                            }
                        },
                        () =>
                        {
                            if (ViewModel.IsRootView)
                            {
                                return IsCartVisible || IsLargeDetailVisible || (SelectedItemViewModel != null) || IsGroupDetailVisible || (LevelStack.Count > 0);
                            }
                            else
                            {
                                return ViewModel.NavigateBackManagedCommand.CanExecute();
                            }
                        },
                        () => RelativeBackCommand,
                        this,
                        ViewModel.HandlerCommandException);
                }

                return relativeBackCommand;
            }
        }

        /// <summary>
        /// Selected item viewmodel.
        /// </summary>
        public StoreItemViewModel SelectedItemViewModel
        {
            get
            {
                return selectedItemViewModel;
            }

            set
            {
                selectedItemViewModel = value;

                OnPropertyChanged("SelectedItemViewModel");

                ShowItemOptionsCommand.RaiseCanExecuteChanged();
                HideItemOptionsCommand.RaiseCanExecuteChanged();
                ShowSearchCommand.RaiseCanExecuteChanged();
                RefreshCommand.RaiseCanExecuteChanged();
                SelectGroupTypeCommand.RaiseCanExecuteChanged();
                ShareContentCommand.RaiseCanExecuteChanged();
                RelativeBackCommand.RaiseCanExecuteChanged();

                if (ViewModel != null)
                {
                    ViewModel.IncreaseItemCartCommand.RaiseCanExecuteChanged();
                    ViewModel.DecreaseItemCartCommand.RaiseCanExecuteChanged();
                    ViewModel.ViewItemDetailCommand.RaiseCanExecuteChanged();
                    ViewModel.IncreaseItemCartAndPlaceOrderCommand.RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        /// Select payment type command.
        /// </summary>
        public DelegateCommandAsync SelectGroupTypeCommand
        {
            get
            {
                if (selectGroupTypeCommand == null)
                {
                    selectGroupTypeCommand = new DelegateCommandAsync(
                        async () =>
                        {
                            List<string> groupOptionsOptions = new List<string>();

                            groupOptionsOptions.Add(ViewModel.LocalizationResources.StoreListGroupAllValue.LetterCasingSentence());
                            groupOptionsOptions.Add(ViewModel.LocalizationResources.StoreListGroupNewValue.LetterCasingSentence());
                            groupOptionsOptions.Add(ViewModel.LocalizationResources.StoreListGroupFeaturedValue.LetterCasingSentence());

                            int selectedGroupType = await Parcero.Core.App.UserInteraction.DisplayActionSheetIndex(ViewModel.LocalizationResources.ListGroupLabel, ViewModel.LocalizationResources.CancelButton, null, groupOptionsOptions);

                            if ((selectedGroupType > -1))
                            {
                                ViewModel.GroupType = selectedGroupType;
                            }
                        },
                        () =>
                        {
                            return SelectedItemViewModel == null;
                        },
                        () => SelectGroupTypeCommand,
                        this,
                        this.ViewModel.HandlerCommandException);
                }

                return selectGroupTypeCommand;
            }
        }

        /// <summary>
        /// Share app.
        /// </summary>
        public DelegateCommandAsync ShareContentCommand
        {
            get
            {
                if (shareContentCommand == null)
                {
                    shareContentCommand = new DelegateCommandAsync(
                        async () =>
                        {
                            await Task.FromResult(0);

                            if (IsLargeDetailVisible && (DetailViewModel != null) && (DetailViewModel.CurrentItemViewModel != null))
                            {
                                string contentUrl = DetailViewModel.CurrentItemViewModel.Item.GetShareUrl();
                                ViewModel.ShareTask.ShareLink(ViewModel.LocalizationResources.SocialShareStoreItemMessage, ViewModel.LocalizationResources.SocialShareStoreItemTitle, contentUrl);
                            }
                            else if (IsGroupDetailVisible && (DetailViewModel != null) && (DetailViewModel.CurrentLoadedGroup != null))
                            {
                                string contentUrl = DetailViewModel.CurrentLoadedGroup.Item.GetShareUrl();
                                ViewModel.ShareTask.ShareLink(ViewModel.LocalizationResources.SocialShareStoreItemMessage, ViewModel.LocalizationResources.SocialShareStoreItemTitle, contentUrl);
                            }
                            else if (SelectedItemViewModel != null)
                            {
                                string contentUrl = SelectedItemViewModel.Item.GetShareUrl();
                                ViewModel.ShareTask.ShareLink(ViewModel.LocalizationResources.SocialShareStoreItemMessage, ViewModel.LocalizationResources.SocialShareStoreItemTitle, contentUrl);
                            }
                            else
                            {
                                ViewModel.ShareContentCommand.ExecuteIfCan();
                            }
                        },
                        () =>
                        {
                            return !IsCartVisible;
                        },
                        () => ShareContentCommand,
                        this,
                        ViewModel.HandlerCommandException);
                }

                return shareContentCommand;
            }
        }

        /// <summary>
        /// Show store.
        /// </summary>
        public DelegateCommandAsync ShowCartCommand
        {
            get
            {
                if (showCartCommand == null)
                {
                    showCartCommand = new DelegateCommandAsync(
                        async () =>
                        {
                            await Task.FromResult(0);

                            if (!IsCartVisible)
                            {
                                IsCartVisible = true;

                                if ((CartView != null) && CartView.ShowOverlayCommand.CanExecute())
                                {
                                    await CartView.ShowOverlayCommand.ExecuteAsync();
                                }
                            }
                        },
                        () =>
                        {
                            return !IsCartVisible;
                        },
                        () => ShowCartCommand,
                        this,
                        ViewModel.HandlerCommandException);
                }

                return showCartCommand;
            }
        }

        /// <summary>
        /// Show the items options.
        /// </summary>
        public DelegateCommandAsync<object> ShowItemLargeDetailCommand
        {
            get
            {
                if (showItemLargeDetailCommand == null)
                {
                    showItemLargeDetailCommand = new DelegateCommandAsync<object>(
                        async (selectedElement) =>
                        {
                            await Task.FromResult(0);

                            StoreItemViewModel selectedViewModel = selectedElement as StoreItemViewModel;

                            if (selectedViewModel != null)
                            {
                                if (selectedViewModel.Item.IsGroupParent)
                                {
                                    if (IsLargeDetailVisible && HideItemLargeDetailCommand.CanExecute())
                                    {
                                        await HideItemLargeDetailCommand.ExecuteAsync();
                                    }

                                    if (HideItemOptionsCommand.CanExecute())
                                    {
                                        await HideItemOptionsCommand.ExecuteAsync();
                                    }

                                    IsGroupDetailVisible = true;

                                    if (GroupItemsView != null)
                                    {
                                        if (GroupItemsView.BindingContext != DetailViewModel)
                                        {
                                            GroupItemsView.BindingContext = DetailViewModel;
                                        }

                                        GroupItemsView.InitializeView();
                                        GroupItemsView.PrepareBindings();
                                        GroupItemsView.SetupViewValues(true);

                                        if (GroupItemsView.HideOverlayCommand.CanExecute())
                                        {
                                            await GroupItemsView.HideOverlayCommand.ExecuteAsync();
                                        }

                                        DetailViewModel.CurrentLoadedGroup = selectedViewModel;

                                        if (DetailViewModel.LoadGroupedItemsCommand.CanExecute(selectedViewModel))
                                        {
                                            DetailViewModel.LoadGroupedItemsCommand.Execute(selectedViewModel);
                                        }

                                        if (GroupItemsView.ShowOverlayCommand.CanExecute())
                                        {
                                            await GroupItemsView.ShowOverlayCommand.ExecuteAsync();
                                        }
                                        else
                                        {
                                            GroupItemsView.UpdateProductInfo();
                                        }
                                    }
                                }
                                else
                                {
                                    DetailViewModel.CurrentItemViewModel = selectedViewModel;

                                    if (DetailLargeView.BindingContext != DetailViewModel)
                                    {
                                        DetailLargeView.BindingContext = DetailViewModel;
                                    }

                                    if (IsLargeDetailVisible)
                                    {
                                        DetailLargeView.UpdateProductDetail();
                                    }
                                    else
                                    {
                                        if (DetailLargeView.ShowOverlayCommand.CanExecute())
                                        {
                                            await DetailLargeView.ShowOverlayCommand.ExecuteAsync();
                                        }

                                        IsLargeDetailVisible = true;
                                    }
                                }
                            }

                            StoreItemCartViewModel selectedCartItem = selectedElement as StoreItemCartViewModel;

                            if (selectedCartItem != null)
                            {
                                selectedCartItem.ItemViewModel.LoadCartCount(CancellationToken.None);

                                if ((selectedCartItem.ItemViewModel.ItemPrice == null) && (selectedCartItem.ItemPriceNotifyTask != null))
                                {
                                    AC.ThreadManager.ScheduleManaged(
                                        async () =>
                                        {
                                            await selectedCartItem.ItemPriceNotifyTask.Task;

                                            selectedCartItem.ItemViewModel.ItemPrice = selectedCartItem.ItemPriceNotifyTask.Result;
                                        });
                                }

                                if (IsLargeDetailVisible)
                                {
                                    CacheItemDetailCart = DetailViewModel.CurrentItemViewModel;
                                }
                                else
                                {
                                    IsLargeDetailVisible = true;
                                }

                                DetailViewModel.CurrentItemViewModel = selectedCartItem.ItemViewModel;
                                LastSelectedCartItem = selectedCartItem.ItemViewModel;

                                if (DetailLargeView.BindingContext != DetailViewModel)
                                {
                                    DetailLargeView.BindingContext = DetailViewModel;
                                }

                                AC.ThreadManager.ScheduleManaged(
                                    () =>
                                    {
                                        DetailLargeView.UpdateProductDetail();
                                    });

                                if (DetailLargeView.ShowOverlayCommand.CanExecute())
                                {
                                    await DetailLargeView.ShowOverlayCommand.ExecuteAsync();
                                }

                                if (HideCartCommand.CanExecute())
                                {
                                    await HideCartCommand.ExecuteAsync();
                                }
                            }
                        },
                        (selectedElement) =>
                        {
                            return (selectedElement != null) && ((selectedElement is StoreItemViewModel) || (selectedElement is StoreItemCartViewModel));
                        },
                        () => ShowItemLargeDetailCommand,
                        this,
                        ViewModel.HandlerCommandException);
                }

                return showItemLargeDetailCommand;
            }
        }

        /// <summary>
        /// Show the items options.
        /// </summary>
        public DelegateCommandAsync<StoreItemViewModel> ShowItemOptionsCommand
        {
            get
            {
                if (showItemOptionsCommand == null)
                {
                    showItemOptionsCommand = new DelegateCommandAsync<StoreItemViewModel>(
                        async (selectedElement) =>
                        {
                            await Task.FromResult(0);

                            if (selectedElement.Item.IsGroupParent)
                            {
                                IsGroupDetailVisible = true;

                                if (GroupItemsView != null)
                                {
                                    if (GroupItemsView.ViewModel != DetailViewModel)
                                    {
                                        GroupItemsView.ViewModel = DetailViewModel;
                                    }

                                    GroupItemsView.InitializeView();
                                    GroupItemsView.PrepareBindings();
                                    GroupItemsView.SetupViewValues(true);

                                    if (GroupItemsView.HideOverlayCommand.CanExecute())
                                    {
                                        await GroupItemsView.HideOverlayCommand.ExecuteAsync();
                                    }

                                    DetailViewModel.CurrentLoadedGroup = selectedElement;

                                    if (DetailViewModel.LoadGroupedItemsCommand.CanExecute(selectedElement))
                                    {
                                        DetailViewModel.LoadGroupedItemsCommand.Execute(selectedElement);
                                    }

                                    if (GroupItemsView.ShowOverlayCommand.CanExecute())
                                    {
                                        await GroupItemsView.ShowOverlayCommand.ExecuteAsync();
                                    }
                                    else
                                    {
                                        GroupItemsView.UpdateProductInfo();
                                    }
                                }
                            }
                            else
                            {
                                SelectedItemViewModel = selectedElement;

                                if (ItemDetailView != null)
                                {
                                    ItemDetailView.SelectedItemViewModel = selectedElement;
                                    ItemDetailView.ShowOverlayCommand.ExecuteIfCan();
                                }
                            }
                        },
                        (selectedElement) =>
                        {
                            return selectedElement != null && !selectedElement.Equals(SelectedItemViewModel);
                        },
                        () => ShowItemOptionsCommand,
                        this,
                        ViewModel.HandlerCommandException);
                }

                return showItemOptionsCommand;
            }
        }

        /// <summary>
        /// Show search.
        /// </summary>
        public override DelegateCommandAsync ShowSearchCommand
        {
            get
            {
                if (showSearchCommand == null)
                {
                    showSearchCommand = new DelegateCommandAsync(
                        async () =>
                        {
                            await Task.FromResult(0);

                            if (IsCartVisible)
                            {
                                HideCartCommand.ExecuteIfCan();
                            }

                            if (IsLargeDetailVisible)
                            {
                                HideItemLargeDetailCommand.ExecuteIfCan();
                            }

                            if (IsGroupDetailVisible)
                            {
                                HideItemGroupDetailCommand.ExecuteIfCan();
                            }

                            if (SelectedItemViewModel != null)
                            {
                                HideItemOptionsCommand.ExecuteIfCan();
                            }

                            if (ViewModel.IsProductListMode || ViewModel.IsFullSearchMode)
                            {
                                base.ShowSearchCommand.ExecuteIfCan();
                            }
                            else
                            {
                                ViewModel.IsFullSearchMode = string.IsNullOrWhiteSpace(ViewModel.Level.Department);

                                SwitchToProductionListModeCommand.ExecuteIfCan();
                                base.ShowSearchCommand.ExecuteIfCan();
                            }
                        },
                        () =>
                        {
                            return !IsSearchVisible;
                        },
                        () => ShowSearchCommand,
                        this,
                        ViewModel.HandlerCommandException);
                }

                return showSearchCommand;
            }
        }

        /// <summary>
        /// Store list viewmodel.
        /// </summary>
        public IStoreListViewModel StoreListViewModel
        {
            get
            {
                return (IStoreListViewModel)ViewModel;
            }
        }

        /// <summary>
        /// Command to change the view to category mode.
        /// </summary>
        public DelegateCommandAsync<bool?> SwitchToCategoryModeCommand
        {
            get
            {
                if (switchToCategoryModeCommand == null)
                {
                    switchToCategoryModeCommand = new DelegateCommandAsync<bool?>(
                        async (shouldRefresh) =>
                        {
                            if (ViewModel.IsProductListMode)
                            {
                                if (HideCartCommand.CanExecute())
                                {
                                    await HideCartCommand.ExecuteAsync();
                                }

                                if (HideItemLargeDetailCommand.CanExecute())
                                {
                                    await HideItemLargeDetailCommand.ExecuteAsync();
                                }

                                if (HideItemGroupDetailCommand.CanExecute())
                                {
                                    await HideItemGroupDetailCommand.ExecuteAsync();
                                }

                                if (HideItemOptionsCommand.CanExecute())
                                {
                                    await HideItemOptionsCommand.ExecuteAsync();
                                }

                                if (HideSearchCommand.CanExecute())
                                {
                                    await HideSearchCommand.ExecuteAsync();
                                }

                                ViewModel.IsFullSearchMode = false;
                                ViewModel.IsProductListMode = false;

                                if (CategoryView != null)
                                {
                                    CategoryView.InitializeView();
                                    CategoryView.PrepareBindings();

                                    if (CategoryView.Opacity > 0)
                                    {
                                        CategoryView.Opacity = 0;
                                    }

                                    if (!CategoryView.IsVisible)
                                    {
                                        CategoryView.IsVisible = true;
                                    }

                                    await CategoryView.FadeTo(1);
                                }

                                if (InnerContentView != null)
                                {
                                    await InnerContentView.FadeTo(0);
                                }

                                if (InnerContentView != null)
                                {
                                    InnerContentView.IsVisible = false;
                                }

                                SwitchToProductionListModeCommand.RaiseCanExecuteChanged();

                                if (shouldRefresh.HasValue && shouldRefresh.Value)
                                {
                                    ViewModel.RefreshCommand.ExecuteIfCan();
                                }
                                else
                                {
                                    if (ViewModel.LoadSublevelsCommand.CanExecute())
                                    {
                                        await ViewModel.LoadSublevelsCommand.ExecuteAsync();
                                    }

                                    if (ViewModel.LoadFeaturedItemsCommand.CanExecute())
                                    {
                                        await ViewModel.LoadFeaturedItemsCommand.ExecuteAsync();
                                    }

                                    if (ViewModel.LoadItemsCommand.CanExecute())
                                    {
                                        await ViewModel.LoadItemsCommand.ExecuteAsync();
                                    }
                                }

                                UpdateBackgroundOpactity();

                                Parcero.Core.App.ThreadManager.ScheduleManaged(
                                TimeSpan.FromSeconds(0.1),
                                () =>
                                {
                                    RelativeBackCommand.RaiseCanExecuteChanged();
                                });
                            }
                            else
                            {
                                RelativeBackCommand.ExecuteIfCan();
                            }
                        },
                        (shouldRefresh) =>
                        {
                            var subLevel = ViewModel.Level;
                            bool calculatedProductMode = (subLevel != null) && !string.IsNullOrWhiteSpace(subLevel.Department) && !string.IsNullOrWhiteSpace(subLevel.Category) && !string.IsNullOrWhiteSpace(subLevel.Subcategory);
                            return (ViewModel.IsProductListMode || IsCartVisible || IsLargeDetailVisible || IsGroupDetailVisible) && !calculatedProductMode;
                        },
                        () => SwitchToCategoryModeCommand,
                        this,
                        ViewModel.HandlerCommandException);
                }

                return switchToCategoryModeCommand;
            }
        }

        /// <summary>
        /// Command to change the view to production list mode.
        /// </summary>
        public DelegateCommandAsync<bool?> SwitchToProductionListModeCommand
        {
            get
            {
                if (switchToProductionListModeCommand == null)
                {
                    switchToProductionListModeCommand = new DelegateCommandAsync<bool?>(
                        async (shouldRefresh) =>
                        {
                            ViewModel.IsProductListMode = true;

                            if (InnerContentView != null)
                            {
                                if (InnerContentView.Opacity > 0)
                                {
                                    InnerContentView.Opacity = 0;
                                }

                                if (!InnerContentView.IsVisible)
                                {
                                    InnerContentView.IsVisible = true;
                                }

                                await InnerContentView.FadeTo(1);
                            }

                            if (CategoryView != null)
                            {
                                await CategoryView.FadeTo(0);
                            }

                            if (CategoryView != null)
                            {
                                CategoryView.IsVisible = false;
                            }

                            SwitchToCategoryModeCommand.RaiseCanExecuteChanged();

                            if (shouldRefresh.HasValue && shouldRefresh.Value)
                            {
                                ViewModel.RefreshCommand.ExecuteIfCan();
                            }
                            else
                            {
                                if (ViewModel.LoadSublevelsCommand.CanExecute())
                                {
                                    await ViewModel.LoadSublevelsCommand.ExecuteAsync();
                                }

                                if (ViewModel.LoadFeaturedItemsCommand.CanExecute())
                                {
                                    await ViewModel.LoadFeaturedItemsCommand.ExecuteAsync();
                                }

                                if (ViewModel.LoadItemsCommand.CanExecute())
                                {
                                    await ViewModel.LoadItemsCommand.ExecuteAsync();
                                }
                            }

                            UpdateBackgroundOpactity();

                            Parcero.Core.App.ThreadManager.ScheduleManaged(
                                TimeSpan.FromSeconds(0.1),
                                () =>
                                {
                                    RelativeBackCommand.RaiseCanExecuteChanged();
                                });
                        },
                        (shouldRefresh) =>
                        {
                            return !ViewModel.IsProductListMode;
                        },
                        () => SwitchToProductionListModeCommand,
                        this,
                        ViewModel.HandlerCommandException);
                }

                return switchToProductionListModeCommand;
            }
        }

        /// <summary>
        /// Update the view to reflect a specifig navigation step.
        /// </summary>
        public DelegateCommandAsync<NavigationStep> UpdateViewToStepCommand
        {
            get
            {
                updateViewToStepCommand = new DelegateCommandAsync<NavigationStep>(
                        async (newStep) =>
                        {
                            bool updateData = true;

                            ViewModel.Level = newStep.Level;
                            ViewModel.FilterTerm = newStep.FilterTerm;
                            ViewModel.IsFullSearchMode = newStep.IsFullSearchMode;

                            if (HideCartCommand.CanExecute())
                            {
                                await HideCartCommand.ExecuteAsync();
                            }

                            if (HideItemLargeDetailCommand.CanExecute())
                            {
                                await HideItemLargeDetailCommand.ExecuteAsync();
                            }

                            if (HideItemGroupDetailCommand.CanExecute())
                            {
                                await HideItemGroupDetailCommand.ExecuteAsync();
                            }

                            if (HideItemOptionsCommand.CanExecute())
                            {
                                await HideItemOptionsCommand.ExecuteAsync();
                            }

                            if (ViewModel.IsProductListMode != newStep.IsProductListMode)
                            {
                                if (newStep.IsProductListMode)
                                {
                                    if (SwitchToProductionListModeCommand.CanExecute(false))
                                    {
                                        updateData = false;
                                        await SwitchToProductionListModeCommand.ExecuteAsync(false);
                                    }
                                }
                                else
                                {
                                    if (SwitchToCategoryModeCommand.CanExecute(false))
                                    {
                                        updateData = false;
                                        await SwitchToCategoryModeCommand.ExecuteAsync(false);
                                    }
                                }
                            }

                            if (newStep.IsSearchMode && !IsSearchVisible)
                            {
                                if (ShowSearchCommand.CanExecute())
                                {
                                    updateData = false;
                                    await ShowSearchCommand.ExecuteAsync();
                                }
                            }

                            if (newStep.SelectedItemViewModel != null)
                            {
                                if (ShowItemOptionsCommand.CanExecute(newStep.SelectedItemViewModel))
                                {
                                    await ShowItemOptionsCommand.ExecuteAsync(newStep.SelectedItemViewModel);
                                }
                            }

                            if (newStep.IsGroupDetailVisible)
                            {
                                if (ShowItemOptionsCommand.CanExecute(newStep.CurrentLoadedGroup))
                                {
                                    await ShowItemOptionsCommand.ExecuteAsync(newStep.CurrentLoadedGroup);
                                }
                            }

                            if (newStep.IsLargeDetailVisible)
                            {
                                if (ShowItemLargeDetailCommand.CanExecute(newStep.SelectedDetailItemViewModel))
                                {
                                    await ShowItemLargeDetailCommand.ExecuteAsync(newStep.SelectedDetailItemViewModel);
                                }
                            }

                            if (newStep.IsCartVisible)
                            {
                                if (ShowCartCommand.CanExecute())
                                {
                                    await ShowCartCommand.ExecuteAsync();
                                }
                            }

                            if (updateData)
                            {
                                if (ViewModel.NavigationCancellationToken != null && !ViewModel.NavigationCancellationToken.IsCancellationRequested)
                                {
                                    ViewModel.NavigationCancellationToken.Cancel();

                                    ViewModel.NavigationCancellationToken = new CancellationTokenSource();
                                }

                                if (ViewModel.LoadSublevelsCommand.CanExecute())
                                {
                                    await ViewModel.LoadSublevelsCommand.ExecuteAsync();
                                }

                                if (ViewModel.LoadFeaturedItemsCommand.CanExecute())
                                {
                                    await ViewModel.LoadFeaturedItemsCommand.ExecuteAsync();
                                }

                                if (ViewModel.LoadItemsCommand.CanExecute())
                                {
                                    await ViewModel.LoadItemsCommand.ExecuteAsync();
                                }
                            }

                            RelativeBackCommand.RaiseCanExecuteChanged();
                        },
                        (newStep) =>
                        {
                            return newStep != null;
                        },
                        () => UpdateViewToStepCommand,
                        this,
                        ViewModel.HandlerCommandException);

                return updateViewToStepCommand;
            }
        }

        /// <summary>
        /// Show the items options.
        /// </summary>
        public DelegateCommandAsync<StoreItemViewModel> UseSuggestionCommand
        {
            get
            {
                if (useSuggestionCommand == null)
                {
                    useSuggestionCommand = new DelegateCommandAsync<StoreItemViewModel>(
                        async (selectedElement) =>
                        {
                            await Task.FromResult(0);

                            if ((selectedElement != null) && (selectedElement.Item != null))
                            {
                                ExecuteSearchCommand.ExecuteIfCan(selectedElement.Item.Name);
                            }
                        },
                        (selectedElement) =>
                        {
                            return (selectedElement != null);
                        },
                        () => UseSuggestionCommand,
                        this,
                        ViewModel.HandlerCommandException);
                }

                return useSuggestionCommand;
            }
        }

        /// <summary>
        /// Add extra layers.
        /// </summary>
        protected override bool AutoAddExtraLayers
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// View model already selected before opening detail from cart.
        /// </summary>
        protected StoreItemViewModel CacheItemDetailCart { get; set; }

        /// <summary>
        /// Cart view.
        /// </summary>
        protected CartLargeView CartView { get; set; }

        /// <summary>
        /// Category view.
        /// </summary>
        protected StoreListCategoryView CategoryView { get; set; }

        /// <summary>
        /// Store detail large view.
        /// </summary>
        protected StoreDetailLargeSimpleView DetailLargeView { get; set; }

        /// <summary>
        /// Scroll for the featured.
        /// </summary>
        protected ScrollView FeaturedScrollView { get; set; }

        /// <summary>
        /// Store detail large view.
        /// </summary>
        protected StoreDetailGroupView GroupItemsView { get; set; }

        /// <summary>
        /// Flag if has auto scroll.
        /// </summary>
        protected virtual bool HasAutoScroll
        {
            get
            {
                return Device.OS.OnPlatform(true, true, true, true, true);
            }
        }

        /// <summary>
        /// Has filter bar.
        /// </summary>
        protected override bool HasFilterBar
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Has a background that needs render.
        /// </summary>
        protected override bool HasRenderBackground
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Item detail view.
        /// </summary>
        protected StoreDetailSmallSimpleView ItemDetailView { get; set; }

        /// <summary>
        /// Level group value.
        /// </summary>
        protected TextContentViewButton LabelGroupValue { get; set; }

        /// <summary>
        /// Selected view model for large detail from cart.
        /// </summary>
        protected StoreItemViewModel LastSelectedCartItem { get; set; }

        /// <summary>
        /// View for the recent search view items.
        /// </summary>
        protected RecentSearchView ListRecentSearchView { get; set; }

        /// <summary>
        /// Suggestions label.
        /// </summary>
        protected ExtendedLabel RecentSearchLabel { get; set; }

        /// <summary>
        /// Suggestions background.
        /// </summary>
        protected BoxView SuggestionsBackground { get; set; }

        /// <summary>
        /// Suggestions label.
        /// </summary>
        protected ExtendedLabel SuggestionsLabel { get; set; }

        /// <summary>
        /// Repeater for the suggestions.
        /// </summary>
        protected RepeaterRecycleView SuggestionsRepeater { get; set; }

        /// <summary>
        /// Use single view for the sublevels.
        /// </summary>
        protected virtual bool UseSingleViewSublevels
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Render a chat line.
        /// </summary>
        /// <typeparam name="TViewModel">View model to use.</typeparam>
        /// <param name="stackContainer">Stack container to use.</param>
        /// <param name="expressionLabelValue">Expression for the label.</param>
        /// <param name="expressionImagePath">Expression for the image.</param>
        /// <param name="expressionDateValue">Expression for the date.</param>
        /// <param name="dateValue">Date value to use.</param>
        /// <param name="dateValueConverter">Value converter to use for the date.</param>
        public static void RenderChatLine<TViewModel>(
            StackLayout stackContainer,
            View chatView,
            View imageView = null,
            Expression<Func<TViewModel, object>> expressionDateValue = null,
            Expression<Func<TViewModel, object>> expressionIsVisible = null,
            object dateValue = null,
            IValueConverter dateValueConverter = null)
        {
            Color boubleBackground = Theme.CommonResources.PagesBackgroundColorLight;

            if (dateValueConverter == null)
            {
                dateValueConverter = Theme.CommonResources.DateTimeUtcToRelativeStringConverter;
            }

            if (stackContainer != null)
            {
                StackLayout stackChatLine = new StackLayout()
                {
                    Style = Theme.ApplicationStyles.SimpleStackContainerStyle,
                    Orientation = StackOrientation.Horizontal
                };

                if (expressionIsVisible != null)
                {
                    stackChatLine.SetBinding<TViewModel>(StackLayout.IsVisibleProperty, expressionIsVisible);
                }

                StackLayout stackImage = new StackLayout()
                {
                    Style = Theme.ApplicationStyles.SimpleStackContainerStyle,
                    WidthRequest = Theme.CommonResources.UserImageWidth * 1.5f,
                    Orientation = StackOrientation.Horizontal
                };

                if (imageView != null)
                {
                    imageView.HorizontalOptions = LayoutOptions.Center;
                    imageView.VerticalOptions = LayoutOptions.Center;
                    stackImage.Children.Add(imageView);
                }

                stackChatLine.Children.Add(stackImage);

                RenderUtil.RenderSpace(stackChatLine);

                StackLayout stackChatContent = new StackLayout()
                {
                    Style = Theme.ApplicationStyles.SimpleStackContainerStyle,
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    BackgroundColor = boubleBackground
                };

                RenderUtil.RenderSpace(stackChatContent, 10, 10);

                stackChatContent.Children.Add(chatView);

                RenderUtil.RenderSpace(stackChatContent, 10, 10);

                ExtendedLabel textChatDate = new ExtendedLabel()
                {
                    Style = Theme.ApplicationStyles.DefaultExtendedLabelStyle,
                    HorizontalOptions = LayoutOptions.Start,
                    FontSize = Theme.CommonResources.TextSizeMicro,
                    VerticalOptions = LayoutOptions.Fill,
                    VerticalTextAlignment = TextAlignment.End,
                    LineBreakMode = LineBreakMode.NoWrap,
                    TextColor = Theme.CommonResources.Accent
                };

                if (dateValue == null)
                {
                    if (expressionDateValue != null)
                    {
                        textChatDate.SetBinding<TViewModel>(ExtendedLabel.TextProperty, expressionDateValue, converter: dateValueConverter);
                    }
                }
                else
                {
                    if (dateValueConverter == null)
                    {
                        textChatDate.Text = dateValue.ToString();
                    }
                    else
                    {
                        textChatDate.Text = dateValueConverter.Convert(dateValue, typeof(string), null, AC.Locale.Culture) as string;
                    }
                }

                stackChatContent.Children.Add(textChatDate);

                RenderUtil.RenderSpace(stackChatContent);

                stackChatLine.Children.Add(stackChatContent);

                RenderUtil.RenderSpace(stackChatLine);

                stackContainer.Children.Add(stackChatLine);
            }
        }

        /// <summary>
        /// Render a chat line.
        /// </summary>
        /// <typeparam name="TViewModel">View model to use.</typeparam>
        /// <param name="stackContainer">Stack container to use.</param>
        /// <param name="expressionLabelValue">Expression for the label.</param>
        /// <param name="expressionImagePath">Expression for the image.</param>
        /// <param name="expressionDateValue">Expression for the date.</param>
        /// <param name="dateValue">Date value to use.</param>
        /// <param name="dateValueConverter">Value converter to use for the date.</param>
        public static void RenderChatLine<TViewModel>(
            StackLayout stackContainer,
            Expression<Func<TViewModel, object>> expressionLabelValue = null,
            Expression<Func<TViewModel, object>> expressionImagePath = null,
            Expression<Func<TViewModel, object>> expressionDateValue = null,
            Expression<Func<TViewModel, object>> expressionIsVisible = null,
            object dateValue = null,
            IValueConverter dateValueConverter = null,
            ICommand imageCommand = null)
        {
            if (stackContainer != null)
            {
                View imageView = null;
                View chatView = null;

                if (expressionImagePath != null)
                {
                    double imageWidthPercent = 0.85;
                    double imageButtonMargin = 5;

                    var imageLogoButton = new ImageContentViewButton()
                    {
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        WidthRequest = Theme.CommonResources.UserImageWidth,
                        HeightRequest = Theme.CommonResources.UserImageWidth,
                        ImageHeightRequest = Theme.CommonResources.UserImageWidth * imageWidthPercent,
                        ImageWidthRequest = Theme.CommonResources.UserImageWidth * imageWidthPercent,
                        ShapeType = ShapeType.Circle,
                        ButtonBackgroundColor = Color.White,
                        StrokeWidth = 1,
                        StrokeColor = Theme.CommonResources.AccentDark,
                        MarginBorders = imageButtonMargin,
                        Command = imageCommand
                    };

                    imageLogoButton.SetBinding<TViewModel>(ImageContentViewButton.SourceProperty, expressionImagePath);

                    StackLayout imageViewLayout = new StackLayout()
                    {
                        Style = Theme.ApplicationStyles.FormRowContainerStyle,
                        Orientation = StackOrientation.Horizontal
                    };

                    RenderUtil.RenderSpace(imageViewLayout, 5, 7);
                    imageViewLayout.Children.Add(imageLogoButton);
                    RenderUtil.RenderSpace(imageViewLayout);

                    imageView = imageViewLayout;
                }

                ExtendedLabel textChat = new ExtendedLabel()
                {
                    Style = Theme.ApplicationStyles.DefaultExtendedLabelStyle,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    VerticalTextAlignment = TextAlignment.Center,
                    FontSize = Theme.CommonResources.TextSizeSmall,
                    LineBreakMode = LineBreakMode.WordWrap
                };

                textChat.SetBinding<TViewModel>(ExtendedLabel.TextProperty, expressionLabelValue);

                chatView = textChat;

                RenderChatLine<TViewModel>(stackContainer, chatView, imageView, expressionDateValue: expressionDateValue, expressionIsVisible: expressionIsVisible, dateValue: dateValue, dateValueConverter: dateValueConverter);
            }
        }

        /// <summary>
        /// Clean bindings.
        /// </summary>
        public override void CleanBindings()
        {
            base.CleanBindings();

            if ((CartView != null) && (CartView.BindingContext != null))
            {
                CartView.BindingContext = null;
            }

            if ((DetailLargeView != null) && (DetailLargeView.BindingContext != null))
            {
                DetailLargeView.BindingContext = null;
            }

            if ((ItemDetailView != null) && (ItemDetailView.BindingContext != null))
            {
                ItemDetailView.BindingContext = null;
            }

            if ((GroupItemsView != null) && (GroupItemsView.BindingContext != null))
            {
                GroupItemsView.BindingContext = null;
            }
        }

        /// <summary>
        /// Render background filter.
        /// </summary>
        /// <returns>View to use.</returns>
        protected static View InstanceBackgroundDetail(Action actionTapped)
        {
            BoxView backgroundBox = new BoxView()
            {
                Color = Theme.CommonResources.BackgroundColorTranslucent,
                IsVisible = false,
                Opacity = 0
            };

            TapGestureRecognizer tapBackground = new TapGestureRecognizer();

            tapBackground.Tapped += (object sender, System.EventArgs e) =>
            {
                if (actionTapped != null)
                {
                    actionTapped();
                }
            };

            backgroundBox.GestureRecognizers.Add(tapBackground);

            return backgroundBox;
        }

        /// <summary>
        /// Add the add toolbar item.
        /// </summary>
        protected override void AddAddToolbarItem()
        {
            // No add button.
        }

        /// <summary>
        /// Add cart large view.
        /// </summary>
        protected void AddCartLargeView()
        {
            if (CartView == null && PageCanvas != null)
            {
                CartView = new CartLargeView()
                {
                    ContentMargin = ContentMargin,
                    AddMoreItemsCommand = HideCartCommand,
                    BottomAppBarMargin = BottomAppBarMargin,
                    ShowItemLargeDetailExternalCommand = ShowItemLargeDetailCommand
                };

                PageCanvas.Children.Add(CartView);
            }
        }

        /// <summary>
        /// Add cart view.
        /// </summary>
        protected void AddCartSmallView()
        {
            if (!isCartViewSmallAdded && !IsRecylced)
            {
                isCartViewSmallAdded = true;

                var cartIndicator = new CartSmallView(ShowCartCommand)
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                };

                if (StackShortCuts != null)
                {
                    StackShortCuts.Children.Add(cartIndicator);
                }
            }
        }

        /// <summary>
        /// Add custom toolbar items.
        /// </summary>
        protected override void AddCustomToolBarItems()
        {
            base.AddCustomToolBarItems();

            if (StackShortCuts != null)
            {
                var filterButton = new GlyphOnlyContentViewButton()
                {
                    Style = Theme.ApplicationStyles.GlyphOnlyNavbarButtonStyle,
                    GlyphText = Theme.CommonResources.GlyphTextFilter,
                    Command = SelectGroupTypeCommand
                };

                filterButton.SetBinding<StoreListViewModel>(ContentViewButton.IsVisibleContentProperty, vm => vm.IsProductListMode);

                StackShortCuts.Children.Insert(0, filterButton);
            }
        }

        /// <summary>
        /// Add extra layers.
        /// </summary>
        protected override void AddExtraLayers()
        {
            AddCartSmallView();
            AddStoreDetailGroupView();
            AddStoreDetailSmallView();
            AddStoreDetailLargeView();
            AddCartLargeView();

            // Base layer is the buttons.
            base.AddExtraLayers();
        }

        /// <summary>
        /// Add extra layers to the filter.
        /// </summary>
        protected override List<View> AddFilterExtraLayers()
        {
            List<View> list = null;

            list = new List<View>();

            Style titleStyle = new Xamarin.Forms.Style(typeof(ExtendedLabel));
            titleStyle.BasedOn = Theme.ApplicationStyles.DefaultExtendedLabelStyle;
            titleStyle.Setters.Add(ExtendedLabel.FontNameProperty, Theme.CommonResources.FontRobotBoldCondensedName);
            titleStyle.Setters.Add(ExtendedLabel.FriendlyFontNameProperty, Theme.CommonResources.FontRobotBoldCondensedFriendlyName);
            titleStyle.Setters.Add(ExtendedLabel.FontSizeProperty, Theme.CommonResources.TextSizeSmall);
            titleStyle.Setters.Add(ExtendedLabel.FontAttributesProperty, FontAttributes.Bold);

            // Suggestion background.
            SuggestionsBackground = new BoxView()
            {
                Color = Theme.CommonResources.PagesBackgroundColor,
                Opacity = 0
            };

            list.Add(SuggestionsBackground);

            SuggestionsLabel = new ExtendedLabel()
            {
                Style = titleStyle,
                Text = ViewModel.LocalizationResources.SuggestionsLabel,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                Opacity = 0
            };

            list.Add(SuggestionsLabel);

            double borderWith = 4;
            double itemHeight = 100 + (borderWith * 2f);
            double itemWidth = 200 + (borderWith * 2f);

            var itemTemplate = new DataTemplate(
                    () =>
                    {
                        ContentViewButton detailItemButton = new ContentTemplateViewButton(
                           new DataTemplate(
                               () =>
                               {
                                   var cell = new StoreItemThumbVerticalBarView();
                                   cell.BindingContext = null;
                                   cell.PrepareBindings();
                                   return cell;
                               }))
                        {
                            Command = UseSuggestionCommand,
                            Style = Theme.ApplicationStyles.TextOnlyContentButtonStyle
                        };

                        detailItemButton.BindingContext = null;

                        detailItemButton.SetBinding(ContentViewButton.CommandParameterProperty, ".");

                        return detailItemButton;
                    });

            SuggestionsRepeater = new RepeaterRecycleView(showActivityIndicator: true)
            {
                ItemHeight = itemHeight,
                ItemWidth = itemWidth,
                ItemTemplate = itemTemplate,
                Spacing = 5,
                Opacity = 0,
                IsVisible = false
            };

            SuggestionsRepeater.SetBinding<StoreListViewModel>(RepeaterRecycleView.ItemsSourceProperty, vm => vm.SuggestedItems);

            list.Add(SuggestionsRepeater);

            RecentSearchLabel = new ExtendedLabel()
            {
                Style = titleStyle,
                Text = ViewModel.LocalizationResources.RecentSearchesLabel,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                Opacity = 0
            };

            list.Add(RecentSearchLabel);

            ListRecentSearchView = new RecentSearchView()
            {
                SearchItemCommand = ExecuteSearchCommand,
                BindingContext = ViewModel,
                IsVisible = false,
                Opacity = 0
            };

            list.Add(ListRecentSearchView);

            return list;
        }

        /// <summary>
        /// Add the refresh button.
        /// </summary>
        protected override void AddRefreshToolbarItem()
        {
            if (StackShortCuts != null)
            {
                var refreshButton = new GlyphOnlyContentViewButton()
                {
                    Style = Theme.ApplicationStyles.GlyphOnlyNavbarButtonStyle,
                    GlyphText = Theme.CommonResources.GlyphTextRefresh,
                    Command = RefreshCommand,
                    BindingContext = this
                };

                StackShortCuts.Children.Insert(0, refreshButton);
            }
            else
            {
                if (!IsRecylced)
                {
                    var tbi = new ToolbarItem()
                    {
                        Text = ViewModel.LocalizationResources.RefreshButton,
                        Icon = Theme.CommonResources.PathImageRefreshAction,
                        Command = RefreshCommand,
                        Order = ToolbarItemOrder.Secondary
                    };

                    ToolbarItems.Add(tbi);
                }
            }
        }

        /// <summary>
        /// Add the refresh button.
        /// </summary>
        protected override void AddShareToolbarItem()
        {
            if (StackShortCuts != null)
            {
                var shareButton = new GlyphOnlyContentViewButton()
                {
                    Style = Theme.ApplicationStyles.GlyphOnlyNavbarButtonStyle,
                    GlyphText = Theme.CommonResources.GlyphTextShare,
                    GlyphFriendlyFontName = Theme.CommonResources.GlyphFriendlyFontNameAlternate,
                    GlyphFontName = Theme.CommonResources.GlyphFontNameAlternate,
                    Command = ShareContentCommand,
                    BindingContext = this
                };

                StackShortCuts.Children.Insert(0, shareButton);
            }
        }

        /// <summary>
        /// Add cart large view.
        /// </summary>
        protected void AddStoreDetailGroupView()
        {
            if (GroupItemsView == null && PageCanvas != null)
            {
                GroupItemsView = new StoreDetailGroupView()
                {
                    ContentMargin = ContentMargin,
                    CloseOverlayCommand = HideItemGroupDetailCommand,
                    BottomAppBarMargin = BottomAppBarMargin,
                    ItemDetailCommand = ShowItemOptionsCommand
                };

                PageCanvas.Children.Add(GroupItemsView);
            }
        }

        /// <summary>
        /// Add cart large view.
        /// </summary>
        protected void AddStoreDetailLargeView()
        {
            if (DetailLargeView == null && PageCanvas != null)
            {
                DetailLargeView = new StoreDetailLargeSimpleView()
                {
                    ContentMargin = ContentMargin,
                    CloseOverlayCommand = HideItemLargeDetailCommand,
                    BottomAppBarMargin = BottomAppBarMargin,
                    NavigateBackCommand = RelativeBackCommand,
                    NavigateToStoreLevelCommand = NavigateToStoreLevelCommand
                };

                PageCanvas.Children.Add(DetailLargeView);
            }
        }

        /// <summary>
        /// Add cart large view.
        /// </summary>
        protected void AddStoreDetailSmallView()
        {
            if (ItemDetailView == null && PageCanvas != null)
            {
                ItemDetailView = new StoreDetailSmallSimpleView()
                {
                    ContentMargin = ContentMargin,
                    CloseOverlayCommand = HideItemOptionsCommand,
                    BottomAppBarMargin = BottomAppBarMargin,
                    ViewItemDetailCommand = ShowItemLargeDetailCommand,
                    StoreListViewModel = StoreListViewModel
                };

                PageCanvas.Children.Add(ItemDetailView);
            }
        }

        /// <summary>
        /// Set the properties for the label when there is no elements in the list.
        /// A binding is recomended.
        /// </summary>
        /// <param name="labelToSet">Label to set.</param>
        protected override void BindLabelNoElements(Xamarin.Forms.Label labelToSet)
        {
            base.BindLabelNoElements(labelToSet);

            labelToSet.SetBinding<StoreListViewModel>(Label.TextProperty, vm => vm.LocalizationResources.SearchEmptyToAllStoreText);
        }

        /// <summary>
        /// Set the binding of the list of elements.
        /// </summary>
        /// <param name="ListElementsAll">List to use.</param>
        protected override void ConfigListElementsAll(ListView listElements)
        {
            listElements.ItemTemplate = new DataTemplate(
                () =>
                {
                    return new SimpleViewViewCell<StoreItemThumbHorizontalSimpleView>();
                });
        }

        /// <summary>
        /// Get if the label no elements should be visible.
        /// </summary>
        /// <returns>Task to await.</returns>
        protected override async Task<bool> GetLabelNoElementsVisibilty()
        {
            bool elementVisible = false;

            if (ViewModel.IsFullSearchMode)
            {
                var baseVisible = await base.GetLabelNoElementsVisibilty();
                elementVisible = baseVisible && !string.IsNullOrEmpty(ViewModel.FilterTerm);
            }
            else if (ViewModel.IsProductListMode)
            {
                elementVisible = await base.GetLabelNoElementsVisibilty();
            }
            else
            {
                elementVisible = ViewModel.SublevelsViewModels.Count == 0;
            }

            return elementVisible;
        }

        /// <summary>
        /// Instance the navigation back button.
        /// </summary>
        /// <returns>Button to use.</returns>
        protected override Parcero.Core.Views.Common.ContentViewButton InstanceNavigateBackButton()
        {
            var button = base.InstanceNavigateBackButton();

            if (button != null)
            {
                button.Command = RelativeBackCommand;
            }

            return button;
        }

        /// <summary>
        /// Instance the title view.
        /// </summary>
        /// <returns></returns>
        protected override View InstanceTitleView()
        {
            GlyphContentViewButton storeContentButton1 = new GlyphContentViewButton(true, true, Anuracode.Support.Model.ImageOrientation.ImageToLeft)
            {
                Style = Theme.ApplicationStyles.TextWithGlyphImportantContentButtonStyle,
                MarginBorders = Device.Idiom == TargetIdiom.Phone ? 0 : 2.5f,
                MarginElements = 0,
                HorizontalOptions = LayoutOptions.Center,
                Command = SwitchToCategoryModeCommand,
                GlyphText = Theme.CommonResources.GlyphTextStore,
                Text = Device.Idiom == TargetIdiom.Phone ? ViewModel.LocalizationResources.StoreLabel : ViewModel.LocalizationResources.SearchGotoStoreLabel,
                TextColor = Theme.CommonResources.PagesBackgroundColorLight,
                GlyphTextColor = Theme.CommonResources.PagesBackgroundColorLight,
                InvisibleWhenDisabled = true,
                HeightRequest = Theme.CommonResources.RoundedButtonWidth
            };

            return storeContentButton1;
        }

        /// <summary>
        /// Layout extra layers.
        /// </summary>
        /// <param name="pageSize">Page size.</param>
        /// <param name="filterPosition">Position of the filter.</param>
        protected override void LayoutFilterExtraLayers(Rectangle pageSize, Rectangle filterPosition)
        {
            base.LayoutFilterExtraLayers(pageSize, filterPosition);

            if (ViewModel.IsFullSearchMode)
            {
                Rectangle suggestionsLabelPosition = new Rectangle();
                Rectangle suggestionsListPosition = new Rectangle();
                Rectangle searchesLabelPosition = new Rectangle();
                Rectangle searchesListPosition = new Rectangle();

                if (SuggestionsLabel != null)
                {
                    var elementSize = SuggestionsLabel.GetSizeRequest(pageSize.Width, pageSize.Height).Request;
                    double elementLeft = ContentMargin;
                    double elementTop = filterPosition.Y + filterPosition.Height + (ContentMargin * 1.5f);
                    double elementWidth = elementSize.Width;
                    double elementHeight = elementSize.Height;

                    suggestionsLabelPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                    SuggestionsLabel.LayoutUpdate(suggestionsLabelPosition);
                }

                if (SuggestionsRepeater != null)
                {
                    double borderWith = 4;
                    double itemHeight = 100 + (borderWith * 2f);

                    double elementLeft = ContentMargin;
                    double elementTop = suggestionsLabelPosition.Y + suggestionsLabelPosition.Height + (ContentMargin * 0f);
                    double elementWidth = pageSize.Width - (ContentMargin * 1f);
                    double elementHeight = itemHeight;

                    suggestionsListPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                    SuggestionsRepeater.LayoutUpdate(suggestionsListPosition);
                }

                if (RecentSearchLabel != null)
                {
                    var elementSize = RecentSearchLabel.GetSizeRequest(pageSize.Width, pageSize.Height).Request;
                    double elementLeft = ContentMargin;
                    double elementTop = suggestionsListPosition.Y + suggestionsListPosition.Height + (ContentMargin * 1f);
                    double elementWidth = elementSize.Width;
                    double elementHeight = elementSize.Height;

                    searchesLabelPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                    RecentSearchLabel.LayoutUpdate(searchesLabelPosition);
                }

                if (ListRecentSearchView != null)
                {
                    double elementLeft = ContentMargin;
                    double elementTop = searchesLabelPosition.Y + searchesLabelPosition.Height + (ContentMargin * 0.5f);
                    double elementWidth = pageSize.Width - (ContentMargin * 2f);
                    double elementHeight = ((pageSize.Height - elementTop) + ContentMargin).Clamp(0, 180f);

                    searchesListPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                    ListRecentSearchView.LayoutUpdate(searchesListPosition);
                }

                if (SuggestionsBackground != null)
                {
                    double elementLeft = 0;
                    double elementTop = filterPosition.Y + filterPosition.Height;
                    double elementWidth = pageSize.Width;
                    double elementHeight = (searchesListPosition.Y + searchesListPosition.Height + ContentMargin) - elementTop;

                    var elementPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                    SuggestionsBackground.LayoutUpdate(elementPosition);
                }
            }
        }

        /// <summary>
        /// Selected item changed.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Arguments of the event.</param>
        protected override void ListElements_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                var selectedItem = e.Item;
                Parcero.Core.App.ThreadManager.ScheduleManaged(
                    () =>
                    {
                        try
                        {
                            if (Device.OS.OnPlatform(true, true, true, true, false))
                            {
                                if (ShowItemOptionsCommand.CanExecute(selectedItem))
                                {
                                    ShowItemOptionsCommand.Execute(selectedItem);
                                }
                            }

                            if ((ListElementsAll != null) && (ListElementsAll.SelectedItem != null))
                            {
                                ListElementsAll.SelectedItem = null;
                            }
                        }
                        catch
                        {
                        }

                        return Task.FromResult(0);
                    });
            }
        }

        /// <summary>
        /// Page appears.
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();

            Parcero.Core.App.ThreadManager.ScheduleManaged(
                async () =>
                {
                    await Task.FromResult(0);

                    if (!App.PagesInitilized)
                    {
                        if (App.InitPageCompleteAction != null)
                        {
                            App.InitPageCompleteAction();
                        }
                    }
                });

            if (HasAutoScroll)
            {
                if (timerSubscription != null)
                {
                    timerSubscription.Dispose();
                    timerSubscription = null;
                }

                timerSubscription = NewThreadScheduler.Default.SchedulePeriodic(
                    autoScrollInterval,
                    () =>
                    {
                        AutoScrollFeaturedItemsCommand.ExecuteIfCan();
                    });

                if (FeaturedScrollView != null)
                {
                    FeaturedScrollView.Scrolled -= FeaturedScrollView_Scrolled;
                    FeaturedScrollView.Scrolled += FeaturedScrollView_Scrolled;
                }
            }

            if ((CartView != null) && (CartView.BindingContext == null))
            {
                CartView.BindingContext = CartView.ViewModel;
            }
        }

        /// <summary>
        /// Event when the back button is pressed.
        /// </summary>
        /// <returns></returns>
        protected override bool OnBackButtonPressed()
        {
            RelativeBackCommand.ExecuteIfCan();

            return true;
        }

        /// <summary>
        /// Page disapperas.
        /// </summary>
        protected override void OnDisappearing()
        {
            if (timerSubscription != null)
            {
                timerSubscription.Dispose();
                timerSubscription = null;
            }

            if (FeaturedScrollView != null)
            {
                FeaturedScrollView.Scrolled -= FeaturedScrollView_Scrolled;
            }

            base.OnDisappearing();
        }

        /// <summary>
        /// Layout the children for the background.
        /// </summary>
        /// <param name="pageSize">Page size.</param>
        /// <param name="headerPosition">Header position.</param>
        /// <param name="contentPosition">Content position.</param>
        /// <param name="footerPosition">Footer position.</param>
        protected override void PageExtraLayersLayoutChildren(Rectangle pageSize, Rectangle headerPosition, Rectangle contentPosition, Rectangle footerPosition)
        {
            base.PageExtraLayersLayoutChildren(pageSize, headerPosition, contentPosition, footerPosition);

            if (CategoryView != null)
            {
                CategoryView.LayoutUpdate(new Rectangle(pageSize.X, LastPositionInnerContentView.Y, pageSize.Width, LastPositionInnerContentView.Height));
            }

            if (ItemDetailView != null)
            {
                ItemDetailView.BottomAppBarMargin = BottomAppBarMargin;
                ItemDetailView.TopLayoutMargin = headerPosition.Y - ContentMargin;
                ItemDetailView.LayoutUpdate(pageSize);
            }

            if (CartView != null)
            {
                CartView.TopLayoutMargin = headerPosition.Y - ContentMargin;

                CartView.LayoutUpdate(pageSize);
            }

            if (DetailLargeView != null)
            {
                DetailLargeView.BottomAppBarMargin = BottomAppBarMargin;
                DetailLargeView.TopLayoutMargin = headerPosition.Y - ContentMargin;

                DetailLargeView.LayoutUpdate(pageSize);
            }

            if (GroupItemsView != null)
            {
                GroupItemsView.BottomAppBarMargin = BottomAppBarMargin;
                GroupItemsView.TopLayoutMargin = headerPosition.Y - ContentMargin;

                GroupItemsView.LayoutUpdate(pageSize);
            }
        }

        /// <summary>
        /// Render the page layout.
        /// </summary>
        /// <returns>View to use.</returns>
        protected override View RenderContent()
        {
            var contentView = base.RenderContent();

            contentView.IsVisible = ViewModel.IsProductListMode;

            return contentView;
        }

        /// <summary>
        /// Render filters in the header.
        /// </summary>
        /// <param name="headerLayout">Stack to use.</param>
        protected override void RenderFilters(StackLayout headerLayout)
        {
            base.RenderFilters(headerLayout);
            headerLayout.Spacing = 0.2;

            bool useGridView = Device.OS.OnPlatform(true, true, true, false, false);

            if (useGridView)
            {
                double itemWidth = Theme.CommonResources.CategoryImageWidth * 4;
                double itemHeight = Theme.CommonResources.CategoryImageWidth + (Theme.CommonResources.CategoryImageWidth * (1f / 3f));

                DataTemplate itemTemplate = new DataTemplate(
                     () =>
                     {
                         ContentViewButton featureButton = new ContentTemplateViewButton(
                                   new DataTemplate(
                                       () =>
                                       {
                                           var cell = new StoreItemThumbFeaturedView();
                                           cell.BindingContext = null;
                                           cell.PrepareBindings();
                                           return cell;
                                       }))
                         {
                             Command = ShowItemLargeDetailCommand,
                             Style = Theme.ApplicationStyles.TextOnlyContentButtonStyle
                         };

                         featureButton.SetBinding(ContentViewButton.CommandParameterProperty, ".");
                         featureButton.BindingContext = null;

                         return featureButton;
                     });

                FeaturedScrollView = new RepeaterRecycleView(2)
                {
                    Padding = 0,
                    ItemHeight = itemHeight,
                    ItemWidth = itemWidth,
                    ItemTemplate = itemTemplate
                };

                FeaturedScrollView.SetBinding<StoreListViewModel>(RepeaterRecycleView.IsVisibleProperty, vm => vm.IsFullSearchMode, converter: Theme.CommonResources.InvertBooleanToBooleanConverter);
                FeaturedScrollView.SetBinding<StoreListViewModel>(RepeaterRecycleView.ItemsSourceProperty, vm => vm.FeaturedItems);

                headerLayout.Children.Add(FeaturedScrollView);
            }
            else
            {
                FeaturedScrollView = new ScrollView()
                {
                    Orientation = ScrollOrientation.Horizontal,
                    Padding = 0,
                    MinimumHeightRequest = 80
                };

                FeaturedScrollView.SetBinding<StoreListViewModel>(ScrollView.IsVisibleProperty, vm => vm.IsFullSearchMode, converter: Theme.CommonResources.InvertBooleanToBooleanConverter);

                bool hasVisibleButton = Device.OS.OnPlatform(true, true, false, false, true);

                DataTemplate itemTemplate = null;

                if (hasVisibleButton)
                {
                    itemTemplate = new DataTemplate(
                            () =>
                            {
                                ContentViewButton featureButton = new ContentTemplateViewButton(
                                    new DataTemplate(
                                        () =>
                                        {
                                            var cell = new StoreItemThumbFeaturedView();

                                            cell.PrepareBindings();

                                            return cell;
                                        }))
                                {
                                    Command = ShowItemLargeDetailCommand,
                                    Style = Theme.ApplicationStyles.TextOnlyContentButtonStyle
                                };

                                featureButton.SetBinding(ContentViewButton.CommandParameterProperty, ".");

                                return featureButton;
                            });
                }
                else
                {
                    itemTemplate = new DataTemplate(
                            () =>
                            {
                                var cell = new StoreItemThumbFeaturedView();

                                cell.PrepareBindings();

                                return cell;
                            });
                }

                var repeaterFeatured = new RepeaterView<StoreItemViewModel>()
                {
                    IsUILocable = false,
                    Orientation = StackOrientation.Horizontal,
                    Spacing = 30,
                    ItemTemplate = itemTemplate
                };

                if (!hasVisibleButton)
                {
                    repeaterFeatured.ItemClickCommand = ShowItemLargeDetailCommand;
                }

                repeaterFeatured.SetBinding<StoreListViewModel>(RepeaterView<StoreItemViewModel>.ItemsSourceProperty, vm => vm.FeaturedItems);

                FeaturedScrollView.Content = repeaterFeatured;

                headerLayout.Children.Add(FeaturedScrollView);
            }

            ScrollView sublevelsScrollView = new ScrollView()
            {
                Orientation = ScrollOrientation.Horizontal,
                Padding = 0,
                MinimumHeightRequest = 60
            };

            sublevelsScrollView.SetBinding<StoreListViewModel>(ScrollView.IsVisibleProperty, vm => vm.IsProductListModeNoFullSearch);

            var repeaterSublevels = new RepeaterView<StoreItemLevel>()
            {
                IsUILocable = false,
                Orientation = StackOrientation.Horizontal,
                Spacing = 10,
                ItemTemplate = new DataTemplate(
                    () =>
                    {
                        var view = new StoreItemLevelSimpleView(true)
                        {
                            NavigateToStoreLevelCommand = NavigateToStoreLevelCommand
                        };

                        view.PrepareBindings();

                        return view;
                    })
            };

            repeaterSublevels.SetBinding<StoreListViewModel>(RepeaterView<StoreItemLevel>.ItemsSourceProperty, vm => vm.Sublevels);

            sublevelsScrollView.Content = repeaterSublevels;

            headerLayout.Children.Add(sublevelsScrollView);

            StackLayout stackSeparatorContainer = new StackLayout()
            {
                Style = Theme.ApplicationStyles.SimpleStackContainerStyle,
                Orientation = StackOrientation.Horizontal
            };

            stackSeparatorContainer.SetBinding<StoreListViewModel>(StackLayout.IsVisibleProperty, vm => vm.IsProductListModeNoFullSearch);

            var separatorView = RenderUtil.InstaceLineSeparator();

            separatorView.SetBinding<StoreListViewModel>(View.IsVisibleProperty, vm => vm.Sublevels.Count, converter: Theme.CommonResources.IntToBooleanConverter);

            stackSeparatorContainer.Children.Add(separatorView);

            headerLayout.Children.Add(stackSeparatorContainer);

            // Grouping.
            StackLayout stackGroupLabel = new StackLayout()
            {
                Style = Theme.ApplicationStyles.SimpleStackContainerStyle,
                Orientation = StackOrientation.Horizontal
            };

            stackGroupLabel.SetBinding<StoreListViewModel>(StackLayout.IsVisibleProperty, vm => vm.IsProductListModeNoFullSearch);

            ExtendedLabel labelGroupText = new ExtendedLabel()
            {
                Style = Theme.ApplicationStyles.DefaultExtendedLabelStyle,
                Text = App.LocalizationResources.ListGroupLabel,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start
            };

            stackGroupLabel.Children.Add(labelGroupText);

            stackGroupLabel.Children.Add(new BoxView() { Color = Color.Transparent, WidthRequest = 1, HeightRequest = 5 });

            LabelGroupValue = new TextContentViewButton()
            {
                Style = Theme.ApplicationStyles.TextOnlyContentButtonStyle,
                TextColor = Theme.CommonResources.Accent,
                Command = SelectGroupTypeCommand,
                FontName = labelGroupText.FontName,
                FriendlyFontName = labelGroupText.FriendlyFontName,
                FontSize = labelGroupText.FontSize,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                MinimumWidthRequest = 250
            };

            stackGroupLabel.Children.Add(LabelGroupValue);

            headerLayout.Children.Add(stackGroupLabel);

            UpdatePickerGroup(ViewModel.GroupType);
        }

        /// <summary>
        /// Render the floating search filter.
        /// </summary>
        /// <returns>View to add.</returns>
        protected override View RenderFloatingSearchFilter()
        {
            var newFilter = new FilterBarStoreView(ViewModel.ShowProfileCommand)
            {
                FilterBackgroundColor = Theme.CommonResources.Accent,
                Command = ExecuteSearchCommand,
                Opacity = 0,
                IsVisible = false
            };

            newFilter.PrepareBindings();

            newFilter.SetBinding<StoreListViewModel>(FilterBarView.TextProperty, vm => vm.FilterTerm, mode: BindingMode.TwoWay);
            newFilter.SetBinding<StoreListViewModel>(FilterBarView.CommandParameterProperty, vm => vm.FilterTerm);
            newFilter.SetBinding<StoreListViewModel>(FilterBarView.PlaceholderProperty, vm => vm.LocalizationResources.SearchPlaceHolderText);

            FilterBar = newFilter;

            return newFilter;
        }

        /// <summary>
        /// Render the header of the list.
        /// </summary>
        /// <returns>View to use.</returns>
        protected override View RenderHeader()
        {
            StackLayout headerLayout = base.RenderHeader() as StackLayout;

            if (headerLayout != null)
            {
                StackLayout stackSeparatorContainer = new StackLayout()
                {
                    Style = Theme.ApplicationStyles.SimpleStackContainerStyle,
                    Orientation = StackOrientation.Vertical
                };

                stackSeparatorContainer.SetBinding<StoreListViewModel>(StackLayout.IsVisibleProperty, vm => vm.IsProductListModeNoFullSearch);

                RenderUtil.RenderSpace(stackSeparatorContainer, heightRequest: 5);

                // Navigation bar.
                var levelView = new StoreItemLevelSimpleView(false, true, true, true)
                {
                    Padding = 5,
                    NavigateToStoreLevelCommand = NavigateToStoreLevelCommand,
                    NavigateBackCommand = ViewModel.NavigateBackCommand,
                    BackgroundMargin = 20,
                    BackgroundTranslateX = ContentMargin
                };

                levelView.PrepareBindings();

                levelView.SetBinding<StoreListViewModel>(StoreItemLevelView.BindingContextProperty, vm => vm.Level);
                stackSeparatorContainer.Children.Add(levelView);

                RenderUtil.RenderSpace(stackSeparatorContainer, heightRequest: 5);

                headerLayout.Children.Insert(1, stackSeparatorContainer);
            }

            return headerLayout;
        }

        /// <summary>
        /// Render layer layout like the notifications and the progress.
        /// </summary>
        /// <param name="baseLayout">Layout to use</param>
        protected override void RenderLayerLayout(AbsoluteLayout baseLayout)
        {
            if (baseLayout != null)
            {
                CategoryView = new StoreListCategoryView(this)
                {
                    ContentMargin = ContentMargin,
                    NavigateBackCommand = RelativeBackCommand
                };

                if (CategoryView.Opacity > 0)
                {
                    CategoryView.Opacity = 0;
                }

                if (CategoryView.IsVisible)
                {
                    CategoryView.IsVisible = false;
                }

                if (CategoryView != null)
                {
                    baseLayout.Children.Add(CategoryView);

                    if (ViewModel.IsProductListMode)
                    {
                        if (ViewModel.IsFullSearchMode)
                        {
                            AC.ThreadManager.ScheduleManaged(
                            () =>
                            {
                                ViewModel.LoadRecentSearch();
                            });
                        }
                    }
                    else
                    {
                        AC.ThreadManager.ScheduleManaged(
                            () =>
                            {
                                CategoryView.InitializeView();
                                CategoryView.PrepareBindings();

                                if (CategoryView.Opacity < 1)
                                {
                                    CategoryView.Opacity = 1;
                                }

                                if (!CategoryView.IsVisible)
                                {
                                    CategoryView.IsVisible = true;
                                }
                            });
                    }
                }
            }

            base.RenderLayerLayout(baseLayout);

            if (ViewModel.IsFullSearchMode)
            {
                if (StackShortCuts != null)
                {
                    StackShortCuts.Opacity = 0;
                    StackShortCuts.IsVisible = false;
                }

                if (Header != null)
                {
                    Header.Opacity = 0;
                }
            }

            if (ViewModel.IsProductListMode && ViewModel.IsStartViewSearchMode)
            {
                Parcero.Core.App.ThreadManager.ScheduleManaged(
                    TimeSpan.FromSeconds(0.1),
                    () =>
                    {
                        ShowSearchCommand.ExecuteIfCan();
                    });
            }
        }

        /// <summary>
        /// Render the page layout.
        /// </summary>
        /// <param name="baseLayout">Layout to use.</param>
        protected override void RenderPageLayout(AbsoluteLayout baseLayout)
        {
            base.RenderPageLayout(baseLayout);
        }

        /// <summary>
        /// Render panel when no elements in list.
        /// </summary>
        /// <param name="panelNoElementsLayout">Stack to use.</param>
        protected override void RenderPanelNoElements(StackLayout panelNoElementsLayout)
        {
            var panelFoundNoElementsLayout = new StackLayout()
            {
                Style = Theme.ApplicationStyles.FormRowContainerStyle,
                Orientation = StackOrientation.Vertical
            };

            panelFoundNoElementsLayout.SetBinding<StoreListViewModel>(View.IsVisibleProperty, vm => vm.IsProductListMode);

            base.RenderPanelNoElements(panelFoundNoElementsLayout);

            GlyphContentViewButton searchAllStore = new GlyphContentViewButton(true, true, Anuracode.Support.Model.ImageOrientation.ImageToLeft)
            {
                Text = App.LocalizationResources.SearchAllStoreLabel,
                Style = Theme.ApplicationStyles.MainSubMenuContentButtonStyle,
                HorizontalOptions = LayoutOptions.Center,
                Command = ViewModel.SearchAllStoreCommand,
                GlyphText = Theme.CommonResources.GlyphTextSearch,
                ButtonBackgroundColor = Theme.CommonResources.Accent,
                ContentAlignment = TextAlignment.Start
            };

            searchAllStore.SetBinding<StoreListViewModel>(GlyphContentViewButton.IsVisibleContentProperty, vm => vm.IsFullSearchMode, converter: Theme.CommonResources.InvertBooleanToBooleanConverter);

            panelFoundNoElementsLayout.Children.Add(searchAllStore);

            panelNoElementsLayout.Children.Add(panelFoundNoElementsLayout);
        }

        /// <summary>
        /// Scroll moved.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Arguments of the event.</param>
        private void FeaturedScrollView_Scrolled(object sender, ScrolledEventArgs e)
        {
            featuredScrollIdle = false;
        }

        /// <summary>
        /// When the load items is complete.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Arguments of the event.</param>
        private async void LoadFeaturedItemsCommand_CommandCompleted(object sender, IEnumerable<StoreItemViewModel> e)
        {
            int elementsCount = await e.CountAsync();

            Parcero.Core.App.ThreadManager.ScheduleManaged(
                    TimeSpan.FromSeconds(0.1),
                    () =>
                    {
                        UpdateBackgroundOpactity(elementsCount);
                    });
        }

        /// <summary>
        /// When the load items is complete.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Arguments of the event.</param>
        private void LoadItemsCommand_CommandCompleted(object sender, IEnumerable<StoreItemViewModel> newElements)
        {
            UpdatePickerGroup(ViewModel.GroupType);

            if (ViewModel.IsProductListMode)
            {
                Parcero.Core.App.ThreadManager.ScheduleManaged(
                    TimeSpan.FromSeconds(0.1),
                    () =>
                    {
                        AddExtraLayers();
                    });

                if (ViewModel.IsFullSearchMode)
                {
                    Parcero.Core.App.ThreadManager.ScheduleManaged(
                    async () =>
                    {
                        while (!SuggestionsRepeater.IsVisible)
                        {
                            await Task.Delay(500);
                        }

                        try
                        {
                            if (SuggestionsRepeater != null && (SuggestionsRepeater.ScrollX > 0) && (SuggestionsRepeater.ContentSize.Width > SuggestionsRepeater.Width))
                            {
                                var task = SuggestionsRepeater.ScrollToAsync(0, SuggestionsRepeater.ScrollY, false);
                            }
                        }
                        catch
                        {
                        }
                    });
                }
            }
        }

        /// <summary>
        /// When the load items is complete.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Arguments of the event.</param>
        private void LoadSublevelsCommand_CommandCompleted(object sender, IEnumerable<StoreItemLevel> e)
        {
            if (!ViewModel.IsProductListMode)
            {
                Parcero.Core.App.ThreadManager.ScheduleManaged(
                    TimeSpan.FromSeconds(0.1),
                    async () =>
                    {
                        AddExtraLayers();

                        if (e != null)
                        {
                            var first = await e.FirstOrDefaultAsync();
                            if (first == null)
                            {
                                SwitchToProductionListModeCommand.ExecuteIfCan();
                            }
                        }
                        else
                        {
                            SwitchToProductionListModeCommand.ExecuteIfCan();
                        }
                    });
            }
        }

        /// <summary>
        /// Update the background opacity.
        /// </summary>
        /// <param name="featuredCount">Feautes count.</param>
        private void UpdateBackgroundOpactity(int? featuredCount = null)
        {
            int itemsCount = (featuredCount == null) ? ViewModel.FeaturedItems.Count : featuredCount.Value;

            bool backgroundVisible = ViewModel.IsProductListMode || (!ViewModel.IsProductListMode && itemsCount == 0);

            if (BackgroundTopShape != null)
            {
                BackgroundTopShape.Opacity = backgroundVisible ? 1 : 0;
            }

            if (BackgroundTopRightShape != null)
            {
                BackgroundTopRightShape.Opacity = backgroundVisible ? 1 : 0;
            }
        }

        /// <summary>
        /// Updates the selected value.
        /// </summary>
        /// <param name="listGroup">Grouping to use.</param>
        private void UpdatePickerGroup(int groupType)
        {
            if (LabelGroupValue != null)
            {
                Parcero.Core.App.ThreadManager.ScheduleManaged(() =>
                {
                    Model.StoreListingGroup groupTypeValue = (Model.StoreListingGroup)groupType;

                    switch (groupTypeValue)
                    {
                        case StoreListingGroup.Featured:
                            LabelGroupValue.Text = App.LocalizationResources.StoreListGroupFeaturedValue;
                            break;

                        case StoreListingGroup.New:
                            LabelGroupValue.Text = App.LocalizationResources.StoreListGroupNewValue;
                            break;

                        case StoreListingGroup.Name:
                        case StoreListingGroup.Price:
                        default:
                            LabelGroupValue.Text = App.LocalizationResources.StoreListGroupAllValue;
                            break;
                    }

                    return Task.FromResult(0);
                });
            }
        }
    }
}
