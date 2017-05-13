// <copyright file="ExtendedImage.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Xamarin.Forms;

namespace Anuracode.Forms.Controls
{
    /// <summary>
    /// Extended image.
    /// </summary>
    public class ExtendedImage : Image
    {
        /// <summary>
		/// The loading placeholder property.
		/// </summary>
        public static readonly BindableProperty LoadingPlaceholderProperty = BindableProperty.Create(nameof(LoadingPlaceholder), typeof(ImageSource), typeof(ExtendedImage), default(ImageSource));

        /// <summary>
        /// Prefix for platform.
        /// </summary>
        private const string PREFIX_ANDROID = "";

        /// <summary>
        /// Prefix for platform.
        /// </summary>
        private const string PREFIX_IOS = "Images/";

        /// <summary>
        /// Prefix for platform.
        /// </summary>
        private const string PREFIX_WP = "Resources/Images/";

        /// <summary>
        /// Gets or sets the loading placeholder image.
        /// </summary>
        [TypeConverter(typeof(ImageSourceConverter))]
        public ImageSource LoadingPlaceholder
        {
            get
            {
                return (ImageSource)GetValue(LoadingPlaceholderProperty);
            }
            set
            {
                SetValue(LoadingPlaceholderProperty, value);
            }
        }

        /// <summary>
        /// Extension to complete the prefix of the images for the device.
        /// </summary>
        /// <param name="imagePath">Image path to use.</param>
        /// <returns>New path to use.</returns>
        public static string CompleteImagePrefix(string imagePath)
        {
            string returnPath = string.Empty;

            switch (Device.OS)
            {
                case TargetPlatform.iOS:
                    returnPath = (PREFIX_IOS + imagePath).Trim();
                    break;

                case TargetPlatform.Android:
                    returnPath = (PREFIX_ANDROID + imagePath).Trim();
                    break;

                case TargetPlatform.WinPhone:
                case TargetPlatform.Windows:
                case TargetPlatform.Other:
                default:
                    returnPath = (PREFIX_WP + imagePath).Trim();
                    break;
            }

            return returnPath;
        }
    }
}