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
        /// Glyph all.
        /// </summary>
        public string GlyphTextAll = "\uE14C";

        /// <summary>
        /// Converter from int to boolean.
        /// </summary>
        public IntToBooleanConverter IntToBooleanConverter = new IntToBooleanConverter();

        /// <summary>
        /// Invert value of a boolean.
        /// </summary>
        public InvertBooleanToBooleanConverter InvertBooleanToBooleanConverter = new InvertBooleanToBooleanConverter();

        /// <summary>
        /// Color for status.
        /// </summary>
        public Color ItemTrackingStatusNewColor = Color.FromHex("003E6D");

        /// <summary>
        /// Path for the image.
        /// </summary>
        public string PathImageAppLogoLarge = ExtendedImage.CompleteImagePrefix("Icon-Small.png");

        /// <summary>
        /// Path for the image.
        /// </summary>
        public string PathImageFeaturesAction = ExtendedImage.CompleteImagePrefix("Icon-Small.png");

        /// <summary>
        /// Path for the image.
        /// </summary>
        public string PathImageHambuergerLogo = ExtendedImage.CompleteImagePrefix("Icon-Small.png");

        /// <summary>
        /// Path to profile background.
        /// </summary>
        public ImageSource PathImageProfileBackground = Model.StoreItem.DEFAULT_SERVER_IMAGE_PATH + "profile_background.jpg";

        /// <summary>
        /// Image width.
        /// </summary>
        public int PreviewImageWidth = Device.Idiom == TargetIdiom.Phone ? 40 : 80;

        /// <summary>
        /// Converter from StoreItemLevel to string of the lowest clasifier.
        /// </summary>
        public StoreItemLevelToLowerLevelStringConverter StoreItemLevelToLowerLevelStringConverter = new StoreItemLevelToLowerLevelStringConverter();

        /// <summary>
        /// String to boolean converter.
        /// </summary>
        public StringToBooleanConverter StringToBooleanConverter = new StringToBooleanConverter();

        /// <summary>
        /// User image with.
        /// </summary>
        public int UserImageWidth = 50;

        /// <summary>
        /// Height of a line.
        /// </summary>
        private double? lineHeight;

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
        /// Height of a line.
        /// </summary>
        public double LineHeight
        {
            get
            {
                if (!lineHeight.HasValue)
                {
                    lineHeight = Theme.CommonResources.TextSizeMicro * 1.205f;
                }

                return lineHeight.Value;
            }
        }

        /// <summary>
        /// Logo mirror.
        /// </summary>
        public ImageSource PathImageAppLogoLargeMirror = ExtendedImage.CompleteImagePrefix("Icon-Small.png");

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
    }
}