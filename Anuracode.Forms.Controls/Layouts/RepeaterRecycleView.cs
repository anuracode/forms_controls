// <copyright file="RepeaterRecycleView.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Extensions;
using Anuracode.Forms.Controls.Views.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls
{
    /// <summary>
    /// Repeater view that recycles the elements.
    /// </summary>
    public class RepeaterRecycleView : ScrollView
    {
        /// <summary>
        /// Definition for <see cref="ItemsSource"/>
        /// </summary>
        /// Element created at 15/11/2014,3:11 PM by Charles
        public static readonly BindableProperty ItemsSourceProperty =
            BindablePropertyHelper.Create<RepeaterRecycleView, IList>(nameof(ItemsSource), null, propertyChanged: ItemsSourceChanged);

        /// <summary>
        /// Definition for <see cref="ItemTemplate"/>
        /// </summary>
        /// Element created at 15/11/2014,3:11 PM by Charles
        public static readonly BindableProperty ItemTemplateProperty =
            BindablePropertyHelper.Create<RepeaterRecycleView, DataTemplate>(nameof(ItemTemplate), defaultValue: default(DataTemplate));

        /// <summary>
        /// Definition for <see cref="ItemsSource"/>
        /// </summary>
        /// Element created at 15/11/2014,3:11 PM by Charles
        public static readonly BindableProperty SpacingProperty =
            BindablePropertyHelper.Create<RepeaterRecycleView, double>(nameof(Spacing), defaultValue: (double)0);

        /// <summary>
        /// Instance all pool ahead items.
        /// </summary>
        protected readonly bool InstanceAllPoolAheadItems;

        /// <summary>
        /// Number of items that should the pool be ahead of the visible.
        /// </summary>
        protected readonly int PoolAheadItems;

        /// <summary>
        /// Semaphore for the repository file.
        /// </summary>
        private static SemaphoreSlim lockInstanceViewUILevel1;

        /// <summary>
        /// Semaphore for the repository file.
        /// </summary>
        private static SemaphoreSlim lockInstanceViewUILevel2;

        /// <summary>
        /// Controsl pool.
        /// </summary>
        private List<View> controlsPool;

        /// <summary>
        /// Flag when is loading.
        /// </summary>
        private bool isLoading;

        /// <summary>
        /// Item height.
        /// </summary>
        private double itemHeight = 50;

        /// <summary>
        /// Item width.
        /// </summary>
        private double itemWidth = 50;

        /// <summary>
        /// Semaphore for the repository file.
        /// </summary>
        private SemaphoreSlim lockPool;

        /// <summary>
        /// Semaphore for the repository file.
        /// </summary>
        private SemaphoreSlim lockUI;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="poolAheadItems">Number of items the pool should be ahead.</param>
        /// <param name="showActivityIndicator">Activity indicator.</param>
        /// <param name="instanceAllPoolAheadItems">Instance all pool ahead items even if not needed for the datasource.</param>
        public RepeaterRecycleView(int poolAheadItems = 4, bool showActivityIndicator = true, bool instanceAllPoolAheadItems = false)
        {
            InstanceAllPoolAheadItems = instanceAllPoolAheadItems;
            PoolAheadItems = poolAheadItems.Clamp(1, int.MaxValue);

            this.Orientation = ScrollOrientation.Horizontal;

            this.PropertyChanged += RepeaterRecycleView_PropertyChanged;

            ContentLayout = new SimpleLayout()
            {
                IsHandlingLayoutManually = true,
                Padding = 0
            };

            ContentLayout.OnLayoutChildren += ContentLayout_OnLayoutChildren;
            ContentLayout.ManualSizeCalculationDelegate = ContentLayout_OnSizeRequest;

            if (Device.OS == TargetPlatform.Windows)
            {
                showActivityIndicator = false;
            }

            if (showActivityIndicator)
            {
                ActivityView = new ActivityIndicator()
                {
                    Color = Styles.ThemeManager.CommonResourcesBase.Accent
                };

                ActivityView.BindingContext = this;

                ActivityView.SetBinding<RepeaterRecycleView>(View.IsVisibleProperty, vm => vm.IsLoading);
                ActivityView.SetBinding<RepeaterRecycleView>(ActivityIndicator.IsRunningProperty, vm => vm.IsLoading);

                ContentLayout.Children.Add(ActivityView);
            }

            Content = ContentLayout;
        }

        /// <summary>
        /// Clean when binding change.
        /// </summary>
        public bool CleanOnBindingChange { get; set; }

        /// <summary>
        /// Layout of the view.
        /// </summary>
        public SimpleLayout ContentLayout { get; set; }

        /// <summary>
        /// Instance in a second level lock.
        /// </summary>
        public bool InstanceOnSublevelLock { get; set; }

        /// <summary>
        /// Flag when is loading.
        /// </summary>
        public bool IsLoading
        {
            get
            {
                return isLoading;
            }

            set
            {
                if (isLoading != value)
                {
                    isLoading = value;

                    OnPropertyChanged("IsLoading");
                }
            }
        }

        /// <summary>
        /// Item height.
        /// </summary>
        public double ItemHeight
        {
            get
            {
                return itemHeight;
            }

            set
            {
                if (itemHeight != value)
                {
                    itemHeight = value.Clamp(0, 4000);
                }
            }
        }

        /// <summary>
        /// Item source for the collection.
        /// </summary>
        public IList ItemsSource
        {
            get
            {
                return (IList)GetValue(ItemsSourceProperty);
            }

            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }

        /// <summary>
        /// The item template property
        /// This can be used on it's own or in combination with
        /// the <see cref="TemplateSelector"/>
        /// </summary>
        /// Element created at 15/11/2014,3:10 PM by Charles
        public DataTemplate ItemTemplate
        {
            get
            {
                return (DataTemplate)GetValue(ItemTemplateProperty);
            }

            set
            {
                SetValue(ItemTemplateProperty, value);
            }
        }

        /// <summary>
        /// Item width.
        /// </summary>
        public double ItemWidth
        {
            get
            {
                return itemWidth;
            }

            set
            {
                if (itemWidth != value)
                {
                    itemWidth = value.Clamp(0, 4000);
                }
            }
        }

        /// <summary>
        /// Semaphore for the repository file.
        /// </summary>
        public SemaphoreSlim LockInstanceViewUILevel1
        {
            get
            {
                if (lockInstanceViewUILevel1 == null)
                {
                    lockInstanceViewUILevel1 = new SemaphoreSlim(1);
                }

                return lockInstanceViewUILevel1;
            }
        }

        /// <summary>
        /// Semaphore for the repository file.
        /// </summary>
        public SemaphoreSlim LockInstanceViewUILevel2
        {
            get
            {
                if (lockInstanceViewUILevel2 == null)
                {
                    lockInstanceViewUILevel2 = new SemaphoreSlim(1);
                }

                return lockInstanceViewUILevel2;
            }
        }

        /// <summary>
        /// Semaphore for pool.
        /// </summary>
        public SemaphoreSlim LockPool
        {
            get
            {
                if (lockPool == null)
                {
                    lockPool = new SemaphoreSlim(1);
                }

                return lockPool;
            }
        }

        /// <summary>
        /// Semaphore for the repository file.
        /// </summary>
        public SemaphoreSlim LockUI
        {
            get
            {
                if (lockUI == null)
                {
                    lockUI = new SemaphoreSlim(1);
                }

                return lockUI;
            }
        }

        /// <summary>
        /// Spacing to use.
        /// </summary>
        public double Spacing
        {
            get
            {
                return (double)GetValue(SpacingProperty);
            }

            set
            {
                SetValue(SpacingProperty, value);
            }
        }

        /// <summary>
        /// Activity indicator.
        /// </summary>
        protected ActivityIndicator ActivityView { get; set; }

        /// <summary>
        /// Pool of controls to use.
        /// </summary>
        protected List<View> ControlsPool
        {
            get
            {
                if (controlsPool == null)
                {
                    controlsPool = new List<View>();
                }

                return controlsPool;
            }
        }

        /// <summary>
        /// Current pool diference.
        /// </summary>
        protected int CurrentPoolDiff { get; set; }

        /// <summary>
        /// Current step.
        /// </summary>
        protected int CurrentStep { get; set; }

        /// <summary>
        /// Last step.
        /// </summary>
        protected int LastStep { get; set; }

        /// <summary>
        /// Current pool count.
        /// </summary>
        protected int PoolCount { get; set; }

        /// <summary>
        /// Render cancellation token.
        /// </summary>
        protected CancellationTokenSource RenderCancellationToken { get; set; }

        /// <summary>
        /// Scroll to an index in the collection.
        /// </summary>
        /// <param name="indexInSource">Index to scroll to.</param>
        /// <param name="scrollToPosition">Position in the scroll.</param>
        /// <param name="animated">Should be animated.</param>
        /// <returns>Task to await.</returns>
        public async Task ScrollToIndexAsync(int indexInSource, ScrollToPosition scrollToPosition, bool animated)
        {
            int maxItems = 0;

            if (ItemsSource != null)
            {
                maxItems = GetItemSourceTotalCount();
            }

            double maxX = ContentSize.Width;
            double maxY = ContentSize.Height;
            double currentY = ScrollY;
            double currentX = ScrollX;
            double componentSize = (Orientation == ScrollOrientation.Horizontal) ? ItemWidth + Spacing : ItemHeight + Spacing;
            double maxComponentValue = (Orientation == ScrollOrientation.Horizontal) ? maxX : maxY;

            double newPosition = (indexInSource.Clamp(0, maxItems) * componentSize);

            switch (scrollToPosition)
            {
                case ScrollToPosition.Center:
                    newPosition -= ((Width - componentSize) * 0.5f);
                    break;

                case ScrollToPosition.End:
                    newPosition += ((Width - componentSize) * 1f);
                    break;

                case ScrollToPosition.MakeVisible:
                case ScrollToPosition.Start:
                default:
                    break;
            }

            newPosition = newPosition.Clamp(0, maxComponentValue);

            if (Orientation == ScrollOrientation.Horizontal)
            {
                var task = ScrollToAsync(newPosition, currentY, animated);

                if (animated)
                {
                    await task;
                }
            }
            else
            {
                var task = ScrollToAsync(currentX, newPosition, animated);

                if (animated)
                {
                    await task;
                }
            }
        }

        /// <summary>
        /// Cancel render.
        /// </summary>
        /// <param name="hideItems">Hide all the items.</param>
        protected async Task CancelRenderAsync(bool hideItems = false)
        {
            try
            {
                await LockPool.WaitAsync();

                // Cancel the last operation.
                if (RenderCancellationToken != null && !RenderCancellationToken.IsCancellationRequested)
                {
                    RenderCancellationToken.Cancel();
                }

                if (hideItems)
                {
                    IsLoading = true;

                    if (ControlsPool != null)
                    {
                        View itemView;
                        for (int i = 0; i < ControlsPool.Count; i++)
                        {
                            itemView = ControlsPool[i];

                            if (itemView != null)
                            {
                                itemView.UpdateOpacity(0);
                                itemView.UpdateIsVisible(false);
                            }
                        }
                    }
                }
            }
            catch (System.ObjectDisposedException)
            {
            }
            finally
            {
                LockPool.Release();
            }
        }

        /// <summary>
        /// Layout children.
        /// </summary>
        /// <param name="x">Top to use.</param>
        /// <param name="y">Left to use.</param>
        /// <param name="width">Width to use.</param>
        /// <param name="height">Height to use.</param>
        protected void ContentLayout_OnLayoutChildren(double x, double y, double width, double height)
        {
            // Do nothing, only layout when itemsources changes or the scroll moves enough.
        }

        /// <summary>
        /// Mesure the content.
        /// </summary>
        /// <param name="widthConstraint">Width to constarint.</param>
        /// <param name="heightConstraint">Height to constraint.</param>
        /// <returns>Size to use.</returns>
        protected virtual SizeRequest ContentLayout_OnSizeRequest(double widthConstraint, double heightConstraint)
        {
            double itemCount = 0;
            double totalheight = 0;
            double totalWidth = 0;

            if (ItemsSource != null)
            {
                itemCount = GetItemSourceTotalCount();
            }

            if (itemCount != 0)
            {
                totalheight = ItemHeight;
                totalWidth = ItemWidth;
            }

            if (Orientation == ScrollOrientation.Horizontal)
            {
                totalWidth = ((ItemWidth + Spacing) * itemCount);
            }
            else
            {
                totalheight = ((ItemHeight + Spacing) * itemCount);
            }

            SizeRequest resultRequest = new SizeRequest(new Size(totalWidth.Clamp(0, widthConstraint), totalheight.Clamp(0, heightConstraint)), new Size(totalWidth.Clamp(0, widthConstraint), totalheight.Clamp(0, heightConstraint)));

            return resultRequest;
        }

        /// <summary>
        /// Get Item source total count.
        /// </summary>
        /// <returns>Item source total count.</returns>
        protected virtual int GetItemSourceTotalCount()
        {
            int totalCount = 0;

            if (ItemsSource != null)
            {
                totalCount = ItemsSource.Count;
            }

            return totalCount;
        }

        /// <summary>
        /// Use once, it cancels the previous token and gets a new one.
        /// </summary>
        /// <returns>Token to use.</returns>
        protected async Task<CancellationToken> GetRenewedCancellationTokenAsync()
        {
            await CancelRenderAsync();

            RenderCancellationToken = new CancellationTokenSource();

            return RenderCancellationToken.Token;
        }

        /// <summary>
        /// Collection changed.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Arguments of the event.</param>
        protected void ItemSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            AC.ScheduleManaged(
                    async () =>
                    {
                        await CancelRenderAsync(e.Action == NotifyCollectionChangedAction.Reset ? CleanOnBindingChange : false);

                        await UpdateStep(forceRedraw: (e.Action == NotifyCollectionChangedAction.Reset));
                    });
        }

        /// <summary>
        /// Layout internal items.
        /// </summary>
        /// <returns>Task to await.</returns>
        protected virtual async Task LayoutInternalItems(CancellationToken cancellationToken)
        {
            try
            {
                if (ActivityView != null)
                {
                    var aelementSize = ActivityView.Measure(ItemWidth, ItemHeight).Request;
                    double aelementHeight = aelementSize.Height.Clamp(ItemHeight * 0.33f, ItemHeight);
                    double aelementWidth = aelementSize.Width.Clamp(ItemWidth * 0.33f, ItemWidth);
                    double aelementLeft = ((ContentSize.Width - aelementSize.Width) * 0.5f) + ScrollX;
                    double aelementTop = ((ContentSize.Height - aelementSize.Height) * 0.5f) + ScrollY;

                    var aelementPosition = new Rectangle(aelementLeft, aelementTop, aelementWidth, aelementHeight);

                    ActivityView.LayoutUpdate(aelementPosition);
                }

                await LockUI.WaitAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();

                int step = CurrentStep;
                int poolCount = PoolCount;
                int poolDiff = CurrentPoolDiff;
                int poolDiffAhead = CurrentPoolDiff + 1;
                double elementLeft = 0;
                double elementTop = 0;
                double elementWeight = ItemWidth;
                double elementHeight = ItemHeight;
                Rectangle elementPosition = new Rectangle();
                Rectangle outOfRangePostion = new Rectangle();

                View itemView = null;
                object content = null;

                int indexToUpdate = 0;

                var nullValues = from poolItem in ControlsPool
                                 where poolItem == null
                                 select poolItem;

                var nullValuesCount = await nullValues.CountAsync();

                bool requiresFullLock = nullValuesCount > 0;

                try
                {
                    if (requiresFullLock)
                    {
                        IsLoading = true;

                        if (InstanceOnSublevelLock)
                        {
                            await LockInstanceViewUILevel2.WaitAsync(cancellationToken);
                        }
                        else
                        {
                            await LockInstanceViewUILevel1.WaitAsync(cancellationToken);
                        }
                    }

                    cancellationToken.ThrowIfCancellationRequested();

                    if (poolCount > 0)
                    {
                        for (int i = 0; i < poolCount; i++)
                        {
                            itemView = ControlsPool[i];

                            if (itemView == null)
                            {
                                content = ItemTemplate.CreateContent();

                                if (!(content is View) && !(content is ViewCell))
                                {
                                    throw new ArgumentException(content.GetType().Name);
                                }

                                itemView = (content is View) ? content as View : ((ViewCell)content).View;

                                ControlsPool[i] = itemView;

                                try
                                {
                                    await Task.Delay(Device.Idiom == TargetIdiom.Phone ? 15 : 6, cancellationToken);
                                }
                                finally
                                {
                                    ContentLayout.Children.Add(itemView);
                                }

                                await Task.Delay(Device.Idiom == TargetIdiom.Phone ? 15 : 6, cancellationToken);

                                cancellationToken.ThrowIfCancellationRequested();
                            }

                            if (i < step)
                            {
                                indexToUpdate = i + (poolCount * poolDiffAhead);

                                if (Orientation == ScrollOrientation.Horizontal)
                                {
                                    elementLeft = ((ItemWidth + Spacing) * i) + ((poolCount * (ItemWidth + Spacing)) * poolDiffAhead);
                                }
                                else
                                {
                                    elementTop = ((ItemHeight + Spacing) * i) + ((poolCount * (ItemHeight + Spacing)) * poolDiffAhead);
                                }

                                UpdatePoolBindingContext(indexToUpdate, itemView, cancellationToken);
                            }
                            else
                            {
                                indexToUpdate = i + (poolCount * poolDiff);

                                if (Orientation == ScrollOrientation.Horizontal)
                                {
                                    elementLeft = ((ItemWidth + Spacing) * i) + ((poolCount * (ItemWidth + Spacing)) * poolDiff);
                                }
                                else
                                {
                                    elementTop = ((ItemHeight + Spacing) * i) + ((poolCount * (ItemHeight + Spacing)) * poolDiff);
                                }

                                UpdatePoolBindingContext(indexToUpdate, itemView, cancellationToken);
                            }

                            if ((ItemsSource != null) && (ItemsSource.Count > indexToUpdate) && (itemView != null))
                            {
                                elementPosition = new Rectangle(elementLeft, elementTop, elementWeight, elementHeight);
                            }
                            else
                            {
                                elementPosition = outOfRangePostion;
                            }

                            cancellationToken.ThrowIfCancellationRequested();

                            itemView.UpdateIsVisible(true);
                            itemView.UpdateOpacity(1);

                            itemView.LayoutUpdate(elementPosition);
                        }
                    }
                }
                finally
                {
                    if (requiresFullLock)
                    {
                        if (InstanceOnSublevelLock)
                        {
                            LockInstanceViewUILevel2.Release();
                        }
                        else
                        {
                            LockInstanceViewUILevel1.Release();
                        }
                    }
                }

                // Hide the items that are not used.
                for (int i = poolCount; i < ControlsPool.Count; i++)
                {
                    itemView = ControlsPool[i];

                    if (InstanceAllPoolAheadItems && (itemView == null))
                    {
                        content = ItemTemplate.CreateContent();

                        if (!(content is View) && !(content is ViewCell))
                        {
                            throw new ArgumentException(content.GetType().Name);
                        }

                        itemView = (content is View) ? content as View : ((ViewCell)content).View;

                        ControlsPool[i] = itemView;

                        try
                        {
                            await Task.Delay(Device.Idiom == TargetIdiom.Phone ? 15 : 6, cancellationToken);
                        }
                        finally
                        {
                            ContentLayout.Children.Add(itemView);
                        }

                        await Task.Delay(Device.Idiom == TargetIdiom.Phone ? 15 : 6, cancellationToken);

                        cancellationToken.ThrowIfCancellationRequested();
                    }

                    if (itemView != null)
                    {
                        itemView.UpdateOpacity(0);
                        itemView.UpdateIsVisible(false);
                    }

                    cancellationToken.ThrowIfCancellationRequested();
                }
            }
            finally
            {
                LockUI.Release();
                IsLoading = false;
            }
        }

        /// <summary>
        /// Prepare the controls pool needed.
        /// </summary>
        protected async Task PrepareControlPool()
        {
            await Task.FromResult(0);

            try
            {
                await LockPool.WaitAsync();

                if ((ItemTemplate != null) && (ItemsSource != null))
                {
                    var itemSourceCount = GetItemSourceTotalCount();

                    // Extra space.
                    double visibleCount = 0;
                    double fullpool = 0;

                    if (Orientation == ScrollOrientation.Horizontal)
                    {
                        fullpool = ((Width * (1 / (ItemWidth + Spacing))) + PoolAheadItems);
                        visibleCount = fullpool.Clamp(0, itemSourceCount);
                    }
                    else
                    {
                        fullpool = ((Height * (1 / (ItemHeight + Spacing))) + PoolAheadItems);
                        visibleCount = fullpool.Clamp(0, itemSourceCount);
                    }

                    if (InstanceAllPoolAheadItems)
                    {
                        while (ControlsPool.Count < fullpool)
                        {
                            ControlsPool.Add(null);
                        }
                    }
                    else
                    {
                        while (ControlsPool.Count < visibleCount)
                        {
                            ControlsPool.Add(null);
                        }
                    }

                    PoolCount = ControlsPool.Count.Clamp(0, itemSourceCount);
                }
                else
                {
                    PoolCount = 0;
                }
            }
            finally
            {
                LockPool.Release();
            }
        }

        /// <summary>
        /// Property changed.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Arguments of the event.</param>
        protected virtual void RepeaterRecycleView_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (ContentLayout != null)
            {
                if ((e.PropertyName == RepeaterRecycleView.ScrollXProperty.PropertyName) ||
                    (e.PropertyName == RepeaterRecycleView.ScrollYProperty.PropertyName))
                {
                    AC.ScheduleManaged(
                        async () =>
                        {
                            await UpdateStep();
                        });
                }
            }
        }

        /// <summary>
        /// Update binding context.
        /// </summary>
        /// <param name="indexToUpdate">Index to use.</param>
        /// <param name="itemView">View to update.</param>
        /// <param name="cancellationToken">Cancellation token to use.</param>
        protected void UpdatePoolBindingContext(int indexToUpdate, View itemView, CancellationToken cancellationToken)
        {
            if ((ItemsSource != null) && (ItemsSource.Count > indexToUpdate) && (itemView != null))
            {
                cancellationToken.ThrowIfCancellationRequested();

                if ((ItemsSource != null) && (ItemsSource.Count > indexToUpdate) && (itemView != null))
                {
                    SimpleViewBase baseView = itemView as SimpleViewBase;

                    if (baseView != null)
                    {
                        baseView.InitializeView();
                    }

                    cancellationToken.ThrowIfCancellationRequested();

                    if (itemView.BindingContext != ItemsSource[indexToUpdate])
                    {
                        itemView.BindingContext = ItemsSource[indexToUpdate];
                    }

                    cancellationToken.ThrowIfCancellationRequested();

                    if (baseView != null)
                    {
                        baseView.PrepareBindings();
                        baseView.SetupViewValues(true);
                    }
                }
            }
        }

        /// <summary>
        /// Update the step count.
        /// </summary>
        /// <param name="forceRedraw">Force redraw.</param>
        /// <returns>Task to await.</returns>
        protected virtual async Task UpdateStep(bool forceRedraw = false)
        {
            int lastPoolCount = PoolCount;

            await PrepareControlPool();

            double itemLenght = Orientation == ScrollOrientation.Horizontal ? ItemWidth + Spacing : ItemHeight + Spacing;
            double axisScroll = Orientation == ScrollOrientation.Horizontal ? ScrollX : ScrollY;

            int calculatedStep = 0;
            int poolCount = PoolCount;
            int calculatedPoolDiff = 0;

            if ((axisScroll > 0) && (poolCount > 0))
            {
                calculatedStep = (int)(axisScroll / itemLenght);

                double initialCalculatedPool = (double)calculatedStep / (double)poolCount;

                double integerPool = Math.Truncate(initialCalculatedPool);

                double remain = initialCalculatedPool - integerPool;

                calculatedPoolDiff = Convert.ToInt32(remain == 0 ? integerPool - 1 : integerPool);
                calculatedStep = calculatedStep - (calculatedPoolDiff * poolCount);
            }

            if ((calculatedPoolDiff != CurrentPoolDiff) || (calculatedStep != CurrentStep) || forceRedraw || (lastPoolCount < poolCount))
            {
                LastStep = CurrentStep;
                CurrentStep = calculatedStep;
                CurrentPoolDiff = calculatedPoolDiff;

                var cancellationToken = await GetRenewedCancellationTokenAsync();

                await LayoutInternalItems(cancellationToken);
            }
        }

        /// <summary>
        /// Reset the collection of bound objects
        /// Remove the old collection changed eventhandler (if any)
        /// Create new cells for each new item
        /// </summary>
        /// <param name="bindable">The control</param>
        /// <param name="oldValue">Previous bound collection</param>
        /// <param name="newValue">New bound collection</param>
        private static void ItemsSourceChanged(
            BindableObject bindable,
            object oldValue,
            object newValue)
        {
            var control = bindable as RepeaterRecycleView;

            if (control != null)
            {
                AC.ScheduleManaged(
                    async () =>
                    {
                        INotifyCollectionChanged oldNotifiedCollection = oldValue as INotifyCollectionChanged;

                        if (oldNotifiedCollection != null)
                        {
                            oldNotifiedCollection.CollectionChanged -= control.ItemSource_CollectionChanged;
                        }

                        await control.CancelRenderAsync(control.CleanOnBindingChange);

                        INotifyCollectionChanged newNotifiedCollection = newValue as INotifyCollectionChanged;

                        if (newNotifiedCollection != null)
                        {
                            newNotifiedCollection.CollectionChanged -= control.ItemSource_CollectionChanged;
                            newNotifiedCollection.CollectionChanged += control.ItemSource_CollectionChanged;
                        }

                        await control.UpdateStep(forceRedraw: true);
                    });
            }
        }
    }
}