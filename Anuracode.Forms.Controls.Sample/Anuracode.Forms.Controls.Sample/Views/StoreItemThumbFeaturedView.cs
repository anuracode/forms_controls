// <copyright file="StoreItemThumbFeaturedView.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Xamarin.Forms;
using Anuracode.Forms.Controls.Extensions;

namespace Anuracode.Forms.Controls.Sample.Views
{
    /// <summary>
    /// Thumb view featured.
    /// </summary>
    public class StoreItemThumbFeaturedView : StoreItemThumbView
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public StoreItemThumbFeaturedView()
            : this(true)
        {
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StoreItemThumbFeaturedView(bool autoInit)
            : base(autoInit)
        {
        }

        /// <summary>
        /// Background extra border to cover the corners.
        /// </summary>
        protected View BackgroundExtraBorder { get; set; }

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
        /// Add the control to the layout in the proper order.
        /// </summary>
        protected override void AddControlsToLayout()
        {
            base.AddControlsToLayout();

            AddViewToLayout(BackgroundExtraBorder);
        }

        /// <summary>
        /// Layout children.
        /// </summary>
        /// <param name="x">Top to use.</param>
        /// <param name="y">Left to use.</param>
        /// <param name="width">Width to use.</param>
        /// <param name="height">Height to use.</param>
        protected override void ContentLayout_OnLayoutChildren(double x, double y, double width, double height)
        {
            base.ContentLayout_OnLayoutChildren(x, y, width, height);

            if (BackgroundExtraBorder != null)
            {
                double borderDiff = -(BackgroundBorderWidth * 1.5f);
                double elementLeft = borderDiff;
                double elementTop = borderDiff;
                double elementWidth = width - (borderDiff * 2f);
                double elementHeight = height - (borderDiff * 2f);

                var elementPostion = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                BackgroundExtraBorder.LayoutUpdate(elementPostion);
            }
        }

        /// <summary>
        /// Internal initialze view.
        /// </summary>
        protected override void InternalInitializeView()
        {
            base.InternalInitializeView();

            BackgroundExtraBorder = new ShapeView()
            {
                Color = Color.Transparent,
                ShapeType = ShapeType.Box,
                StrokeColor = StrokeColorPageBackground ? PageBackgroundColor : Theme.CommonResources.StrokeColorDefaultItem,
                CornerRadius = 10,
                StrokeWidth = BackgroundBorderWidth * 2f
            };
        }
    }
}
