// <copyright file="ObjectExtension.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anuracode.Forms.Controls.Extensions
{
    /// <summary>
    /// Object extension.
    /// </summary>
    public static class ObjectExtension
    {
        /// <summary>
        /// Returns the values withing the range.
        /// </summary>
        /// <typeparam name="T">Type to use.</typeparam>
        /// <param name="val">Value to use.</param>
        /// <param name="min">Min value to use.</param>
        /// <param name="max">Max value to use.</param>
        /// <returns></returns>
        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
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
            TaskCompletionSource<int> tc = new TaskCompletionSource<int>();

            AC.ScheduleManagedBackground(
                async () =>
                {
                    await Task.FromResult(0);

                    try
                    {
                        var result = list.Count();
                        tc.TrySetResult(result);
                    }
                    catch (Exception ex)
                    {
                        tc.TrySetException(ex);
                    }
                });

            return tc.Task;
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
            TaskCompletionSource<List<T>> tc = new TaskCompletionSource<List<T>>();

            AC.ScheduleManagedBackground(
                async () =>
                {
                    await Task.FromResult(0);

                    try
                    {
                        var result = list.ToList();
                        tc.TrySetResult(result);
                    }
                    catch (Exception ex)
                    {
                        tc.TrySetException(ex);
                    }
                });

            return tc.Task;
        }
    }
}