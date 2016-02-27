// <copyright file="GlyphLeftContentViewButton.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

namespace Anuracode.Forms.Controls
{
    /// <summary>
    /// Content view button.
    /// </summary>
    public class GlyphLeftContentViewButton : GlyphContentViewButton
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public GlyphLeftContentViewButton()
            : base(true, true, ImageOrientation.ImageToLeft)
        {
        }
    }
}