using System.ComponentModel;
using PregnancyCountdown.ViewModels;
using Xamarin.Forms;

namespace PregnancyCountdown.Views
{
    [DesignTimeVisible(false)]
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            BindingContext = new AboutViewModel();
        }
    }
}