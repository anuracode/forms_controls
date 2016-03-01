// <copyright file="BaseViewModel.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Sample.Localization;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls.Sample.ViewModels
{
    /// <summary>
    /// Base view model.
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Share content.
        /// </summary>
        private Command shareContentCommand;

        /// <summary>
        /// Show main menu.
        /// </summary>
        private Command showMainMenuCommand;

        /// <summary>
        /// Title to use.
        /// </summary>
        private string title = string.Empty;
        internal static readonly string PREFIX_EMAIL;

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler InitializationComplete;

        /// <summary>
        /// Event when property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Is initialized, not bindinble.
        /// </summary>
        public bool IsInitialized { get; set; }

        /// <summary>
        /// Localization resources for the App.
        /// </summary>
        public LocalizationResources LocalizationResources
        {
            get
            {
                return App.LocalizationResources;
            }
        }

        /// <summary>
        /// Share app.
        /// </summary>
        public Command ShareContentCommand
        {
            get
            {
                if (shareContentCommand == null)
                {
                    shareContentCommand = new Command(
                        ShareContentAsync,
                        CanShareContent);
                }

                return shareContentCommand;
            }
        }

        /// <summary>
        /// Show main menu.
        /// </summary>
        public Command ShowMainMenuCommand
        {
            get
            {
                if (showMainMenuCommand == null)
                {
                    showMainMenuCommand = new Command(
                        () =>
                        {
                            if (App.ShowMainMenuCommand != null && App.ShowMainMenuCommand.CanExecute(null))
                            {
                                App.ShowMainMenuCommand.Execute(null);
                            }
                        });
                }

                return showMainMenuCommand;
            }
        }

        /// <summary>
        /// Gets or sets the "Title" property
        /// </summary>
        public virtual string Title
        {
            get
            {
                return title;
            }

            set
            {
                if (!object.Equals(title, value))
                {
                    title = value;
                    OnPropertyChanged(nameof(Title));
                }
            }
        }

        /// <summary>
        /// Handle exception from commands.
        /// </summary>
        /// <param name="ex">Exception to handle.</param>
        public virtual void HandlerCommandException(Exception ex)
        {
            AC.ScheduleManaged(
                    async () =>
                    {
                        await Task.FromResult(0);
                        try
                        {
                            if (ex is System.Threading.Tasks.TaskCanceledException)
                            {
                                return;
                            }
                            else if (ex is System.OperationCanceledException)
                            {
                                return;
                            }
                            else
                            {
                                AC.TraceError("Command exception", ex);
                            }
                        }
                        catch (Exception exe)
                        {
                            AC.TraceError("Handle exception", exe);
                        }
                    });
        }

        /// <summary>
        /// Can share content.
        /// </summary>
        /// <returns>True when can share content.</returns>
        protected virtual bool CanShareContent()
        {
            return false;
        }

        /// <summary>
        /// Notify that a property changed.
        /// </summary>
        /// <param name="propertyName">Name of the property that changed.</param>
        /// [Obsolete("Use OnPropertyChanged with expression.", false)]
        protected void OnPropertyChanged(string propertyName)
        {
            AC.ScheduleManaged(
                async () =>
            {
                await Task.FromResult(0);
                RaisePropertyChangedEvent(propertyName);
            });
        }

        /// <summary>
        /// Raise the property change.
        /// </summary>
        /// <param name="whichProperty"></param>
        protected void RaisePropertyChangedEvent(string whichProperty)
        {
            // check for subscription before going multithreaded
            if (PropertyChanged != null)
            {
                var handler = PropertyChanged;

                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs(whichProperty));
                }
            }
        }

        /// <summary>
        /// Share content of the view model.
        /// </summary>
        /// <returns>Task to await.</returns>
        protected virtual void ShareContentAsync()
        {
            // Implement.
        }
    }
}