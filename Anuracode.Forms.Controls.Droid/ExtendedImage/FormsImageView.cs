// <copyright file="FormsImageView.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Android.Content;
using Android.Widget;

namespace Anuracode.Forms.Controls.Renderers
{
    /// <summary>
    /// Image view with skipping invalidate.
    /// </summary>
    public class FormsImageView : ImageView
    {
        /// <summary>
        /// Skip invalidate.
        /// </summary>
        private bool skipInvalidate;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context">Context to use.</param>
        public FormsImageView(Context context)
            : base(context)
        {
        }

        /// <summary>
        /// Invalidate view.
        /// </summary>
        public override void Invalidate()
        {
            if (this.skipInvalidate)
            {
                this.skipInvalidate = false;
            }
            else
            {
                base.Invalidate();
            }
        }

        /// <summary>
        /// Skip invalidate.
        /// </summary>
        public void SkipInvalidate()
        {
            this.skipInvalidate = true;
        }
    }
}