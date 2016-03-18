// <copyright file="ImageCircleRenderer.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System;
using System.ComponentModel;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Anuracode.Forms.Controls.CircleImage), typeof(Anuracode.Forms.Controls.Renderers.ImageCircleRenderer))]

namespace Anuracode.Forms.Controls.Renderers
{
    /// <summary>
    /// ImageCircle Implementation
    /// </summary>
    public class ImageCircleRenderer : ImageRenderer
    {
        /// <summary>
        /// Element changed.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);
            if (Element == null)
            {
                return;
            }

            CreateCircle();
        }

        /// <summary>
        /// On element property changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == VisualElement.HeightProperty.PropertyName ||
                e.PropertyName == VisualElement.WidthProperty.PropertyName ||
              e.PropertyName == CircleImage.BorderColorProperty.PropertyName ||
              e.PropertyName == CircleImage.BorderThicknessProperty.PropertyName)
            {
                CreateCircle();
            }
        }

        /// <summary>
        /// Create circle.
        /// </summary>
        private void CreateCircle()
        {
            try
            {
                double min = Math.Min(Element.Width, Element.Height);
                Control.Layer.CornerRadius = (float)(min / 2.0);
                Control.Layer.MasksToBounds = false;
                Control.Layer.BorderColor = ((CircleImage)Element).BorderColor.ToCGColor();
                Control.Layer.BorderWidth = ((CircleImage)Element).BorderThickness;
                Control.ClipsToBounds = true;
            }
            catch (Exception ex)
            {
                AC.TraceError("Unable to create circle image: ", ex);
            }
        }
    }
}