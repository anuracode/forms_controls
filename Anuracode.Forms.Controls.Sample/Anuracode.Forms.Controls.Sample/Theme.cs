// <copyright file="Theme.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Anuracode.Forms.Controls.Sample.Views.Common;
using Anuracode.Forms.Controls.Styles;

namespace Anuracode.Forms.Controls.Sample
{
    /// <summary>
    /// Theme for the app.
    /// </summary>
    public static class Theme
    {
        /// <summary>
        /// Application styles.
        /// </summary>
        private static ApplicationStyles applicationStyles;

        /// <summary>
        /// Common resources.
        /// </summary>
        private static CommonResources commonResources;

        /// <summary>
        /// Render util to use.
        /// </summary>
        private static RenderUtilBase renderUtil;

        /// <summary>
        /// Application styles.
        /// </summary>
        public static ApplicationStyles ApplicationStyles
        {
            get
            {
                if (applicationStyles == null)
                {
                    SetTheme(new ApplicationStyles(new CommonResources(), new RenderUtilBase()));
                }

                return applicationStyles;
            }
        }

        /// <summary>
        /// Common resources to use.
        /// </summary>
        public static CommonResources CommonResources
        {
            get
            {
                // Force the instance of the resources.
                return (ApplicationStyles == null) ? null : commonResources;
            }
        }

        /// <summary>
        /// Common resources to use.
        /// </summary>
        public static RenderUtilBase RenderUtil
        {
            get
            {
                // Force the instance of the resources.
                return (ApplicationStyles == null) ? null : renderUtil;
            }
        }

        /// <summary>
        /// Set theme.
        /// </summary>
        /// <param name="themeId">Id of the theme.</param>
        public static void SetTheme(ApplicationStyles newStyles)
        {
            if (newStyles != null)
            {
                commonResources = newStyles.CommonResources as CommonResources;
                if (commonResources == null)
                {
                    throw new System.ArgumentOutOfRangeException("newStyles", "The resources should be of type CommonResources");
                }

                renderUtil = newStyles.RenderUtilBase;

                if (renderUtil == null)
                {
                    throw new System.ArgumentOutOfRangeException("newStyles", "The render util should be definded.");
                }

                Anuracode.Forms.Controls.Styles.ThemeManager.SetTheme(newStyles);
                applicationStyles = Anuracode.Forms.Controls.Styles.ThemeManager.ApplicationStyles;
            }
        }
    }
}