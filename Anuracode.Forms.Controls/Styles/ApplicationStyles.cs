// <copyright file="ApplicationStyles.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Extensions;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls.Styles
{
    /// <summary>
    /// Application styles to use.
    /// </summary>
    public class ApplicationStyles : ApplicationStyles<CommonResourcesBase>
    {
        /// <summary>
        /// Style for a normal editor.
        /// </summary>
        private Style commonEditorStyle;

        /// <summary>
        /// Style for a normal entry.
        /// </summary>
        private Style commonEntryStyle;

        /// <summary>
        /// Style for the lists.
        /// </summary>
        private Style commonListViewStyle;

        /// <summary>
        /// Default label.
        /// </summary>
        private Style defaultExtendedLabelStyle;

        /// <summary>
        /// Style for the text of the description on a form.
        /// </summary>
        private Style descriptionExtendedLabelStyle;

        /// <summary>
        /// Style for the detail name.
        /// </summary>
        private Style detailNameExtendedLabelStyle;

        /// <summary>
        /// Style for the detail value.
        /// </summary>
        private Style detailValueExtendedLabelStyle;

        /// <summary>
        /// Style for the filter stack.
        /// </summary>
        private Style filterStackStyle;

        /// <summary>
        /// Style for the errors on a form.
        /// </summary>
        private Style formErrorsExtendedLabelStyle;

        /// <summary>
        /// Style for the label on a form.
        /// </summary>
        private Style formExtendedLabelStyle;

        /// <summary>
        /// Form row container style.
        /// </summary>
        private Style formRowContainerStyle;

        /// <summary>
        /// Form section container style.
        /// </summary>
        private Style formSectionContainerStyle;

        /// <summary>
        /// Style for glyph only button on the nav bar.
        /// </summary>
        private Style glyphOnlyNavbarButtonStyle;

        /// <summary>
        /// Style for glyph only rounded button.
        /// </summary>
        private Style glyphOnlyRoundedContentButtonStyle;

        /// <summary>
        /// Style for glyph only rounded button.
        /// </summary>
        private Style glyphOnlyRoundedUnfilledContentButtonStyle;

        /// <summary>
        /// Style for image only rounded button.
        /// </summary>
        private Style imageOnlyRoundedContentButtonStyle;

        /// <summary>
        /// Style similar to a link.
        /// </summary>
        private Style linkContentViewButtonStyle;

        /// <summary>
        /// Main menu style.
        /// </summary>
        private Style mainMenuContentButtonStyle;

        /// <summary>
        /// Default style for the main menu.
        /// </summary>
        private Style mainMenuPageStyle;

        /// <summary>
        /// Style for the sub menu.
        /// </summary>
        private Style mainSubMenuContentButtonStyle;

        /// <summary>
        /// Style for number entry.
        /// </summary>
        private Style numberEntryStyle;

        /// <summary>
        /// Page main title style.
        /// </summary>
        private Style pageMainTitleExtendedLabelStyle;

        /// <summary>
        /// Simple stack container style.
        /// </summary>
        private Style simpleStackContainerStyle;

        /// <summary>
        /// Style for the rows.
        /// </summary>
        private Style summaryRowContainerStyle;

        /// <summary>
        /// Style for text only buttons.
        /// </summary>
        private Style textOnlyContentButtonStyle;

        /// <summary>
        /// Style for text only buttons.
        /// </summary>
        private Style textOnlyImportantContentButtonStyle;

        /// <summary>
        /// Style for text with glyph buttons.
        /// </summary>
        private Style textWithGlyphImportantContentButtonStyle;

        /// <summary>
        /// Style for text with glyph buttons.
        /// </summary>
        private Style textWithGlyphImportantLargeContentButtonStyle;

        /// <summary>
        /// Default style for a view.
        /// </summary>
        private Style viewPageStyle;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="commonResources">Common resources to use.</param>
        public ApplicationStyles(CommonResourcesBase commonResources)
            : base(commonResources)
        {
        }

        /// <summary>
        /// Style for a normal entry.
        /// </summary>
        public Style CommonEditorStyle
        {
            get
            {
                if (commonEditorStyle == null)
                {
                    commonEditorStyle = new Style(typeof(Editor));
                    commonEditorStyle.Setters.Add(Editor.KeyboardProperty, Keyboard.Create(KeyboardFlags.Spellcheck));
                    commonEditorStyle.Setters.Add(Editor.BackgroundColorProperty, CommonResources.PagesBackgroundColorLight);
                }

                return commonEditorStyle;
            }
        }

        /// <summary>
        /// Style for a normal entry.
        /// </summary>
        public Style CommonEntryStyle
        {
            get
            {
                if (commonEntryStyle == null)
                {
                    commonEntryStyle = new Style(typeof(Entry));
                    commonEntryStyle.Setters.Add(Entry.KeyboardProperty, Keyboard.Create(KeyboardFlags.Spellcheck));
                    commonEntryStyle.Setters.Add(Entry.BackgroundColorProperty, CommonResources.PagesBackgroundColorLight);
                }

                return commonEntryStyle;
            }
        }

        /// <summary>
        /// Style for the tasks.
        /// </summary>
        public virtual Style CommonListViewStyle
        {
            get
            {
                if (commonListViewStyle == null)
                {
                    commonListViewStyle = new Style(typeof(ListView));
                    commonListViewStyle.Setters.Add(ListView.HasUnevenRowsProperty, true);
                    commonListViewStyle.Setters.Add(ListView.BackgroundColorProperty, CommonResources.PagesBackgroundColor);
                }

                return commonListViewStyle;
            }
        }

        /// <summary>
        /// Default label.
        /// </summary>
        public virtual Style DefaultExtendedLabelStyle
        {
            get
            {
                if (defaultExtendedLabelStyle == null)
                {
                    defaultExtendedLabelStyle = new Style(typeof(ExtendedLabel));

                    defaultExtendedLabelStyle.Setters.Add(ExtendedLabel.TextColorProperty, CommonResources.DefaultLabelTextColor);

                    defaultExtendedLabelStyle.Setters.Add(ExtendedLabel.FontNameProperty, CommonResources.FontRobotMediumName);
                    defaultExtendedLabelStyle.Setters.Add(ExtendedLabel.FriendlyFontNameProperty, CommonResources.FontRobotMediumFriendlyName);
                    defaultExtendedLabelStyle.Setters.Add(ExtendedLabel.FontSizeProperty, CommonResources.TextSizeMedium);
                }

                return defaultExtendedLabelStyle;
            }
        }

        /// <summary>
        /// Style for the text of the description on a form.
        /// </summary>
        public virtual Style DescriptionExtendedLabelStyle
        {
            get
            {
                if (descriptionExtendedLabelStyle == null)
                {
                    descriptionExtendedLabelStyle = new Style(typeof(ExtendedLabel));
                    descriptionExtendedLabelStyle.BasedOn = DefaultExtendedLabelStyle;

                    descriptionExtendedLabelStyle.Setters.Add(ExtendedLabel.FontSizeProperty, CommonResources.TextSizeMedium);
                    descriptionExtendedLabelStyle.Setters.Add(ExtendedLabel.TextColorProperty, CommonResources.TextColorDisable);
                }

                return descriptionExtendedLabelStyle;
            }
        }

        /// <summary>
        /// Style for the detail name.
        /// </summary>
        public virtual Style DetailNameExtendedLabelStyle
        {
            get
            {
                if (detailNameExtendedLabelStyle == null)
                {
                    detailNameExtendedLabelStyle = new Style(typeof(ExtendedLabel));
                    detailNameExtendedLabelStyle.BasedOn = DefaultExtendedLabelStyle;

                    detailNameExtendedLabelStyle.Setters.Add(ExtendedLabel.FontNameProperty, CommonResources.FontRobotBoldCondensedName);
                    detailNameExtendedLabelStyle.Setters.Add(ExtendedLabel.FriendlyFontNameProperty, CommonResources.FontRobotBoldCondensedFriendlyName);
                    detailNameExtendedLabelStyle.Setters.Add(ExtendedLabel.FontSizeProperty, CommonResources.TextSizeSmall);
                    detailNameExtendedLabelStyle.Setters.Add(ExtendedLabel.VerticalOptionsProperty, LayoutOptions.Center);
                    detailNameExtendedLabelStyle.Setters.Add(ExtendedLabel.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
                    detailNameExtendedLabelStyle.Setters.Add(ExtendedLabel.HorizontalTextAlignmentProperty, TextAlignment.Start);
                }

                return detailNameExtendedLabelStyle;
            }
        }

        /// <summary>
        /// Style for the detail value.
        /// </summary>
        public virtual Style DetailValueExtendedLabelStyle
        {
            get
            {
                if (detailValueExtendedLabelStyle == null)
                {
                    detailValueExtendedLabelStyle = new Style(typeof(ExtendedLabel));
                    detailValueExtendedLabelStyle.BasedOn = DetailNameExtendedLabelStyle;

                    detailValueExtendedLabelStyle.Setters.Add(ExtendedLabel.FontNameProperty, CommonResources.FontRobotRegularName);
                    detailValueExtendedLabelStyle.Setters.Add(ExtendedLabel.FriendlyFontNameProperty, CommonResources.FontRobotRegularFriendlyName);
                    detailValueExtendedLabelStyle.Setters.Add(ExtendedLabel.TextColorProperty, CommonResources.TextColorDetailValue);
                }

                return detailValueExtendedLabelStyle;
            }
        }

        /// <summary>
        /// Style for the filter stack.
        /// </summary>
        public virtual Style FilterStackStyle
        {
            get
            {
                if (filterStackStyle == null)
                {
                    filterStackStyle = new Style(typeof(StackLayout));
                    filterStackStyle.BasedOn = this.FormRowContainerStyle;
                    filterStackStyle.Setters.Add(StackLayout.SpacingProperty, 0);
                }

                return filterStackStyle;
            }
        }

        /// <summary>
        /// Style for the text of the description on a form.
        /// </summary>
        public virtual Style FormErrorsExtendedLabelStyle
        {
            get
            {
                if (formErrorsExtendedLabelStyle == null)
                {
                    formErrorsExtendedLabelStyle = new Style(typeof(ExtendedLabel));
                    formErrorsExtendedLabelStyle.BasedOn = DescriptionExtendedLabelStyle;

                    formErrorsExtendedLabelStyle.Setters.Add(ExtendedLabel.FontSizeProperty, CommonResources.TextSizeMicro);
                    formErrorsExtendedLabelStyle.Setters.Add(ExtendedLabel.TextColorProperty, CommonResources.TextColorFormErrors);
                    formErrorsExtendedLabelStyle.Setters.Add(Label.VerticalOptionsProperty, LayoutOptions.Start);
                    formErrorsExtendedLabelStyle.Setters.Add(Label.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
                    formErrorsExtendedLabelStyle.Setters.Add(Label.HorizontalTextAlignmentProperty, TextAlignment.Center);
                    formErrorsExtendedLabelStyle.Setters.Add(Label.FontAttributesProperty, FontAttributes.Bold);
                }

                return formErrorsExtendedLabelStyle;
            }
        }

        /// <summary>
        /// Style for the label on a form.
        /// </summary>
        public virtual Style FormExtendedLabelStyle
        {
            get
            {
                if (formExtendedLabelStyle == null)
                {
                    formExtendedLabelStyle = new Style(typeof(ExtendedLabel));
                    formExtendedLabelStyle.BasedOn = DetailNameExtendedLabelStyle;

                    formExtendedLabelStyle.Setters.Add(ExtendedLabel.FontSizeProperty, CommonResources.TextSizeMedium);
                    formExtendedLabelStyle.Setters.Add(Label.TextColorProperty, CommonResourcesBase.SubtleColor);
                }

                return formExtendedLabelStyle;
            }
        }

        /// <summary>
        /// Form row container style.
        /// </summary>
        public virtual Style FormRowContainerStyle
        {
            get
            {
                if (formRowContainerStyle == null)
                {
                    formRowContainerStyle = new Style(typeof(StackLayout));
                    formRowContainerStyle.Setters.Add(StackLayout.SpacingProperty, 4);
                    formRowContainerStyle.Setters.Add(StackLayout.MinimumHeightRequestProperty, 0);
                    formRowContainerStyle.Setters.Add(StackLayout.MinimumWidthRequestProperty, 0);
                }

                return formRowContainerStyle;
            }
        }

        /// <summary>
        /// Form section container style.
        /// </summary>
        public virtual Style FormSectionContainerStyle
        {
            get
            {
                if (formSectionContainerStyle == null)
                {
                    formSectionContainerStyle = new Style(typeof(StackLayout));
                    formSectionContainerStyle.Setters.Add(StackLayout.SpacingProperty, 15);
                    formSectionContainerStyle.Setters.Add(StackLayout.BackgroundColorProperty, CommonResourcesBase.Accent);
                    formSectionContainerStyle.Setters.Add(StackLayout.MinimumHeightRequestProperty, 0);
                    formSectionContainerStyle.Setters.Add(StackLayout.MinimumWidthRequestProperty, 0);
                }

                return formSectionContainerStyle;
            }
        }

        /// <summary>
        /// Style for image only rounded button.
        /// </summary>
        public virtual Style GlyphOnlyNavbarButtonStyle
        {
            get
            {
                if (glyphOnlyNavbarButtonStyle == null)
                {
                    glyphOnlyNavbarButtonStyle = new Style(typeof(ContentViewButton));
                    glyphOnlyNavbarButtonStyle.BasedOn = GlyphOnlyRoundedUnfilledContentButtonStyle;

                    glyphOnlyNavbarButtonStyle.Setters.Add(GlyphContentViewButton.GlyphTextColorProperty, CommonResources.PagesBackgroundColorLight);
                    glyphOnlyNavbarButtonStyle.Setters.Add(ContentViewButton.TextColorProperty, CommonResources.PagesBackgroundColorLight);
                    glyphOnlyNavbarButtonStyle.Setters.Add(GlyphContentViewButton.DisableGlyphOnlyProperty, true);
                    glyphOnlyNavbarButtonStyle.Setters.Add(ContentViewButton.ButtonBackgroundColorProperty, CommonResources.AccentDark);
                    glyphOnlyNavbarButtonStyle.Setters.Add(ContentViewButton.ShapeTypeProperty, ShapeType.Box);
                    glyphOnlyNavbarButtonStyle.Setters.Add(ContentViewButton.VerticalOptionsProperty, LayoutOptions.Center);
                    glyphOnlyNavbarButtonStyle.Setters.Add(ContentViewButton.HorizontalOptionsProperty, LayoutOptions.Start);
                    glyphOnlyNavbarButtonStyle.Setters.Add(ContentViewButton.ImageHeightRequestProperty, CommonResources.RoundedButtonWidth);
                    glyphOnlyNavbarButtonStyle.Setters.Add(ContentViewButton.ImageWidthRequestProperty, CommonResources.RoundedButtonWidth);
                    glyphOnlyNavbarButtonStyle.Setters.Add(ContentViewButton.MarginElementsProperty, 0);
                    glyphOnlyNavbarButtonStyle.Setters.Add(ContentViewButton.MarginBordersProperty, 0);
                    glyphOnlyNavbarButtonStyle.Setters.Add(GlyphContentViewButton.GlyphFontSizeProperty, Device.OS == TargetPlatform.Windows ? CommonResources.TextSizeMedium : CommonResources.TextSizeMedium * 1.25f);
                    glyphOnlyNavbarButtonStyle.Setters.Add(GlyphContentViewButton.FontSizeProperty, Device.OS == TargetPlatform.Windows ? CommonResources.TextSizeMedium : CommonResources.TextSizeMedium * 1.25f);
                }

                return glyphOnlyNavbarButtonStyle;
            }
        }

        /// <summary>
        /// Style for image only rounded button.
        /// </summary>
        public virtual Style GlyphOnlyRoundedContentButtonStyle
        {
            get
            {
                if (glyphOnlyRoundedContentButtonStyle == null)
                {
                    glyphOnlyRoundedContentButtonStyle = new Style(typeof(ContentViewButton));
                    glyphOnlyRoundedContentButtonStyle.BasedOn = ImageOnlyRoundedContentButtonStyle;

                    glyphOnlyRoundedContentButtonStyle.Setters.Add(GlyphContentViewButton.GlyphTextColorProperty, CommonResources.TextColorSection);
                    glyphOnlyRoundedContentButtonStyle.Setters.Add(GlyphContentViewButton.GlyphFontSizeProperty, CommonResources.TextSizeMicro);
                    glyphOnlyRoundedContentButtonStyle.Setters.Add(GlyphContentViewButton.DisableGlyphOnlyProperty, false);
                    glyphOnlyRoundedContentButtonStyle.Setters.Add(ContentViewButton.TextColorProperty, CommonResources.Accent);
                }

                return glyphOnlyRoundedContentButtonStyle;
            }
        }

        /// <summary>
        /// Style for image only rounded button.
        /// </summary>
        public virtual Style GlyphOnlyRoundedUnfilledContentButtonStyle
        {
            get
            {
                if (glyphOnlyRoundedUnfilledContentButtonStyle == null)
                {
                    glyphOnlyRoundedUnfilledContentButtonStyle = new Style(typeof(ContentViewButton));
                    glyphOnlyRoundedUnfilledContentButtonStyle.BasedOn = GlyphOnlyRoundedContentButtonStyle;

                    glyphOnlyRoundedUnfilledContentButtonStyle.Setters.Add(GlyphContentViewButton.GlyphTextColorProperty, CommonResources.Accent);
                    glyphOnlyRoundedUnfilledContentButtonStyle.Setters.Add(GlyphContentViewButton.DisableGlyphOnlyProperty, true);
                    glyphOnlyRoundedUnfilledContentButtonStyle.Setters.Add(ContentViewButton.TextColorProperty, CommonResources.Accent);
                    glyphOnlyRoundedUnfilledContentButtonStyle.Setters.Add(ContentViewButton.ButtonBackgroundColorProperty, Color.Transparent);
                }

                return glyphOnlyRoundedUnfilledContentButtonStyle;
            }
        }

        /// <summary>
        /// Style for image only rounded button.
        /// </summary>
        public virtual Style ImageOnlyRoundedContentButtonStyle
        {
            get
            {
                if (imageOnlyRoundedContentButtonStyle == null)
                {
                    imageOnlyRoundedContentButtonStyle = new Style(typeof(ContentViewButton));
                    imageOnlyRoundedContentButtonStyle.Setters.Add(ContentViewButton.ButtonBackgroundColorProperty, CommonResources.Accent);
                    imageOnlyRoundedContentButtonStyle.Setters.Add(ContentViewButton.ShapeTypeProperty, ShapeType.Circle);
                    imageOnlyRoundedContentButtonStyle.Setters.Add(ContentViewButton.TextColorProperty, CommonResources.TextColorSection);
                    imageOnlyRoundedContentButtonStyle.Setters.Add(ContentViewButton.ImageHeightRequestProperty, CommonResources.RoundedButtonWidth * 0.9);
                    imageOnlyRoundedContentButtonStyle.Setters.Add(ContentViewButton.ImageWidthRequestProperty, CommonResources.RoundedButtonWidth * 0.9);
                    imageOnlyRoundedContentButtonStyle.Setters.Add(ContentViewButton.HeightRequestProperty, CommonResources.RoundedButtonWidth);
                    imageOnlyRoundedContentButtonStyle.Setters.Add(ContentViewButton.WidthRequestProperty, CommonResources.RoundedButtonWidth);
                    imageOnlyRoundedContentButtonStyle.Setters.Add(ContentViewButton.MinimumHeightRequestProperty, CommonResources.RoundedButtonWidth);
                    imageOnlyRoundedContentButtonStyle.Setters.Add(ContentViewButton.MinimumWidthRequestProperty, CommonResources.RoundedButtonWidth);
                }

                return imageOnlyRoundedContentButtonStyle;
            }
        }

        /// <summary>
        /// Style similar to a link.
        /// </summary>
        public Style LinkContentViewButtonStyle
        {
            get
            {
                if (linkContentViewButtonStyle == null)
                {
                    linkContentViewButtonStyle = new Style(typeof(ContentViewButton));
                    linkContentViewButtonStyle.Setters.Add(ContentViewButton.TextColorProperty, CommonResources.TextColorLink);
                    linkContentViewButtonStyle.Setters.Add(ContentViewButton.IsUnderlineProperty, true);
                    linkContentViewButtonStyle.Setters.Add(ContentViewButton.FontNameProperty, CommonResources.FontRobotBoldCondensedName);
                    linkContentViewButtonStyle.Setters.Add(ContentViewButton.FriendlyFontNameProperty, CommonResources.FontRobotBoldCondensedFriendlyName);
                    linkContentViewButtonStyle.Setters.Add(ContentViewButton.FontSizeProperty, CommonResources.TextSizeSmall);
                    linkContentViewButtonStyle.Setters.Add(ContentViewButton.MarginBordersProperty, 5);
                }

                return linkContentViewButtonStyle;
            }
        }

        /// <summary>
        /// Main menu style.
        /// </summary>
        public virtual Style MainMenuContentButtonStyle
        {
            get
            {
                if (mainMenuContentButtonStyle == null)
                {
                    mainMenuContentButtonStyle = new Style(typeof(ContentViewButton));
                    mainMenuContentButtonStyle.Setters.Add(ContentViewButton.TextColorProperty, CommonResources.TextColorMainMenu);
                    mainMenuContentButtonStyle.Setters.Add(ContentViewButton.ShapeTypeProperty, ShapeType.Box);
                    mainMenuContentButtonStyle.Setters.Add(ContentViewButton.CornerRadiusProperty, 10);
                    mainMenuContentButtonStyle.Setters.Add(ContentViewButton.VerticalOptionsProperty, LayoutOptions.Center);
                    mainMenuContentButtonStyle.Setters.Add(ContentViewButton.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
                    mainMenuContentButtonStyle.Setters.Add(ContentViewButton.FontNameProperty, CommonResources.FontRobotRegularName);
                    mainMenuContentButtonStyle.Setters.Add(ContentViewButton.FriendlyFontNameProperty, CommonResources.FontRobotRegularFriendlyName);
                    mainMenuContentButtonStyle.Setters.Add(ContentViewButton.MarginElementsProperty, 5);
                    mainMenuContentButtonStyle.Setters.Add(ContentViewButton.MarginBordersProperty, 5);

                    mainMenuContentButtonStyle.Setters.Add(ContentViewButton.FontSizeProperty, CommonResources.TextSizeMedium);
                }

                return mainMenuContentButtonStyle;
            }
        }

        /// <summary>
        /// Default style for the main menu.
        /// </summary>
        public virtual Style MainMenuPageStyle
        {
            get
            {
                if (mainMenuPageStyle == null)
                {
                    mainMenuPageStyle = new Style(typeof(ContentPage));
                    mainMenuPageStyle.BasedOn = ViewPageStyle;
                }

                return mainMenuPageStyle;
            }
        }

        /// <summary>
        /// Main menu style.
        /// </summary>
        public virtual Style MainSubMenuContentButtonStyle
        {
            get
            {
                if (mainSubMenuContentButtonStyle == null)
                {
                    mainSubMenuContentButtonStyle = new Style(typeof(ContentViewButton));
                    mainSubMenuContentButtonStyle.BasedOn = MainMenuContentButtonStyle;
                    mainSubMenuContentButtonStyle.Setters.Add(ContentViewButton.FontSizeProperty, CommonResources.TextSizeSmall);
                }

                return mainSubMenuContentButtonStyle;
            }
        }

        /// <summary>
        /// Style for number entry.
        /// </summary>
        public Style NumberEntryStyle
        {
            get
            {
                if (numberEntryStyle == null)
                {
                    numberEntryStyle = new Style(typeof(Entry));
                    numberEntryStyle.BasedOn = CommonEntryStyle;
                    numberEntryStyle.Setters.Add(Entry.KeyboardProperty, Keyboard.Numeric);
                }

                return numberEntryStyle;
            }
        }

        /// <summary>
        /// Page main title style.
        /// </summary>
        public Style PageMainTitleExtendedLabelStyle
        {
            get
            {
                if (pageMainTitleExtendedLabelStyle == null)
                {
                    pageMainTitleExtendedLabelStyle = new Style(typeof(ExtendedLabel));
                    pageMainTitleExtendedLabelStyle.BasedOn = DefaultExtendedLabelStyle;

                    pageMainTitleExtendedLabelStyle.Setters.Add(ExtendedLabel.FontNameProperty, CommonResources.FontRobotBoldCondensedName);
                    pageMainTitleExtendedLabelStyle.Setters.Add(ExtendedLabel.FriendlyFontNameProperty, CommonResources.FontRobotBoldCondensedFriendlyName);
                    pageMainTitleExtendedLabelStyle.Setters.Add(ExtendedLabel.TextColorProperty, CommonResourcesBase.Accent);
                    pageMainTitleExtendedLabelStyle.Setters.Add(ExtendedLabel.FontSizeProperty, CommonResources.TextSizeLarge);
                    pageMainTitleExtendedLabelStyle.Setters.Add(ExtendedLabel.VerticalOptionsProperty, LayoutOptions.Start);
                    pageMainTitleExtendedLabelStyle.Setters.Add(ExtendedLabel.LineBreakModeProperty, LineBreakMode.NoWrap);
                }

                return pageMainTitleExtendedLabelStyle;
            }
        }

        /// <summary>
        /// Simple stack container style.
        /// </summary>
        public virtual Style SimpleStackContainerStyle
        {
            get
            {
                if (simpleStackContainerStyle == null)
                {
                    simpleStackContainerStyle = new Style(typeof(ExtendedLabel));

                    simpleStackContainerStyle.Setters.Add(StackLayout.SpacingProperty, 0);
                    simpleStackContainerStyle.Setters.Add(StackLayout.PaddingProperty, 0);
                    simpleStackContainerStyle.Setters.Add(StackLayout.MinimumHeightRequestProperty, 0);
                    simpleStackContainerStyle.Setters.Add(StackLayout.MinimumWidthRequestProperty, 0);
                }

                return simpleStackContainerStyle;
            }
        }

        /// <summary>
        /// Row container style.
        /// </summary>
        public virtual Style SummaryRowContainerStyle
        {
            get
            {
                if (summaryRowContainerStyle == null)
                {
                    summaryRowContainerStyle = new Style(typeof(StackLayout));
                    summaryRowContainerStyle.BasedOn = FormRowContainerStyle;
                    summaryRowContainerStyle.Setters.Add(StackLayout.SpacingProperty, 0);
                }

                return summaryRowContainerStyle;
            }
        }

        /// <summary>
        /// Text only style.
        /// </summary>
        public virtual Style TextOnlyContentButtonStyle
        {
            get
            {
                if (textOnlyContentButtonStyle == null)
                {
                    textOnlyContentButtonStyle = new Style(typeof(ContentViewButton));
                    textOnlyContentButtonStyle.BasedOn = MainSubMenuContentButtonStyle;
                    textOnlyContentButtonStyle.Setters.Add(ContentViewButton.HorizontalOptionsProperty, LayoutOptions.Start);

                    textOnlyContentButtonStyle.Setters.Add(ContentViewButton.TextColorProperty, CommonResources.Accent);
                }

                return textOnlyContentButtonStyle;
            }
        }

        /// <summary>
        /// Text only style.
        /// </summary>
        public virtual Style TextOnlyImportantContentButtonStyle
        {
            get
            {
                if (textOnlyImportantContentButtonStyle == null)
                {
                    textOnlyImportantContentButtonStyle = new Style(typeof(ContentViewButton));
                    textOnlyImportantContentButtonStyle.BasedOn = TextOnlyContentButtonStyle;
                    textOnlyImportantContentButtonStyle.Setters.Add(ContentViewButton.HorizontalOptionsProperty, LayoutOptions.Center);
                    textOnlyImportantContentButtonStyle.Setters.Add(ContentViewButton.VerticalOptionsProperty, LayoutOptions.Center);
                    textOnlyImportantContentButtonStyle.Setters.Add(ContentViewButton.TextColorProperty, CommonResources.TextColorSection);
                    textOnlyImportantContentButtonStyle.Setters.Add(ContentViewButton.MarginBordersProperty, 5);
                    textOnlyImportantContentButtonStyle.Setters.Add(ContentViewButton.CornerRadiusProperty, 10);
                    textOnlyImportantContentButtonStyle.Setters.Add(ContentViewButton.ButtonBackgroundColorProperty, CommonResources.Accent);
                    textOnlyImportantContentButtonStyle.Setters.Add(ContentViewButton.HorizontalOptionsProperty, LayoutOptions.Center);
                    textOnlyImportantContentButtonStyle.Setters.Add(ContentViewButton.UseDisableBoxProperty, false);
                }

                return textOnlyImportantContentButtonStyle;
            }
        }

        /// <summary>
        /// Text only style.
        /// </summary>
        public virtual Style TextWithGlyphImportantContentButtonStyle
        {
            get
            {
                if (textWithGlyphImportantContentButtonStyle == null)
                {
                    textWithGlyphImportantContentButtonStyle = new Style(typeof(GlyphContentViewButton));
                    textWithGlyphImportantContentButtonStyle.BasedOn = TextOnlyImportantContentButtonStyle;
                    textWithGlyphImportantContentButtonStyle.Setters.Add(ContentViewButton.MarginBordersProperty, 10);
                    textWithGlyphImportantContentButtonStyle.Setters.Add(ContentViewButton.MarginElementsProperty, 5);
                    textWithGlyphImportantContentButtonStyle.Setters.Add(ContentViewButton.ButtonBackgroundColorProperty, Color.Transparent);
                    textWithGlyphImportantContentButtonStyle.Setters.Add(ContentViewButton.TextColorProperty, Color.Black);
                    textWithGlyphImportantContentButtonStyle.Setters.Add(GlyphContentViewButton.GlyphTextColorProperty, Color.Black);
                    textWithGlyphImportantContentButtonStyle.Setters.Add(ContentViewButton.IsUnderlineProperty, true);
                }

                return textWithGlyphImportantContentButtonStyle;
            }
        }

        /// <summary>
        /// Text only style.
        /// </summary>
        public virtual Style TextWithGlyphImportantLargeContentButtonStyle
        {
            get
            {
                if (textWithGlyphImportantLargeContentButtonStyle == null)
                {
                    textWithGlyphImportantLargeContentButtonStyle = new Style(typeof(GlyphContentViewButton));
                    textWithGlyphImportantLargeContentButtonStyle.BasedOn = TextOnlyImportantContentButtonStyle;
                    textWithGlyphImportantLargeContentButtonStyle.Setters.Add(ContentViewButton.MarginBordersProperty, Device.OS.OnPlatform(10, 15, 15, 15, 15));
                    textWithGlyphImportantLargeContentButtonStyle.Setters.Add(ContentViewButton.MarginElementsProperty, 0);
                    textWithGlyphImportantLargeContentButtonStyle.Setters.Add(ContentViewButton.MinimumWidthRequestProperty, Device.OS.OnPlatform(130, 150, 150, 150, 150));
                }

                return textWithGlyphImportantLargeContentButtonStyle;
            }
        }

        /// <summary>
        /// Default style for a view.
        /// </summary>
        public virtual Style ViewPageStyle
        {
            get
            {
                if (viewPageStyle == null)
                {
                    viewPageStyle = new Style(typeof(ContentPage));

                    if (Device.OS == TargetPlatform.Android)
                    {
                        viewPageStyle.Setters.Add(ContentPage.TitleProperty, CommonResources.ApplicationTitle);
                    }

                    viewPageStyle.Setters.Add(ContentPage.BackgroundColorProperty, CommonResources.PagesBackgroundColor);
                }

                return viewPageStyle;
            }
        }
    }
}