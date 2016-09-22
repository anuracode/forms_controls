// <copyright file="RepeaterRecycleView.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Xamarin.Forms;
using static Xamarin.Forms.BindableProperty;

namespace Anuracode.Forms.Controls
{
    /// <summary>
    /// Helper for the bindiable properties.
    /// </summary>
    public static class BindablePropertyHelper
    {
        /// <summary>
        /// Create bindinble property simpler.
        /// </summary>
        /// <typeparam name="TDeclarer">Type of the declarer.</typeparam>
        /// <typeparam name="TPropertyType">Type of the property.</typeparam>
        /// <param name="propertyName">Property name.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <param name="defaultBindingMode">Default binding mode.</param>
        /// <param name="validateValue">Validate value.</param>
        /// <param name="propertyChanged">Property changed delegate.</param>
        /// <param name="propertyChanging">Property changing delegate.</param>
        /// <param name="coerceValue">Coerce value.</param>
        /// <param name="defaultValueCreator">Default value creator.</param>
        /// <returns>Bindable property.</returns>
        public static BindableProperty Create<TDeclarer, TPropertyType>(
            string propertyName,
            TPropertyType defaultValue = default(TPropertyType),
            BindingMode defaultBindingMode = BindingMode.OneWay,
            ValidateValueDelegate validateValue = null,
            BindingPropertyChangedDelegate propertyChanged = null,
            BindingPropertyChangingDelegate propertyChanging = null,
            CoerceValueDelegate coerceValue = null,
            CreateDefaultValueDelegate defaultValueCreator = null)
        {
            return BindableProperty.Create(
                propertyName,
                typeof(TPropertyType),
                typeof(TDeclarer),
                defaultValue: defaultValue,
                defaultBindingMode: defaultBindingMode,
                validateValue: validateValue,
                propertyChanged: propertyChanged,
                propertyChanging: propertyChanging,
                coerceValue: coerceValue,
                defaultValueCreator: defaultValueCreator);
        }
    }
}