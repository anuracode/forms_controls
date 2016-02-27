// <copyright file="ImageLeftContentViewButton.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

namespace Anuracode.Forms.Controls
{
    /// <summary>
    /// Content view button.
    /// </summary>
    public class ImageLeftContentViewButton : ContentViewButton
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public ImageLeftContentViewButton()
            : base(true, true, ImageOrientation.ImageToLeft)
        {
        }
    }
}