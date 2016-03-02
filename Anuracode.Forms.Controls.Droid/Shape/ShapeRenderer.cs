// <copyright file="ShapeRenderer.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Anuracode.Forms.Controls.ShapeView), typeof(Anuracode.Forms.Controls.Renderers.ShapeRenderer))]

namespace Anuracode.Forms.Controls.Renderers
{
    /// <summary>
    /// Shape renderer.
    /// </summary>
    public class ShapeRenderer : ViewRenderer<ShapeView, Shape>
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public ShapeRenderer()
        {
        }

        /// <summary>
        /// Element changed.
        /// </summary>
        /// <param name="e">Arguments of the event.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<ShapeView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || this.Element == null)
            {
                return;
            }

            var shape = new Shape(Resources.DisplayMetrics.Density, Context)
            {
                ShapeView = Element
            };

            shape.SetPadding(0, 0, 0, 0);

            SetNativeControl(shape);
        }

        /// <summary>
        /// Element property changed.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Arguments of the event.</param>
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if ((this.Element == null) || (this.Control == null))
            {
                return;
            }

            if (e.PropertyName == ShapeView.ColorProperty.PropertyName)
            {
                Control.PostInvalidate();
            }
            else if (e.PropertyName == ShapeView.CornerRadiusProperty.PropertyName)
            {
                Control.PostInvalidate();
            }
            else if (e.PropertyName == ShapeView.StrokeColorProperty.PropertyName)
            {
                Control.PostInvalidate();
            }
            else if (e.PropertyName == ShapeView.StrokeWidthProperty.PropertyName)
            {
                Control.PostInvalidate();
            }
            else if (e.PropertyName == ShapeView.ShapeTypeProperty.PropertyName)
            {
                Control.PostInvalidate();
            }
        }
    }
}