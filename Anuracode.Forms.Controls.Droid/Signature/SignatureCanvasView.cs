// <copyright file="SignatureCanvasView.cs" company="">
// All rights reserved.
// </copyright>

using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;

namespace Anuracode.Forms.Controls.Renderers
{
    /// <summary>
    /// View for the signature.
    /// </summary>
    internal class SignatureCanvasView : View
    {
        /// <summary>
        /// Current paint.
        /// </summary>
        private Paint currentPaint;

        /// <summary>
        /// Current path.
        /// </summary>
        private Path currentPath;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Context to use.</param>
        public SignatureCanvasView(Context context) :
            base(context)
        {
            Initialize();
        }

        /// <summary>
        /// Constructor with attributes.
        /// </summary>
        /// <param name="context">Context to use.</param>
        /// <param name="attrs">Attributes to use.</param>
        public SignatureCanvasView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }

        /// <summary>
        /// Constructor with attributes.
        /// </summary>
        /// <param name="context">Context to use.</param>
        /// <param name="attrs">Attributes to use.</param>
        /// <param name="defStyle">Style to use.</param>
        public SignatureCanvasView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
        }

        /// <summary>
        /// Current paint.
        /// </summary>
        public Paint Paint
        {
            set
            {
                this.currentPaint = value;
            }
        }

        /// <summary>
        /// Current path.
        /// </summary>
        public Path Path
        {
            set
            {
                this.currentPath = value;
            }
        }

        /// <summary>
        /// When draw.
        /// </summary>
        /// <param name="canvas">Canvas to use.</param>
        protected override void OnDraw(Canvas canvas)
        {
            if (this.currentPath == null || this.currentPath.IsEmpty)
            {
                return;
            }

            canvas.DrawPath(this.currentPath, this.currentPaint);
        }

        /// <summary>
        /// Initialize view.
        /// </summary>
        private void Initialize()
        {
        }
    }
}