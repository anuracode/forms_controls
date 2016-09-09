// <copyright file="CachedImageRenderer.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Android.Runtime;
using Android.Widget;
using Anuracode.Forms.Controls;
using FFImageLoading;
using FFImageLoading.Views;
using FFImageLoading.Work;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Anuracode.Forms.Controls.Renderers.ExtendedImageRenderer), typeof(Anuracode.Forms.Controls.Renderers.ExtendedImageRenderer))]

namespace Anuracode.Forms.Controls.Renderers
{
    /// <summary>
    /// CachedImage Implementation
    /// </summary>
    [Preserve(AllMembers = true)]
    public class ExtendedImageRenderer : ViewRenderer<ExtendedImage, ImageViewAsync>
    {
        /// <summary>
        /// Lock for the source update.
        /// </summary>
        private static SemaphoreSlim lockSource;

        /// <summary>
        /// Current task.
        /// </summary>
        private IScheduledWork _currentTask;

        /// <summary>
        /// Is disposed.
        /// </summary>
        private bool _isDisposed;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ExtendedImageRenderer()
        {
            AutoPackage = false;
        }

        /// <summary>
        /// Lock for the source update.
        /// </summary>
        public SemaphoreSlim LockSource
        {
            get
            {
                if (lockSource == null)
                {
                    int semaphoreLimit = 2 * GetNumberOfCores();
                    lockSource = new SemaphoreSlim(semaphoreLimit);
                }

                return lockSource;
            }
        }

        /// <summary>
        /// Token created when the view model is navigated and cancel when is navigated off.
        /// </summary>
        protected CancellationTokenSource UpdateSourceCancellationToken { get; set; }

        /// <summary>
        ///   Used for registration with dependency service
        /// </summary>
        public static void Init()
        {
        }

        /// <summary>
        /// Cancel task.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="args">Arguments of the event.</param>
        public void Cancel(object sender, EventArgs args)
        {
            if (_currentTask != null && !_currentTask.IsCancelled)
            {
                _currentTask.Cancel();
                _currentTask = null;
            }
        }

        /// <summary>
        /// Dispose control.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                Cancel(this, EventArgs.Empty);

                if (UpdateSourceCancellationToken != null)
                {
                    UpdateSourceCancellationToken.Cancel();

                    UpdateSourceCancellationToken = null;
                }

                _isDisposed = true;
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
        /// Get the number of cores.
        /// </summary>
        /// <returns>Number of cores of the device.</returns>
        private int GetNumberOfCores()
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
        /// Image loading.
        /// </summary>
        /// <param name="element">Element to use.</param>
        private void ImageLoadingFinished(ExtendedImage element)
        {
            if ((element != null) && !_isDisposed && (Control != null))
            {
                AC.ThreadManager.ScheduleManagedFull(
                    async () =>
                    {
                        await Task.FromResult(0);

                        if ((element != null) && !_isDisposed && (Control != null))
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

                        if ((Element != null) && (Control != null) && !_isDisposed)
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
            try
            {
                Cancel(this, EventArgs.Empty);

                if (UpdateSourceCancellationToken != null)
                {
                    UpdateSourceCancellationToken.Cancel();

                    UpdateSourceCancellationToken = null;
                }

                if (UpdateSourceCancellationToken == null)
                {
                    UpdateSourceCancellationToken = new CancellationTokenSource();
                }

                await LockSource.WaitAsync(UpdateSourceCancellationToken.Token);

                if (previous == null || !object.Equals(previous.Source, Element.Source))
                {
                    Xamarin.Forms.ImageSource source = Element.Source;
                    ImageViewAsync formsImageView = Control as ImageViewAsync;

                    if (formsImageView == null)
                        return;

                    if (Element != null && object.Equals(Element.Source, source) && !_isDisposed)
                    {
                        TaskCompletionSource<ExtendedImage> tc = new TaskCompletionSource<ExtendedImage>();
                        ExtendedImage ei = Element;

                        try
                        {
                            TaskParameter imageLoader = null;

                            var ffSource = ImageSourceBinding.GetImageSourceBinding(source);

                            if (ffSource == null)
                            {
                                if (Control != null)
                                    Control.SetImageDrawable(null);

                                tc.SetResult(ei);
                            }
                            else if (ffSource.ImageSource == FFImageLoading.Work.ImageSource.Url)
                            {
                                imageLoader = ImageService.LoadUrl(ffSource.Path, TimeSpan.FromDays(1));
                            }
                            else if (ffSource.ImageSource == FFImageLoading.Work.ImageSource.CompiledResource)
                            {
                                imageLoader = ImageService.LoadCompiledResource(ffSource.Path);
                            }
                            else if (ffSource.ImageSource == FFImageLoading.Work.ImageSource.ApplicationBundle)
                            {
                                imageLoader = ImageService.LoadFileFromApplicationBundle(ffSource.Path);
                            }
                            else if (ffSource.ImageSource == FFImageLoading.Work.ImageSource.Filepath)
                            {
                                imageLoader = ImageService.LoadFile(ffSource.Path);
                            }

                            if (imageLoader != null)
                            {
                                // Downsample
                                if (ei.HeightRequest > 0 || ei.WidthRequest > 0)
                                {
                                    if (ei.HeightRequest > ei.WidthRequest)
                                    {
                                        if (ei.WidthRequest > 500)
                                        {
                                            imageLoader.DownSample(height: (int)ei.WidthRequest);
                                        }
                                    }
                                    else
                                    {
                                        if (ei.HeightRequest > 500)
                                        {
                                            imageLoader.DownSample(width: (int)ei.HeightRequest);
                                        }
                                    }
                                }

                                imageLoader.Retry(2, 30000);

                                imageLoader.Finish(
                                    (work) =>
                                    {
                                        tc.TrySetResult(ei);
                                    });

                                imageLoader.Error(
                                    (iex) =>
                                {
                                    tc.TrySetException(iex);
                                });

                                _currentTask = imageLoader.Into(Control);
                            }
                        }
                        catch (Exception ex)
                        {
                            tc.SetException(ex);
                        }

                        var waitedElement = await tc.Task;

                        ImageLoadingFinished(waitedElement);
                    }
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
            finally
            {
                LockSource.Release();
            }
        }
    }
}