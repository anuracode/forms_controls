﻿// <copyright file="SignaturePadRenderer.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Anuracode.Forms.Controls.Renderers
{
    /// <summary>
    /// Signature pad renderer.
    /// </summary>
    public class SignaturePadRenderer : ViewRenderer<SignaturePadView, SignaturePad>
    {
        /// <summary>
        /// Flag when the element has been disposed.
        /// </summary>
        protected bool Disposed { get; set; }

        /// <summary>
        /// Get the points in string format.
        /// </summary>
        public async Task<string> GetPointsStringAsync()
        {
            string result = null;

            if (Control != null && !Disposed)
            {
                result = await Control.GetPointsStringAsync();
            }

            return result;
        }

        /// <summary>
        /// Clear view.
        /// </summary>
        protected void Clear()
        {
            if (!Disposed && Control != null)
            {
                Control.Clear();
            }
        }

        /// <summary>
        /// Dispose renderer.
        /// </summary>
        /// <param name="disposing">Disposing managed.</param>
        protected override void Dispose(bool disposing)
        {
            if (!disposing)
            {
                if (Element != null)
                {
                    Element.GetIsBlankDelegate = null;
                    Element.GetImageStringBase64Delegate = null;
                    Element.GetPointsStringDelegate = null;
                    Element.ClearDelegate = null;
                }

                if (Control != null)
                {
                    Control.RaiseIsBlankChangedDelegate = null;
                }
            }

            Disposed = true;

            base.Dispose(disposing);
        }

        /// <summary>
        /// Get image as string.
        /// </summary>
        /// <returns>Task to await.</returns>
        protected async Task<string> GetImageStringBase64Async()
        {
            string result = null;

            await Task.FromResult(0);

            if (!Disposed && Control != null)
            {
                using (var image = this.Control.GetImage(UIKit.UIColor.Black, UIKit.UIColor.White, shouldCrop: true, keepAspectRatio: true))
                {
                    using (var stream = image.AsJPEG().AsStream())
                    {
                        using (var mstream = new MemoryStream())
                        {
                            await stream.CopyToAsync(mstream);
                            result = Convert.ToBase64String(mstream.ToArray());
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Get is blank.
        /// </summary>
        /// <returns>True when is blank.</returns>
        protected bool GetIsBlank()
        {
            bool isBlank = true;

            if (!Disposed && Control != null)
            {
                isBlank = Control.IsBlank;
            }

            return isBlank;
        }

        /// <summary>
        /// Called when [element changed].
        /// </summary>
        /// <param name="e">The e.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<SignaturePadView> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null || this.Element == null)
            {
                return;
            }

            if (Control == null)
            {
                var view = new SignaturePad();
                var el = e.NewElement;

                if (el.BackgroundColor != Color.Default)
                {
                    view.BackgroundColor = el.BackgroundColor.ToUIColor();
                }

                if (!String.IsNullOrWhiteSpace(el.CaptionText))
                {
                    view.Caption.Text = el.CaptionText;
                }

                if (el.CaptionTextColor != Color.Default)
                {
                    view.Caption.TextColor = el.CaptionTextColor.ToUIColor();
                }

                if (!String.IsNullOrWhiteSpace(el.PromptText))
                {
                    view.SignaturePrompt.Text = el.PromptText;
                }

                if (el.PromptTextColor != Color.Default)
                {
                    view.SignaturePrompt.TextColor = el.PromptTextColor.ToUIColor();
                }

                if (el.SignatureLineColor != Color.Default)
                {
                    view.SignatureLineColor = el.SignatureLineColor.ToUIColor();
                }

                if (el.StrokeColor != Color.Default)
                {
                    view.StrokeColor = el.StrokeColor.ToUIColor();
                }

                if (el.StrokeWidth > 0)
                {
                    view.StrokeWidth = el.StrokeWidth;
                }

                e.NewElement.ClearDelegate = Clear;
                e.NewElement.GetIsBlankDelegate = GetIsBlank;
                e.NewElement.GetImageStringBase64Delegate = GetImageStringBase64Async;
                e.NewElement.GetPointsStringDelegate = GetPointsStringAsync;
                view.RaiseIsBlankChangedDelegate = e.NewElement.RaiseIsBlankChanged;

                this.SetNativeControl(view);
            }
        }

        /// <summary>
        /// Element property changed.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Arguments of the event.</param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (this.Control == null || this.Element == null)
            {
                return;
            }

            if (!Disposed)
            {
                var el = this.Element;
                if (e.PropertyName == SignaturePadView.BackgroundColorProperty.PropertyName)
                {
                    this.Control.BackgroundColor = el.BackgroundColor.ToUIColor();
                }
                else if (e.PropertyName == SignaturePadView.CaptionTextProperty.PropertyName)
                {
                    this.Control.Caption.Text = el.CaptionText;
                }
                else if (e.PropertyName == SignaturePadView.CaptionTextColorProperty.PropertyName)
                {
                    this.Control.Caption.TextColor = el.CaptionTextColor.ToUIColor();
                }
                else if (e.PropertyName == SignaturePadView.PromptTextProperty.PropertyName)
                {
                    this.Control.SignaturePrompt.Text = el.PromptText;
                }
                else if (e.PropertyName == SignaturePadView.PromptTextColorProperty.PropertyName)
                {
                    this.Control.SignaturePrompt.TextColor = el.PromptTextColor.ToUIColor();
                }
                else if (e.PropertyName == SignaturePadView.SignatureLineColorProperty.PropertyName)
                {
                    this.Control.SignatureLineColor = el.SignatureLineColor.ToUIColor();
                }
                else if (e.PropertyName == SignaturePadView.StrokeColorProperty.PropertyName)
                {
                    this.Control.StrokeColor = el.StrokeColor.ToUIColor();
                }
                else if (e.PropertyName == SignaturePadView.StrokeWidthProperty.PropertyName)
                {
                    this.Control.StrokeWidth = el.StrokeWidth;
                }
            }
        }
    }
}