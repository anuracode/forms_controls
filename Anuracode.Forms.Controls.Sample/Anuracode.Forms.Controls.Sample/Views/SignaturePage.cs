// <copyright file="SignaturePage.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Extensions;
using Anuracode.Forms.Controls.Sample.ViewModels;
using Anuracode.Forms.Controls.Views.Extensions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls.Sample.Views
{
    /// <summary>
    /// Page for the about.
    /// </summary>
    public class SignaturePage : ContentBaseView<SignatureViewModel>
    {
        /// <summary>
        /// Hide signature.
        /// </summary>
        private Command hideSignaturePadCommand;

        /// <summary>
        /// Flag for the large detail.
        /// </summary>
        private bool isSignaturePadVisible;

        /// <summary>
        /// Command for showing signature pad.
        /// </summary>
        private Command saveSignatureCommand;

        /// <summary>
        /// Show the item detail.
        /// </summary>
        private Command showSignaturePadCommand;

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
        /// Hide signature.
        /// </summary>
        public Command HideSignaturePadCommand
        {
            get
            {
                if (hideSignaturePadCommand == null)
                {
                    hideSignaturePadCommand = new Command(
                        async () =>
                        {
                            await Task.FromResult(0);

                            if (IsSignaturePadVisible)
                            {
                                IsSignaturePadVisible = false;

                                if (SignatureView != null)
                                {
                                    SignatureView.HideOverlayCommand.ExecuteIfCan();
                                }
                            }
                        },
                        () =>
                        {
                            return IsSignaturePadVisible;
                        });
                }

                return hideSignaturePadCommand;
            }
        }

        /// <summary>
        /// Flag for the large detail.
        /// </summary>
        public bool IsSignaturePadVisible
        {
            get
            {
                return isSignaturePadVisible;
            }

            set
            {
                if (isSignaturePadVisible != value)
                {
                    isSignaturePadVisible = value;
                    OnPropertyChanged("IsLargeDetailVisible");
                    ShowSignaturePadCommand.RaiseCanExecuteChanged();
                    HideSignaturePadCommand.RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        /// Command for showing signature pad.
        /// </summary>
        public Command SaveSignatureCommand
        {
            get
            {
                if (this.saveSignatureCommand == null)
                {
                    var tmpCommand = new Command(
                        async () =>
                        {
                            await Task.FromResult(0);

                            HideSignaturePadCommand.ExecuteIfCan();

                            if (SignatureView != null)
                            {
                                var signaturePointsString = await SignatureView.GetPointsStringAsync();
                            }

                            NavigateBackCommand.ExecuteIfCan();
                        },
                        () =>
                        {
                            return (SignatureView != null) && !SignatureView.IsBlank;
                        });

                    this.saveSignatureCommand = tmpCommand;
                }

                return this.saveSignatureCommand;
            }
        }

        /// <summary>
        /// Show the items options.
        /// </summary>
        public Command ShowSignaturePadCommand
        {
            get
            {
                if (showSignaturePadCommand == null)
                {
                    showSignaturePadCommand = new Command(
                        async () =>
                        {
                            await Task.FromResult(0);

                            IsSignaturePadVisible = true;

                            SignatureView.ShowOverlayCommand.ExecuteIfCan();
                        },
                        () =>
                        {
                            return !IsSignaturePadVisible;
                        });
                }

                return showSignaturePadCommand;
            }
        }

        /// <summary>
        /// Signature view.
        /// </summary>
        protected SignatureLargeSimpleView SignatureView { get; set; }

        /// <summary>
        /// Add extra layers.
        /// </summary>
        protected override void AddExtraLayers()
        {
            base.AddExtraLayers();

            AddSignaturePadView();
        }

        /// <summary>
        /// Add cart large view.
        /// </summary>
        protected void AddSignaturePadView()
        {
            if (SignatureView == null && PageCanvas != null)
            {
                SignatureView = new SignatureLargeSimpleView()
                {
                    ContentMargin = ContentMargin,
                    CloseOverlayCommand = HideSignaturePadCommand,
                    BottomAppBarMargin = BottomAppBarMargin,
                    NavigateBackCommand = NavigateBackCommand,
                    SaveCommand = SaveSignatureCommand,
                    BindingContext = ViewModel
                };

                SignatureView.PropertyChanged += SignatureView_PropertyChanged;

                PageCanvas.Children.Add(SignatureView);
            }
        }

        /// <summary>
        /// Layout the children for the background.
        /// </summary>
        /// <param name="pageSize">Page size.</param>
        /// <param name="headerPosition">Header position.</param>
        /// <param name="contentPosition">Content position.</param>
        /// <param name="footerPosition">Footer position.</param>
        protected override void PageExtraLayersLayoutChildren(Rectangle pageSize, Rectangle headerPosition, Rectangle contentPosition, Rectangle footerPosition)
        {
            base.PageExtraLayersLayoutChildren(pageSize, headerPosition, contentPosition, footerPosition);

            if (SignatureView != null)
            {
                SignatureView.BottomAppBarMargin = BottomAppBarMargin;
                SignatureView.TopLayoutMargin = headerPosition.Y - ContentMargin;

                SignatureView.LayoutUpdate(pageSize);
            }
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

            TextContentViewButton showSignatureButton = new TextContentViewButton()
            {
                Text = App.LocalizationResources.ShowSignatureLabel,
                Style = Theme.ApplicationStyles.TextOnlyImportantContentButtonStyle,
                Command = ShowSignaturePadCommand
            };

            contentLayout.Children.Add(showSignatureButton);

            return contentLayout;
        }

        /// <summary>
        /// Event when a property changes in the view.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Arguments of the event.</param>
        private void SignatureView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsBlank")
            {
                SaveSignatureCommand.RaiseCanExecuteChanged();
            }
        }
    }
}