// <copyright file="ExtendedLabelRender.cs" company="Open source">
// All rights reserved.
// </copyright>
// <author>https://github.com/XLabs/Xamarin-Forms-Labs</author>

using Android.Graphics;
using Android.Widget;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Anuracode.Forms.Controls.Droid;

namespace Anuracode.Forms.Controls.Renderers
{
    /// <summary>
    /// Extended renderer.
    /// </summary>
    public class ExtendedLabelRenderer : LabelRenderer
    {
        /// <summary>
        ///  Last text size.
        /// </summary>
        private float lastTextSize = -1f;

        /// <summary>
        /// Called when [element changed].
        /// </summary>
        /// <param name="e">The e.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            var view = (ExtendedLabel)Element;
            var control = Control;

            UpdateUi(view, control);
        }

        /// <summary>
        /// Element property changed.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Arguments of the event.</param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if ((this.Element == null) || (this.Control == null))
            {
                return;
            }

            if ((e.PropertyName == Label.TextColorProperty.PropertyName) ||
                (e.PropertyName == Label.FontProperty.PropertyName) ||
                (e.PropertyName == Label.TextProperty.PropertyName) ||
                (e.PropertyName == Label.FormattedTextProperty.PropertyName))
            {
                UpdateUi(this.Element as ExtendedLabel, this.Control);
                Control.PostInvalidate();
            }
        }

        /// <summary>
        /// Tries the set font.
        /// </summary>
        /// <param name="fontName">Name of the font.</param>
        /// <returns>Typeface.</returns>
        protected Typeface TrySetFont(string fontName)
        {
            try
            {
                return Typeface.CreateFromAsset(Xamarin.Forms.Forms.Context.Assets, fontName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("not found in assets. Exception: {0}", ex);
                try
                {
                    return Typeface.CreateFromFile(fontName);
                }
                catch (Exception ex1)
                {
                    Console.WriteLine("not found by file. Exception: {0}", ex1);

                    return Typeface.Default;
                }
            }
        }

        /// <summary>
        /// Updates the UI.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="control">The control.</param>
        protected void UpdateUi(ExtendedLabel view, TextView control)
        {
            if ((view != null) && (control != null))
            {
                if (!string.IsNullOrEmpty(view.FontName))
                {
                    string filename = view.FontName;
                    //if no extension given then assume and add .ttf
                    if (filename.LastIndexOf(".", System.StringComparison.Ordinal) != filename.Length - 4)
                    {
                        filename = string.Format("{0}.ttf", filename);
                    }
                    control.Typeface = TrySetFont(filename);
                }
                else if (view.Font != Font.Default)
                {
                    control.Typeface = view.Font.ToExtendedTypeface(Context);
                }

                if (view.FontSize > 0 && (lastTextSize != view.FontSize))
                {
                    control.SetTextSize(Android.Util.ComplexUnitType.Sp, (float)view.FontSize);
                    lastTextSize = (float)view.FontSize;
                }

                if (view.IsUnderline)
                {
                    control.PaintFlags = control.PaintFlags | PaintFlags.UnderlineText;
                }

                if (view.IsStrikeThrough)
                {
                    control.PaintFlags = control.PaintFlags | PaintFlags.StrikeThruText;
                }
            }
        }
    }
}