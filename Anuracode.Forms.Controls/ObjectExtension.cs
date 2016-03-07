// <copyright file="ObjectExtension.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

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
            return Task.Run(() => list.Count());
        }

        /// <summary>
        /// Verify if the layout is different before update.
        /// </summary>
        /// <param name="view">View to use.</param>
        /// <param name="elementPosition">Position to use.</param>
        public static void LayoutUpdate(this View view, Rectangle elementPosition)
        {
            if ((view.X != elementPosition.X) || (view.Y != elementPosition.Y) || (view.Width != elementPosition.Width) || (view.Height != elementPosition.Height))
            {
                view.Layout(elementPosition);
            }
        }

        /// <summary>
        /// Platform delegate.
        /// </summary>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="os">Os to use.</param>
        /// <param name="iOS">Delegate to use.</param>
        /// <param name="android">Delegate to use.</param>
        /// <param name="windowsPhone">Delegate to use.</param>
        /// <param name="windows">Delegate to use.</param>
        /// <param name="other">Delegate to use.</param>
        /// <returns>Value to use.</returns>
        public static TValue OnPlatform<TValue>(this TargetPlatform os, TValue iOS, TValue android, TValue windowsPhone, TValue windows, TValue other = default(TValue))
        {
            TValue returnValue = default(TValue);

            switch (os)
            {
                case TargetPlatform.Android:
                    returnValue = android;
                    break;

                case TargetPlatform.WinPhone:
                    returnValue = windowsPhone;
                    break;

                case TargetPlatform.Windows:
                    returnValue = windows;
                    break;

                case TargetPlatform.iOS:
                    returnValue = iOS;
                    break;

                default:
                    returnValue = other;
                    break;
            }

            return returnValue;
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
        /// Update is visible.
        /// </summary>
        /// <param name="view">View to use.</param>
        /// <param name="isVisible">Is visible value.</param>
        public static void UpdateIsVisible(this View view, bool isVisible)
        {
            if ((view != null) && (view.IsVisible != isVisible))
            {
                AC.ScheduleManaged(
                    () =>
                    {
                        view.IsVisible = isVisible;
                    });
            }
        }

        /// <summary>
        /// Update is visible.
        /// </summary>
        /// <param name="view">View to use.</param>
        /// <param name="newOpacity">Opacity value.</param>
        public static void UpdateOpacity(this View view, double newOpacity)
        {
            if ((view != null) && (view.Opacity != newOpacity))
            {
                view.Opacity = newOpacity;
            }
        }
    }
}