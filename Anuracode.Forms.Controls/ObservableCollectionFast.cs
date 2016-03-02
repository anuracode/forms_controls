// <copyright file="ObservableCollectionFast.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Anuracode.Forms.Controls
{
    /// <summary>
    /// Special observable collection to use with xamarin.forms.
    /// </summary>
    /// <typeparam name="T"></typeparam>    
    public class ObservableCollectionFast<T> : ObservableCollection<T>
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public ObservableCollectionFast()
            : base()
        {
        }

        /// <summary>
        /// Constructor with range.
        /// </summary>
        /// <param name="collection">Collection to use.</param>
        public ObservableCollectionFast(IEnumerable<T> collection)
            : base(collection)
        {
        }

        /// <summary>
        /// Constructor with range.
        /// </summary>
        /// <param name="list">List to use.</param>
        public ObservableCollectionFast(List<T> list)
            : base(list)
        {
        }

        /// <summary>
        /// Event when the item count changes.
        /// </summary>
        public event EventHandler<EventArgs<int>> ItemCountChanged;

        /// <summary>
        /// Add range.
        /// </summary>
        /// <param name="range">Range to use.</param>
        public virtual void AddRange(IEnumerable<T> range)
        {
            if (range != null)
            {
                foreach (var item in range)
                {
                    Items.Add(item);
                }

                AC.ScheduleManaged(
                   async () =>
                   {
                       await Task.FromResult(0);
                       OnItemCountChanged();
                       this.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
                       this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
                       this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                   });
            }
        }

        /// <summary>
        /// Clear items.
        /// </summary>
        public new void Clear()
        {
            try
            {
                if (Items.Count > 0)
                {
                    base.Clear();
                }
            }
            finally
            {
                AC.ScheduleManaged(
                    async () =>
                    {
                        await Task.FromResult(0);
                        OnItemCountChanged();
                    });
            }
        }

        /// <summary>
        /// Clear items an notify change.
        /// </summary>
        public virtual void ClearAndNotify()
        {
            this.Items.Clear();

            AC.ScheduleManaged(
                   async () =>
                   {
                       await Task.FromResult(0);
                       OnItemCountChanged();
                       this.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
                       this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
                       this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                   });
        }

        /// <summary>
        /// Clear list and add range.
        /// </summary>
        /// <param name="range">Range to use.</param>
        public void Reset(IEnumerable<T> range)
        {
            this.Items.Clear();

            AddRange(range);
        }

        /// <summary>
        /// Item count changes.
        /// </summary>
        protected void OnItemCountChanged()
        {
            if (ItemCountChanged != null)
            {
                ItemCountChanged(this, new EventArgs<int>(this.Items.Count));
            }
        }
    }
}