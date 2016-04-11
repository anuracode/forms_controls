// <copyright file="SimpleRowBaseView.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Xamarin.Forms;
using Anuracode.Forms.Controls.Extensions;
using Anuracode.Forms.Controls.Views.Extensions;
using Anuracode.Forms.Controls.Styles;

namespace Anuracode.Forms.Controls
{
    /// <summary>
    /// Cell for an address.
    /// </summary>
    public abstract class SimpleRowBaseView : SimpleViewBase
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public SimpleRowBaseView()
            : base(true)
        {
        }

        /// <summary>
        /// Extra padding used in iOS grouped lists.
        /// </summary>
        public double RightExtraPadding { get; set; }

        /// <summary>
        /// Layout for the image.
        /// </summary>
        protected SimpleLayout InnerConentLayout { get; set; }

        /// <summary>
        /// Content margin.
        /// </summary>
        public override double ContentMargin
        {
            get
            {
                return 5;
            }

            set
            {
            }
        }

        /// <summary>
        /// Setup the bindings of the elements.
        /// </summary>
        protected override void SetupBindings()
        {
        }

        /// <summary>
        /// Add controls.
        /// </summary>
        protected override void AddControlsToLayout()
        {
            AddViewToLayout(InnerConentLayout);
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
            if (InnerConentLayout != null)
            {
                double elementLeft = ContentMargin;
                double elementTop = ContentMargin;
                double elementWidth = width - (ContentMargin * 2f) - RightExtraPadding;
                double elementHeight = height - (ContentMargin * 2f);

                var elementPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                InnerConentLayout.LayoutUpdate(elementPosition);
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
            double calculatedWidth = widthConstraint;

            Size innerContentSize = new Size();

            if (InnerConentLayout != null)
            {
                innerContentSize = InnerConentLayout.Measure(calculatedWidth - (ContentMargin * 2f) - RightExtraPadding, heightConstraint).Request;
            }

            double calculatedHeigh = innerContentSize.Height + (ContentMargin * 2f);
            return new SizeRequest(new Size(calculatedWidth.Clamp(0, 4000), calculatedHeigh.Clamp(0, 4000)), new Size(calculatedWidth.Clamp(0, 4000), calculatedHeigh.Clamp(0, 4000)));
        }

        /// <summary>
        /// Layout children.
        /// </summary>
        /// <param name="x">Top to use.</param>
        /// <param name="y">Left to use.</param>
        /// <param name="width">Width to use.</param>
        /// <param name="height">Height to use.</param>
        protected abstract void InnerConentLayout_OnLayoutChildren(double x, double y, double width, double height);

        /// <summary>
        /// Mesure the content.
        /// </summary>
        /// <param name="widthConstraint">Width to constarint.</param>
        /// <param name="heightConstraint">Height to constraint.</param>
        /// <returns></returns>
        protected abstract SizeRequest InnerContentLayout_OnSizeRequest(double widthConstraint, double heightConstraint);

        /// <summary>
        /// Initialize control.
        /// </summary>
        protected override void InternalInitializeView()
        {
            // Image layout.
            InnerConentLayout = new SimpleLayout()
            {
                IsHandlingLayoutManually = true,
                Padding = 0,
                IsClippedToBounds = true,
                BackgroundColor = ThemeManager.CommonResourcesBase.PagesBackgroundColorLight
            };

            InnerConentLayout.OnLayoutChildren += InnerConentLayout_OnLayoutChildren;
            InnerConentLayout.ManualSizeCalculationDelegate = InnerContentLayout_OnSizeRequest;
        }
    }
}