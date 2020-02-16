using System;
using System.Net.Http;
using System.Threading.Tasks;
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
            if (previousSettings.EnableNotifications != settings.EnableNotifications)
            {
                if (settings.EnableNotifications)
                    _ = RegisterDevice();
                else
                    _ = UnregisterDevice();
            }

            Preferences.Set(nameof(Settings.DueDate), settings.DueDate);
            Preferences.Set(nameof(Settings.BabyName), settings.BabyName);
            Preferences.Set(nameof(Settings.EnableNotifications), settings.EnableNotifications);
        }

        private async Task RegisterDevice()
        {
            using var client = new HttpClient();
            var body = new JObject();
            body["deviceId"] = GetDeviceId();
            var httpContent = new StringContent(body.ToString());
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await client.PostAsync(Constants.RegisterDeviceUri, httpContent).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
        }

        private async Task UnregisterDevice()
        {
            using var client = new HttpClient();
            var body = new JObject();
            body["deviceId"] = GetDeviceId();
            var httpContent = new StringContent(body.ToString());
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await client.PostAsync(Constants.UnregisterDeviceUri, httpContent).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
        }

        public static string GetDeviceId()
        {
            if (!Preferences.ContainsKey(deviceIdKey))
                Preferences.Set(deviceIdKey, Guid.NewGuid().ToString());

            return Preferences.Get(deviceIdKey, null) ?? throw new ArgumentException(nameof(deviceIdKey));
        }
    }
}
