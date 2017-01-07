// <copyright file="ImageCircleRenderer.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Android.Graphics;
using Android.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Anuracode.Forms.Controls.Renderers
{
    /// <summary>
    /// ImageCircle Implementation
    /// </summary>
    public class ImageCircleRenderer : ExtendedImageRenderer
    {
        /// <summary>
        /// Draw child.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="child"></param>
        /// <param name="drawingTime"></param>
        /// <returns></returns>
        protected override bool DrawChild(Canvas canvas, global::Android.Views.View child, long drawingTime)
        {
            try
            {
                var radius = Math.Min(Width, Height) / 2;
                var strokeWidth = 10;
                radius -= strokeWidth / 2;

                Path path = new Path();
                path.AddCircle(Width / 2, Height / 2, radius, Path.Direction.Ccw);
                canvas.Save();
                canvas.ClipPath(path);

                var result = base.DrawChild(canvas, child, drawingTime);

                canvas.Restore();

                path = new Path();
                path.AddCircle(Width / 2, Height / 2, radius, Path.Direction.Ccw);

                var paint = new Paint();
                paint.AntiAlias = true;
                paint.StrokeWidth = ((CircleImage)Element).BorderThickness;
                paint.SetStyle(Paint.Style.Stroke);
                paint.Color = ((CircleImage)Element).BorderColor.ToAndroid();

                canvas.DrawPath(path, paint);

                paint.Dispose();
                path.Dispose();
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Unable to create circle image: " + ex);
            }

            return base.DrawChild(canvas, child, drawingTime);
        }

        /// <summary>
        /// On element changed.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnElementChanged(ElementChangedEventArgs<ExtendedImage> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                // Only enable hardware accelleration on lollipop
                if ((int)global::Android.OS.Build.VERSION.SdkInt < 21)
                {
                    SetLayerType(LayerType.Software, null);
                }
            }
        }
    }
}