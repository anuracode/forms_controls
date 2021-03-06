﻿// <copyright file="SimpleViewBase.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Extensions;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls
{
    /// <summary>
    /// View for the top items.
    /// </summary>
    public abstract class SimpleViewBase : ContentView
    {
        /// <summary>
        /// Margin for the element.s
        /// </summary>
        private double contentMargin = 10;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SimpleViewBase()
            : this(true)
        {
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SimpleViewBase(bool autoInit)
        {
            ContentLayout = new SimpleLayout()
            {
                IsHandlingLayoutManually = true,
                Padding = 0
            };

            ContentLayout.OnLayoutChildren += InternalContentLayout_OnLayoutChildren;
            ContentLayout.ManualSizeCalculationDelegate = InternalContentLayout_OnSizeRequest;

            if (autoInit)
            {
                InitializeView();
            }

            Content = ContentLayout;
        }

        /// <summary>
        /// Layout of the view.
        /// </summary>
        public SimpleLayout ContentLayout { get; set; }

        /// <summary>
        /// Margin for the element.s
        /// </summary>
        public virtual double ContentMargin
        {
            get
            {
                return contentMargin;
            }

            set
            {
                contentMargin = value;
            }
        }

        /// <summary>
        /// The binding is already set.
        /// </summary>
        protected bool IsBindingSet { get; set; }

        /// <summary>
        /// Flag to check if the view has been initialized.
        /// </summary>
        protected bool IsInitialized { get; set; }

        /// <summary>
        /// Initialize the control of the view.
        /// </summary>
        public void InitializeView()
        {
            try
            {
                lock (this)
                {
                    if (!IsInitialized && (ContentLayout != null))
                    {
                        IsInitialized = true;
                        InternalInitializeView();
                        AddControlsToLayout();
                    }
                }
            }
            catch (System.Exception ex)
            {
                AC.TraceError("Simple view initialize problem", ex);
            }
        }

        /// <summary>
        /// Prepare binding context.
        /// </summary>
        public void PrepareBindings()
        {
            lock (this)
            {
                if (!IsBindingSet)
                {
                    IsBindingSet = true;
                    SetupBindings();
                }
            }
        }

        /// <summary>
        /// This is called when the view is going to be recycled.
        /// </summary>
        public virtual void RecycleView()
        {
        }

        /// <summary>
        /// Set up cell values.
        /// </summary>
        /// <param name="isRecycled">Is recycled.</param>
        public virtual void SetupViewValues(bool isRecycled)
        {
        }

        /// <summary>
        /// Add the control to the layout in the proper order.
        /// </summary>
        protected abstract void AddControlsToLayout();

        /// <summary>
        /// Validate view before add.
        /// </summary>
        /// <param name="view">View to use.</param>
        protected void AddViewToLayout(View view)
        {
            AddViewToLayout(view, ContentLayout);
        }

        /// <summary>
        /// Validate view before add.
        /// </summary>
        /// <param name="view">View to use.</param>
        protected void AddViewToLayout(View view, SimpleLayout container)
        {
            if ((view != null) && (container != null))
            {
                container.Children.Add(view);
            }
        }

        /// <summary>
        /// Layout children.
        /// </summary>
        /// <param name="x">Top to use.</param>
        /// <param name="y">Left to use.</param>
        /// <param name="width">Width to use.</param>
        /// <param name="height">Height to use.</param>
        protected abstract void ContentLayout_OnLayoutChildren(double x, double y, double width, double height);

        /// <summary>
        /// Mesure the content.
        /// </summary>
        /// <param name="widthConstraint">Width to constarint.</param>
        /// <param name="heightConstraint">Height to constraint.</param>
        /// <returns>Size to use.</returns>
        protected virtual SizeRequest ContentLayout_OnSizeRequest(double widthConstraint, double heightConstraint)
        {
            SizeRequest resultRequest = new SizeRequest(new Size(widthConstraint, heightConstraint), new Size(widthConstraint, heightConstraint));

            return resultRequest;
        }

        /// <summary>
        /// Internal initialze view.
        /// </summary>
        protected abstract void InternalInitializeView();

        /// <summary>
        /// Get size request.
        /// </summary>
        /// <param name="widthConstraint">Width constraint.</param>
        /// <param name="heightConstraint">Height constraint.</param>
        /// <returns>Size to use.</returns>
        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            if (ContentLayout == null)
            {
                return new SizeRequest(new Size(), new Size());
            }
            else
            {
                var resultSize = ContentLayout.Measure(widthConstraint, heightConstraint);

                double requestWidth = resultSize.Request.Width.Clamp(0, widthConstraint);
                double requestHeight = resultSize.Request.Height.Clamp(0, heightConstraint);
                double minimumWidth = resultSize.Minimum.Width.Clamp(0, widthConstraint);
                double minimumHeight = resultSize.Minimum.Height.Clamp(0, widthConstraint);

                return new SizeRequest(new Size(requestWidth, requestHeight), new Size(minimumWidth, minimumHeight));
            }
        }

        /// <summary>
        /// Setup the bindings of the elements.
        /// </summary>
        protected abstract void SetupBindings();

        /// <summary>
        /// Layout children.
        /// </summary>
        /// <param name="x">Top to use.</param>
        /// <param name="y">Left to use.</param>
        /// <param name="width">Width to use.</param>
        /// <param name="height">Height to use.</param>
        private void InternalContentLayout_OnLayoutChildren(double x, double y, double width, double height)
        {
            try
            {
                ContentLayout_OnLayoutChildren(x, y, width, height);
            }
            catch (System.Exception ex)
            {
                AC.TraceError("OnSize error", ex);
            }
        }

        /// <summary>
        /// Mesure the content.
        /// </summary>
        /// <param name="widthConstraint">Width to constarint.</param>
        /// <param name="heightConstraint">Height to constraint.</param>
        /// <returns>Size to use.</returns>
        private SizeRequest InternalContentLayout_OnSizeRequest(double widthConstraint, double heightConstraint)
        {
            SizeRequest returnValue = new SizeRequest();
            try
            {
                returnValue = ContentLayout_OnSizeRequest(widthConstraint, heightConstraint);
            }
            catch (System.Exception ex)
            {
                AC.TraceError("OnSize error", ex);
            }

            return returnValue;
        }
    }
}