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
        /// Application styles.
        /// </summary>
        public static ApplicationStyles ApplicationStyles
        {
            get
            {
                if (applicationStyles == null)
                {
                    SetTheme(new ApplicationStyles(new CommonResources()));
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

                Anuracode.Forms.Controls.Styles.ThemeManager.SetTheme(newStyles);
                applicationStyles = Anuracode.Forms.Controls.Styles.ThemeManager.ApplicationStyles;
            }
        }
    }
}