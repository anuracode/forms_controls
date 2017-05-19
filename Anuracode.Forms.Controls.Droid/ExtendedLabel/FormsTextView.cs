// <copyright file="FormsTextView.cs" company="Open source">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using System;

namespace Anuracode.Forms.Controls.Renderers
{
    /// <summary>
    /// Text control.
    /// </summary>
    public class FormsTextView : TextView
    {
        /// <summary>
        /// Skip refresh.
        /// </summary>
        private bool skip;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Context to use.</param>
        public FormsTextView(Context context) : base(context)
        {
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Context to use.</param>
        /// <param name="attrs">Attributes to use.</param>
        public FormsTextView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Context to use.</param>
        /// <param name="attrs">Attributes to use.</param>
        /// <param name="defStyle">Style to use.</param>
        public FormsTextView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="javaReference">Java reference.</param>
        /// <param name="transfer">Transfer handle.</param>
        protected FormsTextView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        /// <summary>
        /// Invalidate view.
        /// </summary>
        public override void Invalidate()
        {
            if (!skip)
            {
                base.Invalidate();
            }

            skip = false;
        }

        /// <summary>
        /// Skip invalidate.
        /// </summary>
        public void SkipNextInvalidate()
        {
            skip = true;
        }
    }
}