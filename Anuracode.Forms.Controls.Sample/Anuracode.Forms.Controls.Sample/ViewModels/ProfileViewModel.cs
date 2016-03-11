// <copyright file="ProfileViewModel.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Extensions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls.Sample.ViewModels
{
    /// <summary>
    /// Login view model
    /// </summary>
    public class ProfileViewModel : BaseViewModel
    {
        /// <summary>
        /// Default thumb image path.
        /// </summary>
        private string defaultThumbImagePath = Theme.CommonResources.PathImageAppLogoLarge;

        /// <summary>
        /// Is logged in.
        /// </summary>
        private bool isLoggedIn;

        /// <summary>
        /// Logged client.
        /// </summary>
        private Model.Client loggedClient;

        /// <summary>
        /// Log in.
        /// </summary>
        private Command loginCommand;

        /// <summary>
        /// Login with facebook.
        /// </summary>
        private Command loginFacebookCommand;

        /// <summary>
        /// Login with facebook.
        /// </summary>
        private Command loginGoogleCommand;

        /// <summary>
        /// Login with facebook.
        /// </summary>
        private Command loginMicrosoftCommand;        

        /// <summary>
        /// Place order after.
        /// </summary>
        private bool placeOrderAfter;

        /// <summary>
        /// User thumb image.
        /// </summary>
        private string userThumbImage;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ProfileViewModel()
            : base()
        {
            Title = LocalizationResources.ProfileRegistrationTitleLabel;
            UserThumbImage = DefaultThumbImagePath;
        }

        /// <summary>
        /// Is logged in.
        /// </summary>
        public bool IsLoggedIn
        {
            get
            {
                return isLoggedIn;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref isLoggedIn, value);
            }
        }

        /// <summary>
        /// Logged client.
        /// </summary>
        public Model.Client LoggedClient
        {
            get
            {
                return loggedClient;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref loggedClient, value);
            }
        }

        /// <summary>
        /// Log in.
        /// </summary>
        public virtual Command LoginCommand
        {
            get
            {
                if (this.loginCommand == null)
                {
                    var tmpCommand = new Command(
                        async () =>
                        {
                            await Task.FromResult(0);
                            UpdateLoginStatus();
                        },
                        () =>
                        {
                            return !IsLoggedIn;
                        });

                    this.loginCommand = tmpCommand;
                }

                return this.loginCommand;
            }
        }

        /// <summary>
        /// Login with facebook.
        /// </summary>
        public Command LoginFacebookCommand
        {
            get
            {
                if (loginFacebookCommand == null)
                {
                    loginFacebookCommand = new Command(
                        async () =>
                        {
                            await LoginCommand.ExecuteAsync();
                        },
                        () =>
                        {
                            return LoginCommand.CanExecute();
                        });
                }

                return loginFacebookCommand;
            }
        }

        /// <summary>
        /// Login with google.
        /// </summary>
        public Command LoginGoogleCommand
        {
            get
            {
                if (loginGoogleCommand == null)
                {
                    loginGoogleCommand = new Command(
                        async () =>
                        {
                            await LoginCommand.ExecuteAsync();
                        },
                        () =>
                        {
                            return LoginCommand.CanExecute();
                        });
                }

                return loginGoogleCommand;
            }
        }

        /// <summary>
        /// Login with microsoft.
        /// </summary>
        public Command LoginMicrosoftCommand
        {
            get
            {
                if (loginMicrosoftCommand == null)
                {
                    loginMicrosoftCommand = new Command(
                        async () =>
                        {
                            await LoginCommand.ExecuteAsync();
                        },
                        () =>
                        {
                            return LoginCommand.CanExecute();
                        });
                }

                return loginMicrosoftCommand;
            }
        }

        /// <summary>
        /// Place order after.
        /// </summary>
        public bool PlaceOrderAfter
        {
            get
            {
                return placeOrderAfter;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref placeOrderAfter, value);
            }
        }

        /// <summary>
        /// User thumb image.
        /// </summary>
        public string UserThumbImage
        {
            get
            {
                if (string.IsNullOrWhiteSpace(userThumbImage))
                {
                    userThumbImage = DefaultThumbImagePath;
                }

                return userThumbImage;
            }

            set
            {
                ValidateRaiseAndSetIfChanged(ref userThumbImage, value);
            }
        }

        /// <summary>
        /// Default thumb image path.
        /// </summary>
        protected string DefaultThumbImagePath
        {
            get
            {
                return defaultThumbImagePath;
            }
        }

        /// <summary>
        /// Validate navigation on navigate.
        /// </summary>
        protected virtual bool ValidateRegistrationOnNavigate
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Update login status.
        /// </summary>
        public virtual void UpdateLoginStatus()
        {
            IsLoggedIn = false;

            LoginMicrosoftCommand.RaiseCanExecuteChanged();
            LoginFacebookCommand.RaiseCanExecuteChanged();
            LoginGoogleCommand.RaiseCanExecuteChanged();
        }
    }
}