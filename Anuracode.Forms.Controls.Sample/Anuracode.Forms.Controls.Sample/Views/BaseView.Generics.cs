// <copyright file="BaseView.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System.Threading;

namespace Anuracode.Forms.Controls.Sample.Views
{
    /// <summary>
    /// Base view.
    /// </summary>
    public class BaseView<TViewModel> : BaseView
        where TViewModel : class
    {
        /// <summary>
        /// Lock for the animtaion.
        /// </summary>
        private SemaphoreSlim lockAnimation;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="viewModel">View model to use.</param>
        public BaseView(TViewModel viewModel)
            : base()
        {
            ViewModel = viewModel;

            if (ViewModel != null)
            {
                this.BindingContext = ViewModel;
            }

            this.Title = string.Empty;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public BaseView()
            : this(null)
        {
        }

        /// <summary>
        /// Animation token source.
        /// </summary>
        protected CancellationTokenSource AnimationTokenSource { get; set; }

        /// <summary>
        /// Lock for the animtaion.
        /// </summary>
        protected SemaphoreSlim LockAnimation
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
        /// View model from the AppNavigation.
        /// </summary>
        protected TViewModel ViewModel { get; private set; }
    }
}