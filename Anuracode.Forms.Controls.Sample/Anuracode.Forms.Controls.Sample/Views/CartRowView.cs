// <copyright file="CartRowView.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Sample.ViewModels;
using System;
using System.Windows.Input;
using Xamarin.Forms;
using Anuracode.Forms.Controls.Extensions;
using Anuracode.Forms.Controls.Views.Extensions;

namespace Anuracode.Forms.Controls.Sample.Views
{
    /// <summary>
    /// View for the cart.
    /// </summary>
    public class CartRowView : SimpleViewBase
    {
        
        

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="renderDelete">Falg to render delte button.</param>
        public CartRowView()
            : base()
        {
        }

        

        

        /// <summary>
        /// Image preview.
        /// </summary>
        protected ContentViewButton ImageDetailPreview { get; set; }        

        /// <summary>
        /// Label name.
        /// </summary>
        protected Label LabelName { get; set; }

        /// <summary>
        /// Label price value.
        /// </summary>
        protected Label LabelPriceValue { get; set; }

        /// <summary>
        /// Label quanity.
        /// </summary>
        protected Label LabelQuantity { get; set; }

        /// <summary>
        /// Label total price.
        /// </summary>
        protected ExtendedLabel LabelTotalPrice { get; set; }

        /// <summary>
        /// Render delete.
        /// </summary>
        protected bool RenderDelete
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Setup bindings.
        /// </summary>
        protected override void SetupBindings()
        {
            if (ImageDetailPreview != null)
            {
                ImageDetailPreview.SetBinding<StoreItemCartViewModel>(ContentViewButton.SourceProperty, mv => mv.ItemViewModel.ThumbnailImagePath);                
            }

            if (LabelName != null)
            {
                LabelName.SetBinding<StoreItemCartViewModel>(Label.TextProperty, mv => mv.ItemViewModel.Item.Name);
            }

            if (LabelPriceValue != null)
            {
                LabelPriceValue.Text = "$0";
            }            
            
            if (LabelQuantity != null)
            {
                LabelQuantity.Text = "0";
            }
           

            if (LabelTotalPrice != null)
            {
                LabelTotalPrice.Text = "$0";
            }            
        }

        /// <summary>
        /// Add controls.
        /// </summary>
        protected override void AddControlsToLayout()
        {
            AddViewToLayout(LabelName);
            AddViewToLayout(LabelPriceValue);
            AddViewToLayout(LabelQuantity);
            AddViewToLayout(LabelTotalPrice);
            AddViewToLayout(ImageDetailPreview);            
        }

        /// <summary>
        /// Layout controls.
        /// </summary>
        /// <param name="x">Top to use.</param>
        /// <param name="y">Left to use.</param>
        /// <param name="width">Width to use.</param>
        /// <param name="height">Height to use.</param>
        protected override void ContentLayout_OnLayoutChildren(double x, double y, double width, double height)
        {
            Rectangle imagePosition = new Rectangle();
            Rectangle actionButtonPosition = new Rectangle(width - Theme.CommonResources.CartActionButtonWidth, 0, Theme.CommonResources.CartActionButtonWidth, height);
            Rectangle totalPricePosition = new Rectangle();
            Rectangle addButtonPosition = new Rectangle();
            Rectangle quantityPosition = new Rectangle();
            Rectangle substractButtonPosition = new Rectangle();
            Rectangle namePosition = new Rectangle();
            Rectangle pricePosition = new Rectangle();

            // Image
            if (ImageDetailPreview != null)
            {
                var elementSize = ImageDetailPreview.GetSizeRequest(width, height).Request;
                double elementLeft = 0;
                double elementHeight = elementSize.Height;
                double elementWidth = elementSize.Width;
                double elementTop = (height - elementHeight) * 0.5f;

                imagePosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                ImageDetailPreview.LayoutUpdate(imagePosition);
            }

            double contentLeft = imagePosition.Width + Margin;
            double contentWidth = width - imagePosition.Width - Margin - (Theme.CommonResources.RoundedButtonWidth * 2f) - (Theme.CommonResources.CartPriceWidth * 0.5f) - (Theme.CommonResources.CartPriceWidth);

            double priceWidth = Theme.CommonResources.CartPriceWidth;

            bool useAutoSize = contentWidth < 100;           

            // Total price.
            if (LabelTotalPrice != null)
            {
                double elementWidth = priceWidth;
                var elementSize = LabelTotalPrice.GetSizeRequest(useAutoSize ? width : elementWidth, height).Request;

                if (useAutoSize)
                {
                    elementWidth = elementSize.Width;
                }

                double elementHeight = elementSize.Height;
                double elementLeft = actionButtonPosition.X - elementWidth;
                double elementTop = (height - elementHeight) * 0.5f;

                totalPricePosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                LabelTotalPrice.LayoutUpdate(totalPricePosition);
            }

            // Add button.
            if (true)
            {
                double elementWidth = Theme.CommonResources.RoundedButtonWidth;
                double elementHeight = Theme.CommonResources.RoundedButtonWidth;
                double elementLeft = totalPricePosition.X - elementWidth;
                double elementTop = (height - elementHeight) * 0.5f;

                addButtonPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);                
            }

            if (LabelQuantity != null)
            {
                double elementWidth = priceWidth * 0.5f;
                var elementSize = LabelQuantity.GetSizeRequest(useAutoSize ? width : elementWidth, height).Request;

                if (useAutoSize)
                {
                    elementWidth = elementSize.Width;
                }

                double elementHeight = Theme.CommonResources.RoundedButtonWidth;
                double elementLeft = addButtonPosition.X - elementWidth;
                double elementTop = (height - elementHeight) * 0.5f;

                quantityPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                LabelQuantity.LayoutUpdate(quantityPosition);
            }

            // Substract button.
            if (true)
            {
                double elementWidth = Theme.CommonResources.RoundedButtonWidth;
                double elementHeight = Theme.CommonResources.RoundedButtonWidth;
                double elementLeft = quantityPosition.X - elementWidth;
                double elementTop = (height - elementHeight) * 0.5f;

                substractButtonPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);                
            }

            contentWidth = substractButtonPosition.X - contentLeft;

            if (LabelName != null)
            {
                var elementSize = LabelName.GetSizeRequest(contentWidth, height).Request;
                double elementWidth = contentWidth;
                double elementHeight = elementSize.Height;
                double elementLeft = contentLeft;
                double elementTop = 0;

                namePosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                LabelName.LayoutUpdate(namePosition);
            }

            if (LabelPriceValue != null)
            {
                var elementSize = LabelPriceValue.GetSizeRequest(contentWidth, height).Request;
                double elementWidth = contentWidth;
                double elementHeight = elementSize.Height;
                double elementLeft = contentLeft;
                double elementTop = namePosition.Y + namePosition.Height + (Margin * 0f);

                pricePosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                LabelPriceValue.LayoutUpdate(pricePosition);
            }            
        }

        /// <summary>
        /// Calculate size.
        /// </summary>
        /// <param name="widthConstraint">Width to constraint.</param>
        /// <param name="heightConstraint">height to contratint.</param>
        /// <returns></returns>
        protected override SizeRequest ContentLayout_OnSizeRequest(double widthConstraint, double heightConstraint)
        {
            Size imageSize = new Size();
            Size nameSize = new Size();
            Size priceSize = new Size();
            Size labelOnlySize = new Size();

            if (ImageDetailPreview != null)
            {
                imageSize = ImageDetailPreview.GetSizeRequest(widthConstraint, heightConstraint).Request;
            }

            double contentMaxWidth = imageSize.Width + Margin + Theme.CommonResources.RoundedButtonWidth + (Theme.CommonResources.CartPriceWidth * 0.5f) + Theme.CommonResources.RoundedButtonWidth + (Theme.CommonResources.CartPriceWidth);
            double minContentWidth = widthConstraint.Clamp(0, 4000) - contentMaxWidth.Clamp(0, widthConstraint);

            if (LabelPriceValue != null)
            {
                priceSize = LabelPriceValue.GetSizeRequest(minContentWidth, heightConstraint).Request;
            }

            if (LabelName != null)
            {
                nameSize = LabelName.GetSizeRequest(minContentWidth, heightConstraint).Request;
            }            

            double imageHeight = imageSize.Height;
            double contentHeight = nameSize.Height + priceSize.Height + labelOnlySize.Height;
            double maxHeight = Math.Max(imageHeight, contentHeight).Clamp(0, heightConstraint);

            double minWidth = contentMaxWidth;

            SizeRequest resultRequest = new SizeRequest(new Size(widthConstraint.Clamp(0, 4000), maxHeight.Clamp(0, 4000)), new Size(minWidth.Clamp(0, 4000), maxHeight.Clamp(0, 4000)));

            return resultRequest;
        }

        /// <summary>
        /// Initialize control.
        /// </summary>
        protected override void InternalInitializeView()
        {

            // Image.
            ImageDetailPreview = new ImageContentViewButton()
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                ImageWidthRequest = Theme.CommonResources.PreviewImageWidth / 2,
                ImageHeightRequest = Theme.CommonResources.PreviewImageWidth / 2,
                WidthRequest = (Theme.CommonResources.PreviewImageWidth / 2) + (3 * 2),
                HeightRequest = Theme.CommonResources.PreviewImageWidth / 2 + (3 * 2),
                StrokeWidth = 3,
                StrokeColor = Theme.CommonResources.StrokeColorDefaultItem,
                CornerRadius = 5,
                MarginBorders = 0,
                MarginElements = 0,
                ButtonBackgroundColor = Color.White,
                ImageAspect = Aspect.AspectFill
            };

            // Name
            LabelName = new ExtendedLabel()
            {
                Style = Theme.ApplicationStyles.DetailNameExtendedLabelStyle,
                LineBreakMode = Xamarin.Forms.LineBreakMode.WordWrap,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HorizontalTextAlignment = TextAlignment.Start,
                FontSize = Theme.CommonResources.TextSizeMicro
            };

            // Price
            LabelPriceValue = new ExtendedLabel()
            {
                Style = Theme.ApplicationStyles.DetailValueExtendedLabelStyle,
                FontSize = Theme.CommonResources.TextSizeMicro
            };          

            // Quantity.
            LabelQuantity = new ExtendedLabel()
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                Style = Theme.ApplicationStyles.DetailValueExtendedLabelStyle,
                FontSize = Theme.CommonResources.TextSizeMicro
            };          

            // Total price
            LabelTotalPrice = new ExtendedLabel()
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start,
                HorizontalTextAlignment = TextAlignment.End,
                VerticalTextAlignment = TextAlignment.Center,
                Style = Theme.ApplicationStyles.DetailValueExtendedLabelStyle,
                WidthRequest = Theme.CommonResources.CartPriceWidth,
                MinimumWidthRequest = Theme.CommonResources.CartPriceWidth,
                LineBreakMode = LineBreakMode.NoWrap
            };            
        }
    }
}
