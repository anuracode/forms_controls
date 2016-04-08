// <copyright file="StoreItemLevelTopPreviewView.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Sample.ViewModels;
using Anuracode.Forms.Controls.Extensions;
using Anuracode.Forms.Controls.Views.Extensions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls.Sample.Views
{
    /// <summary>
    /// View for level products.
    /// </summary>
    public class StoreItemLevelTopPreviewView : SimpleViewBase
    {
        /// <summary>
        /// Command to navigate.
        /// </summary>
        public static readonly BindableProperty NavigateToStoreLevelCommandProperty = BindableProperty.Create<StoreItemLevelTopPreviewView, ICommand>(
           p => p.NavigateToStoreLevelCommand,
           (ICommand)null,
           BindingMode.OneWay,
           (BindableProperty.ValidateValueDelegate<ICommand>)null,
           (BindableProperty.BindingPropertyChangedDelegate<ICommand>)(
           (bindable, oldvalue, newvalue) =>
           {
               if (newvalue != null)
               {
                   StoreItemLevelTopPreviewView controlInstance = bindable as StoreItemLevelTopPreviewView;
                   if (controlInstance != null)
                   {
                       if (controlInstance.ButtonCategory != null)
                       {
                           controlInstance.ButtonCategory.Command = newvalue;
                       }

                       if (controlInstance.ButtonExpandCategory != null)
                       {
                           controlInstance.ButtonExpandCategory.Command = newvalue;
                       }
                   }
               }
           }),
           (BindableProperty.BindingPropertyChangingDelegate<ICommand>)null,
           (BindableProperty.CoerceValueDelegate<ICommand>)null);

        /// <summary>
        /// Command to navigate.
        /// </summary>
        public static readonly BindableProperty NavigateToStoreLevelProductsCommandProperty = BindableProperty.Create<StoreItemLevelTopPreviewView, ICommand>(
           p => p.NavigateToStoreLevelProductsCommand,
           (ICommand)null,
           BindingMode.OneWay,
           (BindableProperty.ValidateValueDelegate<ICommand>)null,
           (BindableProperty.BindingPropertyChangedDelegate<ICommand>)(
           (bindable, oldvalue, newvalue) =>
           {
               if (newvalue != null)
               {
                   StoreItemLevelTopPreviewView controlInstance = bindable as StoreItemLevelTopPreviewView;
                   if (controlInstance != null && controlInstance.ButtonAllProducts != null)
                   {
                       controlInstance.ButtonAllProducts.Command = newvalue;
                   }
               }
           }),
           (BindableProperty.BindingPropertyChangingDelegate<ICommand>)null,
           (BindableProperty.CoerceValueDelegate<ICommand>)null);

        /// <summary>
        /// Internal item detail command.
        /// </summary>
        private Command<object> internalItemDetailCommand;

        /// <summary>
        /// Item height.
        /// </summary>
        private double itemHeight = 110;

        /// <summary>
        /// Navigate to store item viewmodel.
        /// </summary>
        private ICommand navigateToStoreItemViewModelCommand;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StoreItemLevelTopPreviewView()
            : this(true)
        {
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StoreItemLevelTopPreviewView(bool autoInit)
            : base(autoInit)
        {
            ContentLayout.IsClippedToBounds = false;
        }

        /// <summary>
        /// Internal item detail command.
        /// </summary>
        public Command<object> InternalItemDetailCommand
        {
            get
            {
                if (internalItemDetailCommand == null)
                {
                    internalItemDetailCommand = new Command<object>(
                        async (selectedItem) =>
                        {
                            await Task.FromResult(0);
                            NavigateToStoreItemViewModelCommand.Execute(selectedItem);
                        },
                        (selectedItem) =>
                        {
                            return (selectedItem is StoreItemViewModel) && (NavigateToStoreItemViewModelCommand != null) && NavigateToStoreItemViewModelCommand.CanExecute(selectedItem);
                        });
                }

                return internalItemDetailCommand;
            }
        }

        /// <summary>
        /// Navigate to store item viewmodel.
        /// </summary>
        public ICommand NavigateToStoreItemViewModelCommand
        {
            get
            {
                return navigateToStoreItemViewModelCommand;
            }
            set
            {
                if (navigateToStoreItemViewModelCommand != null)
                {
                    navigateToStoreItemViewModelCommand.CanExecuteChanged -= NavigateToStoreItemViewModelCommand_CanExecuteChanged;
                }

                navigateToStoreItemViewModelCommand = value;

                OnPropertyChanged("NavigateToStoreItemViewModelCommand");

                if (navigateToStoreItemViewModelCommand != null)
                {
                    navigateToStoreItemViewModelCommand.CanExecuteChanged += NavigateToStoreItemViewModelCommand_CanExecuteChanged;
                }
            }
        }

        /// <summary>
        /// Command to navigate.
        /// </summary>
        public ICommand NavigateToStoreLevelCommand
        {
            get
            {
                return (ICommand)GetValue(NavigateToStoreLevelCommandProperty);
            }

            set
            {
                SetValue(NavigateToStoreLevelCommandProperty, value);
            }
        }

        /// <summary>
        /// Command to navigate.
        /// </summary>
        public ICommand NavigateToStoreLevelProductsCommand
        {
            get
            {
                return (ICommand)GetValue(NavigateToStoreLevelProductsCommandProperty);
            }

            set
            {
                SetValue(NavigateToStoreLevelProductsCommandProperty, value);
            }
        }

        /// <summary>
        /// Activity indicator.
        /// </summary>
        protected ActivityIndicator ActivityView { get; set; }

        /// <summary>
        /// Button all products.
        /// </summary>
        protected ContentViewButton ButtonAllProducts { get; set; }

        /// <summary>
        /// Button category.
        /// </summary>
        protected TextContentViewButton ButtonCategory { get; set; }

        /// <summary>
        /// Button expand category.
        /// </summary>
        protected ContentViewButton ButtonExpandCategory { get; set; }

        /// <summary>
        /// Category background.
        /// </summary>
        protected ShapeView CategoryShapeBackgroundView { get; set; }

        /// <summary>
        /// Grid view for the products.
        /// </summary>
        protected RepeaterRecycleView GridViewProducts { get; set; }

        /// <summary>
        /// Repeater for the items.
        /// </summary>
        protected RepeaterView<StoreItemViewModel> RepeaterTopItems { get; set; }

        /// <summary>
        /// Scroll for the products.
        /// </summary>
        protected ScrollView ScrollProducts { get; set; }

        /// <summary>
        /// Flag to use grid or repeater.
        /// </summary>
        protected bool UseGridView
        {
            get
            {
                return Device.OS.OnPlatform(true, true, true, true, true);
            }
        }

        /// <summary>
        /// Set up cell values.
        /// </summary>
        /// <param name="isRecycled">Is recycled.</param>
        public override void SetupViewValues(bool isRecycled)
        {
            base.SetupViewValues(isRecycled);

            if (GridViewProducts != null)
            {
                StoreItemLevelViewModel viewModel = BindingContext as StoreItemLevelViewModel;

                if (viewModel != null)
                {
                    GridViewProducts.ItemsSource = viewModel.TopItems;

                    viewModel.LoadItemsCommand.ExecuteIfCan();
                }
            }
        }

        /// <summary>
        /// Add the control to the layout in the proper order.
        /// </summary>
        protected override void AddControlsToLayout()
        {
            AddViewToLayout(CategoryShapeBackgroundView);
            AddViewToLayout(ButtonCategory);
            AddViewToLayout(ButtonExpandCategory);
            AddViewToLayout(ButtonAllProducts);
            AddViewToLayout(GridViewProducts);
            AddViewToLayout(ScrollProducts);
            AddViewToLayout(ActivityView);
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
            Rectangle categoryButtonPosition = new Rectangle();
            Rectangle categoryExpandButtonPosition = new Rectangle();

            if (ButtonCategory != null)
            {
                var elementSize = ButtonCategory.Measure(width, height).Request;
                double elementLeft = ContentMargin;
                double elementTop = ContentMargin;
                double elementHeight = elementSize.Height;
                double elementWidth = elementSize.Width;

                categoryButtonPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                ButtonCategory.LayoutUpdate(categoryButtonPosition);
            }

            if (ButtonAllProducts != null)
            {
                var elementSize = ButtonAllProducts.Measure(width, height).Request;
                double elementLeft = Width - elementSize.Width - ContentMargin;
                double elementTop = ContentMargin;
                double elementHeight = elementSize.Height;
                double elementWidth = elementSize.Width;

                var elementPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                ButtonAllProducts.LayoutUpdate(elementPosition);
            }

            double itemsElementLeft = ContentMargin;
            double itemsElementTop = categoryButtonPosition.Y + categoryButtonPosition.Height + (ContentMargin * (1f));
            double itemsElementWidth = width - (ContentMargin * 2f);
            double itemsElementHeight = itemHeight;
            Rectangle itemsPosition = new Rectangle(itemsElementLeft, itemsElementTop, itemsElementWidth, itemsElementHeight);

            if (GridViewProducts != null)
            {
                GridViewProducts.LayoutUpdate(itemsPosition);
            }

            if (ScrollProducts != null)
            {
                ScrollProducts.LayoutUpdate(itemsPosition);
            }

            if (ActivityView != null)
            {
                var elementSize = ActivityView.Measure(itemsPosition.Width * 0.5f, itemsPosition.Height * 0.5f).Request;
                double elementLeft = ((itemsPosition.Width - elementSize.Width) * 0.5f) + itemsPosition.X;
                double elementTop = ((itemsPosition.Height - elementSize.Height) * 0.5f) + itemsPosition.Y;
                double elementHeight = elementSize.Height;
                double elementWidth = elementSize.Width;

                var elementPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                ActivityView.LayoutUpdate(elementPosition);
            }

            if (ButtonExpandCategory != null)
            {
                double elementLeft = categoryButtonPosition.X + categoryButtonPosition.Width + (ContentMargin * 0.5f);
                double elementTop = categoryButtonPosition.Y;
                double elementHeight = categoryButtonPosition.Height;
                double elementWidth = elementHeight;

                categoryExpandButtonPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                double imageWidth = elementWidth;

                if (ButtonExpandCategory.ImageHeightRequest != imageWidth)
                {
                    ButtonExpandCategory.ImageHeightRequest = imageWidth;
                }

                if (ButtonExpandCategory.ImageWidthRequest != imageWidth)
                {
                    ButtonExpandCategory.ImageWidthRequest = imageWidth;
                }

                if (ButtonExpandCategory.MinimumHeightRequest != elementHeight)
                {
                    ButtonExpandCategory.MinimumHeightRequest = elementHeight;
                }

                if (ButtonExpandCategory.MinimumWidthRequest != elementWidth)
                {
                    ButtonExpandCategory.MinimumWidthRequest = elementWidth;
                }

                if (ButtonExpandCategory.HeightRequest != elementHeight)
                {
                    ButtonExpandCategory.HeightRequest = elementHeight;
                }

                if (ButtonExpandCategory.WidthRequest != elementWidth)
                {
                    ButtonExpandCategory.WidthRequest = elementWidth;
                }

                ButtonExpandCategory.LayoutUpdate(categoryExpandButtonPosition);
            }

            if (CategoryShapeBackgroundView != null)
            {
                double elementLeft = categoryButtonPosition.X - (ContentMargin * 1f);
                double elementTop = categoryButtonPosition.Y - (ContentMargin * 0.5f);
                double elementHeight = categoryButtonPosition.Height + (ContentMargin * 1f);
                double elementWidth = categoryButtonPosition.Width + categoryExpandButtonPosition.Width + (ContentMargin * 1.5f);

                var elementPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                CategoryShapeBackgroundView.LayoutUpdate(elementPosition);
            }
        }

        /// <summary>
        /// Mesure the content.
        /// </summary>
        /// <param name="widthConstraint">Width to constarint.</param>
        /// <param name="heightConstraint">Height to constraint.</param>
        /// <returns>Size to use.</returns>
        protected override SizeRequest ContentLayout_OnSizeRequest(double widthConstraint, double heightConstraint)
        {
            Size categorySize = new Size();

            if (ButtonCategory != null)
            {
                categorySize = ButtonCategory.Measure(widthConstraint, heightConstraint).Request;
            }

            double height = categorySize.Height + (ContentMargin * 2f) + itemHeight;
            SizeRequest resultRequest = new SizeRequest(new Size(widthConstraint, height), new Size(widthConstraint, height));

            return resultRequest;
        }

        /// <summary>
        /// Internal initialze view.
        /// </summary>
        protected override void InternalInitializeView()
        {
            // Expand button.
            ButtonExpandCategory = new GlyphOnlyContentViewButton()
            {
                Style = Theme.ApplicationStyles.GlyphOnlyRoundedUnfilledContentButtonStyle,
                GlyphText = Theme.CommonResources.GlyphTextAdd,
                StrokeColor = Theme.CommonResources.TextColorSection,
                GlyphTextColor = Theme.CommonResources.TextColorSection,
                StrokeWidth = 1,
                GlyphFontSize = Theme.CommonResources.TextSizeMicro * 0.7f,
                MarginBorders = 0,
                MarginElements = 0
            };

            // Category button.
            ButtonCategory = new TextContentViewButton()
            {
                Style = Theme.ApplicationStyles.TextOnlyContentButtonStyle,
                FontName = Theme.CommonResources.FontRobotBoldCondensedName,
                FriendlyFontName = Theme.CommonResources.FontRobotBoldCondensedFriendlyName,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start,
                MinimumWidthRequest = 200,
                TextColor = Theme.CommonResources.TextColorSection,
                FontSize = Theme.CommonResources.TextSizeSmall * 0.85f
            };

            // All productos button.
            ButtonAllProducts = new GlyphLeftContentViewButton()
            {
                Style = Theme.ApplicationStyles.TextOnlyContentButtonStyle,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start,
                MinimumWidthRequest = 120,
                Text = App.LocalizationResources.StoreItemLevelAllLabel,
                FontSize = Theme.CommonResources.TextSizeMicro,
                TextColor = Theme.CommonResources.AccentDark,
                GlyphTextColor = Theme.CommonResources.AccentDark,
                GlyphText = Theme.CommonResources.GlyphTextAll,
                MarginElements = 0,
                MarginBorders = 2
            };

            CategoryShapeBackgroundView = new ShapeView()
            {
                ShapeType =  ShapeType.Box,
                Color = Theme.CommonResources.Accent,
                CornerRadius = 15
            };

            if (UseGridView)
            {
                double borderWith = 4;
                double elementHeight = Theme.CommonResources.LineHeight;
                itemHeight = 100 + (borderWith * 2f);
                double itemWidth = 200 + (borderWith * 2f);

                var itemTemplate = new DataTemplate(
                        () =>
                        {
                            ContentViewButton detailItemButton = new ContentTemplateViewButton(
                               new DataTemplate(
                                   () =>
                                   {
                                       var cell = new StoreItemThumbVerticalBarView();
                                       cell.BindingContext = null;
                                       cell.PrepareBindings();
                                       return cell;
                                   }))
                            {
                                Command = InternalItemDetailCommand,
                                Style = Theme.ApplicationStyles.TextOnlyContentButtonStyle
                            };

                            detailItemButton.BindingContext = null;

                            detailItemButton.SetBinding(ContentViewButton.CommandParameterProperty, ".");

                            return detailItemButton;
                        });

                GridViewProducts = new RepeaterRecycleView(poolAheadItems: 3, showActivityIndicator: true)
                {
                    ItemHeight = itemHeight,
                    ItemWidth = itemWidth,
                    ItemTemplate = itemTemplate,
                    Spacing = 5,
                    InstanceOnSublevelLock = true,
                    CleanOnBindingChange = true
                };
            }
            else
            {
                bool hasVisibleButton = Device.OS.OnPlatform(true, false, false, false, true);

                DataTemplate itemTemplate = null;

                if (hasVisibleButton)
                {
                    itemTemplate = new DataTemplate(
                        () =>
                        {
                            ContentViewButton detailItemButton = new ContentTemplateViewButton(
                                new DataTemplate(
                                    () =>
                                    {
                                        var cell = new StoreItemThumbVerticalBarView();
                                        cell.PrepareBindings();
                                        return cell;
                                    }))
                            {
                                Command = InternalItemDetailCommand,
                                Style = Theme.ApplicationStyles.TextOnlyContentButtonStyle
                            };

                            detailItemButton.SetBinding(ContentViewButton.CommandParameterProperty, ".");

                            return detailItemButton;
                        });
                }
                else
                {
                    itemTemplate = new DataTemplate(
                        () =>
                        {
                            var cell = new StoreItemThumbVerticalBarView();
                            cell.PrepareBindings();
                            return cell;
                        });
                }

                // Scroll products.
                ScrollProducts = new ScrollView()
                {
                    Padding = 0,
                    Orientation = ScrollOrientation.Horizontal,
                    VerticalOptions = LayoutOptions.Start,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };

                RepeaterTopItems = new RepeaterView<StoreItemViewModel>()
                {
                    Orientation = StackOrientation.Horizontal,
                    Spacing = 5,
                    ItemTemplate = itemTemplate,
                };

                if (!hasVisibleButton)
                {
                    RepeaterTopItems.ItemClickCommand = InternalItemDetailCommand;
                }

                ScrollProducts.Content = RepeaterTopItems;

                ActivityView = new ActivityIndicator()
                {
                    Color = Theme.CommonResources.Accent
                };
            }
        }

        /// <summary>
        /// When binding context changes.
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (RepeaterTopItems != null)
            {
                StoreItemLevelViewModel viewModel = BindingContext as StoreItemLevelViewModel;

                if (viewModel != null)
                {
                    RepeaterTopItems.ItemsSource = viewModel.TopItems;

                    viewModel.LoadItemsCommand.ExecuteIfCan();
                }
            }
        }

        /// <summary>
        /// Setup the bindings of the elements.
        /// </summary>
        protected override void SetupBindings()
        {
            if (ButtonCategory != null)
            {
                ButtonCategory.SetBinding<StoreItemLevelViewModel>(ContentViewButton.TextProperty, vm => vm.Item, converter: Theme.CommonResources.StoreItemLevelToLowerLevelStringConverter);
                ButtonCategory.SetBinding<StoreItemLevelViewModel>(ContentViewButton.CommandParameterProperty, vm => vm.Item);
            }

            if (ButtonExpandCategory != null)
            {
                ButtonExpandCategory.SetBinding<StoreItemLevelViewModel>(ContentViewButton.TextProperty, vm => vm.Item, converter: Theme.CommonResources.StoreItemLevelToLowerLevelStringConverter);
                ButtonExpandCategory.SetBinding<StoreItemLevelViewModel>(ContentViewButton.CommandParameterProperty, vm => vm.Item);
            }

            if (ButtonAllProducts != null)
            {
                ButtonAllProducts.SetBinding<StoreItemLevelViewModel>(ContentViewButton.CommandParameterProperty, vm => vm.Item);
            }

            if (GridViewProducts != null)
            {
                if (ActivityView != null)
                {
                    if (ActivityView.BindingContext != GridViewProducts)
                    {
                        ActivityView.BindingContext = GridViewProducts;
                    }

                    ActivityView.SetBinding<RepeaterRecycleView>(View.IsVisibleProperty, vm => vm.IsLoading);
                    ActivityView.SetBinding<RepeaterRecycleView>(ActivityIndicator.IsRunningProperty, vm => vm.IsLoading);
                }
            }

            if (RepeaterTopItems != null)
            {
                if (ActivityView != null)
                {
                    if (ActivityView.BindingContext != RepeaterTopItems)
                    {
                        ActivityView.BindingContext = RepeaterTopItems;
                    }

                    ActivityView.SetBinding<RepeaterView<StoreItemViewModel>>(View.IsVisibleProperty, vm => vm.IsLoading);
                    ActivityView.SetBinding<RepeaterView<StoreItemViewModel>>(ActivityIndicator.IsRunningProperty, vm => vm.IsLoading);
                }
            }
        }

        /// <summary>
        /// Can execute changed.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Arguments of the event.</param>
        private void NavigateToStoreItemViewModelCommand_CanExecuteChanged(object sender, System.EventArgs e)
        {
            InternalItemDetailCommand.RaiseCanExecuteChanged();
        }
    }
}