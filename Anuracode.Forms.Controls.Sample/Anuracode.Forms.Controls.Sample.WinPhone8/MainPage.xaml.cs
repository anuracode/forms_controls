using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Anuracode.Forms.Controls.Sample.WinPhone8.Resources;
using System.Reflection;

namespace Anuracode.Forms.Controls.Sample.WinPhone8
{
    public partial class MainPage : global::Xamarin.Forms.Platform.WinPhone.FormsApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            SupportedOrientations = SupportedPageOrientation.PortraitOrLandscape;
            Xamarin.Forms.Forms.Init();
            // global::Xamarin.FormsMaps.Init();                        

            RegisterRenderers();

            LoadApplication(new Anuracode.Forms.Controls.Sample.App());
        }

        /// <summary>
        /// Register renderers.
        /// </summary>
        protected void RegisterRenderers()
        {
            string registrarType = "Xamarin.Forms.Registrar, Xamarin.Forms.Core";
            object registrarInstance;
            MethodInfo registerMethod;

            var type = Type.GetType(registrarType, true);
            var property = type.GetProperty("Registered", BindingFlags.Static | BindingFlags.NonPublic);
            registrarInstance = property.GetValue(null);
            registerMethod = property.PropertyType.GetMethod("Register", BindingFlags.Instance | BindingFlags.Public);

            if (registerMethod != null)
            {
                // This renderers are not using the theme colors.                
                registerMethod.Invoke(registrarInstance, new object[] { typeof(Anuracode.Forms.Controls.ExtendedEntry), typeof(Anuracode.Forms.Controls.Renderers.ExtendedEntryRenderer) });                
            }
        }
    }
}