// <copyright file="MainPage.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Sample.ViewModels;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls.Sample.Views
{
    /// <summary>
    /// Main view.
    /// </summary>
    public partial class MainPage : MasterDetailPage
    {
        /// <summary>
        /// Navigation delay.
        /// </summary>
        private const double navigationDelay = 0.05;

        /// <summary>
        /// Close menu command.
        /// </summary>
        private Command closeMenuCommand;

        /// <summary>
        /// Intro navigation page.
        /// </summary>
        private NavigationPage introNavigationPage;

        /// <summary>
        /// Main menu.
        /// </summary>
        private MenuPage mainMenu;

        /// <summary>
        /// Navigate to About.
        /// </summary>
        private Command showAboutCommand;

        /// <summary>
        /// Navigate to address book.
        /// </summary>
        private Command showAddressBookCommand;

        /// <summary>
        /// Command to show the tasks.
        /// </summary>
        private Command showArticlesCommand;

        /// <summary>
        /// Navigate to cart.
        /// </summary>
        private Command showCartCommand;

        /// <summary>
        /// Show main menu.
        /// </summary>
        private Command showMainMenuCommand;

        /// <summary>
        /// Command to show the orders.
        /// </summary>
        private Command showOrdersCommand;

        /// <summary>
        /// Navigate to search.
        /// </summary>
        private Command showSearchCommand;

        /// <summary>
        /// Navigate to settings.
        /// </summary>
        private Command showSettingsCommand;

        /// <summary>
        /// Navigate to store.
        /// </summary>
        private Command showStoreCommand;

        /// <summary>
        /// Store navigation page.
        /// </summary>
        private NavigationPage storeNavigationPage;

        /// <summary>
        /// Viewmodel for the main.
        /// </summary>
        private MainViewModel viewModel;

        /// <summary>
        /// Default consturctor.
        /// </summary>
        public MainPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            this.PropertyChanged += MainPage_PropertyChanged;

            BindingContext = ViewModel;

            RenderContent();
        }

        /// <summary>
        /// Show shipment items.
        /// </summary>
        public Command CloseMenuCommand
        {
            get
            {
                if (closeMenuCommand == null)
                {
                    closeMenuCommand = new Command(
                        async () =>
                        {
                            await Task.FromResult(0);
                            AC.ScheduleManaged(
                                        TimeSpan.FromSeconds(0.05),
                                        async () =>
                                        {
                                            await Task.FromResult(0);

                                            IsPresented = false;
                                        });
                        },
                        () =>
                        {
                            return true;
                        });
                }

                return closeMenuCommand;
            }
        }

        /// <summary>
        /// Intro navigation page.
        /// </summary>
        public NavigationPage IntroNavigationPage
        {
            get { return introNavigationPage; }
            set
            {
                introNavigationPage = value;
            }
        }

        /// <summary>
        /// Navigate to About.
        /// </summary>
        public Command ShowAboutCommand
        {
            get
            {
                if (showAboutCommand == null)
                {
                    showAboutCommand = new Command(
                        () =>
                    {
                        var aboutViewModel = new AboutViewModel();

                        AboutPage newPage = new AboutPage(aboutViewModel);

                        if (StoreNavigationPage != null)
                        {
                            NavigationPage.SetHasNavigationBar(newPage, false);
                            NavigationPage.SetHasBackButton(newPage, false);
                            StoreNavigationPage.PushAsync(newPage);

                            CloseMenuCommand.Execute(null);
                        }
                    });
                }

                return showAboutCommand;
            }
        }

        /// <summary>
        /// Show address book.
        /// </summary>
        public Command ShowAddressBookCommand
        {
            get
            {
                if (showAddressBookCommand == null)
                {
                    showAddressBookCommand = new Command(
                        () =>
                        {
                            var newViewModel = new AddressListViewModel();

                            AddressesListPage newPage = new AddressesListPage(newViewModel);

                            if (StoreNavigationPage != null)
                            {
                                NavigationPage.SetHasNavigationBar(newPage, false);
                                NavigationPage.SetHasBackButton(newPage, false);
                                StoreNavigationPage.PushAsync(newPage);

                                AC.ScheduleManaged(
                                    TimeSpan.FromSeconds(0.1),
                                    async () =>
                                {
                                    await Task.FromResult(0);

                                    if (newViewModel.LoadItemsCommand.CanExecute(null))
                                    {
                                        newViewModel.LoadItemsCommand.Execute(null);
                                    }
                                });

                                CloseMenuCommand.Execute(null);
                            }
                        });
                }

                return showAddressBookCommand;
            }
        }

        /// <summary>
        /// Command to show the articles.
        /// </summary>
        public Command ShowArticlesCommand
        {
            get
            {
                if (showArticlesCommand == null)
                {
                    if (showArticlesCommand == null)
                    {
                        showArticlesCommand = new Command(AlertFunctionNotIncluded);
                    }
                }

                return showArticlesCommand;
            }
        }

        /// <summary>
        /// Navigate to cart.
        /// </summary>
        public Command ShowCartCommand
        {
            get
            {
                if (showCartCommand == null)
                {
                    showCartCommand = new Command(AlertFunctionNotIncluded);
                }

                return showCartCommand;
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
                            AC.ScheduleManaged(
                                () =>
                                {
                                    this.IsPresented = true;

                                    return Task.FromResult(0);
                                });
                        });
                }

                return showMainMenuCommand;
            }
        }

        /// <summary>
        /// Command to show the articles.
        /// </summary>
        public Command ShowOrdersCommand
        {
            get
            {
                if (showOrdersCommand == null)
                {
                    if (showOrdersCommand == null)
                    {
                        showOrdersCommand = new Command(AlertFunctionNotIncluded);
                    }
                }

                return showOrdersCommand;
            }
        }

        /// <summary>
        /// Navigate to search.
        /// </summary>
        public Command ShowSearchCommand
        {
            get
            {
                if (showSearchCommand == null)
                {
                    showSearchCommand = new Command(AlertFunctionNotIncluded);
                }

                return showSearchCommand;
            }
        }

        /// <summary>
        /// Navigate to settings.
        /// </summary>
        public Command ShowSettingsCommand
        {
            get
            {
                if (showSettingsCommand == null)
                {
                    showSettingsCommand = new Command(AlertFunctionNotIncluded);
                }

                return showSettingsCommand;
            }
        }

        /// <summary>
        /// Navigate to Store.
        /// </summary>
        public Command ShowStoreCommand
        {
            get
            {
                if (showStoreCommand == null)
                {
                    showStoreCommand = new Command(AlertFunctionNotIncluded);
                }

                return showStoreCommand;
            }
        }

        /// <summary>
        /// Store navigation page.
        /// </summary>
        public NavigationPage StoreNavigationPage
        {
            get
            {
                return storeNavigationPage;
            }

            set
            {
                storeNavigationPage = value;
            }
        }

        /// <summary>
        /// Viewmodel for the main.
        /// </summary>
        public MainViewModel ViewModel
        {
            get
            {
                if (viewModel == null)
                {
                    viewModel = new MainViewModel();
                }

                return viewModel;
            }
        }

        /// <summary>
        /// Alert funcition not added.
        /// </summary>
        protected void AlertFunctionNotIncluded()
        {
            DisplayAlert("Sample do not include function", "The sample do not include this functionallity", "Cancel");
        }

        /// <summary>
        /// Updates the gesture enable.
        /// </summary>
        /// <param name="isEnable">True when the gesture should be enabled.</param>
        protected void SetMenuGesture(bool isEnable)
        {
            this.IsGestureEnabled = isEnable;
        }

        /// <summary>
        /// Property changed.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Arguments of the event.</param>
        private void MainPage_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == MasterDetailPage.IsPresentedProperty.PropertyName)
            {
                AC.ScheduleManaged(
                    async () =>
                    {
                        UpdateCommandsStatus();

                        await Task.FromResult(0);
                    });
            }
        }

        /// <summary>
        /// Render content of the page.
        /// </summary>
        private void RenderContent()
        {
            mainMenu = new MenuPage();

            mainMenu.ExternalCloseMenuCommand = CloseMenuCommand;
            mainMenu.ExternalShowSettingsCommand = ShowSettingsCommand;
            mainMenu.ExternalShowSearchCommand = ShowSearchCommand;
            mainMenu.ExternalShowStoreCommand = ShowStoreCommand;
            mainMenu.ExternalShowAboutCommand = ShowAboutCommand;
            mainMenu.ExternalShareAppCommand = ShowArticlesCommand;
            mainMenu.ExternalShowArticlesCommand = ShowArticlesCommand;
            mainMenu.ExternalShowCartCommand = ShowCartCommand;
            mainMenu.ExternalShowAddressBookCommand = ShowAddressBookCommand;

            mainMenu.ExternalShowOrdersCommand = ShowOrdersCommand;

            MasterBehavior = Xamarin.Forms.MasterBehavior.Popover;

            if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
            {
                mainMenu.Icon = Theme.CommonResources.PathImageFeaturesAction;
            }
            else if (Device.OS == TargetPlatform.iOS)
            {
                mainMenu.Icon = Theme.CommonResources.PathImageHambuergerLogo;
            }

            Master = mainMenu;

            var introRawPage = new IntroPage();

            NavigationPage.SetHasBackButton(introRawPage, false);
            NavigationPage.SetHasNavigationBar(introRawPage, false);

            IntroNavigationPage = new NavigationPage(introRawPage);
            Detail = IntroNavigationPage;

            App.InitPageCompleteAction = () =>
            {
                AC.ScheduleManaged(
                   async () =>
                   {
                       await Task.FromResult(0);

                       App.PagesInitilized = true;

                       if (StoreNavigationPage == null)
                       {
                           var storeSearchInitialViewModel = new StoreSearchViewModel();
                           // storeSearchInitialViewModel.IsRootView = true;

                           var rawPage = new StoreSearchPage(storeSearchInitialViewModel);
                           NavigationPage.SetHasBackButton(rawPage, false);
                           NavigationPage.SetHasNavigationBar(rawPage, false);

                           StoreNavigationPage = new NavigationPage(rawPage);
                       }

                       Detail = StoreNavigationPage;
                   });
            };

            App.ShowMainMenuCommand = this.ShowMainMenuCommand;
        }

        /// <summary>
        /// Update the commands status.
        /// </summary>
        private void UpdateCommandsStatus()
        {
            ShowSettingsCommand.ChangeCanExecute();
            ShowSearchCommand.ChangeCanExecute();
            ShowStoreCommand.ChangeCanExecute();
            ShowAboutCommand.ChangeCanExecute();
            ShowArticlesCommand.ChangeCanExecute();
            ShowCartCommand.ChangeCanExecute();
            ShowAddressBookCommand.ChangeCanExecute();

            ShowOrdersCommand.ChangeCanExecute();
            CloseMenuCommand.ChangeCanExecute();
        }
    }
}