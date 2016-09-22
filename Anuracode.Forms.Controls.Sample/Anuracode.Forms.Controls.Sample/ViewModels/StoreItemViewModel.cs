// <copyright file="StoreItemViewModel.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Sample.Model;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Anuracode.Forms.Controls.Sample.ViewModels
{
    /// <summary>
    /// View model for a store item.
    /// Used for binding images.
    /// </summary>
    /// <summary>
    /// View model for a store item.
    /// Used for binding images.
    /// </summary>
    public class StoreItemViewModel : BaseEntity
    {
        /// <summary>
        /// Default thumb image path.
        /// </summary>
        private string defaultBrandThumbImagePath = Theme.CommonResources.PathImageAppLogoLarge;

        /// <summary>
        /// Default image path.
        /// </summary>
        private string defaultImagePath = Theme.CommonResources.PathImageAppLogoLarge;

        /// <summary>
        /// Default thumb image path.
        /// </summary>
        private string defaultThumbImagePath = Theme.CommonResources.PathImageAppLogoLarge;

        /// <summary>
        /// Elemets to display.
        /// </summary>
        private ObservableCollectionFast<string> images = new ObservableCollectionFast<string>();

        /// <summary>
        /// Item to use.
        /// </summary>
        private StoreItem item;

        /// <summary>
        /// Item level.
        /// </summary>
        private StoreItemLevel itemLevel;

        /// <summary>
        /// Loading thumb image.
        /// </summary>
        private string loadingThumbImagePath = Theme.CommonResources.PathImageAppLogoLarge;

        /// <summary>
        /// Constructor with group.
        /// </summary>
        /// <param name="storeItem">Item to use.</param>
        public StoreItemViewModel(StoreItem storeItem)
        {
            PrepareViewModel(storeItem);
        }

        /// <summary>
        /// Thumbnail image path.
        /// </summary>
        public string BrandImagePath { get; protected set; }

        /// <summary>
        /// Elemets to display.
        /// </summary>
        public ObservableCollectionFast<string> Images
        {
            get
            {
                return images;
            }
        }

        /// <summary>
        /// This view model is always readonly.
        /// </summary>
        public override bool IsReadOnly
        {
            get
            {
                return true;
            }
            set
            {
            }
        }

        /// <summary>
        /// Item to use.
        /// </summary>
        public StoreItem Item
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
        /// Item level.
        /// </summary>
        public StoreItemLevel ItemLevel
        {
            get
            {
                return itemLevel;
            }

            protected set
            {
                ValidateRaiseAndSetIfChanged(ref itemLevel, value);
            }
        }

        /// <summary>
        /// Main image path.
        /// </summary>
        public string MainImagePath { get; protected set; }

        /// <summary>
        /// Thumbnail image path.
        /// </summary>
        public string ThumbnailImagePath { get; protected set; }

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
        /// Load thumb images, the images will overwrite.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to use.</param>
        public void LoadBrandThumbImage(CancellationToken cancellationToken)
        {
            if (Item != null)
            {
                if (string.IsNullOrEmpty(Item.BrandImagePath))
                {
                    BrandImagePath = defaultBrandThumbImagePath;
                }
                else
                {
                    BrandImagePath = Item.BrandImagePathExtended;
                }
            }
        }

        /// <summary>
        /// Load  images, the images will overwrite.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to use.</param>
        public void LoadOtherImages(CancellationToken cancellationToken)
        {
            if ((Item != null) && (Item.ImagesPathsExtended != null) && (Images.Count != item.ImagesPathsExtended.Count))
            {
                List<string> tmpList = new List<string>();

                string imagePath = null;

                for (int i = 0; i < Item.ImagesPathsExtended.Count; i++)
                {
                    imagePath = Item.ImagesPathsExtended[i];

                    if (!string.IsNullOrEmpty(imagePath))
                    {
                        tmpList.Add(imagePath);
                    }
                }

                if (tmpList.Count > 0)
                {
                    Images.Reset(tmpList);
                }
                else
                {
                    try
                    {
                        Images.Clear();
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        // Skip becasue there is a bug in Xamarin.Forms.
                    }
                }
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
                if (string.IsNullOrEmpty(Item.ThumbnailImagePath))
                {
                    ThumbnailImagePath = defaultThumbImagePath;
                }
                else
                {
                    ThumbnailImagePath = Item.ThumbnailImagePathExtended;
                }
            }
        }

        /// <summary>
        /// Load  images, the images will overwrite.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to use.</param>
        protected void LoadMainImage(CancellationToken cancellationToken)
        {
            if (Item != null)
            {
                if (string.IsNullOrEmpty(Item.MainImagePath))
                {
                    MainImagePath = defaultImagePath;
                }
                else
                {
                    MainImagePath = item.MainImagePathExtended;
                }
            }
        }

        /// <summary>
        /// Prepare view model.
        /// </summary>
        /// <param name="storeItem">Store item to use.</param>
        protected void PrepareViewModel(StoreItem storeItem)
        {
            Item = storeItem;

            if (Item != null)
            {
                ItemLevel = new StoreItemLevel(Item);

                IsReadOnly = true;

                CancellationToken cancellationToken = CancellationToken.None;

                LoadBrandThumbImage(cancellationToken);
                LoadThumbImage(cancellationToken);
                LoadMainImage(cancellationToken);
                LoadOtherImages(cancellationToken);
            }
        }
    }
}