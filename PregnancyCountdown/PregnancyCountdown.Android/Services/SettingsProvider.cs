using System;
using System.Threading.Tasks;
using Microsoft.Azure.NotificationHubs;
using Newtonsoft.Json.Linq;
using PregnancyCountdown.Models;
using PregnancyCountdown.Services;
using Xamarin.Essentials;

namespace PregnancyCountdown.Droid
{
    public class SettingsProvider : ISettingsProvider
    {
        private static readonly string deviceIdKey = "PregnancyCountdown_DeviceId";

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
            var previousSettings = GetSettings();
            if (!previousSettings.EnableNotifications && settings.EnableNotifications)
                _ = ScheduleWelcomeNotificationAsync();

            Preferences.Set(nameof(Settings.DueDate), settings.DueDate);
            Preferences.Set(nameof(Settings.BabyName), settings.BabyName);
            Preferences.Set(nameof(Settings.EnableNotifications), settings.EnableNotifications);
        }

        private async Task ScheduleWelcomeNotificationAsync()
        {
            var deviceIdTag = $"deviceId:{GetDeviceId()}";
            var hub = NotificationHubClient.CreateClientFromConnectionString(Constants.FullAccessConnectionString, Constants.NotificationHubName);
            await hub.SendFcmNativeNotificationAsync(BuildJsonPayload(MessageType.WelcomeMessage), deviceIdTag);
            var notification = new FcmNotification(BuildJsonPayload(MessageType.DailyMessage), deviceIdTag);
            await hub.ScheduleNotificationAsync(notification, DateTimeOffset.Now.AddMinutes(8), deviceIdTag);
        }

        private string BuildJsonPayload(MessageType messageType)
        {
            var outer = new JObject();
            var inner = new JObject();
            inner[nameof(MessageType)] = (int)messageType;
            outer["data"] = inner;
            return outer.ToString();
        }

        public static string GetDeviceId()
        {
            if (!Preferences.ContainsKey(deviceIdKey))
                Preferences.Set(deviceIdKey, Guid.NewGuid().ToString());

            return Preferences.Get(deviceIdKey, null) ?? throw new ArgumentException(nameof(deviceIdKey));
        }
    }
}
