// <copyright file="ShapeRenderer.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Xamarin.Forms.Platform.WinRT;

// [assembly: ExportRenderer(typeof(Anuracode.Forms.Controls.ShapeView), typeof(Anuracode.Forms.Controls.Renderers.ShapeRenderer))]

namespace Anuracode.Forms.Controls.Renderers
{
    /// <summary>
    /// Shape renderer.
    /// </summary>
    public class ShapeRenderer : ViewRenderer<ShapeView, Border>
    {
        /// <summary>
        /// Transparent brush.
        /// </summary>
        private static Brush transparentBrush = new SolidColorBrush(Windows.UI.Colors.Transparent);

        /// <summary>
        /// Control elipse.
        /// </summary>
        protected Ellipse ControlElipse
        {
            get
            {
                return Control == null ? null : Control.Child as Ellipse;
            }
        }

        /// <summary>
        /// Is circle.
        /// </summary>
        protected bool IsCircle
        {
            get
            {
                return Element == null ? false : Element.ShapeType != ShapeType.Box;
            }
        }

        /// <summary>
        /// Called when [element changed].
        /// </summary>
        /// <param name="e">The e.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<ShapeView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || this.Element == null)
                return;

            bool isCircle = e.NewElement.ShapeType != ShapeType.Box;

            Border border = new Border()
            {
                Background = isCircle ? transparentBrush : e.NewElement.Color.ToBrush(),
                CornerRadius = new CornerRadius(e.NewElement.CornerRadius),
                BorderBrush = isCircle ? transparentBrush : e.NewElement.StrokeColor.ToBrush(),
                BorderThickness = new Thickness(e.NewElement.StrokeWidth)
            };

            Ellipse elipese = new Ellipse()
            {
                Fill = isCircle ? e.NewElement.Color.ToBrush() : transparentBrush,
                Stroke = isCircle ? e.NewElement.StrokeColor.ToBrush() : transparentBrush,
                StrokeThickness = e.NewElement.StrokeWidth
            };

            border.Child = elipese;

            SetNativeControl(border);
        }

        /// <summary>
        /// Element property changed.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Arguments of the event.</param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if ((this.Element == null) || (this.Control == null) || (this.ControlElipse == null))
            {
                return;
            }

            if (e.PropertyName == ShapeView.ColorProperty.PropertyName)
            {
                Control.Background = IsCircle ? transparentBrush : Element.Color.ToBrush();
                ControlElipse.Fill = IsCircle ? Element.Color.ToBrush() : transparentBrush;
            }
            else if (e.PropertyName == ShapeView.CornerRadiusProperty.PropertyName)
            {
                Control.CornerRadius = new CornerRadius(Element.CornerRadius);
            }
            else if (e.PropertyName == ShapeView.StrokeColorProperty.PropertyName)
            {
                Control.BorderBrush = IsCircle ? transparentBrush : Element.StrokeColor.ToBrush();
                ControlElipse.Stroke = IsCircle ? Element.StrokeColor.ToBrush() : transparentBrush;
            }
            else if (e.PropertyName == ShapeView.StrokeWidthProperty.PropertyName)
            {
                Control.BorderThickness = new Thickness(Element.StrokeWidth);
                ControlElipse.StrokeThickness = Element.StrokeWidth;
            }
            else if (e.PropertyName == ShapeView.ShapeTypeProperty.PropertyName)
            {
                Control.BorderBrush = IsCircle ? transparentBrush : Element.StrokeColor.ToBrush();
                ControlElipse.Stroke = IsCircle ? Element.StrokeColor.ToBrush() : transparentBrush;
                Control.Background = IsCircle ? transparentBrush : Element.Color.ToBrush();
                ControlElipse.Fill = IsCircle ? Element.Color.ToBrush() : transparentBrush;
            }
        }
    }
}