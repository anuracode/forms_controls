// <copyright file="CommonResourcesBase.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Extensions;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls.Styles
{
    /// <summary>
    /// Base resources.
    /// </summary>
    public partial class CommonResourcesBase
    {
        /// <summary>
        /// Font friendly name.
        /// </summary>
        public string FontIconMoonFriendlyName = "IcoMoon-Free";

        /// <summary>
        /// Font name.
        /// </summary>
        public string FontIconMoonName = "IcoMoon-Free.ttf";

        /// <summary>
        /// Font friendly name.
        /// </summary>
        public string FontRobotBoldCondensedFriendlyName = "Roboto Bold Condensed";

        /// <summary>
        /// Font name.
        /// </summary>
        public string FontRobotBoldCondensedName = "Roboto-BoldCondensed.ttf";

        /// <summary>
        /// Font friendly name.
        /// </summary>
        public string FontRobotCondensedFriendlyName = "Roboto Condensed";

        /// <summary>
        /// Font name.
        /// </summary>
        public string FontRobotCondensedName = "Roboto-Condensed.ttf";

        /// <summary>
        /// Font friendly name.
        /// </summary>
        public string FontRobotLightFriendlyName = "Roboto Light";

        /// <summary>
        /// Font name.
        /// </summary>
        public string FontRobotLightName = "Roboto-Light.ttf";

        /// <summary>
        /// Font friendly name.
        /// </summary>
        public string FontRobotMediumFriendlyName = "Roboto Medium";

        /// <summary>
        /// Font name.
        /// </summary>
        public string FontRobotMediumName = "Roboto-Medium.ttf";

        /// <summary>
        /// Font friendly name.
        /// </summary>
        public string FontRobotRegularFriendlyName = "Roboto Regular";

        /// <summary>
        /// Font name.
        /// </summary>
        public string FontRobotRegularName = "Roboto-Regular.ttf";

        /// <summary>
        /// Font friendly name.
        /// </summary>
        public string FontSegoeUISymbolFriendlyName = "Segoe UI Symbol";

        /// <summary>
        /// Font name.
        /// </summary>
        public string FontSegoeUISymbolName = "seguisym.ttf";

        /// <summary>
        /// Application name.
        /// </summary>
        public virtual string ApplicationTitle
        {
            get
            {
                return "ApplicationTitle";
            }
        }

        /// <summary>
        /// Glyph friendly name.
        /// </summary>
        public virtual string GlyphFontName
        {
            get
            {
                return FontSegoeUISymbolName;
            }
        }

        /// <summary>
        /// Glyph friendly name.
        /// </summary>
        public virtual string GlyphFontNameAlternate
        {
            get
            {
                return FontSegoeUISymbolFriendlyName;
            }
        }

        /// <summary>
        /// Glyph friendly name.
        /// </summary>
        public virtual string GlyphFriendlyFontName
        {
            get
            {
                return FontIconMoonName;
            }
        }

        /// <summary>
        /// Glyph friendly name.
        /// </summary>
        public virtual string GlyphFriendlyFontNameAlternate
        {
            get
            {
                return FontIconMoonFriendlyName;
            }
        }

        /// <summary>
        /// Text size.
        /// </summary>
        public virtual double TextSizeLarge
        {
            get
            {
                return GetNamedSize(NamedSize.Large) * TextSizeZoomLevel;
            }
        }

        /// <summary>
        /// Text size.
        /// </summary>
        public virtual double TextSizeMedium
        {
            get
            {
                return GetNamedSize(NamedSize.Medium) * TextSizeZoomLevel;
            }
        }

        /// <summary>
        /// Text size.
        /// </summary>
        public virtual double TextSizeMicro
        {
            get
            {
                return GetNamedSize(NamedSize.Micro) * TextSizeZoomLevel;
            }
        }

        /// <summary>
        /// Text size.
        /// </summary>
        public virtual double TextSizeSmall
        {
            get
            {
                return GetNamedSize(NamedSize.Small) * TextSizeZoomLevel;
            }
        }

        /// <summary>
        /// Text size zoom level.
        /// </summary>
        public virtual double TextSizeZoomLevel
        {
            get
            {
                if (Device.OS == TargetPlatform.iOS && Device.Idiom == TargetIdiom.Phone)
                {
                    return 0.8;
                }
                if (Device.OS == TargetPlatform.Android && Device.Idiom == TargetIdiom.Phone)
                {
                    return 0.8;
                }
                else if (Device.OS == TargetPlatform.Windows)
                {
                    return 0.9;
                }
                else
                {
                    return 0.9;
                }
            }
        }

        /// <summary>
        /// Get named size.
        /// </summary>
        /// <param name="namedSize">Named size.</param>
        /// <returns>Size to use.</returns>
        protected double GetNamedSize(NamedSize namedSize)
        {
            double fontSize = 0;

            switch (namedSize)
            {
                case NamedSize.Large:
                    fontSize = Device.OS.OnPlatform(28, 28, 28, 32, 32);
                    break;

                case NamedSize.Micro:
                    fontSize = Device.OS.OnPlatform(13, 13, 13, 15, 15);
                    break;

                case NamedSize.Small:
                    fontSize = Device.OS.OnPlatform(16, 16, 16, 18, 18);
                    break;

                case NamedSize.Default:
                case NamedSize.Medium:
                default:
                    fontSize = Device.OS.OnPlatform(18, 18, 18, 22, 22);
                    break;
            }

            return fontSize;
        }
    }
}