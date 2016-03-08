// <copyright file="StoreItemThumbFeaturedVerticalView.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Xamarin.Forms;

namespace Anuracode.Forms.Controls.Sample.Views
{
    /// <summary>
    /// Thumb view featured.
    /// </summary>
    public class StoreItemThumbFeaturedVerticalView : StoreItemThumbView
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public StoreItemThumbFeaturedVerticalView()
            : this(true)
        {
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StoreItemThumbFeaturedVerticalView(bool autoInit)
            : base(autoInit)
        {
        }

        /// <summary>
        /// Decoration color.
        /// </summary>
        public Color DecorationColor
        {
            get
            {
                return BackgroundShape == null ? Color.Transparent : BackgroundShape.Color;
            }

            set
            {
                if (BackgroundShape != null)
                {
                    BackgroundShape.Color = value;
                }
            }
        }

        /// <summary>
        /// Flag to add the background elements.
        /// </summary>
        protected override bool HasBackground
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Is featured.
        /// </summary>
        protected override bool IsFeatured
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// View orientation.
        /// </summary>
        protected override StackOrientation Orientation
        {
            get
            {
                return StackOrientation.Vertical;
            }
        }
    }
}