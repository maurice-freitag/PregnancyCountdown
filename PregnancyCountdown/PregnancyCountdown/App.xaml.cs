using PregnancyCountdown.Services;
using Xamarin.Forms;

namespace PregnancyCountdown
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            DependencyService.Register<ISettingsProvider, MockSettingsProvider>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            // Configure OnStart
        }

        protected override void OnSleep()
        {
            // Configure OnSleep
        }

        protected override void OnResume()
        {
            // Configure OnResume
        }
    }
}
