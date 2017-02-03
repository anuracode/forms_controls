// <copyright file="ViewExtensions.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Xamarin.Forms;

namespace Anuracode.Forms.Controls.Views.Extensions
{
    /// <summary>
    /// View extension.
    /// </summary>
    public static class ViewExtensions
    {
        /// <summary>
        /// Verify if the layout is different before update.
        /// </summary>
        /// <param name="view">View to use.</param>
        /// <param name="elementPosition">Position to use.</param>
        public static void LayoutUpdate(this View view, Rectangle elementPosition)
        {
            if ((view != null) && (view.X != elementPosition.X) || (view.Y != elementPosition.Y) || (view.Width != elementPosition.Width) || (view.Height != elementPosition.Height))
            {
                view.Layout(elementPosition);
            }
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