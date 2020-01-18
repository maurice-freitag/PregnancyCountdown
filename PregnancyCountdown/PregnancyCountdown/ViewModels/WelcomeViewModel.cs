using PregnancyCountdown.Models;
using PregnancyCountdown.Services;
using Xamarin.Forms;

namespace PregnancyCountdown.ViewModels
{
    public class WelcomeViewModel : BaseViewModel
    {
        private Settings settings;
        private string prefix;

        public Settings Settings
        {
            get => settings;
            set => SetProperty(ref settings, value);
        }

        public string Prefix
        {
            get => prefix;
            set => SetProperty(ref prefix, value);
        }

        public int DaysLeft { get; set; }

        public WelcomeViewModel()
        {
            Title = "Pregnancy Countdown";
            Refresh();
        }

        public void Refresh()
        {
            var settingsProvider = DependencyService.Get<ISettingsProvider>();
            Settings = settingsProvider.GetSettings();
            Prefix = GetPrefix();
        }

        private string GetPrefix()
        {
            if (string.IsNullOrEmpty(Settings.BabyName))
                return "Your baby is just";
            else return $"{Settings.BabyName} is just";
        }
    }
}
