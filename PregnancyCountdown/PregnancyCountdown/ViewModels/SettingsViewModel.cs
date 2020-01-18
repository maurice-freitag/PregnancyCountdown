using System;
using System.Windows.Input;
using PregnancyCountdown.Models;
using PregnancyCountdown.Services;
using Xamarin.Forms;

namespace PregnancyCountdown.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private Settings settings;

        public Settings Settings
        {
            get => settings;
            set => SetProperty(ref settings, value);
        }

        public DateTime MinimumDate { get; } = DateTime.Now.AddDays(1);

        public ICommand SaveCommand { get; }

        public SettingsViewModel()
        {
            Title = "Settings";
            SaveCommand = new Command(OnSaveCommand);

            var settingsProvider = DependencyService.Get<ISettingsProvider>();
            Settings = settingsProvider.GetSettings();
        }

        private void OnSaveCommand(object obj)
        {
            var settingsProvider = DependencyService.Get<ISettingsProvider>();
            settingsProvider.SetSettings(settings);
        }
    }
}