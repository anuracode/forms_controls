// <copyright file="GlyphOnlyContentViewButton.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

namespace Anuracode.Forms.Controls
{
    /// <summary>
    /// Content view button.
    /// </summary>
    public class GlyphOnlyContentViewButton : GlyphContentViewButton
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public GlyphOnlyContentViewButton()
            : base(false, true, ImageOrientation.ImageOnTop)
        {
        }
    }
}