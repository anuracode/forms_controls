// <copyright file="ThemeManager.cs" company="Anura Code">
// All rights reserved.
// </copyright>
// <author>Alberto Puyana</author>

using Xamarin.Forms;

namespace Anuracode.Forms.Controls.Styles
{
    /// <summary>
    /// Theme for the app.
    /// </summary>
    public static class ThemeManager
    {
        /// <summary>
        /// Application styles.
        /// </summary>
        private static ApplicationStyles applicationStyles;

        /// <summary>
        /// Application styles.
        /// </summary>
        public static ApplicationStyles ApplicationStyles
        {
            get
            {
                if (applicationStyles == null)
                {
                    SetTheme(new ApplicationStyles(new CommonResourcesBase()));
                }

                return applicationStyles;
            }
        }

        /// <summary>
        /// Common resources to use.
        /// </summary>
        public static CommonResourcesBase CommonResourcesBase
        {
            get
            {
                return ApplicationStyles.CommonResources;
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
                applicationStyles = newStyles;
            }
        }
    }
}