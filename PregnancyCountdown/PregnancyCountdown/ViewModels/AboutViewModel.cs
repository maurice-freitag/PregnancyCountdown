using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PregnancyCountdown.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";
            OpenLinkCommand = new Command(async (object uri) => await Browser.OpenAsync(uri.ToString()));
        }

        public ICommand OpenLinkCommand { get; }
    }
}