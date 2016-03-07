// <copyright file="CommonResources.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Extensions;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls.Sample.Views.Common
{
    /// <summary>
    /// Common resources for the app.
    /// </summary>
    public partial class CommonResources : Anuracode.Forms.Controls.Styles.CommonResourcesLightTheme
    {
        /// <summary>
        /// Invert value of a boolean.
        /// </summary>
        public InvertBooleanToBooleanConverter InvertBooleanToBooleanConverter = new InvertBooleanToBooleanConverter();

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
        /// Image width.
        /// </summary>
        public int PreviewImageWidth = Device.Idiom == TargetIdiom.Phone ? 40 : 80;

        /// <summary>
        /// String to boolean converter.
        /// </summary>
        public StringToBooleanConverter StringToBooleanConverter = new StringToBooleanConverter();

        /// <summary>
        /// Color for status.
        /// </summary>
        public Color ItemTrackingStatusNewColor = Color.FromHex("003E6D");

        /// <summary>
        /// Count button width.
        /// </summary>
        public virtual double CountButtonWidth
        {
            get
            {
                if (Device.OS == TargetPlatform.iOS)
                {
                    return Device.Idiom == TargetIdiom.Phone ? 15 : 20;
                }
                else
                {
                    return 25;
                }
            }
        }

        /// <summary>
        /// Stroke color featured item.
        /// </summary>
        public virtual Color StrokeColorFeaturedItem
        {
            get
            {
                return Accent;
            }
        }

        /// <summary>
        /// Background color item.
        /// </summary>
        public virtual Color BackgroundColorItem
        {
            get
            {
                return Color.White;
            }
        }

        /// <summary>
        /// Cart action button width.
        /// </summary>
        public virtual double CartActionButtonWidth
        {
            get
            {
                return (TextSizeSmall * 3).Clamp(RoundedButtonWidth, 60);
            }
        }

        /// <summary>
        /// Cart price width.
        /// </summary>
        public virtual double CartPriceWidth
        {
            get
            {
                return TextSizeSmall * 6;
            }
        }

        /// <summary>
        /// Category Image Width.
        /// </summary>
        public int CategoryImageWidth
        {
            get
            {
                if (Device.OS == TargetPlatform.iOS)
                {
                    return Device.Idiom == TargetIdiom.Phone ? 70 : 100;
                }
                else
                {
                    return Device.Idiom == TargetIdiom.Phone ? 80 : 100;
                }
            }
        }

        /// <summary>
        /// Color for confirm order.
        /// </summary>
        public Color ConfirmOrderColor
        {
            get
            {
                return Color.Accent;
            }
        }

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

        /// <summary>
        /// Stroke color default item.
        /// </summary>
        public virtual Color StrokeColorDefaultItem
        {
            get
            {
                return Color.Black;
            }
        }
    }
}