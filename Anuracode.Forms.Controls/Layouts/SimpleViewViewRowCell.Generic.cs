// <copyright file="SimpleViewViewRowCell.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

namespace Anuracode.Forms.Controls
{
    /// <summary>
    /// Generic for making a fastgrid cell from a simple view.
    /// </summary>
    /// <typeparam name="TView">Type to use.</typeparam>
    public class SimpleViewViewRowCell<TView> : SimpleViewViewCell<TView>
        where TView : SimpleRowBaseView, new()
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="renderType">1 if it has delete, other value must be overrided.</param>
        public SimpleViewViewRowCell(int renderType = 0)
            : base()
        {
        }

        /// <summary>
        /// Extra padding used in iOS grouped lists.
        /// </summary>
        public double RightExtraPadding
        {
            get
            {
                return InnerView == null ? 0 : InnerView.RightExtraPadding;
            }

            set
            {
                if (InnerView != null)
                {
                    InnerView.RightExtraPadding = value;
                }
            }
        }
    }
}