// <copyright file="CommonResources.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Xamarin.Forms;

namespace Anuracode.Forms.Controls.Sample.Views.Common
{
    /// <summary>
    /// Common resources for the app.
    /// </summary>
    public partial class CommonResources : Anuracode.Forms.Controls.Styles.CommonResourcesLightTheme
    {
        /// <summary>
        /// String to boolean converter.
        /// </summary>
        public StringToBooleanConverter StringToBooleanConverter = new StringToBooleanConverter();

        /// <summary>
        /// Path for the image.
        /// </summary>
        public string PathImageAppLogoLarge = ExtendedImage.CompleteImagePrefix("LogoApp256.png");

        /// <summary>
        /// Path for image.
        /// </summary>
        public string PathImageCancelAction = ExtendedImage.CompleteImagePrefix("appbar_cancel.png");

        /// <summary>
        /// Path for the image.
        /// </summary>
        public string PathImageFeaturesAction = ExtendedImage.CompleteImagePrefix("appbar_features.png");

        /// <summary>
        /// Path for the image.
        /// </summary>
        public string PathImageHambuergerLogo = ExtendedImage.CompleteImagePrefix("hamburgerlogo.png");

        /// <summary>
        /// Default entry text color.
        /// </summary>
        public virtual Color DefaultEntryTextColor
        {
            get
            {
                return Color.Black;
            }
        }
    }
}