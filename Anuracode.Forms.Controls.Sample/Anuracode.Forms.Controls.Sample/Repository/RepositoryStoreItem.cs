// <copyright file="RepositoryStoreItem.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Anuracode.Forms.Controls.Sample.Model;
using Anuracode.Forms.Controls.Sample.ViewModels;

namespace Anuracode.Forms.Controls.Sample.Repository
{
    /// <summary>
    /// Repository for the store items.
    /// </summary>
    public class RepositoryStoreItem
    {

        /// <summary>
        /// Get all the items that match the conditions.
        /// </summary>
        /// <param name="level">Level of the items to filter.</param>
        /// <param name="filterTerm">Term that the item should match.</param>
        /// <param name="skip">From the results, skip those number of elements.</param>
        /// <param name="pageSize">Page size to return.</param>
        /// <param name="cacheData">True when a cache result should not be used.</param>
        /// <param name="cancellationToken">Cancellation token to use.</param>
        /// <param name="groupType">Group type to use.</param>
        /// <param name="featured">Query only the featured.</param>
        /// <param name="newItems">Filter new items.</param>
        /// <param name="groupParentId">Filter by group id.</param>
        /// <returns>Task to await.</returns>
        public async Task<PagedResult<StoreItemViewModel>> GetPagedAsync(
            StoreItemLevel level,
            string filterTerm,
            int skip,
            int pageSize,
            bool cacheData,
            CancellationToken cancellationToken,
            int groupType,
            bool? featured = null,
            bool? newItems = null,
            string groupParentId = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get sublevels for a particular level.
        /// </summary>
        /// <param name="level">Level to use.</param>
        /// <param name="cacheData">True when a cache result should not be used.</param>
        /// <param name="cancellationToken">Cancellation token to use.</param>
        /// <returns>Task to await.</returns>
        public async Task<IEnumerable<StoreItemLevel>> GetSublevelsAsync(StoreItemLevel level, bool cacheData, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
