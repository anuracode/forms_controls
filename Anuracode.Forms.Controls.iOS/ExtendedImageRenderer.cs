// <copyright file="ExtendedImageRenderer.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using CoreGraphics;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Anuracode.Forms.Controls.Renderers
{
    /// <summary>
    /// Extended image renderer
    /// </summary>
    public class ExtendedImageRenderer : ViewRenderer<ExtendedImage, UIImageView>
    {
        /// <summary>
        /// Is disposed.
        /// </summary>
        private bool isDisposed;

        /// <summary>
        /// Last image source.
        /// </summary>
        private Xamarin.Forms.ImageSource lastImageSource = null;

        /// <summary>
        /// Allow down sample.
        /// </summary>
        public static bool AllowDownSample { get; set; }

        /// <summary>
        /// Flag to dispose the old image.
        /// </summary>
        public static bool DisposeOldImage { get; set; }

        /// <summary>
        /// Token created when the view model is navigated and cancel when is navigated off.
        /// </summary>
        protected CancellationTokenSource UpdateSourceCancellationToken { get; set; }

        /// <summary>
        ///   Used for registration with dependency service
        /// </summary>
        public static new void Init()
        {
            // needed because of this STUPID linker issue: https://bugzilla.xamarin.com/show_bug.cgi?id=31076
            var dummy = new ExtendedImageRenderer();
        }

        /// <summary>
        /// Dispose renderer.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (isDisposed)
            {
                return;
            }

            if (UpdateSourceCancellationToken != null)
            {
                UpdateSourceCancellationToken.Cancel();

                UpdateSourceCancellationToken = null;
            }

            if (disposing && Control != null)
            {
                UIImage image = Control.Image;
                if (image != null)
                {
                    image.Dispose();
                }
            }

            isDisposed = true;
            base.Dispose(disposing);
        }

        /// <summary>
        /// On element changed.
        /// </summary>
        /// <param name="e">Arguments of the event.</param>
        protected override async void OnElementChanged(ElementChangedEventArgs<ExtendedImage> e)
        {
            try
            {
                if (Control == null)
                {
                    SetNativeControl(new UIImageView(CGRect.Empty)
                    {
                        ContentMode = UIViewContentMode.ScaleAspectFit,
                        ClipsToBounds = true
                    });
                }

                if (e.NewElement != null)
                {
                    SetAspect();
                    await SetImage(e.OldElement);
                    SetOpacity();
                }

                base.OnElementChanged(e);
            }
            catch (Exception ex)
            {
                Anuracode.Forms.Controls.AC.TraceError("ManagedImageRenderer problem: {0}", ex);
            }
        }

        /// <summary>
        /// Property changed.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Arguments of the event.</param>
        protected override async void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
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
                    await SetImage(null);
                }
                if (e.PropertyName == ExtendedImage.IsOpaqueProperty.PropertyName)
                {
                    SetOpacity();
                }
                if (e.PropertyName == ExtendedImage.AspectProperty.PropertyName)
                {
                    SetAspect();
                }
            }
            catch (Exception ex)
            {
                Anuracode.Forms.Controls.AC.TraceError("ManagedImageRenderer problem: {0}", ex);
            }
        }

        /// <summary>
        /// Image loading completed.
        /// </summary>
        /// <param name="element">Element to use.</param>
        private void ImageLoadingFinished(ExtendedImage element)
        {
            if ((element != null) && !isDisposed && (Control != null))
            {
                AC.ThreadManager.ScheduleManagedFull(
                    async () =>
                    {
                        await Task.FromResult(0);

                        ((IVisualElementController)element).NativeSizeChanged();
                    });
            }
        }

        /// <summary>
        /// Set aspect.
        /// </summary>
        private void SetAspect()
        {
            if ((Element != null) && (Control != null) && !isDisposed)
            {
                Control.ContentMode = Element.Aspect.ToUIViewContentMode();
            }
        }

        /// <summary>
        /// Set image.
        /// </summary>
        /// <param name="oldElement"></param>
        private async Task SetImage(ExtendedImage oldElement = null)
        {
            await Task.FromResult(0);

            if ((Element != null) && (Element.Source != null) && (Control != null) && !isDisposed)
            {
                Xamarin.Forms.ImageSource source = Element.Source;

                if (oldElement != null)
                {
                    Xamarin.Forms.ImageSource source2 = oldElement.Source;
                    if (object.Equals(source2, source))
                    {
                        return;
                    }

                    if (source2 is FileImageSource && source is FileImageSource && ((FileImageSource)source2).File == ((FileImageSource)source).File)
                    {
                        return;
                    }
                }

                try
                {
                    if (UpdateSourceCancellationToken != null)
                    {
                        UpdateSourceCancellationToken.Cancel();

                        UpdateSourceCancellationToken = null;
                    }

                    if (UpdateSourceCancellationToken == null)
                    {
                        UpdateSourceCancellationToken = new CancellationTokenSource();
                    }

                    if (oldElement == null || !object.Equals(oldElement.Source, Element.Source))
                    {
                        UIImageView formsImageView = Control as UIImageView;

                        if (formsImageView == null)
                        {
                            return;
                        }

                        var completed = await formsImageView.UpdateImageSource(
                            source,
                            lastImageSource: lastImageSource,
                            allowDownSample: AllowDownSample,
                            targetWidth: Element.WidthRequest,
                            targetHeight: Element.HeightRequest,
                            cancellationToken: UpdateSourceCancellationToken.Token);

                        if (completed)
                        {
                            lastImageSource = source;
                        }

                        ImageLoadingFinished(Element);
                    }
                }
                catch (TaskCanceledException)
                {
                }
                catch (OperationCanceledException)
                {
                }
                catch (IOException)
                {
                }
                catch (Exception ex)
                {
                    AC.TraceError("iOS Extended renderer", ex);
                }
            }
        }

        /// <summary>
        /// Set opacity.
        /// </summary>
        private void SetOpacity()
        {
            if ((Element != null) && (Control != null) && !isDisposed)
            {
                Control.Opaque = Element.IsOpaque;
            }
        }
    }
}