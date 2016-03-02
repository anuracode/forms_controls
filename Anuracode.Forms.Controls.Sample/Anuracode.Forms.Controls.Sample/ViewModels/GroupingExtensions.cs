// <copyright file="GroupingExtensions.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anuracode.Forms.Controls.Sample.ViewModels
{
    /// <summary>
    /// Grouping extensions.
    /// </summary>
    public static class GroupingExtensions
    {
        /// <summary>
        /// Add the element if is not already in the list.
        /// </summary>
        /// <typeparam name="T">Type to use.</typeparam>
        /// <param name="list">List to use.</param>
        /// <param name="newElement">Element to add.</param>
        public static void AddNonDuplicate<T>(this List<T> list, T newElement)
        {
            if ((list != null) && (newElement != null) && list.IndexOf(newElement) == -1)
            {
                list.Add(newElement);
            }
        }

        /// <summary>
        /// Async create of the count from an
        /// IEnumerable<T>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="list">The System.Collections.Generic.IEnumerable<T>
        /// to create a System.Collections.Generic.List<T> from.</param>
        /// <returns> Count of the items.</returns>
        public static Task<int> CountAsync<T>(this IEnumerable<T> list)
        {
            return Task.Run(() => list.Count());
        }

        /// <summary>
        /// First or default.
        /// </summary>
        /// <typeparam name="T">Type to use.</typeparam>
        /// <param name="list">List to use.</param>
        /// <returns>Task to await.</returns>
        public static Task<T> FirstOrDefaultAsync<T>(this IEnumerable<T> list)
        {
            return Task.Run(() => list.FirstOrDefault<T>());
        }

        /// <summary>
        /// First or default.
        /// </summary>
        /// <typeparam name="T">Type to use.</typeparam>
        /// <param name="list">List to use.</param>
        /// <param name="predicate">Predicate to use.</param>
        /// <returns>Task to await.</returns>
        public static Task<T> FirstOrDefaultAsync<T>(this IEnumerable<T> list, Func<T, bool> predicate)
        {
            return Task.Run(() => list.FirstOrDefault<T>(predicate));
        }

        /// <summary>
        /// For easy for lists.
        /// </summary>
        /// <typeparam name="T">Type to use.</typeparam>
        /// <param name="list">List to use.</param>
        /// <param name="actionDelegate">Action delegate.</param>
        public static void ForEach<T>(this IList<T> list, Action<T> actionDelegate)
        {
            if ((list != null) && (actionDelegate != null))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    actionDelegate(list[i]);
                }
            }
        }

        /// <summary>
        /// Merge group items.
        /// </summary>
        /// <typeparam name="TItem">Type of the item.</typeparam>
        /// <param name="currentItems">Destination group.</param>
        /// <param name="newItems">New items.</param>
        /// <param name="comparer">Comprar for the element.</param>
        public static void Merge<TItem>(this ICollection<TItem> currentItems, IEnumerable<TItem> newItems)
            where TItem : class
        {
            if (newItems != null)
            {
                currentItems.UnionRange(newItems, new GenericCompare<TItem>());
            }
        }

        /// <summary>
        /// Sum items.
        /// </summary>
        /// <typeparam name="T">Type to use.</typeparam>
        /// <param name="list">List to use.</param>
        /// <param name="selector">Selector to use.</param>
        /// <returns></returns>
        public static Task<decimal> SumAsync<T>(this IEnumerable<T> list, Func<T, decimal> selector)
        {
            return Task.Run(() => list.Sum<T>(selector));
        }

        /// <summary>
        /// Sum items.
        /// </summary>
        /// <typeparam name="T">Type to use.</typeparam>
        /// <param name="list">List to use.</param>
        /// <param name="selector">Selector to use.</param>
        /// <returns></returns>
        public static Task<int> SumAsync<T>(this IEnumerable<T> list, Func<T, int> selector)
        {
            return Task.Run(() => list.Sum<T>(selector));
        }

        /// <summary>
        /// Sum items.
        /// </summary>
        /// <typeparam name="T">Type to use.</typeparam>
        /// <param name="list">List to use.</param>
        /// <param name="selector">Selector to use.</param>
        /// <returns></returns>
        public static Task<double> SumAsync<T>(this IEnumerable<T> list, Func<T, double> selector)
        {
            return Task.Run(() => list.Sum<T>(selector));
        }

        /// <summary>
        /// Async create of a System.Collections.Generic.List<T> from an
        /// IEnumerable<T>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="list">The System.Collections.Generic.IEnumerable<T>
        /// to create a System.Collections.Generic.List<T> from.</param>
        /// <returns> A System.Collections.Generic.List<T> that contains elements
        /// from the input sequence.</returns>
        public static Task<List<T>> ToListAsync<T>(this IEnumerable<T> list)
        {
            return Task.Run(() => list.ToList());
        }

        /// <summary>
        /// Return a list from an element.
        /// </summary>
        /// <typeparam name="T">Type of the element.</typeparam>
        /// <param name="element">Element to use.</param>
        /// <returns>List with the element.</returns>
        public static List<T> ToOneItemList<T>(this T element)
        {
            List<T> returnList = new List<T>();

            returnList.Add(element);

            return returnList;
        }

        /// <summary>
        /// Union async.
        /// </summary>
        /// <typeparam name="T">Type to use.</typeparam>
        /// <param name="list">List to use.</param>
        /// <param name="otherlist">Other list to use.</param>
        /// <param name="comparer"></param>
        /// <returns>Task to await.</returns>
        public static Task<IEnumerable<T>> UnionAsync<T>(this IEnumerable<T> list, IEnumerable<T> otherlist, IEqualityComparer<T> comparer)
        {
            return Task.Run(() => list.Union<T>(otherlist, comparer));
        }

        /// <summary>
        /// Add range.
        /// </summary>
        /// <typeparam name="T">Type to use.</typeparam>
        /// <param name="destination">Destination to use.</param>
        /// <param name="source">Srouce to use.</param>
        /// <param name="comparer">Comprar for the element.</param>
        public static void UnionRange<T>(this ICollection<T> destination, IEnumerable<T> source, IEqualityComparer<T> comparer)
        {
            foreach (T item in source)
            {
                if (!destination.Contains(item, comparer))
                {
                    destination.Add(item);
                }
            }
        }
    }
}