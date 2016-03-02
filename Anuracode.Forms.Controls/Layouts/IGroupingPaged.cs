// <copyright file="IGroupingPaged.cs" company="Anura Code">
// All rights reserved.
// </copyright>

using System.Collections;

namespace Anuracode.Forms.Controls
{
    /// <summary>
    /// Interface for grouping elements.
    /// </summary>
    public interface IGroupingPaged : IList
    {
        /// <summary>
        /// Group key.
        /// </summary>
        string Key { get; }

        /// <summary>
        /// Total items in group.
        /// </summary>
        int TotalGroup { get; }
    }
}