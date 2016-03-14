using System;
using System.Linq;
using System.Reflection;

namespace Anuracode.Forms.Controls.Sample.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            RegisterRenderers();

            LoadApplication(new Anuracode.Forms.Controls.Sample.App());
        }

        /// <summary>
        /// Register renderers.
        /// </summary>
        private void RegisterRenderers()
        {
            string registrarType = "Xamarin.Forms.Registrar, Xamarin.Forms.Core";
            object registrarInstance;
            MethodInfo registerMethod;

            var type = Type.GetType(registrarType, true);

            string propertyName = "Registered";

            var property = (from propertySelect in type.GetRuntimeProperties()
                            where propertySelect.Name == propertyName
                            select propertySelect).FirstOrDefault();

            if (property != null)
            {
                registrarInstance = property.GetValue(null);

                string methodName = "Register";

                registerMethod = (from methodSelect in property.PropertyType.GetRuntimeMethods()
                                  where methodSelect.Name == methodName
                                  select methodSelect).FirstOrDefault();

                if (registerMethod != null)
                {
                    // This renderers are not using the theme colors.
                    registerMethod.Invoke(registrarInstance, new object[] { typeof(ExtendedLabel), typeof(Renderers.ExtendedLabelRenderer) });
                    registerMethod.Invoke(registrarInstance, new object[] { typeof(ExtendedEntry), typeof(Renderers.ExtendedEntryRenderer) });
                    registerMethod.Invoke(registrarInstance, new object[] { typeof(ExtendedImage), typeof(Renderers.ManagedImageRenderer) });
                    registerMethod.Invoke(registrarInstance, new object[] { typeof(ShapeView), typeof(Renderers.ShapeRenderer) });
                    registerMethod.Invoke(registrarInstance, new object[] { typeof(SignaturePadView), typeof(Renderers.SignaturePadViewRenderer) });
                }
            }
        }
    }
}