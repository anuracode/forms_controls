// <copyright file="CommonResourcesBase.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Xamarin.Forms;

namespace Anuracode.Forms.Controls.Styles
{
    /// <summary>
    /// Base resources.
    /// </summary>
    public partial class CommonResourcesBase
    {
        /// <summary>
        /// Accent color darker.
        /// </summary>
        private Color accentDark = Color.Transparent;

        /// <summary>
        /// Accent color lighter.
        /// </summary>
        private Color accentLight = Color.Transparent;

        /// <summary>
        /// Accent color for the application.
        /// </summary>
        public virtual Color Accent
        {
            get
            {
                return Color.Accent;
            }
        }

        /// <summary>
        /// Accent alternative.
        /// </summary>
        public virtual Color AccentAlternative
        {
            get
            {
                return Color.FromHex("FF9E09");
            }
        }

        /// <summary>
        /// Accent color darker.
        /// </summary>
        public virtual Color AccentDark
        {
            get
            {
                if (accentDark == Color.Transparent)
                {
                    // accentDark = Color.FromHex("cc0000");
                    double constant = 0.95;
                    accentDark = new Color(Accent.R * constant, Accent.G * constant, Accent.B * constant);
                }

                return accentDark;
            }
        }

        /// <summary>
        /// Accent color lighter.
        /// </summary>
        public virtual Color AccentLight
        {
            get
            {
                if (accentLight == Color.Transparent)
                {
                    // accentLight = Color.FromHex("ff3333");
                    double constant = 1.1;
                    accentLight = new Color(Accent.R * constant, Accent.G * constant, Accent.B * constant);
                }

                return accentLight;
            }
        }

        /// <summary>
        /// Button background color.
        /// </summary>
        public virtual Color ButtonBackgroundColor
        {
            get
            {
                return Color.FromHex("f3f3f3").MultiplyAlpha(0.5);
            }
        }

        /// <summary>
        /// Default text color.
        /// </summary>
        public virtual Color DefaultLabelTextColor
        {
            get
            {
                return Color.White;
            }
        }

        /// <summary>
        /// Pages background color.
        /// </summary>
        public virtual Color PagesBackgroundColor
        {
            get
            {
                return Color.Black;
            }
        }

        /// <summary>
        /// Darker background page.
        /// </summary>
        public virtual Color PagesBackgroundColorLight
        {
            get
            {
                return PagesBackgroundColor.WithLuminosity(0.1);
            }
        }

        /// <summary>
        /// Rounded button width.
        /// </summary>
        public virtual double RoundedButtonWidth
        {
            get
            {
                if (Device.OS == TargetPlatform.iOS && Device.Idiom == TargetIdiom.Phone)
                {
                    return 30;
                }
                else
                {
                    return 40;
                }
            }
        }

        /// <summary>
        /// Small subtitle color.
        /// </summary>
        public virtual Color SmallSubtitleColor
        {
            get
            {
                if (Device.OS == TargetPlatform.iOS)
                {
                    return new Color(Color.Black.R, Color.Black.G, Color.Black.B, 0.75);
                }
                else
                {
                    return new Color(Color.White.R, Color.White.G, Color.White.B, 0.75);
                }
            }
        }

        /// <summary>
        /// Subtle background color for grouping panels.
        /// </summary>
        public virtual Color SubtleBackgroundColor
        {
            get
            {
                return new Color(Accent.R, Accent.G, Accent.B, 0.5);
            }
        }

        /// <summary>
        /// Subtle color.
        /// </summary>
        public virtual Color SubtleColor
        {
            get
            {
                if (Device.OS == TargetPlatform.iOS)
                {
                    return new Color(Color.Black.R, Color.Black.G, Color.Black.B, 0.7);
                }
                else
                {
                    return new Color(Color.White.R, Color.White.G, Color.White.B, 0.7);
                }
            }
        }

        /// <summary>
        /// Text color for the detail value.
        /// </summary>
        public virtual Color TextColorDetailValue
        {
            get
            {
                return Color.Gray;
            }
        }

        /// <summary>
        /// Text color disable.
        /// </summary>
        public virtual Color TextColorDisable
        {
            get
            {
                return Color.FromHex("999999");
            }
        }

        /// <summary>
        /// Text color forms errors.
        /// </summary>
        public virtual Color TextColorFormErrors
        {
            get
            {
                return AccentAlternative;
            }
        }

        /// <summary>
        /// Text color for links.
        /// </summary>
        public virtual Color TextColorLink
        {
            get
            {
                return AccentLight;
            }
        }

        /// <summary>
        /// Main menu text color.
        /// </summary>
        public virtual Color TextColorMainMenu
        {
            get
            {
                return Color.White;
            }
        }

        /// <summary>
        /// Section text color.
        /// </summary>
        public virtual Color TextColorSection
        {
            get
            {
                return Color.White;
            }
        }
    }
}