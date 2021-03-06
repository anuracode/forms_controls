﻿using Android.App;
using Android.Content.PM;
using Android.OS;
using Anuracode.Forms.Controls.Sample;

namespace SampleAndroid
{
    [Activity(Label = "Anuracode.Forms.Controls.Sample", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity  // global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            Xamarin.Essentials.Platform.Init(this, bundle); // add this line to your code, it may also be called: bundle

            Anuracode.Forms.Controls.Renderers.ExtendedImageRenderer.AllowDownSample = true;
            FFImageLoading.ImageService.Instance.Initialize(
                new FFImageLoading.Config.Configuration()
                {
                    VerboseLoadingCancelledLogging = false,
                    VerboseLogging = false,
                    VerboseMemoryCacheLogging = false,
                    VerbosePerformanceLogging = false,
                    MaxMemoryCacheSize = 100000 * 5,
                    HttpClient = new System.Net.Http.HttpClient(new Xamarin.Android.Net.AndroidClientHandler())
                });

            // set the layout resources first
            global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity.ToolbarResource = Resource.Layout.toolbar;
            global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity.TabLayoutResource = Resource.Layout.tabs;
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            global::Xamarin.Forms.Forms.SetTitleBarVisibility(global::Xamarin.Forms.AndroidTitleBarVisibility.Never);
            LoadApplication(new App());
        }
    }
}