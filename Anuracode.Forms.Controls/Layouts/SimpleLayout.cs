// <copyright file="SimpleLayout.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using System;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls
{
    /// <summary>
    /// Delegate for the children layout.
    /// </summary>
    /// <param name="x">Top to use.</param>
    /// <param name="y">Left to use.</param>
    /// <param name="width">Width to use.</param>
    /// <param name="height">Height to use.</param>
    public delegate void LayoutChildrenDelegate(double x, double y, double width, double height);

    /// <summary>
    /// Simple layout, more control.
    /// </summary>
    public class SimpleLayout : AbsoluteLayout
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public SimpleLayout()
        {
            Padding = 0;
        }

        /// <summary>
        /// Event to use.
        /// </summary>
        public event LayoutChildrenDelegate OnLayoutChildren;

        /// <summary>
        /// Flag to use manual layout.
        /// </summary>
        public bool IsHandlingLayoutManually { get; set; }

        /// <summary>
        /// Manual size calculation.
        /// </summary>
        public Func<double, double, SizeRequest> ManualSizeCalculationDelegate { get; set; }        

        /// <summary>
        /// Layout the children.
        /// </summary>
        /// <param name="x">Top to use.</param>
        /// <param name="y">Left to use.</param>
        /// <param name="width">Width to use.</param>
        /// <param name="height">Height to use.</param>
        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            if (!IsHandlingLayoutManually)
            {
                base.LayoutChildren(x, y, width, height);
            }

            if (OnLayoutChildren != null)
            {
                OnLayoutChildren(x, y, width, height);
            }
        }

        /// <summary>
        /// Mesure the content.
        /// </summary>
        /// <param name="widthConstraint">Width to constarint.</param>
        /// <param name="heightConstraint">Height to constraint.</param>
        /// <returns>Size to use.</returns>
        protected override SizeRequest OnSizeRequest(double widthConstraint, double heightConstraint)
        {
            if (ManualSizeCalculationDelegate == null)
            {
                return base.OnSizeRequest(widthConstraint, heightConstraint);
            }
            else
            {
                return ManualSizeCalculationDelegate(widthConstraint, heightConstraint);
            }
        }
    }
}