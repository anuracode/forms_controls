// <copyright file="AboutPage.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Sample.ViewModels;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls.Sample.Views
{
    /// <summary>
    /// Page for the about.
    /// </summary>
    public class AboutPage : ContentBaseView<AboutViewModel>
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="viewModel">View model to use.</param>
        public AboutPage(AboutViewModel viewModel)
            : base(viewModel)
        {
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public AboutPage()
            : this(null)
        {
        }

        /// <summary>
        /// Content of the page.
        /// </summary>
        protected override View RenderContent()
        {
            StackLayout stackPage = new StackLayout()
            {
                Style = Theme.ApplicationStyles.FormRowContainerStyle
            };

            var stackGeneral = stackPage;

            // Assembly version.
            Theme.RenderUtil.RenderReadField(stackGeneral, "LocalizationResources.AboutAssemblyVersionLabel", "AssemblyVersion", orientation: StackOrientation.Horizontal);

            // Assembly file version.
            Theme.RenderUtil.RenderReadField(stackGeneral, "LocalizationResources.AboutFileVersionLabel", "FileVersion", orientation: StackOrientation.Horizontal);

            // Support mail.
            TextContentViewButton buttonSupportMailValue = new TextContentViewButton()
            {
                Style = Theme.ApplicationStyles.LinkContentViewButtonStyle,
                MinimumWidthRequest = 350
            };

            buttonSupportMailValue.SetBinding(ContentViewButton.TextProperty, "LocalizationResources.ErrorMailTo");
            buttonSupportMailValue.SetBinding(ContentViewButton.CommandParameterProperty, "LocalizationResources.ErrorMailTo", stringFormat: BaseViewModel.PREFIX_EMAIL + "{0}");

            Theme.RenderUtil.RenderFormField(stackGeneral, "LocalizationResources.AboutMailLabel", editorView: buttonSupportMailValue, orientation: StackOrientation.Horizontal, labelSytle: Theme.ApplicationStyles.DetailNameExtendedLabelStyle);

            // Developed by.            
            ContentViewButton buttonDevelopByValue = new TextContentViewButton()
            {
                Style = Theme.ApplicationStyles.LinkContentViewButtonStyle,
                MinimumWidthRequest = 350
            };

            buttonDevelopByValue.Text = "AnuraCode";
            buttonDevelopByValue.SetBinding(ContentViewButton.CommandParameterProperty, "LocalizationResources.AboutUrl");

            Theme.RenderUtil.RenderFormField(stackGeneral, "LocalizationResources.DevelopedByLabel", editorView: buttonDevelopByValue, orientation: StackOrientation.Horizontal, labelSytle: Theme.ApplicationStyles.DetailNameExtendedLabelStyle);

            Theme.RenderUtil.RenderReadField(stackGeneral, "LocalizationResources.AboutDisclaimerTitle", orientation: StackOrientation.Horizontal);

            Label labelDisclaimerValue = new ExtendedLabel()
            {
                Style = Theme.ApplicationStyles.DescriptionExtendedLabelStyle,
                MinimumWidthRequest = 250
            };
            labelDisclaimerValue.SetBinding(Label.TextProperty, "LocalizationResources.AboutDisclaimerMessage");
            stackGeneral.Children.Add(labelDisclaimerValue);

            ContentViewButton buttonDisclaimer = new TextContentViewButton(hasBackground: true)
            {
                Style = Theme.ApplicationStyles.TextOnlyImportantContentButtonStyle
            };

            buttonDisclaimer.SetBinding(ContentViewButton.TextProperty, "LocalizationResources.AboutDisclaimerTitle");
            buttonDisclaimer.SetBinding(ContentViewButton.CommandParameterProperty, "LocalizationResources.DisclaimerUrl");
            stackGeneral.Children.Add(buttonDisclaimer);

            Image imageAnuracode = new ExtendedImage()
            {
                Source = Theme.CommonResources.PathImageAppLogoLarge,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                WidthRequest = 256
            };

            stackPage.Children.Add(imageAnuracode);

            ScrollView mainScroll = new ScrollView()
            {
                Content = stackPage
            };

            return mainScroll;
        }
    }
}