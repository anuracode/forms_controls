// <copyright file="ExtendedEntryRenderer.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Xamarin.Forms;

#if WINDOWS_UWP

using Xamarin.Forms.Platform.UWP;

#else

using Xamarin.Forms.Platform.WinRT;

#endif

[assembly: ExportRenderer(typeof(Anuracode.Forms.Controls.ExtendedEntry), typeof(Anuracode.Forms.Controls.Renderers.ExtendedEntryRenderer))]

namespace Anuracode.Forms.Controls.Renderers
{
    /// <summary>
    /// Extended entry renderer.
    /// </summary>
    public class ExtendedEntryRenderer : EntryRenderer
    {
        /// <summary>
        /// When element changed.
        /// </summary>
        /// <param name="e">Argument to use.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            UpdateBorderThickness();
        }

        /// <summary>
        /// Event when element property changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if ((this.Element == null) || (this.Control == null))
            {
                return;
            }

            if ((e.PropertyName == ExtendedEntry.HasInvisibleBordersProperty.PropertyName))
            {
                UpdateBorderThickness();
            }
        }

        /// <summary>
        /// Update border thickness.
        /// </summary>
        protected void UpdateBorderThickness()
        {
            if ((this.Element == null) || (this.Control == null))
            {
                return;
            }

            ExtendedEntry extendedEntry = Element as ExtendedEntry;

            if (extendedEntry != null)
            {
                if (extendedEntry.HasInvisibleBorders)
                {
                    this.Control.BorderThickness = new Windows.UI.Xaml.Thickness(0);
                }
            }
        }
    }
}