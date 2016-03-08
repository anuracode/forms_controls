// <copyright file="IntToBooleanConverter.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System;

namespace Anuracode.Forms.Controls.Sample.Views.Common
{
    /// <summary>
    /// Converter from int to boolean.
    /// </summary>
    public class IntToBooleanConverter : BaseConverter
    {
        /// <summary>
        /// Convert a value from one type to another.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="targetType">Target type of the value.</param>
        /// <param name="parameter">Parameter for conversion (not used).</param>
        /// <param name="culture">Culture to use.</param>
        /// <returns>Value converted.</returns>
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool returnValue = (value is int) && ((int)value > 0);

            return returnValue;
        }
    }
}