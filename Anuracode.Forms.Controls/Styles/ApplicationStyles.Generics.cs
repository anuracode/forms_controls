// <copyright file="ApplicationStyles.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

namespace Anuracode.Forms.Controls.Styles
{
    /// <summary>
    /// Generic for the application styles.
    /// </summary>
    /// <typeparam name="TCommonResources"></typeparam>
    public class ApplicationStyles<TCommonResources> : ApplicationStylesBase
        where TCommonResources : CommonResourcesBase, new()
    {
        /// <summary>
        /// Common resources.
        /// </summary>
        private TCommonResources commonResources;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="commonResources">Common resources.</param>
        /// <param name="renderUtil">Render util to use.</param>
        public ApplicationStyles(TCommonResources commonResources, RenderUtilBase renderUtil)
            : base(commonResources, renderUtil)
        {
        }

        /// <summary>
        /// Common resources.
        /// </summary>
        public TCommonResources CommonResources
        {
            get
            {
                if (commonResources == null)
                {
                    commonResources = CommonResourcesBase as TCommonResources;
                }

                return commonResources;
            }
        }
    }
}