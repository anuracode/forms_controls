// <copyright file="StoreItem.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Anuracode.Forms.Controls.Sample.Model
{
    /// <summary>
    /// Represents a item in the store.
    /// </summary>
    [DataContract]
    public partial class StoreItem : BaseEntity
    {
        /// <summary>
        /// Default path short cut, used to save storage and transfer space.
        /// </summary>
        public const string DEFAULT_PATH_SHORTCUT = "~/";

        /// <summary>
        /// Default server image path.
        /// </summary>        
        public const string DEFAULT_SERVER_IMAGE_PATH = "http://storagetest.parcero.com.co/resources/";

        /// <summary>
        /// Maximum value.
        /// </summary>
        public const double MAX_FEATURED_WEIGHT = 10;

        /// <summary>
        /// Minimum value.
        /// </summary>
        public const double MIN_FEATURED_WEIGHT = 0;

        /// <summary>
        /// Brand of the product.
        /// </summary>
        private string brand;

        /// <summary>
        /// Url for the brand.
        /// </summary>
        private string brandImagePath = string.Empty;

        /// <summary>
        /// Category for the product.
        /// </summary>
        private string category;

        /// <summary>
        /// Department for the product.
        /// </summary>
        private string department;

        /// <summary>
        /// Weight used for sorting featured products.
        /// </summary>
        private double featuredWeight;

        /// <summary>
        /// Group for the product.
        /// </summary>
        private string group;

        /// <summary>
        /// Id of the parent store item of the group.
        /// </summary>
        private string groupParentId;

        /// <summary>
        /// Flag for a thumb full size.
        /// </summary>
        private bool hasFullSizeThumb;

        /// <summary>
        /// GUID of the element.
        /// </summary>
        private string id;

        /// <summary>
        /// Paths for the images.
        /// </summary>
        private List<string> imagesPaths = new List<string>();

        /// <summary>
        /// Paths for the images.
        /// </summary>
        private List<string> imagesPathsExtended;

        /// <summary>
        /// Featured product.
        /// </summary>
        private bool isFeautred;

        /// <summary>
        /// Flag if its group parent.
        /// </summary>
        private bool isGroupParent;

        /// <summary>
        /// Flag for new products.
        /// </summary>
        private bool isNew;

        /// <summary>
        /// Key words for the item.
        /// </summary>
        private string keywords;

        /// <summary>
        /// Link for more detail.
        /// </summary>
        private string link;

        /// <summary>
        /// Long description of the entity.
        /// </summary>
        private string longDescription = string.Empty;

        /// <summary>
        /// Url for the main image.
        /// </summary>
        private string mainImagePath = string.Empty;

        /// <summary>
        /// Name of the entity.
        /// </summary>
        private string name = string.Empty;

        /// <summary>
        /// Short description of the entity.
        /// </summary>
        private string shortDescription = string.Empty;

        /// <summary>
        /// Subcategory for the product.
        /// </summary>
        private string subcategory;

        /// <summary>
        /// Url for the thumbnail.
        /// </summary>
        private string thumbnailImagePath = string.Empty;

        /// <summary>
        /// Brand of the product.
        /// </summary>
        [DataMember]
        public string Brand
        {
            get
            {
                return brand;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref brand, string.IsNullOrEmpty(value) ? value : value.LetterCasingSentence());
            }
        }

        /// <summary>
        /// Url for the brand.
        /// </summary>
        [DataMember]
        public string BrandImagePath
        {
            get
            {
                return brandImagePath;
            }

            set
            {
                string transformedValue = value;

                if (!string.IsNullOrWhiteSpace(value))
                {
                    transformedValue = value.Replace(DEFAULT_SERVER_IMAGE_PATH, DEFAULT_PATH_SHORTCUT);
                }

                ValidateRaiseAndSetIfChanged(ref brandImagePath, transformedValue);

                OnPropertyChanged(nameof(BrandImagePathExtended));
            }
        }

        /// <summary>
        /// Url for the brand.
        /// </summary>
        public string BrandImagePathExtended
        {
            get
            {
                return BrandImagePath.Replace(DEFAULT_PATH_SHORTCUT, DEFAULT_SERVER_IMAGE_PATH);
            }
        }

        /// <summary>
        /// Category for the product.
        /// </summary>
        [DataMember]
        public string Category
        {
            get
            {
                return category;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref category, string.IsNullOrEmpty(value) ? value : value.LetterCasingSentence());
            }
        }

        /// <summary>
        /// Department for the product.
        /// </summary>
        [DataMember]
        public string Department
        {
            get
            {
                return department;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref department, string.IsNullOrEmpty(value) ? value : value.LetterCasingSentence());
            }
        }

        /// <summary>
        /// Weight used for sorting featured products.
        /// </summary>
        [DataMember]
        public double FeaturedWeight
        {
            get
            {
                return featuredWeight;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref featuredWeight, value);
            }
        }

        /// <summary>
        /// Group for the product.
        /// </summary>
        [DataMember]
        public string Group
        {
            get
            {
                return group;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref group, string.IsNullOrEmpty(value) ? value : value.LetterCasingSentence());
            }
        }

        /// <summary>
        /// Id of the parent store item of the group.
        /// </summary>
        [DataMember]
        public string GroupParentId
        {
            get
            {
                return groupParentId;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref groupParentId, value);

                OnPropertyChanged(nameof(IsGroupItem));
            }
        }

        /// <summary>
        /// Flag for a thumb full size.
        /// </summary>
        [DataMember]
        public bool HasFullSizeThumb
        {
            get
            {
                return hasFullSizeThumb;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref hasFullSizeThumb, value);
            }
        }

        /// <summary>
        /// GUID of the task.
        /// </summary>
        [DataMember]
        public string Id
        {
            get
            {
                return id;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref id, value);
            }
        }

        /// <summary>
        /// Paths for the images.
        /// </summary>
        [DataMember]
        public List<string> ImagesPaths
        {
            get
            {
                return imagesPaths;
            }

            set
            {
                List<string> transformedValue = value;

                if (value != null)
                {
                    transformedValue = new List<string>();

                    for (int i = 0; i < value.Count; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(value[i]))
                        {
                            transformedValue.Add(value[i].Replace(DEFAULT_SERVER_IMAGE_PATH, DEFAULT_PATH_SHORTCUT));
                        }
                    }
                }

                ValidateRaiseAndSetIfChanged(ref imagesPaths, transformedValue);

                OnPropertyChanged(nameof(ImagesPathsExtended));
            }
        }

        /// <summary>
        /// Paths for the images.
        /// </summary>
        public List<string> ImagesPathsExtended
        {
            get
            {
                if (((ImagesPaths != null) && (imagesPathsExtended == null)) || ((ImagesPaths != null) && (imagesPathsExtended != null) && (ImagesPaths.Count != imagesPathsExtended.Count)))
                {
                    imagesPathsExtended = new List<string>();

                    for (int i = 0; i < ImagesPaths.Count; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(ImagesPaths[i]))
                        {
                            imagesPathsExtended.Add(ImagesPaths[i].Replace(DEFAULT_PATH_SHORTCUT, DEFAULT_SERVER_IMAGE_PATH));
                        }
                    }
                }

                return imagesPathsExtended;
            }
        }

        /// <summary>
        /// Featured product.
        /// </summary>
        [DataMember]
        public bool IsFeautred
        {
            get
            {
                return isFeautred;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref isFeautred, value);
            }
        }

        /// <summary>
        /// Flag when the item is part of a group.
        /// </summary>
        [IgnoreDataMember]
        public bool IsGroupItem
        {
            get
            {
                return !string.IsNullOrWhiteSpace(GroupParentId);
            }
        }

        /// <summary>
        /// Flag if its group parent.
        /// </summary>
        [DataMember]
        public bool IsGroupParent
        {
            get
            {
                return isGroupParent;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref isGroupParent, value);
            }
        }

        /// <summary>
        /// Flag for new products.
        /// </summary>
        [DataMember]
        public bool IsNew
        {
            get
            {
                return isNew;
            }

            set
            {
                isNew = value;
            }
        }

        /// <summary>
        /// Key words for the item.
        /// </summary>
        [DataMember]
        public string Keywords
        {
            get
            {
                return keywords;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref keywords, value);
            }
        }

        /// <summary>
        /// Link for more detail.
        /// </summary>
        [DataMember]
        public string Link
        {
            get
            {
                return link;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref link, value);
            }
        }

        /// <summary>
        /// Long description of the entity.
        /// </summary>
        [DataMember]
        public string LongDescription
        {
            get
            {
                return longDescription;
            }

            set { ValidateRaiseAndSetIfChanged(ref longDescription, value); }
        }

        /// <summary>
        /// Url for the main image.
        /// </summary>
        [DataMember]
        public string MainImagePath
        {
            get
            {
                return mainImagePath;
            }

            set
            {
                string transformedValue = value;

                if (!string.IsNullOrWhiteSpace(value))
                {
                    transformedValue = value.Replace(DEFAULT_SERVER_IMAGE_PATH, DEFAULT_PATH_SHORTCUT);
                }

                ValidateRaiseAndSetIfChanged(ref mainImagePath, transformedValue);

                OnPropertyChanged(nameof(MainImagePathExtended));
            }
        }

        /// <summary>
        /// Url for the main image.
        /// </summary>
        public string MainImagePathExtended
        {
            get
            {
                return MainImagePath.Replace(DEFAULT_PATH_SHORTCUT, DEFAULT_SERVER_IMAGE_PATH);
            }
        }

        /// <summary>
        /// Name of the entity.
        /// </summary>
        [DataMember]
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref name, value);
            }
        }

        /// <summary>
        /// Name key.
        /// </summary>
        public string NameKey
        {
            get
            {
                string key = "?";

                if (!string.IsNullOrWhiteSpace(Name) && Name.Length > 0)
                {
                    key = Name[0].ToString().ToUpper();
                }

                return key;
            }
        }

        /// <summary>
        /// Short description of the entity.
        /// </summary>
        [DataMember]
        public string ShortDescription
        {
            get
            {
                return shortDescription;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref shortDescription, value);
            }
        }

        /// <summary>
        /// Subcategory for the product.
        /// </summary>
        [DataMember]
        public string Subcategory
        {
            get
            {
                return subcategory;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref subcategory, string.IsNullOrEmpty(value) ? value : value.LetterCasingSentence());
            }
        }

        /// <summary>
        /// Url for the thumb nail.
        /// </summary>
        [DataMember]
        public string ThumbnailImagePath
        {
            get
            {
                return thumbnailImagePath;
            }

            set
            {
                string transformedValue = value;

                if (!string.IsNullOrWhiteSpace(value))
                {
                    transformedValue = value.Replace(DEFAULT_SERVER_IMAGE_PATH, DEFAULT_PATH_SHORTCUT);
                }

                ValidateRaiseAndSetIfChanged(ref thumbnailImagePath, transformedValue);

                OnPropertyChanged(nameof(ThumbnailImagePathExtended));
            }
        }

        /// <summary>
        /// Url for the thumb image.
        /// </summary>
        public string ThumbnailImagePathExtended
        {
            get
            {
                return ThumbnailImagePath.Replace(DEFAULT_PATH_SHORTCUT, DEFAULT_SERVER_IMAGE_PATH);
            }
        }

        /// <summary>
        /// Check a level and determines if the product is in it.
        /// </summary>
        /// <param name="level">Level to use.</param>
        /// <returns>True when the item is in the level.</returns>
        public bool InLevel(StoreItemLevel level)
        {
            bool inLevel =
                (level != null) &&
                (string.IsNullOrEmpty(level.Department) || (!string.IsNullOrEmpty(level.Department) && !string.IsNullOrEmpty(Department) && (string.Compare(Department.Trim(), level.Department.Trim(), StringComparison.CurrentCultureIgnoreCase) == 0))) &&
                (string.IsNullOrEmpty(level.Category) || (!string.IsNullOrEmpty(level.Category) && !string.IsNullOrEmpty(Category) && (string.Compare(Category.Trim(), level.Category.Trim(), StringComparison.CurrentCultureIgnoreCase) == 0))) &&
                (string.IsNullOrEmpty(level.Subcategory) || (!string.IsNullOrEmpty(level.Subcategory) && !string.IsNullOrEmpty(Subcategory) && (string.Compare(Subcategory.Trim(), level.Subcategory.Trim(), StringComparison.CurrentCultureIgnoreCase) == 0)));

            return inLevel;
        }
    }
}