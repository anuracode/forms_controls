// <copyright file="AddressListViewModel.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Sample.Repository;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Anuracode.Forms.Controls.Sample.ViewModels
{
    /// <summary>
    /// View model for the list of address.
    /// </summary>
    public class AddressListViewModel : ListPagedViewModelBase<Model.Address>
    {
        /// <summary>
        /// Default constuctor.
        /// </summary>
        public AddressListViewModel()
            : base()
        {
        }

        /// <summary>
        /// Page title.
        /// </summary>
        public override string PageTitle
        {
            get
            {
                return LocalizationResources.AddressesLabel;
            }
        }

        /// <summary>
        /// Repository for client.
        /// </summary>
        public RepositoryAddress RepositoryItem
        {
            get
            {
                return App.RepositoryAddress;
            }
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
        protected override async Task<PagedResult<Model.Address>> LoadItemsAsync(string filterTerm, int skip, int pageSize, bool cacheData, System.Threading.CancellationToken cancellationToken)
        {
            PagedResult<Model.Address> pagedResults = new PagedResult<Model.Address>();

            var addressList = await RepositoryItem.GetAllAsync(cancellationToken);

            if (addressList != null)
            {
                if (string.IsNullOrEmpty(filterTerm))
                {
                    pagedResults.PagedItems = from filteredItem in addressList
                                              orderby filteredItem.IsDefault descending
                                              select filteredItem;
                }
                else
                {
                    pagedResults.PagedItems = from filteredItem in addressList
                                              where
                                              (!string.IsNullOrWhiteSpace(filteredItem.Name) && filteredItem.Name.ToUpperInvariant().Contains(filterTerm.ToUpperInvariant())) ||
                                              (!string.IsNullOrWhiteSpace(filteredItem.Line1) && filteredItem.Line1.ToUpperInvariant().Contains(filterTerm.ToUpperInvariant())) ||
                                              (!string.IsNullOrWhiteSpace(filteredItem.Line2) && filteredItem.Line2.ToUpperInvariant().Contains(filterTerm.ToUpperInvariant())) ||
                                              (!string.IsNullOrWhiteSpace(filteredItem.State) && filteredItem.State.ToUpperInvariant().Contains(filterTerm.ToUpperInvariant()))
                                              select filteredItem;
                }

                pagedResults.TotalItemsCount = pagedResults.PagedItems.Count();
            }

            return pagedResults;
        }
    }
}