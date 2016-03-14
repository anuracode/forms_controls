// <copyright file="SwitchRenderer.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Anuracode.Forms.Controls.Renderers
{
    /// <summary>
    /// Custom renderer.
    /// </summary>
    public class SwitchRenderer : Xamarin.Forms.Platform.Android.SwitchRenderer
    {
        /// <summary>
        /// Element changed.
        /// </summary>
        /// <param name="e">Arguments of the event.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Switch> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                bool changed = false;

                if (!string.IsNullOrWhiteSpace(Control.TextOff))
                {
                    Control.TextOff = string.Empty;
                    changed = true;
                }

                if (!string.IsNullOrWhiteSpace(Control.TextOn))
                {
                    Control.TextOn = string.Empty;
                    changed = true;
                }

                if (!string.IsNullOrWhiteSpace(Control.Text))
                {
                    Control.Text = string.Empty;
                    changed = true;
                }

                if (changed)
                {
                    ((IVisualElementController)Element).NativeSizeChanged();
                }
            }
        }
    }
}