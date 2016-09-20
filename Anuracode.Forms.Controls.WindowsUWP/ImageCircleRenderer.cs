// <copyright file="ImageCircleRenderer.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;
using Xamarin.Forms;

#if WINDOWS_UWP

using Xamarin.Forms.Platform.UWP;

#else

using Xamarin.Forms.Platform.WinRT;

#endif

[assembly: ExportRenderer(typeof(Anuracode.Forms.Controls.CircleImage), typeof(Anuracode.Forms.Controls.Renderers.ImageCircleRenderer))]

namespace Anuracode.Forms.Controls.Renderers
{
    public sealed class ImageLoaderSourceHandler1 : IImageSourceHandler, IRegisterable
    {
        /// <summary>
        /// Load image.
        /// </summary>
        /// <param name="imagesoure">Image source to use.</param>
        /// <param name="cancelationToken">Token to use.</param>
        /// <returns></returns>
        public async Task<Windows.UI.Xaml.Media.ImageSource> LoadImageAsync(Xamarin.Forms.ImageSource imagesoure, CancellationToken cancelationToken = default(CancellationToken))
        {
            await Task.FromResult(0);

            Windows.UI.Xaml.Media.ImageSource imageSource;
            UriImageSource uriImageSource = imagesoure as UriImageSource;
            if (uriImageSource == null || uriImageSource.Uri == null)
            {
                imageSource = null;
            }
            else
            {
                BitmapImage bitmapImage = new BitmapImage(uriImageSource.Uri);

                imageSource = bitmapImage;
            }
            return imageSource;
        }
    }

    /// <summary>
    /// Shape renderer.
    /// </summary>
    public class ImageCircleRenderer : ViewRenderer<CircleImage, Ellipse>
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
        /// Transparent brush.
        /// </summary>
        private static Brush transparentBrush = new SolidColorBrush(Windows.UI.Colors.Transparent);

        /// <summary>
        /// Called when [element changed].
        /// </summary>
        /// <param name="e">The e.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<CircleImage> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || this.Element == null)
                return;

            Ellipse elipese = new Ellipse()
            {
                Fill = transparentBrush,
                Stroke = e.NewElement.BorderColor.ToBrush(),
                StrokeThickness = e.NewElement.BorderThickness
            };

            UpdateImageSource(e.NewElement, elipese);

            SetNativeControl(elipese);
        }

        /// <summary>
        /// Image token source.
        /// </summary>
        protected CancellationTokenSource ImageTokenSource { get; set; }

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
                    lockImageSource = new SemaphoreSlim(1);
                }

                return lockImageSource;
            }
        }

        /// <summary>
        /// Update image source.
        /// </summary>
        /// <param name="newElement">Element to use.</param>
        /// <param name="elipese">Control to use.</param>
        private void UpdateImageSource(CircleImage newElement, Ellipse elipese)
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
                            var imageSrouceNative = await handler.LoadImageAsync(newElement.Source, ImageTokenSource.Token);

                            if (imageSrouceNative != null)
                            {
                                ImageBrush newImageBrush = new ImageBrush()
                                {
                                    ImageSource = imageSrouceNative
                                };

                                elipese.Fill = newImageBrush;
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
        /// Property changed.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Arguments of the event.</param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if ((this.Element == null) || (this.Control == null))
            {
                return;
            }

            if (e.PropertyName == CircleImage.BorderColorProperty.PropertyName)
            {
                Control.Stroke = Element.BorderColor.ToBrush();
            }
            else if (e.PropertyName == CircleImage.BorderThicknessProperty.PropertyName)
            {
                Control.StrokeThickness = Element.BorderThickness;
            }
            else if (e.PropertyName == CircleImage.SourceProperty.PropertyName)
            {
                UpdateImageSource(Element, Control);
            }
        }
    }
}