// <copyright file="ClearingImageView.cs" company="">
// All rights reserved.
// </copyright>

using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Widget;
using System;

namespace Anuracode.Forms.Controls.Renderers
{
    /// <summary>
    /// Clearing view.
    /// </summary>
    public class ClearingImageView : ImageView
    {
        /// <summary>
        /// Bitmap to use.
        /// </summary>
        private Bitmap imageBitmap = null;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Context to use.</param>
        public ClearingImageView(Context context)
            : base(context)
        {
        }

        /// <summary>
        /// Constructor with attributes.
        /// </summary>
        /// <param name="context">Context to use.</param>
        /// <param name="attrs">Attributes to use.</param>
        public ClearingImageView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
        }

        /// <summary>
        /// Constructor with attributes.
        /// </summary>
        /// <param name="context">Context to use.</param>
        /// <param name="attrs">Attributes to use.</param>
        /// <param name="defStyle">Style to use.</param>
        public ClearingImageView(Context context, IAttributeSet attrs, int defStyle)
            : base(context, attrs, defStyle)
        {
        }

        /// <summary>
        /// Set image.
        /// </summary>
        /// <param name="bm">Bitmap to use.</param>
        public override void SetImageBitmap(Bitmap bm)
        {
            base.SetImageBitmap(bm);
            if (imageBitmap != null)
            {
                imageBitmap.Recycle();
                imageBitmap.Dispose();
            }

            imageBitmap = bm;
            GC.Collect();
        }
    }
}