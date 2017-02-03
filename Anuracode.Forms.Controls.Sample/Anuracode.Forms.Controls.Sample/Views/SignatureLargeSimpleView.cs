// <copyright file="SignatureLargeSimpleView.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Extensions;
using Anuracode.Forms.Controls.Sample.ViewModels;
using Anuracode.Forms.Controls.Views.Extensions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls.Sample.Views
{
    /// <summary>
    /// Store detail view.
    /// </summary>
    public class SignatureLargeSimpleView : SimpleViewBase
    {
        /// <summary>
        /// Command for closing overlay.
        /// </summary>
        private ICommand closeOverlayCommand;

        /// <summary>
        /// Hide cart command.
        /// </summary>
        private Command hideOverlayCommand;

        /// <summary>
        /// Navigate back depending on the view.
        /// </summary>
        private Command internalNavigateBackCommand;

        /// <summary>
        /// Flag for the cart.
        /// </summary>
        private bool isOverlayVisible;

        /// <summary>
        /// Command for saving.
        /// </summary>
        private ICommand saveCommand;

        /// <summary>
        /// Show cart command.
        /// </summary>
        private Command showOverlayCommand;

        /// <summary>
        /// View model to use.
        /// </summary>
        private SignatureViewModel viewModel;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SignatureLargeSimpleView()
            : base(false)
        {
            if (ContentLayout != null)
            {
                ContentLayout.IsClippedToBounds = true;
            }

            if (IsOverlay)
            {
                Opacity = 0;
                IsVisible = false;
            }
        }

        /// <summary>
        /// Margin for the bottom app bar.
        /// </summary>
        public virtual double BottomAppBarMargin { get; set; }

        /// <summary>
        /// Command for closing overlay.
        /// </summary>
        public ICommand CloseOverlayCommand
        {
            get
            {
                return closeOverlayCommand;
            }

            set
            {
                if (closeOverlayCommand != value)
                {
                    closeOverlayCommand = value;

                    OnPropertyChanged("CloseOverlayCommand");
                }

                if (CloseButton != null)
                {
                    AC.ScheduleManaged(
                        () =>
                        {
                            CloseButton.Command = CloseOverlayCommand;
                        });
                }
            }
        }

        /// <summary>
        /// Hide overlay.
        /// </summary>
        public Command HideOverlayCommand
        {
            get
            {
                if (hideOverlayCommand == null)
                {
                    hideOverlayCommand = new Command(
                        async () =>
                        {
                            if (IsOverlayVisible)
                            {
                                IsOverlayVisible = false;

                                await Theme.RenderUtil.AnimateFadeOutViewAsync(CloseButton);
                                await Theme.RenderUtil.AnimateFadeOutViewAsync(ContentDetailLayout);

                                await Theme.RenderUtil.AnimateFadeOutViewAsync(BackgroundWaterMark);

                                if (Opacity > 0)
                                {
                                    Opacity = 0;
                                }

                                if (IsVisible)
                                {
                                    IsVisible = false;
                                }
                            }
                        },
                        () =>
                        {
                            return IsOverlayVisible;
                        });
                }

                return hideOverlayCommand;
            }
        }

        /// <summary>
        /// Is blank.
        /// </summary>
        public bool IsBlank
        {
            get
            {
                bool isBlank = true;

                if (SignatureView != null)
                {
                    isBlank = SignatureView.IsBlank;
                }

                if (Device.OS == TargetPlatform.WinPhone)
                {
                    isBlank = false;
                }

                return isBlank;
            }
        }

        /// <summary>
        /// Flag for the cart.
        /// </summary>
        public bool IsOverlayVisible
        {
            get
            {
                return isOverlayVisible;
            }

            set
            {
                if (isOverlayVisible != value)
                {
                    isOverlayVisible = value;
                    OnPropertyChanged("IsOverlayVisible");
                    ShowOverlayCommand.RaiseCanExecuteChanged();
                    HideOverlayCommand.RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        /// Nacigate back.
        /// </summary>
        public Command NavigateBackCommand { get; set; }

        /// <summary>
        /// Command for saving.
        /// </summary>
        public ICommand SaveCommand
        {
            get
            {
                return saveCommand;
            }

            set
            {
                if (saveCommand != value)
                {
                    saveCommand = value;

                    OnPropertyChanged("SaveCommand");
                }

                if (ButtonSave != null)
                {
                    AC.ScheduleManaged(
                        () =>
                        {
                            ButtonSave.Command = SaveCommand;
                        });
                }
            }
        }

        /// <summary>
        /// Show store.
        /// </summary>
        public Command ShowOverlayCommand
        {
            get
            {
                if (showOverlayCommand == null)
                {
                    showOverlayCommand = new Command(
                        async () =>
                        {
                            InitializeView();
                            PrepareBindings();

                            await Task.FromResult(0);

                            if (!IsOverlayVisible)
                            {
                                IsOverlayVisible = true;

                                await Task.Delay(10);

                                // Prepare animation.
                                if (Opacity < 1)
                                {
                                    Opacity = 1;
                                }

                                Theme.RenderUtil.AnimatePrepareFadeInView(BackgroundWaterMark);
                                Theme.RenderUtil.AnimatePrepareFadeInView(CloseButton);
                                Theme.RenderUtil.AnimatePrepareFadeInView(ContentDetailLayout);

                                // No animation.
                                if (!IsVisible)
                                {
                                    IsVisible = true;
                                }

                                await Theme.RenderUtil.AnimateFadeInViewAsync(BackgroundWaterMark);

                                await Theme.RenderUtil.AnimateFadeInViewAsync(ContentDetailLayout);

                                await Theme.RenderUtil.AnimateFadeInViewAsync(CloseButton);
                            }
                        },
                        () =>
                        {
                            return !IsOverlayVisible;
                        });
                }

                return showOverlayCommand;
            }
        }

        /// <summary>
        /// Top layout margin.
        /// </summary>
        public double TopLayoutMargin { get; set; }

        /// <summary>
        /// View model to use.
        /// </summary>
        public SignatureViewModel ViewModel
        {
            get
            {
                if (viewModel == null)
                {
                    viewModel = BindingContext as SignatureViewModel;
                }

                return viewModel;
            }

            set { viewModel = value; }
        }

        /// <summary>
        /// Background
        /// </summary>
        protected View BackgroundWaterMark { get; set; }

        /// <summary>
        /// Bottom shape.
        /// </summary>
        protected ShapeView BottomShape { get; set; }

        /// <summary>
        /// Button count.
        /// </summary>
        protected ContentViewButton ButtonClear { get; set; }

        /// <summary>
        /// Button add.
        /// </summary>
        protected ContentViewButton ButtonSave { get; set; }

        /// <summary>
        /// Layout for the image.
        /// </summary>
        protected SimpleLayout ContentDetailLayout { get; set; }

        /// <summary>
        /// Has control bar.
        /// </summary>
        protected bool HasControlBar
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Flag for vertical transition.
        /// </summary>
        protected virtual bool HasVerticalTransition
        {
            get
            {
                return Device.OS == TargetPlatform.iOS;
            }
        }

        /// <summary>
        /// Navigate back depending on the view.
        /// </summary>
        protected Command InternalNavigateBackCommand
        {
            get
            {
                if (internalNavigateBackCommand == null)
                {
                    internalNavigateBackCommand = new Command(
                        async () =>
                        {
                            await Task.FromResult(0);

                            if (NavigateBackCommand != null)
                            {
                                NavigateBackCommand.ExecuteIfCan();
                            }
                        });
                }

                return internalNavigateBackCommand;
            }
        }

        /// <summary>
        /// Flag when thew view is render as overlay.
        /// </summary>
        protected virtual bool IsOverlay
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Last size.
        /// </summary>
        protected Rectangle LastSizePageCanvas { get; set; }

        /// <summary>
        /// Signature view.
        /// </summary>
        protected SignaturePadView SignatureView { get; set; }

        /// <summary>
        /// Close button.
        /// </summary>
        private ContentViewButton CloseButton { get; set; }

        /// <summary>
        /// Get the draw point.
        /// </summary>
        public async Task<string> GetImageStringBase64Async()
        {
            string result = null;

            if (SignatureView != null)
            {
                result = await SignatureView.GetImageStringBase64Async();
            }

            return result;
        }

        /// <summary>
        /// Get the points in string format.
        /// </summary>
        public async Task<string> GetPointsStringAsync()
        {
            string result = null;

            if (SignatureView != null)
            {
                result = await SignatureView.GetPointsStringAsync();
            }

            return result;
        }

        /// <summary>
        /// Add controls to layout.
        /// </summary>
        protected override void AddControlsToLayout()
        {
            AddViewToLayout(BackgroundWaterMark);
            AddViewToLayout(ContentDetailLayout);
            AddViewToLayout(BottomShape, ContentDetailLayout);
            AddViewToLayout(SignatureView, ContentDetailLayout);

            AddViewToLayout(ButtonClear, ContentDetailLayout);
            AddViewToLayout(ButtonSave, ContentDetailLayout);

            AddViewToLayout(CloseButton);
        }

        /// <summary>
        /// Layout children.
        /// </summary>
        /// <param name="x">Top to use.</param>
        /// <param name="y">Left to use.</param>
        /// <param name="width">Width to use.</param>
        /// <param name="height">Height to use.</param>
        protected virtual void ContentDetailLayout_OnLayoutChildren(double x, double y, double width, double height)
        {
            Rectangle savePosition = new Rectangle();
            Rectangle clearPosition = new Rectangle();
            Rectangle signaturePosition = new Rectangle();

            double marginMultiplier = 0.5f;

            if (ButtonClear != null)
            {
                var elementSize = ButtonClear.Measure(width, height).Request;
                double elementWidth = elementSize.Width;
                double elementHeight = elementSize.Height;
                double elementTop = height - (ContentMargin * marginMultiplier) - elementHeight;
                double elementLeft = (ContentMargin * marginMultiplier);

                clearPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);
                ButtonClear.LayoutUpdate(clearPosition);
            }

            if (ButtonSave != null)
            {
                var elementSize = ButtonSave.Measure(width, height).Request;
                double elementWidth = elementSize.Width;
                double elementHeight = elementSize.Height;
                double elementTop = height - (ContentMargin * marginMultiplier) - elementHeight;
                double elementLeft = width - (ContentMargin * marginMultiplier) - elementWidth;

                savePosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);
                ButtonSave.LayoutUpdate(savePosition);
            }

            if (BottomShape != null)
            {
                double elementLeft = 0;
                double elementTop = 0;
                double elementWidth = width;
                double elementHeight = height;
                var elementPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);
                BottomShape.LayoutUpdate(elementPosition);
            }

            if (SignatureView != null)
            {
                double elementTop = (ContentMargin * marginMultiplier);
                double elementLeft = (ContentMargin * marginMultiplier);
                double elementWidth = width - ((ContentMargin * marginMultiplier) * 2f);
                double elementHeight = (savePosition.Y - (ContentMargin * 0.5f)) - elementTop;
                signaturePosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);
                SignatureView.LayoutUpdate(signaturePosition);
            }
        }

        /// <summary>
        /// Mesure the content.
        /// </summary>
        /// <param name="widthConstraint">Width to constarint.</param>
        /// <param name="heightConstraint">Height to constraint.</param>
        /// <returns></returns>
        protected virtual SizeRequest ContentDetailLayout_OnSizeRequest(double widthConstraint, double heightConstraint)
        {
            SizeRequest resultRequest = new SizeRequest(new Size(widthConstraint, heightConstraint), new Size(widthConstraint, heightConstraint));
            return resultRequest;
        }

        /// <summary>
        /// Layout the children.
        /// </summary>
        /// <param name="x">Top to use.</param>
        /// <param name="y">Left to use.</param>
        /// <param name="width">Width to use.</param>
        /// <param name="height">Height to use.</param>
        protected override void ContentLayout_OnLayoutChildren(double x, double y, double width, double height)
        {
            Rectangle contentPosition = new Rectangle();

            if (BackgroundWaterMark != null)
            {
                BackgroundWaterMark.LayoutUpdate(new Rectangle(x, y, width, height));
            }

            if (IsOverlay)
            {
                LastSizePageCanvas = new Rectangle(x, y + TopLayoutMargin, width, height - BottomAppBarMargin - TopLayoutMargin);
            }
            else
            {
                LastSizePageCanvas = new Rectangle(x, y, width, height);
            }

            if (ContentDetailLayout != null)
            {
                double elementTop = (TopLayoutMargin * 0.6);
                double elementWidth = width - (ContentMargin * 6f);
                double elementHeight = height - BottomAppBarMargin - (ContentMargin * 1f) - elementTop;
                double elementLeft = (width - elementWidth) * 0.5f;
                contentPosition = new Rectangle(elementLeft, elementTop, elementWidth, elementHeight);
                ContentDetailLayout.LayoutUpdate(contentPosition);
            }

            if (CloseButton != null)
            {
                double elementHeight = Theme.CommonResources.RoundedButtonWidth;
                double elementLeft = contentPosition.X + contentPosition.Width - (elementHeight * 0.6);
                double elementTop = contentPosition.Y - (elementHeight * 0.4f);

                CloseButton.LayoutUpdate(new Rectangle(elementLeft, elementTop, elementHeight, elementHeight));
            }
        }

        /// <summary>
        /// Initialize elements.
        /// </summary>
        protected override void InternalInitializeView()
        {
            // Layout.
            ContentDetailLayout = new SimpleLayout()
            {
                IsHandlingLayoutManually = true,
                Padding = 0,
                IsClippedToBounds = true
            };

            ContentDetailLayout.OnLayoutChildren += ContentDetailLayout_OnLayoutChildren;
            ContentDetailLayout.ManualSizeCalculationDelegate = ContentDetailLayout_OnSizeRequest;

            if (IsOverlay && HasControlBar)
            {
                // Substract button.
                CloseButton = new GlyphOnlyContentViewButton(hasBackground: true)
                {
                    Style = Theme.ApplicationStyles.GlyphOnlyRoundedContentButtonStyle,
                    GlyphText = Theme.CommonResources.GlyphTextCancel,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Opacity = 0,
                    IsVisible = false
                };
            }

            if (IsOverlay)
            {
                if (HasControlBar)
                {
                    BackgroundWaterMark = Theme.RenderUtil.InstanceBackgroundDetail(null);
                }
                else
                {
                    BackgroundWaterMark = Theme.RenderUtil.InstanceBackgroundDetail(() => { if (this.CloseOverlayCommand != null) { this.CloseOverlayCommand.ExecuteIfCan(); } });
                }
            }

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

            SignatureView.PropertyChanged += SignatureView_PropertyChanged;

            BottomShape = new ShapeView()
            {
                Color = Theme.CommonResources.PagesBackgroundColorLight,
                ShapeType = ShapeType.Box,
                StrokeColor = Theme.CommonResources.PagesBackgroundColorLight,
                StrokeWidth = 2,
                CornerRadius = 6
            };

            // Count button.
            ButtonClear = new GlyphOnlyContentViewButton(hasBackground: true, useDisableBox: true)
            {
                Style = Theme.ApplicationStyles.GlyphOnlyRoundedContentButtonStyle,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                GlyphText = Theme.CommonResources.GlyphTextClear,
                GlyphFontName = Theme.CommonResources.GlyphFontNameAlternate,
                GlyphFriendlyFontName = Theme.CommonResources.GlyphFriendlyFontNameAlternate,
                Command = SignatureView.ClearCommand
            };

            // Add button.
            ButtonSave = new GlyphOnlyContentViewButton(hasBackground: true, useDisableBox: true)
            {
                Style = Theme.ApplicationStyles.GlyphOnlyRoundedContentButtonStyle,
                GlyphText = Theme.CommonResources.GlyphTextSave,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };
        }

        /// <summary>
        /// Binding context changed.
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            ViewModel = null;

            base.OnBindingContextChanged();
        }

        /// <summary>
        /// Setup bindings.
        /// </summary>
        protected override void SetupBindings()
        {
            if (CloseButton != null)
            {
                CloseButton.Command = CloseOverlayCommand;
            }

            if (ButtonSave != null)
            {
                ButtonSave.Command = SaveCommand;
            }
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
                OnPropertyChanged("IsBlank");
            }
        }
    }
}