// <copyright file="ApplicationStylesBase.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

namespace Anuracode.Forms.Controls.Styles
{
    /// <summary>
    /// Base styles for an application.
    /// </summary>
    public class ApplicationStylesBase
    {
        /// <summary>
        /// Common resources to use.
        /// </summary>
        private CommonResourcesBase commonResourcesBase;

        /// <summary>
        /// Utility to render.
        /// </summary>
        private RenderUtilBase renderUtilBase;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="commonResources">Common resources to use.</param>
        /// <param name="renderUtil">Render util to use.</param>
        public ApplicationStylesBase(CommonResourcesBase commonResources, RenderUtilBase renderUtil)
        {
            this.commonResourcesBase = commonResources;
            this.renderUtilBase = renderUtil;
        }

        /// <summary>
        /// Utility to render.
        /// </summary>
        public RenderUtilBase RenderUtilBase
        {
            get
            {
                return renderUtilBase;
            }
        }

        /// <summary>
        /// Common resources.
        /// </summary>
        protected CommonResourcesBase CommonResourcesBase
        {
            get
            {
                return commonResourcesBase;
            }
        }
    }
}