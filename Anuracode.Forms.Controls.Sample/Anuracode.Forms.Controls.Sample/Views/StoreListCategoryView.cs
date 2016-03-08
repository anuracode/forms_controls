// <copyright file="StoreListCategoryView.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Anuracode.Forms.Controls.Extensions;
using Anuracode.Forms.Controls.Sample.Interfaces;
using Anuracode.Forms.Controls.Sample.ViewModels;

namespace Anuracode.Forms.Controls.Sample.Views
{
    /// <summary>
    /// View for the categories.
    /// </summary>
    public class StoreListCategoryView : SimpleViewBase
    {
        /// <summary>
        /// Navigate back depending on the view.
        /// </summary>
        private Command internalNavigateBackCommand;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="storeListView">Page to use.</param>
        public StoreListCategoryView(IStoreListPage storeListView)
            : base(false)
        {
            if (storeListView == null)
            {
                throw new ArgumentNullException("storeListView");
            }

            StoreListView = storeListView;

            ContentLayout.IsClippedToBounds = false;
        }

        /// <summary>
        /// Content margin.
        /// </summary>
        public double ContentMargin { get; set; }

        /// <summary>
        /// Nacigate back.
        /// </summary>
        public Command NavigateBackCommand { get; set; }

        /// <summary>
        /// Flag to use grid.
        /// </summary>
        public bool UseGrid
        {
            get
            {
                return Device.OS.OnPlatform(true, true, true, true, true);
            }
        }

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
        /// Level view.
        /// </summary>
        protected StoreItemLevelSimpleView LevelView { get; set; }

        /// <summary>
        /// Repeater to use.
        /// </summary>
        protected RepeaterView<StoreItemLevelViewModel> RepeaterStoreItemsLevels { get; set; }

        /// <summary>
        /// Scroll view to use.
        /// </summary>
        protected ScrollView ScrollViewStoreItems { get; set; }

        /// <summary>
        /// All products button.
        /// </summary>
        protected GlyphContentViewButton StoreAllProductsButton { get; set; }

        /// <summary>
        /// View to use.
        /// </summary>
        protected IStoreListPage StoreListView { get; set; }

        /// <summary>
        /// Grid products.
        /// </summary>
        private RepeaterRecycleView GridViewProducts { get; set; }

        /// <summary>
        /// Setup bindings.
        /// </summary>
        protected override void SetupBindings()
        {
            if (GridViewProducts != null)
            {
                DataTemplate itemTemplate = new DataTemplate(
                   () =>
                   {
                       var cell = new StoreItemLevelTopPreviewView()
                       {
                           NavigateToStoreLevelCommand = StoreListView.NavigateToStoreLevelCommand,
                           NavigateToStoreLevelProductsCommand = StoreListView.NavigateToStoreLevelProductsCommand,
                           NavigateToStoreItemViewModelCommand = StoreListView.ShowItemOptionsCommand
                       };

                       cell.BindingContext = null;

                       return cell;
                   });

                GridViewProducts.ItemTemplate = itemTemplate;
                GridViewProducts.ItemsSource = StoreListView.StoreListViewModel.SublevelsViewModels;
            }

            if (StoreAllProductsButton != null)
            {
                StoreAllProductsButton.SetBinding<StoreListViewModel>(ContentViewButton.CommandParameterProperty, vm => vm.Level);
                StoreAllProductsButton.Command = StoreListView.NavigateToStoreLevelProductsCommand;
            }

            if (RepeaterStoreItemsLevels != null)
            {
                var itemTemplate = new DataTemplate(
                       () =>
                       {
                           var cell = new StoreItemLevelTopPreviewView()
                           {
                               NavigateToStoreLevelCommand = StoreListView.NavigateToStoreLevelCommand,
                               NavigateToStoreLevelProductsCommand = StoreListView.NavigateToStoreLevelProductsCommand,
                               NavigateToStoreItemViewModelCommand = StoreListView.ShowItemOptionsCommand
                           };

                           cell.PrepareBindings();

                           return cell;
                       });

                RepeaterStoreItemsLevels.ItemTemplate = itemTemplate;
                RepeaterStoreItemsLevels.ItemsSource = StoreListView.StoreListViewModel.SublevelsViewModels;
            }

            if (LevelView != null)
            {
                LevelView.PrepareBindings();
                LevelView.SetBinding<StoreListViewModel>(View.BindingContextProperty, vm => vm.Level);
                LevelView.NavigateBackCommand = InternalNavigateBackCommand;
                LevelView.NavigateToStoreLevelCommand = StoreListView.NavigateToStoreLevelCommand;
            }
        }

        /// <summary>
        /// Add control to the layout.
        /// </summary>
        protected override void AddControlsToLayout()
        {
            AddViewToLayout(GridViewProducts);
            AddViewToLayout(ScrollViewStoreItems);
            AddViewToLayout(LevelView);
            AddViewToLayout(StoreAllProductsButton);
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
            Rectangle levelViewPosition = new Rectangle();

            bool hasLevel = (StoreListView != null) && (StoreListView.StoreListViewModel != null) && (StoreListView.StoreListViewModel.Level != null) && !string.IsNullOrWhiteSpace(StoreListView.StoreListViewModel.Level.Department);

            if (LevelView != null)
            {
                if (hasLevel)
                {
                    var elementSize = LevelView.GetSizeRequest(width, height).Request;
                    double elementLeft = x + ContentMargin;
                    double elementTop = y + ContentMargin;
                    double elementWidth = elementSize.Width;
                    double elementHeight = elementSize.Height;
                    levelViewPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);
                    LevelView.LayoutUpdate(levelViewPosition);
                }

                LevelView.IsVisible = hasLevel;
                LevelView.Opacity = hasLevel ? 1 : 0;
            }

            if (StoreAllProductsButton != null)
            {
                if (hasLevel)
                {
                    var elementSize = StoreAllProductsButton.GetSizeRequest(width, height).Request;
                    double elementLeft = Width - elementSize.Width - Margin;
                    double elementTop = levelViewPosition.Y;
                    double elementHeight = elementSize.Height;
                    double elementWidth = elementSize.Width;

                    // Center from the level view.
                    elementTop += (levelViewPosition.Height - elementHeight) * 0.5f;

                    var elementPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                    StoreAllProductsButton.LayoutUpdate(elementPosition);
                }

                StoreAllProductsButton.IsVisible = hasLevel;
                StoreAllProductsButton.Opacity = hasLevel ? 1 : 0;
            }

            double gridElementLeft = x;
            double gridElementTop = levelViewPosition.Y + levelViewPosition.Height + ContentMargin;
            double gridElementWidth = width;
            double gridElementHeight = height - gridElementTop;
            Rectangle mainContentSize = new Rectangle(gridElementLeft, gridElementTop, gridElementWidth, gridElementHeight);

            if (GridViewProducts != null)
            {
                GridViewProducts.ItemWidth = width - (ContentMargin * 0f);
                GridViewProducts.LayoutUpdate(mainContentSize);
            }

            if (ScrollViewStoreItems != null)
            {
                ScrollViewStoreItems.LayoutUpdate(mainContentSize);
            }
        }

        /// <summary>
        /// Initialize internal components.
        /// </summary>
        protected override void InternalInitializeView()
        {
            StoreAllProductsButton = new GlyphLeftContentViewButton()
            {
                Style = Theme.ApplicationStyles.TextOnlyContentButtonStyle,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start,
                MinimumWidthRequest = 120,
                Text = App.LocalizationResources.StoreItemLevelAllLabel,
                FontSize = Theme.CommonResources.TextSizeMicro,
                TextColor = Theme.CommonResources.AccentAlternative,
                GlyphTextColor = Theme.CommonResources.AccentAlternative,
                GlyphText = Theme.CommonResources.GlyphTextAll,
                MarginElements = 0,
                MarginBorders = 2
            };

            if (UseGrid)
            {
                GridViewProducts = new RepeaterRecycleView()
                {
                    Padding = 0,
                    ItemHeight = 150,
                    ItemWidth = Width,
                    Orientation = ScrollOrientation.Vertical,
                    CleanOnBindingChange = true
                };
            }
            else
            {
                ScrollViewStoreItems = new ScrollView()
                {
                    Orientation = ScrollOrientation.Vertical,
                    Padding = 0,
                    MinimumHeightRequest = 80,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand
                };

                RepeaterStoreItemsLevels = new RepeaterView<StoreItemLevelViewModel>()
                {
                    Orientation = StackOrientation.Vertical,
                    Spacing = 0
                };

                ScrollViewStoreItems.Content = RepeaterStoreItemsLevels;
            }

            LevelView = new StoreItemLevelSimpleView(false, true, false, true)
            {
                Padding = 5,
                BackgroundMargin = 20,
                BackgroundTranslateX = ContentMargin
            };
        }
    }
}