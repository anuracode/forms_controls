// <copyright file="ImageLoader.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using FFImageLoading;
using FFImageLoading.Work;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UIKit;

namespace Anuracode.Forms.Controls.Renderers
{
    /// <summary>
    /// Logic for loading images in queue with FFImageLoading.
    /// </summary>
    public static class ImageLoader
    {
        /// <summary>
        /// Lock for the source update.
        /// </summary>
        private static SemaphoreSlim lockSource;

        /// <summary>
        /// Lock for the source update.
        /// </summary>
        public static SemaphoreSlim LockSource
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
        /// Get the number of cores.
        /// </summary>
        /// <returns>Number of cores of the device.</returns>
        public static int GetNumberOfCores()
        {
            return 2;
        }

        /// <summary>
        /// Set the lock for the images, default count depents on the cores.
        /// </summary>
        /// <param name="newLock">New lock to use.</param>
        public static void SetLockSource(SemaphoreSlim newLock)
        {
            lockSource = newLock;
        }

        /// <summary>
        /// Update image source.
        /// </summary>
        /// <param name="control">Control to use.</param>
        /// <param name="source">Source to use.</param>
        /// <param name="lastImageSource">Last image source.</param>
        /// <param name="allowDownSample">Allow down sample.</param>
        /// <param name="targetWidth">Target width.</param>
        /// <param name="targetHeight">Target height.</param>
        /// <param name="cancellationToken">Cancel token to use.</param>
        /// <returns>Task to await.</returns>
        public static async Task<bool> UpdateImageSource(this UIImageView control, Xamarin.Forms.ImageSource source, Xamarin.Forms.ImageSource lastImageSource = null, bool allowDownSample = true, double targetWidth = 0, double targetHeight = 0, CancellationToken cancellationToken = default(CancellationToken))
        {
            await Task.FromResult(0);

            bool controlUpdatedCorrectly = false;
            IScheduledWork currentTask = null;
            TaskParameter imageLoader = null;

            if (control == null)
            {
                return false;
            }

            try
            {
                await LockSource.WaitAsync(cancellationToken);

                TaskCompletionSource<bool> tc = new TaskCompletionSource<bool>();

                try
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var ffSource = ImageSourceBinding.GetImageSourceBinding(source);

                    if (ffSource == null)
                    {
                        control.Image = null;

                        tc.SetResult(true);
                    }
                    else if (ffSource.ImageSource == FFImageLoading.Work.ImageSource.Url)
                    {
                        imageLoader = ImageService.Instance.LoadUrl(ffSource.Path, TimeSpan.FromDays(1));
                    }
                    else if (ffSource.ImageSource == FFImageLoading.Work.ImageSource.CompiledResource)
                    {
                        imageLoader = ImageService.Instance.LoadCompiledResource(ffSource.Path);
                    }
                    else if (ffSource.ImageSource == FFImageLoading.Work.ImageSource.ApplicationBundle)
                    {
                        imageLoader = ImageService.Instance.LoadFileFromApplicationBundle(ffSource.Path);
                    }
                    else if (ffSource.ImageSource == FFImageLoading.Work.ImageSource.Filepath)
                    {
                        imageLoader = ImageService.Instance.LoadFile(ffSource.Path);
                    }

                    if (imageLoader != null)
                    {
                        if (lastImageSource != null)
                        {
                            control.Image = null;
                        }

                        // Downsample
                        if (allowDownSample && (targetHeight > 0 || targetWidth > 0))
                        {
                            if (targetHeight > targetWidth)
                            {
                                imageLoader.DownSampleInDip(height: (int)targetWidth);
                            }
                            else
                            {
                                imageLoader.DownSampleInDip(width: (int)targetHeight);
                            }
                        }

                        imageLoader.Retry(2, 30000);

                        imageLoader.Finish(
                            (work) =>
                            {
                                tc.TrySetResult(true);
                            });

                        imageLoader.Error(
                            (iex) =>
                            {
                                tc.TrySetException(iex);
                            });

                        currentTask = imageLoader.Into(control);
                    }
                }
                catch (Exception ex)
                {
                    tc.SetException(ex);
                }

                controlUpdatedCorrectly = await tc.Task;
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
                AC.TraceError("iOS image loader", ex);
            }
            finally
            {
                if (!controlUpdatedCorrectly && (currentTask != null) && !currentTask.IsCancelled)
                {
                    currentTask.Cancel();
                }

                LockSource.Release();

                if (imageLoader != null)
                {
                    imageLoader.Dispose();
                }
            }

            return controlUpdatedCorrectly;
        }
    }
}