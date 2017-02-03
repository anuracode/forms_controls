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
        /// <param name="hasBorder">Has border.</param>
        /// <param name="hasBackground">Has background.</param>
        /// <param name="useDisableBox">Use disable box.</param>
        public ImageLeftContentViewButton(bool hasBorder = false, bool hasBackground = false, bool useDisableBox = false)
            : base(true, true, ImageOrientation.ImageToLeft, hasBorder: hasBorder, hasBackground: hasBackground, useDisableBox: useDisableBox)
        {
        }
    }
}