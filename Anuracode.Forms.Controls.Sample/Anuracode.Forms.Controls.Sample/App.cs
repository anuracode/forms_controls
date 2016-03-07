using Anuracode.Forms.Controls.Sample.Localization;
using Anuracode.Forms.Controls.Sample.Repository;
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
        /// Repository for the address.
        /// </summary>
        private static RepositoryAddress repositoryAddress;

        /// <summary>
        /// Repository cart.
        /// </summary>
        private static RepositoryCart repositoryCart;

        /// <summary>
        /// Repository for store items.
        /// </summary>
        private static RepositoryStoreItem repositoryStoreItem;

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
        /// Repository for the address.
        /// </summary>
        public static RepositoryAddress RepositoryAddress
        {
            get
            {
                if (repositoryAddress == null)
                {
                    repositoryAddress = new RepositoryAddress();
                }

                return repositoryAddress;
            }
        }

        /// <summary>
        /// Repository cart.
        /// </summary>
        public static RepositoryCart RepositoryCart
        {
            get
            {
                if (repositoryCart == null)
                {
                    repositoryCart = new RepositoryCart();
                }

                return repositoryCart;
            }
        }

        /// <summary>
        /// Repository for store items.
        /// </summary>
        public static RepositoryStoreItem RepositoryStoreItem
        {
            get
            {
                if (repositoryStoreItem == null)
                {
                    repositoryStoreItem = new RepositoryStoreItem();
                }

                return repositoryStoreItem;
            }
        }

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