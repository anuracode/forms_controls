﻿// <copyright file="CachedImageRenderer.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using CoreGraphics;
using FFImageLoading;
using FFImageLoading.Work;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Anuracode.Forms.Controls.ExtendedImage), typeof(Anuracode.Forms.Controls.Renderers.ExtendedImageRenderer))]

namespace Anuracode.Forms.Controls.Renderers
{
    /// <summary>
    /// Extended image renderer
    /// </summary>
    public class ExtendedImageRenderer : ViewRenderer<ExtendedImage, UIImageView>
    {
        /// <summary>
        /// Lock for the source update.
        /// </summary>
        private static SemaphoreSlim lockSource;

        /// <summary>
        /// Scheduled work.
        /// </summary>
        private IScheduledWork _currentTask;

        /// <summary>
        /// Is disposed.
        /// </summary>
        private bool _isDisposed;

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
        public static new void Init()
        {
            // needed because of this STUPID linker issue: https://bugzilla.xamarin.com/show_bug.cgi?id=31076
#pragma warning disable 0219
            var dummy = new ExtendedImageRenderer();
#pragma warning restore 0219
        }

        /// <summary>
        /// Dispose renderer.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }

            Cancel();

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

            _isDisposed = true;
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
        /// Cancel current loading.
        /// </summary>
        private void Cancel()
        {
            if (_currentTask != null && !_currentTask.IsCancelled)
            {
                _currentTask.Cancel();
            }
        }

        /// <summary>
        /// Get the number of cores.
        /// </summary>
        /// <returns>Number of cores of the device.</returns>
        private int GetNumberOfCores()
        {
            return 2;
        }

        /// <summary>
        /// Image loading completed.
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

                        ((IVisualElementController)element).NativeSizeChanged();
                    });
            }
        }

        /// <summary>
        /// Set aspect.
        /// </summary>
        private void SetAspect()
        {
            Control.ContentMode = Element.Aspect.ToUIViewContentMode();
        }

        /// <summary>
        /// Set image.
        /// </summary>
        /// <param name="oldElement"></param>
        private async Task SetImage(ExtendedImage oldElement = null)
        {
            await Task.FromResult(0);

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
                Cancel();

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

                TaskParameter imageLoader = null;

                var ffSource = ImageSourceBinding.GetImageSourceBinding(source);

                if (ffSource == null)
                {
                    if (Control != null)
                        Control.Image = null;

                    ImageLoadingFinished(Element);
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
                else if (ffSource.ImageSource == FFImageLoading.Work.ImageSource.Stream)
                {
                    imageLoader = ImageService.LoadStream(ffSource.Stream);
                }

                if (imageLoader != null)
                {
                    TaskCompletionSource<ExtendedImage> tc = new TaskCompletionSource<ExtendedImage>();
                    ExtendedImage ei = Element;

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

                    var waitedElement = await tc.Task;

                    ImageLoadingFinished(waitedElement);
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
                AC.TraceError("iOS Extended renderer", ex);
            }
            finally
            {
                LockSource.Release();
            }
        }

        /// <summary>
        /// Set opacity.
        /// </summary>
        private void SetOpacity()
        {
            Control.Opaque = Element.IsOpaque;
        }
    }
}