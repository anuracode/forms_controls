// <copyright file="SignaturePage.cs" company="Anura Code">
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
    public class SignaturePage : ContentBaseView<SignatureViewModel>
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="viewModel">View model to use.</param>
        public SignaturePage(SignatureViewModel viewModel)
            : base(viewModel)
        {
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SignaturePage()
            : this(null)
        {
        }

        /// <summary>
        /// Signature view.
        /// </summary>
        protected SignaturePadView SignatureView { get; set; }

        /// <summary>
        /// Content of the page.
        /// </summary>
        protected override View RenderContent()
        {
            SignatureView = new SignaturePadView()
            {
                BackgroundColor = Theme.CommonResources.PagesBackgroundColor,
                StrokeColor = Theme.CommonResources.DefaultLabelTextColor,
                SignatureLineColor = Theme.CommonResources.DefaultLabelTextColor,
                CaptionTextColor = Theme.CommonResources.DefaultLabelTextColor,
                PromptTextColor = Theme.CommonResources.DefaultLabelTextColor,
                CaptionText = App.LocalizationResources.SignhereLabel,
                PromptText = "X"
            };

            return SignatureView;
        }

        /// <summary>
        /// Render footer.
        /// </summary>
        /// <returns>View to use.</returns>
        protected override View RenderFooter()
        {
            TextContentViewButton clearButton = new TextContentViewButton()
            {
                Style = Theme.ApplicationStyles.TextOnlyContentButtonStyle,
                Text = App.LocalizationResources.ClearLabel,
                Command = SignatureView.ClearCommand
            };

            return clearButton;
        }
    }
}