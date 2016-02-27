// <copyright file="ImageContentViewButton.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

namespace Anuracode.Forms.Controls
{
    /// <summary>
    /// Content view button.
    /// </summary>
    public class ImageContentViewButton : ContentViewButton
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public ImageContentViewButton()
            : base(false, true, ImageOrientation.ImageOnTop)
        {
        }
    }
}