// <copyright file="ImageSampleViewModel.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

namespace Anuracode.Forms.Controls.Sample.ViewModels
{
    /// <summary>
    /// Login view model
    /// </summary>
    public class ImageSampleViewModel : BaseViewModel
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public ImageSampleViewModel()
            : base()
        {
            Title = string.Empty;
        }

        /// <summary>
        /// Path to image.
        /// </summary>
        public string SampleImagePath
        {
            get
            {
                return Theme.CommonResources.PathImageSample;
            }
        }
    }
}