// <copyright file="Shape.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Util;
using Android.Views;
using System;
using Xamarin.Forms.Platform.Android;
using Anuracode.Forms.Controls.Extensions;

namespace Anuracode.Forms.Controls.Renderers
{
    /// <summary>
    /// This is our class responsible for drawing our shapes
    /// </summary>
    public class Shape : View
    {
        /// <summary>
        /// Pixel density.
        /// </summary>
        private readonly float density;

        /// <summary>
        /// Drawable Background.
        /// </summary>
        private GradientDrawable drawableBackground;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="density">Pixel density.</param>
        /// <param name="context">Context to use.</param>
        public Shape(float density, Context context)
            : base(context)
        {
            this.density = density == 0 ? 0 : density - 0.6f;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="density">Density to use.</param>
        /// <param name="context">Context to use.</param>
        /// <param name="attributes">Attributes to use.</param>
        public Shape(float density, Context context, IAttributeSet attributes)
            : base(context, attributes)
        {
            this.density = density;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="density">Density to use.</param>
        /// <param name="context">Context to use.</param>
        /// <param name="attributes">Attributes to use.</param>
        /// <param name="defStyle">Style to use.</param>
        public Shape(float density, Context context, IAttributeSet attributes, int defStyle)
            : base(context, attributes, defStyle)
        {
            this.density = density;
        }

        /// <summary>
        /// Shape to use.
        /// </summary>
        public ShapeView ShapeView { get; set; }

        /// <summary>
        /// Handle shape draw.
        /// </summary>
        /// <param name="canvas">Canvas to use.</param>
        protected virtual void HandleShapeDraw(Canvas canvas)
        {
            if (ShapeView != null)
            {
                var elementView = ShapeView;

                if (drawableBackground == null)
                {
                    drawableBackground = new GradientDrawable();
                }

                if (elementView.ShapeType == ShapeType.Circle)
                {
                    drawableBackground.SetShape(global::Android.Graphics.Drawables.ShapeType.Oval);
                }
                else
                {
                    drawableBackground.SetCornerRadius(elementView.CornerRadius);
                }

                var strokeWidth = Resize(elementView.StrokeWidth);

                drawableBackground.SetColor(elementView.Color.ToAndroid());
                drawableBackground.SetStroke(Convert.ToInt32(strokeWidth), elementView.StrokeColor.ToAndroid());

                float border = (strokeWidth - 0.6f).Clamp(0, float.MaxValue);
                MidpointRounding rounding = MidpointRounding.AwayFromZero;

                int top = Convert.ToInt32(Math.Round(border, rounding));
                int left = Convert.ToInt32(Math.Round(border, rounding));
                int right = Convert.ToInt32(Math.Round(this.Width - border, rounding));
                int bottom = Convert.ToInt32(Math.Round(this.Height - border, rounding));

                drawableBackground.SetBounds(left, top, right, bottom);

                drawableBackground.Draw(canvas);
            }
        }

        /// <summary>
        /// On draw.
        /// </summary>
        /// <param name="canvas">Canvas to use.</param>
        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            HandleShapeDraw(canvas);
        }

        /// <summary>
        /// Helper functions for dealing with pizel density.
        /// </summary>
        /// <param name="input">Input to use.</param>
        /// <returns>New size.</returns>
        private float Resize(float input)
        {
            return input * density;
        }
    }
}