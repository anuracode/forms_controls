// <copyright file="ImageLoopPage.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Sample.ViewModels;
using System;
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
            TextContentViewButton startLoopButton = new TextContentViewButton(hasBackground: true)
            {
                Text = "Start",
                Style = Theme.ApplicationStyles.TextOnlyImportantContentButtonStyle,
                Command = ViewModel.StartLoopCommand,
                InvisibleWhenDisabled = true
            };

            buttonsLayout.Children.Add(startLoopButton);

            // Stop button.
            TextContentViewButton stopLoopButton = new TextContentViewButton(hasBackground: true)
            {
                Text = "Stop",
                Style = Theme.ApplicationStyles.TextOnlyImportantContentButtonStyle,
                Command = ViewModel.StopLoopCommand,
                InvisibleWhenDisabled = true
            };

            buttonsLayout.Children.Add(stopLoopButton);
            contentLayout.Children.Add(buttonsLayout);

            // Count.
            Theme.RenderUtil.RenderReadField(contentLayout, labelValue: "Count", expressionValue: "CurrentCount", orientation: StackOrientation.Horizontal);

            // Path.
            Theme.RenderUtil.RenderReadField(contentLayout, labelValue: "Path", expressionValue: "CurrentImagePath", orientation: StackOrientation.Horizontal);

            // Delay.
            Theme.RenderUtil.RenderReadField(contentLayout, labelValue: "Delay", expressionValue: "DelaySeconds", orientation: StackOrientation.Horizontal);

            ExtendedSlider sliderFeaturedWeight = new ExtendedSlider();
            sliderFeaturedWeight.StepValue = 0.25f;
            sliderFeaturedWeight.SetBinding(ExtendedSlider.ValueProperty, "DelaySeconds", BindingMode.TwoWay);
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

            img.SetBinding(ExtendedImage.SourceProperty, "CurrentImagePath");

            contentLayout.Children.Add(img);

            // Sample formated text.
            FormattedString fs = new FormattedString();
            fs.Spans.Add(
                new Span()
                {
                    Text = "Title",
                    FontAttributes = FontAttributes.Bold,
                    FontSize = Theme.CommonResources.TextSizeLarge
                });

            fs.Spans.Add(
                new Span()
                {
                    Text = Environment.NewLine
                });

            fs.Spans.Add(
                new Span()
                {
                    Text = "Subtitle",
                    FontAttributes = FontAttributes.Italic,
                    FontSize = Theme.CommonResources.TextSizeMedium
                });

            fs.Spans.Add(
               new Span()
               {
                   Text = Environment.NewLine
               });

            fs.Spans.Add(
               new Span()
               {
                   Text = "Some text that makes no sense, aasdklj askj alkjjda asdkj hnbpe npqnsp knasp eiena asdoe ppos eojd pnjjnsp okjs lkjasdkjs oiena,xpe oksndno.",
                   FontSize = Theme.CommonResources.TextSizeMedium
               });

            fs.Spans.Add(
              new Span()
              {
                  Text = Environment.NewLine
              });

            fs.Spans.Add(
               new Span()
               {
                   Text = "asdk asdk e pjeslkjs pedsaskd lkjsdposd ejsdlkjs sdlkajsd aslkjasd sskljdalkds eposdn slkjdepslkjs pekjdsd",
                   FontSize = Theme.CommonResources.TextSizeMicro
               });

            var descriptionLabel = new ExtendedLabel()
            {
                FontName = Theme.CommonResources.FontRobotLightName,
                FriendlyFontName = Theme.CommonResources.FontRobotLightFriendlyName,
                TextColor = Color.Black,
                FontSize = Theme.CommonResources.TextSizeMedium,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                HorizontalTextAlignment = TextAlignment.Start,
                FormattedText = fs
            };

            contentLayout.Children.Add(descriptionLabel);

            var viewScroll = new ScrollView()
            {
                Orientation = ScrollOrientation.Vertical,
                Content = contentLayout,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            return viewScroll;
        }
    }
}