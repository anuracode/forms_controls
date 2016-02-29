// <copyright file="CommonResources.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anuracode.Forms.Controls.Sample.Views.Common
{
    /// <summary>
    /// Common resources for the app.
    /// </summary>
    public partial class CommonResources : Anuracode.Forms.Controls.Styles.CommonResourcesLightTheme
    {
        /// <summary>
        /// Path for the image.
        /// </summary>
        public string PathImageAppLogoLarge = ExtendedImage.CompleteImagePrefix("LogoApp256.png");

        /// <summary>
        /// Path for the image.
        /// </summary>
        public string PathImageFeaturesAction = ExtendedImage.CompleteImagePrefix("appbar_features.png");

        /// <summary>
        /// Path for the image.
        /// </summary>
        public string PathImageHambuergerLogo = ExtendedImage.CompleteImagePrefix("hamburgerlogo.png");

        /// <summary>
        /// Path for image.
        /// </summary>
        public string PathImageCancelAction = ExtendedImage.CompleteImagePrefix("appbar_cancel.png");
    }
}
