// <copyright file="RepositoryCart.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Sample.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Anuracode.Forms.Controls.Sample.Repository
{
    /// <summary>
    /// Repository cart.
    /// </summary>
    public class RepositoryCart
    {
        /// <summary>
        /// Load the items to show.
        /// </summary>
        /// <param name="filterTerm">Term to filter the elements with.</param>
        /// <param name="skip">The number of elements to skip from the result.</param>
        /// <param name="pageSize">The number of elements to take from the result.</param>
        /// <param name="cacheData">True when the data should be reloaded without cache.</param>
        /// <param name="cancellationToken">Cancellation token for when the operation is cancel.</param>
        /// <returns>Results to use.</returns>
        public async Task<PagedResult<StoreItemCartViewModel>> GetItemsAsync(string filterTerm, int skip, int pageSize, bool cacheData, System.Threading.CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
