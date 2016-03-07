// <copyright file="CartListViewModel.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Sample.Repository;
using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls.Sample.ViewModels
{
    /// <summary>
    /// View model for the shopping kart.
    /// </summary>
    public class CartListViewModel : ListPagedViewModelBase<StoreItemCartViewModel>
    {
        /// <summary>
        /// Flag to know that is the first run.
        /// </summary>
        private static bool isFirstRun = true;

        /// <summary>
        /// Lock for the load.
        /// </summary>
        private static SemaphoreSlim lockLoad = new SemaphoreSlim(1);

        /// <summary>
        /// Place order automatically.
        /// </summary>
        private bool autoPlaceOrder;

        /// <summary>
        /// Select payment type.
        /// </summary>
        private bool canSelectPaymentType;

        /// <summary>
        /// Can select shipping type.
        /// </summary>
        private bool canSelectShippingType;

        /// <summary>
        /// Destination address.
        /// </summary>
        private Model.Address destinationAddress;

        /// <summary>
        /// Flag to force client reload.
        /// </summary>
        private bool forceCientReload = false;

        /// <summary>
        /// Flag when the client has multiple addresses.
        /// </summary>
        private bool hasMultipleAddresses;

        /// <summary>
        /// Cache of the number of items last time the shipping options were calculated.
        /// </summary>
        private int itemsCountShippingCache;

        /// <summary>
        /// Place an order of the items in the cart.
        /// </summary>
        private Command placeOrderCommand;

        /// <summary>
        /// Command for process order.
        /// </summary>
        private Command<object> processPreorderCommand;

        /// <summary>
        /// Subscription for the repository.
        /// </summary>
        private IDisposable repositorySubscriptionClient;

        /// <summary>
        /// Subscription for the repository.
        /// </summary>
        private IDisposable repositorySubscriptionLocal;

        /// <summary>
        /// Default constuctor.
        /// </summary>
        public CartListViewModel()
            : base()
        {
        }

        /// <summary>
        /// Lock for the load.
        /// </summary>
        public static SemaphoreSlim LockLoad
        {
            get
            {
                return lockLoad;
            }
        }

        /// <summary>
        /// Repository for item.
        /// </summary>
        public static RepositoryCart RepositoryCartLocal
        {
            get
            {
                return App.RepositoryCart;
            }
        }

        /// <summary>
        /// Place order automatically.
        /// </summary>
        public bool AutoPlaceOrder
        {
            get
            {
                return autoPlaceOrder;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref autoPlaceOrder, value);
            }
        }

        /// <summary>
        /// Page title.
        /// </summary>
        public override string PageTitle
        {
            get
            {
                return LocalizationResources.ShoppingCartLabel;
            }
        }

        /// <summary>
        /// Place an order of the items in the cart.
        /// </summary>
        public Command PlaceOrderCommand
        {
            get
            {
                if (placeOrderCommand == null)
                {
                    placeOrderCommand = new Command(
                        async (itemsOrder) =>
                        {
                            await Task.FromResult(0);
                        });
                }

                return placeOrderCommand;
            }
        }

        /// <summary>
        /// Total items cart.
        /// </summary>
        public int TotalItemsCart
        {
            get
            {
                return Items.Count;
            }
        }

        /// <summary>
        /// Loading items token source.
        /// </summary>
        protected CancellationTokenSource LoadingItemsTokenSource { get; set; }

        /// <summary>
        /// Load the items to show.
        /// </summary>
        /// <param name="filterTerm">Term to filter the elements with.</param>
        /// <param name="skip">The number of elements to skip from the result.</param>
        /// <param name="pageSize">The number of elements to take from the result.</param>
        /// <param name="cacheData">True when the data should be reloaded without cache.</param>
        /// <param name="cancellationToken">Cancellation token for when the operation is cancel.</param>
        /// <returns>Results to use.</returns>
        protected override async Task<PagedResult<StoreItemCartViewModel>> LoadItemsAsync(string filterTerm, int skip, int pageSize, bool cacheData, System.Threading.CancellationToken cancellationToken)
        {
            try
            {
                CancellationToken loadingToken = CancellationToken.None;
                if (LoadingItemsTokenSource != null)
                {
                    loadingToken = LoadingItemsTokenSource.Token;
                }

                await LockLoad.WaitAsync(loadingToken);

                loadingToken.ThrowIfCancellationRequested();              

                var pagedResults = await RepositoryCartLocal.GetItemsAsync(filterTerm, skip, pageSize, cacheData, loadingToken);

                return pagedResults;
            }
            finally
            {
                LockLoad.Release();
            }
        }
    }
}