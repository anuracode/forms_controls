// <copyright file="ContentViewButton.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Extensions;
using Anuracode.Forms.Controls.Views.Extensions;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls
{
    /// <summary>
    /// Content view button.
    /// </summary>
    public class ContentViewButton : ContentView
    {
        /// <summary>
        /// Max heigth for control size calculation.
        /// </summary>
        public const double MAX_HEIGHT = 4000;

        /// <summary>
        /// Max width for control size calculation.
        /// </summary>
        public const double MAX_WIDTH = 4000;

        /// <summary>
        /// Background color.
        /// </summary>
        public static readonly BindableProperty ButtonBackgroundColorProperty = BindablePropertyHelper.Create<ContentViewButton, Color>(
            nameof(ButtonBackgroundColor),
            defaultValue: Color.Transparent,
            propertyChanged:
            (bindable, oldvalue, newvalue) =>
            {
                if (newvalue != null)
                {
                }
            });

        /// <summary>
        /// Disable color.
        /// </summary>
        public static readonly BindableProperty ButtonDisableBackgroundColorProperty = BindablePropertyHelper.Create<ContentViewButton, Color>(
            nameof(ButtonDisableBackgroundColor),
            defaultValue: Styles.ThemeManager.CommonResourcesBase.ButtonBackgroundColor,
            propertyChanged:
            (bindable, oldvalue, newvalue) =>
            {
                if (newvalue != null)
                {
                    var button = bindable as ContentViewButton;
                    if (button != null)
                    {
                        button.ButtonTappedCommand_CanExecuteChanged(null, EventArgs.Empty);
                    }
                }
            });

        /// <summary>
        /// Background color when tapped.
        /// </summary>
        public static readonly BindableProperty ButtonTappedBackgroundColorProperty = BindablePropertyHelper.Create<ContentViewButton, Color>(nameof(ButtonTappedBackgroundColor), new Color(Color.Black.R, Color.Black.G, Color.Black.B, 0.5));

        /// <summary>
        /// Command paramter.
        /// </summary>
        public static readonly BindableProperty CommandParameterProperty = BindablePropertyHelper.Create<ContentViewButton, object>(
            nameof(CommandParameter),
            propertyChanged:
            (bindable, oldvalue, newvalue) =>
            {
                AC.ScheduleManaged(
                    () =>
                    {
                        var button = bindable as ContentViewButton;
                        if (button != null)
                        {
                            button.ButtonTappedCommand_CanExecuteChanged(null, EventArgs.Empty);
                        }

                        return Task.FromResult(0);
                    });
            });

        /// <summary>
        /// Backing field for the command property.
        /// </summary>
        public static readonly BindableProperty CommandProperty = BindablePropertyHelper.Create<ContentViewButton, ICommand>(
            nameof(Command),
            propertyChanged:
            (bindable, oldvalue, newvalue) =>
            {
                ICommand newCommand = newvalue as ICommand;
                if (newCommand != null)
                {
                    var button = bindable as ContentViewButton;
                    if (button != null)
                    {
                        newCommand.CanExecuteChanged -= button.CanExecuteChangedHandler;
                        newCommand.CanExecuteChanged += button.CanExecuteChangedHandler;
                    }
                }
            });

        /// <summary>
        /// Content aligment.
        /// </summary>
        public static readonly BindableProperty ContentAlignmentProperty = BindablePropertyHelper.Create<ContentViewButton, TextAlignment>(nameof(ContentAlignment), TextAlignment.Center);

        /// <summary>
        /// Corner radius property.
        /// </summary>
        public static readonly BindableProperty CornerRadiusProperty = BindablePropertyHelper.Create<ContentViewButton, float>(nameof(CornerRadius), 0f);

        /// <summary>
        /// The font name property.
        /// </summary>
        public static readonly BindableProperty FontNameProperty = BindablePropertyHelper.Create<ContentViewButton, string>(nameof(FontName), string.Empty);

        /// <summary>
        /// Font size.
        /// </summary>
        public static readonly BindableProperty FontSizeProperty = BindablePropertyHelper.Create<ContentViewButton, double>(nameof(FontSize), Styles.ThemeManager.CommonResourcesBase.TextSizeMedium);

        /// <summary>
        /// The formatted placeholder property.
        /// </summary>
        public static readonly BindableProperty FormattedPlaceholderProperty = BindablePropertyHelper.Create<ContentViewButton, FormattedString>(nameof(FormattedPlaceholder), default(FormattedString));

        /// <summary>
        /// Formatted String.
        /// </summary>
        public static readonly BindableProperty FormattedTextProperty = BindablePropertyHelper.Create<ContentViewButton, FormattedString>(nameof(FormattedText), default(FormattedString));

        /// <summary>
        /// The friendly font name property. This can be found on the first line of the font or in the font preview.
        /// This is only required on Windows Phone. If not given then the file name excl. the extension is used.
        /// </summary>
        public static readonly BindableProperty FriendlyFontNameProperty = BindablePropertyHelper.Create<ContentViewButton, string>(nameof(FriendlyFontName), string.Empty);

        /// <summary>
        /// Backing field for the image height property.
        /// </summary>
        public static readonly BindableProperty ImageHeightRequestProperty = BindablePropertyHelper.Create<ContentViewButton, double>(nameof(ImageHeightRequest), (double)30);

        /// <summary>
        /// Backing field for the image width property.
        /// </summary>
        public static readonly BindableProperty ImageWidthRequestProperty = BindablePropertyHelper.Create<ContentViewButton, double>(nameof(ImageWidthRequest), (double)30);

        /// <summary>
        /// Flag to make button invisible if disabled.
        /// </summary>
        public static readonly BindableProperty InvisibleWhenDisabledProperty = BindablePropertyHelper.Create<ContentViewButton, bool>(
            nameof(InvisibleWhenDisabled),
            false,
            propertyChanged:
            (bindable, oldvalue, newvalue) =>
            {
                var button = bindable as ContentViewButton;
                if (button != null)
                {
                    button.ButtonTappedCommand_CanExecuteChanged(null, EventArgs.Empty);
                }
            });

        /// <summary>
        /// Is underline.
        /// </summary>
        public static readonly BindableProperty IsUnderlineProperty = BindablePropertyHelper.Create<ContentViewButton, bool>(nameof(IsUnderline), false);

        /// <summary>
        /// Is visible property.
        /// </summary>
        public static readonly BindableProperty IsVisibleContentProperty = BindableProperty.Create(
            nameof(IsVisibleContent),
            typeof(bool),
            typeof(ContentViewButton),
            true,
            propertyChanged:
            (bindable, oldvalue, newvalue) =>
            {
                var button = bindable as ContentViewButton;
                if (button != null)
                {
                    button.IsVisible = (bool)newvalue;

                    if (!button.IsVisible && (bool)newvalue)
                    {
                        button.ButtonTappedCommand_CanExecuteChanged(null, EventArgs.Empty);
                    }
                }
            });

        /// <summary>
        /// Linebreak mode.
        /// </summary>
        public static readonly BindableProperty LineBreakModeProperty = BindablePropertyHelper.Create<ContentViewButton, LineBreakMode>(nameof(LineBreakMode), LineBreakMode.NoWrap);

        /// <summary>
        /// Margin borders.
        /// </summary>
        public static readonly BindableProperty MarginBordersProperty = BindablePropertyHelper.Create<ContentViewButton, double>(nameof(MarginBorders), 0);

        /// <summary>
        /// Margin between elements.
        /// </summary>
        public static readonly BindableProperty MarginElementsProperty = BindablePropertyHelper.Create<ContentViewButton, double>(nameof(MarginElements), 0);

        /// <summary>
        /// The placeholder property.
        /// </summary>
        public static readonly BindableProperty PlaceholderProperty = BindablePropertyHelper.Create<ContentViewButton, string>(nameof(Placeholder), default(string));

        /// <summary>
        /// Shape type.
        /// </summary>
        public static readonly BindableProperty ShapeTypeProperty = BindablePropertyHelper.Create<ContentViewButton, ShapeType>(nameof(ShapeType), ShapeType.Box);

        /// <summary>
        /// Backing field for the Image property.
        /// </summary>
        public static readonly BindableProperty SourceProperty = BindablePropertyHelper.Create<ContentViewButton, ImageSource>(
            nameof(Source),
            (ImageSource)null,
            propertyChanged:
            (bindable, oldvalue, newvalue) =>
            {
                ImageSource newImageSource = newvalue as ImageSource;
                if (newImageSource != null)
                {
                    var button = bindable as ContentViewButton;
                    if (button != null)
                    {
                        if (button.ButtonImage != null)
                        {
                            button.ButtonImage.Source = newImageSource;
                        }
                    }
                }
            });

        /// <summary>
        /// Shape color.
        /// </summary>
        public static readonly BindableProperty StrokeColorProperty = BindablePropertyHelper.Create<ContentViewButton, Color>(nameof(StrokeColor), Color.Default);

        /// <summary>
        /// Stroke width.
        /// </summary>
        public static readonly BindableProperty StrokeWidthProperty = BindablePropertyHelper.Create<ContentViewButton, float>(nameof(StrokeWidth), 1f);

        /// <summary>
        /// Text color disabled.
        /// </summary>
        public static readonly BindableProperty TextColorDisabledProperty = BindablePropertyHelper.Create<ContentViewButton, Color>(
            nameof(TextColorDisabled),
            Styles.ThemeManager.CommonResourcesBase.TextColorDisable,
            propertyChanged:
            (bindable, oldvalue, newvalue) =>
            {
                if (newvalue != null)
                {
                    var button = bindable as ContentViewButton;
                    if (button != null)
                    {
                        button.ButtonTappedCommand_CanExecuteChanged(null, EventArgs.Empty);
                    }
                }
            });

        /// <summary>
        /// Text color property.
        /// </summary>
        public static readonly BindableProperty TextColorProperty = BindablePropertyHelper.Create<ContentViewButton, Color>(
            nameof(TextColor),
             Color.White,
            propertyChanged:
            (bindable, oldvalue, newvalue) =>
            {
                if (newvalue != null)
                {
                    var button = bindable as ContentViewButton;
                    if (button != null)
                    {
                        button.ButtonTappedCommand_CanExecuteChanged(null, EventArgs.Empty);
                    }
                }
            });

        /// <summary>
        /// Text property.
        /// </summary>
        public static readonly BindableProperty TextProperty = BindablePropertyHelper.Create<ContentViewButton, string>(nameof(Text), (string)null);

        /// <summary>
        /// Disable color.
        /// </summary>
        public static readonly BindableProperty UseDisableBoxProperty = BindablePropertyHelper.Create<ContentViewButton, bool>(
            nameof(UseDisableBox),
            true,
            propertyChanged:
            (bindable, oldvalue, newvalue) =>
            {
                var button = bindable as ContentViewButton;
                if (button != null)
                {
                    button.ButtonTappedCommand_CanExecuteChanged(null, EventArgs.Empty);
                }
            });

        /// <summary>
        /// Debug colors working.
        /// </summary>
        protected virtual bool DebugColors
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Control orientation.
        /// </summary>
        protected readonly ImageOrientation orientation;

        /// <summary>
        /// Button tapped command.
        /// </summary>
        private Command buttonTappedCommand;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="hasText">Has text.</param>
        /// <param name="hasImage">Has image.</param>
        /// <param name="orientation">Orientation.</param>
        public ContentViewButton(bool hasText, bool hasImage, ImageOrientation orientation)
            : this(hasText, hasImage)
        {
            this.orientation = orientation;
            RenderContent(hasText, hasImage, orientation);
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        protected ContentViewButton(bool hasText, bool hasImage)
        {
            Padding = 0;
            HasText = hasText;
            HasImage = hasImage;
        }

        /// <summary>
        /// Background color.
        /// </summary>
        public Color ButtonBackgroundColor
        {
            get
            {
                return (Color)GetValue(ButtonBackgroundColorProperty);
            }

            set
            {
                SetValue(ButtonBackgroundColorProperty, value);
            }
        }

        /// <summary>
        /// Disabled background color.
        /// </summary>
        public Color ButtonDisableBackgroundColor
        {
            get
            {
                return (Color)GetValue(ButtonDisableBackgroundColorProperty);
            }

            set
            {
                SetValue(ButtonDisableBackgroundColorProperty, value);
            }
        }

        /// <summary>
        /// Delegate for an animation when the button is tapped.
        /// </summary>
        public Func<View, Task> ButtonTappedAnimationDelegate { get; set; }

        /// <summary>
        /// Tapped background color.
        /// </summary>
        public Color ButtonTappedBackgroundColor
        {
            get
            {
                return (Color)GetValue(ButtonTappedBackgroundColorProperty);
            }

            set
            {
                SetValue(ButtonTappedBackgroundColorProperty, value);
            }
        }

        /// <summary>
        /// Button tapped command.
        /// </summary>
        public Command ButtonTappedCommand
        {
            get
            {
                if (buttonTappedCommand == null)
                {
                    buttonTappedCommand = new Command(
                        () =>
                        {
                            if (BackgroundBox != null)
                            {
                                AC.ScheduleManaged(
                                    async () =>
                                    {
                                        TappedBox.Opacity = 1;

                                        await Task.Delay(TimeSpan.FromSeconds(0.1));

                                        if (ButtonTappedAnimationDelegate != null)
                                        {
                                            await ButtonTappedAnimationDelegate(this);
                                        }

                                        await Task.Delay(TimeSpan.FromSeconds(0.1));

                                        TappedBox.Opacity = 0;

                                        if (Command != null && Command.CanExecute(CommandParameter))
                                        {
                                            Command.Execute(CommandParameter);
                                        }
                                    });
                            }
                        },
                        () =>
                        {
                            return (Command != null) && Command.CanExecute(CommandParameter);
                        });
                }

                return buttonTappedCommand;
            }
        }

        /// <summary>
        /// Command of the button.
        /// </summary>
        public ICommand Command
        {
            get
            {
                return (ICommand)GetValue(CommandProperty);
            }

            set
            {
                SetValue(CommandProperty, value);
            }
        }

        /// <summary>
        /// Command paramter.
        /// </summary>
        public object CommandParameter
        {
            get
            {
                return (object)GetValue(CommandParameterProperty);
            }

            set
            {
                SetValue(CommandParameterProperty, value);
            }
        }

        /// <summary>
        /// Content aligment.
        /// </summary>
        public TextAlignment ContentAlignment
        {
            get
            {
                return (TextAlignment)GetValue(ContentAlignmentProperty);
            }

            set
            {
                SetValue(ContentAlignmentProperty, value);
            }
        }

        /// <summary>
        /// Corder radius.
        /// </summary>
        public float CornerRadius
        {
            get
            {
                return (float)GetValue(CornerRadiusProperty);
            }

            set
            {
                if (ShapeType != ShapeType.Box)
                {
                    throw new ArgumentException("Can only specify this property with Box");
                }

                SetValue(CornerRadiusProperty, value);
            }
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
        /// Font size.
        /// </summary>
        public double FontSize
        {
            get
            {
                return (double)GetValue(FontSizeProperty);
            }

            set
            {
                SetValue(FontSizeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the FormattedString value that is used when the label's Text property is empty.
        /// </summary>
        /// <value>The placeholder FormattedString.</value>
        public FormattedString FormattedPlaceholder
        {
            get
            {
                return (FormattedString)GetValue(FormattedPlaceholderProperty);
            }

            set
            {
                SetValue(FormattedPlaceholderProperty, value);
            }
        }

        /// <summary>
        /// Formated string.
        /// </summary>
        public FormattedString FormattedText
        {
            get
            {
                return (FormattedString)GetValue(FormattedTextProperty);
            }

            set
            {
                SetValue(FormattedTextProperty, value);
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
        /// Image aspect.
        /// </summary>
        public Aspect ImageAspect
        {
            get
            {
                return ButtonImage == null ? Aspect.AspectFit : ButtonImage.Aspect;
            }
            set
            {
                if (ButtonImage != null)
                {
                    ButtonImage.Aspect = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the requested height of the image.  If less than or equal to zero than a
        /// height of 50 will be used.
        /// </summary>
        /// <value>
        /// The ImageHeightRequest property gets/sets the value of the backing field, ImageHeightRequestProperty.
        /// </value>
        public double ImageHeightRequest
        {
            get
            {
                return (double)GetValue(ImageHeightRequestProperty);
            }

            set
            {
                SetValue(ImageHeightRequestProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the requested width of the image.  If less than or equal to zero than a
        /// width of 50 will be used.
        /// </summary>
        /// <value>
        /// The ImageHeightRequest property gets/sets the value of the backing field, ImageHeightRequestProperty.
        /// </value>
        public double ImageWidthRequest
        {
            get
            {
                return (double)GetValue(ImageWidthRequestProperty);
            }

            set
            {
                SetValue(ImageWidthRequestProperty, value);
            }
        }

        /// <summary>
        /// Flag to make button invisible if disabled.
        /// </summary>
        public bool InvisibleWhenDisabled
        {
            get
            {
                return (bool)GetValue(InvisibleWhenDisabledProperty);
            }

            set
            {
                SetValue(InvisibleWhenDisabledProperty, value);
            }
        }

        /// <summary>
        /// Is Underline.
        /// </summary>
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
        /// Replace for the is visible, used to force a relayout.
        /// </summary>
        public bool IsVisibleContent
        {
            get
            {
                return (bool)GetValue(IsVisibleContentProperty);
            }

            set
            {
                SetValue(IsVisibleContentProperty, value);
            }
        }

        /// <summary>
        /// Line Break Mode
        /// </summary>
        public LineBreakMode LineBreakMode
        {
            get
            {
                return (LineBreakMode)GetValue(LineBreakModeProperty);
            }

            set
            {
                SetValue(LineBreakModeProperty, value);
            }
        }

        /// <summary>
        /// Margin borders.
        /// </summary>
        public double MarginBorders
        {
            get
            {
                return (double)GetValue(MarginBordersProperty);
            }

            set
            {
                SetValue(MarginBordersProperty, value);
            }
        }

        /// <summary>
        /// Margin between elements.
        /// </summary>
        public double MarginElements
        {
            get
            {
                return (double)GetValue(MarginElementsProperty);
            }

            set
            {
                SetValue(MarginElementsProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the string value that is used when the label's Text property is empty.
        /// </summary>
        /// <value>The placeholder string.</value>
        public string Placeholder
        {
            get
            {
                return (string)GetValue(PlaceholderProperty);
            }

            set
            {
                SetValue(FormattedPlaceholderProperty, null);
                SetValue(PlaceholderProperty, value);
            }
        }

        /// <summary>
        /// Shape type.
        /// </summary>
        public ShapeType ShapeType
        {
            get
            {
                return (ShapeType)GetValue(ShapeTypeProperty);
            }

            set
            {
                SetValue(ShapeTypeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the ImageSource to use with the control.
        /// </summary>
        /// <value>
        /// The Source property gets/sets the value of the backing field, SourceProperty.
        /// </value>
        [TypeConverter(typeof(Xamarin.Forms.ImageSourceConverter))]
        public ImageSource Source
        {
            get
            {
                return (ImageSource)GetValue(SourceProperty);
            }

            set
            {
                SetValue(SourceProperty, value);
            }
        }

        /// <summary>
        /// Color stroke.
        /// </summary>
        public Color StrokeColor
        {
            get
            {
                return (Color)GetValue(StrokeColorProperty);
            }

            set
            {
                SetValue(StrokeColorProperty, value);
            }
        }

        /// <summary>
        /// Stroke width.
        /// </summary>
        public float StrokeWidth
        {
            get
            {
                return (float)GetValue(StrokeWidthProperty);
            }

            set
            {
                SetValue(StrokeWidthProperty, value);
            }
        }

        /// <summary>
        /// Text value.
        /// </summary>
        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }

            set
            {
                SetValue(TextProperty, value);
            }
        }

        /// <summary>
        /// Text color.
        /// </summary>
        public Color TextColor
        {
            get
            {
                return (Color)GetValue(TextColorProperty);
            }

            set
            {
                SetValue(TextColorProperty, value);
            }
        }

        /// <summary>
        /// Text color disabled.
        /// </summary>
        public Color TextColorDisabled
        {
            get
            {
                return (Color)GetValue(TextColorDisabledProperty);
            }

            set
            {
                SetValue(TextColorDisabledProperty, value);
            }
        }

        /// <summary>
        /// Use disable box for showing disable state.
        /// </summary>
        public bool UseDisableBox
        {
            get
            {
                return (bool)GetValue(UseDisableBoxProperty);
            }

            set
            {
                SetValue(UseDisableBoxProperty, value);
            }
        }

        /// <summary>
        /// Background box.
        /// </summary>
        protected ShapeView BackgroundBox { get; set; }

        /// <summary>
        /// Border box.
        /// </summary>
        protected ShapeView BorderBox { get; set; }

        /// <summary>
        /// Image for the button.
        /// </summary>
        protected ExtendedImage ButtonImage { get; set; }

        /// <summary>
        /// Button image.
        /// </summary>
        protected View ButtonImageView { get; set; }

        /// <summary>
        /// Content layout.
        /// </summary>
        protected SimpleLayout ContentLayout { get; set; }

        /// <summary>
        /// Box for the disable.
        /// </summary>
        protected ShapeView DisableBox { get; set; }

        /// <summary>
        /// Gesture box.
        /// </summary>
        protected BoxView GesturesBox { get; set; }

        /// <summary>
        /// Has image.
        /// </summary>
        protected bool HasImage { get; private set; }

        /// <summary>
        /// Has text.
        /// </summary>
        protected bool HasText { get; private set; }

        /// <summary>
        /// Flag to determine if the image size is variable, used with the glyphs.
        /// </summary>
        protected virtual bool IsImageSizeVariable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Box when the button is tapped.
        /// </summary>
        protected ShapeView TappedBox { get; set; }

        /// <summary>
        /// Template content view.
        /// </summary>
        protected View TemplateContentView { get; set; }

        /// <summary>
        /// Extended label.
        /// </summary>
        protected ExtendedLabel TextExtendedLabel { get; set; }

        /// <summary>
        /// Button tapped command changed.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Arguments of the event.</param>
        protected virtual void ButtonTappedCommand_CanExecuteChanged(object sender, EventArgs e)
        {
            AC.ScheduleManaged(
                TimeSpan.FromSeconds(0.1),
                () =>
                {
                    bool canExecute = ButtonTappedCommand.CanExecute(null);

                    SetContentEnable(canExecute);

                    return Task.FromResult(0);
                });
        }

        /// <summary>
        /// Event when the can execute changes.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Arguments of the event.</param>
        protected void CanExecuteChangedHandler(object sender, EventArgs e)
        {
            if (ButtonTappedCommand != null)
            {
                ButtonTappedCommand_CanExecuteChanged(null, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Layout children.
        /// </summary>
        /// <param name="x">Top to use.</param>
        /// <param name="y">Left to use.</param>
        /// <param name="width">Width to use.</param>
        /// <param name="height">Height to use.</param>
        protected virtual void ContentLayout_OnLayoutChildren(double x, double y, double width, double height)
        {
            Rectangle controlSizeWithMargin = new Rectangle(x + MarginBorders, y + MarginBorders, width - (MarginBorders * 2), height - (MarginBorders * 2));
            Rectangle controlSize = new Rectangle(x, y, width, height);

            BackgroundBox.LayoutUpdate(controlSize);
            BorderBox.LayoutUpdate(controlSize);
            TappedBox.LayoutUpdate(controlSize);
            DisableBox.LayoutUpdate(controlSize);
            GesturesBox.LayoutUpdate(controlSize);

            if (TemplateContentView != null)
            {
                TemplateContentView.LayoutUpdate(controlSizeWithMargin);
            }

            ContentLayout_OnLayoutTextAndImageChildren(controlSize.X, controlSize.Y, controlSize.Width, controlSize.Height);
        }

        /// <summary>
        /// Layout children.
        /// </summary>
        /// <param name="x">Top to use.</param>
        /// <param name="y">Left to use.</param>
        /// <param name="width">Width to use.</param>
        /// <param name="height">Height to use.</param>
        protected virtual void ContentLayout_OnLayoutTextAndImageChildren(double x, double y, double width, double height)
        {
            if (HasText || HasImage)
            {
                if (HasText && !HasImage)
                {
                    if (TextExtendedLabel != null)
                    {
                        var requestImageSize = TextExtendedLabel.Measure(width, height).Request;
                        double textWidth = requestImageSize.Width.Clamp(0, width);
                        double textHeight = requestImageSize.Height.Clamp(0, height);
                        double textX = (width - textWidth) * 0.5f;
                        double textY = (height - textHeight) * 0.5f;

                        Rectangle textPosition = new Rectangle(x + textX, y + textY, textWidth, textHeight);

                        TextExtendedLabel.LayoutUpdate(textPosition);
                    }
                }
                else if (!HasText && HasImage)
                {
                    if (ButtonImageView != null)
                    {
                        var requestImageSize = ButtonImageView.Measure(width, height).Request;
                        double imageWidth = requestImageSize.Width.Clamp(0, ImageWidthRequest);
                        double imageHeight = requestImageSize.Height.Clamp(0, ImageHeightRequest);

                        if (ImageAspect == Aspect.AspectFill)
                        {
                            imageWidth = ImageWidthRequest;
                            imageHeight = ImageHeightRequest;
                        }

                        double imageX = (width - imageWidth) * 0.5f;
                        double imageY = (height - imageHeight) * 0.5f;

                        if (imageHeight <= 0)
                        {
                            imageHeight = ImageHeightRequest;
                        }

                        if (imageWidth <= 0)
                        {
                            imageHeight = ImageWidthRequest;
                        }

                        Rectangle imagePosition = new Rectangle(x + imageX, y + imageY, imageWidth, imageHeight);

                        ButtonImageView.LayoutUpdate(imagePosition);
                    }
                }
                else
                {
                    var requestTextSize = TextExtendedLabel.Measure(width, height).Request;
                    var requestImageSize = ButtonImageView.Measure(width, height).Request;

                    double imageX = 0;
                    double imageY = 0;
                    double imageWidth = requestImageSize.Width.Clamp(0, ImageWidthRequest);
                    double imageHeight = requestImageSize.Height.Clamp(0, ImageHeightRequest);
                    double textX = 0;
                    double textY = 0;
                    double textWidth = requestTextSize.Width.Clamp(0, width);
                    double textHeight = requestTextSize.Height.Clamp(0, height);

                    if (imageHeight <= 0)
                    {
                        imageHeight = ImageHeightRequest;
                    }

                    if (imageWidth <= 0)
                    {
                        imageHeight = ImageWidthRequest;
                    }

                    switch (orientation)
                    {
                        case ImageOrientation.ImageOnBottom:
                            textX = (width - textWidth) * 0.5;
                            textY = 0;
                            imageX = (width - imageWidth) * 0.5f;
                            imageY = textY + MarginElements + textHeight;

                            // Center content in Y
                            double contentYdiff1 = (height - (imageHeight + MarginElements + textHeight));

                            switch (ContentAlignment)
                            {
                                case TextAlignment.End:
                                    textY += contentYdiff1;
                                    imageY += contentYdiff1;
                                    break;

                                case TextAlignment.Start:
                                    break;

                                case TextAlignment.Center:
                                default:
                                    textY += contentYdiff1 * 0.5f;
                                    imageY += contentYdiff1 * 0.5f;
                                    break;
                            }

                            break;

                        case ImageOrientation.ImageOnTop:
                            imageX = (width - imageWidth) * 0.5f;
                            imageY = 0;
                            textX = (width - textWidth) * 0.5f;
                            textY = imageY + MarginElements + imageHeight;

                            // Center content in Y
                            double contentYdiff2 = (height - (imageHeight + MarginElements + textHeight));

                            switch (ContentAlignment)
                            {
                                case TextAlignment.End:
                                    textY += contentYdiff2;
                                    imageY += contentYdiff2;
                                    break;

                                case TextAlignment.Start:
                                    textY += MarginBorders;
                                    imageY += MarginBorders;
                                    break;

                                case TextAlignment.Center:
                                default:
                                    textY += contentYdiff2 * 0.5f;
                                    imageY += contentYdiff2 * 0.5f;
                                    break;
                            }

                            break;

                        case ImageOrientation.ImageToLeft:
                            imageX = 0;
                            imageY = (height - imageHeight) * 0.5f;
                            textX = imageWidth + MarginElements;
                            textWidth = (width - textX) - (MarginBorders * 2f);
                            textY = (height - textHeight) * 0.5f;

                            // Center content in X
                            double contentXdiff2 = (width - (imageWidth + MarginElements + textHeight));

                            switch (ContentAlignment)
                            {
                                case TextAlignment.End:
                                    textX += contentXdiff2;
                                    imageX += contentXdiff2;
                                    break;

                                case TextAlignment.Start:
                                    textX += MarginBorders;
                                    imageX += MarginBorders;
                                    break;

                                case TextAlignment.Center:
                                default:
                                    textX += contentXdiff2 * 0.5f;
                                    imageX += contentXdiff2 * 0.5f;
                                    break;
                            }

                            break;

                        case ImageOrientation.ImageToRight:
                            textX = 0;
                            textY = (height - textHeight) * 0.5f;
                            textWidth = width - imageWidth - MarginElements - (MarginBorders * 2f);
                            imageX = textX + textWidth + MarginElements;
                            imageY = (height - imageHeight) * 0.5f;

                            // Center content in X
                            double contentXdiff1 = (width - (imageWidth + MarginElements + textHeight));

                            switch (ContentAlignment)
                            {
                                case TextAlignment.End:
                                    textX += contentXdiff1;
                                    imageX += contentXdiff1;
                                    break;

                                case TextAlignment.Start:
                                    textX += MarginBorders;
                                    imageX += MarginBorders;
                                    break;

                                case TextAlignment.Center:
                                default:
                                    textX += contentXdiff1 * 0.5f;
                                    imageX += contentXdiff1 * 0.5f;
                                    break;
                            }

                            break;

                        default:
                            break;
                    }

                    Rectangle imagePosition = new Rectangle(x + imageX, y + imageY, imageWidth, imageHeight);
                    Rectangle textPosition = new Rectangle(x + textX, y + textY, textWidth, textHeight);

                    if (ButtonImageView != null)
                    {
                        ButtonImageView.LayoutUpdate(imagePosition);
                    }

                    if (TextExtendedLabel != null)
                    {
                        TextExtendedLabel.LayoutUpdate(textPosition);
                    }
                }
            }
        }

        /// <summary>
        /// Mesure the content.
        /// </summary>
        /// <param name="widthConstraint">Width to constarint.</param>
        /// <param name="heightConstraint">Height to constraint.</param>
        /// <returns></returns>
        protected SizeRequest ContentLayout_OnSizeRequest(double widthConstraint, double heightConstraint)
        {
            double heightConstraintCustom = heightConstraint;
            double widthConstraintCustom = widthConstraint;

            if (HeightRequest > 0)
            {
                heightConstraintCustom = (HeightRequest - MarginBorders).Clamp(0, heightConstraint);
            }

            if (WidthRequest > 0)
            {
                widthConstraintCustom = (WidthRequest - MarginBorders).Clamp(0, widthConstraint);
            }

            SizeRequest resultRequest = new SizeRequest(new Size(widthConstraintCustom, heightConstraintCustom), new Size(widthConstraintCustom, heightConstraintCustom));

            if (WidthRequest > 0 && HeightRequest > 0)
            {
                return resultRequest;
            }

            if (TemplateContentView != null)
            {
                resultRequest = TemplateContentView.Measure(widthConstraintCustom, heightConstraintCustom);
            }
            else
            {
                if (HasText || HasImage)
                {
                    if (HasText && !HasImage)
                    {
                        if (TextExtendedLabel != null)
                        {
                            resultRequest = TextExtendedLabel.Measure(widthConstraintCustom, heightConstraintCustom);
                        }
                    }
                    else if (!HasText && HasImage)
                    {
                        if ((ButtonImageView != null) && IsImageSizeVariable)
                        {
                            resultRequest = ButtonImageView.Measure(widthConstraintCustom, heightConstraintCustom);
                        }
                        else
                        {
                            resultRequest = new SizeRequest(new Size(ImageWidthRequest + MarginBorders, ImageHeightRequest + MarginBorders), new Size(ImageWidthRequest + MarginBorders, ImageHeightRequest + MarginBorders));
                        }
                    }
                    else
                    {
                        SizeRequest textSize = new SizeRequest();

                        double contentWidth = 0;
                        double contentHeight = 0;

                        if (TextExtendedLabel != null)
                        {
                            textSize = TextExtendedLabel.Measure(widthConstraintCustom, heightConstraintCustom);
                        }

                        switch (orientation)
                        {
                            case ImageOrientation.ImageOnBottom:
                            case ImageOrientation.ImageOnTop:
                                contentHeight = ImageHeightRequest + MarginElements + textSize.Request.Height;
                                contentWidth = Math.Max(ImageWidthRequest, textSize.Request.Width);
                                break;

                            case ImageOrientation.ImageToLeft:
                            case ImageOrientation.ImageToRight:
                                contentHeight = Math.Max(ImageHeightRequest, textSize.Request.Height);
                                contentWidth = ImageWidthRequest + MarginElements + textSize.Request.Width;
                                break;

                            default:
                                break;
                        }

                        if (HeightRequest > 0)
                        {
                            contentHeight = HeightRequest;
                        }

                        if (WidthRequest > 0)
                        {
                            contentWidth = WidthRequest;
                        }

                        contentWidth = contentWidth.Clamp(0, widthConstraintCustom);
                        contentHeight = contentHeight.Clamp(0, heightConstraintCustom);

                        resultRequest = new SizeRequest(new Size(contentWidth, contentHeight), new Size(contentWidth, contentHeight));
                    }
                }
            }

            var bothMargins = MarginBorders * 2;

            var resultRequestWithMargin = new SizeRequest(new Size(resultRequest.Request.Width + bothMargins, resultRequest.Request.Height + bothMargins), new Size(resultRequest.Minimum.Width + bothMargins, resultRequest.Minimum.Height + bothMargins));

            return resultRequestWithMargin;
        }

        /// <summary>
        /// Instance the view to use for the image.
        /// </summary>
        /// <returns>View to use.</returns>
        protected virtual View InstaceImageView()
        {
            ButtonImage = new ExtendedImage()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Aspect = ImageAspect,
            };

            if (DebugColors)
            {
                ButtonImage.BackgroundColor = Color.Teal;
            }

            return ButtonImage;
        }        

        /// <summary>
        /// Mesure the content.
        /// </summary>
        /// <param name="widthConstraint">Width to constarint.</param>
        /// <param name="heightConstraint">Height to constraint.</param>
        /// <returns>Size to use.</returns>
        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            SizeRequest request = new SizeRequest();

            if (ContentLayout != null)
            {
                request = ContentLayout.Measure(widthConstraint, heightConstraint);
            }

            return request;
        }

        /// <summary>
        /// Render button content.
        /// </summary>
        /// <param name="hasText">Has text.</param>
        /// <param name="hasImage">Has image.</param>
        /// <param name="orientation">Orientation.</param>
        /// <returns>View to use.</returns>
        protected virtual View RenderButtonContent(bool hasText, bool hasImage, ImageOrientation orientation)
        {
            return null;
        }

        /// <summary>
        /// Render button content.
        /// </summary>
        /// <param name="hasText">Has text.</param>
        /// <param name="hasImage">Has image.</param>
        /// <param name="orientation">Orientation.</param>
        /// <returns>View to use.</returns>
        protected virtual void RenderContent(bool hasText, bool hasImage, ImageOrientation orientation)
        {
            ContentLayout = new SimpleLayout()
            {
                IsHandlingLayoutManually = true,
                Padding = 0,
                BindingContext = this,
                IsClippedToBounds = true
            };

            ContentLayout.SetBinding<ContentViewButton>(SimpleLayout.VerticalOptionsProperty, vm => vm.VerticalOptions);
            ContentLayout.SetBinding<ContentViewButton>(SimpleLayout.HorizontalOptionsProperty, vm => vm.HorizontalOptions);

            ContentLayout.ManualSizeCalculationDelegate = ContentLayout_OnSizeRequest;
            ContentLayout.OnLayoutChildren += ContentLayout_OnLayoutChildren;

            if (DebugColors)
            {
                ContentLayout.BackgroundColor = new Color(Color.Purple.R, Color.Purple.G, Color.Purple.B, 0.4);
            }

            // Background.
            BackgroundBox = new ShapeView()
            {
                BindingContext = this
            };

            BackgroundBox.SetBinding<ContentViewButton>(ShapeView.ColorProperty, vm => vm.ButtonBackgroundColor);
            BackgroundBox.SetBinding<ContentViewButton>(ShapeView.StrokeColorProperty, vm => vm.StrokeColor);
            BackgroundBox.SetBinding<ContentViewButton>(ShapeView.StrokeWidthProperty, vm => vm.StrokeWidth);
            BackgroundBox.SetBinding<ContentViewButton>(ShapeView.ShapeTypeProperty, vm => vm.ShapeType);
            BackgroundBox.SetBinding<ContentViewButton>(ShapeView.CornerRadiusProperty, vm => vm.CornerRadius);
            BackgroundBox.SetBinding<ContentViewButton>(ShapeView.IsVisibleProperty, vm => vm.IsVisible);

            // Tapped view
            TappedBox = new ShapeView()
            {
                BindingContext = this,
                Opacity = 0
            };

            TappedBox.SetBinding<ContentViewButton>(ShapeView.ColorProperty, vm => vm.ButtonTappedBackgroundColor);
            TappedBox.SetBinding<ContentViewButton>(ShapeView.StrokeColorProperty, vm => vm.StrokeColor);
            TappedBox.SetBinding<ContentViewButton>(ShapeView.StrokeWidthProperty, vm => vm.StrokeWidth);
            TappedBox.SetBinding<ContentViewButton>(ShapeView.ShapeTypeProperty, vm => vm.ShapeType);
            TappedBox.SetBinding<ContentViewButton>(ShapeView.CornerRadiusProperty, vm => vm.CornerRadius);

            if (hasImage)
            {
                ButtonImageView = InstaceImageView();
            }

            if (hasText)
            {
                // Text
                TextExtendedLabel = new ExtendedLabel()
                {
                    BindingContext = this,
                    VerticalOptions = LayoutOptions.Start,
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalTextAlignment = TextAlignment.Center,
                    HorizontalTextAlignment = TextAlignment.Center
                };

                if (DebugColors)
                {
                    TextExtendedLabel.BackgroundColor = new Color(Color.Blue.R, Color.Blue.G, Color.Blue.B, 0.4);
                }

                TextExtendedLabel.SetBinding<ContentViewButton>(ExtendedLabel.IsUnderlineProperty, vm => vm.IsUnderline);
                TextExtendedLabel.SetBinding<ContentViewButton>(ExtendedLabel.LineBreakModeProperty, vm => vm.LineBreakMode);
                TextExtendedLabel.SetBinding<ContentViewButton>(ExtendedLabel.FontNameProperty, vm => vm.FontName);
                TextExtendedLabel.SetBinding<ContentViewButton>(ExtendedLabel.FontSizeProperty, vm => vm.FontSize);
                TextExtendedLabel.SetBinding<ContentViewButton>(ExtendedLabel.FormattedPlaceholderProperty, vm => vm.FormattedPlaceholder);
                TextExtendedLabel.SetBinding<ContentViewButton>(ExtendedLabel.PlaceholderProperty, vm => vm.Placeholder);
                TextExtendedLabel.SetBinding<ContentViewButton>(ExtendedLabel.FriendlyFontNameProperty, vm => vm.FriendlyFontName);
                TextExtendedLabel.SetBinding<ContentViewButton>(ExtendedLabel.TextProperty, vm => vm.Text);
                TextExtendedLabel.SetBinding<ContentViewButton>(ExtendedLabel.FormattedTextProperty, vm => vm.FormattedText);

                if (HasImage)
                {
                    TextExtendedLabel.SetBinding<ContentViewButton>(ExtendedLabel.TextColorProperty, vm => vm.TextColor);
                }
                else
                {
                    TextExtendedLabel.SetBinding<ContentViewButton>(ExtendedLabel.TextColorProperty, vm => vm.TextColorDisabled);
                }
            }

            TemplateContentView = RenderButtonContent(hasText, hasImage, orientation);

            // Tapped trigger and disable view.
            DisableBox = new ShapeView()
            {
                BackgroundColor = Color.Transparent,
                BindingContext = this,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            DisableBox.SetBinding<ContentViewButton>(ShapeView.ShapeTypeProperty, vm => vm.ShapeType);
            DisableBox.SetBinding<ContentViewButton>(ShapeView.CornerRadiusProperty, vm => vm.CornerRadius);

            // Define tap area.
            GesturesBox = new BoxView()
            {
                Color = Color.Transparent,
                BindingContext = this,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            TapGestureRecognizer tapRecognizer = new TapGestureRecognizer()
            {
                Command = ButtonTappedCommand
            };

            GesturesBox.GestureRecognizers.Add(tapRecognizer);

            BorderBox = new ShapeView()
            {
                BackgroundColor = Color.Transparent,
                BindingContext = this,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            BorderBox.SetBinding<ContentViewButton>(ShapeView.ShapeTypeProperty, vm => vm.ShapeType);
            BorderBox.SetBinding<ContentViewButton>(ShapeView.CornerRadiusProperty, vm => vm.CornerRadius);
            BorderBox.SetBinding<ContentViewButton>(ShapeView.StrokeColorProperty, vm => vm.StrokeColor);
            BorderBox.SetBinding<ContentViewButton>(ShapeView.StrokeWidthProperty, vm => vm.StrokeWidth);

            // Add layers.
            ContentLayout.Children.Add(BackgroundBox);

            if (TextExtendedLabel != null)
            {
                ContentLayout.Children.Add(TextExtendedLabel);
            }

            if (ButtonImageView != null)
            {
                ContentLayout.Children.Add(ButtonImageView);
            }

            if (TemplateContentView != null)
            {
                ContentLayout.Children.Add(TemplateContentView);
            }

            if (HasImage)
            {
                if (DisableBox.Color != ButtonDisableBackgroundColor)
                {
                    DisableBox.Color = ButtonDisableBackgroundColor;
                }

                DisableBox.Opacity = 1;
            }

            ContentLayout.Children.Add(BorderBox);
            ContentLayout.Children.Add(TappedBox);
            ContentLayout.Children.Add(DisableBox);
            ContentLayout.Children.Add(GesturesBox);

            this.Content = this.ContentLayout;

            ButtonTappedCommand_CanExecuteChanged(this, null);
        }

        /// <summary>
        /// Set the content disable or enabled.
        /// </summary>
        /// <param name="canExecute">Can execute command.</param>
        protected virtual void SetBackgroundEnable(bool canExecute)
        {
            if (!UseDisableBox && (BackgroundBox != null) && (DisableBox != null))
            {
                if (DisableBox.Opacity != 1)
                {
                    DisableBox.Opacity = 1;
                }

                if (DisableBox.Color != Color.Transparent)
                {
                    DisableBox.Color = Color.Transparent;
                }

                if (canExecute)
                {
                    if (BackgroundBox.Color != ButtonBackgroundColor)
                    {
                        BackgroundBox.Color = ButtonBackgroundColor;
                    }
                }
                else
                {
                    if (BackgroundBox.Color != ButtonDisableBackgroundColor)
                    {
                        BackgroundBox.Color = ButtonDisableBackgroundColor;
                    }
                }
            }
        }

        /// <summary>
        /// Set the content disable or enabled.
        /// </summary>
        /// <param name="canExecute">Can execute command.</param>
        protected virtual void SetContentEnable(bool canExecute)
        {
            if (HasImage)
            {
                SetImageEnable(canExecute);
            }
            else if (HasText)
            {
                SetTextEnable(canExecute);
            }

            SetBackgroundEnable(canExecute);

            if (InvisibleWhenDisabled)
            {
                this.UpdateIsVisible(canExecute);
            }
        }

        /// <summary>
        /// Set image disable or enabled.
        /// </summary>
        /// <param name="canExecute">Can execute command.</param>
        protected virtual void SetImageEnable(bool canExecute)
        {
            if (DisableBox != null && UseDisableBox)
            {
                if (DisableBox.Opacity != 1)
                {
                    DisableBox.Opacity = 1;
                }

                Color calculatedColor = Color.Transparent;

                if (DebugColors)
                {
                    calculatedColor = new Color(Color.Fuchsia.R, Color.Fuchsia.G, Color.Fuchsia.B, 0.4);
                }
                else if (!canExecute)
                {
                    calculatedColor = ButtonDisableBackgroundColor;
                }

                if (DisableBox.Color != calculatedColor)
                {
                    DisableBox.Color = calculatedColor;
                }
            }
        }

        /// <summary>
        /// Set text disable or enabled.
        /// </summary>
        /// <param name="canExecute"></param>
        protected virtual void SetTextEnable(bool canExecute)
        {
            if (TextExtendedLabel != null)
            {
                if (canExecute)
                {
                    if (TextExtendedLabel.TextColor != TextColor)
                    {
                        TextExtendedLabel.TextColor = TextColor;
                    }
                }
                else
                {
                    if (TextExtendedLabel.TextColor != TextColorDisabled)
                    {
                        TextExtendedLabel.TextColor = TextColorDisabled;
                    }
                }
            }
        }
    }
}