// <copyright file="FilterBarView.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Extensions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls.Sample.Views
{
    /// <summary>
    /// Filter bar for the store.
    /// </summary>
    public class FilterBarStoreView : FilterBarView
    {
        /// <summary>
        /// Command to show the profile.
        /// </summary>
        protected readonly ICommand showProfileCommand;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="showProfileCommand">Command to use.</param>
        public FilterBarStoreView(ICommand showProfileCommand) :
            base()
        {
            this.showProfileCommand = showProfileCommand;

            if (ContentLayout != null)
            {
                ContentLayout.Padding = 0;
                ContentLayout.IsClippedToBounds = false;
            }
        }

        /// <summary>
        /// Info label.
        /// </summary>
        protected Label InfoLabel { get; set; }

        /// <summary>
        /// Add the control to the layout in the proper order.
        /// </summary>
        protected override void AddControlsToLayout()
        {
            base.AddControlsToLayout();
            AddViewToLayout(InfoLabel);
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
            double maxWidth = width - (Margin * 6);

            // A third of the screen.
            double thirdMax = (maxWidth * 0.33f);
            maxWidth = maxWidth.Clamp(minWidth, thirdMax > minWidth ? thirdMax : width);

            Rectangle buttonPosition = new Rectangle();
            Rectangle infoPosition = new Rectangle();
            Rectangle entryPosition = new Rectangle();
            Rectangle profilePosition = new Rectangle();

            if (InfoLabel != null)
            {
                var elementSize = InfoLabel.GetSizeRequest(maxWidth, height - (MarginBorders * 2f)).Request;
                double elementHeight = elementSize.Height;
                double elementWidth = elementSize.Width;
                double elementLeft = (width - elementWidth) * 0.5f;
                double elementTop = Margin;

                infoPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                InfoLabel.LayoutUpdate(infoPosition);
            }

            if (true)
            {
                var elementSize = new Size(Theme.CommonResources.UserImageWidth, Theme.CommonResources.UserImageWidth);
                double elementHeight = elementSize.Height;
                double elementWidth = elementSize.Width;
                double elementLeft = 0;
                double elementTop = infoPosition.Y + infoPosition.Height + (Margin * 1.5f);

                // Center elements
                elementLeft += (width - maxWidth) * 0.5f;

                profilePosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);
            }

            if (SearchButton != null)
            {
                var elementSize = SearchButton.GetSizeRequest(maxWidth, height - (MarginBorders * 2f) - infoPosition.Height).Request;
                double elementHeight = elementSize.Height;
                double elementWidth = elementSize.Width;
                double elementLeft = maxWidth - MarginBorders - elementWidth;
                double elementTop = profilePosition.Y + ((profilePosition.Height - elementHeight) * 0.5f);

                // Center elements
                elementLeft += (width - maxWidth) * 0.5f;

                buttonPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                SearchButton.LayoutUpdate(buttonPosition);
            }

            if (FilterEntry != null)
            {
                double elementWidth = maxWidth - buttonPosition.Width - (MarginElements * 3f) - MarginBorders - profilePosition.Width;
                var elementSize = FilterEntry.GetSizeRequest(elementWidth, height - (MarginBorders * 2f) - infoPosition.Height).Request;
                double elementHeight = elementSize.Height;
                double elementLeft = MarginElements + profilePosition.X + profilePosition.Width;
                double elementTop = profilePosition.Y + ((profilePosition.Height - elementHeight) * 0.5f);

                entryPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                FilterEntry.LayoutUpdate(entryPosition);
            }

            if (InfoDecoration != null)
            {
                double elementMargin = Margin * 0.5f;
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
            double minWidth = 245;
            double maxWidth = widthConstraint - (Margin * 6);

            // A third of the screen.
            double thirdMax = (maxWidth * 0.33f);
            maxWidth = maxWidth.Clamp(minWidth, thirdMax > minWidth ? thirdMax : widthConstraint);

            Size infoSize = new Size();
            var profileSize = new Size(Theme.CommonResources.UserImageWidth, Theme.CommonResources.UserImageWidth);

            if (InfoLabel != null)
            {
                infoSize = SearchButton.GetSizeRequest(maxWidth, heightConstraint - (MarginBorders * 2f)).Request;
            }

            double maxHeight = (profileSize.Height + (MarginBorders * 2f) + infoSize.Height + Margin).Clamp(Theme.CommonResources.RoundedButtonWidth, heightConstraint);

            SizeRequest resultRequest = new SizeRequest(new Size(widthConstraint, maxHeight), new Size(widthConstraint, maxHeight));

            return resultRequest;
        }

        /// <summary>
        /// Internal initialze view.
        /// </summary>
        protected override void InternalInitializeView()
        {
            base.InternalInitializeView();

            InfoLabel = new ExtendedLabel()
            {
                Style = Theme.ApplicationStyles.PageMainTitleExtendedLabelStyle,
                Text = App.LocalizationResources.SearchIntroLabel,
                LineBreakMode = LineBreakMode.WordWrap,
                FontSize = Theme.CommonResources.TextSizeSmall,
                FontAttributes = FontAttributes.Bold,
                TextColor = Theme.CommonResources.TextColorSection,
                HorizontalTextAlignment = TextAlignment.Center
            };

            AC.ScheduleManaged(
                async () =>
                {
                    int delaycount = 0;
                    int delayOverflow = 20;
                    while ((showProfileCommand == null) && (delaycount < delayOverflow))
                    {
                        delaycount++;
                        await Task.Delay(500);
                    }
                });
        }

        /// <summary>
        /// Setup the bindings of the elements.
        /// </summary>
        protected override void SetupBindings()
        {
            base.SetupBindings();
        }
    }
}