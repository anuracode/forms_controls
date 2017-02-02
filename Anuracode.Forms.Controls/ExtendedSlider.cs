// <copyright file="ExtendedSlider.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls
{
    /// <summary>
    /// Better slider.
    /// </summary>
    public class ExtendedSlider : Slider
    {
        /// <summary>
        /// Current step property.
        /// </summary>
        public static readonly BindableProperty StepValueProperty = BindablePropertyHelper.Create<ExtendedSlider, double>(nameof(StepValue), 1f);

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ExtendedSlider()
        {
            ValueChanged += OnSliderValueChanged;
        }

        /// <summary>
        /// Step value.
        /// </summary>
        public double StepValue
        {
            get
            {
                return (double)GetValue(StepValueProperty);
            }

            set
            {
                SetValue(StepValueProperty, value);
            }
        }

        /// <summary>
        /// When slider value changes.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Arguments of the event.</param>
        private void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            var newStep = Math.Round(e.NewValue / StepValue);

            Value = newStep * StepValue;
        }
    }
}