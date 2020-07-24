// <copyright file="ManagedImageRenderer.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

#if WINDOWS_UWP

using Xamarin.Forms.Platform.UWP;
using Stretch = Windows.UI.Xaml.Media.Stretch;

#else

using Xamarin.Forms.Platform.WinRT;

#endif

namespace Anuracode.Forms.Controls.Renderers
{
    /// <summary>
    /// Shape renderer.
    /// </summary>
    public class ExtendedImageRenderer : ViewRenderer<ExtendedImage, Windows.UI.Xaml.Controls.Image>
    {
        /// <summary>
        /// Returns the proper <see cref="IImageSourceHandler"/> based on the type of <see cref="ImageSource"/> provided.
        /// </summary>
        /// <param name="source">The <see cref="ImageSource"/> to get the handler for.</param>
        /// <returns>The needed handler.</returns>
        protected static IImageSourceHandler GetHandler(Xamarin.Forms.ImageSource source)
        {
            IImageSourceHandler returnValue = null;
            if (source is UriImageSource)
            {
                returnValue = new ImageLoaderSourceHandler1();
            }
            else if (source is FileImageSource)
            {
                returnValue = new FileImageSourceHandler();
            }
            else if (source is StreamImageSource)
            {
                returnValue = new StreamImageSourceHandler();
            }
            return returnValue;
        }

        /// <summary>
        /// Called when [element changed].
        /// </summary>
        /// <param name="e">The e.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<ExtendedImage> e)
        {
            try
            {
                base.OnElementChanged(e);

                if (e.OldElement != null || this.Element == null)
                    return;

                if (Control == null)
                {
                    Windows.UI.Xaml.Controls.Image controlImage = new Windows.UI.Xaml.Controls.Image();
                    controlImage.ImageOpened += OnImageOpened;

                    UpdateImageSource(e.NewElement, controlImage);
                    UpdateImageAspect(e.NewElement, controlImage);

                    SetNativeControl(controlImage);
                }
            }
            catch (Exception ex)
            {
                Anuracode.Forms.Controls.AC.TraceError("ManagedImageRenderer problem: {0}", ex);
            }
        }

        /// <summary>
        /// Image token source.
        /// </summary>
        protected CancellationTokenSource ImageTokenSource { get; set; }

        /// <summary>
        /// Render disposed.
        /// </summary>
        /// <param name="disposing">True for managed disposed.</param>
        protected override void Dispose(bool disposing)
        {
            if (Control != null)
            {
                Control.ImageOpened -= OnImageOpened;
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Event when image opened.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="routedEventArgs">Arguments of the event.</param>
        private void OnImageOpened(object sender, RoutedEventArgs routedEventArgs)
        {
            RefreshImage();
        }

        /// <summary>
        /// Refresh image.
        /// </summary>
        private void RefreshImage()
        {
            if (Element != null)
            {
                ((IVisualElementController)Element)?.InvalidateMeasure(InvalidationTrigger.RendererReady);
            }
        }

        /// <summary>
        /// Lock for the  image source.
        /// </summary>
        private SemaphoreSlim lockImageSource;

        /// <summary>
        /// Lock for the  image source.
        /// </summary>
        protected SemaphoreSlim LockImageSource
        {
            get
            {
                if (lockImageSource == null)
                {
                    lockImageSource = new SemaphoreSlim(5);
                }

                return lockImageSource;
            }
        }

        /// <summary>
        /// Update image source.
        /// </summary>
        /// <param name="newElement">Element to use.</param>
        /// <param name="imageControl">Control to use.</param>
        protected virtual void UpdateImageAspect(ExtendedImage newElement, Windows.UI.Xaml.Controls.Image imageControl)
        {
            AC.ScheduleManaged(
                async () =>
                {
                    await Task.FromResult(0);

                    if ((newElement == null) || (imageControl == null))
                    {
                        return;
                    }

                    imageControl.Stretch = GetStretch(Element.Aspect);

                    imageControl.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
                    imageControl.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
                });
        }

        /// <summary>
        /// Update image source.
        /// </summary>
        /// <param name="newElement">Element to use.</param>
        /// <param name="imageControl">Control to use.</param>
        protected virtual void UpdateImageSource(ExtendedImage newElement, Windows.UI.Xaml.Controls.Image imageControl)
        {
            AC.ScheduleManaged(
                async () =>
                {
                    try
                    {
                        if ((ImageTokenSource != null) && !ImageTokenSource.IsCancellationRequested)
                        {
                            ImageTokenSource.Cancel();
                            ImageTokenSource = null;
                        }

                        if ((this.Element == null) || (this.Control == null))
                        {
                            return;
                        }

                        if (ImageTokenSource == null)
                        {
                            ImageTokenSource = new CancellationTokenSource();
                        }

                        await LockImageSource.WaitAsync(ImageTokenSource.Token);

                        var handler = GetHandler(newElement.Source);

                        if (handler != null)
                        {
                            var imagesourceNative = await handler.LoadImageAsync(newElement.Source, ImageTokenSource.Token);

                            if (imagesourceNative != null)
                            {
                                imageControl.Source = imagesourceNative;
                            }
                        }
                    }
                    finally
                    {
                        LockImageSource.Release();
                    }
                });
        }

        /// <summary>
        /// Get strech.
        /// </summary>
        /// <param name="aspect">Aspect to translate.</param>
        /// <returns>Strech to use.</returns>
        private static Stretch GetStretch(Aspect aspect)
        {
            switch (aspect)
            {
                case Aspect.Fill:
                    return Stretch.Fill;

                case Aspect.AspectFill:
                    return Stretch.UniformToFill;

                default:
                case Aspect.AspectFit:
                    return Stretch.Uniform;
            }
        }

        /// <summary>
        /// Property changed.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Arguments of the event.</param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                base.OnElementPropertyChanged(sender, e);

                if ((this.Element == null) || (this.Control == null))
                {
                    return;
                }

                if (e.PropertyName == ExtendedImage.SourceProperty.PropertyName)
                {
                    UpdateImageSource(Element, Control);
                }
                else if (e.PropertyName == ExtendedImage.AspectProperty.PropertyName)
                {
                    UpdateImageAspect(Element, Control);
                }
            }
            catch (Exception ex)
            {
                Anuracode.Forms.Controls.AC.TraceError("ManagedImageRenderer problem: {0}", ex);
            }
        }
    }
}