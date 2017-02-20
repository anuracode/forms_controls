// <copyright file="ExtendedLabelRenderer.cs" company="">
// All rights reserved.
// </copyright>
// <author>https://github.com/XLabs/Xamarin-Forms-Labs</author>

using Foundation;
using System.IO;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Anuracode.Forms.Controls.Renderers
{
    /// <summary>
    /// The extended label renderer.
    /// </summary>
    public class ExtendedLabelRenderer : LabelRenderer
    {
        /// <summary>
        /// Flag when the element has been disposed.
        /// </summary>
        protected bool Disposed { get; set; }

        /// <summary>
        /// Dispose renderer.
        /// </summary>
        /// <param name="disposing">Disposing managed.</param>
        protected override void Dispose(bool disposing)
        {
            if (!disposing)
            {
            }

            Disposed = true;

            base.Dispose(disposing);
        }

        /// <summary>
        /// The on element changed callback.
        /// </summary>
        /// <param name="e">
        /// The event arguments.
        /// </param>
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            if (!Disposed)
            {
                base.OnElementChanged(e);

                var view = (ExtendedLabel)Element;

                UpdateUi(view, Control);
            }
        }

        /// <summary>
        /// Raises the element property changed event.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">The event arguments</param>
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!Disposed)
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
                    (e.PropertyName == ExtendedLabel.TextProperty.PropertyName) ||
                    (e.PropertyName == ExtendedLabel.FormattedTextProperty.PropertyName) ||
                    (e.PropertyName == ExtendedLabel.TextColorProperty.PropertyName))
                {
                    UpdateUi(this.Element as ExtendedLabel, this.Control);
                }
            }
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
        private void UpdateUi(ExtendedLabel view, UILabel control)
        {
            if ((view != null) && (control != null))
            {
                UIFont font = null;

                if ((font == null) && !string.IsNullOrEmpty(view.FriendlyFontName))
                {
                    font = (UIFont)(UIFont.FromName(view.FriendlyFontName, (view.FontSize > 0) ? (float)view.FontSize : 12.0f));
                }

                if ((font == null) && !string.IsNullOrEmpty(view.FontName))
                {
                    var fontName = Path.GetFileNameWithoutExtension(view.FontName);

                    font = (UIFont)(UIFont.FromName(fontName, (view.FontSize > 0) ? (float)view.FontSize : 12.0f));
                }

                if ((font == null) && (view.FontSize > 0))
                {
                    font = (UIFont)(UIFont.FromName(control.Font.Name, (float)view.FontSize));
                }

                if (font != null)
                {
                    control.Font = font;
                }
            }

            // For some reason, if we try and convert Color.Default to a UIColor, the resulting color is
            // either white or transparent. The net result is the ExtendedLabel does not display.
            // Only setting the control's TextColor if is not Color.Default will prevent this issue.
            if (view.TextColor != Color.Default)
            {
                control.TextColor = view.TextColor.ToUIColor();
            }

            if (view.FormattedText != null)
            {
                control.AttributedText = view.FormattedText.ToAttributed(Font.OfSize(control.Font.Name, view.FontSize), view.TextColor);
            }
            else if (!string.IsNullOrEmpty(view.Text) && (!view.IsUnderline || !view.IsStrikeThrough || !view.IsDropShadow))
            {
                control.Text = null;

                var attrString = new NSMutableAttributedString(view.Text);

                if (view.IsUnderline)
                {
                    attrString.AddAttribute(UIStringAttributeKey.UnderlineStyle,
                                            NSNumber.FromInt32((int)NSUnderlineStyle.Single),
                                            new NSRange(0, attrString.Length));
                }

                if (view.IsStrikeThrough)
                {
                    attrString.AddAttribute(UIStringAttributeKey.StrikethroughStyle,
                                            NSNumber.FromInt32((int)NSUnderlineStyle.Single),
                                            new NSRange(0, attrString.Length));
                }

                if (view.IsDropShadow)
                {
                    var dropShadow = new NSShadow
                    {
                        ShadowColor = UIColor.DarkGray,
                        ShadowBlurRadius = 1.4f,
                        ShadowOffset = new CoreGraphics.CGSize(new CoreGraphics.CGPoint(0.3f, 0.8f))
                    };

                    attrString.AddAttribute(UIStringAttributeKey.Shadow,
                                            dropShadow,
                                            new NSRange(0, attrString.Length));
                }

                control.AttributedText = attrString;
            }
        }
    }
}