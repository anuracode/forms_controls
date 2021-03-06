﻿// <copyright file="ProfilePage.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Sample.ViewModels;
using Anuracode.Forms.Controls.Extensions;
using Anuracode.Forms.Controls.Views.Extensions;
using Xamarin.Forms;
using Anuracode.Forms.Controls.Sample.Model;

namespace Anuracode.Forms.Controls.Sample.Views
{
    /// <summary>
    /// Page for the about.
    /// </summary>
    public class ProfilePage : ContentBaseView<ProfileViewModel>
    {
        /// <summary>
        /// Command for the logo.
        /// </summary>
        private Command logoAnimateCommand;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="viewModel">View model to use.</param>
        public ProfilePage(ProfileViewModel viewModel)
            : base(viewModel)
        {
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ProfilePage()
            : this(null)
        {
        }

        /// <summary>
        /// Command for the logo.
        /// </summary>
        public Command LogoAnimateCommand
        {
            get
            {
                if (logoAnimateCommand == null)
                {
                    logoAnimateCommand = new Command(
                        async () =>
                        {
                            if (ImageLogoButton != null)
                            {
                                await ImageLogoButton.RotateTo(360, easing: Easing.Linear);
                                await ImageLogoButton.RotateTo(0, 0);
                            }
                        });
                }

                return logoAnimateCommand;
            }
        }

        /// <summary>
        /// Background image.
        /// </summary>
        protected ExtendedImage BackgroundExtendedImage { get; set; }

        /// <summary>
        /// Has a background that needs render.
        /// </summary>
        protected override bool HasRenderBackground
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Image logo.
        /// </summary>
        protected CircleImage ImageLogoButton { get; set; }

        /// <summary>
        /// Grid top info.
        /// </summary>
        private Grid GridTopInfo { get; set; }

        /// <summary>
        /// Layout the children for the background.
        /// </summary>
        /// <param name="pageSize">Page size.</param>
        /// <param name="headerPosition">Header position.</param>
        /// <param name="contentPosition">Content position.</param>
        /// <param name="footerPosition">Footer position.</param>
        protected override void PageBackgroundLayoutChildren(Rectangle pageSize, Rectangle headerPosition, Rectangle contentPosition, Rectangle footerPosition)
        {
            if (ViewModel.IsLoggedIn)
            {
                if (BackgroundExtendedImage != null)
                {
                    double infoHeight = 0;

                    if (GridTopInfo != null)
                    {
                        infoHeight = GridTopInfo.Height.Clamp(headerPosition.Height, pageSize.Height);
                    }

                    double elementLeft = 0;
                    double elementTop = headerPosition.Y + headerPosition.Height - (ContentMargin * 3f);
                    double elementWidth = pageSize.Width;
                    double elementHeight = infoHeight + (ContentMargin * 3.5f);

                    var elementPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);

                    BackgroundExtendedImage.LayoutUpdate(elementPosition);
                }
            }
            else
            {
                if (BackgroundExtendedImage != null)
                {
                    BackgroundExtendedImage.LayoutUpdate(pageSize);
                }
            }
        }

        /// <summary>
        /// Render background layout.
        /// </summary>
        /// <param name="absoluteLayout">Layout to add the items to.</param>
        protected override void RenderBackgroundLayout(SimpleLayout absoluteLayout)
        {
            BackgroundExtendedImage = new ExtendedImage()
            {
                Source = Theme.CommonResources.PathImageProfileBackground,
                Aspect = Aspect.AspectFill,
                IsVisible = true,
                Opacity = 1
            };

            absoluteLayout.Children.Add(BackgroundExtendedImage);
        }

        /// <summary>
        /// Content of the page.
        /// </summary>
        protected override View RenderContent()
        {
            StackLayout stackGeneral = new StackLayout()
            {
                Style = Theme.ApplicationStyles.FormRowContainerStyle
            };

            StackLayout stackLoggedIn = new StackLayout()
            {
                Style = Theme.ApplicationStyles.FormRowContainerStyle
            };

            stackLoggedIn.SetBinding(StackLayout.IsVisibleProperty, "IsLoggedIn");

            GridTopInfo = new Grid()
            {
                RowSpacing = 0,
                ColumnSpacing = 0
            };

            StackLayout stackmainInfo = new StackLayout()
            {
                Style = Theme.ApplicationStyles.FormRowContainerStyle
            };

            var stackDetail = Theme.RenderUtil.RenderIdentedVerticalContainer(stackLoggedIn);

            double imageProfileWidth = 120;

            StackLayout stackLoggedInButtons = new StackLayout()
            {
                Style = Theme.ApplicationStyles.FormRowContainerStyle,
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.End
            };

            ContentViewButton buttonEdit = new GlyphOnlyContentViewButton()
            {
                Style = Theme.ApplicationStyles.GlyphOnlyRoundedContentButtonStyle,
                HorizontalOptions = LayoutOptions.Center,
                GlyphText = Theme.CommonResources.GlyphTextEdit
            };

            buttonEdit.SetBinding(ContentViewButton.TextProperty, "LocalizationResources.EditButton");
            // buttonEdit.SetBinding(ContentViewButton.CommandProperty, vm => vm.EditClientInfoCommand);
            stackLoggedInButtons.Children.Add(buttonEdit);

            ContentViewButton buttonLogout = new GlyphOnlyContentViewButton()
            {
                Style = Theme.ApplicationStyles.GlyphOnlyRoundedContentButtonStyle,
                HorizontalOptions = LayoutOptions.Center,
                GlyphText = Theme.CommonResources.GlyphTextLogout
            };

            buttonLogout.SetBinding(ContentViewButton.TextProperty, "LocalizationResources.SignOutButton");
            // buttonLogout.SetBinding(ContentViewButton.CommandProperty, vm => vm.LogoutCommand);
            stackLoggedInButtons.Children.Add(buttonLogout);

            stackmainInfo.Children.Add(stackLoggedInButtons);

            // Profile image.
            Image imageProfile = null;
            imageProfile = new ExtendedImage();

            imageProfile.Aspect = Aspect.AspectFill;
            imageProfile.VerticalOptions = LayoutOptions.Center;
            imageProfile.HorizontalOptions = LayoutOptions.Center;
            imageProfile.WidthRequest = imageProfileWidth;
            imageProfile.HeightRequest = imageProfileWidth;

            imageProfile.SetBinding(Image.SourceProperty, "UserThumbImage");

            stackmainInfo.Children.Add(imageProfile);

            // Full name.
            ExtendedLabel labelName = new ExtendedLabel()
            {
                Style = Theme.ApplicationStyles.DefaultExtendedLabelStyle,
                TextColor = Theme.CommonResources.Accent,
                HorizontalOptions = LayoutOptions.Center
            };

            // labelName.SetBinding(Label.TextProperty, vm => vm.UserInfo.Name);

            stackmainInfo.Children.Add(labelName);

            // Provider used.
            ExtendedLabel labelProvider = new ExtendedLabel()
            {
                Style = Theme.ApplicationStyles.DefaultExtendedLabelStyle,
                HorizontalOptions = LayoutOptions.Center,
                FontSize = Theme.CommonResources.TextSizeMicro * 0.7
            };

            // labelProvider.SetBinding(Label.TextProperty, vm => vm.UserInfo.Provider);

            stackmainInfo.Children.Add(labelProvider);

            Theme.RenderUtil.RenderSpace(stackmainInfo, heightRequest: 15);

            var sizeRequest = stackmainInfo.Measure(ContentViewButton.MAX_WIDTH, ContentViewButton.MAX_HEIGHT);

            GridTopInfo.Children.Add(stackmainInfo);
            stackDetail.Children.Add(GridTopInfo);

            Theme.RenderUtil.RenderSpace(stackDetail, heightRequest: 25);

            double lineHeight = 1;
            Color lineColor = Theme.CommonResources.SubtleBackgroundColor;

            // First Name.
            Theme.RenderUtil.RenderReadField(stackDetail, "LocalizationResources.FirstNameLabel", "LoggedClient.Name", orientation: StackOrientation.Horizontal, horizontalOptions: TextAlignment.Center);

            Theme.RenderUtil.RenderLineSeparator(stackDetail, lineColor, heightRequest: lineHeight);

            // Last Name.
            Theme.RenderUtil.RenderReadField(stackDetail, "LocalizationResources.LastNameLabel", "LoggedClient.Name", orientation: StackOrientation.Horizontal, horizontalOptions: TextAlignment.Center);

            Theme.RenderUtil.RenderLineSeparator(stackDetail, lineColor, heightRequest: lineHeight);

            // Nick name.
            Theme.RenderUtil.RenderReadField(stackDetail, "LocalizationResources.NickNameLabel", "LoggedClient.NickName", orientation: StackOrientation.Horizontal, horizontalOptions: TextAlignment.Center);

            Theme.RenderUtil.RenderLineSeparator(stackDetail, lineColor, heightRequest: lineHeight);

            // Mobile phone.
            Theme.RenderUtil.RenderReadField(stackDetail, "LocalizationResources.MobilePhoneLabel", "LoggedClient.MobilePhone", orientation: StackOrientation.Horizontal, horizontalOptions: TextAlignment.Center);

            Theme.RenderUtil.RenderLineSeparator(stackDetail, lineColor, heightRequest: lineHeight);

            // phone.
            Theme.RenderUtil.RenderReadField(stackDetail, "LocalizationResources.PhoneLabel", "LoggedClient.Phone", orientation: StackOrientation.Horizontal, horizontalOptions: TextAlignment.Center);

            Theme.RenderUtil.RenderLineSeparator(stackDetail, lineColor, heightRequest: lineHeight);

            // Contact mail.
            ContentViewButton buttonMailValue = new TextContentViewButton()
            {
                Style = Theme.ApplicationStyles.LinkContentViewButtonStyle,
                // Command = BaseViewModel.OpenUrlCommand
            };

            // buttonMailValue.SetBinding(ContentViewButton.TextProperty, vm => vm.UserInfo.Email);
            // buttonMailValue.SetBinding(ContentViewButton.CommandParameterProperty, vm => vm.UserInfo.Email, stringFormat: BaseViewModel.PREFIX_EMAIL + "{0}");

            Theme.RenderUtil.RenderField(stackDetail, "LocalizationResources.EmailLabel", orientation: StackOrientation.Horizontal, contentView: buttonMailValue, horizontalOptions: TextAlignment.Center);

            // When logged in.
            stackGeneral.Children.Add(stackLoggedIn);

            // When logout.
            StackLayout stackButtons = new StackLayout()
            {
                Style = Theme.ApplicationStyles.FormRowContainerStyle,
                Orientation = (Device.Idiom == TargetIdiom.Phone) ? StackOrientation.Vertical : StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            double buttonWidth = 150;

            ContentViewButton buttonLoginFacebook = new ContentViewButton(true, true, ImageOrientation.ImageToLeft, hasBackground: true)
            {
                Style = Theme.ApplicationStyles.TextOnlyImportantContentButtonStyle,
                HorizontalOptions = LayoutOptions.Center,
                ButtonBackgroundColor = Color.FromHex("5874B2"),
                TextColor = Color.White,
                Source = StoreItem.DEFAULT_SERVER_IMAGE_PATH + "sign-facebook.png",
                WidthRequest = buttonWidth
            };

            buttonLoginFacebook.Text = "Facebook";
            buttonLoginFacebook.SetBinding(ContentViewButton.CommandProperty, "LoginFacebookCommand");
            stackButtons.Children.Add(buttonLoginFacebook);

            ContentViewButton buttonLoginGoogle = new ContentViewButton(true, true, ImageOrientation.ImageToLeft, hasBackground: true)
            {
                Style = Theme.ApplicationStyles.TextOnlyImportantContentButtonStyle,
                HorizontalOptions = LayoutOptions.Center,
                ButtonBackgroundColor = Color.FromHex("F62B03"),
                TextColor = Color.White,
                Source = StoreItem.DEFAULT_SERVER_IMAGE_PATH + "sign-google.png",
                WidthRequest = buttonWidth
            };
            buttonLoginGoogle.Text = "Google+";
            buttonLoginGoogle.SetBinding(ContentViewButton.CommandProperty, "LoginGoogleCommand");
            stackButtons.Children.Add(buttonLoginGoogle);

            ContentViewButton buttonLoginMicrosoft = new ContentViewButton(true, true, ImageOrientation.ImageToLeft, hasBackground: true)
            {
                Style = Theme.ApplicationStyles.TextOnlyImportantContentButtonStyle,
                ButtonBackgroundColor = Color.White,
                TextColor = Color.Black,
                StrokeColor = Color.Black,
                StrokeWidth = 1,
                HorizontalOptions = LayoutOptions.Center,
                Source = StoreItem.DEFAULT_SERVER_IMAGE_PATH + "sign-microsoft.png",
                WidthRequest = buttonWidth
            };
            buttonLoginMicrosoft.Text = "Microsoft";
            buttonLoginMicrosoft.SetBinding(ContentViewButton.CommandProperty, "LoginMicrosoftCommand");
            stackButtons.Children.Add(buttonLoginMicrosoft);

            StackLayout stackLoggedOut = new StackLayout()
            {
                Style = Theme.ApplicationStyles.FormRowContainerStyle
            };

            stackLoggedOut.SetBinding(StackLayout.IsVisibleProperty, "IsLoggedIn", converter: Theme.CommonResources.InvertBooleanToBooleanConverter);

            double marginLogin = 40;

            Theme.RenderUtil.RenderSpace(stackLoggedOut, marginLogin);

            double marginBordersLogo = 5;

            // Image logo.
            ImageLogoButton = new CircleImage()
            {
                Source = Theme.CommonResources.PathImageAppLogoLargeMirror,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Aspect = Aspect.AspectFill,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                BorderThickness = 1,
                BorderColor = Theme.CommonResources.Accent,
                WidthRequest = imageProfileWidth + marginBordersLogo,
                HeightRequest = imageProfileWidth + marginBordersLogo
            };

            stackLoggedOut.Children.Add(ImageLogoButton);

            Theme.RenderUtil.RenderSpace(stackLoggedOut, heightRequest: 20);

            ExtendedLabel labelLoginInfoText = new ExtendedLabel()
            {
                Style = Theme.ApplicationStyles.DescriptionExtendedLabelStyle,
                Text = App.LocalizationResources.ProfileLoginLabel,
                TextColor = Color.Black,
                FontSize = Theme.CommonResources.TextSizeSmall,
                HorizontalTextAlignment = TextAlignment.Center,
                LineBreakMode = LineBreakMode.WordWrap
            };

            stackLoggedOut.Children.Add(labelLoginInfoText);

            Theme.RenderUtil.RenderSpace(stackLoggedOut, marginLogin);

            ExtendedLabel labelLoginSelectProviderText = new ExtendedLabel()
            {
                Style = Theme.ApplicationStyles.DescriptionExtendedLabelStyle,
                Text = string.Format("{0}", App.LocalizationResources.ProfileLoginChooseLabel.ToUpper()),
                TextColor = Theme.CommonResources.Accent,
                HorizontalTextAlignment = TextAlignment.Center,
                LineBreakMode = LineBreakMode.WordWrap,
                FontAttributes = FontAttributes.Bold
            };

            stackLoggedOut.Children.Add(labelLoginSelectProviderText);

            Theme.RenderUtil.RenderSpace(stackLoggedOut);

            stackLoggedOut.Children.Add(stackButtons);

            Theme.RenderUtil.RenderSpace(stackLoggedOut, marginLogin);

            stackGeneral.Children.Add(stackLoggedOut);

            ScrollView mainScroll = new ScrollView()
            {
                Content = stackGeneral
            };

            ViewModel.UpdateLoginStatus();

            return mainScroll;
        }
    }
}