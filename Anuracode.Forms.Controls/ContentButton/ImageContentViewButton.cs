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
        /// <param name="hasBorder">Has border.</param>
        /// <param name="hasBackground">Has background.</param>
        /// <param name="useDisableBox">Use disable box.</param>
        public ImageContentViewButton(bool hasBorder = false, bool hasBackground = false, bool useDisableBox = false)
            : base(false, true, ImageOrientation.ImageOnTop, hasBorder: hasBorder, hasBackground: hasBackground, useDisableBox: useDisableBox)
        {
        }
    }
}