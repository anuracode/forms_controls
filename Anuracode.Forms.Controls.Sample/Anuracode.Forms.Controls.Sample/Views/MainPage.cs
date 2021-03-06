﻿// <copyright file="MainPage.cs" company="Anura Code">
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
        /// Navigate to cart.
        /// </summary>
        private Command showCartCommand;

        /// <summary>
        /// Navigate to image.
        /// </summary>
        private Command showImageLoopSampleCommand;

        /// <summary>
        /// Navigate to image.
        /// </summary>
        private Command showImageSampleCommand;

        /// <summary>
        /// Show main menu.
        /// </summary>
        private Command showMainMenuCommand;

        /// <summary>
        /// Command to show the orders.
        /// </summary>
        private Command showOrdersCommand;

        /// <summary>
        /// Command to show the tasks.
        /// </summary>
        private Command showProfileCommand;

        /// <summary>
        /// Navigate to search.
        /// </summary>
        private Command showSearchCommand;

        /// <summary>
        /// Navigate to store.
        /// </summary>
        private Command showStoreCommand;

        /// <summary>
        /// Navigate to web view.
        /// </summary>
        private Command showWebViewCommand;

        /// <summary>
        /// Navigate to signature.
        /// </summary>
        private Command signatureCommand;

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
            SetMenuGesture(false);

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
        /// Navigate to sample.
        /// </summary>
        public Command ShowImageLoopSampleCommand
        {
            get
            {
                if (showImageLoopSampleCommand == null)
                {
                    showImageLoopSampleCommand = new Command(
                        () =>
                        {
                            var newViewModel = new ImageLoopViewModel();

                            var newPage = new ImageLoopPage(newViewModel);

                            if (StoreNavigationPage != null)
                            {
                                NavigationPage.SetHasNavigationBar(newPage, false);
                                NavigationPage.SetHasBackButton(newPage, false);
                                StoreNavigationPage.PushAsync(newPage);

                                CloseMenuCommand.Execute(null);
                            }
                        });
                }

                return showImageLoopSampleCommand;
            }
        }

        /// <summary>
        /// Navigate to sample.
        /// </summary>
        public Command ShowImageSampleCommand
        {
            get
            {
                if (showImageSampleCommand == null)
                {
                    showImageSampleCommand = new Command(
                        () =>
                        {
                            var newViewModel = new ImageSampleViewModel();

                            var newPage = new ImageSamplePage(newViewModel);

                            if (StoreNavigationPage != null)
                            {
                                NavigationPage.SetHasNavigationBar(newPage, false);
                                NavigationPage.SetHasBackButton(newPage, false);
                                StoreNavigationPage.PushAsync(newPage);

                                CloseMenuCommand.Execute(null);
                            }
                        });
                }

                return showImageSampleCommand;
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
        /// Command to show the articles.
        /// </summary>
        public Command ShowProfileCommand
        {
            get
            {
                if (showProfileCommand == null)
                {
                    showProfileCommand = new Command(
                        () =>
                        {
                            var newViewModel = new ProfileViewModel();

                            ProfilePage newPage = new ProfilePage(newViewModel);

                            if (StoreNavigationPage != null)
                            {
                                NavigationPage.SetHasNavigationBar(newPage, false);
                                NavigationPage.SetHasBackButton(newPage, false);
                                StoreNavigationPage.PushAsync(newPage);

                                CloseMenuCommand.Execute(null);
                            }
                        });
                }

                return showProfileCommand;
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
        /// Navigate to web view.
        /// </summary>
        public Command ShowWebViewCommand
        {
            get
            {
                if (showWebViewCommand == null)
                {
                    showWebViewCommand = new Command(
                        () =>
                        {
                            var newViewModel = new WebViewViewModel();

                            var newPage = new WebViewPage(newViewModel);

                            if (StoreNavigationPage != null)
                            {
                                NavigationPage.SetHasNavigationBar(newPage, false);
                                NavigationPage.SetHasBackButton(newPage, false);
                                StoreNavigationPage.PushAsync(newPage);

                                CloseMenuCommand.Execute(null);
                            }
                        });
                }

                return showWebViewCommand;
            }
        }

        /// <summary>
        /// Navigate to About.
        /// </summary>
        public Command SignatureCommand
        {
            get
            {
                if (signatureCommand == null)
                {
                    signatureCommand = new Command(
                        () =>
                        {
                            var newViewModel = new SignatureViewModel();

                            var newPage = new SignaturePage(newViewModel);

                            if (StoreNavigationPage != null)
                            {
                                NavigationPage.SetHasNavigationBar(newPage, false);
                                NavigationPage.SetHasBackButton(newPage, false);
                                StoreNavigationPage.PushAsync(newPage);

                                CloseMenuCommand.Execute(null);
                            }
                        });
                }

                return signatureCommand;
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
            mainMenu.ExternalShowImageSampleCommand = ShowImageSampleCommand;
            mainMenu.ExternalShowImageLoopSampleCommand = ShowImageLoopSampleCommand;
            mainMenu.ExternalShowSearchCommand = ShowSearchCommand;
            mainMenu.ExternalShowStoreCommand = ShowStoreCommand;
            mainMenu.ExternalShowAboutCommand = ShowAboutCommand;
            mainMenu.ExternalShowWebViewCommand = ShowWebViewCommand;
            mainMenu.ExternalShowProfileCommand = ShowProfileCommand;
            mainMenu.ExternalShowCartCommand = ShowCartCommand;
            mainMenu.ExternalShowAddressBookCommand = ShowAddressBookCommand;
            mainMenu.ExternalSignatureCommand = SignatureCommand;

            mainMenu.ExternalShowOrdersCommand = ShowOrdersCommand;

            MasterBehavior = Xamarin.Forms.MasterBehavior.Popover;

            if (Device.RuntimePlatform == DeviceExtension.WinPhone || Device.RuntimePlatform == DeviceExtension.UWP || Device.RuntimePlatform == DeviceExtension.WinRT)
            {
                mainMenu.Icon = Theme.CommonResources.PathImageFeaturesAction;
            }
            else if (Device.RuntimePlatform == DeviceExtension.iOS)
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
            ShowImageLoopSampleCommand.ChangeCanExecute();
            ShowImageSampleCommand.ChangeCanExecute();
            ShowSearchCommand.ChangeCanExecute();
            ShowStoreCommand.ChangeCanExecute();
            ShowWebViewCommand.ChangeCanExecute();
            ShowAboutCommand.ChangeCanExecute();
            ShowProfileCommand.ChangeCanExecute();
            ShowCartCommand.ChangeCanExecute();
            ShowAddressBookCommand.ChangeCanExecute();
            SignatureCommand.ChangeCanExecute();

            ShowOrdersCommand.ChangeCanExecute();
            CloseMenuCommand.ChangeCanExecute();
        }
    }
}