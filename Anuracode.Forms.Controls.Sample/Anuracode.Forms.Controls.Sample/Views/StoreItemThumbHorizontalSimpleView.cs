// <copyright file="StoreItemThumbHorizontalSimpleView.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Extensions;
using Anuracode.Forms.Controls.Views.Extensions;
using Anuracode.Forms.Controls.Sample.ViewModels;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls.Sample.Views
{
    /// <summary>
    /// Thumb view featured.
    /// </summary>
    public class StoreItemThumbHorizontalSimpleView : StoreItemThumbView
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public StoreItemThumbHorizontalSimpleView()
            : this(true)
        {
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StoreItemThumbHorizontalSimpleView(bool autoInit)
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
        /// Flag if has description.
        /// </summary>
        public override bool HasNewStack
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Background shape, circle.
        /// </summary>
        protected ShapeView BackgroundShapeFeaturedLeft { get; set; }

        /// <summary>
        /// Background shape, circle.
        /// </summary>
        protected ShapeView BackgroundShapeLeft { get; set; }

        /// <summary>
        /// Background shape, circle.
        /// </summary>
        protected ShapeView BackgroundShapeNewLeft { get; set; }

        /// <summary>
        /// Flag when the thumb has border.
        /// </summary>
        protected override bool HasThumbBorder
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
        /// Flag when the new is rotated.
        /// </summary>
        protected override bool IsNewRotated
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Layout for the image.
        /// </summary>
        protected SimpleLayout LeftDecorationLayout { get; set; }

        /// <summary>
        /// View orientation.
        /// </summary>
        protected override StackOrientation Orientation
        {
            get
            {
                return StackOrientation.Horizontal;
            }
        }

        /// <summary>
        /// Page background color.
        /// </summary>
        protected override Color PageBackgroundColor
        {
            get
            {
                return Theme.CommonResources.PagesBackgroundColorLight;
            }
        }

        /// <summary>
        /// Brand image width.
        /// </summary>
        protected override double ThumbImageWidth
        {
            get
            {
                return Theme.CommonResources.CategoryImageWidth * 0.5;
            }
        }

        /// <summary>
        /// Add the control to the layout in the proper order.
        /// </summary>
        protected override void AddControlsToLayout()
        {
            AddViewToLayout(BackgroundBox);
            AddViewToLayout(BackgroundImage);
            AddViewToLayout(BackgroundShape);
            AddViewToLayout(BackgroundBorder);

            AddViewToLayout(ThumbBackgroundShape, ImageLayout);
            AddViewToLayout(ThumbImage, ImageLayout);
            AddViewToLayout(NewLabel, ImageLayout);
            AddViewToLayout(ThumbNormalBorder, ImageLayout);
            AddViewToLayout(ThumbFeaturedBorder, ImageLayout);

            AddViewToLayout(ImageLayout);

            AddViewToLayout(BackgroundShapeNewLeft, LeftDecorationLayout);
            AddViewToLayout(BackgroundShapeLeft, LeftDecorationLayout);
            AddViewToLayout(BackgroundShapeFeaturedLeft, LeftDecorationLayout);
            AddViewToLayout(LeftDecorationLayout);

            AddViewToLayout(ProductNameLabel);
            AddViewToLayout(ProductPriceLabel);
            AddViewToLayout(ProductShortDescriptionLabel);
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
            var borderDistance = BackgroundBorderWidth * 1.05;
            Rectangle layoutSize = new Rectangle(x, y, width, height);
            Rectangle layoutPartialBorderSize = new Rectangle(x + borderDistance, y + borderDistance, width - (borderDistance * 2f), height - (borderDistance * 2f));

            Rectangle imagePosition = new Rectangle();
            Rectangle pricePosition = new Rectangle();
            Rectangle namePosition = new Rectangle();
            Rectangle decorationPosition = new Rectangle();

            if (ImageLayout != null)
            {
                var imageSize = ImageLayout.Measure(width, height).Request;
                double elementWidth = imageSize.Width;
                double elementHeight = imageSize.Height;
                double elementLeft = ContentMargin;
                double elementTop = (height - elementHeight) * 0.5f;

                imagePosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                ImageLayout.LayoutUpdate(imagePosition);
            }

            if (ProductPriceLabel != null)
            {
                var elementMargin = ContentMargin * 0.5f;
                var elementSize = ProductPriceLabel.Measure(Theme.CommonResources.CartPriceWidth, height - (elementMargin * 2f)).Request;
                double elementHeight = elementSize.Height;
                double elementWidth = elementSize.Width;
                double elementLeft = width - elementWidth - (ContentMargin * 2f);
                double elementTop = (height - elementHeight) * 0.5f;

                pricePosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                ProductPriceLabel.LayoutUpdate(pricePosition);
            }

            if (BackgroundBox != null)
            {
                double elementLeft = BackgroundBorderWidth;
                double elementTop = BackgroundBorderWidth;
                double elementWidth = width - (BackgroundBorderWidth * 2f);
                double elementHeight = height - (BackgroundBorderWidth * 2f);

                var elementPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                BackgroundBox.LayoutUpdate(elementPosition);
            }

            if (LeftDecorationLayout != null)
            {
                double elementLeft = width - Theme.CommonResources.CartPriceWidth - (ContentMargin * 2f) - BackgroundBorderWidth;
                double elementTop = BackgroundBorderWidth;
                double elementWidth = width - elementLeft - BackgroundBorderWidth;
                double elementHeight = height - (BackgroundBorderWidth * 2f);

                decorationPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                LeftDecorationLayout.LayoutUpdate(decorationPosition);
            }

            if (ProductNameLabel != null)
            {
                var elementMargin = ContentMargin * 0.5f;
                double elementLeft = imagePosition.X + imagePosition.Width + (ContentMargin * 2f);
                double elementWidth = decorationPosition.X - elementLeft;
                var elementSize = ProductNameLabel.Measure(elementWidth, height - (elementMargin * 2f)).Request;
                double elementHeight = elementSize.Height;
                double elementTop = (height - elementHeight) * 0.5f;

                namePosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                ProductNameLabel.LayoutUpdate(namePosition);
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
            double contentMaxWidth = ThumbImageWidth + (ContentMargin * 5f) + (BackgroundBorderWidth * 2f) + Theme.CommonResources.CartPriceWidth;
            double minContentWidth = widthConstraint.Clamp(0, 4000) - contentMaxWidth.Clamp(0, widthConstraint);
            double maxHeight = ((BackgroundBorderWidth * 4f) + ThumbImageWidth).Clamp(0, heightConstraint);
            double minWidth = contentMaxWidth;

            SizeRequest resultRequest = new SizeRequest(new Size(widthConstraint.Clamp(0, 4000), maxHeight.Clamp(0, 4000)), new Size(minWidth.Clamp(0, 4000), maxHeight.Clamp(0, 4000)));

            return resultRequest;
        }

        /// <summary>
        /// Internal initialze view.
        /// </summary>
        protected override void InternalInitializeView()
        {
            base.InternalInitializeView();

            // Image layout.
            LeftDecorationLayout = new SimpleLayout()
            {
                IsHandlingLayoutManually = true,
                Padding = 0,
                IsClippedToBounds = true
            };

            LeftDecorationLayout.OnLayoutChildren += LeftDecorationLayout_OnLayoutChildren;
            LeftDecorationLayout.ManualSizeCalculationDelegate = LeftDecorationLayout_OnSizeRequest;

            if (ProductNameLabel != null)
            {
                ProductNameLabel.LineBreakMode = LineBreakMode.WordWrap;
                ProductNameLabel.FontSize = Theme.CommonResources.TextSizeSmall;
            }

            if (ProductPriceLabel != null)
            {
                ProductPriceLabel.TextColor = Color.White;
                ProductPriceLabel.FontSize = Theme.CommonResources.TextSizeSmall;
                ProductPriceLabel.LineBreakMode = LineBreakMode.WordWrap;
                ProductPriceLabel.HorizontalTextAlignment = TextAlignment.End;
            }

            BackgroundShapeLeft = new ShapeView()
            {
                Color = Theme.CommonResources.AccentAlternative,
                ShapeType = ShapeType.Circle
            };

            BackgroundShapeFeaturedLeft = new ShapeView()
            {
                Color = Theme.CommonResources.Accent,
                ShapeType = ShapeType.Circle
            };

            BackgroundShapeNewLeft = new ShapeView()
            {
                Color = Theme.CommonResources.ItemTrackingStatusNewColor,
                ShapeType = ShapeType.Circle
            };

            BackgroundBox = new BoxView()
            {
                Color = Color.White
            };
        }

        /// <summary>
        /// Layout children.
        /// </summary>
        /// <param name="x">Top to use.</param>
        /// <param name="y">Left to use.</param>
        /// <param name="width">Width to use.</param>
        /// <param name="height">Height to use.</param>
        protected virtual void LeftDecorationLayout_OnLayoutChildren(double x, double y, double width, double height)
        {
            if (BackgroundShapeLeft != null)
            {
                double elementWidth = width * 4f;
                double elementHeight = elementWidth;
                double elementLeft = ContentMargin;
                double elementTop = (height - elementHeight) * 0.55f;

                var elementPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                BackgroundShapeLeft.LayoutUpdate(elementPosition);
            }

            if (BackgroundShapeFeaturedLeft != null)
            {
                double elementWidth = width * 4f;
                double elementHeight = elementWidth;
                double elementLeft = ContentMargin;
                double elementTop = (height - elementHeight) * 0.55f;

                var elementPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                BackgroundShapeFeaturedLeft.LayoutUpdate(elementPosition);
            }

            if (BackgroundShapeNewLeft != null)
            {
                double elementWidth = width * 4f;
                double elementHeight = elementWidth;
                double elementLeft = 0;
                double elementTop = (height - elementHeight) * 0.55f;

                var elementPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                BackgroundShapeNewLeft.LayoutUpdate(elementPosition);
            }
        }

        /// <summary>
        /// Mesure the content.
        /// </summary>
        /// <param name="widthConstraint">Width to constarint.</param>
        /// <param name="heightConstraint">Height to constraint.</param>
        /// <returns></returns>
        protected virtual SizeRequest LeftDecorationLayout_OnSizeRequest(double widthConstraint, double heightConstraint)
        {
            double contentMaxWidth = (ContentMargin * 3f) + (BackgroundBorderWidth * 2f) + Theme.CommonResources.CartPriceWidth;
            double maxHeight = ((BackgroundBorderWidth * 2f) + ThumbImageWidth).Clamp(0, heightConstraint);
            double minWidth = contentMaxWidth;

            SizeRequest resultRequest = new SizeRequest(new Size(widthConstraint.Clamp(0, 4000), maxHeight.Clamp(0, 4000)), new Size(minWidth.Clamp(0, 4000), maxHeight.Clamp(0, 4000)));

            return resultRequest;
        }

        /// <summary>
        /// Setup the bindings of the elements.
        /// </summary>
        protected override void SetupBindings()
        {
            if (ThumbNormalBorder != null)
            {
                ThumbNormalBorder.SetBinding(ShapeView.IsVisibleProperty, "Item.IsFeautred", converter: Theme.CommonResources.InvertBooleanToBooleanConverter);
            }

            if (ThumbFeaturedBorder != null)
            {
                ThumbFeaturedBorder.SetBinding(ShapeView.IsVisibleProperty, "Item.IsFeautred");
            }

            if (ThumbImage != null)
            {
                ThumbImage.SetBinding(Image.SourceProperty, "BrandImagePath");
            }

            if (NewLabel != null)
            {
                NewLabel.SetBinding(View.IsVisibleProperty, "Item.IsNew");
            }

            if (BackgroundShapeNewLeft != null)
            {
                BackgroundShapeNewLeft.SetBinding(View.IsVisibleProperty, "Item.IsNew");
            }

            if (BackgroundShapeFeaturedLeft != null)
            {
                BackgroundShapeFeaturedLeft.SetBinding(View.IsVisibleProperty, "Item.IsFeautred");
            }

            if (ProductPriceLabel != null)
            {
                ProductPriceLabel.Text = "$0";
            }

            if (ProductNameLabel != null)
            {
                ProductNameLabel.SetBinding(Label.TextProperty, "Item.Name");
            }
        }
    }
}