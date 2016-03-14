// <copyright file="ExtendedEntryRenderer.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

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

            if ((this.Element == null) || (this.Control == null))
            {
                return;
            }

            var toolbar = new UIToolbar(new CGRect(0.0f, 0.0f, Control.Frame.Size.Width, 44.0f));

            toolbar.Items = new[]
            {
                new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
                new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate { Control.ResignFirstResponder(); })
            };

            this.Control.KeyboardAppearance = UIKeyboardAppearance.Dark;
            this.Control.InputAccessoryView = toolbar;
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
                Control.BorderStyle = extendedEntry.HasInvisibleBorders ? UITextBorderStyle.None : UITextBorderStyle.RoundedRect;
            }
        }
    }
}
