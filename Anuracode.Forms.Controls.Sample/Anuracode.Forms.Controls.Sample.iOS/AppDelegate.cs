using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using System.Reflection;

namespace Anuracode.Forms.Controls.Sample.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            RegisterRenderers();

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
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
            var property = type.GetProperty("Registered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            registrarInstance = property.GetValue(null);
            registerMethod = property.PropertyType.GetMethod("Register", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            // This renderers are not using the theme colors.            
            registerMethod.Invoke(registrarInstance, new object[] { typeof(Anuracode.Forms.Controls.ExtendedLabel), typeof(Anuracode.Forms.Controls.Renderers.ExtendedLabelRenderer) });
            registerMethod.Invoke(registrarInstance, new object[] { typeof(Anuracode.Forms.Controls.ShapeView), typeof(Anuracode.Forms.Controls.Renderers.ShapeRenderer) });
        }
    }
}
