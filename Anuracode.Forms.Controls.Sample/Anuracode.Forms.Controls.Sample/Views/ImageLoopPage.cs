// <copyright file="ImageLoopPage.cs" company="Anura Code">
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
    public class ImageLoopPage : ContentBaseView<ImageLoopViewModel>
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="viewModel">View model to use.</param>
        public ImageLoopPage(ImageLoopViewModel viewModel)
            : base(viewModel)
        {
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ImageLoopPage()
            : this(null)
        {
        }

        /// <summary>
        /// Content of the page.
        /// </summary>
        protected override View RenderContent()
        {
            StackLayout contentLayout = new StackLayout()
            {
                Style = Theme.ApplicationStyles.SimpleStackContainerStyle,
                Orientation = StackOrientation.Vertical
            };

            StackLayout buttonsLayout = new StackLayout()
            {
                Style = Theme.ApplicationStyles.SimpleStackContainerStyle,
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            // Start button
            TextContentViewButton startLoopButton = new TextContentViewButton()
            {
                Text = "Start",
                Style = Theme.ApplicationStyles.TextOnlyImportantContentButtonStyle,
                Command = ViewModel.StartLoopCommand,
                InvisibleWhenDisabled = true
            };

            buttonsLayout.Children.Add(startLoopButton);

            // Stop button.
            TextContentViewButton stopLoopButton = new TextContentViewButton()
            {
                Text = "Stop",
                Style = Theme.ApplicationStyles.TextOnlyImportantContentButtonStyle,
                Command = ViewModel.StopLoopCommand,
                InvisibleWhenDisabled = true
            };

            buttonsLayout.Children.Add(stopLoopButton);
            contentLayout.Children.Add(buttonsLayout);

            // Count.
            Theme.RenderUtil.RenderReadField<ImageLoopViewModel>(contentLayout, labelValue: "Count", expressionValue: vm => vm.CurrentCount, orientation: StackOrientation.Horizontal);

            // Path.
            Theme.RenderUtil.RenderReadField<ImageLoopViewModel>(contentLayout, labelValue: "Path", expressionValue: vm => vm.CurrentImagePath, orientation: StackOrientation.Horizontal);

            // Delay.
            Theme.RenderUtil.RenderReadField<ImageLoopViewModel>(contentLayout, labelValue: "Delay", expressionValue: vm => vm.DelaySeconds, orientation: StackOrientation.Horizontal);

            ExtendedSlider sliderFeaturedWeight = new ExtendedSlider();
            sliderFeaturedWeight.StepValue = 0.25f;
            sliderFeaturedWeight.SetBinding<ImageLoopViewModel>(ExtendedSlider.ValueProperty, vm => vm.DelaySeconds, BindingMode.TwoWay);
            sliderFeaturedWeight.VerticalOptions = LayoutOptions.Center;
            sliderFeaturedWeight.Minimum = 0.25f;
            sliderFeaturedWeight.Maximum = 5;

            contentLayout.Children.Add(sliderFeaturedWeight);

            // Image
            ExtendedImage img = new ExtendedImage()
            {
                WidthRequest = 500,
                HeightRequest = 500,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Aspect = Aspect.AspectFill
            };

            img.SetBinding<ImageLoopViewModel>(ExtendedImage.SourceProperty, vm => vm.CurrentImagePath);

            contentLayout.Children.Add(img);

            return contentLayout;
        }
    }
}