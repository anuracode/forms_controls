// <copyright file="CartSmallView.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Views.Extensions;
using Anuracode.Forms.Controls.Sample.ViewModels;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls.Sample.Views
{
    /// <summary>
    /// Cart small view.
    /// </summary>
    public class CartSmallView : ContentView
    {
        /// <summary>
        /// Lock for the animtaion.
        /// </summary>
        private SemaphoreSlim lockAnimation;

        /// <summary>
        /// Navigate to cart.
        /// </summary>
        private ICommand showCartCommand;

        /// <summary>
        /// Last items count.
        /// </summary>
        private int totalItemCountCache = 0;

        /// <summary>
        /// Viewmodel for the view.
        /// </summary>
        private CartListViewModel viewModel;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="showCartCommand">Command for showing the cart.</param>
        public CartSmallView(ICommand showCartCommand)
        {
            this.UpdateOpacity(0);
            this.showCartCommand = showCartCommand;

            BindingContext = ViewModel;
            Padding = new Thickness(3);
            HorizontalOptions = LayoutOptions.End;
            VerticalOptions = LayoutOptions.End;

            Content = RenderContent();
        }

        /// <summary>
        /// Lock for the animtaion.
        /// </summary>
        public SemaphoreSlim LockAnimation
        {
            get
            {
                if (lockAnimation == null)
                {
                    lockAnimation = new SemaphoreSlim(1);
                }

                return lockAnimation;
            }
        }

        /// <summary>
        /// Navigate to cart.
        /// </summary>
        public ICommand ShowCartCommand
        {
            get
            {
                return showCartCommand;
            }
        }

        /// <summary>
        /// Viewmodel for the view.
        /// </summary>
        public CartListViewModel ViewModel
        {
            get
            {
                if (viewModel == null)
                {
                    viewModel = new CartListViewModel();
                }

                return viewModel;
            }
        }

        /// <summary>
        /// Loading items token source.
        /// </summary>
        protected CancellationTokenSource AnimationTokenSource { get; set; }

        /// <summary>
        /// Button for the cart.
        /// </summary>
        protected ContentViewButton CartButton { get; set; }

        /// <summary>
        /// Content of the page.
        /// </summary>
        protected View RenderContent()
        {
            CartButton = new GlyphLeftContentViewButton()
            {
                Style = Theme.ApplicationStyles.GlyphOnlyNavbarButtonStyle,
                MarginBorders = 2,
                MarginElements = 0,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start,
                WidthRequest = -1,
                Command = ShowCartCommand,
                GlyphText = Theme.CommonResources.GlyphTextCart,
                GlyphFriendlyFontName = Theme.CommonResources.GlyphFriendlyFontNameAlternate,
                GlyphFontName = Theme.CommonResources.GlyphFontNameAlternate
            };

            CartButton.Text = ViewModel.TotalItemsCart.ToString("N0");

            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            AC.ScheduleManaged(
                TimeSpan.FromSeconds(0.15),
                async () =>
                {
                    try
                    {
                        await LockAnimation.WaitAsync();

                        if (this.Content != null)
                        {
                            await this.FadeTo(1);
                        }
                    }
                    catch
                    {
                    }
                    finally
                    {
                        LockAnimation.Release();
                    }
                });

            return CartButton;
        }

        /// <summary>
        /// Animate count changed.
        /// </summary>
        private void AnimateCountChanged()
        {
            if (AnimationTokenSource != null && !AnimationTokenSource.IsCancellationRequested)
            {
                AnimationTokenSource.Cancel();
            }

            AnimationTokenSource = new CancellationTokenSource();

            if (ViewModel.TotalItemsCart != totalItemCountCache)
            {
                AC.ScheduleManaged(
                    async () =>
                    {
                        CancellationToken animationToken = AnimationTokenSource.Token;
                        try
                        {
                            await LockAnimation.WaitAsync(animationToken);

                            if (this.Content != null)
                            {
                                if (CartButton != null)
                                {
                                    CartButton.Text = ViewModel.TotalItemsCart.ToString("N0");
                                }

                                await this.ScaleTo(1.5, 100, Easing.SinIn);
                                animationToken.ThrowIfCancellationRequested();

                                await this.ScaleTo(1.00, 100, Easing.SinOut);

                                animationToken.ThrowIfCancellationRequested();
                                await this.ScaleTo(1.4, 100, Easing.SinIn);

                                animationToken.ThrowIfCancellationRequested();
                                await this.ScaleTo(1.00, 250, Easing.SinOut);
                            }
                        }
                        catch
                        {
                        }
                        finally
                        {
                            this.Scale = 1;
                            totalItemCountCache = ViewModel.TotalItemsCart;
                            LockAnimation.Release();
                        }
                    });
            }
        }

        /// <summary>
        /// Event when a property changed.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Arguments of the event.</param>
        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TotalItemsCart")
            {
                AnimateCountChanged();

                AC.ScheduleManaged(
                    async () =>
                    {
                        if ((Content != null) && (this.Opacity < 1))
                        {
                            try
                            {
                                await LockAnimation.WaitAsync();

                                await Task.Delay(TimeSpan.FromSeconds(0.25f));
                                await this.FadeTo(1, 250, Easing.SinIn);
                            }
                            catch
                            {
                            }
                            finally
                            {
                                LockAnimation.Release();
                            }
                        }
                    });
            }
        }
    }
}