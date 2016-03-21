// <copyright file="StoreItemThumbFeaturedView.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System;
using Xamarin.Forms;
using Anuracode.Forms.Controls.Extensions;
using Anuracode.Forms.Controls.Views.Extensions;
using Anuracode.Forms.Controls.Sample.ViewModels;

namespace Anuracode.Forms.Controls.Sample.Views
{
    /// <summary>
    /// Thumb view featured.
    /// </summary>
    public class StoreItemThumbVerticalBarView : StoreItemThumbView
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public StoreItemThumbVerticalBarView()
            : this(true)
        {
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StoreItemThumbVerticalBarView(bool autoInit)
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
                return true;
            }
        }

        /// <summary>
        /// Flag when the thumb has border.
        /// </summary>
        protected override bool HasBackgroundBorder
        {
            get
            {
                return Device.OS.OnPlatform(true, false, true, true, true);
            }
        }

        /// <summary>
        /// Has native font strike throuth.
        /// </summary>
        protected bool HasNativeStrikeThrough
        {
            get
            {
                return Device.OS.OnPlatform(true, true, false, false, false);
            }
        }

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
        /// Is new rotated.
        /// </summary>
        protected override bool IsNewRotated
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

        /// <summary>
        /// Strike throught.
        /// </summary>
        protected BoxView PriceStrikeThrought { get; set; }

        /// <summary>
        /// Price of the product.
        /// </summary>
        protected Label ProductPriceWithoutDiscountLabel { get; set; }

        /// <summary>
        /// Add the control to the layout in the proper order.
        /// </summary>
        protected override void AddControlsToLayout()
        {
            AddViewToLayout(BackgroundBox);
            AddViewToLayout(ThumbImage);
            AddViewToLayout(NewLabel);
            AddViewToLayout(BackgroundShape, ImageLayout);
            AddViewToLayout(ImageLayout);

            AddViewToLayout(ProductNameLabel);
            AddViewToLayout(ProductPriceWithoutDiscountLabel);
            AddViewToLayout(PriceStrikeThrought);
            AddViewToLayout(ProductPriceLabel);

            AddViewToLayout(ThumbNormalBorder);
            AddViewToLayout(ThumbFeaturedBorder);
            AddViewToLayout(BackgroundBorder);
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
            Rectangle bottomBarPosition = new Rectangle();
            Rectangle priceNonDiscountPosition = new Rectangle();

            double maxLineHeight = LineHeight * 2f;

            if (BackgroundBox != null)
            {
                BackgroundBox.LayoutUpdate(layoutPartialBorderSize);
            }

            if (BackgroundBorder != null)
            {
                BackgroundBorder.LayoutUpdate(layoutSize);
            }

            if (ImageLayout != null)
            {
                double elementLeft = width * 0.4f;
                double elementTop = layoutPartialBorderSize.Y;
                double elementWidth = layoutPartialBorderSize.Width - elementLeft;
                double elementHeight = layoutPartialBorderSize.Height;

                decorationPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                ImageLayout.LayoutUpdate(decorationPosition);
            }

            if (ThumbImage != null)
            {
                double elementWidth = layoutPartialBorderSize.Width * 0.5f;
                double elementHeight = layoutPartialBorderSize.Height;
                double elementLeft = layoutPartialBorderSize.X;
                double elementTop = layoutPartialBorderSize.Y;

                imagePosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                ThumbImage.LayoutUpdate(imagePosition);
            }

            double maxContentWidth = (layoutPartialBorderSize.Width - decorationPosition.X) - (BackgroundBorderWidth * 2f);

            Size priceSize = new Size();
            Size nameSize = new Size();

            if (ProductPriceLabel != null)
            {
                priceSize = ProductPriceLabel.GetSizeRequest(maxContentWidth, maxLineHeight).Request;
            }

            if (ProductNameLabel != null)
            {
                nameSize = ProductNameLabel.GetSizeRequest(maxContentWidth, maxLineHeight).Request;
            }

            if (ProductNameLabel != null)
            {
                double elementHeight = nameSize.Height.Clamp(0, maxLineHeight);
                double elementWidth = nameSize.Width.Clamp(0, maxContentWidth);
                double elementLeft = decorationPosition.X + (((width - decorationPosition.X) - elementWidth) * 0.5f);
                double elementTop = (height * 0.5f) - elementHeight;

                if (elementHeight > (LineHeight * 1.5))
                {
                    elementTop = (height - (LineHeight * 3f)) * 0.5f;
                }

                namePosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                ProductNameLabel.LayoutUpdate(namePosition);
            }

            if (ProductPriceLabel != null)
            {
                double elementHeight = priceSize.Height.Clamp(0, maxLineHeight);
                double elementWidth = priceSize.Width.Clamp(0, maxContentWidth);
                double elementLeft = decorationPosition.X + (((width - decorationPosition.X) - elementWidth) * 0.5f);
                double elementTop = namePosition.Y + namePosition.Height;

                pricePosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                ProductPriceLabel.LayoutUpdate(pricePosition);
            }

            if (ProductPriceWithoutDiscountLabel != null)
            {
                var elementSize = ProductPriceWithoutDiscountLabel.GetSizeRequest(maxContentWidth, layoutPartialBorderSize.Height).Request;
                double elementHeight = elementSize.Height.Clamp(0, maxLineHeight);
                double elementWidth = elementSize.Width.Clamp(0, maxContentWidth);
                double elementLeft = decorationPosition.X + (((width - decorationPosition.X) - elementWidth) * 0.5f);
                double elementTop = pricePosition.Y + pricePosition.Height;

                priceNonDiscountPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                ProductPriceWithoutDiscountLabel.LayoutUpdate(priceNonDiscountPosition);
            }

            if (PriceStrikeThrought != null)
            {
                double elementHeight = 1;
                double elementWidth = priceNonDiscountPosition.Width;
                double elementLeft = priceNonDiscountPosition.X;
                double elementTop = priceNonDiscountPosition.Y + ((priceNonDiscountPosition.Height - elementHeight) * 0.5f);

                var elementPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                PriceStrikeThrought.LayoutUpdate(elementPosition);
            }

            if (ThumbNormalBorder != null)
            {
                double elementHeight = layoutPartialBorderSize.Height + Margin;
                double elementWidth = 5;
                double elementLeft = layoutPartialBorderSize.X;
                double elementTop = layoutPartialBorderSize.Y;

                bottomBarPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                ThumbNormalBorder.LayoutUpdate(bottomBarPosition);
            }

            if (ThumbFeaturedBorder != null)
            {
                ThumbFeaturedBorder.LayoutUpdate(bottomBarPosition);
            }

            if (NewLabel != null)
            {
                if (IsNewRotated)
                {
                    if (NewLabel.Rotation != 320)
                    {
                        NewLabel.Rotation = 320;
                    }

                    if (NewLabel.AnchorX != 0)
                    {
                        NewLabel.AnchorX = 0;
                    }

                    if (NewLabel.AnchorY != 0.5)
                    {
                        NewLabel.AnchorY = 0.5;
                    }
                }

                var elementSize = NewLabel.GetSizeRequest(layoutPartialBorderSize.Width, layoutPartialBorderSize.Height).Request;

                double elementLeft = BackgroundBorderWidth;
                double elementTop = BackgroundBorderWidth;
                double elementHeight = elementSize.Height + 6;
                double elementWidth = width - (BackgroundBorderWidth * 2f);

                if (IsNewRotated)
                {
                    elementWidth = elementSize.Width + (Margin * 6);
                    elementLeft = BackgroundBorderWidth - (elementWidth * (0.18f));
                    elementTop = BackgroundBorderWidth + (height * (0.33f));
                }

                var elementPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                NewLabel.LayoutUpdate(elementPosition);
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
            double widthRequest = 200 + (BackgroundBorderWidth * 2f);
            double heightRequest = 100 + (BackgroundBorderWidth * 2f);
            var resultRequest = new SizeRequest(new Size(widthRequest, heightRequest), new Size(widthRequest, heightRequest));

            return resultRequest;
        }

        /// <summary>
        /// Layout children.
        /// </summary>
        /// <param name="x">Top to use.</param>
        /// <param name="y">Left to use.</param>
        /// <param name="width">Width to use.</param>
        /// <param name="height">Height to use.</param>
        protected override void ImageLayout_OnLayoutChildren(double x, double y, double width, double height)
        {
            if (BackgroundShape != null)
            {
                double elementHeight = height * 2f;
                double elementWidth = elementHeight;
                double elementLeft = 0;
                double elementTop = (height - elementHeight) * 0.5f;

                var elementPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                BackgroundShape.LayoutUpdate(elementPosition);
            }
        }

        /// <summary>
        /// Mesure the content.
        /// </summary>
        /// <param name="widthConstraint">Width to constarint.</param>
        /// <param name="heightConstraint">Height to constraint.</param>
        /// <returns>Size to use.</returns>
        protected override SizeRequest ImageLayout_OnSizeRequest(double widthConstraint, double heightConstraint)
        {
            double valueConstant = 100;
            SizeRequest resultRequest = new SizeRequest(new Size(valueConstant, valueConstant), new Size(valueConstant, valueConstant));

            return resultRequest;
        }

        /// <summary>
        /// Internal initialze view.
        /// </summary>
        protected override void InternalInitializeView()
        {
            // Image layout.
            ImageLayout = new SimpleLayout()
            {
                IsHandlingLayoutManually = true,
                Padding = 0,
                IsClippedToBounds = true
            };

            ImageLayout.OnLayoutChildren += ImageLayout_OnLayoutChildren;
            ImageLayout.ManualSizeCalculationDelegate = ImageLayout_OnSizeRequest;

            ProductNameLabel = new ExtendedLabel()
            {
                Style = Theme.ApplicationStyles.DetailNameExtendedLabelStyle,
                FontSize = Theme.CommonResources.TextSizeMicro,
                FontAttributes = Xamarin.Forms.FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start,
                LineBreakMode = LineBreakMode.WordWrap
            };

            // Price
            ProductPriceLabel = new ExtendedLabel()
            {
                Style = Theme.ApplicationStyles.DetailValueExtendedLabelStyle,
                FontSize = Theme.CommonResources.TextSizeMicro * 0.9f,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Start,
                HorizontalTextAlignment = TextAlignment.Start,
                TextColor = Theme.CommonResources.TextColorDetailValue,
                LineBreakMode = LineBreakMode.WordWrap
            };

            BackgroundBox = new BoxView()
            {
                Color = Color.White
            };

            BackgroundShape = new ShapeView()
            {
                Color = Color.White,
                ShapeType = ShapeType.Box
            };

            if (HasBackgroundBorder)
            {
                BackgroundBorder = new ShapeView()
                {
                    Color = Color.Transparent,
                    ShapeType = ShapeType.Box,
                    StrokeColor = StrokeColorPageBackground ? PageBackgroundColor : Theme.CommonResources.StrokeColorDefaultItem,
                    CornerRadius = 0,
                    StrokeWidth = BackgroundBorderWidth
                };
            }

            ProductPriceWithoutDiscountLabel = new ExtendedLabel()
            {
                Style = Theme.ApplicationStyles.DetailValueExtendedLabelStyle,
                FontSize = Theme.CommonResources.TextSizeMicro * 0.7,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Start,
                HorizontalTextAlignment = TextAlignment.Start,
                TextColor = Theme.CommonResources.TextColorDetailValue,
                LineBreakMode = LineBreakMode.WordWrap,
                IsStrikeThrough = true
            };

            if (!HasNativeStrikeThrough)
            {
                PriceStrikeThrought = new BoxView()
                {
                    Color = ProductPriceWithoutDiscountLabel.TextColor,
                };
            }

            if (HasNewStack)
            {
                NewLabel = new ExtendedLabel()
                {
                    Style = Theme.ApplicationStyles.DetailValueExtendedLabelStyle,
                    BackgroundColor = Theme.CommonResources.ItemTrackingStatusNewColor,
                    FontSize = Theme.CommonResources.TextSizeMicro * (0.6f),
                    TextColor = Theme.CommonResources.TextColorSection,
                    Text = App.LocalizationResources.StoreItemNewLabel,
                    VerticalOptions = LayoutOptions.Start,
                    HorizontalOptions = LayoutOptions.Start,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    Opacity = 0
                };

                var stackNewBarRef = NewLabel;

                if (CanAnimate)
                {
                    AC.ScheduleManaged(
                        TimeSpan.FromSeconds(3.5),
                        async () =>
                        {
                            await stackNewBarRef.FadeTo(1, 350, Easing.SinOut);
                        });
                }
                else
                {
                    stackNewBarRef.UpdateOpacity(1);
                }
            }

            // Thumb image.
            ThumbImage = new ExtendedImage()
            {
                Aspect = Aspect.AspectFill
            };

            // Thumb border.
            ThumbNormalBorder = new ShapeView()
            {
                Color = Theme.CommonResources.AccentAlternative,
                ShapeType = ShapeType.Box
            };

            ThumbFeaturedBorder = new ShapeView()
            {
                Color = Theme.CommonResources.Accent,
                ShapeType = ShapeType.Box
            };
        }

        /// <summary>
        /// Setup the bindings of the elements.
        /// </summary>
        protected override void SetupBindings()
        {
            base.SetupBindings();

            if (ProductPriceWithoutDiscountLabel != null)
            {
                ProductPriceWithoutDiscountLabel.IsVisible = false;
            }

            if (PriceStrikeThrought != null)
            {
                PriceStrikeThrought.IsVisible = false;
            }

            if (BackgroundShape != null)
            {
                BackgroundShape.SetBinding<StoreItemViewModel>(View.IsVisibleProperty, vm => vm.Item.HasFullSizeThumb);
            }
        }
    }
}
