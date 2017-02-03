// <copyright file="StoreDetailGroupView.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Sample.ViewModels;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Anuracode.Forms.Controls.Extensions;
using Anuracode.Forms.Controls.Views.Extensions;

namespace Anuracode.Forms.Controls.Sample.Views
{
    /// <summary>
    /// View for the groups.
    /// </summary>
    public class StoreDetailGroupView : SimpleViewBase
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
        public StoreDetailGroupView()
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

                                this.UpdateOpacity(0);
                                this.UpdateIsVisible(false);
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
        /// Command for the item detail.
        /// </summary>
        public Command<StoreItemViewModel> ItemDetailCommand { get; set; }

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

                            UpdateProductInfo();

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
        /// Layout for the content.
        /// </summary>
        protected SimpleLayout ContentDetailLayout { get; set; }

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
        /// Last size.
        /// </summary>
        protected Rectangle LastSizePageCanvas { get; set; }

        /// <summary>
        /// List of all elements.
        /// </summary>
        protected InfiniteListView ListGroupedElements { get; set; }

        /// <summary>
        /// Selected item preview.
        /// </summary>
        protected StoreItemThumbFeaturedVerticalView SelectedItemPreview { get; set; }

        /// <summary>
        /// Close button.
        /// </summary>
        private ContentViewButton CloseButton { get; set; }

        /// <summary>
        /// Set up cell values.
        /// </summary>
        /// <param name="isRecycled">Is recycled.</param>
        public override void SetupViewValues(bool isRecycled)
        {
            base.SetupViewValues(isRecycled);

            if (ViewModel != null)
            {
                if ((ListGroupedElements != null) && (ListGroupedElements.ItemsSource != ViewModel.GroupItems))
                {
                    ListGroupedElements.ItemsSource = ViewModel.GroupItems;
                }
            }
        }

        /// <summary>
        /// Update product info.
        /// </summary>
        public void UpdateProductInfo()
        {
            if ((ViewModel != null) && (SelectedItemPreview != null) && (ViewModel.CurrentLoadedGroup != null))
            {
                if (SelectedItemPreview.BindingContext != ViewModel.CurrentLoadedGroup)
                {
                    SelectedItemPreview.BindingContext = ViewModel.CurrentLoadedGroup;
                }

                SelectedItemPreview.PrepareBindings();
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
            AddViewToLayout(ListGroupedElements, ContentDetailLayout);
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
            Rectangle listPosition = new Rectangle();
            Rectangle previewPosition = new Rectangle();

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

                if (IsOverlay)
                {
                    double elementMargin = 3;
                    double elementLeft = elementMargin;
                    double elementTop = elementMargin;
                    double elementWidth = elementSize.Width.Clamp(0, width);
                    double elementHeight = elementSize.Height.Clamp(0, height);
                    previewPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);
                    SelectedItemPreview.LayoutUpdate(previewPosition);
                }
                else
                {
                    double elementLeft = 0;
                    double elementTop = 0;
                    double elementWidth = width;
                    double elementHeight = elementSize.Height.Clamp(0, height);
                    previewPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);
                    SelectedItemPreview.LayoutUpdate(previewPosition);
                }
            }

            if (ListGroupedElements != null)
            {
                double elementMargin = ContentMargin * 2f;
                double elementTop = previewPosition.Y + previewPosition.Height + elementMargin;
                double elementHeight = height - elementTop - ContentMargin;
                double elementWidth = (backgroundPosition.Width - (elementMargin * 2f)).Clamp(0, 400f);
                double elementLeft = (width - elementWidth) * 0.5f;
                listPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                ListGroupedElements.LayoutUpdate(listPosition);
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
                if (IsOverlay)
                {
                    Size elementSize = new Size();

                    if (SelectedItemPreview != null)
                    {
                        elementSize = SelectedItemPreview.Measure(width, height).Request;
                    }

                    double elementWidth = elementSize.Width + (3 * 2);
                    double elementLeft = (width - elementWidth) * 0.5f;
                    double elementHeight = height - BottomAppBarMargin - TopLayoutMargin - (ContentMargin * 4);
                    double elementTop = TopLayoutMargin + (ContentMargin * 2f);
                    detailPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);
                    ContentDetailLayout.LayoutUpdate(detailPosition);
                }
                else
                {
                    ContentDetailLayout.LayoutUpdate(LastSizePageCanvas);
                }
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

                // Substract button.
                CloseButton = new GlyphOnlyContentViewButton(hasBackground: true)
                {
                    Style = Theme.ApplicationStyles.GlyphOnlyRoundedContentButtonStyle,
                    GlyphText = Theme.CommonResources.GlyphTextCancel,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Command = CloseOverlayCommand
                };

                BackgroundOverBorderShape = new ShapeView()
                {
                    Color = Color.Transparent,
                    StrokeColor = Theme.CommonResources.Accent,
                    CornerRadius = 10,
                    StrokeWidth = 6
                };
            }

            ListGroupedElements = new InfiniteListView(ListViewCachingStrategy.RecycleElement)
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                SeparatorVisibility = SeparatorVisibility.None,
                Style = Theme.ApplicationStyles.CommonListViewStyle
            };

            ListGroupedElements.ItemTapped -= ListGroupedElements_ItemTapped;
            ListGroupedElements.ItemTapped += ListGroupedElements_ItemTapped;

            BackgroundShape = new ShapeView()
            {
                Color = Theme.CommonResources.PagesBackgroundColor,
                StrokeColor = Theme.CommonResources.PagesBackgroundColor,
                CornerRadius = 10,
                StrokeWidth = 3
            };

            SelectedItemPreview = new StoreItemThumbFeaturedVerticalView()
            {
                DecorationColor = Theme.CommonResources.PagesBackgroundColor
            };

            SelectedItemPreview.ContentLayout.IsClippedToBounds = IsOverlay;
        }

        /// <summary>
        /// Selected item changed.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Arguments of the event.</param>
        protected virtual void ListGroupedElements_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                if (ItemDetailCommand != null)
                {
                    ItemDetailCommand.ExecuteIfCan(e.Item);
                }

                AC.ScheduleManaged(
                    () =>
                    {
                        try
                        {
                            if ((ListGroupedElements != null) && (ListGroupedElements.SelectedItem != null))
                            {
                                ListGroupedElements.SelectedItem = null;
                            }
                        }
                        catch
                        {
                        }

                        return Task.FromResult(0);
                    });
            }
        }

        /// <summary>
        /// Setup the bindings.
        /// </summary>
        protected override void SetupBindings()
        {
            if (ListGroupedElements != null)
            {
                ListGroupedElements.ItemTemplate = new DataTemplate(
                () =>
                {
                    return new StoreItemThumbVerticalBarViewCell();
                });
            }
        }
    }
}