// <copyright file="ImageSamplePage.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Extensions;
using Anuracode.Forms.Controls.Views.Extensions;
using Anuracode.Forms.Controls.Sample.ViewModels;
using System;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls.Sample.Views
{
    /// <summary>
    /// Presents an image.
    /// </summary>
    public class ImageSamplePage : BaseView
    {
        /// <summary>
        /// Current scale.
        /// </summary>
        private double currentScale;

        /// <summary>
        /// Start scale.
        /// </summary>
        private double startScale;

        /// <summary>
        /// X offset.
        /// </summary>
        private double xOffset;

        /// <summary>
        /// Y offset.
        /// </summary>
        private double yOffset;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="imageUrl"></param>
        public ImageSamplePage(ImageSampleViewModel viewModel) : base()
        {
            ViewModel = viewModel;

            if (ViewModel != null)
            {
                this.BindingContext = ViewModel;
            }

            var absoluteLayout = new SimpleLayout()
            {
                IsHandlingLayoutManually = true,
                Padding = 0
            };

            absoluteLayout.OnLayoutChildren += PageCanvasLayoutChildren;

            var pangr = new PanGestureRecognizer();
            pangr.PanUpdated += Pangr_PanUpdated;

            TapGestureRecognizer tgr = new TapGestureRecognizer()
            {
                NumberOfTapsRequired = 2
            };

            tgr.Tapped += OnTapped;

            PinchGestureRecognizer pgr = new PinchGestureRecognizer();
            pgr.PinchUpdated += PinchUpdated;

            Image = new ExtendedImage()
            {
                Aspect = Aspect.AspectFit,
                Source = ViewModel.SampleImagePath
            };

            Image.GestureRecognizers.Add(tgr);
            Image.GestureRecognizers.Add(pgr);
            Image.GestureRecognizers.Add(pangr);

            BackButton = new GlyphOnlyContentViewButton()
            {
                Style = Theme.ApplicationStyles.GlyphOnlyNavbarButtonStyle,
                BindingContext = this,
                GlyphText = Theme.CommonResources.GlyphTextNavigateBack,
                InvisibleWhenDisabled = false,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                Command = NavigateBackCommand
            };

            TopBackground = new BoxView()
            {
                HeightRequest = BackButton.HeightRequest,
                Color = BackButton.ButtonBackgroundColor
            };

            absoluteLayout.Children.Add(Image);
            absoluteLayout.Children.Add(TopBackground);
            absoluteLayout.Children.Add(BackButton);

            Content = absoluteLayout;
        }

        /// <summary>
        /// Back button.
        /// </summary>
        public ContentViewButton BackButton { get; set; }

        /// <summary>
        /// Image to use.
        /// </summary>
        protected Image Image { get; set; }

        /// <summary>
        /// Background to on the top.
        /// </summary>
        protected BoxView TopBackground { get; set; }

        /// <summary>
        /// Viewmodel to use.
        /// </summary>
        protected ImageSampleViewModel ViewModel { get; set; }

        /// <summary>
        /// Layout the children.
        /// </summary>
        /// <param name="x">Top to use.</param>
        /// <param name="y">Left to use.</param>
        /// <param name="width">Width to use.</param>
        /// <param name="height">Height to use.</param>
        protected virtual void PageCanvasLayoutChildren(double x, double y, double width, double height)
        {
            Rectangle backButtonPosition = new Rectangle();

            if (BackButton != null)
            {
                var sizeRequest = BackButton.GetSizeRequest(width, height).Request;

                backButtonPosition = new Rectangle(
                    0,
                    0,
                    sizeRequest.Width,
                    sizeRequest.Height);

                BackButton.LayoutUpdate(backButtonPosition);
            }

            if (TopBackground != null)
            {
                Rectangle elementPosition = new Rectangle(
                    0,
                    0,
                    width,
                    backButtonPosition.Height);

                TopBackground.LayoutUpdate(elementPosition);
            }

            if (Image != null)
            {
                Rectangle elementPosition = new Rectangle(
                    0,
                    0,
                    width,
                    height);

                Image.LayoutUpdate(elementPosition);
            }
        }

        /// <summary>
        /// Pinch update.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Arguments of the event.</param>
        protected void PinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            View content = Image;

            if (content != null)
            {
                if (e.Status == GestureStatus.Started)
                {
                    // Store the current scale factor applied to the wrapped user interface element,
                    // and zero the components for the center point of the translate transform.
                    startScale = content.Scale;
                    content.AnchorX = 0;
                    content.AnchorY = 0;
                }
                if (e.Status == GestureStatus.Running)
                {
                    // Calculate the scale factor to be applied.
                    currentScale += (e.Scale - 1) * startScale;
                    currentScale = Math.Max(1, currentScale);

                    // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
                    // so get the X pixel coordinate.
                    double renderedX = content.X + xOffset;
                    double deltaX = renderedX / Width;
                    double deltaWidth = Width / (content.Width * startScale);
                    double originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;

                    // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
                    // so get the Y pixel coordinate.
                    double renderedY = content.Y + yOffset;
                    double deltaY = renderedY / Height;
                    double deltaHeight = Height / (content.Height * startScale);
                    double originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;

                    // Calculate the transformed element pixel coordinates.
                    double targetX = xOffset - (originX * content.Width) * (currentScale - startScale);
                    double targetY = yOffset - (originY * content.Height) * (currentScale - startScale);

                    // Apply translation based on the change in origin.
                    content.TranslationX = targetX.Clamp(-content.Width * (currentScale - 1), 0);
                    content.TranslationY = targetY.Clamp(-content.Height * (currentScale - 1), 0);

                    // Apply scale factor.
                    content.Scale = currentScale;
                }
                if (e.Status == GestureStatus.Completed)
                {
                    // Store the translation delta's of the wrapped user interface element.
                    xOffset = content.TranslationX;
                    yOffset = content.TranslationY;
                }
            }
        }

        /// <summary>
        /// Event when tapped.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Arguments of the event.</param>
        private void OnTapped(object sender, EventArgs e)
        {
            View content = Image;

            if (content != null)
            {
                if (currentScale > 1.5)
                {
                    content.AnchorX = 0;
                    content.AnchorY = 0;
                    content.TranslationX = 0;
                    content.TranslationY = 0;
                    currentScale = 1;
                }
                else
                {
                    content.AnchorX = 0.5;
                    content.AnchorY = 0.5;
                    content.TranslationX = 0;
                    content.TranslationY = 0;
                    currentScale = 4;
                }

                xOffset = content.TranslationX;
                yOffset = content.TranslationY;
                content.Scale = currentScale;
            }
        }

        /// <summary>
        /// Pan updated.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Arguments of the event.</param>
        private void Pangr_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            View content = Image;

            if (content.Scale > 1.25)
            {
                switch (e.StatusType)
                {
                    case GestureStatus.Running:
                        // Translate and ensure we don't pan beyond the wrapped user interface element bounds.
                        content.TranslationX = (xOffset + e.TotalX).Clamp(-content.Width * content.Scale, content.Width * content.Scale);
                        content.TranslationY = (yOffset + e.TotalY).Clamp(-content.Height * content.Scale, content.Height * content.Scale);
                        break;

                    case GestureStatus.Completed:
                        // Store the translation applied during the pan
                        xOffset = content.TranslationX;
                        yOffset = content.TranslationY;
                        break;
                }
            }
        }
    }
}