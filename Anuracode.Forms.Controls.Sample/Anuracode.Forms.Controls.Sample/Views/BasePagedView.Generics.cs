// <copyright file="BasePagedView.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Extensions;
using Anuracode.Forms.Controls.Views.Extensions;
using Anuracode.Forms.Controls.Sample.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls.Sample.Views
{
    /// <summary>
    /// Base view.
    /// </summary>
    public abstract class BasePagedView<TViewModel, TItem> : ContentBaseView<TViewModel>
        where TViewModel : ViewModels.BaseViewModel, IListPagedViewModelBase<TItem>
        where TItem : class
    {
        /// <summary>
        /// Command for the filter bar.
        /// </summary>
        private Command<string> executeSearchCommand;

        /// <summary>
        /// Hide search.
        /// </summary>
        private Command hideSearchCommand;

        /// <summary>
        /// Flag for the seach.
        /// </summary>
        private bool isSearchVisible;

        /// <summary>
        /// Show search.
        /// </summary>
        private Command showSearchCommand;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="viewModel">View model to use.</param>
        public BasePagedView(TViewModel viewModel)
            : base(viewModel)
        {
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public BasePagedView()
            : this(null)
        {
        }

        /// <summary>
        /// Command for the filter bar.
        /// </summary>
        public virtual Command<string> ExecuteSearchCommand
        {
            get
            {
                if (executeSearchCommand == null)
                {
                    executeSearchCommand = new Command<string>(
                        async (newFilterTerm) =>
                        {
                            if (!string.IsNullOrWhiteSpace(newFilterTerm) && string.Compare(ViewModel.FilterTerm, newFilterTerm, StringComparison.CurrentCultureIgnoreCase) != 0)
                            {
                                ViewModel.FilterTerm = newFilterTerm;
                            }

                            await Task.FromResult(0);

                            HideSearchCommand.ExecuteIfCan();

                            FilterBar_Completed(this, null);
                        });
                }

                return executeSearchCommand;
            }
        }

        /// <summary>
        /// Hide search.
        /// </summary>
        public virtual Command HideSearchCommand
        {
            get
            {
                if (hideSearchCommand == null)
                {
                    hideSearchCommand = new Command(
                        () =>
                        {
                            AC.ScheduleManaged(
                                async () =>
                                {
                                    try
                                    {
                                        await LockAnimation.WaitAsync();

                                        if (FilterExtraLayers != null)
                                        {
                                            for (int i = 0; i < FilterExtraLayers.Count; i++)
                                            {
                                                if (FilterExtraLayers[i] != null)
                                                {
                                                    await Theme.RenderUtil.AnimateFadeOutViewAsync(FilterExtraLayers[i]);
                                                }
                                            }
                                        }

                                        if (FloatingFilterContainer != null)
                                        {
                                            await FloatingFilterContainer.TranslateTo((FilterBar.Width + (ContentMargin * 2)) * -1, ContentMargin, easing: Easing.SinIn);
                                            FloatingFilterContainer.Opacity = 0;
                                            FloatingFilterContainer.IsVisible = false;
                                        }

                                        if (Header != null)
                                        {
                                            Header.HeightRequest = -1;
                                            Header.IsVisible = true;
                                            await Header.FadeTo(1, easing: Easing.SinIn);
                                        }

                                        if (StackShortCuts != null)
                                        {
                                            StackShortCuts.IsVisible = true;
                                            await StackShortCuts.FadeTo(1);
                                        }

                                        await UpdateBackgroundOpactity(true);

                                        if (BackgroundFilter != null)
                                        {
                                            await BackgroundFilter.FadeTo(0, easing: Easing.SinIn);
                                        }

                                        if (BackgroundFilter != null)
                                        {
                                            BackgroundFilter.IsVisible = false;
                                        }

                                        IsSearchVisible = false;
                                    }
                                    finally
                                    {
                                        LockAnimation.Release();
                                    }
                                });
                        },
                        () =>
                        {
                            return IsSearchVisible;
                        });
                }

                return hideSearchCommand;
            }
        }

        /// <summary>
        /// Flag for the seach.
        /// </summary>
        public bool IsSearchVisible
        {
            get
            {
                return isSearchVisible;
            }

            set
            {
                isSearchVisible = value;
                OnPropertyChanged(nameof(IsSearchVisible));
                ShowSearchCommand.RaiseCanExecuteChanged();
                HideSearchCommand.ChangeCanExecute();
            }
        }

        /// <summary>
        /// Show search.
        /// </summary>
        public virtual Command ShowSearchCommand
        {
            get
            {
                if (showSearchCommand == null)
                {
                    showSearchCommand = new Command(
                        () =>
                        {
                            AC.ScheduleManaged(
                                async () =>
                                {
                                    try
                                    {
                                        await LockAnimation.WaitAsync();

                                        if (FilterExtraLayers != null)
                                        {
                                            SimpleViewBase simpleView = null;
                                            View itemView = null;
                                            for (int i = 0; i < FilterExtraLayers.Count; i++)
                                            {
                                                itemView = FilterExtraLayers[i];
                                                if (itemView != null)
                                                {
                                                    simpleView = itemView as SimpleViewBase;

                                                    if (simpleView != null)
                                                    {
                                                        simpleView.InitializeView();
                                                        simpleView.PrepareBindings();
                                                    }

                                                    Theme.RenderUtil.AnimatePrepareFadeInView(itemView);
                                                }
                                            }
                                        }

                                        if (StackShortCuts != null)
                                        {
                                            await StackShortCuts.FadeTo(0);
                                        }

                                        if (Header != null)
                                        {
                                            await Header.FadeTo(0, easing: Easing.SinOut);
                                            Header.IsVisible = true;

                                            if (FloatingFilterContainer != null)
                                            {
                                                Header.HeightRequest = FloatingFilterContainer.Height;
                                            }
                                        }

                                        await UpdateBackgroundOpactity(false);

                                        if (FilterBar != null)
                                        {
                                            AC.ScheduleManaged(
                                                () =>
                                                {
                                                    FilterBar.FocusEntry();
                                                    return Task.FromResult(0);
                                                });
                                        }

                                        if (StackShortCuts != null)
                                        {
                                            StackShortCuts.IsVisible = false;
                                        }

                                        if (BackgroundFilter != null)
                                        {
                                            BackgroundFilter.Opacity = 0;
                                            BackgroundFilter.IsVisible = true;
                                            await BackgroundFilter.FadeTo(1, easing: Easing.SinOut);
                                        }

                                        if (FloatingFilterContainer != null)
                                        {
                                            await FloatingFilterContainer.TranslateTo((FloatingFilterContainer.Width + (ContentMargin * 2)) * -1, ContentMargin, 0);
                                            FloatingFilterContainer.Opacity = 0;
                                            FloatingFilterContainer.IsVisible = true;
                                            await FloatingFilterContainer.FadeTo(1, 0);
                                            await FloatingFilterContainer.TranslateTo(ContentMargin, ContentMargin, easing: Easing.SinOut);
                                        }

                                        if (FilterExtraLayers != null)
                                        {
                                            for (int i = 0; i < FilterExtraLayers.Count; i++)
                                            {
                                                if (FilterExtraLayers[i] != null)
                                                {
                                                    await Theme.RenderUtil.AnimateFadeInViewAsync(FilterExtraLayers[i]);
                                                }
                                            }
                                        }

                                        IsSearchVisible = true;
                                    }
                                    finally
                                    {
                                        LockAnimation.Release();
                                    }
                                });
                        },
                        () =>
                        {
                            return !IsSearchVisible;
                        });
                }

                return showSearchCommand;
            }
        }

        /// <summary>
        /// Backgorund when the filter is visible.
        /// </summary>
        protected View BackgroundFilter { get; set; }

        /// <summary>
        /// Filter bar.
        /// </summary>
        protected FilterBarView FilterBar { get; set; }

        /// <summary>
        /// List of extra layers.
        /// </summary>
        protected List<View> FilterExtraLayers { get; set; }

        /// <summary>
        /// Floating filter container.
        /// </summary>
        protected View FloatingFilterContainer { get; set; }

        /// <summary>
        /// View has filter.
        /// </summary>
        protected virtual bool HasFilterBar
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
        /// True then the page should render a filter.
        /// </summary>
        protected virtual bool RenderFilter
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Add the add toolbar item.
        /// </summary>
        protected virtual void AddAddToolbarItem()
        {
            if (!IsRecylced)
            {
                if (StackShortCuts != null)
                {
                    var addButton = new GlyphOnlyContentViewButton()
                    {
                        Style = Theme.ApplicationStyles.GlyphOnlyNavbarButtonStyle,
                        GlyphText = Theme.CommonResources.GlyphTextAdd,
                        Text = ViewModel.LocalizationResources.AddButton,
                        Command = ViewModel.AddItemCommand,
                        BindingContext = this
                    };

                    StackShortCuts.Children.Add(addButton);
                }
            }
        }

        /// <summary>
        /// Add custom toolbar items.
        /// </summary>
        protected virtual void AddCustomToolBarItems()
        {
        }

        /// <summary>
        /// Add extra layers to the filter.
        /// </summary>
        protected virtual List<View> AddFilterExtraLayers()
        {
            return null;
        }

        /// <summary>
        /// Add the refresh button.
        /// </summary>
        protected virtual void AddRefreshToolbarItem()
        {
            if (StackShortCuts != null)
            {
                var refreshButton = new GlyphOnlyContentViewButton()
                {
                    Style = Theme.ApplicationStyles.GlyphOnlyNavbarButtonStyle,
                    GlyphText = Theme.CommonResources.GlyphTextRefresh,
                    Command = ViewModel.RefreshCommand,
                    BindingContext = this
                };

                StackShortCuts.Children.Insert(0, refreshButton);
            }
        }

        /// <summary>
        /// Add the refresh button.
        /// </summary>
        protected virtual void AddSearchToolbarItem()
        {
            if (HasFilterBar)
            {
                if (StackShortCuts != null)
                {
                    var searchButton = new GlyphOnlyContentViewButton()
                    {
                        Style = Theme.ApplicationStyles.GlyphOnlyNavbarButtonStyle,
                        GlyphText = Theme.CommonResources.GlyphTextSearch,
                        Command = ShowSearchCommand,
                        BindingContext = this
                    };

                    StackShortCuts.Children.Insert(0, searchButton);
                }
            }
        }

        /// <summary>
        /// Add custom toolbar items.
        /// </summary>
        protected override void AddToolBarItems()
        {
            AddRefreshToolbarItem();
            AddAddToolbarItem();
            AddSearchToolbarItem();
            AddCustomToolBarItems();
            base.AddToolBarItems();
        }

        /// <summary>
        /// Animate the Lable no elements.
        /// </summary>
        /// <param name="shouldBeVisible">True when it should be visible.</param>
        /// <param name="animationToken">Animation cancellation token.</param>
        /// <returns>Task to await.</returns>
        protected virtual async Task AnimateLabelNoElements(bool shouldBeVisible, CancellationToken animationToken)
        {
            animationToken.ThrowIfCancellationRequested();

            if (this.Content != null)
            {
                if (shouldBeVisible && LabelNoElements.Opacity != 1)
                {
                    await Task.Delay(TimeSpan.FromSeconds(2), animationToken);
                    Theme.RenderUtil.AnimatePrepareFadeInView(LabelNoElements);
                    animationToken.ThrowIfCancellationRequested();
                    await Theme.RenderUtil.AnimateFadeInViewAsync(LabelNoElements);
                }
                else if (!shouldBeVisible && LabelNoElements.Opacity != 0)
                {
                    await Theme.RenderUtil.AnimateFadeOutViewAsync(LabelNoElements);
                }
            }
        }

        /// <summary>
        /// Set the properties for the label when there is no elements in the list.
        /// A binding is recomended.
        /// </summary>
        /// <param name="labelToSet">Label to set.</param>
        protected virtual void BindLabelNoElements(ExtendedLabel labelToSet)
        {
            labelToSet.Style = Theme.ApplicationStyles.EmptyListExtendedLabelStyle;
        }

        /// <summary>
        /// Collection changed.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Argument of the event.</param>
        protected virtual void CollectionsChanged(object sender, EventArgs<int> e)
        {
            AC.ScheduleManaged(
                async () =>
                {
                    bool shouldBeVisible = await GetLabelNoElementsVisibilty();

                    if (LabelNoElements != null)
                    {
                        try
                        {
                            if (AnimationTokenSource != null && !AnimationTokenSource.IsCancellationRequested)
                            {
                                AnimationTokenSource.Cancel();
                                AnimationTokenSource = null;
                            }

                            if (AnimationTokenSource == null)
                            {
                                AnimationTokenSource = new CancellationTokenSource();
                            }

                            CancellationToken animationToken = AnimationTokenSource.Token;

                            animationToken.ThrowIfCancellationRequested();

                            await LockAnimation.WaitAsync(animationToken);

                            if (this.Content != null)
                            {
                                await AnimateLabelNoElements(shouldBeVisible, animationToken);
                            }
                        }
                        finally
                        {
                            LockAnimation.Release();
                        }
                    }
                });
        }

        /// <summary>
        /// Filter bar enter pressed.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Arguments of the event.</param>
        protected virtual void FilterBar_Completed(object sender, System.EventArgs e)
        {
        }

        /// <summary>
        /// Get if the label no elements should be visible.
        /// </summary>
        /// <returns>Task to await.</returns>
        protected virtual Task<bool> GetLabelNoElementsVisibilty()
        {
            return Task.FromResult((ViewModel != null) && (ViewModel.Items.Count == 0));
        }

        /// <summary>
        /// Layout extra layers.
        /// </summary>
        /// <param name="pageSize">Page size.</param>
        /// <param name="filterPosition">Position of the filter.</param>
        protected virtual void LayoutFilterExtraLayers(Rectangle pageSize, Rectangle filterPosition)
        {
        }

        /// <summary>
        /// Page appears.
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();

            CollectionsChanged(this, new EventArgs<int>(0));
        }

        /// <summary>
        /// Layout the children for the background.
        /// </summary>
        /// <param name="pageSize">Page size.</param>
        /// <param name="headerPosition">Header position.</param>
        /// <param name="contentPosition">Content position.</param>
        /// <param name="footerPosition">Footer position.</param>
        protected override void PageExtraLayersLayoutChildren(Rectangle pageSize, Rectangle headerPosition, Rectangle contentPosition, Rectangle footerPosition)
        {
            base.PageExtraLayersLayoutChildren(pageSize, headerPosition, contentPosition, footerPosition);

            Rectangle filterPosition = new Rectangle();

            if (FloatingFilterContainer != null)
            {
                var filterSize = FloatingFilterContainer.Measure(pageSize.Width, pageSize.Height).Request;

                double elementWidth = pageSize.Width + ContentMargin;
                double elementLeft = -ContentMargin;
                double elementTop = headerPosition.Y - (ContentMargin * 2f);
                double elementHeight = filterSize.Height;

                filterPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                FloatingFilterContainer.LayoutUpdate(filterPosition);
            }

            if ((FilterExtraLayers != null) && (FilterExtraLayers.Count > 0))
            {
                LayoutFilterExtraLayers(pageSize, filterPosition);
            }

            if (BackgroundFilter != null)
            {
                Rectangle elementSize = new Rectangle(
                    0,
                    0,
                    pageSize.Width,
                    pageSize.Height);

                BackgroundFilter.LayoutUpdate(elementSize);
            }
        }

        /// <summary>
        /// Render background filter.
        /// </summary>
        /// <returns>View to use.</returns>
        protected virtual View RenderBackgroundFilter()
        {
            BoxView backgroundBox = new BoxView()
            {
                Color = Theme.CommonResources.BackgroundColorTranslucent,
                IsVisible = false,
                Opacity = 0
            };

            TapGestureRecognizer tapBackground = new TapGestureRecognizer();

            tapBackground.Tapped += (object sender, System.EventArgs e) =>
            {
                if (ExecuteSearchCommand.CanExecute(ViewModel.FilterTerm))
                {
                    ExecuteSearchCommand.Execute(ViewModel.FilterTerm);
                }
            };

            backgroundBox.GestureRecognizers.Add(tapBackground);

            return backgroundBox;
        }

        /// <summary>
        /// Render filters in the header.
        /// </summary>
        /// <param name="headerLayout">Stack to use.</param>
        protected virtual void RenderFilters(StackLayout headerLayout)
        {
            // Permantent filters.
        }

        /// <summary>
        /// Render the floating search filter.
        /// </summary>
        /// <returns>View to add.</returns>
        protected virtual View RenderFloatingSearchFilter()
        {
            FilterBarView newFilter = new FilterBarView()
            {
                FilterBackgroundColor = Theme.CommonResources.Accent,
                Command = ExecuteSearchCommand,
                Opacity = 0,
                IsVisible = false
            };

            newFilter.PrepareBindings();

            newFilter.SetBinding<TViewModel>(FilterBarView.TextProperty, vm => vm.FilterTerm, mode: BindingMode.TwoWay);
            newFilter.SetBinding<TViewModel>(FilterBarView.CommandParameterProperty, vm => vm.FilterTerm);
            newFilter.SetBinding<TViewModel>(FilterBarView.PlaceholderProperty, vm => vm.LocalizationResources.FilterLabel);

            FilterBar = newFilter;

            return newFilter;
        }

        /// <summary>
        /// Render the header of the list.
        /// </summary>
        /// <returns>View to use.</returns>
        protected override View RenderHeader()
        {
            StackLayout headerLayout = base.RenderHeader() as StackLayout;

            if (headerLayout != null)
            {
                var panelNoElementsLayout = new StackLayout()
                {
                    Style = Theme.ApplicationStyles.SimpleStackContainerStyle
                };

                if (RenderFilter)
                {
                    RenderFilters(headerLayout);
                }

                RenderPanelNoElements(panelNoElementsLayout);

                headerLayout.Children.Add(panelNoElementsLayout);
            }

            return headerLayout;
        }

        /// <summary>
        /// Render layer layout like the notifications and the progress.
        /// </summary>
        /// <param name="baseLayout">Layout to use</param>
        protected override void RenderLayerLayout(AbsoluteLayout baseLayout)
        {
            if (baseLayout != null)
            {
                if (HasFilterBar)
                {
                    BackgroundFilter = RenderBackgroundFilter();

                    if (BackgroundFilter != null)
                    {
                        baseLayout.Children.Add(BackgroundFilter);
                    }

                    FilterExtraLayers = AddFilterExtraLayers();

                    if (FilterExtraLayers != null)
                    {
                        for (int i = 0; i < FilterExtraLayers.Count; i++)
                        {
                            if (FilterExtraLayers[i] != null)
                            {
                                baseLayout.Children.Add(FilterExtraLayers[i]);
                            }
                        }
                    }

                    FloatingFilterContainer = RenderFloatingSearchFilter();

                    if (FloatingFilterContainer != null)
                    {
                        baseLayout.Children.Add(FloatingFilterContainer);
                    }
                }
            }

            base.RenderLayerLayout(baseLayout);
        }

        /// <summary>
        /// Render panel when no elements in list.
        /// </summary>
        /// <param name="panelNoElementsLayout">Stack to use.</param>
        protected virtual void RenderPanelNoElements(StackLayout panelNoElementsLayout)
        {
            LabelNoElements = new ExtendedLabel()
            {
                Style = Theme.ApplicationStyles.DescriptionExtendedLabelStyle,
                Opacity = 0,
                IsVisible = false
            };

            BindLabelNoElements(LabelNoElements);
            panelNoElementsLayout.Children.Add(LabelNoElements);
        }
    }
}