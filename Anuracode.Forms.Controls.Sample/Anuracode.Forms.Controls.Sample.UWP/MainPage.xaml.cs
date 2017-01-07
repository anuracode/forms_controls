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

            LoadApplication(new Anuracode.Forms.Controls.Sample.App());
        }        
    }
}