// <copyright file="IntroPage.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System.Threading.Tasks;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls.Sample.Views
{
    /// <summary>
    /// Intro page.
    /// </summary>
    public class IntroPage : BaseView
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public IntroPage()
        {
            RelativeLayout gridIntro = new RelativeLayout()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            LogoImage = new ExtendedImage()
            {
                Source = Theme.CommonResources.PathImageAppLogoLarge,
                Aspect = Aspect.AspectFit,
                Opacity = 1
            };

            double imageWidthPercent = 0.20;

            gridIntro.Children.Add(
                LogoImage,
                Constraint.RelativeToParent(
                (parent) =>
                {
                    return (parent.Width * 0.5f) - ((parent.Width * imageWidthPercent) * 0.5f);
                }),
                Constraint.RelativeToParent(
                (parent) =>
                {
                    return (parent.Height * 0.5f) - ((parent.Width * imageWidthPercent) * 0.5f);
                }),
                Constraint.RelativeToParent(
                (parent) =>
                {
                    return (parent.Width * imageWidthPercent);
                }),
                Constraint.RelativeToParent(
                (parent) =>
                {
                    return (parent.Width * imageWidthPercent);
                }));

            this.Content = gridIntro;
        }

        /// <summary>
        /// Logo image.
        /// </summary>
        protected ExtendedImage LogoImage { get; set; }

        /// <summary>
        /// Page appears.
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();

            AC.ScheduleManaged(
                async () =>
                {
                    await Task.FromResult(0);

                    if (App.InitPageCompleteAction != null)
                    {
                        App.InitPageCompleteAction();
                    }
                });
        }
    }
}