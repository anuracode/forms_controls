// <copyright file="ImageLoopViewModel.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Extensions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls.Sample.ViewModels
{
    /// <summary>
    /// View model for the image loop.
    /// </summary>
    public class ImageLoopViewModel : BaseViewModel
    {
        /// <summary>
        /// Count for the loop.
        /// </summary>
        private int currentCount;

        /// <summary>
        /// Current image path.
        /// </summary>
        private string currentImagePath;

        /// <summary>
        /// Delay in seconds to change the path.
        /// </summary>
        private double delaySeconds = 0.25;

        /// <summary>
        /// Loop token source.
        /// </summary>
        private CancellationTokenSource loopCancelTokenSource;

        /// <summary>
        /// Sample paths to use.
        /// </summary>
        private List<string> samplePaths;

        /// <summary>
        /// Command to start the loop.
        /// </summary>
        private Command startLoopCommand;

        /// <summary>
        /// Comand to stop the loop.
        /// </summary>
        private Command stopLoopCommand;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ImageLoopViewModel()
            : base()
        {
            Title = App.LocalizationResources.ImageLoopLabel;
        }

        /// <summary>
        /// Count for the loop.
        /// </summary>
        public int CurrentCount
        {
            get
            {
                return currentCount;
            }

            protected set
            {
                ValidateRaiseAndSetIfChanged(ref currentCount, value);
            }
        }

        /// <summary>
        /// Current image path.
        /// </summary>
        public string CurrentImagePath
        {
            get
            {
                return currentImagePath;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref currentImagePath, value);
            }
        }

        /// <summary>
        /// Delay in seconds to change the path.
        /// </summary>
        public double DelaySeconds
        {
            get
            {
                return delaySeconds;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref delaySeconds, value.Clamp(0.1d, 5d));
            }
        }

        /// <summary>
        /// Sample paths to use.
        /// </summary>
        public List<string> SamplePaths
        {
            get
            {
                if (samplePaths == null)
                {
                    samplePaths = InitializeSamples();
                }

                return samplePaths;
            }
        }

        /// <summary>
        /// Command to start the loop.
        /// </summary>
        public Command StartLoopCommand
        {
            get
            {
                if (startLoopCommand == null)
                {
                    startLoopCommand = new Command(
                        () =>
                        {
                            if (LoopCancelTokenSource == null)
                            {
                                LoopCancelTokenSource = new CancellationTokenSource();

                                var cancelToken = LoopCancelTokenSource.Token;
                                AC.ThreadManager.ScheduleManagedFull(
                                    async () =>
                                    {
                                        try
                                        {
                                            CurrentCount = 0;

                                            while (!cancelToken.IsCancellationRequested)
                                            {
                                                cancelToken.ThrowIfCancellationRequested();

                                                for (int i = 0; i < SamplePaths.Count; i++)
                                                {
                                                    CurrentImagePath = SamplePaths[i];

                                                    await Task.Delay(TimeSpan.FromSeconds(DelaySeconds));
                                                    cancelToken.ThrowIfCancellationRequested();
                                                    CurrentCount++;
                                                }
                                            }
                                        }
                                        catch (TaskCanceledException)
                                        {
                                        }
                                        catch (OperationCanceledException)
                                        {
                                        }
                                        finally
                                        {
                                            LoopCancelTokenSource = null;
                                        }
                                    });
                            }
                        },
                        () =>
                        {
                            return LoopCancelTokenSource == null;
                        });
                }

                return startLoopCommand;
            }
        }

        /// <summary>
        /// Command to stop the loop.
        /// </summary>
        public Command StopLoopCommand
        {
            get
            {
                if (stopLoopCommand == null)
                {
                    stopLoopCommand = new Command(
                        () =>
                        {
                            AC.ThreadManager.ScheduleManagedFull(
                                async () =>
                                {
                                    await Task.FromResult(0);

                                    if (LoopCancelTokenSource != null && !LoopCancelTokenSource.IsCancellationRequested)
                                    {
                                        LoopCancelTokenSource.Cancel();
                                    }
                                });
                        },
                        () =>
                        {
                            return LoopCancelTokenSource != null;
                        });
                }

                return stopLoopCommand;
            }
        }

        /// <summary>
        /// Loop token source.
        /// </summary>
        protected CancellationTokenSource LoopCancelTokenSource
        {
            get
            {
                return loopCancelTokenSource;
            }

            set
            {
                loopCancelTokenSource = value;

                StartLoopCommand.ChangeCanExecute();
                StopLoopCommand.ChangeCanExecute();
            }
        }

        /// <summary>
        /// List of sample paths.
        /// </summary>
        /// <returns>Paths to use.</returns>
        protected virtual List<string> InitializeSamples()
        {
            List<string> newPaths = new List<string>();

            var serverImagePath = "http://storagetest.parcero.com.co/resources/";

            //newPaths.Add(serverImagePath + "");

            newPaths.Add(serverImagePath + "ADAPTADOR_WIFI-m.jpg");
            newPaths.Add(serverImagePath + "cintura-m.jpg");
            newPaths.Add(serverImagePath + "DR_DRAGON_CONCENTRATE-1.jpg");
            newPaths.Add(serverImagePath + "DR_DRAGON_CONCENTRATE-2.jpg");
            newPaths.Add(serverImagePath + "DR_DRAGON_CONCENTRATE-t.jpg");            

            return newPaths;
        }
    }
}