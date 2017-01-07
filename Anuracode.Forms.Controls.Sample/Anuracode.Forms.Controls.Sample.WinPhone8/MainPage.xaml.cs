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

            LoadApplication(new Anuracode.Forms.Controls.Sample.App());
        }        
    }
}