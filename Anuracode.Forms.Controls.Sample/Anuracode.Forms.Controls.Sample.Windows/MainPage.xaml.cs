namespace Anuracode.Forms.Controls.Sample.Windows
{
    public sealed partial class MainPage: Xamarin.Forms.Platform.WinRT.WindowsPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new Anuracode.Forms.Controls.Sample.App());
        }
    }
}