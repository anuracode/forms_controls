// <copyright file="StoreDetailLargeAbsoluteView.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Extensions;
using Anuracode.Forms.Controls.Views.Extensions;
using Anuracode.Forms.Controls.Sample.Model;
using Anuracode.Forms.Controls.Sample.ViewModels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls.Sample.Views
{
    /// <summary>
    /// Store detail view.
    /// </summary>
    public class StoreDetailLargeSimpleView : SimpleViewBase
    {
        /// <summary>
        /// Command for closing overlay.
        /// </summary>
        private ICommand closeOverlayCommand;

        /// <summary>
        /// Hide cart command.
        /// </summary>
        private Command hideOverlayCommand;

        /// <summary>
        /// Navigate back depending on the view.
        /// </summary>
        private Command internalNavigateBackCommand;

        /// <summary>
        /// Flag for the cart.
        /// </summary>
        private bool isOverlayVisible;

        /// <summary>
        /// Margin borders.
        /// </summary>
        private double marginBordersImageThumb = 3;

        /// <summary>
        /// Command for navigating to a store level.
        /// </summary>
        private Command<StoreItemLevel> navigateToStoreLevelCommand;

        /// <summary>
        /// Show image gallery.
        /// </summary>
        private Command<object> showGalleryCommand;

        /// <summary>
        /// Show cart command.
        /// </summary>
        private Command showOverlayCommand;

        /// <summary>
        /// View model to use.
        /// </summary>
        private StoreDetailViewModel viewModel;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StoreDetailLargeSimpleView()
            : base(true)
        {
            if (ContentLayout != null)
            {
                ContentLayout.IsClippedToBounds = true;
            }

            if (IsOverlay)
            {
                Opacity = 0;
                IsVisible = false;
            }
        }

        /// <summary>
        /// Margin for the bottom app bar.
        /// </summary>
        public virtual double BottomAppBarMargin { get; set; }

        /// <summary>
        /// Command for closing overlay.
        /// </summary>
        public ICommand CloseOverlayCommand
        {
            get
            {
                return closeOverlayCommand;
            }

            set
            {
                if (closeOverlayCommand != value)
                {
                    closeOverlayCommand = value;

                    OnPropertyChanged("CloseOverlayCommand");
                }

                if (CloseButton != null)
                {
                    AC.ScheduleManaged(
                        () =>
                        {
                            CloseButton.Command = CloseOverlayCommand;
                        });
                }
            }
        }

        /// <summary>
        /// Margin for the relative layout calculations.
        /// </summary>
        public double ContentMargin { get; set; }

        /// <summary>
        /// Hide overlay.
        /// </summary>
        public Command HideOverlayCommand
        {
            get
            {
                if (hideOverlayCommand == null)
                {
                    hideOverlayCommand = new Command(
                        async () =>
                        {
                            if (IsOverlayVisible)
                            {
                                IsOverlayVisible = false;

                                await Theme.RenderUtil.AnimateFadeOutViewAsync(CloseButton);
                                await Theme.RenderUtil.AnimateFadeOutViewAsync(ContentDetailLayout);

                                await Theme.RenderUtil.AnimateFadeOutViewAsync(BackgroundWaterMark);

                                if (Opacity > 0)
                                {
                                    Opacity = 0;
                                }

                                if (IsVisible)
                                {
                                    IsVisible = false;
                                }
                            }
                        },
                        () =>
                        {
                            return IsOverlayVisible;
                        });
                }

                return hideOverlayCommand;
            }
        }

        /// <summary>
        /// Flag for the cart.
        /// </summary>
        public bool IsOverlayVisible
        {
            get
            {
                return isOverlayVisible;
            }

            set
            {
                if (isOverlayVisible != value)
                {
                    isOverlayVisible = value;
                    OnPropertyChanged("IsOverlayVisible");
                    ShowOverlayCommand.RaiseCanExecuteChanged();
                    HideOverlayCommand.RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        /// Nacigate back.
        /// </summary>
        public ICommand NavigateBackCommand { get; set; }

        /// <summary>
        /// Command for navigating to a store level.
        /// </summary>
        public Command<StoreItemLevel> NavigateToStoreLevelCommand
        {
            get
            {
                return navigateToStoreLevelCommand;
            }

            set
            {
                navigateToStoreLevelCommand = value;

                if (LevelView != null)
                {
                    LevelView.NavigateToStoreLevelCommand = value;
                }
            }
        }

        /// <summary>
        /// Show image gallery.
        /// </summary>
        public Command<object> ShowGalleryCommand
        {
            get
            {
                if (showGalleryCommand == null)
                {
                    showGalleryCommand = new Command<object>(
                        async (selectedValue) =>
                        {
                            var castedValue = selectedValue as string;

                            if (!string.IsNullOrEmpty(castedValue))
                            {
                                ViewModel.ShowGalleryCommand.ExecuteIfCan(selectedValue);
                            }
                            else if ((ViewModel != null) && (ViewModel.CurrentItemViewModel != null) && (ViewModel.CurrentItemViewModel.Images != null) && (ViewModel.CurrentItemViewModel.Images.Count > 0))
                            {
                                ViewModel.ShowGalleryCommand.ExecuteIfCan(await ViewModel.CurrentItemViewModel.Images.FirstOrDefaultAsync());
                            }
                        },
                        (selectedValue) =>
                        {
                            var castedValue = selectedValue as string;

                            if (!string.IsNullOrEmpty(castedValue))
                            {
                                return true;
                            }
                            else
                            {
                                return (ViewModel != null) && (ViewModel.CurrentItemViewModel != null) && (ViewModel.CurrentItemViewModel.Images != null) && (ViewModel.CurrentItemViewModel.Images.Count > 0);
                            }
                        });
                }

                return showGalleryCommand;
            }
        }

        /// <summary>
        /// Show store.
        /// </summary>
        public Command ShowOverlayCommand
        {
            get
            {
                if (showOverlayCommand == null)
                {
                    showOverlayCommand = new Command(
                        async () =>
                        {
                            InitializeView();
                            PrepareBindings();

                            UpdateLevelView();

                            AC.ScheduleManaged(
                                () =>
                                {
                                    UpdateProductDetail();
                                });

                            await Task.FromResult(0);

                            if (!IsOverlayVisible)
                            {
                                IsOverlayVisible = true;

                                await Task.Delay(10);

                                // Prepare animation.
                                if (Opacity < 1)
                                {
                                    Opacity = 1;
                                }

                                Theme.RenderUtil.AnimatePrepareFadeInView(BackgroundWaterMark);
                                Theme.RenderUtil.AnimatePrepareFadeInView(CloseButton);
                                Theme.RenderUtil.AnimatePrepareFadeInView(ContentDetailLayout);

                                // No animation.
                                if (!IsVisible)
                                {
                                    IsVisible = true;
                                }

                                await Theme.RenderUtil.AnimateFadeInViewAsync(BackgroundWaterMark);

                                await Theme.RenderUtil.AnimateFadeInViewAsync(ContentDetailLayout);

                                await Theme.RenderUtil.AnimateFadeInViewAsync(CloseButton);
                            }
                        },
                        () =>
                        {
                            return !IsOverlayVisible;
                        });
                }

                return showOverlayCommand;
            }
        }

        /// <summary>
        /// Top layout margin.
        /// </summary>
        public double TopLayoutMargin { get; set; }

        /// <summary>
        /// View model to use.
        /// </summary>
        public StoreDetailViewModel ViewModel
        {
            get
            {
                if (viewModel == null)
                {
                    viewModel = BindingContext as StoreDetailViewModel;
                }

                return viewModel;
            }

            set { viewModel = value; }
        }

        /// <summary>
        /// Background
        /// </summary>
        protected View BackgroundWaterMark { get; set; }

        /// <summary>
        /// Bottom shape.
        /// </summary>
        protected ShapeView BottomShape { get; set; }

        /// <summary>
        /// Button thumb.
        /// </summary>
        protected ContentViewButton ButtonThumbImage { get; set; }

        /// <summary>
        /// Layout for the image.
        /// </summary>
        protected SimpleLayout ContentDetailLayout { get; set; }

        /// <summary>
        /// Has control bar.
        /// </summary>
        protected bool HasControlBar
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Flag for vertical transition.
        /// </summary>
        protected virtual bool HasVerticalTransition
        {
            get
            {
                return Device.OS.OnPlatform(true, true, false, false, false);
            }
        }

        /// <summary>
        /// Main background image.
        /// </summary>
        protected Image ImageMain { get; set; }

        /// <summary>
        /// Image preview button width.
        /// </summary>
        protected double ImagePreviewButtonWidth
        {
            get
            {
                return Theme.CommonResources.PreviewImageWidth + (marginBordersImageThumb * 2);
            }
        }

        /// <summary>
        /// Images count.
        /// </summary>
        protected int ImagesCount { get; set; }

        /// <summary>
        /// Navigate back depending on the view.
        /// </summary>
        protected Command InternalNavigateBackCommand
        {
            get
            {
                if (internalNavigateBackCommand == null)
                {
                    internalNavigateBackCommand = new Command(
                        async () =>
                        {
                            await Task.FromResult(0);

                            if (NavigateBackCommand != null)
                            {
                                NavigateBackCommand.ExecuteIfCan();
                            }
                        });
                }

                return internalNavigateBackCommand;
            }
        }

        /// <summary>
        /// Flag when thew view is render as overlay.
        /// </summary>
        protected virtual bool IsOverlay
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Label name.
        /// </summary>
        protected ExtendedLabel LabelName { get; set; }

        /// <summary>
        /// Label short description.
        /// </summary>
        protected ExtendedLabel LabelShortDescription { get; set; }

        /// <summary>
        /// Last size.
        /// </summary>
        protected Rectangle LastSizePageCanvas { get; set; }

        /// <summary>
        /// Level view.
        /// </summary>
        protected StoreItemLevelSimpleView LevelView { get; set; }

        /// <summary>
        /// Nav bar back.
        /// </summary>
        protected ShapeView NavBarBack { get; set; }

        /// <summary>
        /// Price button.
        /// </summary>
        protected ExtendedLabel PriceLabel { get; set; }

        /// <summary>
        /// Recycler images.
        /// </summary>
        protected RepeaterRecycleView RecyclerImages { get; set; }

        /// <summary>
        /// Thumb image.
        /// </summary>
        protected StoreItemThumbOnlyView ThumbImage { get; set; }

        /// <summary>
        /// Web view for the description.
        /// </summary>
        protected WebView WebViewDescription { get; set; }

        /// <summary>
        /// Close button.
        /// </summary>
        private ContentViewButton CloseButton { get; set; }

        /// <summary>
        /// Update the product description.
        /// </summary>
        public void UpdateProductDetail()
        {
            if ((ViewModel != null) && (ViewModel.CurrentItemViewModel != null) && (WebViewDescription != null))
            {
                // Description name.
                var descriptionContent = new HtmlWebViewSource();

                StringBuilder longDescription = new StringBuilder("<html>");
                longDescription.Append("<head>");
                longDescription.Append("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">");
                longDescription.Append("<meta name=\"viewport\" content=\"width=device-width, height=device-height, initial-scale=1\" />");
                longDescription.Append("</head>");
                longDescription.Append("<style>");
                longDescription.Append("@import url(http://fonts.googleapis.com/css?family=Roboto+Condensed);");
                longDescription.Append("body,p,h1 ");
                longDescription.Append("{");
                longDescription.Append("font-family: \"Roboto Condensed\", sans-serif; ");
                longDescription.AppendFormat("font-size:{0} px; ", Theme.CommonResources.TextSizeMedium);
                longDescription.Append("}");
                longDescription.Append("</style>");
                longDescription.Append("<body>");

                if (ViewModel != null)
                {
                    longDescription.Append(ViewModel.CurrentItemViewModel.Item.LongDescription);
                }

                longDescription.Append("</body>");
                longDescription.Append("</html>");

                descriptionContent.Html = longDescription.ToString();

                WebViewDescription.Source = descriptionContent;
            }
        }

        /// <summary>
        /// Add controls to layout.
        /// </summary>
        protected override void AddControlsToLayout()
        {
            AddViewToLayout(BackgroundWaterMark);
            AddViewToLayout(ContentDetailLayout);
            AddViewToLayout(ImageMain, ContentDetailLayout);
            AddViewToLayout(BottomShape, ContentDetailLayout);
            AddViewToLayout(ThumbImage, ContentDetailLayout);
            AddViewToLayout(NavBarBack, ContentDetailLayout);
            AddViewToLayout(LevelView, ContentDetailLayout);
            AddViewToLayout(LabelName, ContentDetailLayout);
            AddViewToLayout(LabelShortDescription, ContentDetailLayout);
            AddViewToLayout(WebViewDescription, ContentDetailLayout);
            AddViewToLayout(PriceLabel, ContentDetailLayout);
            AddViewToLayout(ButtonThumbImage, ContentDetailLayout);
            AddViewToLayout(RecyclerImages, ContentDetailLayout);
            AddViewToLayout(CloseButton);
        }

        /// <summary>
        /// Layout children.
        /// </summary>
        /// <param name="x">Top to use.</param>
        /// <param name="y">Left to use.</param>
        /// <param name="width">Width to use.</param>
        /// <param name="height">Height to use.</param>
        protected virtual void ContentDetailLayout_OnLayoutChildren(double x, double y, double width, double height)
        {
            float archSize = 3;
            float thumpPosition = 0.21f;
            double minHeightDetails = 200;

            Rectangle imageThumbPosition = new Rectangle();
            Rectangle levelViewPosition = new Rectangle();
            Rectangle namePosition = new Rectangle();
            Rectangle shortDescriptionPosition = new Rectangle();
            Rectangle longDescriptionPosition = new Rectangle();
            Rectangle pricePosition = new Rectangle();

            if (BottomShape != null)
            {
                double elementLeft = (width * 0.5f) - ((width * archSize) * 0.5f);
                double elementTop = (height * thumpPosition);
                double elementWidth = width * archSize;
                double elementHeight = elementWidth;
                var elementPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);
                BottomShape.LayoutUpdate(elementPosition);
            }

            if (ImageMain != null)
            {
                double elementMargin = ContentMargin;
                double elementLeft = -elementMargin;
                double elementTop = 0;
                double elementWidth = width + (elementMargin * 2f);
                double elementHeight = height * 0.5f;
                var elementPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);
                ImageMain.LayoutUpdate(elementPosition);
            }

            if (ThumbImage != null)
            {
                var elementSize = ThumbImage.GetSizeRequest(width, height).Request;
                double elementLeft = (width - elementSize.Width) * 0.5f;
                double elementTop = (height * thumpPosition) - (elementSize.Height * 0.5f);
                double elementWidth = elementSize.Width;
                double elementHeight = elementSize.Height;
                imageThumbPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);
                ThumbImage.LayoutUpdate(imageThumbPosition);
            }

            if (ButtonThumbImage != null)
            {
                ButtonThumbImage.LayoutUpdate(imageThumbPosition);
            }

            if (LevelView != null)
            {
                var elementSize = LevelView.GetSizeRequest(width, height).Request;
                double elementLeft = ContentMargin;
                double elementTop = ContentMargin;
                double elementWidth = elementSize.Width;
                double elementHeight = elementSize.Height;
                levelViewPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);
                LevelView.LayoutUpdate(levelViewPosition);
            }

            if (NavBarBack != null)
            {
                NavBarBack.LayoutUpdate(levelViewPosition);
            }

            if (PriceLabel != null)
            {
                double maxWidth = width - imageThumbPosition.X;
                var elementSize = PriceLabel.GetSizeRequest(maxWidth, height).Request;
                double elementWidth = elementSize.Width;
                double elementHeight = elementSize.Height;
                double elementTop = imageThumbPosition.Y + imageThumbPosition.Height + (Margin * 0.5f);

                double elementLeft = (width - elementWidth) * 0.5f;

                pricePosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);
                PriceLabel.LayoutUpdate(pricePosition);
            }

            double detailsWidth = width - ((ContentMargin * 3) * 2);

            if (LabelName != null)
            {
                var elementSize = LabelName.GetSizeRequest(detailsWidth, height).Request;
                double elementTop = pricePosition.Y + pricePosition.Height + (ContentMargin * 0.5f);
                double elementHeight = elementSize.Height;
                double elementWidth = elementSize.Width;
                double elementLeft = (width - elementWidth) * 0.5f;

                namePosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);
                LabelName.LayoutUpdate(namePosition);
            }

            if (LabelShortDescription != null)
            {
                var elementSize = LabelShortDescription.GetSizeRequest(detailsWidth, height).Request;
                double elementTop = namePosition.Y + namePosition.Height + (ContentMargin * 0);
                double elementHeight = elementSize.Height;
                double elementWidth = elementSize.Width;
                double elementLeft = (width - elementWidth) * 0.5f;

                shortDescriptionPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);
                LabelShortDescription.LayoutUpdate(shortDescriptionPosition);
            }

            double controlImagesHeight = 0;

            if (ImagesCount > 0)
            {
                controlImagesHeight = Theme.CommonResources.PreviewImageWidth + (marginBordersImageThumb * 2);
            }

            if ((height - shortDescriptionPosition.Y) - controlImagesHeight < minHeightDetails)
            {
                controlImagesHeight = 0;
            }

            if (WebViewDescription != null)
            {
                double elementLeft = (ContentMargin * 3);
                double elementTop = shortDescriptionPosition.Y + shortDescriptionPosition.Height + (ContentMargin * 1);
                double elementWidth = detailsWidth;
                double elementHeight = height - elementTop - controlImagesHeight - (ContentMargin * 2);

                longDescriptionPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);
                WebViewDescription.LayoutUpdate(longDescriptionPosition);
            }

            if (RecyclerImages != null)
            {
                double maxWidth = width - imageThumbPosition.X;

                double elementWidth = (ImagesCount * (ImagePreviewButtonWidth + RecyclerImages.Spacing)).Clamp(0, detailsWidth);
                double elementHeight = controlImagesHeight;
                double elementTop = height - elementHeight - (ContentMargin * 1);
                double elementLeft = (width - elementWidth) * 0.5f;

                var elementPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);
                RecyclerImages.LayoutUpdate(elementPosition);
            }
        }

        /// <summary>
        /// Mesure the content.
        /// </summary>
        /// <param name="widthConstraint">Width to constarint.</param>
        /// <param name="heightConstraint">Height to constraint.</param>
        /// <returns></returns>
        protected virtual SizeRequest ContentDetailLayout_OnSizeRequest(double widthConstraint, double heightConstraint)
        {
            SizeRequest resultRequest = new SizeRequest(new Size(widthConstraint, heightConstraint), new Size(widthConstraint, heightConstraint));
            return resultRequest;
        }

        /// <summary>
        /// Layout the children.
        /// </summary>
        /// <param name="x">Top to use.</param>
        /// <param name="y">Left to use.</param>
        /// <param name="width">Width to use.</param>
        /// <param name="height">Height to use.</param>
        protected override void ContentLayout_OnLayoutChildren(double x, double y, double width, double height)
        {
            if (BackgroundWaterMark != null)
            {
                BackgroundWaterMark.LayoutUpdate(new Rectangle(x, y, width, height));
            }

            if (IsOverlay)
            {
                LastSizePageCanvas = new Rectangle(x, y + TopLayoutMargin, width, height - BottomAppBarMargin - TopLayoutMargin);
            }
            else
            {
                LastSizePageCanvas = new Rectangle(x, y, width, height);
            }

            if (CloseButton != null)
            {
                double elementHeight = Theme.CommonResources.RoundedButtonWidth;
                double elementLeft = width - Theme.CommonResources.RoundedButtonWidth - 2;
                double elementTop = y + ContentMargin + TopLayoutMargin - (elementHeight * 0.5f);

                CloseButton.LayoutUpdate(new Rectangle(elementLeft, elementTop, elementHeight, elementHeight));
            }

            if (ContentDetailLayout != null)
            {
                ContentDetailLayout.LayoutUpdate(LastSizePageCanvas);
            }
        }

        /// <summary>
        /// Initialize elements.
        /// </summary>
        protected override void InternalInitializeView()
        {
            // Layout.
            ContentDetailLayout = new SimpleLayout()
            {
                IsHandlingLayoutManually = true,
                Padding = 0,
                IsClippedToBounds = true,
                BackgroundColor = Theme.CommonResources.Accent
            };

            ContentDetailLayout.OnLayoutChildren += ContentDetailLayout_OnLayoutChildren;
            ContentDetailLayout.ManualSizeCalculationDelegate = ContentDetailLayout_OnSizeRequest;

            if (IsOverlay && HasControlBar)
            {
                // Substract button.
                CloseButton = new GlyphOnlyContentViewButton()
                {
                    Style = Theme.ApplicationStyles.GlyphOnlyRoundedContentButtonStyle,
                    GlyphText = Theme.CommonResources.GlyphTextCancel,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Command = CloseOverlayCommand,
                    Opacity = 0,
                    IsVisible = false
                };
            }

            if (IsOverlay)
            {
                BackgroundWaterMark = Theme.RenderUtil.InstanceBackgroundDetail(
                        () =>
                        {
                            if (this.CloseOverlayCommand != null)
                            {
                                this.CloseOverlayCommand.ExecuteIfCan();
                            }
                        });
            }

            // Main image.
            ImageMain = new ExtendedImage()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Aspect = Aspect.AspectFill
            };

            BottomShape = new ShapeView()
            {
                Color = Theme.CommonResources.BackgroundColorItem,
                ShapeType = ShapeType.Box
            };

            ThumbImage = new StoreItemThumbOnlyView();

            // Image thumb button.
            ButtonThumbImage = new ContentViewButton(false, false, ImageOrientation.ImageOnTop)
            {
                ButtonBackgroundColor = Color.Transparent,
                CornerRadius = 10
            };

            LevelView = new StoreItemLevelSimpleView(false, false, false, true)
            {
                Padding = 5,
                BackgroundCustomColor = Theme.CommonResources.Accent,
                BackgroundMargin = 10,
                BackgroundTranslateX = ContentMargin + 20
            };

            // label name.
            LabelName = new ExtendedLabel()
            {
                TextColor = Color.Black,
                FontName = Theme.CommonResources.FontRobotBoldCondensedName,
                FriendlyFontName = Theme.CommonResources.FontRobotBoldCondensedFriendlyName,
                FontSize = Theme.CommonResources.TextSizeLarge,
                FontAttributes = FontAttributes.Bold
            };

            // Short description.
            LabelShortDescription = new ExtendedLabel()
            {
                FontName = Theme.CommonResources.FontRobotLightName,
                FriendlyFontName = Theme.CommonResources.FontRobotLightFriendlyName,
                FontSize = Theme.CommonResources.TextSizeMicro,
                TextColor = Theme.CommonResources.TextColorDetailValue
            };

            WebViewDescription = new WebView()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Theme.CommonResources.PagesBackgroundColorLight
            };

            PriceLabel = new ExtendedLabel()
            {
                Style = Theme.ApplicationStyles.DetailNameExtendedLabelStyle,
                TextColor = Theme.CommonResources.Accent,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };

            DataTemplate itemTemplate = new DataTemplate(
                    () =>
                    {
                        ContentViewButton imageDetailPreview = new ImageContentViewButton()
                        {
                            Style = Theme.ApplicationStyles.TextOnlyContentButtonStyle,
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.Center,
                            ImageWidthRequest = Theme.CommonResources.PreviewImageWidth,
                            ImageHeightRequest = Theme.CommonResources.PreviewImageWidth,
                            ButtonDisableBackgroundColor = Color.Transparent,
                            ButtonBackgroundColor = Color.White,
                            StrokeColor = Color.Black,
                            StrokeWidth = 3,
                            CornerRadius = 5,
                            MarginBorders = 0,
                            MarginElements = 0,
                            Command = ShowGalleryCommand,
                            WidthRequest = ImagePreviewButtonWidth,
                            HeightRequest = ImagePreviewButtonWidth,
                            ImageAspect = Aspect.AspectFill
                        };

                        imageDetailPreview.SetBinding(ContentViewButton.SourceProperty, ".");
                        imageDetailPreview.SetBinding(ContentViewButton.CommandParameterProperty, ".");

                        return imageDetailPreview;
                    });

            RecyclerImages = new RepeaterRecycleView()
            {
                Padding = 0,
                Spacing = 10,
                ItemTemplate = itemTemplate,
                ItemHeight = ImagePreviewButtonWidth,
                ItemWidth = ImagePreviewButtonWidth
            };
        }

        /// <summary>
        /// Binding context changed.
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            ViewModel = null;

            base.OnBindingContextChanged();

            if (ViewModel != null)
            {
                ImagesCount = (ViewModel != null) && (ViewModel.CurrentItemViewModel != null) && (ViewModel.CurrentItemViewModel.Images != null) ? ViewModel.CurrentItemViewModel.Images.Count : 0;
            }
        }

        /// <summary>
        /// Setup bindings.
        /// </summary>
        protected override void SetupBindings()
        {
            if (ImageMain != null)
            {
                ImageMain.SetBinding<StoreDetailViewModel>(Image.SourceProperty, vm => vm.CurrentItemViewModel.MainImagePath);
            }

            if (ThumbImage != null)
            {
                ThumbImage.SetBinding<StoreDetailViewModel>(View.BindingContextProperty, vm => vm.CurrentItemViewModel);
                ThumbImage.PrepareBindings();
            }

            if (ButtonThumbImage != null)
            {
                ButtonThumbImage.Command = ShowGalleryCommand;
            }

            if (LevelView != null)
            {
                LevelView.SetBinding<StoreDetailViewModel>(StoreItemLevelSimpleView.BindingContextProperty, vm => vm.CurrentItemViewModel.ItemLevel);
                LevelView.PrepareBindings();
            }

            if (LabelName != null)
            {
                LabelName.SetBinding<StoreDetailViewModel>(ExtendedLabel.TextProperty, vm => vm.CurrentItemViewModel.Item.Name);
            }

            if (LabelShortDescription != null)
            {
                LabelShortDescription.SetBinding<StoreDetailViewModel>(ExtendedLabel.TextProperty, vm => vm.CurrentItemViewModel.Item.ShortDescription);
            }

            if (PriceLabel != null)
            {
                PriceLabel.Text = "$0";
            }

            if (RecyclerImages != null)
            {
                RecyclerImages.SetBinding<StoreDetailViewModel>(RepeaterRecycleView.ItemsSourceProperty, vm => vm.CurrentItemViewModel.Images);
            }
        }

        /// <summary>
        /// Update level view.
        /// </summary>
        protected void UpdateLevelView()
        {
            if (ViewModel != null)
            {
                if (LevelView != null)
                {
                    LevelView.NavigateBackCommand = InternalNavigateBackCommand;
                    LevelView.NavigateToStoreLevelCommand = NavigateToStoreLevelCommand;
                }

                ImagesCount = (ViewModel != null) && (ViewModel.CurrentItemViewModel != null) && (ViewModel.CurrentItemViewModel.Images != null) ? ViewModel.CurrentItemViewModel.Images.Count : 0;
            }
        }
    }
}