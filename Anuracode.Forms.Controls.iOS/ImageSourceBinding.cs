// <copyright file="CachedImageRenderer.cs" company="">
// All rights reserved.
// </copyright>
// https://github.com/molinch/FFImageLoading/blob/master/source/FFImageLoading.Forms.Touch/ImageSourceBinding.cs

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls.Renderers
{
    /// <summary>
    /// Image source binding.
    /// </summary>
    internal class ImageSourceBinding
    {
        /// <summary>
        /// Defalut constructor.
        /// </summary>
        /// <param name="imageSource">Source to use.</param>
        /// <param name="path">Path to use.</param>
        public ImageSourceBinding(FFImageLoading.Work.ImageSource imageSource, string path)
        {
            ImageSource = imageSource;
            Path = path;
        }

        /// <summary>
        /// Constructor with stream.
        /// </summary>
        /// <param name="stream">Stream to use.</param>
        public ImageSourceBinding(Func<CancellationToken, Task<Stream>> stream)
        {
            ImageSource = FFImageLoading.Work.ImageSource.Stream;
            Stream = stream;
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
        /// Stream to use.
        /// </summary>
        public Func<CancellationToken, Task<Stream>> Stream { get; private set; }

        /// <summary>
        /// Equals overload.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True if equal.</returns>
        public override bool Equals(object obj)
        {
            var item = obj as ImageSourceBinding;

            if (item == null)
            {
                return false;
            }

            return this.ImageSource.Equals(item.ImageSource) && this.Path.Equals(item.Path) && this.Stream.Equals(item.Stream);
        }

        /// <summary>
        /// Has code to use.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + this.ImageSource.GetHashCode();
                hash = hash * 23 + Path.GetHashCode();
                hash = hash * 23 + Stream.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Get binding.
        /// </summary>
        /// <param name="source">Source to use.</param>
        /// <returns>Image source binding.</returns>
        internal static ImageSourceBinding GetImageSourceBinding(ImageSource source)
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

            var streamImageSource = source as StreamImageSource;
            if (streamImageSource != null)
            {
                return new ImageSourceBinding(streamImageSource.Stream);
            }

            throw new NotImplementedException("ImageSource type not supported");
        }
    }
}