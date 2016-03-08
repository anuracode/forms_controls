// <copyright file="StoreItemThumbVerticalBarViewCell.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Xamarin.Forms;
using Anuracode.Forms.Controls.Extensions;

namespace Anuracode.Forms.Controls.Sample.Views
{
    /// <summary>
    /// Thumb vertial cell.
    /// </summary>
    public class StoreItemThumbVerticalBarViewCell : ViewCell
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public StoreItemThumbVerticalBarViewCell()
        {
            ContentLayout = new SimpleLayout()
            {
                IsHandlingLayoutManually = true,
                Padding = 0
            };

            ContentLayout.OnLayoutChildren += ContentLayout_OnLayoutChildren;
            ContentLayout.ManualSizeCalculationDelegate = ContentLayout_OnSizeRequest;

            InnerView = new StoreItemThumbVerticalBarView();

            ContentLayout.Children.Add(InnerView);

            this.View = ContentLayout;
        }

        /// <summary>
        /// Layout of the view.
        /// </summary>
        public SimpleLayout ContentLayout { get; set; }

        /// <summary>
        /// Margin to use.
        /// </summary>
        protected virtual double Margin
        {
            get
            {
                return 5f;
            }
        }

        /// <summary>
        ///
        /// </summary>
        private StoreItemThumbVerticalBarView InnerView { get; set; }

        /// <summary>
        /// Layout children.
        /// </summary>
        /// <param name="x">Top to use.</param>
        /// <param name="y">Left to use.</param>
        /// <param name="width">Width to use.</param>
        /// <param name="height">Height to use.</param>
        protected virtual void ContentLayout_OnLayoutChildren(double x, double y, double width, double height)
        {
            if (InnerView != null)
            {
                double elementLeft = Margin;
                double elementTop = Margin;
                double elementWidth = width - (Margin * 2f);
                double elementHeight = height - (Margin * 2f);

                var elementPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                InnerView.LayoutUpdate(elementPosition);
            }
        }

        /// <summary>
        /// Mesure the content.
        /// </summary>
        /// <param name="widthConstraint">Width to constarint.</param>
        /// <param name="heightConstraint">Height to constraint.</param>
        /// <returns>Size to use.</returns>
        protected virtual SizeRequest ContentLayout_OnSizeRequest(double widthConstraint, double heightConstraint)
        {
            if (InnerView != null)
            {
                return InnerView.GetSizeRequest(widthConstraint, heightConstraint);
            }
            else
            {
                SizeRequest resultRequest = new SizeRequest(new Size(widthConstraint, heightConstraint), new Size(widthConstraint, heightConstraint));
                return resultRequest;
            }
        }

        /// <summary>
        /// Binding context changed.
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            if (InnerView != null)
            {
                InnerView.PrepareBindings();
            }

            base.OnBindingContextChanged();

            if ((InnerView != null) && (InnerView.BindingContext != BindingContext))
            {
                InnerView.BindingContext = BindingContext;
            }
        }
    }
}