﻿// <copyright file="CartSmallView.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Anuracode.Forms.Controls.Extensions;
using Anuracode.Forms.Controls.Sample.ViewModels;

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

                                if (Opacity > 0)
                                {
                                    Opacity = 0;
                                }

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

                                if (HasVerticalTransition)
                                {
                                    await CartView.TranslateTo(0, LastSizePageCanvas.Height + (ContentMargin * 2), 0);
                                }
                                else
                                {
                                    CartView.UpdateOpacity(0);
                                }

                                if (Opacity < 1)
                                {
                                    Opacity = 1;
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
                return Device.OS.OnPlatform(true, true, false, false, false);
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

                // Substract button.
                CloseButton = new GlyphOnlyContentViewButton()
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

            ButtonConfirmOrder = new GlyphContentViewButton(true, true, ImageOrientation.ImageToLeft)
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

            ButtonConfirmOrder.SetBinding<CartListViewModel>(ContentViewButton.CommandParameterProperty, vm => vm.Items);

            panelHeaderButtonsLayout.Children.Add(ButtonConfirmOrder);

            Theme.RenderUtil.RenderSpace(PanelHeaderRequestLayout, 5, 5);

            ButtonAddMoreItems = new GlyphContentViewButton(true, true, ImageOrientation.ImageToLeft)
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

            StackLayout panelCartLayout = new StackLayout()
            {
                Style = Theme.ApplicationStyles.FormRowContainerStyle,
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            StackLayout panelCartLine1 = new StackLayout()
            {
                Style = Theme.ApplicationStyles.FormRowContainerStyle,
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Center
            };

            panelCartLayout.Children.Add(panelCartLine1);

            StackLayout panelCartLine2 = new StackLayout()
            {
                Style = Theme.ApplicationStyles.FormRowContainerStyle,
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Center
            };

            panelCartLayout.Children.Add(panelCartLine2);

            StackLayout panelCartLine3 = new StackLayout()
            {
                Style = Theme.ApplicationStyles.FormRowContainerStyle,
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Center
            };

            panelCartLayout.Children.Add(panelCartLine3);

            // Start frase.
            ExtendedLabel labelRequest = new ExtendedLabel()
            {
                Style = Theme.ApplicationStyles.DetailNameExtendedLabelStyle,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start
            };

            labelRequest.Text = App.LocalizationResources.CartConnector1Text;

            panelCartLine1.Children.Add(labelRequest);

            int controlRequestHeight = 25;

            // shipping type.
            var buttonShippingType = new TextContentViewButton()
            {
                MarginBorders = 0,
                MarginElements = 0,                
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start,
                MinimumWidthRequest = 150,
                TextColor = Theme.CommonResources.Accent,
                TextColorDisabled = labelRequest.TextColor,
                FontName = labelRequest.FontName,
                FontSize = labelRequest.FontSize,
                FriendlyFontName = labelRequest.FriendlyFontName,
                HeightRequest = controlRequestHeight,
                IsUnderline = true,
                Text = App.LocalizationResources.ShippingTypeValueLabel
            };            

            panelCartLine1.Children.Add(buttonShippingType);

            // Connector 1
            ExtendedLabel labelConnector1 = new ExtendedLabel()
            {
                Style = labelRequest.Style,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start,
            };

            labelConnector1.Text = App.LocalizationResources.CartConnector2Text;

            panelCartLine2.Children.Add(labelConnector1);

            // Address
            var buttonAddress = new TextContentViewButton()
            {
                MarginBorders = 0,
                MarginElements = 0,                
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start,
                MinimumWidthRequest = 150,
                TextColor = Theme.CommonResources.Accent,
                TextColorDisabled = labelRequest.TextColor,
                FontName = labelRequest.FontName,
                FontSize = labelRequest.FontSize,
                FriendlyFontName = labelRequest.FriendlyFontName,
                HeightRequest = controlRequestHeight,
                IsUnderline = true,
                Text = "No address"
            };            

            panelCartLine2.Children.Add(buttonAddress);

            // Connector 2
            ExtendedLabel labelConnector2 = new ExtendedLabel()
            {
                Style = labelRequest.Style,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start,
            };

            labelConnector2.SetBinding<CartListViewModel>(ExtendedLabel.TextProperty, vm => vm.TotalItemsCart);

            panelCartLine2.Children.Add(labelConnector2);

            // Connector 3
            ExtendedLabel labelConnector3 = new ExtendedLabel()
            {
                Style = labelRequest.Style,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start,
                Text = "$ 0"
            };            

            panelCartLine3.Children.Add(labelConnector3);

            // Payment method.
            var buttonPaymentMethod = new TextContentViewButton()
            {
                MarginBorders = 0,
                MarginElements = 0,                
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start,
                MinimumWidthRequest = 150,
                TextColor = Theme.CommonResources.Accent,
                TextColorDisabled = labelRequest.TextColor,
                FontName = labelRequest.FontName,
                FontSize = labelRequest.FontSize,
                FriendlyFontName = labelRequest.FriendlyFontName,
                HeightRequest = controlRequestHeight,
                IsUnderline = true,
                Text = App.LocalizationResources.PaymentTypeValueLabel                
            };            

            panelCartLine3.Children.Add(buttonPaymentMethod);

            PanelHeaderRequestLayout.Children.Add(panelCartLayout);

            Theme.RenderUtil.RenderSpace(PanelHeaderRequestLayout, 10, 10);

            generalLayout.Children.Add(PanelHeaderRequestLayout);

            PanelSummaryLayout = generalLayout;
            PanelSummaryLayout.UpdateOpacity(0);

            Theme.RenderUtil.RenderSectionTitle<CartListViewModel>(generalLayout, vm => vm.LocalizationResources.DetailLabel);

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
                        var cell = new CartRowView()
                        {
                            DeleteCommand = ViewModel.DeleteItemCommand,
                            DetailCommand = InternalShowItemLargeDetailCommand
                        };

                        cell.PrepareBindings();

                        return cell;
                    })
            };

            repeaterStoreItemsLevels.SetBinding<CartListViewModel>(RepeaterView<StoreItemCartViewModel>.ItemsSourceProperty, vm => vm.Items);
            detailLayout.Children.Add(repeaterStoreItemsLevels);

            var totalLayout = Theme.RenderUtil.RenderIdentedVerticalContainer(generalLayout); ;
            totalLayout.Spacing = 0;

            // Line summary.
            var lineSeparator1 = Theme.RenderUtil.InstaceLineSeparator();

            lineSeparator1.WidthRequest = 300;
            lineSeparator1.HorizontalOptions = LayoutOptions.End;

            totalLayout.Children.Add(lineSeparator1);

            Theme.RenderUtil.RenderSpace(totalLayout);           

            // Separator
            Theme.RenderUtil.RenderSpace(totalLayout, 10, 10);

            // Render section title.
            Theme.RenderUtil.RenderSectionTitle<CartListViewModel>(generalLayout, vm => vm.LocalizationResources.CartAdjustmentsLabel, vm => vm.IsAuthenticated);

            StackLayout optionsLayout = Theme.RenderUtil.RenderIdentedVerticalContainer(generalLayout);
            optionsLayout.SetBinding<CartListViewModel>(StackLayout.IsVisibleProperty, vm => vm.IsAuthenticated);

            // Layout shipping type.
            StackLayout panelPaymentTypeLayout = new StackLayout()
            {
                Style = Theme.ApplicationStyles.SummaryRowContainerStyle,
                Orientation = StackOrientation.Horizontal
            };

            // Label payment type.
            Label labelPaymentType = new ExtendedLabel()
            {
                Style = Theme.ApplicationStyles.DetailNameExtendedLabelStyle,
                HorizontalOptions = LayoutOptions.Start
            };

            labelPaymentType.SetBinding<CartListViewModel>(Label.TextProperty, vm => vm.LocalizationResources.PaymentTypeLabel, stringFormat: "{0}: ");
            panelPaymentTypeLayout.Children.Add(labelPaymentType);

            panelPaymentTypeLayout.Children.Add(new BoxView { Color = Color.Transparent, WidthRequest = 5 });

            // PaymentType value
            Label labelPaymentTypeValue = new ExtendedLabel()
            {
                Style = Theme.ApplicationStyles.DetailValueExtendedLabelStyle,
                HorizontalOptions = LayoutOptions.Start
            };

            labelPaymentTypeValue.SetBinding<CartListViewModel>(Label.TextProperty, vm => vm.LocalizationResources.PaymentTypeValueLabel);
            panelPaymentTypeLayout.Children.Add(labelPaymentTypeValue);

            // Edit payment type.
            ContentViewButton buttonEditPaymentType = new GlyphOnlyContentViewButton()
            {
                Style = Theme.ApplicationStyles.GlyphOnlyRoundedContentButtonStyle,
                GlyphText = Theme.CommonResources.GlyphTextEdit,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start,                
                InvisibleWhenDisabled = true
            };

            panelPaymentTypeLayout.Children.Add(buttonEditPaymentType);

            optionsLayout.Children.Add(panelPaymentTypeLayout);

            // Layout shipping type.
            StackLayout panelShippingTypeLayout = new StackLayout()
            {
                Style = Theme.ApplicationStyles.SummaryRowContainerStyle,
                Orientation = StackOrientation.Horizontal
            };

            // Label shipping type.
            Label labelShippingType = new ExtendedLabel()
            {
                Style = Theme.ApplicationStyles.DetailNameExtendedLabelStyle,
                HorizontalOptions = LayoutOptions.Start,
            };

            labelShippingType.SetBinding<CartListViewModel>(Label.TextProperty, vm => vm.LocalizationResources.ShippingTypeLabel, stringFormat: "{0}: ");
            panelShippingTypeLayout.Children.Add(labelShippingType);

            panelShippingTypeLayout.Children.Add(new BoxView { Color = Color.Transparent, WidthRequest = 5 });

            // ShippingType value
            Label labelShippingTypeValue = new ExtendedLabel()
            {
                Style = Theme.ApplicationStyles.DetailValueExtendedLabelStyle,
                HorizontalOptions = LayoutOptions.Start
            };

            labelShippingTypeValue.SetBinding<CartListViewModel>(Label.TextProperty, vm => vm.LocalizationResources.ShippingTypeValueLabel);
            panelShippingTypeLayout.Children.Add(labelShippingTypeValue);

            panelShippingTypeLayout.Children.Add(new BoxView { Color = Color.Transparent, WidthRequest = 5 });

            // Edit shipping type.
            var buttonEditShippingType = new GlyphOnlyContentViewButton()
            {
                Style = Theme.ApplicationStyles.GlyphOnlyRoundedContentButtonStyle,
                GlyphText = Theme.CommonResources.GlyphTextEdit,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,                
                BindingContext = this,
                InvisibleWhenDisabled = true
            };

            panelShippingTypeLayout.Children.Add(buttonEditShippingType);

            optionsLayout.Children.Add(panelShippingTypeLayout);

            // Layout address.
            StackLayout panelAddressLayout = new StackLayout()
            {
                Style = Theme.ApplicationStyles.SummaryRowContainerStyle,
                Orientation = StackOrientation.Horizontal
            };

            panelAddressLayout.SetBinding<CartListViewModel>(StackLayout.IsVisibleProperty, vm => vm.IsAuthenticated);

            // Address
            Label labelAddress = new ExtendedLabel()
            {
                Style = Theme.ApplicationStyles.DetailNameExtendedLabelStyle,
                HorizontalOptions = LayoutOptions.Start
            };

            labelAddress.SetBinding<CartListViewModel>(Label.TextProperty, vm => vm.LocalizationResources.AddressLabel, stringFormat: "{0}: ");
            panelAddressLayout.Children.Add(labelAddress);

            panelAddressLayout.Children.Add(new BoxView { Color = Color.Transparent, WidthRequest = 5 });

            // Address value
            Label labelAddressValue = new ExtendedLabel()
            {
                Style = Theme.ApplicationStyles.DetailValueExtendedLabelStyle,
                HorizontalOptions = LayoutOptions.Start,
                Text = "No address"
            };
                        
            panelAddressLayout.Children.Add(labelAddressValue);

            panelAddressLayout.Children.Add(new BoxView { Color = Color.Transparent, WidthRequest = 5 });

            // Edit address.
            ContentViewButton buttonEditAddress = new GlyphOnlyContentViewButton()
            {
                Style = Theme.ApplicationStyles.GlyphOnlyRoundedContentButtonStyle,
                GlyphText = Theme.CommonResources.GlyphTextEdit,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,                
                InvisibleWhenDisabled = true
            };

            panelAddressLayout.Children.Add(buttonEditAddress);
            

            optionsLayout.Children.Add(panelAddressLayout);

            StackLayout coverageLayout = Theme.RenderUtil.RenderIdentedVerticalContainer(generalLayout);            

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
                        LabelNoElements.IsVisible = (totalItemsCount == 0);
                        LabelNoElements.Opacity = LabelNoElements.IsVisible ? 1 : 0;
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

                                PanelHeaderRequestLayout.IsVisible = showHeader;
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