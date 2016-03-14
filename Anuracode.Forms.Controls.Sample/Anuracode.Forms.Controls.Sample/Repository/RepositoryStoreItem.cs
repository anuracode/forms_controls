// <copyright file="RepositoryStoreItem.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Extensions;
using Anuracode.Forms.Controls.Sample.Model;
using Anuracode.Forms.Controls.Sample.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Anuracode.Forms.Controls.Sample.Repository
{
    /// <summary>
    /// Repository for the store items.
    /// </summary>
    public class RepositoryStoreItem
    {
        /// <summary>
        /// Sublevels cache.
        /// </summary>
        protected List<StoreItemLevel> sublevelsCache = new List<StoreItemLevel>();

        /// <summary>
        /// Flag for the sample data.
        /// </summary>
        private bool sampleDataInitilized = false;

        /// <summary>
        /// Store item number.
        /// </summary>
        private int storeItemNumber;

        /// <summary>
        /// Store items cache.
        /// </summary>
        private List<StoreItem> storeItemsCache = new List<StoreItem>();

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
            await Task.FromResult(0);

            InitSampleData();

            PagedResult<StoreItemViewModel> results = new PagedResult<StoreItemViewModel>();

            IEnumerable<StoreItem> filteredItems = from item in storeItemsCache
                                                   orderby item.IsFeautred, item.FeaturedWeight, item.Name
                                                   select item;

            if (level != null)
            {
                if (!string.IsNullOrEmpty(level.Department) && string.IsNullOrEmpty(level.Category) && string.IsNullOrEmpty(level.Subcategory))
                {
                    filteredItems = from item in storeItemsCache
                                    where (string.Compare(level.Department, item.Department, StringComparison.CurrentCultureIgnoreCase) == 0)
                                    orderby item.IsFeautred, item.FeaturedWeight, item.Name
                                    select item;
                }
                else if (!string.IsNullOrEmpty(level.Department) && !string.IsNullOrEmpty(level.Category) && string.IsNullOrEmpty(level.Subcategory))
                {
                    filteredItems = from item in storeItemsCache
                                    where (string.Compare(level.Department, item.Department, StringComparison.CurrentCultureIgnoreCase) == 0) &&
                                    (string.Compare(level.Category, item.Category, StringComparison.CurrentCultureIgnoreCase) == 0)
                                    orderby item.IsFeautred, item.FeaturedWeight, item.Name
                                    select item;
                }
                else if (!string.IsNullOrEmpty(level.Department) && !string.IsNullOrEmpty(level.Category) && !string.IsNullOrEmpty(level.Subcategory))
                {
                    filteredItems = from item in storeItemsCache
                                    where (string.Compare(level.Department, item.Department, StringComparison.CurrentCultureIgnoreCase) == 0) &&
                                    (string.Compare(level.Category, item.Category, StringComparison.CurrentCultureIgnoreCase) == 0) &&
                                    (string.Compare(level.Subcategory, item.Subcategory, StringComparison.CurrentCultureIgnoreCase) == 0)
                                    orderby item.IsFeautred, item.FeaturedWeight, item.Name
                                    select item;
                }
            }

            if (featured.HasValue)
            {
                filteredItems = from item in filteredItems
                                where item.IsFeautred == featured
                                select item;
            }

            results.TotalItemsCount = filteredItems.Count();

            var pagedItems = (from item in filteredItems
                              select item).Skip(skip).Take(pageSize);

            results.PagedItems = from item in pagedItems
                                 select new StoreItemViewModel(item);

            return results;
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
            await Task.FromResult(0);

            InitSampleData();

            IEnumerable<StoreItemLevel> sublevels = null;

            if (level != null)
            {
                if (string.IsNullOrEmpty(level.Department) && string.IsNullOrEmpty(level.Category) && string.IsNullOrEmpty(level.Subcategory))
                {
                    sublevels = from sublevel in sublevelsCache
                                where !string.IsNullOrWhiteSpace(sublevel.Department) && string.IsNullOrEmpty(sublevel.Category) && string.IsNullOrEmpty(sublevel.Subcategory)
                                select sublevel;
                }
                else if (!string.IsNullOrEmpty(level.Department) && string.IsNullOrEmpty(level.Category) && string.IsNullOrEmpty(level.Subcategory))
                {
                    sublevels = from sublevel in sublevelsCache
                                where !string.IsNullOrWhiteSpace(sublevel.Department) &&
                                !string.IsNullOrWhiteSpace(sublevel.Category) &&
                                string.IsNullOrWhiteSpace(sublevel.Subcategory) &&
                                (string.Compare(level.Department, sublevel.Department, StringComparison.CurrentCultureIgnoreCase) == 0)
                                select sublevel;
                }
                else if (!string.IsNullOrEmpty(level.Department) && !string.IsNullOrEmpty(level.Category) && string.IsNullOrEmpty(level.Subcategory))
                {
                    sublevels = from sublevel in sublevelsCache
                                where !string.IsNullOrWhiteSpace(sublevel.Department) &&
                                !string.IsNullOrWhiteSpace(sublevel.Category) &&
                                !string.IsNullOrWhiteSpace(sublevel.Subcategory) &&
                                (string.Compare(level.Department, sublevel.Department, StringComparison.CurrentCultureIgnoreCase) == 0) &&
                                (string.Compare(level.Category, sublevel.Category, StringComparison.CurrentCultureIgnoreCase) == 0)
                                select sublevel;
                }
            }

            return sublevels;
        }

        /// <summary>
        /// Generate sample store items.
        /// </summary>
        /// <param name="quantity">Quanity to generate.</param>
        /// <param name="level">Level to use.</param>
        protected virtual void AddGeneratedStoreItems(int quantity, StoreItemLevel level)
        {
            if (level != null)
            {
                StoreItem sampleItem = null;

                for (int i = 0; i < quantity; i++)
                {
                    storeItemNumber++;

                    sampleItem = new StoreItem()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = string.Format("Name {0}", storeItemNumber),
                        ShortDescription = "Short description " + storeItemNumber,
                        LongDescription = "Long description " + storeItemNumber,
                        ThumbnailImagePath = string.Format("~/sample{0}-t.jpg", (i + 1).Clamp(1, 9)),
                        MainImagePath = string.Format("~/MP_Default-m.jpg"),
                        Brand = "Brand " + (i + 1),
                        Department = level.Department,
                        Category = level.Category,
                        Subcategory = level.Subcategory,
                        IsFeautred = storeItemNumber < 10
                    };

                    sampleItem.ImagesPaths = new List<string>();

                    sampleItem.ImagesPaths.Add(string.Format("~/sample{0}-1.jpg", (i + 1).Clamp(1, 9)));

                    storeItemsCache.Add(sampleItem);
                }
            }
        }

        /// <summary>
        /// Init sample data.
        /// </summary>
        protected virtual void InitSampleData()
        {
            if (!sampleDataInitilized)
            {
                StoreItemLevel sampleLevel = null;

                sublevelsCache.Clear();

                for (int i = 0; i < 6; i++)
                {
                    sampleLevel = new StoreItemLevel()
                    {
                        Department = "Department " + (i + 1)
                    };

                    sublevelsCache.Add(sampleLevel);
                    AddGeneratedStoreItems(2, sampleLevel);

                    for (int j = 0; j < 3; j++)
                    {
                        sampleLevel = new StoreItemLevel()
                        {
                            Department = "Department " + (i + 1),
                            Category = "Category " + (j + 1)
                        };

                        sublevelsCache.Add(sampleLevel);
                        AddGeneratedStoreItems(2, sampleLevel);

                        if (i == 0 && j == 0)
                        {
                            for (int k = 0; k < 4; k++)
                            {
                                sampleLevel = new StoreItemLevel()
                                {
                                    Department = "Department " + (i + 1),
                                    Category = "Category " + (j + 1),
                                    Subcategory = "SubCategory " + (k + 1)
                                };

                                sublevelsCache.Add(sampleLevel);
                                AddGeneratedStoreItems(5, sampleLevel);
                            }
                        }
                    }
                }

                sampleDataInitilized = true;
            }
        }
    }
}