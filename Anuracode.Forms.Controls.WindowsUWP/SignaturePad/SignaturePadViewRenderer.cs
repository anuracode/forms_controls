// <copyright file="SignaturePadViewRenderer.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System.Threading.Tasks;
using Xamarin.Forms;

#if WINDOWS_UWP
using Xamarin.Forms.Platform.UWP;
#else
using Xamarin.Forms.Platform.WinRT;
#endif

namespace Anuracode.Forms.Controls.Renderers
{
    /// <summary>
    /// Signature pad renderer.
    /// </summary>
    public class SignaturePadViewRenderer : ViewRenderer<SignaturePadView, SignaturePad>
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

                await Control.LoadPointsStringAsync(result);
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
                SignaturePad view = new SignaturePad();

                try
                {
                    e.NewElement.ClearDelegate = Clear;
                    e.NewElement.GetIsBlankDelegate = GetIsBlank;
                    e.NewElement.GetImageStringBase64Delegate = GetImageStringBase64Async;
                    e.NewElement.GetPointsStringDelegate = GetPointsStringAsync;
                    view.RaiseIsBlankChangedDelegate = e.NewElement.RaiseIsBlankChanged;

                    var el = e.NewElement;

                    if (el.BackgroundColor != Color.Default)
                    {
                        view.InkPresenter.Background = el.BackgroundColor.ToBrush();
                    }

                    if (!string.IsNullOrWhiteSpace(el.CaptionText))
                    {
                        view.TextBlockCaption.Text = el.CaptionText;
                    }

                    if (el.CaptionTextColor != Color.Default)
                    {
                        view.TextBlockCaption.Foreground = el.CaptionTextColor.ToBrush();
                    }

                    if (!string.IsNullOrWhiteSpace(el.PromptText))
                    {
                        view.TextBlockPrompt.Text = el.PromptText;
                    }

                    if (el.PromptTextColor != Color.Default)
                    {
                        view.TextBlockPrompt.Foreground = el.PromptTextColor.ToBrush();
                    }

                    if (el.SignatureLineColor != Color.Default)
                    {
                        view.StrokeColor = el.SignatureLineColor.ToBrush();
                    }

                    if (el.StrokeColor != Color.Default)
                    {
                        view.StrokeColor = el.StrokeColor.ToBrush();
                    }

                    if (el.StrokeWidth > 0)
                    {
                        view.StrokeThickness = el.StrokeWidth;
                    }
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }

                SetNativeControl(view);
            }
        }

        /// <summary>
        /// Element property changed.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Arguments of the event.</param>
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (this.Control == null || this.Element == null)
            {
                return;
            }

            if (!Disposed)
            {
                SignaturePad view = Control;
                var el = this.Element;

                if (e.PropertyName == SignaturePadView.BackgroundColorProperty.PropertyName)
                {
                    view.InkPresenter.Background = el.BackgroundColor.ToBrush();
                }
                else if (e.PropertyName == SignaturePadView.CaptionTextProperty.PropertyName)
                {
                    view.TextBlockCaption.Text = el.CaptionText;
                }
                else if (e.PropertyName == SignaturePadView.CaptionTextColorProperty.PropertyName)
                {
                    view.TextBlockCaption.Foreground = el.CaptionTextColor.ToBrush();
                }
                else if (e.PropertyName == SignaturePadView.PromptTextProperty.PropertyName)
                {
                    view.TextBlockPrompt.Text = el.PromptText;
                }
                else if (e.PropertyName == SignaturePadView.PromptTextColorProperty.PropertyName)
                {
                    view.TextBlockPrompt.Foreground = el.PromptTextColor.ToBrush();
                }
                else if (e.PropertyName == SignaturePadView.SignatureLineColorProperty.PropertyName)
                {
                    view.StrokeColor = el.SignatureLineColor.ToBrush();
                }
                else if (e.PropertyName == SignaturePadView.StrokeColorProperty.PropertyName)
                {
                    view.StrokeColor = el.StrokeColor.ToBrush();
                }
                else if (e.PropertyName == SignaturePadView.StrokeWidthProperty.PropertyName)
                {
                    view.StrokeThickness = el.StrokeWidth;
                }
            }
        }
    }
}
