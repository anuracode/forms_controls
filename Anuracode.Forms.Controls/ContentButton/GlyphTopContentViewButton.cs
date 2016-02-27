// <copyright file="GlyphTopContentViewButton.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

namespace Anuracode.Forms.Controls
{
    /// <summary>
    /// Content view button.
    /// </summary>
    public class GlyphTopContentViewButton : GlyphContentViewButton
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public GlyphTopContentViewButton()
            : base(true, true, ImageOrientation.ImageOnTop)
        {
        }
    }
}