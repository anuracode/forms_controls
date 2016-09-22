// <copyright file="InfiniteListView.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Extensions;
using System.Collections;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls
{
    /// <summary>
    /// A simple listview that exposes a bindable command to allow infinite loading behaviour.
    /// </summary>
    public class InfiniteListView : ListView
    {
        /// <summary>
        /// Respresents the command that is fired to ask the view model to load additional data bound collection.
        /// </summary>
        public static readonly BindableProperty LoadMoreCommandProperty = BindablePropertyHelper.Create<InfiniteListView, ICommand>(nameof(LoadMoreCommand), default(ICommand));

        /// <summary>
        /// Total items count property.
        /// </summary>
        public static readonly BindableProperty TotalItemsCountProperty = BindablePropertyHelper.Create<InfiniteListView, int>(nameof(TotalItemsCount), 0);

        /// <summary>
        /// Creates a new instance of a <see cref="InfiniteListView" />
        /// </summary>
        public InfiniteListView(ListViewCachingStrategy strategy)
            : base(strategy)
        {
            ItemAppearing += InfiniteListView_ItemAppearing;

            GroupHeaderTemplate = new DataTemplate(typeof(HeaderCell));
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
        /// When an item appears.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Arguments of the event.</param>
        protected virtual void InfiniteListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            var items = ItemsSource as IList;

            if ((LoadMoreCommand != null) && (items != null) && (items.Count > 0))
            {
                if (IsGroupingEnabled)
                {
                    IGroupingPaged itemsGroup = items[0] as IGroupingPaged;

                    if (itemsGroup == null)
                    {
                        var lastGroup = items[items.Count - 1] as IList;

                        if ((lastGroup != null) && (lastGroup.Count > 0) && (e.Item == lastGroup[lastGroup.Count - 1]))
                        {
                            AC.ScheduleManaged(
                                        () =>
                                        {
                                            LoadMoreCommand.ExecuteIfCan();

                                            return Task.FromResult(0);
                                        });
                        }
                    }
                    else
                    {
                        // Get the ungrouped item index.
                        int ungroupedItemIndex = 0;

                        int indexGroup = -1;
                        int totalInGroup = 0;
                        int itemsGroupCount = 0;

                        for (int i = 0; (i < items.Count) && (indexGroup < 0); i++)
                        {
                            itemsGroup = (IGroupingPaged)items[i];
                            indexGroup = itemsGroup.IndexOf(e.Item);

                            if (indexGroup < 0)
                            {
                                ungroupedItemIndex += itemsGroup.Count;
                            }
                            else
                            {
                                ungroupedItemIndex += indexGroup;
                                totalInGroup = itemsGroup.TotalGroup;
                                itemsGroupCount = itemsGroup.Count;
                            }
                        }

                        if (indexGroup > -1 && ungroupedItemIndex > 0)
                        {
                            if ((itemsGroupCount < totalInGroup) && ((itemsGroupCount - indexGroup) == 1))
                            {
                                if (LoadMoreCommand.CanExecute())
                                {
                                    AC.ScheduleManaged(
                                        () =>
                                        {
                                            LoadMoreCommand.Execute();

                                            return Task.FromResult(0);
                                        });
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (e.Item == items[items.Count - 1])
                    {
                        AC.ScheduleManaged(
                                        () =>
                                        {
                                            LoadMoreCommand.ExecuteIfCan();

                                            return Task.FromResult(0);
                                        });
                    }
                }
            }
        }
    }
}