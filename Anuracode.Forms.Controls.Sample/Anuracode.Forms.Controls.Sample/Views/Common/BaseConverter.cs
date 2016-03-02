// <copyright file="BaseConverter.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls.Sample.Views.Common
{
    /// <summary>
    /// Base for all the converters.
    /// </summary>
    public abstract class BaseConverter : IValueConverter
    {
        /// <summary>
        /// Convert a value from one type to another.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="targetType">Target type of the value.</param>
        /// <param name="parameter">Parameter for conversion (not used).</param>
        /// <param name="culture">Culture to use.</param>
        /// <returns>Value converted.</returns>
        public abstract object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture);

        /// <summary>
        /// Convert a value from one type to another.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="targetType">Target type of the value.</param>
        /// <param name="parameter">Parameter for conversion (not used).</param>
        /// <param name="culture">Culture to use.</param>
        /// <returns>Value converted.</returns>
        public virtual object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}