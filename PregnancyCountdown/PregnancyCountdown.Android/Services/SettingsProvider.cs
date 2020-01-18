using System;
using PregnancyCountdown.Models;
using PregnancyCountdown.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PregnancyCountdown.Droid
{
    public class SettingsProvider : ISettingsProvider
    {
        public Settings GetSettings()
        {
            return new Settings
            {
                DueDate = Preferences.Get(nameof(Settings.DueDate), default(DateTime)),
                BabyName = Preferences.Get(nameof(Settings.BabyName), string.Empty),
                EnableNotifications = Preferences.Get(nameof(Settings.EnableNotifications), false)
            };
        }

        public void SetSettings(Settings settings)
        {
            Preferences.Set(nameof(Settings.DueDate), settings.DueDate);
            Preferences.Set(nameof(Settings.BabyName), settings.BabyName);
            Preferences.Set(nameof(Settings.EnableNotifications), settings.EnableNotifications);

            var notificationHandler = DependencyService.Get<INotificationHandler>();
            notificationHandler.UpdateNotificationPreferences(settings.EnableNotifications);
        }
    }
}
