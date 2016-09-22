// <copyright file="ExtendedEntry.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Xamarin.Forms;

namespace Anuracode.Forms.Controls
{
    /// <summary>
    /// Custom control for extra functionally on the entry.
    /// </summary>
    public class ExtendedEntry : Entry
    {
        /// <summary>
        /// Flag to hide the borders.
        /// </summary>
        public static readonly BindableProperty HasInvisibleBordersProperty = BindablePropertyHelper.Create<ExtendedEntry, bool>(nameof(HasInvisibleBorders), false);

        /// <summary>
        /// Flag to hide the borders.
        /// </summary>
        public bool HasInvisibleBorders
        {
            get
            {
                return (bool)GetValue(HasInvisibleBordersProperty);
            }

            set
            {
                SetValue(HasInvisibleBordersProperty, value);
            }
        }
    }
}
