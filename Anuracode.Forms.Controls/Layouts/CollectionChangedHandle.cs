// <copyright file="ContentViewButton.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls
{
    /// <summary>
    /// Small utility class that takes
    /// gyuwon's idea to it's logical
    /// conclusion.
    /// The code in the ItemsCollectionChanged methods
    /// rarely changes.  The only real change is projecting
    /// from source type T to targeted type TSyncType which
    /// is then inserted into the target collection
    /// </summary>
    public class CollectionChangedHandle<TSyncType, T> : IDisposable
        where T : class
        where TSyncType : class
    {
        /// <summary>
        /// Semaphore for the repository file.
        /// </summary>
        private static SemaphoreSlim lockUI;

        /// <summary>
        /// Cleanup action.
        /// </summary>
        private readonly Action<TSyncType> cleanupAction;

        /// <summary>
        /// Action to notify that is loading.
        /// </summary>
        private readonly Action<bool> isLoadingAction;

        /// <summary>
        /// Flag to use the LockUI.
        /// </summary>
        private readonly bool isUILockable;

        /// <summary>
        /// Item srouce collection changed.
        /// </summary>
        private readonly INotifyCollectionChanged itemsSourceCollectionChangedImplementation;

        /// <summary>
        /// Parent view.
        /// </summary>
        private readonly View parentView;

        /// <summary>
        /// Post add action.
        /// </summary>
        private readonly Action<TSyncType, T, int> postaddAction;

        /// <summary>
        /// Proyecto to use.
        /// </summary>
        private readonly Func<T, Task<TSyncType>> projector;

        /// <summary>
        /// Source collection.
        /// </summary>
        private readonly IEnumerable<T> sourceCollection;

        /// <summary>
        /// Target collection.
        /// </summary>
        private readonly IList<TSyncType> target;

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionChangedHandle{TSyncType,T}"/> class.
        /// </summary>
        /// <param name="parentView">Parent view.</param>
        /// <param name="target">The collection to be kept in sync with <see cref="source"/>source</param>
        /// <param name="source">The original collection</param>
        /// <param name="projector">A function that returns {TSyncType} for a {T}</param>
        /// <param name="postaddAction">A functino called right after insertion into the synced collection</param>
        /// <param name="cleanupAction">A function that performs any needed cleanup when {TSyncType} is removed from the <see cref="target"/></param>
        /// <param name="isUILockable">Is UI locable.</param>
        public CollectionChangedHandle(
            View parentView,
            IList<TSyncType> target,
            IEnumerable<T> source,
            Func<T, Task<TSyncType>> projector,
            Action<TSyncType, T, int> postaddAction = null,
            Action<TSyncType> cleanupAction = null,
            Action<bool> isLoadingAction = null,
            bool isUILockable = true)
        {
            this.parentView = parentView;
            this.isUILockable = isUILockable;

            if (source == null)
            {
                return;
            }

            this.itemsSourceCollectionChangedImplementation = source as INotifyCollectionChanged;
            this.sourceCollection = source;
            this.target = target;
            this.projector = projector;
            this.postaddAction = postaddAction;
            this.cleanupAction = cleanupAction;
            this.isLoadingAction = isLoadingAction;

            Task initTask = this.InitialPopulation();

            if (this.itemsSourceCollectionChangedImplementation == null)
            {
                return;
            }

            this.itemsSourceCollectionChangedImplementation.CollectionChanged += this.CollectionChanged;
        }

        /// <summary>
        /// Semaphore for the repository file.
        /// </summary>
        public static SemaphoreSlim LockUI
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
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (this.itemsSourceCollectionChangedImplementation == null) return;
            this.itemsSourceCollectionChangedImplementation.CollectionChanged -= this.CollectionChanged;
        }

        /// <summary>Keeps <see cref="target"/> in sync with <see cref="sourceCollection"/>.</summary>
        /// <param name="sender">The sender, completely ignored.</param>
        /// <param name="args">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        /// Element created at 15/11/2014,2:57 PM by Charles
        private async void CollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.Action == NotifyCollectionChangedAction.Reset)
            {
                await InitialPopulation();
            }
            else
            {
                //Create a temp list to prevent multiple enumeration issues
                var tlist = await sourceCollection.ToListAsync();

                if (args.OldItems != null)
                {
                    var syncitem = target[args.OldStartingIndex];
                    if (syncitem != null && cleanupAction != null)
                    {
                        cleanupAction(syncitem);
                    }

                    target.RemoveAt(args.OldStartingIndex);
                }

                if (args.NewItems == null) return;

                try
                {
                    View view = null;

                    if (isLoadingAction != null)
                    {
                        isLoadingAction(true);
                    }

                    if (isUILockable)
                    {
                        await LockUI.WaitAsync();
                    }

                    foreach (var obj in args.NewItems)
                    {
                        var item = obj as T;
                        if (item != null)
                        {
                            var index = tlist.IndexOf(item);

                            await Task.Delay(6);

                            var newsyncitem = await this.projector(item).ConfigureAwait(true);

                            await Task.Delay(12);

                            if (view != null)
                            {
                                view.Opacity = 0;
                            }

                            this.target.Insert(index, newsyncitem);
                            if (postaddAction != null)
                            {
                                postaddAction(newsyncitem, item, index);
                            }

                            if (view != null)
                            {
                                view.Opacity = 1;
                            }

                            await Task.Delay(6);
                        }
                    }
                }
                finally
                {
                    if (isUILockable)
                    {
                        LockUI.Release();
                    }

                    if (isLoadingAction != null)
                    {
                        isLoadingAction(false);
                    }
                }
            }
        }

        /// <summary>Initials the population.</summary>
        /// Element created at 15/11/2014,2:53 PM by Charles
        private async Task InitialPopulation()
        {
            await SafeClearTarget();
            TSyncType tmpProyection;

            var filteredList = await this.sourceCollection.Where(x => x != null).ToListAsync();

            View view = null;

            try
            {
                if (isLoadingAction != null)
                {
                    isLoadingAction(true);
                }

                if (isUILockable)
                {
                    await LockUI.WaitAsync();
                }

                foreach (var t in filteredList)
                {
                    tmpProyection = await this.projector(t);

                    await Task.Delay(15);

                    view = tmpProyection as View;

                    if (view != null)
                    {
                        view.Opacity = 0;
                    }

                    target.Add(tmpProyection);

                    if (view != null)
                    {
                        view.Opacity = 1;
                    }

                    await Task.Delay(15);
                }
            }
            finally
            {
                if (isUILockable)
                {
                    LockUI.Release();
                }

                if (isLoadingAction != null)
                {
                    isLoadingAction(false);
                }
            }
        }

        /// <summary>
        /// Safe clear target.
        /// </summary>
        private Task SafeClearTarget()
        {
            while (target.Count > 0)
            {
                var syncitem = target[0];
                target.RemoveAt(0);
                if (cleanupAction != null)
                {
                    cleanupAction(syncitem);
                }
            }

            return Task.FromResult(0);
        }
    }
}