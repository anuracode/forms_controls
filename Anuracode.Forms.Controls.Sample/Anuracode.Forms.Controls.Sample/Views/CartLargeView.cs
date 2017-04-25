// <copyright file="CartSmallView.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Extensions;
using Anuracode.Forms.Controls.Views.Extensions;
using Anuracode.Forms.Controls.Sample.ViewModels;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls.Sample.Views
{
    /// <summary>
    /// Cart large view.
    /// </summary>
    public class CartLargeView : SimpleViewBase
    {
        /// <summary>
        /// Hide cart command.
        /// </summary>
        private Command hideOverlayCommand;

        /// <summary>
        /// Navigate to store level.
        /// </summary>
        private Command<object> internalShowItemLargeDetailCommand;

        /// <summary>
        /// Flag for the cart.
        /// </summary>
        private bool isOverlayVisible;

        /// <summary>
        /// Lock for the animtaion.
        /// </summary>
        private SemaphoreSlim lockAnimation;

        /// <summary>
        /// Show cart command.
        /// </summary>
        private Command showOverlayCommand;

        /// <summary>
        /// Viewmodel for the view.
        /// </summary>
        private CartListViewModel viewModel;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public CartLargeView()
            : base(false)
        {
            BindingContext = ViewModel;

            if (IsOverlay)
            {
                Opacity = 0;
                IsVisible = false;
            }
        }

        /// <summary>
        /// Command for adding more items.
        /// </summary>
        public ICommand AddMoreItemsCommand
        {
            get
            {
                return ButtonAddMoreItems == null ? null : ButtonAddMoreItems.Command;
            }

            set
            {
                if (ButtonAddMoreItems != null)
                {
                    ButtonAddMoreItems.Command = value;
                }

                if (CloseButton != null)
                {
                    AC.ScheduleManaged(
                        async () =>
                        {
                            await Task.FromResult(0);

                            CloseButton.Command = AddMoreItemsCommand;
                        });
                }
            }
        }

        /// <summary>
        /// Margin for the bottom app bar.
        /// </summary>
        public virtual double BottomAppBarMargin { get; set; }

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
                            await Task.FromResult(0);

                            if (IsOverlayVisible)
                            {
                                IsOverlayVisible = false;

                                if (OverlaySeparator != null)
                                {
                                    OverlaySeparator.UpdateOpacity(0);
                                    OverlaySeparator.UpdateIsVisible(false);
                                }

                                if (CloseButton != null)
                                {
                                    CloseButton.UpdateOpacity(0);
                                    CloseButton.UpdateIsVisible(false);
                                }

                                if (HasVerticalTransition)
                                {
                                    await CartView.TranslateTo(0, LastSizePageCanvas.Height + (ContentMargin * 2), 500);
                                }
                                else
                                {
                                    await CartView.FadeTo(0, 500);
                                }

                                if (BackgroundWaterMark != null)
                                {
                                    await BackgroundWaterMark.FadeTo(0);
                                }

                                if (CartView.Opacity > 0)
                                {
                                    CartView.UpdateOpacity(0);
                                }

                                this.UpdateOpacity(0);

                                CartView.UpdateIsVisible(false);

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
        /// Show the items options.
        /// </summary>
        public ICommand ShowItemLargeDetailExternalCommand { get; set; }

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

                            await Task.FromResult(0);

                            if (!IsOverlayVisible)
                            {
                                IsOverlayVisible = true;

                                CartView.UpdateOpacity(0);
                                OverlaySeparator.UpdateIsVisible(true);

                                if (HasVerticalTransition)
                                {
                                    await CartView.TranslateTo(0, LastSizePageCanvas.Height + (ContentMargin * 2), 0);
                                }

                                if (HasVerticalTransition && CartView.Opacity < 1)
                                {
                                    CartView.UpdateOpacity(1);
                                }

                                if (BackgroundWaterMark != null)
                                {
                                    if (BackgroundWaterMark.Opacity > 0)
                                    {
                                        BackgroundWaterMark.UpdateOpacity(0);
                                    }

                                    BackgroundWaterMark.UpdateIsVisible(true);
                                }

                                if (CloseButton != null)
                                {
                                    if (CloseButton.Opacity > 0)
                                    {
                                        CloseButton.UpdateOpacity(0);
                                    }

                                    CloseButton.UpdateIsVisible(true);
                                }

                                this.UpdateIsVisible(true);
                                this.UpdateOpacity(1);

                                CartView.UpdateIsVisible(true);

                                if (BackgroundWaterMark != null)
                                {
                                    await BackgroundWaterMark.FadeTo(1);
                                }

                                if (HasVerticalTransition)
                                {
                                    await CartView.TranslateTo(0, 0, 500);
                                }
                                else
                                {
                                    await CartView.FadeTo(1, 500);
                                }

                                UpdateVisibleElements();

                                if (OverlaySeparator != null)
                                {
                                    OverlaySeparator.UpdateOpacity(1);
                                    OverlaySeparator.UpdateIsVisible(true);
                                }

                                if (CloseButton != null)
                                {
                                    await CloseButton.FadeTo(1);
                                }
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
        /// Viewmodel for the view.
        /// </summary>
        public CartListViewModel ViewModel
        {
            get
            {
                if (viewModel == null)
                {
                    viewModel = new CartListViewModel();
                }

                return viewModel;
            }
        }

        /// <summary>
        /// Background
        /// </summary>
        protected View BackgroundWaterMark { get; set; }

        /// <summary>
        /// Add more items button.
        /// </summary>
        protected ContentViewButton ButtonAddMoreItems { get; set; }

        /// <summary>
        /// Button confirm order.
        /// </summary>
        protected ContentViewButton ButtonConfirmOrder { get; set; }

        /// <summary>
        /// Cart view.
        /// </summary>
        protected View CartView { get; set; }

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
                return Device.RuntimePlatform.OnPlatform(true, true, false, false, false);
            }
        }

        /// <summary>
        /// Navigate to store level.
        /// </summary>
        protected Command<object> InternalShowItemLargeDetailCommand
        {
            get
            {
                if (internalShowItemLargeDetailCommand == null)
                {
                    internalShowItemLargeDetailCommand = new Command<object>(
                        async (selectedItem) =>
                        {
                            await Task.FromResult(0);

                            if (ShowItemLargeDetailExternalCommand != null)
                            {
                                ShowItemLargeDetailExternalCommand.ExecuteIfCan(selectedItem);
                            }
                        },
                        (selectedItem) =>
                        {
                            return selectedItem != null;
                        });
                }

                return internalShowItemLargeDetailCommand;
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
        /// Label for when there are no elements.
        /// </summary>
        protected ExtendedLabel LabelNoElements { get; set; }

        /// <summary>
        /// Last size.
        /// </summary>
        protected Rectangle LastSizePageCanvas { get; set; }

        /// <summary>
        /// Lock for the animtaion.
        /// </summary>
        protected SemaphoreSlim LockAnimation
        {
            get
            {
                if (lockAnimation == null)
                {
                    lockAnimation = new SemaphoreSlim(1);
                }

                return lockAnimation;
            }
        }

        /// <summary>
        /// Overlay separator.
        /// </summary>
        protected View OverlaySeparator { get; set; }

        /// <summary>
        /// Panel header request layout, used for the animations.
        /// </summary>
        protected StackLayout PanelHeaderRequestLayout { get; set; }

        /// <summary>
        /// Panel summary layout.
        /// </summary>
        protected View PanelSummaryLayout { get; set; }

        /// <summary>
        /// Close button.
        /// </summary>
        private ContentViewButton CloseButton { get; set; }

        /// <summary>
        /// Update visible elements.
        /// </summary>
        public void UpdateVisibleElements()
        {
            UpdateVisibleElements(ViewModel.TotalItemsCount);
        }

        /// <summary>
        /// Add controls to layout.
        /// </summary>
        protected override void AddControlsToLayout()
        {
            AddViewToLayout(BackgroundWaterMark);
            AddViewToLayout(CartView);
            AddViewToLayout(OverlaySeparator);
            AddViewToLayout(CloseButton);
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
            if (BackgroundWaterMark != null)
            {
                BackgroundWaterMark.LayoutUpdate(new Rectangle(x, y, width, height));
            }

            if (IsOverlay)
            {
                LastSizePageCanvas = new Rectangle(0, y + TopLayoutMargin, width, height - BottomAppBarMargin - TopLayoutMargin);
            }
            else
            {
                LastSizePageCanvas = new Rectangle(x, y, width, height);
            }

            if (CartView != null)
            {
                CartView.LayoutUpdate(LastSizePageCanvas);
            }

            if (CloseButton != null)
            {
                double elementHeight = Theme.CommonResources.RoundedButtonWidth;
                double elementLeft = width - Theme.CommonResources.RoundedButtonWidth - 2;
                double elementTop = y + ContentMargin + TopLayoutMargin - (elementHeight * 0.5f);

                CloseButton.LayoutUpdate(new Rectangle(elementLeft, elementTop, elementHeight, elementHeight));
            }

            if (OverlaySeparator != null)
            {
                double elementHeight = ContentMargin;
                double elementTop = y + ContentMargin + TopLayoutMargin - (elementHeight * 0.5f);

                OverlaySeparator.LayoutUpdate(new Rectangle(0, elementTop, width, elementHeight));
            }
        }

        /// <summary>
        /// Initialize elements.
        /// </summary>
        protected override void InternalInitializeView()
        {
            if (IsOverlay)
            {
                BackgroundWaterMark = Theme.RenderUtil.InstanceBackgroundDetail(
                        () =>
                        {
                            if (this.AddMoreItemsCommand != null)
                            {
                                this.AddMoreItemsCommand.ExecuteIfCan();
                            }
                        });
            }

            CartView = RenderContentRepeater();

            if (IsOverlay)
            {
                CartView.BackgroundColor = Theme.CommonResources.PagesBackgroundColor;
            }

            if (IsOverlay && HasControlBar)
            {
                OverlaySeparator = Theme.RenderUtil.InstaceLineSeparator(ContentMargin);

                OverlaySeparator.UpdateOpacity(0);
                OverlaySeparator.UpdateIsVisible(false);
                                
                CloseButton = new GlyphOnlyContentViewButton(hasBackground: true)
                {
                    Style = Theme.ApplicationStyles.GlyphOnlyRoundedContentButtonStyle,
                    GlyphText = Theme.CommonResources.GlyphTextCancel,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Command = AddMoreItemsCommand,
                    Opacity = 0,
                    IsVisible = false
                };
            }
        }

        /// <summary>
        /// Render the page layout.
        /// </summary>
        /// <returns>View to use.</returns>
        protected View RenderContentRepeater()
        {
            var completeLayout = new StackLayout()
            {
                Style = Theme.ApplicationStyles.FormRowContainerStyle,
                Orientation = StackOrientation.Vertical
            };

            if (IsOverlay)
            {
                Theme.RenderUtil.RenderSpace(completeLayout, heightRequest: ContentMargin);
            }

            var initialLayout = new StackLayout()
            {
                Style = Theme.ApplicationStyles.FormRowContainerStyle,
                Orientation = StackOrientation.Vertical
            };

            var generalLayout = new StackLayout()
            {
                Style = Theme.ApplicationStyles.FormRowContainerStyle,
                Orientation = StackOrientation.Vertical
            };

            var panelHeaderButtonsLayout = new StackLayout()
            {
                Style = Theme.ApplicationStyles.FormRowContainerStyle,
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Center,
                Opacity = 1
            };

            panelHeaderButtonsLayout.Children.Add(new BoxView
            {
                Color = Color.Transparent,
                MinimumHeightRequest = 5,
                MinimumWidthRequest = 1,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.FillAndExpand
            });

            ButtonConfirmOrder = new GlyphContentViewButton(true, true, ImageOrientation.ImageToLeft, hasBackground: true)
            {
                Style = Theme.ApplicationStyles.TextWithGlyphImportantLargeContentButtonStyle,
                Command = ViewModel.PlaceOrderCommand,
                Text = App.LocalizationResources.PlaceOrderButton,
                GlyphText = Theme.CommonResources.GlyphTextSelect,
                Opacity = 0,
                IsVisibleContent = false,
                ButtonBackgroundColor = Theme.CommonResources.ConfirmOrderColor,
                TextColor = Theme.CommonResources.TextColorSection,
                GlyphTextColor = Theme.CommonResources.TextColorSection,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start
            };

            ButtonConfirmOrder.SetBinding(ContentViewButton.CommandParameterProperty, "Items");

            panelHeaderButtonsLayout.Children.Add(ButtonConfirmOrder);

            Theme.RenderUtil.RenderSpace(PanelHeaderRequestLayout, 5, 5);

            ButtonAddMoreItems = new GlyphContentViewButton(true, true, ImageOrientation.ImageToLeft, hasBackground: true)
            {
                Style = Theme.ApplicationStyles.TextWithGlyphImportantLargeContentButtonStyle,
                Text = App.LocalizationResources.AddMoreProductsAction,
                GlyphText = Theme.CommonResources.GlyphTextAdd,
                ButtonBackgroundColor = Theme.CommonResources.AccentAlternative,
                TextColor = Theme.CommonResources.TextColorSection,
                GlyphTextColor = Theme.CommonResources.TextColorSection,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start
            };

            panelHeaderButtonsLayout.Children.Add(ButtonAddMoreItems);

            panelHeaderButtonsLayout.Children.Add(new BoxView
            {
                Color = Color.Transparent,
                MinimumHeightRequest = 5,
                MinimumWidthRequest = 1,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.FillAndExpand
            });

            initialLayout.Children.Add(panelHeaderButtonsLayout);

            Theme.RenderUtil.RenderSpace(initialLayout);

            LabelNoElements = new ExtendedLabel()
            {
                Style = Theme.ApplicationStyles.DescriptionExtendedLabelStyle,
                Opacity = 0,
                Text = ViewModel.LocalizationResources.ShoppingCartEmpyText,
                HorizontalOptions = LayoutOptions.Center
            };

            initialLayout.Children.Add(LabelNoElements);

            PanelHeaderRequestLayout = new StackLayout()
            {
                Style = Theme.ApplicationStyles.FormRowContainerStyle,
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Opacity = 0
            };

            generalLayout.Children.Add(PanelHeaderRequestLayout);

            PanelSummaryLayout = generalLayout;
            PanelSummaryLayout.UpdateOpacity(0);

            var detailLayout = Theme.RenderUtil.RenderIdentedVerticalContainer(generalLayout);

            var repeaterStoreItemsLevels = new RepeaterView<StoreItemCartViewModel>()
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Start,
                Spacing = 5,
                ItemTemplate = new DataTemplate(
                    () =>
                    {
                        var cell = new CartRowView();

                        cell.PrepareBindings();

                        return cell;
                    })
            };

            repeaterStoreItemsLevels.SetBinding(RepeaterView<StoreItemCartViewModel>.ItemsSourceProperty, "Items");
            detailLayout.Children.Add(repeaterStoreItemsLevels);

            var totalLayout = Theme.RenderUtil.RenderIdentedVerticalContainer(generalLayout); ;
            totalLayout.Spacing = 0;

            // Line summary.
            var lineSeparator1 = Theme.RenderUtil.InstaceLineSeparator();

            lineSeparator1.WidthRequest = 300;
            lineSeparator1.HorizontalOptions = LayoutOptions.End;

            totalLayout.Children.Add(lineSeparator1);

            Theme.RenderUtil.RenderSpace(totalLayout);

            completeLayout.Children.Add(initialLayout);
            completeLayout.Children.Add(generalLayout);

            ScrollView scrollViewItems = new ScrollView()
            {
                Orientation = ScrollOrientation.Vertical,
                Padding = 0,
                MinimumHeightRequest = 80,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Content = completeLayout
            };

            return scrollViewItems;
        }

        /// <summary>
        /// Setup bindings.
        /// </summary>
        protected override void SetupBindings()
        {
            // Nothing else to set.
        }

        /// <summary>
        /// Update visible items.
        /// </summary>
        /// <param name="totalItemsCount"></param>
        protected void UpdateVisibleElements(int totalItemsCount)
        {
            AC.ScheduleManaged(
                () =>
                {
                    if (LabelNoElements != null)
                    {
                        LabelNoElements.UpdateIsVisible((totalItemsCount == 0));
                        LabelNoElements.UpdateOpacity(LabelNoElements.IsVisible ? 1 : 0);
                    }

                    return Task.FromResult(0);
                });

            AC.ScheduleManaged(
                async () =>
                {
                    try
                    {
                        uint fadeTime = 500;

                        await LockAnimation.WaitAsync();

                        if (this.Content != null)
                        {
                            if (PanelSummaryLayout != null)
                            {
                                if (totalItemsCount == 0)
                                {
                                    if (PanelSummaryLayout.Opacity > 0)
                                    {
                                        await PanelSummaryLayout.FadeTo(0, fadeTime, Easing.SinIn);
                                    }
                                }
                                else
                                {
                                    if (PanelSummaryLayout.Opacity < 1)
                                    {
                                        await PanelSummaryLayout.FadeTo(1, fadeTime, Easing.SinIn);
                                    }
                                }
                            }

                            if (PanelHeaderRequestLayout != null)
                            {
                                bool showHeader = ViewModel.IsAuthenticated;

                                if (showHeader)
                                {
                                    if (totalItemsCount == 0)
                                    {
                                        if (PanelHeaderRequestLayout.Opacity > 0)
                                        {
                                            await PanelHeaderRequestLayout.FadeTo(0, fadeTime, Easing.SinIn);
                                        }
                                    }
                                    else
                                    {
                                        if (PanelHeaderRequestLayout.Opacity < 1)
                                        {
                                            await PanelHeaderRequestLayout.FadeTo(1, fadeTime, Easing.SinIn);
                                        }
                                    }
                                }
                                else
                                {
                                    if (PanelHeaderRequestLayout.Opacity > 0)
                                    {
                                        await PanelHeaderRequestLayout.FadeTo(0, fadeTime, Easing.SinIn);
                                    }
                                }

                                PanelHeaderRequestLayout.UpdateIsVisible(showHeader);
                            }

                            if (ButtonConfirmOrder != null)
                            {
                                bool shouldBeVisible = totalItemsCount > 0;

                                if (shouldBeVisible)
                                {
                                    if (ButtonConfirmOrder.Opacity < 1)
                                    {
                                        await ButtonConfirmOrder.FadeTo(1, fadeTime, Easing.SinIn);
                                    }
                                }
                                else
                                {
                                    if (ButtonConfirmOrder.Opacity > 0)
                                    {
                                        await ButtonConfirmOrder.FadeTo(0, fadeTime, Easing.SinIn);
                                    }
                                }

                                ButtonConfirmOrder.IsVisibleContent = shouldBeVisible;
                            }
                        }
                    }
                    finally
                    {
                        LockAnimation.Release();
                    }
                });
        }

        /// <summary>
        /// Cart items loaded.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Arguments of the event.</param>
        private async void LoadItemsCommand_CommandCompleted(object sender, IEnumerable<StoreItemCartViewModel> e)
        {
            int totalItemsCount = await e.CountAsync();

            UpdateVisibleElements(totalItemsCount);
        }
    }
}