// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Anuracode.Forms.Controls.Sample.Windows
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage: Xamarin.Forms.Platform.WinRT.WindowsPhonePage
    {
        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new Anuracode.Forms.Controls.Sample.App());
        }
    }
}