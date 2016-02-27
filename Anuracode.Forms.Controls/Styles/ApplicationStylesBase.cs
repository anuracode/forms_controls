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
        /// Default constructor.
        /// </summary>
        /// <param name="commonResources">Common resources to use.</param>
        public ApplicationStylesBase(CommonResourcesBase commonResources)
        {
            this.commonResourcesBase = commonResources;
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