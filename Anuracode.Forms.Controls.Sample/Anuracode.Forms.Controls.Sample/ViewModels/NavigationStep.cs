// <copyright file="NavigationStep.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Sample.Model;

namespace Anuracode.Forms.Controls.Sample.ViewModels
{
    /// <summary>
    /// Navigation step.
    /// </summary>
    public class NavigationStep
    {
        /// <summary>
        /// Selected item for the group.
        /// </summary>
        public StoreItemViewModel CurrentLoadedGroup { get; set; }

        /// <summary>
        /// Filter term of the step.
        /// </summary>
        public string FilterTerm { get; set; }

        /// <summary>
        /// Flag for the cart.
        /// </summary>
        public bool IsCartVisible { get; set; }

        /// <summary>
        /// True when the list is a list of product not of categories.
        /// </summary>
        public bool IsFullSearchMode { get; set; }

        /// <summary>
        /// Flag for the large detail.
        /// </summary>
        public bool IsGroupDetailVisible { get; set; }

        /// <summary>
        /// Flag for the large detail.
        /// </summary>
        public bool IsLargeDetailVisible { get; set; }

        /// <summary>
        /// Level of the navigation step.
        /// </summary>
        public bool IsProductListMode { get; set; }

        /// <summary>
        /// Is search mode.
        /// </summary>
        public bool IsSearchMode { get; set; }

        /// <summary>
        /// Level of the navigation step.
        /// </summary>
        public StoreItemLevel Level { get; set; }

        /// <summary>
        /// Selected item for the detail viewmodel.
        /// </summary>
        public StoreItemViewModel SelectedDetailItemViewModel { get; set; }

        /// <summary>
        /// Selected item viewmodel.
        /// </summary>
        public StoreItemViewModel SelectedItemViewModel { get; set; }
    }
}