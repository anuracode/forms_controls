// <copyright file="ContentBaseView.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Sample.ViewModels;
using Anuracode.Forms.Controls.Extensions;
using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls.Sample.Views
{
    /// <summary>
    /// Base view.
    /// </summary>
    public abstract class ContentBaseView<TViewModel> : BaseView<TViewModel>
        where TViewModel : BaseViewModel
    {
        /// <summary>
        /// Flag when the extra layers have been already added.
        /// </summary>
        private bool extraLayersAdded;

        /// <summary>
        /// Navigation disposable.
        /// </summary>
        private IDisposable navigationDisposable;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="viewModel">View model to use.</param>
        public ContentBaseView(TViewModel viewModel)
            : base(viewModel)
        {
            if (HasInitialAnimation && (ViewModel != null) && !IsRecylced)
            {
                ViewModel.InitializationComplete += ViewModel_InitializationComplete;
            }

            if (BindingContext == null)
            {
                BindingContext = ViewModel;
            }

            if (this.Content == null)
            {
                RenderContentTemplate();
            }
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ContentBaseView()
            : this(null)
        {
        }

        /// <summary>
        /// Size of the status bar.
        /// </summary>
        public static double StatusBarHeight
        {
            get
            {
                return Device.OS.OnPlatform(20, 0, 0, 0, 0);
            }
        }

        /// <summary>
        /// Page canvas.
        /// </summary>
        public Layout<View> PageCanvas { get; set; }

        /// <summary>
        /// App name view.
        /// </summary>
        protected View AppNameView { get; set; }

        /// <summary>
        /// Add the extra layers automatically.
        /// </summary>
        protected virtual bool AutoAddExtraLayers
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Bottom background shape.
        /// </summary>
        protected ShapeView BackgroundBottomLeftShape { get; set; }

        /// <summary>
        /// Bottom background shape.
        /// </summary>
        protected BoxView BackgroundBottomShape { get; set; }

        /// <summary>
        /// Top background shape.
        /// </summary>
        protected ShapeView BackgroundTopRightShape { get; set; }

        /// <summary>
        /// Top background shape.
        /// </summary>
        protected BoxView BackgroundTopShape { get; set; }

        /// <summary>
        /// Margin for the bottom app bar.
        /// </summary>
        protected virtual double BottomAppBarMargin
        {
            get
            {
                return Device.OS.OnPlatform(0, 0, 0, 95, 0);
            }
        }

        /// <summary>
        /// Margin for the relative layout calculations.
        /// </summary>
        protected virtual double ContentMargin
        {
            get
            {
                return 10;
            }
        }

        /// <summary>
        /// Footer of the view.
        /// </summary>
        protected View Footer { get; set; }

        /// <summary>
        /// Has initial animation.
        /// </summary>
        protected virtual bool HasInitialAnimation
        {
            get
            {
                return Device.OS.OnPlatform(true, false, true, true, true);
            }
        }

        /// <summary>
        /// Has a background that needs render.
        /// </summary>
        protected virtual bool HasRenderBackground
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Header of the view.
        /// </summary>
        protected View Header { get; set; }

        /// <summary>
        /// Inner content of the view.
        /// </summary>
        protected View InnerContentView { get; set; }

        /// <summary>
        /// True when the view is been loaded again (from back navigation probably).
        /// </summary>
        protected bool IsRecylced { get; private set; }

        /// <summary>
        /// Size of the last layout of the inner content view.
        /// </summary>
        protected Rectangle LastPositionInnerContentView { get; set; }

        /// <summary>
        /// Size of the last layout page canvas.
        /// </summary>
        protected Rectangle LastSizePageCanvas { get; set; }

        /// <summary>
        /// Main menu button.
        /// </summary>
        protected GlyphOnlyContentViewButton MainMenuButton { get; set; }

        /// <summary>
        /// Menu bar backgorund.
        /// </summary>
        protected BoxView MenubarBackground { get; set; }

        /// <summary>
        /// Navigation bar backgorund.
        /// </summary>
        protected BoxView NavbarBackground { get; set; }

        /// <summary>
        /// Navigate back button.
        /// </summary>
        protected ContentViewButton NavigateBackButton { get; set; }

        /// <summary>
        /// Navigation disposable.
        /// </summary>
        protected IDisposable NavigationDisposable
        {
            get
            {
                return navigationDisposable;
            }

            set
            {
                navigationDisposable = value;
            }
        }        

        /// <summary>
        /// Render cancel button.
        /// </summary>
        protected virtual bool RenderCancelButton
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Render menu button.
        /// </summary>
        protected virtual bool RenderMenuButton
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Render navigation back button.
        /// </summary>
        protected virtual bool RenderNavigationBackButton
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Render title bar.
        /// </summary>
        protected virtual bool RenderTitleBar
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Render title label.
        /// </summary>
        protected virtual bool RenderTitleLabel
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Stack for the shortcuts.
        /// </summary>
        protected StackLayout StackShortCuts { get; set; }

        /// <summary>
        /// View to use as title.
        /// </summary>
        protected View TitleView { get; set; }

        /// <summary>
        /// Add the add toolbar item.
        /// </summary>
        protected virtual void AddCancelToolbarItem()
        {
            if (!IsRecylced)
            {
                var tbi = new ToolbarItem()
                {
                    Text = App.LocalizationResources.CancelButton,
                    Icon = Theme.CommonResources.PathImageCancelAction,
                    Command = NavigateBackCommand
                };

                ToolbarItems.Add(tbi);

                if (StackShortCuts != null)
                {
                    var cancelButton = new GlyphOnlyContentViewButton()
                    {
                        Style = Theme.ApplicationStyles.GlyphOnlyNavbarButtonStyle,
                        GlyphText = Theme.CommonResources.GlyphTextCancel,
                        Text = App.LocalizationResources.CancelButton,
                        Command = NavigateBackCommand,
                        BindingContext = this
                    };

                    StackShortCuts.Children.Add(cancelButton);
                }
            }
        }

        /// <summary>
        /// Add extra layers.
        /// </summary>
        protected virtual void AddExtraLayers()
        {
            if ((PageCanvas != null) && !extraLayersAdded)
            {
                if (MenubarBackground != null)
                {
                    PageCanvas.Children.Add(MenubarBackground);
                }

                if (AppNameView != null)
                {
                    PageCanvas.Children.Add(AppNameView);
                }

                if (NavbarBackground != null)
                {
                    PageCanvas.Children.Add(NavbarBackground);
                }

                if (TitleView != null)
                {
                    PageCanvas.Children.Add(TitleView);
                }

                if (MainMenuButton != null)
                {
                    PageCanvas.Children.Add(MainMenuButton);
                }

                if (NavigateBackButton != null)
                {
                    PageCanvas.Children.Add(NavigateBackButton);
                }

                PageCanvas.Children.Add(StackShortCuts);                

                extraLayersAdded = true;
            }
        }

        /// <summary>
        /// Add the refresh button.
        /// </summary>
        protected virtual void AddShareToolbarItem()
        {
            if (StackShortCuts != null)
            {
                var shareButton = new GlyphOnlyContentViewButton()
                {
                    Style = Theme.ApplicationStyles.GlyphOnlyNavbarButtonStyle,
                    GlyphText = Theme.CommonResources.GlyphTextShare,
                    GlyphFriendlyFontName = Theme.CommonResources.GlyphFriendlyFontNameAlternate,
                    GlyphFontName = Theme.CommonResources.GlyphFontNameAlternate,
                    Command = ViewModel.ShareContentCommand,
                    BindingContext = this,
                    InvisibleWhenDisabled = true
                };

                StackShortCuts.Children.Insert(0, shareButton);
            }
        }

        /// <summary>
        /// Add custom toolbar items.
        /// </summary>
        protected virtual void AddToolBarItems()
        {
            if (RenderCancelButton)
            {
                AddCancelToolbarItem();
            }

            AddShareToolbarItem();
        }

        /// <summary>
        /// Initial animation.
        /// </summary>
        /// <returns>Task to await.</returns>
        protected virtual async Task InitialAnimation()
        {
            if (HasInitialAnimation && (InnerContentView != null) && !IsRecylced)
            {
                try
                {
                    await LockAnimation.WaitAsync();

                    if (this.Content != null)
                    {
                        await InnerContentView.FadeTo(1);
                    }
                }
                finally
                {
                    LockAnimation.Release();
                }
            }
        }

        /// <summary>
        /// Instance the view for the applciation title.
        /// </summary>
        /// <returns></returns>
        protected virtual View InstanceAppNameView()
        {
            ExtendedLabel appName = new ExtendedLabel()
            {
                Style = Theme.ApplicationStyles.PageMainTitleExtendedLabelStyle,
                Text = App.LocalizationResources.ApplicationTitle,
                TextColor = Theme.CommonResources.PagesBackgroundColorLight
            };

            return appName;
        }

        /// <summary>
        /// Instance the navigation back button.
        /// </summary>
        /// <returns>Button to use.</returns>
        protected virtual ContentViewButton InstanceNavigateBackButton()
        {
            return new GlyphOnlyContentViewButton()
            {
                Style = Theme.ApplicationStyles.GlyphOnlyNavbarButtonStyle,
                BindingContext = this,
                GlyphText = Theme.CommonResources.GlyphTextNavigateBack,
                Command = NavigateBackCommand,
                InvisibleWhenDisabled = true
            };
        }

        /// <summary>
        /// Instance title view.
        /// </summary>
        /// <returns>View to use.</returns>
        protected virtual View InstanceTitleView()
        {
            var labelTitle = new ExtendedLabel()
            {
                Style = Theme.ApplicationStyles.PageMainTitleExtendedLabelStyle,
                FontAttributes = FontAttributes.Bold,
                FontSize = Theme.CommonResources.TextSizeSmall,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                TextColor = Theme.CommonResources.PagesBackgroundColorLight,
                HorizontalTextAlignment = TextAlignment.Start
            };

            labelTitle.SetBinding<TViewModel>(ExtendedLabel.TextProperty, vm => vm.Title);

            return labelTitle;
        }

        /// <summary>
        /// Page appears.
        /// </summary>
        protected override void OnAppearing()
        {
            if (BindingContext == null)
            {
                BindingContext = ViewModel;
            }

            if (this.Content == null)
            {
                RenderContentTemplate();
            }

            base.OnAppearing();
        }

        /// <summary>
        /// Page disapperas.
        /// </summary>
        protected override void OnDisappearing()
        {
            if (!IsRecylced)
            {
                IsRecylced = true;
            }

            base.OnDisappearing();
        }

        /// <summary>
        /// When the attached viewmodel navigates.
        /// </summary>
        /// <param name="navigateTo">True when navigated to page.</param>
        protected virtual void OnViewModelNavigated(bool navigateTo)
        {
            AC.ScheduleManaged(
                async () =>
            {
                await Task.FromResult(0);
                if (navigateTo)
                {
                    OnViewModelNavigatedTo();
                }
                else
                {
                    OnViewModelNavigatedFrom();
                }
            });
        }

        /// <summary>
        /// When navigated from the view.
        /// </summary>
        protected virtual void OnViewModelNavigatedFrom()
        {
        }

        /// <summary>
        /// When navigated to the view.
        /// </summary>
        protected virtual void OnViewModelNavigatedTo()
        {
        }

        /// <summary>
        /// Layout the children for the background.
        /// </summary>
        /// <param name="pageSize">Page size.</param>
        /// <param name="headerPosition">Header position.</param>
        /// <param name="contentPosition">Content position.</param>
        /// <param name="footerPosition">Footer position.</param>
        protected virtual void PageBackgroundLayoutChildren(Rectangle pageSize, Rectangle headerPosition, Rectangle contentPosition, Rectangle footerPosition)
        {
            if (BackgroundTopShape != null)
            {
                double elementTop = 0;
                double elementLeft = 0;
                double elementWidth = pageSize.Width;
                double elementHeight = pageSize.Height - (headerPosition.Y - headerPosition.Height);
                Rectangle elementPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                BackgroundTopShape.LayoutUpdate(elementPosition);
            }

            Rectangle bottonLeftPostion = new Rectangle();

            if (BackgroundBottomLeftShape != null)
            {
                double elementTop = headerPosition.Y + ContentMargin;
                double elementWidth = pageSize.Width + (ContentMargin * 2f);
                double elementHeight = elementWidth * 0.33f;
                double elementLeft = (pageSize.Width * 0.25f) - (elementWidth * 0.5f);
                bottonLeftPostion = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                BackgroundBottomLeftShape.LayoutUpdate(bottonLeftPostion);
            }

            if (BackgroundBottomShape != null)
            {
                double elementLeft = 0;
                double elementTop = bottonLeftPostion.Y + (bottonLeftPostion.Height * 0.08f);
                double elementWidth = pageSize.Width;
                double elementHeight = pageSize.Height - elementTop;
                Rectangle elementPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                BackgroundBottomShape.LayoutUpdate(elementPosition);
            }

            if (BackgroundTopRightShape != null)
            {
                double elementWidth = bottonLeftPostion.Width * 1.5f;
                double elementHeight = bottonLeftPostion.Height;
                double elementLeft = (pageSize.Width * 0.9f) - (elementWidth * 0.5f);
                double elementTop = bottonLeftPostion.Y - (elementHeight * 0.85f);
                Rectangle elementPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                BackgroundTopRightShape.LayoutUpdate(elementPosition);
            }
        }

        /// <summary>
        /// Layout the children.
        /// </summary>
        /// <param name="x">Top to use.</param>
        /// <param name="y">Left to use.</param>
        /// <param name="width">Width to use.</param>
        /// <param name="height">Height to use.</param>
        protected virtual void PageCanvasLayoutChildren(double x, double y, double width, double height)
        {
            Rectangle pageSize = new Rectangle(ContentMargin, ContentMargin, width - (ContentMargin * 2), height - (ContentMargin * 2) - BottomAppBarMargin);           

            PageContentLayoutChildren(x, y, width, height);
        }

        /// <summary>
        /// Layout the children.
        /// </summary>
        /// <param name="x">Top to use.</param>
        /// <param name="y">Left to use.</param>
        /// <param name="width">Width to use.</param>
        /// <param name="height">Height to use.</param>
        protected virtual void PageContentLayoutChildren(double x, double y, double width, double height)
        {
            Rectangle pageSize = new Rectangle(x, y, width, height);
            Rectangle headerposition = new Rectangle();
            Rectangle footerPosition = new Rectangle();
            Rectangle mainMenuButtonPosition = new Rectangle();
            Rectangle navigateBackButtonPosition = new Rectangle();
            Rectangle stackButtonPosition = new Rectangle();

            LastSizePageCanvas = new Rectangle(ContentMargin, ContentMargin, width - (ContentMargin * 2), height - (ContentMargin * 2) - BottomAppBarMargin);

            if (MainMenuButton != null)
            {
                var elementSize = MainMenuButton.GetSizeRequest(width, height).Request;

                mainMenuButtonPosition = new Rectangle(
                    0,
                    StatusBarHeight,
                    elementSize.Width,
                    elementSize.Height);
                MainMenuButton.LayoutUpdate(mainMenuButtonPosition);
            }

            if (NavigateBackButton != null)
            {
                var elementSize = NavigateBackButton.GetSizeRequest(width, height).Request;

                navigateBackButtonPosition = new Rectangle(
                    0,
                    mainMenuButtonPosition.Y + mainMenuButtonPosition.Height,
                    elementSize.Width,
                    elementSize.Height);
                NavigateBackButton.LayoutUpdate(navigateBackButtonPosition);
            }

            if (MenubarBackground != null)
            {
                var elementPosition = new Rectangle(
                    0,
                    0,
                    width,
                    mainMenuButtonPosition.Height + navigateBackButtonPosition.Height + StatusBarHeight);
                MenubarBackground.LayoutUpdate(elementPosition);
            }

            if (AppNameView != null)
            {
                double maxWidth = width - mainMenuButtonPosition.X - mainMenuButtonPosition.Width - (ContentMargin * 2f);
                var elementSize = AppNameView.GetSizeRequest(maxWidth, mainMenuButtonPosition.Height).Request;

                var elementPosition = new Rectangle(
                    mainMenuButtonPosition.X + mainMenuButtonPosition.Width + ContentMargin,
                    mainMenuButtonPosition.Y + ((mainMenuButtonPosition.Height - elementSize.Height) * 0.5f),
                    elementSize.Width.Clamp(0, maxWidth),
                    elementSize.Height.Clamp(0, mainMenuButtonPosition.Height));

                AppNameView.LayoutUpdate(elementPosition);
            }

            if (NavbarBackground != null)
            {
                var elementPosition = new Rectangle(
                    0,
                    navigateBackButtonPosition.Y,
                    width,
                    navigateBackButtonPosition.Height);
                NavbarBackground.LayoutUpdate(elementPosition);
            }

            if (StackShortCuts != null)
            {
                var elementSize = StackShortCuts.GetSizeRequest(width, height).Request;

                stackButtonPosition = new Rectangle(
                    width - (ContentMargin * 2) - elementSize.Width,
                    navigateBackButtonPosition.Y,
                    elementSize.Width,
                    navigateBackButtonPosition.Height);

                StackShortCuts.LayoutUpdate(stackButtonPosition);
            }

            if (TitleView != null)
            {
                double titleMargin = ContentMargin * (Device.Idiom == TargetIdiom.Phone ? 0f : 1f);
                double maxWidth = stackButtonPosition.X - navigateBackButtonPosition.X - (titleMargin * 2f);
                var elementSize = TitleView.GetSizeRequest(maxWidth, stackButtonPosition.Height).Request;

                var elementPosition = new Rectangle(
                    navigateBackButtonPosition.X + navigateBackButtonPosition.Width + titleMargin,
                    navigateBackButtonPosition.Y + ((navigateBackButtonPosition.Height - elementSize.Height) * 0.5f),
                    elementSize.Width > 0 ? elementSize.Width.Clamp(0, maxWidth) : maxWidth,
                     elementSize.Height > 0 ? elementSize.Height.Clamp(0, navigateBackButtonPosition.Height) : navigateBackButtonPosition.Height);

                TitleView.LayoutUpdate(elementPosition);
            }

            if (Header != null)
            {
                var headerSize = Header.GetSizeRequest(width, height).Request;

                headerposition = new Rectangle(
                    ContentMargin,
                    ContentMargin + navigateBackButtonPosition.Y + navigateBackButtonPosition.Height,
                    width - (ContentMargin * 2),
                    headerSize.Height);

                Header.LayoutUpdate(headerposition);
            }

            if (Footer != null)
            {
                var footerSize = Footer.GetSizeRequest(width, height).Request;

                footerPosition = new Rectangle(
                    ContentMargin,
                    height - footerSize.Height - ContentMargin - BottomAppBarMargin,
                    width - (ContentMargin * 2),
                    footerSize.Height);

                Footer.LayoutUpdate(footerPosition);
            }

            if (InnerContentView != null)
            {
                LastPositionInnerContentView = new Rectangle(
                    ContentMargin,
                    headerposition.Y + headerposition.Height,
                    width - (ContentMargin * 2),
                    height - headerposition.Height - footerPosition.Height - (ContentMargin * 2) - BottomAppBarMargin - navigateBackButtonPosition.Height - mainMenuButtonPosition.Height);

                InnerContentView.LayoutUpdate(LastPositionInnerContentView);
            }

            if (HasRenderBackground)
            {
                PageBackgroundLayoutChildren(pageSize, headerposition, LastPositionInnerContentView, footerPosition);
            }

            PageExtraLayersLayoutChildren(pageSize, headerposition, LastPositionInnerContentView, footerPosition);
        }

        /// <summary>
        /// Layout the children for the background.
        /// </summary>
        /// <param name="pageSize">Page size.</param>
        /// <param name="headerPosition">Header position.</param>
        /// <param name="contentPosition">Content position.</param>
        /// <param name="footerPosition">Footer position.</param>
        protected virtual void PageExtraLayersLayoutChildren(Rectangle pageSize, Rectangle headerPosition, Rectangle contentPosition, Rectangle footerPosition)
        {
        }

        /// <summary>
        /// Prepare initial animation.
        /// </summary>
        /// <param name="content">View to prepare.</param>
        protected virtual void PrepareContentInitalAnimation(View content)
        {
            if (content != null)
            {
                content.Opacity = ViewModel.IsInitialized ? 1 : 0;
            }
        }

        /// <summary>
        /// Render background layout.
        /// </summary>
        /// <param name="absoluteLayout">Layout to add the items to.</param>
        protected virtual void RenderBackgroundLayout(SimpleLayout absoluteLayout)
        {
            if (absoluteLayout != null)
            {
                BackgroundTopShape = new BoxView()
                {
                    Color = Theme.CommonResources.AccentLight
                };

                absoluteLayout.Children.Add(BackgroundTopShape);

                BackgroundBottomLeftShape = new ShapeView()
                {
                    ShapeType = ShapeType.Circle,
                    Color = Theme.CommonResources.PagesBackgroundColor
                };

                absoluteLayout.Children.Add(BackgroundBottomLeftShape);

                BackgroundBottomShape = new BoxView()
                {
                    Color = Theme.CommonResources.PagesBackgroundColor
                };

                absoluteLayout.Children.Add(BackgroundBottomShape);

                BackgroundTopRightShape = new ShapeView()
                {
                    ShapeType = ShapeType.Circle,
                    Color = Theme.CommonResources.AccentLight
                };

                absoluteLayout.Children.Add(BackgroundTopRightShape);
            }
        }

        /// <summary>
        /// Render content of the page.
        /// </summary>
        /// <returns>View to use.</returns>
        protected virtual View RenderContent()
        {
            return null;
        }

        /// <summary>
        /// Render content of the page.
        /// </summary>
        protected virtual void RenderContentTemplate()
        {
            var absoluteLayout = new SimpleLayout()
            {
                IsHandlingLayoutManually = true,
                Padding = 0,
                IsClippedToBounds = true
            };

            absoluteLayout.OnLayoutChildren += PageCanvasLayoutChildren;

            PageCanvas = absoluteLayout;

            if (HasRenderBackground)
            {
                RenderBackgroundLayout(absoluteLayout);
            }

            RenderPageLayout(absoluteLayout);

            RenderLayerLayout(absoluteLayout);

            this.Content = PageCanvas;

            AddToolBarItems();           
        }

        /// <summary>
        /// Render list footer.
        /// </summary>
        /// <returns>View to use.</returns>
        protected virtual View RenderFooter()
        {
            return null;
        }

        /// <summary>
        /// Render the header of the list.
        /// </summary>
        /// <returns>View to use.</returns>
        protected virtual View RenderHeader()
        {
            StackLayout headerLayout = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Style = Theme.ApplicationStyles.FilterStackStyle
            };

            if (HasRenderBackground)
            {
                Theme.RenderUtil.RenderSpace(headerLayout, heightRequest: (ContentMargin * 2f));
            }

            return headerLayout;
        }

        /// <summary>
        /// Render layer layout like the notifications and the progress.
        /// </summary>
        /// <param name="baseLayout">Layout to use</param>
        protected virtual void RenderLayerLayout(AbsoluteLayout baseLayout)
        {
            if (baseLayout != null)
            {
                StackShortCuts = new StackLayout()
                {
                    Style = Theme.ApplicationStyles.SimpleStackContainerStyle,
                    Orientation = StackOrientation.Horizontal,
                    VerticalOptions = LayoutOptions.Start,
                    HorizontalOptions = LayoutOptions.Start
                };

                if (RenderTitleBar)
                {
                    if (RenderMenuButton)
                    {
                        MainMenuButton = new GlyphOnlyContentViewButton()
                        {
                            Style = Theme.ApplicationStyles.GlyphOnlyNavbarButtonStyle,
                            BindingContext = this,
                            GlyphText = Theme.CommonResources.GlyphTextMainMenu,
                            GlyphFriendlyFontName = Theme.CommonResources.GlyphFriendlyFontNameAlternate,
                            GlyphFontName = Theme.CommonResources.GlyphFontNameAlternate,
                            Command = ViewModel.ShowMainMenuCommand,
                            ButtonBackgroundColor = Theme.CommonResources.AccentLight
                        };

                        double factorSize = 1.4;
                        double imageFactorSize = 1.7;

                        MainMenuButton.WidthRequest *= factorSize;
                        MainMenuButton.HeightRequest *= factorSize;
                        MainMenuButton.ImageHeightRequest = MainMenuButton.HeightRequest;
                        MainMenuButton.ImageWidthRequest = MainMenuButton.WidthRequest;
                        MainMenuButton.FontSize *= imageFactorSize;
                        MainMenuButton.GlyphFontSize *= imageFactorSize;
                        MainMenuButton.MarginBorders = 0;
                        MainMenuButton.MarginElements = 0;

                        MenubarBackground = new BoxView()
                        {
                            Color = Theme.CommonResources.AccentLight
                        };

                        AppNameView = InstanceAppNameView();
                    }

                    if (RenderNavigationBackButton)
                    {
                        NavbarBackground = new BoxView()
                        {
                            Color = Theme.CommonResources.AccentDark
                        };

                        NavigateBackButton = InstanceNavigateBackButton();
                    }

                    if (RenderTitleLabel)
                    {
                        TitleView = InstanceTitleView();
                    }
                }

                if (AutoAddExtraLayers)
                {
                    AC.ScheduleManaged(
                        TimeSpan.FromSeconds(0.1),
                        async () =>
                        {
                            await Task.FromResult(0);

                            AddExtraLayers();
                        });
                }
            }
        }

        /// <summary>
        /// Render the page layout.
        /// </summary>
        /// <param name="baseLayout">Layout to use.</param>
        protected virtual void RenderPageLayout(AbsoluteLayout baseLayout)
        {
            if (baseLayout != null)
            {
                Header = RenderHeader();
                InnerContentView = RenderContent();
                Footer = RenderFooter();

                if (InnerContentView != null)
                {
                    if (HasInitialAnimation && !IsRecylced)
                    {
                        PrepareContentInitalAnimation(InnerContentView);
                    }

                    InnerContentView.VerticalOptions = LayoutOptions.FillAndExpand;
                    InnerContentView.HorizontalOptions = LayoutOptions.Fill;

                    baseLayout.Children.Add(InnerContentView);
                }

                if (Header != null)
                {
                    Header.HorizontalOptions = LayoutOptions.Fill;
                    Header.VerticalOptions = LayoutOptions.Start;
                    baseLayout.Children.Add(Header);
                }

                if (Footer != null)
                {
                    Footer.HorizontalOptions = LayoutOptions.Fill;
                    Footer.VerticalOptions = LayoutOptions.End;
                    baseLayout.Children.Add(Footer);
                }
            }
        }

        /// <summary>
        /// Update the background opacity.
        /// </summary>
        /// <param name="featuredCount">Feautes count.</param>
        protected virtual async Task UpdateBackgroundOpactity(bool isVisible)
        {
            double newOpacity = isVisible ? 1 : 0;

            if ((BackgroundTopShape != null) && (BackgroundTopRightShape != null))
            {
                await BackgroundTopShape.FadeTo(newOpacity);
                await BackgroundTopRightShape.FadeTo(newOpacity);
            }
        }

        /// <summary>
        /// Event when the ViewModel initalization is complete.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Arguments of the event.</param>
        protected void ViewModel_InitializationComplete(object sender, EventArgs e)
        {
            if (ViewModel != null)
            {
                ViewModel.InitializationComplete -= ViewModel_InitializationComplete;
            }

            AC.ScheduleManaged((Func<Task>)InitialAnimation);
        }
    }
}