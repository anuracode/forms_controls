// <copyright file="StoreItemViewModel.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Sample.Model;
using System.Threading;

namespace Anuracode.Forms.Controls.Sample.ViewModels
{
    /// <summary>
    /// View model for a store item.
    /// Used for binding images.
    /// </summary>
    public class StoreItemCartViewModel : BaseViewModel
    {
        /// <summary>
        /// Item to use.
        /// </summary>
        private StoreItemViewModel itemViewModel;        

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="storeItem">Store item to use.</param>
        /// <param name="storeItemCart">Reltion with the kart.</param>
        /// <param name="storeItemPrice">Store item price.</param>
        /// <param name="autoLoadPrice">True when the price should be loaded if storeItemPrice is null.</param>
        /// <param name="cancellationToken">Cancel token.</param>
        public StoreItemCartViewModel(StoreItem storeItem, CancellationToken cancellationToken)
        {
            if (storeItem != null)
            {
                ItemViewModel = new StoreItemViewModel(storeItem);
            }
        }

        /// <summary>
        /// Item to use.
        /// </summary>
        public StoreItemViewModel ItemViewModel
        {
            get
            {
                return itemViewModel;
            }

            protected set
            {
                ValidateRaiseAndSetIfChanged(ref itemViewModel, value);
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
            StoreItemCartViewModel compareValue = obj as StoreItemCartViewModel;
            if (compareValue == null)
            {
                return false;
            }
            else
            {
                if (compareValue.ItemViewModel != null && ItemViewModel != null)
                {
                    return ItemViewModel.Equals(compareValue.ItemViewModel);
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
            return ItemViewModel == null ? base.GetHashCode() : ItemViewModel.GetHashCode();
        }
    }
}