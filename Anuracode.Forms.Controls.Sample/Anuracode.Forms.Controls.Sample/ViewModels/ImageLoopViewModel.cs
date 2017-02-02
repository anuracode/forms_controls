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

            newPaths.Add(serverImagePath + "AGUARDIENTE_REAL_1493-m.jpg");
            newPaths.Add(serverImagePath + "Artistry-m.jpg");
            newPaths.Add(serverImagePath + "BACARDI-m.jpg");
            newPaths.Add(serverImagePath + "BAILEYS-m.jpg");
            newPaths.Add(serverImagePath + "Biocros-m.jpg");
            newPaths.Add(serverImagePath + "BUCHANANS-m.jpg");
            newPaths.Add(serverImagePath + "CERVEZA_HEINEKEN_BARRIL-m.jpg");
            newPaths.Add(serverImagePath + "CHAMP_DOM_PERIGNON-m.jpg");
            newPaths.Add(serverImagePath + "CHIVAS-m.jpg");
            newPaths.Add(serverImagePath + "Choco-m.jpg");
            newPaths.Add(serverImagePath + "cintura-m.jpg");
            newPaths.Add(serverImagePath + "Cofee-m.jpg");
            newPaths.Add(serverImagePath + "CONVERSE-m.jpg");
            newPaths.Add(serverImagePath + "Dishdrops-m.jpg");
            newPaths.Add(serverImagePath + "Dymatize-m.jpg");
            newPaths.Add(serverImagePath + "Egomint-m.jpg");
            newPaths.Add(serverImagePath + "Ertia-m.jpg");
            newPaths.Add(serverImagePath + "g180-m.jpg");
            newPaths.Add(serverImagePath + "GATOS-m.jpg");
            newPaths.Add(serverImagePath + "GINEBRA_BOMBAY-m.jpg");
            newPaths.Add(serverImagePath + "gnc-m.jpg");
            newPaths.Add(serverImagePath + "GUARO_ANTIOQUENO-m.jpg");
            newPaths.Add(serverImagePath + "HAVANA-m.jpg");
            newPaths.Add(serverImagePath + "HelixNutrition-m.jpg");
            newPaths.Add(serverImagePath + "Herbalife-m.jpg");
            newPaths.Add(serverImagePath + "HL_1417-m.jpg");
            newPaths.Add(serverImagePath + "Homo-m.jpg");
            newPaths.Add(serverImagePath + "inmunidad-m.jpg");
            newPaths.Add(serverImagePath + "JACK_DANIELS-m.jpg");
            newPaths.Add(serverImagePath + "JHONNIE_WALKER-m.jpg");
            newPaths.Add(serverImagePath + "JUAN_VALDEZ-m.jpg");
            newPaths.Add(serverImagePath + "LICORES_DE_FRUTAS-m.jpg");
            newPaths.Add(serverImagePath + "Loc-m.jpg");
            newPaths.Add(serverImagePath + "MARTINI-m.jpg");
            newPaths.Add(serverImagePath + "Mascotas-m.jpg");
            newPaths.Add(serverImagePath + "Moiskin-m.jpg");
            newPaths.Add(serverImagePath + "MP_Default-m.jpg");
            newPaths.Add(serverImagePath + "muscletech-m.jpg");
            newPaths.Add(serverImagePath + "NEW_BALANCE-m.jpg");
            newPaths.Add(serverImagePath + "Nutrilite-m.jpg");
            newPaths.Add(serverImagePath + "OLD_PARR-m.jpg");
            newPaths.Add(serverImagePath + "Omniplus-m.jpg");
            newPaths.Add(serverImagePath + "Omniviu-m.jpg");
            newPaths.Add(serverImagePath + "PC_1-m.jpg");
            newPaths.Add(serverImagePath + "PC_2-m.jpg");
            newPaths.Add(serverImagePath + "PERROS_PP-m.jpg");
            newPaths.Add(serverImagePath + "PIJAMA_MUJERES-m.jpg");
            newPaths.Add(serverImagePath + "PONCHE_KUBA-m.jpg");
            newPaths.Add(serverImagePath + "powerbar-m.jpg");
            newPaths.Add(serverImagePath + "Powermaker-m.jpg");
            newPaths.Add(serverImagePath + "PP_XTREME_WHEY_2LB-m.jpg");
            newPaths.Add(serverImagePath + "P_GROUP_ANIMOX-m.jpg");
            newPaths.Add(serverImagePath + "P_GROUP_GU_ENERGY_GEL-m.jpg");
            newPaths.Add(serverImagePath + "P_GROUP_QUEST_BAR-m.jpg");
            newPaths.Add(serverImagePath + "P_GROUP_SMART_SHAKER-m.jpg");
            newPaths.Add(serverImagePath + "Relojes-m.jpg");
            newPaths.Add(serverImagePath + "RON_MEDELLIN-m.jpg");
            newPaths.Add(serverImagePath + "RON_VIEJO_CALDAS-m.jpg");
            newPaths.Add(serverImagePath + "RON_ZACAPA-m.jpg");
            newPaths.Add(serverImagePath + "SA8-m.jpg");
            newPaths.Add(serverImagePath + "Sanitique-m.jpg");
            newPaths.Add(serverImagePath + "SOMETHING_SPECIAL-m.jpg");
            newPaths.Add(serverImagePath + "Starbien-m.jpg");
            newPaths.Add(serverImagePath + "TEQUILA-m.jpg");
            newPaths.Add(serverImagePath + "TEQUILA_JOSE_CUERVO-m.jpg");

            return newPaths;
        }
    }
}