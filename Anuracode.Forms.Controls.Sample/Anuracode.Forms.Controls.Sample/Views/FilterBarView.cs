// <copyright file="FilterBarView.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System.Windows.Input;
using Xamarin.Forms;
using Anuracode.Forms.Controls.Extensions;
using Anuracode.Forms.Controls.Views.Extensions;

namespace Anuracode.Forms.Controls.Sample.Views
{
    /// <summary>
    /// View for a filter bar.
    /// </summary>
    public class FilterBarView : SimpleViewBase
    {
        /// <summary>
        /// Command paramter.
        /// </summary>
        public static readonly BindableProperty CommandParameterProperty = BindablePropertyHelper.Create<FilterBarView, object>(nameof(CommandParameter), (object)null);

        /// <summary>
        /// Backing field for the command property.
        /// </summary>
        public static readonly BindableProperty CommandProperty = BindablePropertyHelper.Create<FilterBarView, object>(nameof(Command), (object)null);

        /// <summary>
        /// Corner radius property.
        /// </summary>
        public static readonly BindableProperty CornerRadiusProperty = BindablePropertyHelper.Create<FilterBarView, float>(nameof(CornerRadius), 10f);

        /// <summary>
        /// Filter background color.
        /// </summary>
        public static readonly BindableProperty FilterBackgroundColorProperty = BindablePropertyHelper.Create<FilterBarView, Color>(nameof(FilterBackgroundColor), Color.Transparent);

        /// <summary>
        /// Backing field for the image height property.
        /// </summary>
        public static readonly BindableProperty ImageHeightRequestProperty = BindablePropertyHelper.Create<FilterBarView, int>(nameof(ImageHeightRequest), 30);

        /// <summary>
        /// Backing field for the image width property.
        /// </summary>
        public static readonly BindableProperty ImageWidthRequestProperty = BindablePropertyHelper.Create<FilterBarView, int>(nameof(ImageWidthRequest), 30);

        /// <summary>
        /// Margin borders.
        /// </summary>
        public static readonly BindableProperty MarginBordersProperty = BindablePropertyHelper.Create<FilterBarView, double>(nameof(MarginBorders), 10);

        /// <summary>
        /// Margin between elements.
        /// </summary>
        public static readonly BindableProperty MarginElementsProperty = BindablePropertyHelper.Create<FilterBarView, double>(nameof(MarginElements), 10);

        /// <summary>
        /// The placeholder property.
        /// </summary>
        public static readonly BindableProperty PlaceholderProperty = BindablePropertyHelper.Create<FilterBarView, string>(nameof(Placeholder), default(string));

        /// <summary>
        /// Shape color.
        /// </summary>
        public static readonly BindableProperty StrokeColorProperty = BindablePropertyHelper.Create<FilterBarView, Color>(nameof(StrokeColor), Color.Default);

        /// <summary>
        /// Stroke width.
        /// </summary>
        public static readonly BindableProperty StrokeWidthProperty = BindablePropertyHelper.Create<FilterBarView, float>(nameof(StrokeWidth), 1f);

        /// <summary>
        /// Text color disabled.
        /// </summary>
        public static readonly BindableProperty TextColorDisabledProperty = BindablePropertyHelper.Create<FilterBarView, Color>(nameof(TextColorDisabled), Theme.CommonResources.TextColorDisable);

        /// <summary>
        /// Text color property.
        /// </summary>
        public static readonly BindableProperty TextColorProperty = BindablePropertyHelper.Create<FilterBarView, Color>(nameof(TextColor), Theme.CommonResources.DefaultEntryTextColor);

        /// <summary>
        /// Text property.
        /// </summary>
        public static readonly BindableProperty TextProperty = BindablePropertyHelper.Create<FilterBarView, string>(nameof(Text), string.Empty);

        /// <summary>
        /// Default constructor.
        /// </summary>
        public FilterBarView()
            : base()
        {
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
                SetValue(CornerRadiusProperty, value);
            }
        }

        /// <summary>
        /// Filter background color.
        /// </summary>
        public Color FilterBackgroundColor
        {
            get
            {
                return (Color)GetValue(FilterBackgroundColorProperty);
            }

            set
            {
                SetValue(FilterBackgroundColorProperty, value);
            }
        }

        /// <summary>
        /// Font size.
        /// </summary>
        public double FontSize
        {
            get
            {
                return (double)GetValue(TextProperty);
            }

            set
            {
                SetValue(TextProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the requested height of the image.  If less than or equal to zero than a
        /// height of 50 will be used.
        /// </summary>
        /// <value>
        /// The ImageHeightRequest property gets/sets the value of the backing field, ImageHeightRequestProperty.
        /// </value>
        public int ImageHeightRequest
        {
            get
            {
                return (int)GetValue(ImageHeightRequestProperty);
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
        public int ImageWidthRequest
        {
            get
            {
                return (int)GetValue(ImageWidthRequestProperty);
            }

            set
            {
                SetValue(ImageWidthRequestProperty, value);
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
                SetValue(PlaceholderProperty, value);
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
        /// Filter entry.
        /// </summary>
        protected Entry FilterEntry { get; set; }

        /// <summary>
        /// Search button.
        /// </summary>
        protected ContentViewButton SearchButton { get; set; }

        /// <summary>
        /// Focus control.
        /// </summary>
        public void FocusEntry()
        {
            AC.ScheduleManaged(() =>
            {
                if (FilterEntry != null)
                {
                    FilterEntry.Focus();
                }
            });
        }

        /// <summary>
        /// Setup the bindings of the elements.
        /// </summary>
        protected override void SetupBindings()
        {
            if (FilterEntry != null)
            {
                FilterEntry.BindingContext = this;
                FilterEntry.SetBinding<FilterBarView>(Entry.TextProperty, vm => vm.Text);
                FilterEntry.SetBinding<FilterBarView>(Entry.PlaceholderProperty, vm => vm.Placeholder);
                FilterEntry.SetBinding<FilterBarView>(Entry.TextColorProperty, vm => vm.TextColor);

                FilterEntry.Completed -= FilterEntry_Completed;
                FilterEntry.Completed += FilterEntry_Completed;
            }

            if (SearchButton != null)
            {
                SearchButton.BindingContext = this;
                SearchButton.SetBinding<FilterBarView>(ContentViewButton.CommandProperty, vm => vm.Command);
                SearchButton.SetBinding<FilterBarView>(ContentViewButton.CommandParameterProperty, vm => vm.CommandParameter);
                SearchButton.SetBinding<FilterBarView>(ContentViewButton.ImageHeightRequestProperty, vm => vm.ImageHeightRequest);
                SearchButton.SetBinding<FilterBarView>(ContentViewButton.ImageWidthRequestProperty, vm => vm.ImageWidthRequest);
            }
        }

        /// <summary>
        /// Add the control to the layout in the proper order.
        /// </summary>
        protected override void AddControlsToLayout()
        {
            AddViewToLayout(InfoDecoration);
            AddViewToLayout(FilterEntry);
            AddViewToLayout(SearchButton);
        }

        /// <summary>
        /// Layout children.
        /// </summary>
        /// <param name="x">Top to use.</param>
        /// <param name="y">Left to use.</param>
        /// <param name="width">Width to use.</param>
        /// <param name="height">Height to use.</param>
        protected override void ContentLayout_OnLayoutChildren(double x, double y, double width, double height)
        {
            double minWidth = 245;
            double maxWidth = width - (ContentMargin * 6);

            // A third of the screen.
            double thirdMax = (maxWidth * 0.33f);

            maxWidth = maxWidth.Clamp(minWidth, thirdMax > minWidth ? thirdMax : width);

            Rectangle buttonPosition = new Rectangle();
            Rectangle entryPosition = new Rectangle();

            if (SearchButton != null)
            {
                var elementSize = SearchButton.Measure(maxWidth, height - (MarginBorders * 2f)).Request;
                double elementHeight = elementSize.Height;
                double elementWidth = elementSize.Width;
                double elementLeft = maxWidth - MarginBorders - elementWidth;
                double elementTop = (height - elementHeight) * 0.5f;

                // Center elements
                elementLeft += (width - maxWidth) * 0.5f;

                buttonPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                SearchButton.LayoutUpdate(buttonPosition);
            }

            if (FilterEntry != null)
            {
                double elementWidth = maxWidth - buttonPosition.Width - (MarginElements * 2f) - MarginBorders;
                var elementSize = FilterEntry.Measure(elementWidth, height - (MarginBorders * 2f)).Request;
                double elementHeight = elementSize.Height;
                double elementLeft = MarginBorders;
                double elementTop = (height - elementHeight) * 0.5f;

                // Center elements
                elementLeft += (width - maxWidth) * 0.5f;

                entryPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                FilterEntry.LayoutUpdate(entryPosition);
            }

            if (InfoDecoration != null)
            {
                double elementMargin = ContentMargin * 0.5f;
                double elementTop = buttonPosition.Top - elementMargin;
                double elementLeft = entryPosition.Left - elementMargin;
                double elementWidth = (buttonPosition.X + buttonPosition.Width) - entryPosition.X + (elementMargin * 2f);
                double elementHeight = buttonPosition.Height + (elementMargin * 2f);

                var elementPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                InfoDecoration.LayoutUpdate(elementPosition);
            }
        }

        /// <summary>
        /// Mesure the content.
        /// </summary>
        /// <param name="widthConstraint">Width to constarint.</param>
        /// <param name="heightConstraint">Height to constraint.</param>
        /// <returns>Size to use.</returns>
        protected override SizeRequest ContentLayout_OnSizeRequest(double widthConstraint, double heightConstraint)
        {
            Size searchSize = new Size();

            if (SearchButton != null)
            {
                searchSize = SearchButton.Measure(widthConstraint, heightConstraint - (MarginBorders * 2f)).Request;
            }

            double maxHeight = (searchSize.Height + (MarginBorders * 2f) + (ContentMargin * 2f)).Clamp(Theme.CommonResources.RoundedButtonWidth, heightConstraint);

            SizeRequest resultRequest = new SizeRequest(new Size(widthConstraint, maxHeight), new Size(widthConstraint, maxHeight));

            return resultRequest;
        }

        /// <summary>
        /// Filter bar enter pressed.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Arguments of the event.</param>
        protected virtual void FilterEntry_Completed(object sender, System.EventArgs e)
        {
            if ((Command != null) && Command.CanExecute(CommandParameter))
            {
                Command.Execute(CommandParameter);
            }
        }

        /// <summary>
        /// Info decoration.
        /// </summary>
        protected ShapeView InfoDecoration { get; set; }

        /// <summary>
        /// Internal initialze view.
        /// </summary>
        protected override void InternalInitializeView()
        {
            InfoDecoration = new ShapeView()
            {
                ShapeType = ShapeType.Box,
                CornerRadius = 10,
                Color = Theme.CommonResources.PagesBackgroundColorLight
            };

            BackgroundColor = Theme.CommonResources.AccentLight;

            FilterEntry = new ExtendedEntry()
            {
                Style = Theme.ApplicationStyles.CommonEntryStyle,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center,
                HasInvisibleBorders = true
            };

            SearchButton = new GlyphOnlyContentViewButton()
            {
                Style = Theme.ApplicationStyles.GlyphOnlyRoundedContentButtonStyle,
                GlyphText = Theme.CommonResources.GlyphTextSearch,
                Text = App.LocalizationResources.SearchButton,
                MarginElements = 0,
                DisableGlyphOnly = true                
            };
        }
    }
}