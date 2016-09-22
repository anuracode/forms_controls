// <copyright file="ExtendedLabel.cs" company="">
// All rights reserved.
// </copyright>
// <author>https://github.com/XLabs/Xamarin-Forms-Labs</author>

using System;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls
{
    /// <summary>
    /// Class ExtendedLabel.
    /// </summary>
    public class ExtendedLabel : Label
    {
        /// <summary>
        /// The font name property.
        /// </summary>
        public static readonly BindableProperty FontNameProperty =
            BindablePropertyHelper.Create<ExtendedLabel, string>(
                nameof(FontName), string.Empty);

        /// <summary>
        /// The formatted placeholder property.
        /// </summary>
        public static readonly BindableProperty FormattedPlaceholderProperty =
            BindablePropertyHelper.Create<ExtendedLabel, FormattedString>(nameof(FormattedPlaceholder), default(FormattedString));

        /// <summary>
        /// The friendly font name property. This can be found on the first line of the font or in the font preview.
        /// This is only required on Windows Phone. If not given then the file name excl. the extension is used.
        /// </summary>
        public static readonly BindableProperty FriendlyFontNameProperty =
            BindablePropertyHelper.Create<ExtendedLabel, string>(
                nameof(FriendlyFontName), string.Empty);

        /// <summary>
        /// This is the drop shadow property
        /// </summary>
        public static readonly BindableProperty IsDropShadowProperty =
            BindablePropertyHelper.Create<ExtendedLabel, bool>(nameof(IsDropShadow), false);

        /// <summary>
        /// The is underlined property.
        /// </summary>
        public static readonly BindableProperty IsStrikeThroughProperty =
            BindablePropertyHelper.Create<ExtendedLabel, bool>(nameof(IsStrikeThrough), false);

        /// <summary>
        /// The is underlined property.
        /// </summary>
        public static readonly BindableProperty IsUnderlineProperty =
            BindablePropertyHelper.Create<ExtendedLabel, bool>(nameof(IsUnderline), false);

        /// <summary>
        /// The placeholder property.
        /// </summary>
        public static readonly BindableProperty PlaceholderProperty =
            BindablePropertyHelper.Create<ExtendedLabel, string>(nameof(Placeholder), default(string));

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedLabel"/> class.
        /// </summary>
        public ExtendedLabel()
        {
        }

        /// <summary>
        /// Gets or sets the name of the font file including extension. If no extension given then ttf is assumed.
        /// Fonts need to be included in projects accoring to the documentation.
        /// </summary>
        /// <value>The full name of the font file including extension.</value>
        public string FontName
        {
            get
            {
                return (string)GetValue(FontNameProperty);
            }
            set
            {
                SetValue(FontNameProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the FormattedString value that is used when the label's Text property is empty.
        /// </summary>
        /// <value>The placeholder FormattedString.</value>
        public FormattedString FormattedPlaceholder
        {
            get { return (FormattedString)GetValue(FormattedPlaceholderProperty); }
            set
            {
                SetValue(PlaceholderProperty, null);
                SetValue(FormattedPlaceholderProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the name of the font.
        /// </summary>
        /// <value>The name of the font.</value>
        public string FriendlyFontName
        {
            get
            {
                return (string)GetValue(FriendlyFontNameProperty);
            }
            set
            {
                SetValue(FriendlyFontNameProperty, value);
            }
        }

        /// <summary>
        /// Has drop shadow.
        /// </summary>
        public bool IsDropShadow
        {
            get
            {
                return (bool)GetValue(IsDropShadowProperty);
            }
            set
            {
                SetValue(IsDropShadowProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the text in the label is underlined.
        /// </summary>
        /// <value>A <see cref="bool"/> indicating if the text in the label should be underlined.</value>
        public bool IsStrikeThrough
        {
            get
            {
                return (bool)GetValue(IsStrikeThroughProperty);
            }
            set
            {
                SetValue(IsStrikeThroughProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the text in the label is underlined.
        /// </summary>
        /// <value>A <see cref="bool"/> indicating if the text in the label should be underlined.</value>
        public bool IsUnderline
        {
            get
            {
                return (bool)GetValue(IsUnderlineProperty);
            }
            set
            {
                SetValue(IsUnderlineProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the string value that is used when the label's Text property is empty.
        /// </summary>
        /// <value>The placeholder string.</value>
        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set
            {
                SetValue(FormattedPlaceholderProperty, null);
                SetValue(PlaceholderProperty, value);
            }
        }        
    }
}
