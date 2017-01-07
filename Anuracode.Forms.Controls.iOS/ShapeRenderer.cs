// <copyright file="ShapeRenderer.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using CoreGraphics;
using System;
using System.Drawing;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Anuracode.Forms.Controls.Renderers
{
    /// <summary>
    /// Shape renderer.
    /// </summary>
    public class ShapeRenderer : VisualElementRenderer<ShapeView>
    {
        /// <summary>
        /// Previous size.
        /// </summary>
        private CGSize previousSize;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ShapeRenderer()
        {
        }

        /// <summary>
        /// Draw element.
        /// </summary>
        /// <param name="rect">Rect to use.</param>
        public override void Draw(CGRect rect)
        {
            var properRect = AdjustForThickness(rect);

            using (var currentContext = UIGraphics.GetCurrentContext())
            {
                HandleShapeDraw(currentContext, properRect);
            }

            base.Draw(rect);

            this.previousSize = this.Bounds.Size;
        }

        /// <summary>
        /// Layout subviews.
        /// </summary>
        public override void LayoutSubviews()
        {
            if (this.previousSize != this.Bounds.Size)
            {
                this.SetNeedsDisplay();
            }
        }

        /// <summary>
        /// Adjust thickness.
        /// </summary>
        /// <param name="rect">Rect to use.</param>
        /// <returns>Rect adjusted.</returns>
        protected RectangleF AdjustForThickness(CGRect rect)
        {
            return new RectangleF((float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height);
        }

        /// <summary>
        /// Draw shape.
        /// </summary>
        /// <param name="currentContext">Context to use.</param>
        /// <param name="rect">Rect to use.</param>
        protected virtual void HandleShapeDraw(CGContext currentContext, RectangleF rect)
        {
            float paddingBorder = Element.StrokeWidth * 0.5f;
            RectangleF rectBox = new RectangleF(rect.X + paddingBorder, rect.Y + paddingBorder, rect.Width - (paddingBorder * 2f), rect.Height - (paddingBorder * 2f));

            switch (Element.ShapeType)
            {
                case ShapeType.Box:
                    HandleStandardDraw(currentContext, rect,
                        () =>
                        {
                            if (Element.CornerRadius > 0)
                            {
                                var path = UIBezierPath.FromRoundedRect(rectBox, Element.CornerRadius);
                                currentContext.AddPath(path.CGPath);
                            }
                            else
                            {
                                currentContext.AddRect(rect);
                            }
                        });
                    break;

                case ShapeType.Circle:
                    HandleStandardDraw(currentContext, rect,
                        () =>
                        {
                            var path = UIBezierPath.FromOval(rectBox);
                            currentContext.AddPath(path.CGPath);
                        });
                    break;
            }
        }

        /// <summary>
        /// A simple method for handling our drawing of the shape. This method is called differently for each type of shape
        /// </summary>
        /// <param name="currentContext">Current context.</param>
        /// <param name="rect">Rect.</param>
        /// <param name="createPathForShape">Create path for shape.</param>
        /// <param name="lineWidth">Line width.</param>
        protected virtual void HandleStandardDraw(CGContext currentContext, RectangleF rect, Action createPathForShape, float? lineWidth = null)
        {
            currentContext.SetLineWidth(lineWidth ?? Element.StrokeWidth);
            currentContext.SetFillColor(base.Element.Color.ToCGColor());
            currentContext.SetStrokeColor(Element.StrokeColor.ToCGColor());

            createPathForShape();

            currentContext.DrawPath(CGPathDrawingMode.FillStroke);
        }

        /// <summary>
        /// Element property changed.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Arguments of the event.</param>
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (this.Element == null)
            {
                return;
            }

            if ((e.PropertyName == ShapeView.ColorProperty.PropertyName) ||
                (e.PropertyName == ShapeView.CornerRadiusProperty.PropertyName) ||
                (e.PropertyName == ShapeView.StrokeColorProperty.PropertyName) ||
                (e.PropertyName == ShapeView.StrokeWidthProperty.PropertyName) ||
                (e.PropertyName == ShapeView.ShapeTypeProperty.PropertyName) ||
                (e.PropertyName == ShapeView.IsVisibleProperty.PropertyName) ||
                (e.PropertyName == ShapeView.OpacityProperty.PropertyName))
            {
                this.SetNeedsDisplay();
            }
        }
    }
}