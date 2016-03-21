// <copyright file="StoreItemThumbOnlyView.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Xamarin.Forms;
using Anuracode.Forms.Controls.Views.Extensions;

namespace Anuracode.Forms.Controls.Sample.Views
{
    /// <summary>
    /// Thumb view featured.
    /// </summary>
    public class StoreItemThumbOnlyView : StoreItemThumbView
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public StoreItemThumbOnlyView()
            : this(true)
        {
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StoreItemThumbOnlyView(bool autoInit)
            : base(autoInit)
        {
        }

        /// <summary>
        /// Flag if has description.
        /// </summary>
        public override bool HasDescription
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Flag to add the background elements.
        /// </summary>
        protected override bool HasBackground
        {
            get
            {
                return false;
            }
        }        

        /// <summary>
        /// Flag to add the product detail elements.
        /// </summary>
        protected override bool HasProductDetail
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Is featured.
        /// </summary>
        protected override bool IsFeatured
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Stroke color for the thumb border is the same as the page.
        /// </summary>
        protected override bool StrokeColorThumbPageBackground
        {
            get
            {
                return false;
            }
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
            if (ImageLayout != null)
            {
                var imageSize = ImageLayout.GetSizeRequest(width, height).Request;
                double elementLeft = 0;
                double elementHeight = imageSize.Height;
                double elementWidth = imageSize.Width;
                double elementTop = 0;

                var imagePosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                ImageLayout.LayoutUpdate(imagePosition);
            }
        }

        /// <summary>
        /// Mesure the content.
        /// </summary>
        /// <param name="widthConstraint">Width to constarint.</param>
        /// <param name="heightConstraint">Height to constraint.</param>
        /// <returns>Size to use.</returns>
        protected override SizeRequest ContentLayout_OnSizeRequest(double widthConstraint, double heightConstraint)
        {
            Size viewSize = new Size((Theme.CommonResources.PreviewImageWidth + BackgroundBorderWidth) * 2, (Theme.CommonResources.PreviewImageWidth + BackgroundBorderWidth) * 2);
            return new SizeRequest(viewSize, viewSize);
        }

        /// <summary>
        /// Mesure the content.
        /// </summary>
        /// <param name="widthConstraint">Width to constarint.</param>
        /// <param name="heightConstraint">Height to constraint.</param>
        /// <returns>Size to use.</returns>
        protected override SizeRequest ImageLayout_OnSizeRequest(double widthConstraint, double heightConstraint)
        {
            Size viewSize = new Size((Theme.CommonResources.PreviewImageWidth + BackgroundBorderWidth) * 2, (Theme.CommonResources.PreviewImageWidth + BackgroundBorderWidth) * 2);
            return new SizeRequest(viewSize, viewSize);
        }
    }
}