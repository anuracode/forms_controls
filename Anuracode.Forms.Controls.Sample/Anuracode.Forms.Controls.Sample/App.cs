using Anuracode.Forms.Controls.Sample.Localization;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Anuracode.Forms.Controls.Sample
{
    public class App : Application
    {
        /// <summary>
        /// Localization resources for the App.
        /// </summary>
        private static LocalizationResources localizationResources;

        /// <summary>
        /// Static constructor.
        /// </summary>
        public App()
        {
            MainPage = new Views.MainPage();
        }

        /// <summary>
        /// Action when the page initialization is complete.
        /// </summary>
        public static Action InitPageCompleteAction { get; set; }

        /// <summary>
        /// Localization resources for the App.
        /// </summary>
        public static LocalizationResources LocalizationResources
        {
            get
            {
                if (localizationResources == null)
                {
                    localizationResources = new LocalizationResources();
                }

                return localizationResources;
            }
        }

        /// <summary>
        /// True when the pages have been initilezed.
        /// </summary>
        public static bool PagesInitilized { get; set; }

        /// <summary>
        /// Show main menu.
        /// </summary>
        public static ICommand ShowMainMenuCommand { get; set; }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }
    }
}