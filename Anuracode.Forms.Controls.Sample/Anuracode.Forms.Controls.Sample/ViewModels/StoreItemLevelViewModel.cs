// <copyright file="StoreItemLevelViewModel.cs" company="Anura Code">
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
    /// View model for a store item.
    /// Used for binding images.
    /// </summary>
    public class StoreItemLevelViewModel : BaseEntity
    {
        /// <summary>
        /// Default image path.
        /// </summary>
        private string defaultImagePath = Theme.CommonResources.PathImageAppLogoLarge;

        /// <summary>
        /// Default thumb image path.
        /// </summary>
        private string defaultThumbImagePath = Theme.CommonResources.PathImageAppLogoLarge;

        /// <summary>
        /// Has items loaded.
        /// </summary>
        private bool hasItemsLoaded = false;

        /// <summary>
        /// Item to use.
        /// </summary>
        private StoreItemLevel item;

        /// <summary>
        /// Command for loading the items.
        /// </summary>
        private Command loadItemsCommand;

        /// <summary>
        /// Semaphore for the repository file.
        /// </summary>
        private SemaphoreSlim lockLoadItems;        

        /// <summary>
        /// Elemets to display.
        /// </summary>
        private ObservableCollectionFast<StoreItemViewModel> topItems = new ObservableCollectionFast<StoreItemViewModel>();

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="storeItemLevel">Store item to use.</param>
        /// <param name="externalExceptionHandler">External exception handler.</param>
        /// <param name="loadThumbImage">Load thumb image.</param>
        /// <param name="loadOtherImages">Load other images.</param>
        /// <param name="cancellationToken">Cancel token.</param>
        public StoreItemLevelViewModel(StoreItemLevel storeItemLevel, Action<Exception> externalExceptionHandler, CancellationToken cancellationToken, bool loadThumbImage = false, bool loadItems = true)
        {
            ExternalExceptionHandler = externalExceptionHandler;
            Item = storeItemLevel;
            CancellationToken = cancellationToken;

            if (Item != null)
            {
                IsReadOnly = true;

                if (loadThumbImage)
                {
                    LoadThumbImage(cancellationToken);
                    LoadOtherImages(cancellationToken);
                }
            }
        }

        /// <summary>
        /// Flag for when the system is loading.
        /// </summary>
        public virtual bool IsLoading
        {
            get
            {
                if (hasItemsLoaded)
                {
                    return !LoadItemsCommand.CanExecute(null);
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Item to use.
        /// </summary>
        public StoreItemLevel Item
        {
            get
            {
                return item;
            }

            protected set
            {
                ValidateRaiseAndSetIfChanged(ref item, value);
            }
        }

        /// <summary>
        /// Flag to load the items once.
        /// </summary>
        private bool itemsLoaded;

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
                            IEnumerable<StoreItemViewModel> newElements = null;

                            OnPropertyChanged(nameof(IsLoading));

                            try
                            {
                                await LockLoadItems.WaitAsync(CancellationToken);

                                var pageResult = await RepositoryStoreItem.GetPagedAsync(item, string.Empty, 0, PageSize, true, CancellationToken.None, 0);

                                if (pageResult != null && pageResult.PagedItems != null)
                                {
                                    itemsLoaded = true;
                                    newElements = pageResult.PagedItems;
                                }
                            }
                            finally
                            {
                                LockLoadItems.Release();
                            }

                            AC.ScheduleManaged(
                                 async () =>
                                 {
                                     try
                                     {
                                         await BaseViewModel.LockUI.WaitAsync(CancellationToken);
                                         if ((newElements != null) && (await newElements.FirstOrDefaultAsync() != null))
                                         {
                                             TopItems.Reset(newElements);
                                             hasItemsLoaded = true;

                                             if (IsReadOnly)
                                             {
                                                 IsReadOnly = false;
                                             }

                                             OnPropertyChanged(nameof(IsLoading));
                                         }
                                         else
                                         {
                                             try
                                             {
                                                 TopItems.Clear();
                                             }
                                             catch (ArgumentOutOfRangeException)
                                             {
                                                 // Skip becasue there is a bug in Xamarin.Forms.
                                             }

                                             hasItemsLoaded = false;

                                             if (IsReadOnly)
                                             {
                                                 IsReadOnly = false;
                                             }

                                             OnPropertyChanged(nameof(IsLoading));
                                         }
                                     }
                                     finally
                                     {
                                         BaseViewModel.LockUI.Release();
                                     }
                                 });
                        },
                        () =>
                        {
                            return (Item != null) && !itemsLoaded;
                        });

                    this.loadItemsCommand = tmpCommand;
                }

                return this.loadItemsCommand;
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
        /// Repository for the store.
        /// </summary>
        public RepositoryStoreItem RepositoryStoreItem
        {
            get
            {
                return App.RepositoryStoreItem;
            }
        }

        /// <summary>
        /// Elemets to display.
        /// </summary>
        public ObservableCollectionFast<StoreItemViewModel> TopItems
        {
            get
            {
                return topItems;
            }
        }

        /// <summary>
        /// Cancellation token.
        /// </summary>
        protected CancellationToken CancellationToken { get; set; }

        /// <summary>
        /// External exception handler.
        /// </summary>
        protected Action<Exception> ExternalExceptionHandler { get; set; }

        /// <summary>
        /// Page size.
        /// </summary>
        protected int PageSize
        {
            get
            {
                return 10;
            }
        }

        /// <summary>
        /// Equal operator.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True if are the same.</returns>
        public override bool Equals(object obj)
        {
            // If parameter cannot be cast to ThreeDPoint return false:
            StoreItemViewModel compareValue = obj as StoreItemViewModel;
            if (compareValue == null)
            {
                return false;
            }
            else
            {
                if (compareValue.Item != null && Item != null)
                {
                    return Item.Equals(compareValue.Item);
                }
            }

            // Return true if the fields match:
            return base.Equals(compareValue);
        }

        /// <summary>
        /// Get hash code.
        /// </summary>
        /// <returns>Hash code of the object.</returns>
        public override int GetHashCode()
        {
            return Item == null ? base.GetHashCode() : Item.GetHashCode();
        }

        /// <summary>
        /// Handle exception from commands.
        /// </summary>
        /// <param name="ex">Exception to handle.</param>
        public void HandlerCommandException(Exception ex)
        {
            if (ExternalExceptionHandler == null)
            {
                if (ex is System.Threading.Tasks.TaskCanceledException)
                {
                    return;
                }
                else if (ex is System.OperationCanceledException)
                {
                    return;
                }
                else
                {
                    AC.TraceError(ex.Message, ex);
                }
            }
            else
            {
                ExternalExceptionHandler(ex);
            }
        }

        /// <summary>
        /// Load  images, the images will overwrite.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to use.</param>
        public void LoadOtherImages(CancellationToken cancellationToken)
        {
            if (Item != null)
            {
                // Not needed yet.
            }
        }

        /// <summary>
        /// Load thumb images, the images will overwrite.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to use.</param>
        public void LoadThumbImage(CancellationToken cancellationToken)
        {
            if (Item != null)
            {
                // Not needed yet.
            }
        }

        /// <summary>
        /// Load the price of the item.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to use.</param>
        public void LoadTopItems(CancellationToken cancellationToken)
        {
            if (LoadItemsCommand.CanExecute(null))
            {
                LoadItemsCommand.Execute(null);
            }
        }
    }
}
