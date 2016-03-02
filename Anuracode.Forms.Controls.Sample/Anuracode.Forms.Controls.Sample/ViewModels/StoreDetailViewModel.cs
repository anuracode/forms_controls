// <copyright file="StoreDetailViewModel.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Sample.Model;
using Anuracode.Forms.Controls.Sample.Repository;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls.Sample.ViewModels
{
    /// <summary>
    /// View model for the detail of an store item in the store mode.
    /// </summary>
    public class StoreDetailViewModel : BaseViewModel
    {
        /// <summary>
        /// Lock for the images.
        /// </summary>
        private static SemaphoreSlim lockQuery = new SemaphoreSlim(1);

        /// <summary>
        /// Current item with view model.
        /// </summary>
        private StoreItemViewModel currentItemViewModel;

        /// <summary>
        /// Current loaded group.
        /// </summary>
        private StoreItemViewModel currentLoadedGroup;

        /// <summary>
        /// Elemets to display.
        /// </summary>
        private ObservableCollectionFast<StoreItemViewModel> groupItems = new ObservableCollectionFast<StoreItemViewModel>();

        /// <summary>
        /// Load grouped items.
        /// </summary>
        private Command<StoreItemViewModel> loadGroupedItemsCommand;

        /// <summary>
        /// Navigate to store level.
        /// </summary>
        private Command<StoreItemLevel> navigateToStoreLevelCommand;

        /// <summary>
        /// Show image gallery.
        /// </summary>
        private Command<string> showGalleryCommand;

        /// <summary>
        /// Show the items options.
        /// </summary>
        private Command<StoreItemViewModel> viewStoreItemDetailCommand;

        /// <summary>
        /// Default constuctor.
        /// </summary>
        public StoreDetailViewModel()
            : base()
        {
        }

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
        /// Current item with view model.
        /// </summary>
        public StoreItemViewModel CurrentItemViewModel
        {
            get
            {
                return currentItemViewModel;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref currentItemViewModel, value);

                if (value != null)
                {
                    Title = value.Item.Name;
                }
            }
        }

        /// <summary>
        /// Current loaded group.
        /// </summary>
        public StoreItemViewModel CurrentLoadedGroup
        {
            get
            {
                return currentLoadedGroup;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref currentLoadedGroup, value);
            }
        }

        /// <summary>
        /// Elemets to display.
        /// </summary>
        public ObservableCollectionFast<StoreItemViewModel> GroupItems
        {
            get
            {
                return groupItems;
            }
        }

        /// <summary>
        /// Load elements.
        /// </summary>
        public virtual Command<StoreItemViewModel> LoadGroupedItemsCommand
        {
            get
            {
                if (this.loadGroupedItemsCommand == null)
                {
                    var tmpCommand = new Command<StoreItemViewModel>(
                        async (selectedItem) =>
                        {
                            IEnumerable<StoreItemViewModel> newElements = null;

                            try
                            {
                                await LockQuery.WaitAsync();

                                var pageResult = await RepositoryStoreItem.GetPagedAsync(null, string.Empty, 0, int.MaxValue, true, CancellationToken.None, 0, groupParentId: selectedItem.Item.Id);
                                newElements = pageResult.PagedItems;

                                CurrentLoadedGroup = selectedItem;
                            }
                            finally
                            {
                                LockQuery.Release();
                            }

                            if ((newElements != null) && (await newElements.FirstOrDefaultAsync() != null))
                            {
                                AC.ScheduleManaged(
                                () =>
                                {
                                    GroupItems.Reset(newElements);

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
                                        GroupItems.Clear();
                                    }
                                    catch (ArgumentOutOfRangeException)
                                    {
                                        // Skip becasue there is a bug in Xamarin.Forms.
                                    }

                                    return Task.FromResult(0);
                                });
                            }
                        },
                        (selectedItem) =>
                        {
                            return (selectedItem != null) && (selectedItem.Item != null) && selectedItem.Item.IsGroupParent;
                        });

                    this.loadGroupedItemsCommand = tmpCommand;
                }

                return this.loadGroupedItemsCommand;
            }
        }

        /// <summary>
        /// Navigate to store level.
        /// </summary>
        public Command<StoreItemLevel> NavigateToStoreLevelCommand
        {
            get
            {
                if (navigateToStoreLevelCommand == null)
                {
                    navigateToStoreLevelCommand = new Command<StoreItemLevel>(
                        (subLevel) =>
                        {
                        },
                        (sublevel) =>
                        {
                            return sublevel != null;
                        });
                }

                return navigateToStoreLevelCommand;
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
        /// Show image gallery.
        /// </summary>
        public Command<string> ShowGalleryCommand
        {
            get
            {
                if (showGalleryCommand == null)
                {
                    showGalleryCommand = new Command<string>(
                        async (selectedValue) =>
                        {
                            await Task.FromResult(0);

                            if (CurrentItemViewModel != null && CurrentItemViewModel.Images != null && CurrentItemViewModel.Images.Count > 0)
                            {
                            }
                        },
                        (selectedValue) =>
                        {
                            return true;
                        });
                }

                return showGalleryCommand;
            }
        }

        /// <summary>
        /// Show the items options.
        /// </summary>
        public Command<StoreItemViewModel> ViewStoreItemDetailCommand
        {
            get
            {
                if (viewStoreItemDetailCommand == null)
                {
                    viewStoreItemDetailCommand = new Command<StoreItemViewModel>(
                        async (selectedElement) =>
                        {
                            await Task.FromResult(0);
                        },
                        (selectedElement) =>
                        {
                            return selectedElement != null;
                        });
                }

                return viewStoreItemDetailCommand;
            }
        }

        /// <summary>
        /// Can share content.
        /// </summary>
        /// <returns>True when can share content.</returns>
        protected override bool CanShareContent()
        {
            return (CurrentItemViewModel != null) && (CurrentItemViewModel.Item != null);
        }
    }
}