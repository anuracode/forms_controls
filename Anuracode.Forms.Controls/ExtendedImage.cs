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
        /// Extension to complete the prefix of the images for the device.
        /// </summary>
        /// <param name="imagePath">Image path to use.</param>
        /// <returns>New path to use.</returns>
        public static string CompleteImagePrefix(string imagePath)
        {
            return Device.OS == TargetPlatform.Windows ? (PREFIX_WP + imagePath).Trim() : Device.OnPlatform<string>(PREFIX_IOS + imagePath, PREFIX_ANDROID + imagePath, PREFIX_WP + imagePath).Trim();
        }
    }
}