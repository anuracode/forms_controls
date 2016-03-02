// <copyright file="AddressRowView.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Extensions;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls.Sample.Views
{
    /// <summary>
    /// Cell for an address.
    /// </summary>
    public class AddressRowView : SimpleRowBaseView
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public AddressRowView()
            : base()
        {
        }

        /// <summary>
        /// Background decoration.
        /// </summary>
        protected ShapeView BackgroundDecoration { get; set; }

        /// <summary>
        /// Address line 1.
        /// </summary>
        protected ExtendedLabel LabelLine1Value { get; set; }

        /// <summary>
        /// Address line 2.
        /// </summary>
        protected ExtendedLabel LabelLine2Value { get; set; }

        /// <summary>
        /// Nick name.
        /// </summary>
        protected ExtendedLabel LabelNickName { get; set; }

        /// <summary>
        /// Phone label.
        /// </summary>
        protected ExtendedLabel LabelPhoneValue { get; set; }

        /// <summary>
        /// Reciepent lable.
        /// </summary>
        protected ExtendedLabel LabelRecipient { get; set; }

        /// <summary>
        /// Set up cell values.
        /// </summary>
        /// <param name="isRecycled">Is recycled.</param>
        public override void SetupViewValues(bool isRecycled)
        {
            base.SetupViewValues(isRecycled);

            Model.Address address = BindingContext as Model.Address;

            if (address != null)
            {
                if (BackgroundDecoration != null)
                {
                    if (address.IsDefault)
                    {
                        if (BackgroundDecoration.Color != Theme.CommonResources.AccentLight)
                        {
                            BackgroundDecoration.Color = Theme.CommonResources.AccentLight;
                        }
                    }
                    else
                    {
                        if (BackgroundDecoration.Color != Theme.CommonResources.AccentAlternative)
                        {
                            BackgroundDecoration.Color = Theme.CommonResources.AccentAlternative;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Add controls.
        /// </summary>
        protected override void AddControlsToLayout()
        {
            base.AddControlsToLayout();

            AddViewToLayout(BackgroundDecoration, InnerConentLayout);
            AddViewToLayout(LabelNickName, InnerConentLayout);
            AddViewToLayout(LabelRecipient, InnerConentLayout);
            AddViewToLayout(LabelPhoneValue, InnerConentLayout);
            AddViewToLayout(LabelLine1Value, InnerConentLayout);
            AddViewToLayout(LabelLine2Value, InnerConentLayout);
        }

        /// <summary>
        /// Layout children.
        /// </summary>
        /// <param name="x">Top to use.</param>
        /// <param name="y">Left to use.</param>
        /// <param name="width">Width to use.</param>
        /// <param name="height">Height to use.</param>
        protected override void InnerConentLayout_OnLayoutChildren(double x, double y, double width, double height)
        {
            double calculatedWidth = width - (Margin * 4f);

            Rectangle nickPosition = new Rectangle();
            Rectangle recievPosition = new Rectangle();
            Rectangle line1Position = new Rectangle();
            Rectangle line2Position = new Rectangle();
            Rectangle phonePosition = new Rectangle();
            Rectangle distancePosition = new Rectangle();
            Rectangle previousPosition = new Rectangle();

            if ((LabelNickName) != null && LabelNickName.IsVisible)
            {
                var elementSize = LabelNickName.GetSizeRequest(calculatedWidth, height).Request;
                double elementLeft = Margin * 6f;
                double elementTop = Margin * 1f;
                double elementWidth = elementSize.Width;
                double elementHeight = elementSize.Height;

                nickPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                LabelNickName.LayoutUpdate(nickPosition);
            }

            previousPosition = nickPosition;

            if ((LabelRecipient) != null && LabelRecipient.IsVisible)
            {
                var elementSize = LabelRecipient.GetSizeRequest(calculatedWidth, height).Request;
                double elementLeft = previousPosition.Left;
                double elementTop = previousPosition.Top + previousPosition.Height;
                double elementWidth = elementSize.Width;
                double elementHeight = elementSize.Height;

                recievPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                LabelRecipient.LayoutUpdate(recievPosition);
            }
            else
            {
                recievPosition = nickPosition;
            }

            previousPosition = recievPosition;

            if ((LabelPhoneValue) != null && LabelPhoneValue.IsVisible)
            {
                var elementSize = LabelPhoneValue.GetSizeRequest(calculatedWidth, height).Request;

                double elementLeft = previousPosition.Left;
                double elementTop = previousPosition.Top + previousPosition.Height;
                double elementWidth = elementSize.Width;
                double elementHeight = elementSize.Height;

                phonePosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                LabelPhoneValue.LayoutUpdate(phonePosition);
            }
            else
            {
                phonePosition = recievPosition;
            }

            previousPosition = phonePosition;

            if ((LabelLine1Value) != null && LabelLine1Value.IsVisible)
            {
                var elementSize = LabelLine1Value.GetSizeRequest(calculatedWidth, height).Request;
                double elementLeft = previousPosition.Left;
                double elementTop = previousPosition.Top + previousPosition.Height;
                double elementWidth = elementSize.Width;
                double elementHeight = elementSize.Height;

                line1Position = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                LabelLine1Value.LayoutUpdate(line1Position);
            }
            else
            {
                line1Position = phonePosition;
            }

            previousPosition = line1Position;

            if ((LabelLine2Value) != null && LabelLine2Value.IsVisible)
            {
                var elementSize = LabelLine2Value.GetSizeRequest(calculatedWidth, height).Request;

                double elementLeft = previousPosition.Left;
                double elementTop = previousPosition.Top + previousPosition.Height;
                double elementWidth = elementSize.Width;
                double elementHeight = elementSize.Height;

                line2Position = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                LabelLine2Value.LayoutUpdate(line2Position);
            }
            else
            {
                line2Position = line1Position;
            }

            previousPosition = line2Position;

            if (BackgroundDecoration != null)
            {
                double elementWidth = height * 2f;
                double elementHeight = elementWidth;
                double elementLeft = nickPosition.X - elementWidth - Margin;
                double elementTop = (height - elementHeight) * 0.5f;

                var elementPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                BackgroundDecoration.LayoutUpdate(elementPosition);
            }
        }

        /// <summary>
        /// Mesure the content.
        /// </summary>
        /// <param name="widthConstraint">Width to constarint.</param>
        /// <param name="heightConstraint">Height to constraint.</param>
        /// <returns></returns>
        protected override SizeRequest InnerContentLayout_OnSizeRequest(double widthConstraint, double heightConstraint)
        {
            double calculatedWidth = widthConstraint - (Margin * 7f);

            Size nickSize = new Size();
            Size recieverSize = new Size();
            Size line1Size = new Size();
            Size line2Size = new Size();
            Size distnaceSize = new Size();
            Size phoneSize = new Size();

            if ((LabelNickName) != null && LabelNickName.IsVisible)
            {
                nickSize = LabelNickName.GetSizeRequest(calculatedWidth, heightConstraint).Request;
            }

            if ((LabelRecipient) != null && LabelRecipient.IsVisible)
            {
                recieverSize = LabelRecipient.GetSizeRequest(calculatedWidth, heightConstraint).Request;
            }

            if ((LabelLine1Value) != null && LabelLine1Value.IsVisible)
            {
                line1Size = LabelLine1Value.GetSizeRequest(calculatedWidth, heightConstraint).Request;
            }

            if ((LabelLine2Value) != null && LabelLine2Value.IsVisible)
            {
                line2Size = LabelLine2Value.GetSizeRequest(calculatedWidth, heightConstraint).Request;
            }

            if ((LabelPhoneValue) != null && LabelPhoneValue.IsVisible)
            {
                phoneSize = LabelPhoneValue.GetSizeRequest(calculatedWidth, heightConstraint).Request;
            }

            double calculatedHeigh = nickSize.Height + recieverSize.Height + line1Size.Height + line2Size.Height + distnaceSize.Height + phoneSize.Height + (Margin * 2f);

            return new SizeRequest(new Size(calculatedWidth, calculatedHeigh), new Size(calculatedWidth, calculatedHeigh));
        }

        /// <summary>
        /// Initialize control.
        /// </summary>
        protected override void InternalInitializeView()
        {
            base.InternalInitializeView();

            BackgroundDecoration = new ShapeView()
            {
                Color = Theme.CommonResources.AccentLight,
                ShapeType = ShapeType.Circle
            };

            // Nick name.
            LabelNickName = new ExtendedLabel()
            {
                FontAttributes = FontAttributes.Bold,
                Style = Theme.ApplicationStyles.DetailNameExtendedLabelStyle,
                HorizontalOptions = LayoutOptions.Start,
                LineBreakMode = LineBreakMode.WordWrap
            };

            LabelRecipient = new ExtendedLabel()
            {
                Style = Theme.ApplicationStyles.DetailNameExtendedLabelStyle,
                HorizontalOptions = LayoutOptions.Start,
                LineBreakMode = LineBreakMode.WordWrap
            };

            LabelPhoneValue = new ExtendedLabel()
            {
                Style = Theme.ApplicationStyles.DetailNameExtendedLabelStyle,
                HorizontalOptions = LayoutOptions.Start,
                LineBreakMode = LineBreakMode.WordWrap
            };

            LabelLine1Value = new ExtendedLabel()
            {
                Style = Theme.ApplicationStyles.DetailNameExtendedLabelStyle,
                HorizontalOptions = LayoutOptions.Start,
                LineBreakMode = LineBreakMode.WordWrap
            };

            LabelLine2Value = new ExtendedLabel()
            {
                Style = Theme.ApplicationStyles.DetailNameExtendedLabelStyle,
                HorizontalOptions = LayoutOptions.Start,
                LineBreakMode = LineBreakMode.WordWrap
            };
        }

        /// <summary>
        /// Setup the bindings of the elements.
        /// </summary>
        protected override void SetupBindings()
        {
            if (LabelNickName != null)
            {
                LabelNickName.SetBinding<Model.Address>(ExtendedLabel.TextProperty, mv => mv.NickName);
                LabelNickName.SetBinding<Model.Address>(ExtendedLabel.IsVisibleProperty, vm => vm.NickName, converter: Theme.CommonResources.StringToBooleanConverter);
            }

            if (LabelRecipient != null)
            {
                // Recipient
                LabelRecipient.SetBinding<Model.Address>(ExtendedLabel.TextProperty, mv => mv.Name);
                LabelRecipient.SetBinding<Model.Address>(ExtendedLabel.IsVisibleProperty, vm => vm.Name, converter: Theme.CommonResources.StringToBooleanConverter);
            }

            if (LabelPhoneValue != null)
            {
                // Phone
                LabelPhoneValue.SetBinding<Model.Address>(ExtendedLabel.TextProperty, vm => vm.Phone);
                LabelPhoneValue.SetBinding<Model.Address>(ExtendedLabel.IsVisibleProperty, vm => vm.Phone, converter: Theme.CommonResources.StringToBooleanConverter);
            }

            if (LabelLine1Value != null)
            {
                // Address line.
                LabelLine1Value.SetBinding<Model.Address>(ExtendedLabel.TextProperty, vm => vm.Line1);
                LabelLine1Value.SetBinding<Model.Address>(ExtendedLabel.IsVisibleProperty, vm => vm.Line1, converter: Theme.CommonResources.StringToBooleanConverter);
            }

            if (LabelLine2Value != null)
            {
                // Address line.
                LabelLine2Value.SetBinding<Model.Address>(ExtendedLabel.TextProperty, vm => vm.Line2);
                LabelLine2Value.SetBinding<Model.Address>(ExtendedLabel.IsVisibleProperty, vm => vm.Line2, converter: Theme.CommonResources.StringToBooleanConverter);
            }
        }
    }
}