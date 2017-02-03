// <copyright file="StoreDetailGroupView.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Anuracode.Forms.Controls.Extensions;
using Anuracode.Forms.Controls.Views.Extensions;
using Anuracode.Forms.Controls.Sample.ViewModels;

namespace Anuracode.Forms.Controls.Sample.Views
{
    /// <summary>
    /// View for the groups.
    /// </summary>
    public class StoreDetailSmallSimpleView : SimpleViewBase
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
        /// Flag for the cart.
        /// </summary>
        private bool isOverlayVisible;

        /// <summary>
        /// Selected item viewmodel.
        /// </summary>
        private StoreItemViewModel selectedItemViewModel;

        /// <summary>
        /// Show cart command.
        /// </summary>
        private Command showOverlayCommand;

        /// <summary>
        /// View model to use.
        /// </summary>
        private IStoreListViewModel storeListViewModel;

        /// <summary>
        /// Command for viewing item detail.
        /// </summary>
        private Command<object> viewItemDetailCommand;

        /// <summary>
        /// View model to use.
        /// </summary>
        private StoreListViewModel viewModel;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StoreDetailSmallSimpleView()
            : base(false)
        {
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

                    OnPropertyChanged(nameof(CloseOverlayCommand));
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
        /// Selected item viewmodel.
        /// </summary>
        public StoreItemViewModel SelectedItemViewModel
        {
            get
            {
                return selectedItemViewModel;
            }

            set
            {
                if (selectedItemViewModel != value)
                {
                    selectedItemViewModel = value;
                    OnPropertyChanged("SelectedItemViewModel");
                }
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

                            if (SelectedItemViewModel != null)
                            {
                                if (SelectedItemPreview.BindingContext != SelectedItemViewModel)
                                {
                                    SelectedItemPreview.BindingContext = SelectedItemViewModel;
                                }

                                SelectedItemPreview.PrepareBindings();
                            }

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
        /// View model to use.
        /// </summary>
        public IStoreListViewModel StoreListViewModel
        {
            get
            {
                return storeListViewModel;
            }

            set
            {
                if (storeListViewModel != value)
                {
                    storeListViewModel = value;                    
                }
            }
        }

        /// <summary>
        /// Top layout margin.
        /// </summary>
        public double TopLayoutMargin { get; set; }

        /// <summary>
        /// Command for viewing item detail.
        /// </summary>
        public Command<object> ViewItemDetailCommand
        {
            get
            {
                return viewItemDetailCommand;
            }

            set
            {
                if (viewItemDetailCommand != value)
                {
                    viewItemDetailCommand = value;

                    if ((ButtonIconShowDetail != null) && (ButtonIconShowDetail.Command != viewItemDetailCommand))
                    {
                        ButtonIconShowDetail.Command = viewItemDetailCommand;
                    }

                    if ((ButtonTextDetail != null) && (ButtonTextDetail.Command != viewItemDetailCommand))
                    {
                        ButtonTextDetail.Command = viewItemDetailCommand;
                    }

                    if ((ButtonThumbImage != null) && (ButtonThumbImage.Command != viewItemDetailCommand))
                    {
                        ButtonThumbImage.Command = viewItemDetailCommand;
                    }
                }
            }
        }

        /// <summary>
        /// View model to use.
        /// </summary>
        public StoreListViewModel ViewModel
        {
            get
            {
                if (viewModel == null)
                {
                    viewModel = BindingContext as StoreListViewModel;
                }

                return viewModel;
            }

            set
            {
                viewModel = value;
            }
        }

        /// <summary>
        /// Border over the picture.
        /// </summary>
        protected View BackgroundOverBorderShape { get; set; }

        /// <summary>
        /// Background shape.
        /// </summary>
        protected View BackgroundShape { get; set; }

        /// <summary>
        /// Background
        /// </summary>
        protected View BackgroundWaterMark { get; set; }

        /// <summary>
        /// Show detail button.
        /// </summary>
        protected ContentViewButton ButtonIconShowDetail { get; set; }



        /// <summary>
        /// Button text detail.
        /// </summary>
        protected ContentViewButton ButtonTextDetail { get; set; }


        /// <summary>
        /// Button thumb.
        /// </summary>
        protected ContentViewButton ButtonThumbImage { get; set; }

        /// <summary>
        /// Layout for the content.
        /// </summary>
        protected SimpleLayout ContentDetailLayout { get; set; }

        /// <summary>
        /// Flag when thew view is render as overlay.
        /// </summary>
        protected bool IsOverlay
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Last size.
        /// </summary>
        protected Rectangle LastSizePageCanvas { get; set; }

        /// <summary>
        /// Selected item preview.
        /// </summary>
        protected StoreItemThumbFeaturedVerticalView SelectedItemPreview { get; set; }

        /// <summary>
        /// Close button.
        /// </summary>
        private ContentViewButton CloseButton { get; set; }

        /// <summary>
        /// Setup the bindings.
        /// </summary>
        protected override void SetupBindings()
        {
            if (ButtonIconShowDetail != null)
            {
                ButtonIconShowDetail.BindingContext = this;
                ButtonIconShowDetail.SetBinding<StoreDetailSmallSimpleView>(ContentViewButton.CommandParameterProperty, vm => vm.SelectedItemViewModel);
                ButtonIconShowDetail.Command = ViewItemDetailCommand;
            }

            if (ButtonTextDetail != null)
            {
                ButtonTextDetail.BindingContext = this;
                ButtonTextDetail.SetBinding<StoreDetailSmallSimpleView>(ContentViewButton.CommandParameterProperty, vm => vm.SelectedItemViewModel);
                ButtonTextDetail.Command = ViewItemDetailCommand;
            }

            if (ButtonThumbImage != null)
            {
                ButtonThumbImage.BindingContext = this;
                ButtonThumbImage.SetBinding<StoreDetailSmallSimpleView>(ContentViewButton.CommandParameterProperty, vm => vm.SelectedItemViewModel);
                ButtonThumbImage.Command = ViewItemDetailCommand;
            }
        }

        /// <summary>
        /// Add control to layout.
        /// </summary>
        protected override void AddControlsToLayout()
        {
            AddViewToLayout(BackgroundWaterMark);
            AddViewToLayout(ContentDetailLayout);
            AddViewToLayout(BackgroundShape, ContentDetailLayout);
            AddViewToLayout(SelectedItemPreview, ContentDetailLayout);
            AddViewToLayout(BackgroundOverBorderShape, ContentDetailLayout);


            AddViewToLayout(ButtonIconShowDetail, ContentDetailLayout);
            AddViewToLayout(ButtonTextDetail, ContentDetailLayout);

            AddViewToLayout(ButtonThumbImage, ContentDetailLayout);

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
            Rectangle backgroundPosition = new Rectangle();
            Rectangle previewPosition = new Rectangle();
            Rectangle iconDetailPosition = new Rectangle();
            Rectangle textDetailPosition = new Rectangle();

            if (BackgroundShape != null)
            {
                double elementLeft = 0;
                double elementTop = 0;
                double elementWidth = width;
                double elementHeight = height;
                backgroundPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);
                BackgroundShape.LayoutUpdate(backgroundPosition);
            }

            if (BackgroundOverBorderShape != null)
            {
                BackgroundOverBorderShape.LayoutUpdate(backgroundPosition);
            }

            if (SelectedItemPreview != null)
            {
                var elementSize = SelectedItemPreview.Measure(width, height).Request;
                double elementMargin = 3;
                double elementLeft = elementMargin;
                double elementTop = elementMargin;
                double elementWidth = elementSize.Width.Clamp(0, width);
                double elementHeight = elementSize.Height.Clamp(0, height);
                previewPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);
                SelectedItemPreview.LayoutUpdate(previewPosition);
            }

            if (ButtonThumbImage != null)
            {
                float borderWidth = 3;
                double elementWidth = Theme.CommonResources.CategoryImageWidth + (borderWidth * 2f);
                double elementHeight = elementWidth;
                double elementLeft = (width - elementWidth) * 0.5f;
                double elementTop = ContentMargin + (borderWidth * 4f);
                var elementPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);
                ButtonThumbImage.LayoutUpdate(elementPosition);
            }

            if ((ButtonIconShowDetail != null) && ButtonIconShowDetail.IsVisible)
            {
                var elementSize = ButtonIconShowDetail.Measure(width, height).Request;
                double elementWidth = elementSize.Width;
                double elementHeight = elementSize.Height;
                double elementTop = previewPosition.Y + previewPosition.Height + ContentMargin;
                double elementLeft = ContentMargin * 2f;

                iconDetailPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);
                ButtonIconShowDetail.LayoutUpdate(iconDetailPosition);
            }

            if ((ButtonTextDetail != null) && ButtonTextDetail.IsVisible)
            {
                double maxWidth = width - (iconDetailPosition.X * 2f) - iconDetailPosition.Width - ContentMargin;
                var elementSize = ButtonTextDetail.Measure(maxWidth, height).Request;
                double elementWidth = elementSize.Width.Clamp(0, maxWidth);
                double elementHeight = elementSize.Height;
                double elementTop = iconDetailPosition.Y;
                double elementLeft = iconDetailPosition.X + iconDetailPosition.Width + ContentMargin;

                textDetailPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);
                ButtonTextDetail.LayoutUpdate(textDetailPosition);
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
        /// Layout children.
        /// </summary>
        /// <param name="x">Top to use.</param>
        /// <param name="y">Left to use.</param>
        /// <param name="width">Width to use.</param>
        /// <param name="height">Height to use.</param>
        protected override void ContentLayout_OnLayoutChildren(double x, double y, double width, double height)
        {
            Rectangle detailPosition = new Rectangle();

            if (IsOverlay)
            {
                LastSizePageCanvas = new Rectangle(x, y + TopLayoutMargin, width, height - BottomAppBarMargin - TopLayoutMargin);
            }
            else
            {
                LastSizePageCanvas = new Rectangle(x, y, width, height);
            }

            if (BackgroundWaterMark != null)
            {
                Rectangle elementPosition = new Rectangle(x, y, width, height);

                BackgroundWaterMark.LayoutUpdate(elementPosition);
            }

            if (ContentDetailLayout != null)
            {
                Size elementSize = new Size();
                Size detailSize = new Size();

                if (SelectedItemPreview != null)
                {
                    elementSize = SelectedItemPreview.Measure(width, height).Request;
                }

                if ((ButtonIconShowDetail != null) && ButtonIconShowDetail.IsVisible)
                {
                    detailSize = ButtonIconShowDetail.Measure(width, height).Request;
                }

                double elementWidth = elementSize.Width + (3 * 2);
                double elementLeft = (width - elementWidth) * 0.5f;
                double elementHeight = detailSize.Height + elementSize.Height + (ContentMargin * 5f);
                double elementTop = TopLayoutMargin + ((height - TopLayoutMargin - BottomAppBarMargin - elementHeight) * 0.5f);

                detailPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);
                ContentDetailLayout.LayoutUpdate(detailPosition);
            }

            if (CloseButton != null)
            {
                var elementSize = CloseButton.Measure(width, height).Request;
                double elementWidth = elementSize.Width;
                double elementHeight = elementSize.Height;
                double elementLeft = (detailPosition.X + detailPosition.Width) - (elementWidth * 0.5f);
                double elementTop = detailPosition.Y - (elementHeight * 0.5f);

                Rectangle elementPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);
                CloseButton.LayoutUpdate(elementPosition);
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
                IsClippedToBounds = true
            };

            ContentDetailLayout.OnLayoutChildren += ContentDetailLayout_OnLayoutChildren;
            ContentDetailLayout.ManualSizeCalculationDelegate = ContentDetailLayout_OnSizeRequest;

            BackgroundWaterMark = Theme.RenderUtil.InstanceBackgroundDetail(
                       () =>
                       {
                           if (this.CloseOverlayCommand != null)
                           {
                               this.CloseOverlayCommand.ExecuteIfCan();
                           }
                       });

            // Substract button.
            CloseButton = new GlyphOnlyContentViewButton(hasBackground: true)
            {
                Style = Theme.ApplicationStyles.GlyphOnlyRoundedContentButtonStyle,
                GlyphText = Theme.CommonResources.GlyphTextCancel,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Command = CloseOverlayCommand
            };

            BackgroundShape = new ShapeView()
            {
                Color = Theme.CommonResources.PagesBackgroundColorLight,
                StrokeColor = Theme.CommonResources.PagesBackgroundColorLight,
                CornerRadius = 10,
                StrokeWidth = 3
            };

            BackgroundOverBorderShape = new ShapeView()
            {
                Color = Color.Transparent,
                StrokeColor = Theme.CommonResources.Accent,
                CornerRadius = 10,
                StrokeWidth = 6
            };

            SelectedItemPreview = new StoreItemThumbFeaturedVerticalView();            

            ButtonIconShowDetail = new GlyphOnlyContentViewButton(hasBackground: true)
            {
                Style = Theme.ApplicationStyles.GlyphOnlyRoundedContentButtonStyle,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start,
                DisableGlyphOnly = false,
                GlyphText = Theme.CommonResources.GlyphTextDetail
            };

            ButtonTextDetail = new TextContentViewButton()
            {
                Style = Theme.ApplicationStyles.TextOnlyContentButtonStyle,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start,
                MinimumWidthRequest = 150,
                Text = App.LocalizationResources.StoreListShowItemDetailLabel,
                TextColor = Theme.CommonResources.Accent,
                MarginBorders = 10
            };           

            // Image thumb button.
            ButtonThumbImage = new ContentViewButton(false, false, ImageOrientation.ImageOnTop)
            {                
                CornerRadius = 10
            };
        }
    }
}