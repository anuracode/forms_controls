// <copyright file="PagedResult.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System.Collections.Generic;

namespace Anuracode.Forms.Controls.Sample.ViewModels
{
    /// <summary>
    /// Class that represents a paged result.
    /// </summary>
    /// <typeparam name="TItem">Item type to use.</typeparam>
    public class PagedResult<TItem>
    {
        /// <summary>
        /// Results paged.
        /// </summary>
        public IEnumerable<TItem> PagedItems { get; set; }

        /// <summary>
        /// Count of the total items in the result.
        /// </summary>
        public int TotalItemsCount { get; set; }
    }
}
