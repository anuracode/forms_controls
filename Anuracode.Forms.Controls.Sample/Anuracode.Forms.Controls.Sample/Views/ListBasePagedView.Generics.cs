// <copyright file="ListBasePagedView.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System.Threading.Tasks;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls.Sample.Views
{
    /// <summary>
    /// Base view.
    /// </summary>
    public abstract class ListBasePagedView<TViewModel, TItem> : BasePagedView<TViewModel, TItem>
        where TViewModel : ViewModels.BaseViewModel, Interfaces.IListPagedViewModelBase<TItem>
        where TItem : class
    {
        /// <summary>
        /// List of all elements.
        /// </summary>
        private InfiniteListView listElementsAll;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="viewModel">View model to use.</param>
        public ListBasePagedView(TViewModel viewModel)
            : base(viewModel)
        {
            if (ViewModel != null)
            {
                ViewModel.Items.ItemCountChanged -= CollectionsChanged;
                ViewModel.Items.ItemCountChanged += CollectionsChanged;
            }
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ListBasePagedView()
            : this(null)
        {
        }

        /// <summary>
        /// Has list cache.
        /// </summary>
        protected virtual bool HasListCache
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// List of all elements.
        /// </summary>
        protected InfiniteListView ListElementsAll
        {
            get
            {
                return listElementsAll;
            }

            set
            {
                listElementsAll = value;
            }
        }

        /// <summary>
        /// Animate the Lable no elements.
        /// </summary>
        /// <param name="shouldBeVisible">True when it should be visible.</param>
        /// <param name="animationToken">Animation cancellation token.</param>
        /// <returns>Task to await.</returns>
        protected override async Task AnimateLabelNoElements(bool shouldBeVisible, System.Threading.CancellationToken animationToken)
        {
            await base.AnimateLabelNoElements(shouldBeVisible, animationToken);

            animationToken.ThrowIfCancellationRequested();

            if ((this.Content != null) && (ListElementsAll != null))
            {
                if (shouldBeVisible)
                {
                    if (ListElementsAll.Opacity != 0)
                    {
                        await ListElementsAll.FadeTo(0);
                    }
                }
                else
                {
                    if (ListElementsAll.Opacity != 1)
                    {
                        await ListElementsAll.FadeTo(1);
                    }
                }
            }
        }

        /// <summary>
        /// Bind the list of elements.
        /// </summary>
        protected virtual void BindListElements()
        {
            ListElementsAll.ItemsSource = ViewModel.Items;
            ListElementsAll.SetBinding<TViewModel>(InfiniteListView.TotalItemsCountProperty, vm => vm.TotalItemsCount);
        }

        /// <summary>
        /// Set the binding of the list of elements.
        /// </summary>
        /// <param name="ListElementsAll">List to use.</param>
        protected abstract void ConfigListElementsAll(ListView listElements);

        /// <summary>
        /// Filter bar enter pressed.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Arguments of the event.</param>
        protected override void FilterBar_Completed(object sender, System.EventArgs e)
        {
            if (ListElementsAll != null)
            {
                AC.ScheduleManaged(
                    () =>
                    {
                        ListElementsAll.Focus();

                        return Task.FromResult(0);
                    });
            }
        }

        /// <summary>
        /// Selected item changed.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Arguments of the event.</param>
        protected virtual void ListElements_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                if (ViewModel.ViewItemDetailCommand.CanExecute(e.Item))
                {
                    ViewModel.ViewItemDetailCommand.Execute(e.Item);
                }

                AC.ScheduleManaged(
                    () =>
                    {
                        try
                        {
                            if ((ListElementsAll != null) && (ListElementsAll.SelectedItem != null))
                            {
                                ListElementsAll.SelectedItem = null;
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
        /// Page disapperas.
        /// </summary>
        protected override void OnDisappearing()
        {
            if (ListElementsAll != null)
            {
                ListElementsAll.RemoveBinding(InfiniteListView.ItemsSourceProperty);
            }

            base.OnDisappearing();
        }

        /// <summary>
        /// Render the page layout.
        /// </summary>
        /// <returns>View to use.</returns>
        protected override View RenderContent()
        {
            ListElementsAll = new InfiniteListView(HasListCache ? ListViewCachingStrategy.RecycleElement : ListViewCachingStrategy.RetainElement)
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                Opacity = 0,
                SeparatorVisibility = SeparatorVisibility.None
            };

            ListElementsAll.ItemTapped -= ListElements_ItemTapped;
            ListElementsAll.ItemTapped += ListElements_ItemTapped;

            ConfigListElementsAll(ListElementsAll);

            ListElementsAll.Style = Theme.ApplicationStyles.CommonListViewStyle;

            BindListElements();

            ListElementsAll.IsPullToRefreshEnabled = true;
            ListElementsAll.SetBinding<TViewModel>(InfiniteListView.RefreshCommandProperty, vm => vm.RefreshCommand);
            ListElementsAll.SetBinding<TViewModel>(InfiniteListView.IsRefreshingProperty, vm => vm.IsLoading, mode: BindingMode.OneWay);
            ListElementsAll.SetBinding<TViewModel>(InfiniteListView.LoadMoreCommandProperty, vm => vm.LoadMoreItemsCommand);

            return ListElementsAll;
        }

        /// <summary>
        /// Render panel when no elements in list.
        /// </summary>
        /// <param name="panelNoElementsLayout">Stack to use.</param>
        protected override void RenderPanelNoElements(StackLayout panelNoElementsLayout)
        {
            if (HasFilterBar && (panelNoElementsLayout != null))
            {
                RenderSearchResultsView(panelNoElementsLayout);
            }

            base.RenderPanelNoElements(panelNoElementsLayout);
        }

        /// <summary>
        /// Render the search results text view.
        /// </summary>
        /// <param name="panelNoElementsLayout">Panel to use.</param>
        protected virtual void RenderSearchResultsView(StackLayout panelNoElementsLayout)
        {
            if (panelNoElementsLayout != null)
            {
                panelNoElementsLayout.Children.Add(new BoxView() { Color = Color.Transparent, HeightRequest = 5, WidthRequest = 5 });

                StackLayout stackFilterLabel = new StackLayout()
                {
                    Style = Theme.ApplicationStyles.SimpleStackContainerStyle,
                    Orientation = StackOrientation.Horizontal
                };

                stackFilterLabel.SetBinding<TViewModel>(StackLayout.IsVisibleProperty, vm => vm.IsFiltered);

                ExtendedLabel labelSearchResults = new ExtendedLabel()
                {
                    Style = Theme.ApplicationStyles.DefaultExtendedLabelStyle,
                    Text = App.LocalizationResources.SearchFilterByLabel,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Start
                };

                stackFilterLabel.Children.Add(labelSearchResults);

                Theme.RenderUtil.RenderSpace(stackFilterLabel, 5, 1);

                TextContentViewButton labelSearchTerm = new TextContentViewButton()
                {
                    Style = Theme.ApplicationStyles.TextOnlyContentButtonStyle,
                    TextColor = Theme.CommonResources.Accent,
                    Command = ShowSearchCommand,
                    FontName = labelSearchResults.FontName,
                    FriendlyFontName = labelSearchResults.FriendlyFontName,
                    FontSize = labelSearchResults.FontSize,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    MinimumWidthRequest = 250
                };

                labelSearchTerm.SetBinding<TViewModel>(TextContentViewButton.TextProperty, vm => vm.FilterTerm);
                stackFilterLabel.Children.Add(labelSearchTerm);

                panelNoElementsLayout.Children.Add(stackFilterLabel);
            }
        }
    }
}
