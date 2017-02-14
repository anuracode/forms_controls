// <copyright file="CachedImageRenderer.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Android.Runtime;
using Android.Widget;
using FFImageLoading.Views;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Anuracode.Forms.Controls.Renderers
{
    /// <summary>
    /// CachedImage Implementation
    /// </summary>
    [Preserve(AllMembers = true)]
    public class ExtendedImageRenderer : ViewRenderer<ExtendedImage, ImageViewAsync>
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
        /// Default constructor.
        /// </summary>
        public ExtendedImageRenderer()
        {
            AutoPackage = false;
        }

        /// <summary>
        /// Allow down sample.
        /// </summary>
        public static bool AllowDownSample { get; set; }

        /// <summary>
        /// Token created when the view model is navigated and cancel when is navigated off.
        /// </summary>
        protected CancellationTokenSource UpdateSourceCancellationToken { get; set; }

        /// <summary>
        /// Get the number of cores.
        /// </summary>
        /// <returns>Number of cores of the device.</returns>
        public static int GetNumberOfCores()
        {
            if (((int)Android.OS.Build.VERSION.SdkInt) >= 17)
            {
                return Java.Lang.Runtime.GetRuntime().AvailableProcessors();
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        ///   Used for registration with dependency service
        /// </summary>
        public static void Init()
        {
        }

        /// <summary>
        /// Dispose control.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (UpdateSourceCancellationToken != null)
                {
                    UpdateSourceCancellationToken.Cancel();

                    UpdateSourceCancellationToken = null;
                }

                isDisposed = true;
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// On element changed.
        /// </summary>
        /// <param name="e">Arguments of the event.</param>
        protected override async void OnElementChanged(ElementChangedEventArgs<ExtendedImage> e)
        {
            try
            {
                base.OnElementChanged(e);

                if (e.OldElement != null || this.Element == null)
                    return;

                if (e.OldElement == null)
                {
                    ImageViewAsync nativeControl = new ImageViewAsync(Context);
                    SetNativeControl(nativeControl);
                }

                await UpdateBitmap(e.OldElement);
                UpdateAspect();
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
                    await UpdateBitmap(null);
                }
                if (e.PropertyName == ExtendedImage.AspectProperty.PropertyName)
                {
                    UpdateAspect();
                }
            }
            catch (Exception ex)
            {
                Anuracode.Forms.Controls.AC.TraceError("ManagedImageRenderer problem: {0}", ex);
            }
        }

        /// <summary>
        /// Image loading.
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

                        if ((element != null) && !isDisposed && (Control != null))
                        {
                            ((IVisualElementController)element).NativeSizeChanged();
                        }
                    });
            }
        }

        /// <summary>
        /// Update aspect.
        /// </summary>
        private void UpdateAspect()
        {
            AC.ThreadManager.ScheduleManagedFull(
                    async () =>
                    {
                        await Task.FromResult(0);

                        if ((Element != null) && (Control != null) && !isDisposed)
                        {
                            if (Element.Aspect == Aspect.AspectFill)
                            {
                                Control.SetScaleType(ImageView.ScaleType.CenterCrop);
                            }
                            else if (Element.Aspect == Aspect.Fill)
                            {
                                Control.SetScaleType(ImageView.ScaleType.FitXy);
                            }
                            else
                            {
                                Control.SetScaleType(ImageView.ScaleType.FitCenter);
                            }
                        }
                    });
        }

        /// <summary>
        /// Update bitmap.
        /// </summary>
        /// <param name="previous">Previous control.</param>
        private async Task UpdateBitmap(ExtendedImage previous = null)
        {
            if ((Element != null) && (Control != null) && !isDisposed)
            {
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

                    if (previous == null || !object.Equals(previous.Source, Element.Source))
                    {
                        Xamarin.Forms.ImageSource source = Element.Source;
                        ImageViewAsync formsImageView = Control as ImageViewAsync;

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
                catch (TaskCanceledException taskCanceledException)
                {
                }
                catch (OperationCanceledException operationCanceledException)
                {
                }
                catch (IOException oException)
                {
                }
                catch (Exception ex)
                {
                    AC.TraceError("Android Extended renderer", ex);
                }
            }
        }
    }
}