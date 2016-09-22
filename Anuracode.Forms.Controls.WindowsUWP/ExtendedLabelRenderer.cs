// <copyright file="ExtendedLabelRenderer.cs" company="Anuracode">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana.</author>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using Xamarin.Forms;

#if WINDOWS_UWP

using Xamarin.Forms.Platform.UWP;

#else

using Xamarin.Forms.Platform.WinRT;

#endif

[assembly: ExportRenderer(typeof(Anuracode.Forms.Controls.ExtendedLabel), typeof(Anuracode.Forms.Controls.Renderers.ExtendedLabelRenderer))]

namespace Anuracode.Forms.Controls.Renderers
{
    /// <summary>
    /// The extended label renderer.
    /// </summary>
    /// <summary>
    /// The extended label renderer.
    /// </summary>
    public class ExtendedLabelRenderer : LabelRenderer
    {
        /// <summary>
        /// Cache for the file exits.
        /// </summary>
        private static Dictionary<string, bool> fileExitsCache = new Dictionary<string, bool>();

        /// <summary>
        /// Lock for the font exits.
        /// </summary>
        private static SemaphoreSlim lockFontExits = new SemaphoreSlim(1);

        /// <summary>
        /// The on element changed callback.
        /// </summary>
        /// <param name="e">
        /// The event arguments.
        /// </param>
        protected async override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            var view = (ExtendedLabel)Element;
            await UpdateUi(view, Control);
        }

        /// <summary>
        /// Event when element property changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override async void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if ((this.Element == null) || (this.Control == null))
            {
                return;
            }

            if ((e.PropertyName == ExtendedLabel.FontNameProperty.PropertyName) ||
                (e.PropertyName == ExtendedLabel.FriendlyFontNameProperty.PropertyName) ||
                (e.PropertyName == ExtendedLabel.FontSizeProperty.PropertyName) ||
                (e.PropertyName == ExtendedLabel.IsUnderlineProperty.PropertyName) ||
                (e.PropertyName == ExtendedLabel.TextProperty.PropertyName))
            {
                await UpdateUi(this.Element as ExtendedLabel, this.Control);
            }
        }

        /// <summary>
        /// Checks if a local resource font file exists
        /// </summary>
        /// <param name="filename">the filename including extension, but not path</param>
        /// <returns></returns>
        private static async Task<bool> LocalFontFileExistsAsync(string filename)
        {
            bool fileExists = true;

            bool hasCache = fileExitsCache.TryGetValue(filename, out fileExists);

            if (!hasCache)
            {
                try
                {
                    await lockFontExits.WaitAsync();

                    // Mayby was added while waiting.
                    hasCache = fileExitsCache.TryGetValue(filename, out fileExists);

                    if (!hasCache)
                    {
                        var uri = new System.Uri(string.Format(@"ms-appx:///Assets/Fonts/{0}", filename));
                        var file = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(uri);
#if WINDOWS_PHONE_APP
                        fileExists = file != null;
#else
                        fileExists = file.IsAvailable;
#endif

                        fileExitsCache.Add(filename, fileExists);
                    }
                }
                catch (Exception)
                {
                    // If the file dosn't exits it throws an exception, make fileExists false in this case
                    fileExists = false;
                }
                finally
                {
                    lockFontExits.Release();
                }
            }

            return fileExists;
        }

        /// <summary>
        /// Updates the UI.
        /// </summary>
        /// <param name="view">
        /// The view.
        /// </param>
        /// <param name="control">
        /// The control.
        /// </param>
        private async Task UpdateUi(ExtendedLabel view, TextBlock control)
        {
            if ((view != null) && (control != null))
            {
                if (view.FontSize > 0)
                {
                    control.FontSize = (float)view.FontSize;
                }

                ////need to do this ahead of font change due to unexpected behaviour if done later.
                if (view.IsStrikeThrough)
                {
                    //isn't perfect, but it's a start
                    var border = new Border
                    {
                        Height = 2,
                        Width = Control.ActualWidth,
                        Background = control.Foreground,
                        HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center
                    };

                    Canvas.SetTop(border, control.FontSize * .72);
                    this.Children.Add(border);
                }

                if (!string.IsNullOrEmpty(view.FontName))
                {
                    string filename = view.FontName;
                    //if no extension given then assume and add .ttf
                    if (filename.LastIndexOf(".", StringComparison.Ordinal) != filename.Length - 4)
                    {
                        filename = string.Format("{0}.ttf", filename);
                    }

                    if (await LocalFontFileExistsAsync(filename)) //only substitute custom font if exists
                    {
                        control.FontFamily =
                            new FontFamily(string.Format(@"/Assets/Fonts/{0}#{1}", filename,
                                string.IsNullOrEmpty(view.FriendlyFontName)
                                    ? filename.Substring(0, filename.Length - 4)
                                    : view.FriendlyFontName));
                    }
                }

                if (view.IsUnderline && !string.IsNullOrEmpty(view.Text))
                {
                    try
                    {
                        control.Text = string.Empty;
                        Underline ul = new Underline();
                        Run r = new Run();
                        r.Text = view.Text;
                        ul.Inlines.Add(r);
                        control.Inlines.Add(ul);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
            }
        }
    }
}