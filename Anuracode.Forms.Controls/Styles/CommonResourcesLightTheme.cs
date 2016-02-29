// <copyright file="CommonResourcesLightTheme.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Xamarin.Forms;

namespace Anuracode.Forms.Controls.Styles
{
    /// <summary>
    /// Common resources light theme.
    /// </summary>
    public class CommonResourcesLightTheme : CommonResourcesBase
    {
        /// <summary>
        /// Default text color.
        /// </summary>
        public override Color DefaultLabelSubtitleTextColor
        {
            get
            {
                return Color.FromHex("333333");
            }
        }

        /// <summary>
        /// Default text color.
        /// </summary>
        public override Color DefaultLabelTextColor
        {
            get
            {
                return Color.FromHex("333333");
            }
        }

        /// <summary>
        /// Pages background color.
        /// </summary>
        public override Color PagesBackgroundColor
        {
            get
            {
                return Color.FromHex("F6F6F6");
            }
        }

        /// <summary>
        /// Pages background color light.
        /// </summary>
        public override Color PagesBackgroundColorLight
        {
            get
            {
                return Color.White;
            }
        }

        /// <summary>
        /// Subtile color.
        /// </summary>
        public override Color SubtleColor
        {
            get
            {
                return new Color(Color.Black.R, Color.Black.G, Color.Black.B, 0.7);
            }
        }
    }
}