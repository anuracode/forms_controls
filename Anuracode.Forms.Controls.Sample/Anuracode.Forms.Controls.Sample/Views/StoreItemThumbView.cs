// <copyright file="StoreItemThumbView.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Extensions;
using Anuracode.Forms.Controls.Sample.ViewModels;
using System;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls.Sample.Views
{
    /// <summary>
    /// View for the store item thumbs.
    /// </summary>
    public class StoreItemThumbView : SimpleViewBase
    {
        /// <summary>
        /// Height of a line.
        /// </summary>
        private static double? lineHeight;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StoreItemThumbView()
            : this(true)
        {
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StoreItemThumbView(bool autoInit)
            : base(autoInit)
        {
            if (ContentLayout != null)
            {
                ContentLayout.IsClippedToBounds = true;
            }
        }

        /// <summary>
        /// Height of a line.
        /// </summary>
        public static double LineHeight
        {
            get
            {
                if (!lineHeight.HasValue)
                {
                    lineHeight = Theme.CommonResources.TextSizeMicro * 1.205f;
                }

                return lineHeight.Value;
            }
        }

        /// <summary>
        /// Flag if has description.
        /// </summary>
        public virtual bool HasDescription
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Flag if has description.
        /// </summary>
        public virtual bool HasNewStack
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Can animate appearing.
        /// </summary>
        protected static bool CanAnimate
        {
            get
            {
                return Device.OS.OnPlatform(true, false, true, true, true);
            }
        }

        /// <summary>
        /// Background border.
        /// </summary>
        protected ShapeView BackgroundBorder { get; set; }

        /// <summary>
        /// Background border width.
        /// </summary>
        protected virtual float BackgroundBorderWidth
        {
            get
            {
                return HasBackgroundBorder ? 4 : 0;
            }
        }

        /// <summary>
        /// Background box.
        /// </summary>
        protected BoxView BackgroundBox { get; set; }

        /// <summary>
        /// Background image.
        /// </summary>
        protected Image BackgroundImage { get; set; }

        /// <summary>
        /// Background shape, circle.
        /// </summary>
        protected ShapeView BackgroundShape { get; set; }

        /// <summary>
        /// Flag to add the background elements.
        /// </summary>
        protected virtual bool HasBackground
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Flag when the thumb has border.
        /// </summary>
        protected virtual bool HasBackgroundBorder
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Flag to add the product detail elements.
        /// </summary>
        protected virtual bool HasProductDetail
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Flag when the thumb has border.
        /// </summary>
        protected virtual bool HasThumbBorder
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Layout for the image.
        /// </summary>
        protected SimpleLayout ImageLayout { get; set; }

        /// <summary>
        /// Is featured.
        /// </summary>
        protected virtual bool IsFeatured
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Flag when the new is rotated.
        /// </summary>
        protected virtual bool IsNewRotated
        {
            get
            {
                return Device.OS.OnPlatform(true, false, true, true, true);
            }
        }

        /// <summary>
        /// New label.
        /// </summary>
        protected Label NewLabel { get; set; }

        /// <summary>
        /// View orientation.
        /// </summary>
        protected virtual StackOrientation Orientation
        {
            get
            {
                return StackOrientation.Horizontal;
            }
        }

        /// <summary>
        /// Page background color.
        /// </summary>
        protected virtual Color PageBackgroundColor
        {
            get
            {
                return Theme.CommonResources.PagesBackgroundColor;
            }
        }

        /// <summary>
        /// Name of the product.
        /// </summary>
        protected Label ProductNameLabel { get; set; }

        /// <summary>
        /// Price of the product.
        /// </summary>
        protected Label ProductPriceLabel { get; set; }

        /// <summary>
        /// Description of the product.
        /// </summary>
        protected Label ProductShortDescriptionLabel { get; set; }

        /// <summary>
        /// Store color for the main border is the same as the page.
        /// </summary>
        protected virtual bool StrokeColorPageBackground
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Stroke color for the thumb border is the same as the page.
        /// </summary>
        protected virtual bool StrokeColorThumbPageBackground
        {
            get
            {
                return !IsFeatured;
            }
        }

        /// <summary>
        /// Background shape, circle.
        /// </summary>
        protected ShapeView ThumbBackgroundShape { get; set; }

        /// <summary>
        /// Background border.
        /// </summary>
        protected ShapeView ThumbFeaturedBorder { get; set; }

        /// <summary>
        /// Background image.
        /// </summary>
        protected ExtendedImage ThumbImage { get; set; }

        /// <summary>
        /// Brand image width.
        /// </summary>
        protected virtual double ThumbImageWidth
        {
            get
            {
                return Theme.CommonResources.CategoryImageWidth;
            }
        }

        /// <summary>
        /// Background border.
        /// </summary>
        protected ShapeView ThumbNormalBorder { get; set; }

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
            switch (Orientation)
            {
                case StackOrientation.Horizontal:
                    if (IsFeatured)
                    {
                        ContentLayoutOnLayoutChildrenFeaturedHorizontal(x, y, width, height);
                    }
                    else
                    {
                        ContentLayoutOnLayoutChildrenHorizontal(x, y, width, height);
                    }
                    break;

                case StackOrientation.Vertical:
                    if (IsFeatured)
                    {
                        ContentLayoutOnLayoutChildrenFeaturedVertical(x, y, width, height);
                    }
                    break;
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
            SizeRequest resultRequest = new SizeRequest(new Size(widthConstraint, heightConstraint), new Size(widthConstraint, heightConstraint));

            switch (Orientation)
            {
                case StackOrientation.Horizontal:
                    if (IsFeatured)
                    {
                        double widthRequest = ThumbImageWidth * 4;
                        double heightRequest = ThumbImageWidth + (ThumbImageWidth * (1f / 3f));
                        resultRequest = new SizeRequest(new Size(widthRequest, heightRequest), new Size(widthRequest, heightRequest));
                    }
                    else
                    {
                        double heightRequest = ThumbImageWidth + (ThumbImageWidth * (1f / 3f));

                        Size sizeName = new Size();
                        Size sizePrice = new Size();
                        Size sizeDescription = new Size();

                        if (ProductPriceLabel != null)
                        {
                            sizePrice = ProductPriceLabel.GetSizeRequest(widthConstraint, heightRequest).Request;
                        }

                        if (ProductNameLabel != null)
                        {
                            sizeName = ProductNameLabel.GetSizeRequest(widthConstraint, heightRequest).Request;
                        }

                        if (ProductShortDescriptionLabel != null)
                        {
                            sizeDescription = ProductShortDescriptionLabel.GetSizeRequest(widthConstraint, heightRequest).Request;
                        }

                        double widthRequest = (BackgroundBorderWidth * 2) + ThumbImageWidth + (Margin * 4f) + Math.Max(Math.Max(sizeName.Width, sizePrice.Width), sizeDescription.Width);

                        resultRequest = new SizeRequest(new Size(widthRequest, heightRequest), new Size(widthRequest, heightRequest));
                    }
                    break;

                case StackOrientation.Vertical:
                    if (IsFeatured)
                    {
                        double widthRequest = ThumbImageWidth * 3.5;

                        Size priceSize = new Size();
                        Size nameSize = new Size();
                        Size descriptionSize = new Size();

                        if (ProductPriceLabel != null)
                        {
                            priceSize = ProductPriceLabel.GetSizeRequest(widthRequest, heightConstraint).Request;
                        }

                        if (ProductNameLabel != null)
                        {
                            nameSize = ProductNameLabel.GetSizeRequest(widthRequest, heightConstraint).Request;
                        }

                        if (ProductShortDescriptionLabel != null)
                        {
                            descriptionSize = ProductShortDescriptionLabel.GetSizeRequest(widthRequest, heightConstraint).Request;
                        }

                        double heightRequest = ThumbImageWidth + (Margin * 5f) + priceSize.Height + nameSize.Height + descriptionSize.Height;

                        resultRequest = new SizeRequest(new Size(widthRequest, heightRequest), new Size(widthRequest, heightRequest));
                    }
                    else
                    {
                        double widthRequest = ThumbImageWidth + (ThumbImageWidth * (1f / 3f));

                        double heightRequest = ThumbImageWidth + (BackgroundBorderWidth * 2f) + (LineHeight * 3) + (Margin * 3);
                        resultRequest = new SizeRequest(new Size(widthRequest, heightRequest), new Size(widthRequest, heightRequest));
                    }
                    break;
            }

            return resultRequest;
        }

        /// <summary>
        /// Layout children.
        /// </summary>
        /// <param name="x">Top to use.</param>
        /// <param name="y">Left to use.</param>
        /// <param name="width">Width to use.</param>
        /// <param name="height">Height to use.</param>
        protected virtual void ContentLayoutOnLayoutChildrenFeaturedHorizontal(double x, double y, double width, double height)
        {
            var borderDistance = BackgroundBorderWidth * 1.05;
            Rectangle layoutSize = new Rectangle(x, y, width, height);
            Rectangle layoutPartialBorderSize = new Rectangle(x + borderDistance, y + borderDistance, width - (borderDistance * 2f), height - (borderDistance * 2f));

            Rectangle imagePosition = new Rectangle();
            Rectangle arcPosition = new Rectangle();
            Rectangle pricePosition = new Rectangle();
            Rectangle namePosition = new Rectangle();

            if (ImageLayout != null)
            {
                var imageSize = ImageLayout.GetSizeRequest(width, height).Request;
                double elementLeft = BackgroundBorderWidth + Margin;
                double elementHeight = imageSize.Height;
                double elementWidth = imageSize.Width;
                double elementTop = (height - elementHeight) * 0.5f;

                imagePosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                ImageLayout.LayoutUpdate(imagePosition);
            }

            if (BackgroundBorder != null)
            {
                BackgroundBorder.LayoutUpdate(layoutSize);
            }

            if (BackgroundImage != null)
            {
                BackgroundImage.LayoutUpdate(layoutPartialBorderSize);
            }

            if (BackgroundBox != null)
            {
                BackgroundBox.LayoutUpdate(layoutPartialBorderSize);
            }

            if (BackgroundShape != null)
            {
                float archSize = 3f;
                double elementLeft = (width - (width * archSize)) * 0.5f;
                double elementTop = (height * 0.33f);
                double elementHeight = width * archSize;
                double elementWidth = width * archSize;

                arcPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                BackgroundShape.LayoutUpdate(arcPosition);
            }

            if (ProductPriceLabel != null)
            {
                var elementSize = ProductPriceLabel.GetSizeRequest(width, height).Request;
                double elementHeight = elementSize.Height;
                double elementWidth = elementSize.Width;
                double elementLeft = arcPosition.X + (arcPosition.Width / 2f) - (elementWidth / 2f);
                double elementTop = arcPosition.Y + (Margin * 1f);

                pricePosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                ProductPriceLabel.LayoutUpdate(pricePosition);
            }

            if (ProductNameLabel != null)
            {
                var elementSize = ProductNameLabel.GetSizeRequest(width, height).Request;
                double elementHeight = elementSize.Height;
                double elementWidth = elementSize.Width.Clamp(0, width - (Margin * 2f));
                double elementLeft = pricePosition.X + (pricePosition.Width * 0.5f) - (elementWidth * 0.5f);
                double elementTop = pricePosition.Y + (Margin * 2f);

                namePosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                ProductNameLabel.LayoutUpdate(namePosition);
            }

            if (ProductShortDescriptionLabel != null)
            {
                float marginMultiplayer = Device.Idiom == TargetIdiom.Phone ? 1.5f : 2.75f;
                double elementWidth = width - (imagePosition.X + imagePosition.Width + (Margin * 1f));
                var elementSize = ProductNameLabel.GetSizeRequest(elementWidth, height).Request;
                double elementHeight = elementSize.Height;
                double elementLeft = imagePosition.X + imagePosition.Width + (Margin * 1f);
                double elementTop = namePosition.Y + (Margin * marginMultiplayer);

                var elementPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                ProductShortDescriptionLabel.LayoutUpdate(elementPosition);
            }
        }

        /// <summary>
        /// Layout children.
        /// </summary>
        /// <param name="x">Top to use.</param>
        /// <param name="y">Left to use.</param>
        /// <param name="width">Width to use.</param>
        /// <param name="height">Height to use.</param>
        protected virtual void ContentLayoutOnLayoutChildrenFeaturedVertical(double x, double y, double width, double height)
        {
            var borderDistance = BackgroundBorderWidth * 1.05;
            Rectangle layoutSize = new Rectangle(x, y, width, height);
            Rectangle layoutPartialBorderSize = new Rectangle(x + borderDistance, y + borderDistance, width - (borderDistance * 2f), height - (borderDistance * 2f));

            Rectangle imagePosition = new Rectangle();
            Rectangle arcPosition = new Rectangle();
            Rectangle pricePosition = new Rectangle();
            Rectangle namePosition = new Rectangle();

            if (BackgroundBox != null)
            {
                BackgroundBox.LayoutUpdate(layoutSize);
            }

            if (ImageLayout != null)
            {
                var imageSize = ImageLayout.GetSizeRequest(width, height).Request;
                double elementWidth = imageSize.Width;
                double elementHeight = imageSize.Height;
                double elementLeft = (width - elementWidth) * 0.5f;
                double elementTop = Margin * 2f;

                imagePosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                ImageLayout.LayoutUpdate(imagePosition);
            }

            if (BackgroundShape != null)
            {
                float archSize = 3f;
                double elementLeft = (width - (width * archSize)) * 0.5f;
                double elementTop = imagePosition.Y + (imagePosition.Height * 0.5f);
                double elementHeight = width * archSize;
                double elementWidth = width * archSize;

                arcPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                BackgroundShape.LayoutUpdate(arcPosition);
            }

            if (BackgroundImage != null)
            {
                double elementTop = 0;
                double elementLeft = 0;
                double elementWidth = width;

                double elementHeight = arcPosition.Y + (arcPosition.Height * 0.15);

                var elementPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                BackgroundImage.LayoutUpdate(elementPosition);
            }

            if (ProductPriceLabel != null)
            {
                var elementSize = ProductPriceLabel.GetSizeRequest(width, height).Request;
                double elementHeight = elementSize.Height;
                double elementWidth = elementSize.Width;
                double elementLeft = (width - elementWidth) * 0.5f;
                double elementTop = imagePosition.Y + imagePosition.Height + (Margin * 2f);

                pricePosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                ProductPriceLabel.LayoutUpdate(pricePosition);
            }

            if (ProductNameLabel != null)
            {
                var elementSize = ProductNameLabel.GetSizeRequest(width, height).Request;
                double elementHeight = elementSize.Height;
                double elementWidth = elementSize.Width.Clamp(0, width - (Margin * 2f));
                double elementLeft = (width - elementWidth) * 0.5f;
                double elementTop = pricePosition.Y + pricePosition.Height + (Margin * 0f);

                namePosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                ProductNameLabel.LayoutUpdate(namePosition);
            }

            if (ProductShortDescriptionLabel != null)
            {
                var elementSize = ProductShortDescriptionLabel.GetSizeRequest(width, height).Request;
                double elementHeight = elementSize.Height;
                double elementWidth = elementSize.Width.Clamp(0, width - (Margin * 2f));
                double elementLeft = (width - elementWidth) * 0.5f;
                double elementTop = namePosition.Y + namePosition.Height + (Margin * 0f);

                var elementPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                ProductShortDescriptionLabel.LayoutUpdate(elementPosition);
            }
        }

        /// <summary>
        /// Layout children.
        /// </summary>
        /// <param name="x">Top to use.</param>
        /// <param name="y">Left to use.</param>
        /// <param name="width">Width to use.</param>
        /// <param name="height">Height to use.</param>
        protected virtual void ContentLayoutOnLayoutChildrenHorizontal(double x, double y, double width, double height)
        {
            var borderDistance = BackgroundBorderWidth * 1.05;
            Rectangle layoutSize = new Rectangle(x, y, width, height);
            Rectangle layoutPartialBorderSize = new Rectangle(x + borderDistance, y + borderDistance, width - (borderDistance * 2f), height - (borderDistance * 2f));

            Rectangle imagePosition = new Rectangle();
            Rectangle descriptionPosition = new Rectangle();
            Rectangle pricePosition = new Rectangle();
            Rectangle namePosition = new Rectangle();

            Size priceSize = new Size();

            if (ImageLayout != null)
            {
                var imageSize = ImageLayout.GetSizeRequest(width, height).Request;
                double elementLeft = BackgroundBorderWidth + Margin;
                double elementHeight = imageSize.Height;
                double elementWidth = imageSize.Width;
                double elementTop = (height - elementHeight) * 0.5f;

                imagePosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                ImageLayout.LayoutUpdate(imagePosition);
            }

            if (ProductNameLabel != null)
            {
                var elementSize = ProductNameLabel.GetSizeRequest(width, height).Request;
                double elementTop = imagePosition.Y + (Margin * 0.25f);
                double elementLeft = imagePosition.X + imagePosition.Width + (Margin * 2f);
                double elementHeight = elementSize.Height;
                double elementWidth = width - elementLeft - Margin - BackgroundBorderWidth;

                namePosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                ProductNameLabel.LayoutUpdate(namePosition);
            }

            if (ProductPriceLabel != null)
            {
                priceSize = ProductPriceLabel.GetSizeRequest(namePosition.Width, height).Request;
            }

            double maxDescriptionHeight = height - (imagePosition.Y * 2f) - namePosition.Height - (Margin * 0.25f) - priceSize.Height;

            if (ProductShortDescriptionLabel != null)
            {
                var elementSize = ProductNameLabel.GetSizeRequest(namePosition.Width, height).Request;
                double elementWidth = namePosition.Width;
                double elementLeft = namePosition.X;
                double elementHeight = Math.Min(elementSize.Height, maxDescriptionHeight);
                double elementTop = namePosition.Y + namePosition.Height + (Margin * 0f);

                descriptionPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                ProductShortDescriptionLabel.LayoutUpdate(descriptionPosition);
            }

            if (ProductPriceLabel != null)
            {
                double elementHeight = priceSize.Height;
                double elementWidth = namePosition.Width;
                double elementLeft = namePosition.Left;
                double elementTop = descriptionPosition.Y + descriptionPosition.Height + (Margin * 0f);

                pricePosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                ProductPriceLabel.LayoutUpdate(pricePosition);
            }
        }

        /// <summary>
        /// Layout children.
        /// </summary>
        /// <param name="x">Top to use.</param>
        /// <param name="y">Left to use.</param>
        /// <param name="width">Width to use.</param>
        /// <param name="height">Height to use.</param>
        protected virtual void ImageLayout_OnLayoutChildren(double x, double y, double width, double height)
        {
            var borderDistance = BackgroundBorderWidth;
            Rectangle layoutSize = new Rectangle(x, y, width, height);
            Rectangle layoutPartialBorderSize = new Rectangle(x + borderDistance, y + borderDistance, width - (borderDistance * 2f), height - (borderDistance * 2f));

            if (ThumbImage != null)
            {
                ThumbImage.LayoutUpdate(layoutPartialBorderSize);
            }

            if (ThumbBackgroundShape != null)
            {
                ThumbBackgroundShape.LayoutUpdate(layoutPartialBorderSize);
            }

            if (ThumbFeaturedBorder != null)
            {
                ThumbFeaturedBorder.LayoutUpdate(layoutSize);
            }

            if (ThumbNormalBorder != null)
            {
                ThumbNormalBorder.LayoutUpdate(layoutSize);
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
        protected virtual SizeRequest ImageLayout_OnSizeRequest(double widthConstraint, double heightConstraint)
        {
            SizeRequest resultRequest = new SizeRequest(new Size(ThumbImageWidth + BackgroundBorderWidth, ThumbImageWidth + BackgroundBorderWidth), new Size(ThumbImageWidth + BackgroundBorderWidth, ThumbImageWidth + BackgroundBorderWidth));

            return resultRequest;
        }

        /// <summary>
        /// Internal initialze view.
        /// </summary>
        protected override void InternalInitializeView()
        {
            if (HasBackground)
            {
                BackgroundBox = new BoxView()
                {
                    Color = Theme.CommonResources.Accent
                };

                BackgroundImage = new ExtendedImage()
                {
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Aspect = Aspect.AspectFill
                };

                BackgroundShape = new ShapeView()
                {
                    Color = Color.White,
                    ShapeType = ShapeType.Circle
                };

                if (HasBackgroundBorder)
                {
                    BackgroundBorder = new ShapeView()
                    {
                        Color = Color.Transparent,
                        ShapeType = ShapeType.Box,
                        StrokeColor = StrokeColorPageBackground ? PageBackgroundColor : Theme.CommonResources.StrokeColorDefaultItem,
                        CornerRadius = 10,
                        StrokeWidth = BackgroundBorderWidth
                    };
                }
            }

            if (HasProductDetail)
            {
                // Name
                if (IsFeatured)
                {
                    ProductNameLabel = new ExtendedLabel()
                    {
                        TextColor = Color.Black,
                        FontName = Theme.CommonResources.FontRobotBoldCondensedName,
                        FriendlyFontName = Theme.CommonResources.FontRobotBoldCondensedFriendlyName,
                        FontSize = Theme.CommonResources.TextSizeMicro,
                        FontAttributes = FontAttributes.Bold,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.Start,
                        HorizontalTextAlignment = TextAlignment.Start,
                    };
                }
                else
                {
                    ProductNameLabel = new ExtendedLabel()
                    {
                        Style = Theme.ApplicationStyles.DetailNameExtendedLabelStyle,
                        FontSize = Theme.CommonResources.TextSizeMicro,
                        HorizontalOptions = LayoutOptions.Start,
                        VerticalOptions = LayoutOptions.Start,
                        LineBreakMode = LineBreakMode.WordWrap
                    };
                }

                // Price
                ProductPriceLabel = new ExtendedLabel()
                {
                    Style = Theme.ApplicationStyles.DetailValueExtendedLabelStyle,
                    FontSize = Theme.CommonResources.TextSizeMicro,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.Start,
                    HorizontalTextAlignment = TextAlignment.Start,
                    TextColor = Theme.CommonResources.Accent,
                    LineBreakMode = LineBreakMode.WordWrap
                };

                if (IsFeatured)
                {
                    ProductPriceLabel.Style = Theme.ApplicationStyles.DetailNameExtendedLabelStyle;
                    ProductPriceLabel.TextColor = Theme.CommonResources.Accent;
                }

                if (HasDescription)
                {
                    // Short description.
                    ProductShortDescriptionLabel = new ExtendedLabel()
                    {
                        FontName = Theme.CommonResources.FontRobotLightName,
                        FriendlyFontName = Theme.CommonResources.FontRobotLightFriendlyName,
                        FontSize = Theme.CommonResources.TextSizeMicro,
                        TextColor = Theme.CommonResources.TextColorDetailValue,
                        HorizontalOptions = LayoutOptions.Center,
                        LineBreakMode = LineBreakMode.WordWrap
                    };
                }
            }

            // Image layout.
            ImageLayout = new SimpleLayout()
            {
                IsHandlingLayoutManually = true,
                Padding = 0,
                IsClippedToBounds = true
            };

            ImageLayout.OnLayoutChildren += ImageLayout_OnLayoutChildren;
            ImageLayout.ManualSizeCalculationDelegate = ImageLayout_OnSizeRequest;

            // Thumb background
            ThumbBackgroundShape = new ShapeView()
            {
                BackgroundColor = Theme.CommonResources.BackgroundColorItem
            };

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

                if (IsNewRotated)
                {
                    NewLabel.Rotation = 320;
                }

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
                    NewLabel.UpdateOpacity(1);
                }
            }

            // Thumb image.
            ThumbImage = new ExtendedImage()
            {
                Aspect = Aspect.AspectFill
            };

            if (HasThumbBorder)
            {
                // Thumb border.
                ThumbNormalBorder = new ShapeView()
                {
                    Color = Color.Transparent,
                    ShapeType = ShapeType.Box,
                    StrokeColor = StrokeColorThumbPageBackground ? PageBackgroundColor : Theme.CommonResources.StrokeColorDefaultItem,
                    CornerRadius = BackgroundBorderWidth * 2.5f,
                    StrokeWidth = BackgroundBorderWidth
                };

                ThumbFeaturedBorder = new ShapeView()
                {
                    Color = Color.Transparent,
                    ShapeType = ShapeType.Box,
                    StrokeColor = Theme.CommonResources.StrokeColorFeaturedItem,
                    CornerRadius = BackgroundBorderWidth * 2f,
                    StrokeWidth = BackgroundBorderWidth
                };
            }
        }

        /// <summary>
        /// Setup the bindings of the elements.
        /// </summary>
        protected override void SetupBindings()
        {
            if (BackgroundImage != null)
            {
                BackgroundImage.SetBinding<StoreItemViewModel>(Image.SourceProperty, mv => mv.MainImagePath);
            }

            if (ThumbNormalBorder != null)
            {
                ThumbNormalBorder.SetBinding<StoreItemViewModel>(ShapeView.IsVisibleProperty, mv => mv.Item.IsFeautred, converter: Theme.CommonResources.InvertBooleanToBooleanConverter);
            }

            if (ThumbFeaturedBorder != null)
            {
                ThumbFeaturedBorder.SetBinding<StoreItemViewModel>(ShapeView.IsVisibleProperty, mv => mv.Item.IsFeautred);
            }

            if (ThumbImage != null)
            {
                ThumbImage.SetBinding<StoreItemViewModel>(Image.SourceProperty, mv => mv.ThumbnailImagePath);
            }

            if (NewLabel != null)
            {
                NewLabel.SetBinding<StoreItemViewModel>(View.IsVisibleProperty, mv => mv.Item.IsNew);
            }

            if (ProductShortDescriptionLabel != null)
            {
                ProductShortDescriptionLabel.SetBinding<StoreItemViewModel>(ExtendedLabel.TextProperty, vm => vm.Item.ShortDescription);
            }

            if (ProductPriceLabel != null)
            {
                ProductPriceLabel.Text = "$0";
            }

            if (ProductNameLabel != null)
            {
                ProductNameLabel.SetBinding<StoreItemViewModel>(Label.TextProperty, vm => vm.Item.Name);
            }
        }
    }
}