// <copyright file="InvertBooleanToBooleanConverter.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

namespace Anuracode.Forms.Controls.Sample.Views.Common
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Converter from boolean to invert visibility.
    /// </summary>
    public class InvertBooleanToBooleanConverter : BaseConverter
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
            object returnValue = false;

            if (value is bool)
            {
                bool originalValue = (bool)value;
                returnValue = !originalValue;
            }

            return returnValue;
        }

        /// <summary>
        /// Convert a value from one type to another.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="targetType">Target type of the value.</param>
        /// <param name="parameter">Parameter for conversion (not used).</param>
        /// <param name="culture">Culture to use.</param>
        /// <returns>Value converted.</returns>
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }
}