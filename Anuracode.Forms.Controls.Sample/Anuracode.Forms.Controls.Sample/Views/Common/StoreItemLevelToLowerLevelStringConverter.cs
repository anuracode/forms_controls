// <copyright file="StoreItemLevelToLowerLevelStringConverter.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Sample.Model;
using System;

namespace Anuracode.Forms.Controls.Sample.Views.Common
{
    /// <summary>
    /// Converter from StoreItemLevel to string of the lowest clasifier.
    /// </summary>
    public class StoreItemLevelToLowerLevelStringConverter : BaseConverter
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
            string returnValue = string.Empty;

            StoreItemLevel typedValue = value as StoreItemLevel;

            if (typedValue != null)
            {
                if (!string.IsNullOrEmpty(typedValue.Subcategory))
                {
                    returnValue = typedValue.Subcategory;
                }
                else if (!string.IsNullOrEmpty(typedValue.Category))
                {
                    returnValue = typedValue.Category;
                }
                else if (!string.IsNullOrEmpty(typedValue.Department))
                {
                    returnValue = typedValue.Department;
                }
            }

            return returnValue;
        }
    }
}