// <copyright file="MenuPage.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Anuracode.Forms.Controls.Extensions;

namespace Anuracode.Forms.Controls.Sample.Views
{
    /// <summary>
    /// Menu page.
    /// </summary>
    public class MenuPage : BaseView
    {
        /// <summary>
        /// Delegate command to use.
        /// </summary>
        private Command externalShowProfileCommand;

        /// <summary>
        /// Command for the logo.
        /// </summary>
        private Command logoAnimateCommand;

        /// <summary>
        /// Show profile command.
        /// </summary>
        private Command showProfileCommand;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="viewModel">View model to use.</param>
        public MenuPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
            {
                this.Title = "Menu";
            }
            else
            {
                this.Title = "Sample app";
            }

            Content = RenderContent();
        }

        /// <summary>
        /// Button to use.
        /// </summary>
        public ContentViewButton ButtonStore { get; private set; }

        /// <summary>
        /// Button to use.
        /// </summary>
        public ContentViewButton ButtonStoreAlt { get; private set; }

        /// <summary>
        /// Close menu command.
        /// </summary>
        public Command ExternalCloseMenuCommand { get; set; }

        /// <summary>
        /// Share app.
        /// </summary>
        public Command ExternalShareAppCommand { get; set; }

        /// <summary>
        /// Navigate to About.
        /// </summary>
        public Command ExternalShowAboutCommand { get; set; }

        /// <summary>
        /// Command to show the address book.
        /// </summary>
        public Command ExternalShowAddressBookCommand { get; set; }

        /// <summary>
        /// Command to show the tasks.
        /// </summary>
        public Command ExternalShowArticlesCommand { get; set; }

        /// <summary>
        /// Navigate to cart.
        /// </summary>
        public Command ExternalShowCartCommand { get; set; }

        /// <summary>
        /// Navigate to deposits.
        /// </summary>
        public Command ExternalShowDepotsCommand { get; set; }

        /// <summary>
        /// Navigate to items.
        /// </summary>
        public Command ExternalShowItemsTrackingDeliveryCommand { get; set; }

        /// <summary>
        /// Navigate to items.
        /// </summary>
        public Command ExternalShowItemsTrackingDispatchCommand { get; set; }

        /// <summary>
        /// Navigate to items.
        /// </summary>
        public Command ExternalShowItemsTrackingProviderCommand { get; set; }

        /// <summary>
        /// Command to show the orders.
        /// </summary>
        public Command ExternalShowOrdersCommand { get; set; }

        /// <summary>
        /// Delegate command to use.
        /// </summary>
        public Command ExternalShowProfileCommand
        {
            get
            {
                return externalShowProfileCommand;
            }

            set
            {
                externalShowProfileCommand = value;

                OnPropertyChanged("ExternalShowProfileCommand");
                ShowProfileCommand.ChangeCanExecute();
            }
        }

        /// <summary>
        /// Navigate to search.
        /// </summary>
        public Command ExternalShowSearchCommand { get; set; }

        /// <summary>
        /// Navigate to settings.
        /// </summary>
        public Command ExternalShowSettingsCommand { get; set; }

        /// <summary>
        /// Navigate to store.
        /// </summary>
        public Command ExternalShowStoreCommand { get; set; }

        /// <summary>
        /// Navigate to store items.
        /// </summary>
        public Command ExternalShowStoreItemsCommand { get; set; }

        /// <summary>
        /// Is staff.
        /// </summary>
        public bool IsStaff { get; set; }

        /// <summary>
        /// Command for the logo.
        /// </summary>
        public Command LogoAnimateCommand
        {
            get
            {
                if (logoAnimateCommand == null)
                {
                    logoAnimateCommand = new Command(
                        async () =>
                        {
                            await Task.Delay(1000);
                        });
                }

                return logoAnimateCommand;
            }
        }

        /// <summary>
        /// Show profile command.
        /// </summary>
        public Command ShowProfileCommand
        {
            get
            {
                if (showProfileCommand == null)
                {
                    showProfileCommand = new Command(
                        async () =>
                        {
                            await Task.FromResult(0);

                            if (ExternalShowProfileCommand != null)
                            {
                                ExternalShowProfileCommand.Execute(null);
                            }
                        },
                        () =>
                        {
                            return ExternalShowProfileCommand != null && ExternalShowProfileCommand.CanExecute(null);
                        });
                }

                return showProfileCommand;
            }
        }

        /// <summary>
        /// Bottom separator.
        /// </summary>
        protected View BottomLineSeparator { get; private set; }

        /// <summary>
        /// Button to use.
        /// </summary>
        protected GlyphContentViewButton ButtonAbout { get; private set; }

        /// <summary>
        /// Button to use.
        /// </summary>
        protected GlyphContentViewButton ButtonAboutAlt { get; private set; }

        /// <summary>
        /// Button to use.
        /// </summary>
        protected ContentViewButton ButtonAddressBook { get; private set; }

        /// <summary>
        /// Button to use.
        /// </summary>
        protected GlyphContentViewButton ButtonArticles { get; private set; }

        /// <summary>
        /// Button to use.
        /// </summary>
        protected GlyphContentViewButton ButtonArticlesAlt { get; private set; }

        /// <summary>
        /// Button to use.
        /// </summary>
        protected ContentViewButton ButtonCart { get; private set; }

        /// <summary>
        /// Button to use.
        /// </summary>
        protected ContentViewButton ButtonCartAlt { get; private set; }

        /// <summary>
        /// Button depots.
        /// </summary>
        protected ContentViewButton ButtonDepots { get; private set; }

        /// <summary>
        /// Button delivery.
        /// </summary>
        protected ContentViewButton ButtonItemsTrackingDelivery { get; private set; }

        /// <summary>
        /// Button dispatcher.
        /// </summary>
        protected ContentViewButton ButtonItemsTrackingDispatch { get; private set; }

        /// <summary>
        /// Button provider.
        /// </summary>
        protected ContentViewButton ButtonItemsTrackingProvider { get; private set; }

        /// <summary>
        /// Button orders.
        /// </summary>
        protected ContentViewButton ButtonOrders { get; private set; }

        /// <summary>
        /// Button orders.
        /// </summary>
        protected ContentViewButton ButtonOrdersAlt { get; private set; }

        /// <summary>
        /// Button to use.
        /// </summary>
        protected ContentViewButton ButtonSearch { get; private set; }

        /// <summary>
        /// Button to use.
        /// </summary>
        protected ContentViewButton ButtonSearchAlt { get; private set; }

        /// <summary>
        /// Button for settings.
        /// </summary>
        protected ContentViewButton ButtonSettings { get; private set; }

        /// <summary>
        /// Button for settings.
        /// </summary>
        protected ContentViewButton ButtonSettingsAlt { get; private set; }

        /// <summary>
        /// Button to use.
        /// </summary>
        protected ContentViewButton ButtonShare { get; private set; }

        /// <summary>
        /// Button to use.
        /// </summary>
        protected ContentViewButton ButtonShareAlt { get; private set; }

        /// <summary>
        /// Button products.
        /// </summary>
        protected ContentViewButton ButtonStoreItems { get; private set; }

        /// <summary>
        /// Close button.
        /// </summary>
        protected ContentViewButton CloseButton { get; private set; }

        /// <summary>
        /// Image logo.
        /// </summary>
        protected ImageContentViewButton ImageLogoButton { get; set; }

        /// <summary>
        /// True when the view is been loaded again (from back navigation probably).
        /// </summary>
        protected bool IsRecylced { get; private set; }

        /// <summary>
        /// The view is small.
        /// </summary>
        protected bool IsSmallView { get; set; }

        /// <summary>
        /// Label admin title.
        /// </summary>
        protected ExtendedLabel LabelAdminTitle { get; set; }

        /// <summary>
        /// Main options.
        /// </summary>
        protected SimpleLayout LayoutMainOptions { get; set; }

        /// <summary>
        /// Menu scroll.
        /// </summary>
        protected ScrollView MenuScroll { get; private set; }


        /// <summary>
        /// Stack for the alternative option.
        /// </summary>
        protected StackLayout StackAlternativeOptions { get; set; }

        /// <summary>
        /// Stack bottom option.
        /// </summary>
        protected StackLayout StackBottomOptions { get; set; }

        /// <summary>
        /// Top line separator.
        /// </summary>
        protected View TopLineSeparator { get; private set; }

        /// <summary>
        /// Layout children.
        /// </summary>
        /// <param name="x">Top to use.</param>
        /// <param name="y">Left to use.</param>
        /// <param name="width">Width to use.</param>
        /// <param name="height">Height to use.</param>
        protected void ContentLayout_OnLayoutChildren(double x, double y, double width, double height)
        {
            double buttonWidth = width * 0.5f;

            Rectangle tlPosition = new Rectangle();
            Rectangle trPosition = new Rectangle();
            Rectangle blPosition = new Rectangle();
            Rectangle brPosition = new Rectangle();

            if (ButtonStore != null)
            {
                double elementLeft = 0;
                double elementTop = 0;
                double elementWidth = buttonWidth;
                double elementHeight = buttonWidth;
                tlPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                if (ButtonStore.HeightRequest != elementHeight)
                {
                    ButtonStore.HeightRequest = elementHeight;
                }

                if (ButtonStore.WidthRequest != elementWidth)
                {
                    ButtonStore.WidthRequest = elementWidth;
                }

                ButtonStore.LayoutUpdate(tlPosition);
            }

            if (ButtonSearch != null)
            {
                double elementLeft = buttonWidth;
                double elementTop = 0;
                double elementWidth = buttonWidth;
                double elementHeight = buttonWidth;
                trPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                if (ButtonSearch.HeightRequest != elementHeight)
                {
                    ButtonSearch.HeightRequest = elementHeight;
                }

                if (ButtonSearch.WidthRequest != elementWidth)
                {
                    ButtonSearch.WidthRequest = elementWidth;
                }

                ButtonSearch.LayoutUpdate(trPosition);
            }

            if (ButtonCart != null)
            {
                double elementLeft = 0;
                double elementTop = buttonWidth;
                double elementWidth = buttonWidth;
                double elementHeight = buttonWidth;
                blPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                if (ButtonCart.HeightRequest != elementHeight)
                {
                    ButtonCart.HeightRequest = elementHeight;
                }

                if (ButtonCart.WidthRequest != elementWidth)
                {
                    ButtonCart.WidthRequest = elementWidth;
                }

                ButtonCart.LayoutUpdate(blPosition);
            }

            if (ButtonOrders != null)
            {
                double elementLeft = buttonWidth;
                double elementTop = buttonWidth;
                double elementWidth = buttonWidth;
                double elementHeight = buttonWidth;
                brPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                if (ButtonOrders.HeightRequest != elementHeight)
                {
                    ButtonOrders.HeightRequest = elementHeight;
                }

                if (ButtonOrders.WidthRequest != elementWidth)
                {
                    ButtonOrders.WidthRequest = elementWidth;
                }

                ButtonOrders.LayoutUpdate(brPosition);
            }

            double imageWidthPercent = 0.4;

            if (ImageLogoButton != null)
            {
                double elementWidth = (buttonWidth * imageWidthPercent);
                double elementHeight = elementWidth;
                double elementLeft = (width - elementWidth) * 0.5f;
                double elementTop = (height - elementHeight) * 0.5f;

                Rectangle elementPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                double imageWidth = elementWidth * 0.9;

                if (ImageLogoButton.ImageHeightRequest != imageWidth)
                {
                    ImageLogoButton.ImageHeightRequest = imageWidth;
                }

                if (ImageLogoButton.ImageWidthRequest != imageWidth)
                {
                    ImageLogoButton.ImageWidthRequest = imageWidth;
                }

                if (ImageLogoButton.HeightRequest != elementHeight)
                {
                    ImageLogoButton.HeightRequest = elementHeight;
                }

                if (ImageLogoButton.WidthRequest != elementWidth)
                {
                    ImageLogoButton.WidthRequest = elementWidth;
                }

                ImageLogoButton.LayoutUpdate(elementPosition);
            }
        }

        /// <summary>
        /// Mesure the content.
        /// </summary>
        /// <param name="widthConstraint">Width to constarint.</param>
        /// <param name="heightConstraint">Height to constraint.</param>
        /// <returns>Size to use.</returns>
        protected virtual SizeRequest ContentLayout_OnSizeRequest(double widthConstraint, double heightConstraint)
        {
            SizeRequest resultRequest = new SizeRequest(new Size(widthConstraint, widthConstraint), new Size(widthConstraint, widthConstraint));

            return resultRequest;
        }

        /// <summary>
        /// Page appears.
        /// </summary>
        protected override void OnAppearing()
        {
            if (this.Content == null)
            {
                Content = RenderContent();
            }

            UpdateCommands();

            UpdateSmallViewVisibility();

            base.OnAppearing();
        }

        /// <summary>
        /// Event when desappearing.
        /// </summary>
        protected override void OnDisappearing()
        {
            if (!IsRecylced)
            {
                IsRecylced = true;
            }

            if (BindingContext != null)
            {
                BindingContext = null;
            }

            base.OnDisappearing();
        }

        /// <summary>
        /// Check orientation.
        /// </summary>
        /// <param name="width">Width to use.</param>
        /// <param name="height">Height to use.</param>
        protected override void OnSizeAllocated(double width, double height)
        {
            try
            {
                base.OnSizeAllocated(width, height);

                if ((Content != null) && (MenuScroll != null) && (MenuScroll.Content != null))
                {
                    double contentHeight = MenuScroll.Content.GetSizeRequest(ContentViewButton.MAX_WIDTH, ContentViewButton.MAX_HEIGHT).Request.Height;
                    var calculatedHeight = height;

                    if (contentHeight < calculatedHeight)
                    {
                        MenuScroll.HeightRequest = contentHeight.Clamp(-1, ContentViewButton.MAX_HEIGHT);
                    }
                    else
                    {
                        MenuScroll.HeightRequest = -1;
                    }

                    if (Device.OS == TargetPlatform.Windows)
                    {
                        IsSmallView = ((width < 290) && (width > 0)) || ((height < 300) && (height > 0));
                    }
                    else if (Device.Idiom == TargetIdiom.Phone)
                    {
                        IsSmallView = width > height;
                    }
                    else
                    {
                        IsSmallView = false;
                    }

                    UpdateSmallViewVisibility();
                }
            }
            catch (System.Exception ex)
            {
                AC.TraceError("OnSizeAllocation", ex);
            }
        }

        /// <summary>
        /// Render the page layout.
        /// </summary>
        /// <returns>View to use.</returns>
        protected virtual View RenderContent()
        {
            this.Style = Theme.ApplicationStyles.MainMenuPageStyle;
            this.BackgroundColor = Theme.CommonResources.PagesBackgroundColorLight;

            Style topMenuOptionsStyle = new Style(typeof(ContentViewButton))
            {
                BasedOn = Theme.ApplicationStyles.MainMenuContentButtonStyle
            };

            Color darkAccent = Theme.CommonResources.AccentDark;
            Color lightAccent = Theme.CommonResources.AccentLight;

            double mainButtonImageSize = Theme.CommonResources.RoundedButtonWidth * 2;

            topMenuOptionsStyle.Setters.Add(ContentViewButton.CornerRadiusProperty, 0);
            topMenuOptionsStyle.Setters.Add(GlyphContentViewButton.GlyphTextColorProperty, Theme.CommonResources.TextColorSection);
            topMenuOptionsStyle.Setters.Add(GlyphContentViewButton.GlyphFontSizeProperty, Theme.CommonResources.TextSizeLarge * 1.5f);
            topMenuOptionsStyle.Setters.Add(ContentViewButton.FontSizeProperty, Theme.CommonResources.TextSizeMedium);
            topMenuOptionsStyle.Setters.Add(ContentViewButton.StrokeColorProperty, darkAccent);
            topMenuOptionsStyle.Setters.Add(ContentViewButton.StrokeWidthProperty, 1);
            topMenuOptionsStyle.Setters.Add(ContentViewButton.ButtonBackgroundColorProperty, lightAccent);
            topMenuOptionsStyle.Setters.Add(GlyphContentViewButton.DisableGlyphOnlyProperty, true);
            topMenuOptionsStyle.Setters.Add(ContentViewButton.UseDisableBoxProperty, false);
            topMenuOptionsStyle.Setters.Add(ContentViewButton.ImageHeightRequestProperty, mainButtonImageSize);
            topMenuOptionsStyle.Setters.Add(ContentViewButton.ImageWidthRequestProperty, mainButtonImageSize);
            topMenuOptionsStyle.Setters.Add(ContentViewButton.MarginElementsProperty, 5);
            topMenuOptionsStyle.Setters.Add(ContentViewButton.MarginBordersProperty, 0);
            topMenuOptionsStyle.Setters.Add(ContentViewButton.LineBreakModeProperty, LineBreakMode.WordWrap);

            Style extraMenuOptionsStyle = Theme.ApplicationStyles.TextOnlyContentButtonStyle;
            extraMenuOptionsStyle.Setters.Add(GlyphContentViewButton.GlyphTextColorProperty, Theme.CommonResources.AccentLight);
            extraMenuOptionsStyle.Setters.Add(GlyphContentViewButton.TextColorProperty, Theme.CommonResources.DefaultLabelTextColor);
            extraMenuOptionsStyle.Setters.Add(GlyphContentViewButton.GlyphTextProperty, Theme.CommonResources.GlyphTextBullet);
            extraMenuOptionsStyle.Setters.Add(ContentViewButton.MarginElementsProperty, 5);
            extraMenuOptionsStyle.Setters.Add(ContentViewButton.ContentAlignmentProperty, TextAlignment.Start);
            extraMenuOptionsStyle.Setters.Add(ContentViewButton.CornerRadiusProperty, 5);
            extraMenuOptionsStyle.Setters.Add(ContentViewButton.MarginBordersProperty, 1);

            StackLayout menuLayout = new StackLayout()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Vertical,
                Style = Theme.ApplicationStyles.SimpleStackContainerStyle
            };

            AC.ThreadManager.ScheduleManaged(
                TimeSpan.FromSeconds(0.5),
                () =>
                {
                    // Profile view.
                    ProfileSmallView = new ProfileSmallView(ShowProfileCommand, Theme.CommonResources.Accent)
                    {
                        VerticalOptions = LayoutOptions.Start,
                        HorizontalOptions = LayoutOptions.FillAndExpand
                    };

                    menuLayout.Children.Insert(0, ProfileSmallView);

                    double menuBarHeight = Theme.CommonResources.RoundedButtonWidth;
                    Color statusBarColor = Theme.CommonResources.Accent;

                    StackLayout stackMenuCloseBar = new StackLayout()
                    {
                        Style = Theme.ApplicationStyles.SimpleStackContainerStyle,
                        HeightRequest = menuBarHeight,
                        Orientation = StackOrientation.Horizontal,
                        BackgroundColor = Theme.CommonResources.Accent
                    };

                    CloseButton = new GlyphOnlyContentViewButton()
                    {
                        Style = Theme.ApplicationStyles.GlyphOnlyRoundedContentButtonStyle,
                        ShapeType = ShapeType.Box,
                        CornerRadius = 0,
                        GlyphText = Theme.CommonResources.GlyphTextCancel,
                        Command = ExternalCloseMenuCommand,
                        ButtonBackgroundColor = Theme.CommonResources.Accent,
                        HeightRequest = menuBarHeight,
                        WidthRequest = menuBarHeight
                    };

                    stackMenuCloseBar.Children.Add(CloseButton);

                    RenderUtil.RenderSpace(stackMenuCloseBar);

                    ExtendedLabel titleLabel = new ExtendedLabel()
                    {
                        Text = App.LocalizationResources.ApplicationTitle,
                        Style = Theme.ApplicationStyles.FormExtendedLabelStyle,
                        TextColor = Theme.CommonResources.TextColorSection,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        VerticalTextAlignment = TextAlignment.Center,
                        HorizontalTextAlignment = TextAlignment.Start,
                        Opacity = 0
                    };

                    stackMenuCloseBar.Children.Add(titleLabel);

                    menuLayout.Children.Insert(0, stackMenuCloseBar);

                    var space = new BoxView()
                    {
                        Color = statusBarColor,
                        HeightRequest = StoreListPage.StatusBarHeight,
                        VerticalOptions = LayoutOptions.Start,
                        HorizontalOptions = LayoutOptions.FillAndExpand
                    };

                    menuLayout.Children.Insert(0, space);
                });

            MenuScroll = new ScrollView()
            {
                Orientation = ScrollOrientation.Vertical,
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            LayoutMainOptions = new SimpleLayout()
            {
                IsHandlingLayoutManually = true,
                Padding = 0
            };

            LayoutMainOptions.OnLayoutChildren += ContentLayout_OnLayoutChildren;
            LayoutMainOptions.ManualSizeCalculationDelegate = ContentLayout_OnSizeRequest;

            // Container.
            StackAlternativeOptions = new StackLayout()
            {
                Style = Theme.ApplicationStyles.SimpleStackContainerStyle,
                VerticalOptions = LayoutOptions.Start
            };

            // Container.
            StackLayout stackOptions = new StackLayout()
            {
                Style = Theme.ApplicationStyles.SimpleStackContainerStyle,
                VerticalOptions = LayoutOptions.Start
            };

            stackOptions.Children.Add(LayoutMainOptions);

            // Store
            ButtonStore = new GlyphTopContentViewButton()
            {
                Style = topMenuOptionsStyle,
                GlyphText = Theme.CommonResources.GlyphTextStore,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Text = App.LocalizationResources.StoreLabel,
                Command = ExternalShowStoreCommand
            };

            ButtonStoreAlt = new GlyphLeftContentViewButton()
            {
                Style = extraMenuOptionsStyle,
                GlyphText = Theme.CommonResources.GlyphTextStore,
                Text = App.LocalizationResources.StoreLabel,
                Command = ExternalShowStoreCommand
            };

            StackAlternativeOptions.Children.Add(ButtonStoreAlt);

            LayoutMainOptions.Children.Add(ButtonStore);

            // Cart
            ButtonCart = new GlyphTopContentViewButton()
            {
                Style = topMenuOptionsStyle,
                GlyphText = Theme.CommonResources.GlyphTextCart,
                GlyphFriendlyFontName = Theme.CommonResources.GlyphFriendlyFontNameAlternate,
                GlyphFontName = Theme.CommonResources.GlyphFontNameAlternate,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Text = App.LocalizationResources.ShoppingCartLabelDiveded,
                Command = ExternalShowCartCommand
            };

            ButtonCartAlt = new GlyphLeftContentViewButton()
            {
                Style = extraMenuOptionsStyle,
                GlyphText = Theme.CommonResources.GlyphTextCart,
                GlyphFriendlyFontName = Theme.CommonResources.GlyphFriendlyFontNameAlternate,
                GlyphFontName = Theme.CommonResources.GlyphFontNameAlternate,
                Text = App.LocalizationResources.ShoppingCartLabel,
                Command = ExternalShowCartCommand
            };

            StackAlternativeOptions.Children.Add(ButtonCartAlt);

            LayoutMainOptions.Children.Add(ButtonCart);

            // Search
            ButtonSearch = new GlyphTopContentViewButton()
            {
                Style = topMenuOptionsStyle,
                GlyphText = Theme.CommonResources.GlyphTextSearch,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Text = App.LocalizationResources.SearchButton,
                Command = ExternalShowSearchCommand
            };

            ButtonSearchAlt = new GlyphLeftContentViewButton()
            {
                Style = extraMenuOptionsStyle,
                GlyphText = Theme.CommonResources.GlyphTextSearch,
                Text = App.LocalizationResources.SearchButton,
                Command = ExternalShowSearchCommand
            };

            StackAlternativeOptions.Children.Add(ButtonSearchAlt);

            LayoutMainOptions.Children.Add(ButtonSearch);

            // Orders
            ButtonOrders = new GlyphTopContentViewButton()
            {
                Style = topMenuOptionsStyle,
                GlyphText = Theme.CommonResources.GlyphTextOrders,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Text = App.LocalizationResources.OrdersLabel,
                Command = ExternalShowOrdersCommand
            };

            ButtonOrdersAlt = new GlyphLeftContentViewButton()
            {
                Style = extraMenuOptionsStyle,
                GlyphText = Theme.CommonResources.GlyphTextOrders,
                Text = App.LocalizationResources.OrdersLabel,
                Command = ExternalShowOrdersCommand
            };

            StackAlternativeOptions.Children.Add(ButtonOrdersAlt);

            LayoutMainOptions.Children.Add(ButtonOrders);

            // Logged in staff operations.
            var stackOtherOperations = RenderUtil.RenderIdentedVerticalContainer(stackOptions);

            RenderUtil.RenderSpace(stackOtherOperations);

            stackOtherOperations.Children.Add(StackAlternativeOptions);

            // Profile option.
            ButtonAddressBook = new GlyphLeftContentViewButton()
            {
                Style = extraMenuOptionsStyle,
                GlyphText = Theme.CommonResources.GlyphTextAddressBook,
                Text = App.LocalizationResources.AddressesLabel,
                InvisibleWhenDisabled = true,
                Command = ExternalShowAddressBookCommand
            };

            stackOtherOperations.Children.Add(ButtonAddressBook);

            // Name
            LabelAdminTitle = new ExtendedLabel()
            {
                Style = Theme.ApplicationStyles.DefaultExtendedLabelStyle,
                FontSize = Theme.CommonResources.TextSizeMedium,
                VerticalOptions = LayoutOptions.End,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                LineBreakMode = LineBreakMode.TailTruncation,
                Text = App.LocalizationResources.AdvancedOptionsLabel,
                FriendlyFontName = Theme.CommonResources.FontRobotBoldCondensedFriendlyName,
                FontName = Theme.CommonResources.FontRobotBoldCondensedName,
                TextColor = Theme.CommonResources.DefaultLabelSubtitleTextColor,
                IsVisible = IsStaff
            };

            stackOtherOperations.Children.Add(LabelAdminTitle);

            ButtonItemsTrackingDispatch = new GlyphLeftContentViewButton()
            {
                Style = extraMenuOptionsStyle,
                Text = App.LocalizationResources.ShipmentInventoryGuyLabel,
                InvisibleWhenDisabled = true,
                Command = ExternalShowItemsTrackingDispatchCommand
            };

            stackOtherOperations.Children.Add(ButtonItemsTrackingDispatch);

            ButtonItemsTrackingProvider = new GlyphLeftContentViewButton()
            {
                Style = extraMenuOptionsStyle,
                Text = App.LocalizationResources.ShipmentProviderGuyLabel,
                InvisibleWhenDisabled = true,
                Command = ExternalShowItemsTrackingProviderCommand
            };

            stackOtherOperations.Children.Add(ButtonItemsTrackingProvider);

            ButtonItemsTrackingDelivery = new GlyphLeftContentViewButton()
            {
                Style = extraMenuOptionsStyle,
                Text = App.LocalizationResources.ShipmentDeliveryGuyLabel,
                InvisibleWhenDisabled = true,
                Command = ExternalShowItemsTrackingDeliveryCommand
            };

            stackOtherOperations.Children.Add(ButtonItemsTrackingDelivery);

            ButtonStoreItems = new GlyphLeftContentViewButton()
            {
                Style = extraMenuOptionsStyle,
                Text = App.LocalizationResources.StoreItemsLabel,
                InvisibleWhenDisabled = true,
                Command = ExternalShowStoreItemsCommand
            };

            stackOtherOperations.Children.Add(ButtonStoreItems);

            ButtonDepots = new GlyphLeftContentViewButton()
            {
                Style = extraMenuOptionsStyle,
                Text = App.LocalizationResources.WarehousesLabel,
                InvisibleWhenDisabled = true,
                Command = ExternalShowDepotsCommand
            };

            stackOtherOperations.Children.Add(ButtonDepots);

            BottomLineSeparator = RenderUtil.InstaceLineSeparator();
            BottomLineSeparator.IsVisible = IsStaff;
            stackOptions.Children.Add(BottomLineSeparator);

            Color darkButtonTextColor = Color.FromHex("999999");
            Color darkButtonIconColor = Color.FromHex("333333");

            bool hasTextExtraButton = true;

            Style bottomMenuOptionsStyle = new Style(typeof(ContentViewButton))
            {
            };

            if (hasTextExtraButton)
            {
                bottomMenuOptionsStyle.Setters.Add(ContentViewButton.HeightRequestProperty, Theme.CommonResources.RoundedButtonWidth * 2f);
                bottomMenuOptionsStyle.Setters.Add(ContentViewButton.WidthRequestProperty, Theme.CommonResources.RoundedButtonWidth * 2f);
            }
            else
            {
                bottomMenuOptionsStyle.BasedOn = Theme.ApplicationStyles.GlyphOnlyRoundedContentButtonStyle;
            }

            bottomMenuOptionsStyle.Setters.Add(ContentViewButton.ShapeTypeProperty, hasTextExtraButton ? ShapeType.Box : ShapeType.Circle);
            bottomMenuOptionsStyle.Setters.Add(ContentViewButton.MinimumHeightRequestProperty, Theme.CommonResources.RoundedButtonWidth * 1.25f);
            bottomMenuOptionsStyle.Setters.Add(ContentViewButton.MinimumWidthRequestProperty, Theme.CommonResources.RoundedButtonWidth * 1.25f);
            bottomMenuOptionsStyle.Setters.Add(ContentViewButton.CornerRadiusProperty, 10);
            bottomMenuOptionsStyle.Setters.Add(GlyphContentViewButton.GlyphTextColorProperty, darkButtonIconColor);
            bottomMenuOptionsStyle.Setters.Add(ContentViewButton.TextColorProperty, darkButtonTextColor);
            bottomMenuOptionsStyle.Setters.Add(ContentViewButton.FontSizeProperty, Theme.CommonResources.TextSizeMicro * 0.8f);
            bottomMenuOptionsStyle.Setters.Add(ContentViewButton.LineBreakModeProperty, LineBreakMode.WordWrap);
            bottomMenuOptionsStyle.Setters.Add(ContentViewButton.ButtonBackgroundColorProperty, Color.Transparent);
            bottomMenuOptionsStyle.Setters.Add(ContentViewButton.VerticalOptionsProperty, LayoutOptions.Start);
            bottomMenuOptionsStyle.Setters.Add(ContentViewButton.HorizontalOptionsProperty, LayoutOptions.CenterAndExpand);
            bottomMenuOptionsStyle.Setters.Add(ContentViewButton.MarginElementsProperty, 5);
            bottomMenuOptionsStyle.Setters.Add(ContentViewButton.MarginBordersProperty, 10);
            bottomMenuOptionsStyle.Setters.Add(ContentViewButton.ContentAlignmentProperty, TextAlignment.Start);

            StackBottomOptions = new StackLayout()
            {
                Style = Theme.ApplicationStyles.SimpleStackContainerStyle,
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Start,
                BackgroundColor = Theme.CommonResources.SubtleBackgroundColor
            };

            // Settings
            ButtonSettings = new GlyphContentViewButton(hasTextExtraButton, true, ImageOrientation.ImageOnTop)
            {
                Style = bottomMenuOptionsStyle,
                GlyphText = Theme.CommonResources.GlyphTextSettings,
                Text = App.LocalizationResources.SettingsButton,
                Command = ExternalShowSettingsCommand
            };

            StackBottomOptions.Children.Add(ButtonSettings);

            ButtonSettingsAlt = new GlyphLeftContentViewButton()
            {
                Style = extraMenuOptionsStyle,
                GlyphText = Theme.CommonResources.GlyphTextSettings,
                Text = App.LocalizationResources.SettingsButton,
                Command = ExternalShowSettingsCommand
            };

            StackAlternativeOptions.Children.Add(ButtonSettingsAlt);

            // Share
            ButtonShare = new GlyphContentViewButton(hasTextExtraButton, true, ImageOrientation.ImageOnTop)
            {
                Style = bottomMenuOptionsStyle,
                GlyphText = Theme.CommonResources.GlyphTextShare,
                GlyphFriendlyFontName = Theme.CommonResources.GlyphFriendlyFontNameAlternate,
                GlyphFontName = Theme.CommonResources.GlyphFontNameAlternate,
                Text = App.LocalizationResources.SocialShareMenu,
                Command = ExternalShareAppCommand
            };

            StackBottomOptions.Children.Add(ButtonShare);

            // Share
            ButtonShareAlt = new GlyphLeftContentViewButton()
            {
                Style = extraMenuOptionsStyle,
                GlyphText = Theme.CommonResources.GlyphTextShare,
                GlyphFriendlyFontName = Theme.CommonResources.GlyphFriendlyFontNameAlternate,
                GlyphFontName = Theme.CommonResources.GlyphFontNameAlternate,
                Text = App.LocalizationResources.SocialShareMenu,
                Command = ExternalShareAppCommand
            };

            StackAlternativeOptions.Children.Add(ButtonShareAlt);

            // Articles
            ButtonArticles = new GlyphContentViewButton(hasTextExtraButton, true, ImageOrientation.ImageOnTop)
            {
                Style = bottomMenuOptionsStyle,
                GlyphText = Theme.CommonResources.GlyphTextArticles,
                Text = App.LocalizationResources.ArticlesLabel,
                Command = ExternalShowArticlesCommand
            };

            StackBottomOptions.Children.Add(ButtonArticles);

            ButtonArticlesAlt = new GlyphLeftContentViewButton()
            {
                Style = extraMenuOptionsStyle,
                GlyphText = Theme.CommonResources.GlyphTextArticles,
                Text = App.LocalizationResources.ArticlesLabel,
                Command = ExternalShowArticlesCommand
            };

            StackAlternativeOptions.Children.Add(ButtonArticlesAlt);

            // About
            ButtonAbout = new GlyphContentViewButton(hasTextExtraButton, true, ImageOrientation.ImageOnTop)
            {
                Style = bottomMenuOptionsStyle,
                GlyphText = Theme.CommonResources.GlyphTextAbout,
                Text = App.LocalizationResources.AboutMenu,
                Command = ExternalShowAboutCommand,
                GlyphFontName = Theme.CommonResources.GlyphFontNameAlternate,
                GlyphFriendlyFontName = Theme.CommonResources.GlyphFriendlyFontNameAlternate
            };

            StackBottomOptions.Children.Add(ButtonAbout);

            ButtonAboutAlt = new GlyphLeftContentViewButton()
            {
                Style = extraMenuOptionsStyle,
                GlyphText = Theme.CommonResources.GlyphTextAbout,
                Text = App.LocalizationResources.AboutMenu,
                Command = ExternalShowAboutCommand,
                GlyphFontName = Theme.CommonResources.GlyphFontNameAlternate,
                GlyphFriendlyFontName = Theme.CommonResources.GlyphFriendlyFontNameAlternate
            };

            ButtonAboutAlt.GlyphFontSize = ButtonAboutAlt.GlyphFontSize * 1.4;
            ButtonAbout.GlyphFontSize = ButtonAbout.GlyphFontSize * 1.4;

            StackAlternativeOptions.Children.Add(ButtonAboutAlt);

            double imageButtonMargin = 5;

            // Logo
            ImageLogoButton = new ImageContentViewButton()
            {
                Source = Theme.CommonResources.PathImageAppLogoLarge,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                ShapeType = ShapeType.Circle,
                ButtonBackgroundColor = Color.White,
                StrokeWidth = 1,
                StrokeColor = darkAccent,
                Command = LogoAnimateCommand,
                MarginBorders = imageButtonMargin,
                ButtonTappedAnimationDelegate = RenderUtil.RotateAnimationAsync
            };

            LayoutMainOptions.Children.Add(ImageLogoButton);

            // Space
            RenderUtil.RenderSpace(stackOptions, 10, 10);

            MenuScroll.Content = stackOptions;

            menuLayout.Children.Add(MenuScroll);

            menuLayout.Children.Add(StackBottomOptions);

            return menuLayout;
        }

        /// <summary>
        /// Update the view.
        /// </summary>
        protected void UpdateSmallViewVisibility()
        {
            if (IsSmallView)
            {
                if ((LayoutMainOptions != null) && LayoutMainOptions.IsVisible)
                {
                    LayoutMainOptions.IsVisible = false;
                }

                if ((StackBottomOptions != null) && StackBottomOptions.IsVisible)
                {
                    StackBottomOptions.IsVisible = false;
                }

                if ((StackAlternativeOptions != null) && !StackAlternativeOptions.IsVisible)
                {
                    StackAlternativeOptions.IsVisible = true;
                }
            }
            else
            {
                if ((LayoutMainOptions != null) && !LayoutMainOptions.IsVisible)
                {
                    LayoutMainOptions.IsVisible = true;
                }

                if ((StackBottomOptions != null) && !StackBottomOptions.IsVisible)
                {
                    StackBottomOptions.IsVisible = true;
                }

                if ((StackAlternativeOptions != null) && StackAlternativeOptions.IsVisible)
                {
                    StackAlternativeOptions.IsVisible = false;
                }
            }
        }

        /// <summary>
        /// Update command for a button.
        /// </summary>
        /// <param name="button">Button to use.</param>
        /// <param name="command">Command to use.</param>
        private void UpdateButtonCommand(ContentViewButton button, Command command)
        {
            if ((button != null) && (button.Command != command) && (command != null))
            {
                button.Command = command;
                command.ChangeCanExecute();
            }
        }

        /// <summary>
        /// Update button commands.
        /// </summary>
        private void UpdateCommands()
        {
            UpdateButtonCommand(CloseButton, ExternalCloseMenuCommand);
            UpdateButtonCommand(ButtonStore, ExternalShowStoreCommand);
            UpdateButtonCommand(ButtonCart, ExternalShowCartCommand);
            UpdateButtonCommand(ButtonSearch, ExternalShowSearchCommand);
            UpdateButtonCommand(ButtonArticles, ExternalShowArticlesCommand);
            UpdateButtonCommand(ButtonOrders, ExternalShowOrdersCommand);
            UpdateButtonCommand(ButtonItemsTrackingDispatch, ExternalShowItemsTrackingDispatchCommand);
            UpdateButtonCommand(ButtonItemsTrackingProvider, ExternalShowItemsTrackingProviderCommand);
            UpdateButtonCommand(ButtonItemsTrackingDelivery, ExternalShowItemsTrackingDeliveryCommand);
            UpdateButtonCommand(ButtonStoreItems, ExternalShowStoreItemsCommand);
            UpdateButtonCommand(ButtonDepots, ExternalShowDepotsCommand);
            UpdateButtonCommand(ButtonSettings, ExternalShowSettingsCommand);
            UpdateButtonCommand(ButtonShare, ExternalShareAppCommand);
            UpdateButtonCommand(ButtonAbout, ExternalShowAboutCommand);

            UpdateButtonCommand(ButtonStoreAlt, ExternalShowStoreCommand);
            UpdateButtonCommand(ButtonCartAlt, ExternalShowCartCommand);
            UpdateButtonCommand(ButtonSearchAlt, ExternalShowSearchCommand);
            UpdateButtonCommand(ButtonArticlesAlt, ExternalShowArticlesCommand);
            UpdateButtonCommand(ButtonOrdersAlt, ExternalShowOrdersCommand);
            UpdateButtonCommand(ButtonSettingsAlt, ExternalShowSettingsCommand);
            UpdateButtonCommand(ButtonShareAlt, ExternalShareAppCommand);
            UpdateButtonCommand(ButtonAboutAlt, ExternalShowAboutCommand);
            UpdateButtonCommand(ButtonAddressBook, ExternalShowAddressBookCommand);

            if (LabelAdminTitle != null)
            {
                LabelAdminTitle.IsVisible = IsStaff;
            }
        }
    }
}
