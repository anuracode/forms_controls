// <copyright file="InfiniteRepeaterRecycleView.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Extensions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls
{
    /// <summary>
    /// A simple listview that exposes a bindable command to allow infinite loading behaviour.
    /// </summary>
    public class InfiniteRepeaterRecycleView : RepeaterRecycleView
    {
        /// <summary>
        /// Respresents the command that is fired to ask the view model to load additional data bound collection.
        /// </summary>
        public static readonly BindableProperty LoadMoreCommandProperty = BindablePropertyHelper.Create<InfiniteRepeaterRecycleView, ICommand>(nameof(LoadMoreCommand), default(ICommand));

        /// <summary>
        /// Total items count property.
        /// </summary>
        public static readonly BindableProperty TotalItemsCountProperty = BindablePropertyHelper.Create<InfiniteRepeaterRecycleView, int>(nameof(TotalItemsCount), 0, propertyChanged: TotalItemsCountChanged);

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="poolAheadItems">Number of items the pool should be ahead.</param>
        /// <param name="showActivityIndicator">Activity indicator.</param>
        /// <param name="instanceAllPoolAheadItems">Instance all pool ahead items even if not needed for the datasource.</param>
        public InfiniteRepeaterRecycleView(int pageSize, int pagesAhead = 2, int poolAheadItems = 4, bool showActivityIndicator = true, bool instanceAllPoolAheadItems = false) : base(poolAheadItems: poolAheadItems, showActivityIndicator: showActivityIndicator, instanceAllPoolAheadItems: instanceAllPoolAheadItems)
        {
            PagesAhead = pagesAhead;
            PageSize = pageSize;
        }

        /// <summary>
        /// Gets or sets the command binding that is called whenever the listview is getting near the bottomn of the list, and therefore requiress more data to be loaded.
        /// </summary>
        public ICommand LoadMoreCommand
        {
            get
            {
                return (ICommand)GetValue(LoadMoreCommandProperty);
            }

            set
            {
                SetValue(LoadMoreCommandProperty, value);
            }
        }

        /// <summary>
        /// Total items count.
        /// </summary>
        public int TotalItemsCount
        {
            get
            {
                return (int)GetValue(TotalItemsCountProperty);
            }

            set
            {
                SetValue(TotalItemsCountProperty, value);
            }
        }

        /// <summary>
        /// Number of pages to be ahead.
        /// </summary>
        protected int PagesAhead { get; set; }

        /// <summary>
        /// Size of the page to use.
        /// </summary>
        protected int PageSize { get; set; }

        /// <summary>
        /// Get Item source total count.
        /// </summary>
        /// <returns>Item source total count.</returns>
        protected override int GetItemSourceTotalCount()
        {
            return TotalItemsCount;
        }

        /// <summary>
        /// Update the step count.
        /// </summary>
        /// <param name="forceRedraw">Force redraw.</param>
        /// <returns>Task to await.</returns>
        protected override async Task UpdateStep(bool forceRedraw = false)
        {
            await base.UpdateStep(forceRedraw);

            if ((LoadMoreCommand != null) && (ItemsSource != null) && (LoadMoreCommand.CanExecute()))
            {
                var totalCount = GetItemSourceTotalCount();
                var sourceCount = ItemsSource.Count;

                if ((sourceCount < totalCount) && (((CurrentStep * PageSize) + (PageSize * PagesAhead)) >= sourceCount))
                {
                    LoadMoreCommand.ExecuteIfCan();
                }
            }
        }

        /// <summary>
        /// Update step.
        /// </summary>
        /// <param name="bindable">The control</param>
        /// <param name="oldValue">Previous bound collection</param>
        /// <param name="newValue">New bound collection</param>
        private static void TotalItemsCountChanged(
            BindableObject bindable,
            object oldValue,
            object newValue)
        {
            var control = bindable as InfiniteRepeaterRecycleView;

            if (control != null)
            {
                AC.ScheduleManaged(
                    async () =>
                    {
                        await control.UpdateStep(forceRedraw: true);
                    });
            }
        }
    }
}