// <copyright file="GlyphContentViewButton.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls
{
    /// <summary>
    /// Content view button.
    /// </summary>
    public class GlyphContentViewButton : ContentViewButton
    {
        /// <summary>
        /// Dsiable glyph and not the background.
        /// </summary>
        public static readonly BindableProperty DisableGlyphOnlyProperty = BindablePropertyHelper.Create<GlyphContentViewButton, bool>(nameof(DisableGlyphOnly), true);

        /// <summary>
        /// The font name property.
        /// </summary>
        public static readonly BindableProperty GlyphFontNameProperty = BindablePropertyHelper.Create<GlyphContentViewButton, string>(nameof(GlyphFontName), Styles.ThemeManager.CommonResourcesBase.GlyphFontName);

        /// <summary>
        /// Font size.
        /// </summary>
        public static readonly BindableProperty GlyphFontSizeProperty = BindablePropertyHelper.Create<GlyphContentViewButton, double>(nameof(GlyphFontSize), Styles.ThemeManager.CommonResourcesBase.TextSizeMicro);

        /// <summary>
        /// The friendly font name property. This can be found on the first line of the font or in the font preview.
        /// This is only required on Windows Phone. If not given then the file name excl. the extension is used.
        /// </summary>
        public static readonly BindableProperty GlyphFriendlyFontNameProperty = BindablePropertyHelper.Create<GlyphContentViewButton, string>(nameof(GlyphFriendlyFontName), Styles.ThemeManager.CommonResourcesBase.GlyphFriendlyFontName);

        /// <summary>
        /// Glyph Text color disabled.
        /// </summary>
        public static readonly BindableProperty GlyphTextColorDisabledProperty = BindablePropertyHelper.Create<GlyphContentViewButton, Color>(
            nameof(GlyphTextColorDisabled),
            Styles.ThemeManager.CommonResourcesBase.TextColorDisable,
            BindingMode.OneWay,
            propertyChanged:
            (bindable, oldvalue, newvalue) =>
            {
                if (newvalue != null)
                {
                    GlyphContentViewButton button = bindable as GlyphContentViewButton;
                    if (button != null)
                    {
                        button.ButtonTappedCommand_CanExecuteChanged(null, EventArgs.Empty);
                    }
                }
            });

        /// <summary>
        /// Glyph Text color property.
        /// </summary>
        public static readonly BindableProperty GlyphTextColorProperty = BindablePropertyHelper.Create<GlyphContentViewButton, Color>(
            nameof(GlyphTextColor),
            Color.White,
            propertyChanged:
            (bindable, oldvalue, newvalue) =>
            {
                if (newvalue != null)
                {
                    GlyphContentViewButton button = bindable as GlyphContentViewButton;
                    if (button != null)
                    {
                        button.ButtonTappedCommand_CanExecuteChanged(null, EventArgs.Empty);
                    }
                }
            });

        /// <summary>
        /// Text property.
        /// </summary>
        public static readonly BindableProperty GlyphTextProperty = BindablePropertyHelper.Create<GlyphContentViewButton, string>(
            nameof(GlyphText),
            (string)null,
            propertyChanged:
            (bindable, oldvalue, newvalue) =>
            {
                string newString = newvalue as String;
                if (!string.IsNullOrEmpty(newString))
                {
                    GlyphContentViewButton button = bindable as GlyphContentViewButton;
                    if (button != null && button.LabelGlyph != null)
                    {
                        button.LabelGlyph.Text = newString;
                    }
                }
            });

        /// <summary>
        /// Default constructor.
        /// </summary>        
        /// <param name="hasText">Has text.</param>
        /// <param name="hasImage">Has image.</param>
        /// <param name="orientation">Orientation.</param>
        /// <param name="hasBorder">Has border.</param>
        /// <param name="hasBackground">Has background.</param>
        /// <param name="useDisableBox">Use disable box.</param>
        public GlyphContentViewButton(bool hasText, bool hasImage, ImageOrientation orientation, bool hasBorder = false, bool hasBackground = false, bool useDisableBox = false)
            : base(hasText, hasImage, orientation, hasBorder: hasBorder, hasBackground: hasBackground, useDisableBox: useDisableBox)
        {
        }

        /// <summary>
        /// Dsiable glyph and not the background.
        /// </summary>
        public bool DisableGlyphOnly
        {
            get
            {
                return (bool)GetValue(DisableGlyphOnlyProperty);
            }

            set
            {
                SetValue(DisableGlyphOnlyProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the name of the font file including extension. If no extension given then ttf is assumed.
        /// Fonts need to be included in projects accoring to the documentation.
        /// </summary>
        /// <value>The full name of the font file including extension.</value>
        public string GlyphFontName
        {
            get
            {
                return (string)GetValue(GlyphFontNameProperty);
            }

            set
            {
                SetValue(GlyphFontNameProperty, value);
            }
        }

        /// <summary>
        /// Font size.
        /// </summary>
        public double GlyphFontSize
        {
            get
            {
                return (double)GetValue(GlyphFontSizeProperty);
            }

            set
            {
                SetValue(GlyphFontSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the name of the font.
        /// </summary>
        /// <value>The name of the font.</value>
        public string GlyphFriendlyFontName
        {
            get
            {
                return (string)GetValue(GlyphFriendlyFontNameProperty);
            }
            set
            {
                SetValue(GlyphFriendlyFontNameProperty, value);
            }
        }

        /// <summary>
        /// Text value.
        /// </summary>
        public string GlyphText
        {
            get
            {
                return (string)GetValue(GlyphTextProperty);
            }

            set
            {
                SetValue(GlyphTextProperty, value);
            }
        }

        /// <summary>
        /// Text color.
        /// </summary>
        public Color GlyphTextColor
        {
            get
            {
                return (Color)GetValue(GlyphTextColorProperty);
            }

            set
            {
                SetValue(GlyphTextColorProperty, value);
            }
        }

        /// <summary>
        /// Glyph Text color disabled.
        /// </summary>
        public Color GlyphTextColorDisabled
        {
            get
            {
                return (Color)GetValue(GlyphTextColorDisabledProperty);
            }

            set
            {
                SetValue(GlyphTextColorDisabledProperty, value);
            }
        }

        /// <summary>
        /// Flag to determine if the image size is variable, used with the glyphs.
        /// </summary>
        protected override bool IsImageSizeVariable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Label for the glyph.
        /// </summary>
        protected ExtendedLabel LabelGlyph { get; set; }

        /// <summary>
        /// Instance the view to use for the image.
        /// </summary>
        /// <returns>View to use.</returns>
        protected override View InstaceImageView()
        {
            LabelGlyph = new ExtendedLabel()
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                LineBreakMode = Xamarin.Forms.LineBreakMode.NoWrap,
                Text = GlyphText,
                BindingContext = this
            };

            if (DebugColors)
            {
                LabelGlyph.BackgroundColor = Color.Teal;
            }

            LabelGlyph.SetBinding(ExtendedLabel.TextColorProperty, nameof(GlyphTextColor));
            LabelGlyph.SetBinding(ExtendedLabel.FontSizeProperty, nameof(GlyphFontSize));
            LabelGlyph.SetBinding(ExtendedLabel.FontNameProperty, nameof(GlyphFontName));
            LabelGlyph.SetBinding(ExtendedLabel.FriendlyFontNameProperty, nameof(GlyphFriendlyFontName));

            return LabelGlyph;
        }

        /// <summary>
        /// Set image disable or enabled.
        /// </summary>
        /// <param name="canExecute">Can execute command.</param>
        protected override void SetImageEnable(bool canExecute)
        {
            if (DisableGlyphOnly)
            {
                if (DisableBox != null)
                {
                    if (DebugColors)
                    {
                        DisableBox.BackgroundColor = new Color(Color.Fuchsia.R, Color.Fuchsia.G, Color.Fuchsia.B, 0.4);
                    }
                    else
                    {
                        DisableBox.Color = Color.Transparent;
                    }
                }

                if (canExecute)
                {
                    LabelGlyph.TextColor = GlyphTextColor;
                }
                else
                {
                    LabelGlyph.TextColor = GlyphTextColorDisabled;
                }

                if (HasText)
                {
                    SetTextEnable(canExecute);
                }
            }
            else
            {
                base.SetImageEnable(canExecute);
            }
        }
    }
}