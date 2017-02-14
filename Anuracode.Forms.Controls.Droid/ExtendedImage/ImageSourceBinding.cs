// <copyright file="ExtendedImageRenderer.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System;
using System.IO;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls.Renderers
{
    /// <summary>
    /// Image source binding.
    /// </summary>
    public class ImageSourceBinding
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="imageSource">Image source to use.</param>
        /// <param name="path">Path to use.</param>
        public ImageSourceBinding(FFImageLoading.Work.ImageSource imageSource, string path)
        {
            ImageSource = imageSource;
            Path = path;
        }

        /// <summary>
        /// Image source.
        /// </summary>
        public FFImageLoading.Work.ImageSource ImageSource { get; private set; }

        /// <summary>
        /// Path to use.
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// Equals to use.
        /// </summary>
        /// <param name="obj">Object to use.</param>
        /// <returns>Result to use.</returns>
        public override bool Equals(object obj)
        {
            var item = obj as ImageSourceBinding;

            if (item == null)
            {
                return false;
            }

            return this.ImageSource.Equals(item.ImageSource) && this.Path.Equals(item.Path);
        }

        /// <summary>
        /// Get hash code.
        /// </summary>
        /// <returns>Code to use.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + this.ImageSource.GetHashCode();
                hash = hash * 23 + Path.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Get binding.
        /// </summary>
        /// <param name="source">Source to use.</param>
        /// <returns>Binding to use.</returns>
        public static ImageSourceBinding GetImageSourceBinding(ImageSource source)
        {
            if (source == null)
            {
                return null;
            }

            var uriImageSource = source as UriImageSource;
            if (uriImageSource != null)
            {
                return new ImageSourceBinding(FFImageLoading.Work.ImageSource.Url, uriImageSource.Uri.ToString());
            }

            var fileImageSource = source as FileImageSource;
            if (fileImageSource != null)
            {
                if (File.Exists(fileImageSource.File))
                    return new ImageSourceBinding(FFImageLoading.Work.ImageSource.Filepath, fileImageSource.File);

                return new ImageSourceBinding(FFImageLoading.Work.ImageSource.CompiledResource, fileImageSource.File);
            }

            throw new NotImplementedException("ImageSource type not supported");
        }
    }
}