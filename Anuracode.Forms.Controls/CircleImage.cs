// <copyright file="CircleImage.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Xamarin.Forms;

namespace Anuracode.Forms.Controls
{
    /// <summary>
    /// ImageCircle Interface
    /// </summary>
    public class CircleImage : Image
    {
        /// <summary>
        /// Color property of border
        /// </summary>
        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create<CircleImage, Color>(p => p.BorderColor, Color.White);

        /// <summary>
        /// Thickness property of border
        /// </summary>
        public static readonly BindableProperty BorderThicknessProperty = BindableProperty.Create<CircleImage, int>(p => p.BorderThickness, 0);

        /// <summary>
        /// Border Color of circle image
        /// </summary>
        public Color BorderColor
        {
            get
            {
                return (Color)GetValue(BorderColorProperty);
            }

            set
            {
                SetValue(BorderColorProperty, value);
            }
        }

        /// <summary>
        /// Border thickness of circle image
        /// </summary>
        public int BorderThickness
        {
            get
            {
                return (int)GetValue(BorderThicknessProperty);
            }

            set
            {
                SetValue(BorderThicknessProperty, value);
            }
        }
    }
}