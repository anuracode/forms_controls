// <copyright file="MenuPage.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Extensions;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls.Sample.Views
{
    /// <summary>
    /// Menu page.
    /// </summary>
    public class MenuPage : BaseView
    {
        /// <summary>
        /// Command for the logo.
        /// </summary>
        private Command logoAnimateCommand;

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
        public Command ExternalShowProfileCommand { get; set; }

        /// <summary>
        /// Navigate to cart.
        /// </summary>
        public Command ExternalShowCartCommand { get; set; }

        /// <summary>
        /// Command to show the orders.
        /// </summary>
        public Command ExternalShowOrdersCommand { get; set; }

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
        /// Bottom separator.
        /// </summary>
        protected View BottomLineSeparator { get; private set; }

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
        protected GlyphContentViewButton ButtonProfileAlt { get; private set; }

        /// <summary>
        /// Button to use.
        /// </summary>
        protected ContentViewButton ButtonCartAlt { get; private set; }

        /// <summary>
        /// Button orders.
        /// </summary>
        protected ContentViewButton ButtonOrdersAlt { get; private set; }

        /// <summary>
        /// Button to use.
        /// </summary>
        protected ContentViewButton ButtonSearchAlt { get; private set; }

        /// <summary>
        /// Button for settings.
        /// </summary>
        protected ContentViewButton ButtonSettingsAlt { get; private set; }

        /// <summary>
        /// Button to use.
        /// </summary>
        protected ContentViewButton ButtonShareAlt { get; private set; }

        /// <summary>
        /// Close button.
        /// </summary>
        protected ContentViewButton CloseButton { get; private set; }

        /// <summary>
        /// True when the view is been loaded again (from back navigation probably).
        /// </summary>
        protected bool IsRecylced { get; private set; }

        /// <summary>
        /// The view is small.
        /// </summary>
        protected bool IsSmallView { get; set; }

        /// <summary>
        /// Menu scroll.
        /// </summary>
        protected ScrollView MenuScroll { get; private set; }

        /// <summary>
        /// Stack for the alternative option.
        /// </summary>
        protected StackLayout StackAlternativeOptions { get; set; }

        /// <summary>
        /// Top line separator.
        /// </summary>
        protected View TopLineSeparator { get; private set; }

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
            Color darkAccent = Theme.CommonResources.AccentDark;
            Color lightAccent = Theme.CommonResources.AccentLight;

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

            AC.ScheduleManaged(
                TimeSpan.FromSeconds(0.5),
                async () =>
                {
                    await Task.FromResult(0);

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

                    Theme.RenderUtil.RenderSpace(stackMenuCloseBar);

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
                });

            MenuScroll = new ScrollView()
            {
                Orientation = ScrollOrientation.Vertical,
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

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

            // Store
            ButtonStoreAlt = new GlyphLeftContentViewButton()
            {
                Style = extraMenuOptionsStyle,
                GlyphText = Theme.CommonResources.GlyphTextStore,
                Text = App.LocalizationResources.StoreLabel,
                Command = ExternalShowStoreCommand
            };

            StackAlternativeOptions.Children.Add(ButtonStoreAlt);

            // Cart
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

            // Search
            ButtonSearchAlt = new GlyphLeftContentViewButton()
            {
                Style = extraMenuOptionsStyle,
                GlyphText = Theme.CommonResources.GlyphTextSearch,
                Text = App.LocalizationResources.SearchButton,
                Command = ExternalShowSearchCommand
            };

            StackAlternativeOptions.Children.Add(ButtonSearchAlt);

            // Orders
            ButtonOrdersAlt = new GlyphLeftContentViewButton()
            {
                Style = extraMenuOptionsStyle,
                GlyphText = Theme.CommonResources.GlyphTextOrders,
                Text = App.LocalizationResources.OrdersLabel,
                Command = ExternalShowOrdersCommand
            };

            StackAlternativeOptions.Children.Add(ButtonOrdersAlt);

            // Logged in staff operations.
            var stackOtherOperations = Theme.RenderUtil.RenderIdentedVerticalContainer(stackOptions);

            Theme.RenderUtil.RenderSpace(stackOtherOperations);

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

            // Settings
            ButtonSettingsAlt = new GlyphLeftContentViewButton()
            {
                Style = extraMenuOptionsStyle,
                GlyphText = Theme.CommonResources.GlyphTextSettings,
                Text = App.LocalizationResources.SettingsButton,
                Command = ExternalShowSettingsCommand
            };

            StackAlternativeOptions.Children.Add(ButtonSettingsAlt);

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

            // Profile
            ButtonProfileAlt = new GlyphLeftContentViewButton()
            {
                Style = extraMenuOptionsStyle,
                GlyphText = Theme.CommonResources.GlyphTextProfile,
                Text = App.LocalizationResources.ProfileLabel,
                Command = ExternalShowProfileCommand
            };

            StackAlternativeOptions.Children.Add(ButtonProfileAlt);

            // About
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

            StackAlternativeOptions.Children.Add(ButtonAboutAlt);

            // Space
            Theme.RenderUtil.RenderSpace(stackOptions, 10, 10);

            MenuScroll.Content = stackOptions;

            menuLayout.Children.Add(MenuScroll);

            return menuLayout;
        }

        /// <summary>
        /// Update the view.
        /// </summary>
        protected void UpdateSmallViewVisibility()
        {
            if ((StackAlternativeOptions != null) && !StackAlternativeOptions.IsVisible)
            {
                StackAlternativeOptions.IsVisible = true;
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
            UpdateButtonCommand(ButtonStoreAlt, ExternalShowStoreCommand);
            UpdateButtonCommand(ButtonCartAlt, ExternalShowCartCommand);
            UpdateButtonCommand(ButtonSearchAlt, ExternalShowSearchCommand);
            UpdateButtonCommand(ButtonProfileAlt, ExternalShowProfileCommand);
            UpdateButtonCommand(ButtonOrdersAlt, ExternalShowOrdersCommand);
            UpdateButtonCommand(ButtonSettingsAlt, ExternalShowSettingsCommand);
            UpdateButtonCommand(ButtonShareAlt, ExternalShareAppCommand);
            UpdateButtonCommand(ButtonAboutAlt, ExternalShowAboutCommand);
            UpdateButtonCommand(ButtonAddressBook, ExternalShowAddressBookCommand);
        }
    }
}