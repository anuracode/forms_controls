// <copyright file="ManagedImageRenderer.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Anuracode.Forms.Controls.ExtendedImage), typeof(Anuracode.Forms.Controls.Renderers.ManagedImageRenderer))]

namespace Anuracode.Forms.Controls.Renderers
{
    /// <summary>
    /// Image renderer managed.
    /// </summary>
    public class ManagedImageRenderer : ImageRenderer
    {
        /// <summary>
        /// On element changed.
        /// </summary>
        /// <param name="e">Arguments of the event.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            try
            {
                base.OnElementChanged(e);
            }
            catch (Exception ex)
            {
                AC.TraceError("ManagedImageRenderer problem: {0}", ex);
            }
        }

        /// <summary>
        /// Property changed.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Arguments of the event.</param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (this.Control == null || this.Element == null)
            {
                return;
            }

            try
            {
                base.OnElementPropertyChanged(sender, e);
            }
            catch (Exception ex)
            {
                AC.TraceError("ManagedImageRenderer problem: {0}", ex);
            }
        }
    }
}