// <copyright file="ColorExtensions.cs" company="">
// All rights reserved.
// </copyright>
// <author>https://github.com/XLabs/Xamarin-Forms-Labs</author>

using Windows.UI.Xaml.Media;

namespace Anuracode.Forms.Controls.Renderers
{
    /// <summary>
    /// Class ColorExtensions.
    /// </summary>
    public static class ColorExtensions
    {
        public static Brush ToBrush(this Xamarin.Forms.Color color)
        {
            return new SolidColorBrush(color.ToMediaColor());
        }

        /// <summary>
        /// To media color.
        /// </summary>
        /// <param name="color">Color to use.</param>
        /// <returns>Color converted.</returns>
        public static Windows.UI.Color ToMediaColor(this Xamarin.Forms.Color color)
        {
            return Windows.UI.Color.FromArgb((byte)(color.A * 255.0), (byte)(color.R * 255.0), (byte)(color.G * 255.0), (byte)(color.B * 255.0));
        }
    }
}