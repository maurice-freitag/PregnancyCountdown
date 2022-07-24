using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Support.V4.App;
using Android.Util;
using Firebase.Messaging;
using PregnancyCountdown.Converter;
using PregnancyCountdown.Models;
using PregnancyCountdown.Services;
using WindowsAzure.Messaging;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PregnancyCountdown.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class MessagingService : FirebaseMessagingService
    {
        public override void OnMessageReceived(RemoteMessage p0)
        {
            var notification = p0.GetNotification();

            Log.Debug(nameof(MessagingService), $"Message received from {p0.From}");
            Log.Debug(nameof(MessagingService), $"Notification Message Body: {notification?.Body}");

            if (!p0.Data.TryGetValue(nameof(MessageType), out var raw) || !int.TryParse(raw, out var intValue))
                return;

            switch ((MessageType)intValue)
            {
                case MessageType.WelcomeMessage:
                    SendWelcomeNotification();
                    break;
                case MessageType.DailyMessage:
                    SendDailyNotification();
                    break;
            }
        }

        private void SendWelcomeNotification()
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);

            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);
            var notification = BuildNotification($"You will start receiving notifications like this starting tomorrow!", pendingIntent);
            var notificationManager = NotificationManager.FromContext(this);
            notificationManager.Notify(0, notification);
        }

        private void SendDailyNotification()
        {
            var settings = GetSettings();
            if (settings == null || !settings.EnableNotifications || settings.DueDate.Date <= DateTime.Now.Date)
                return;

            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);

            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);
            var notification = BuildNotification(GetDailyNotificationText(settings), pendingIntent);
            var notificationManager = NotificationManager.FromContext(this);
            notificationManager.Notify(0, notification);
        }

        private Android.App.Notification BuildNotification(string message, PendingIntent intent)
        {
            return new NotificationCompat.Builder(this, MainActivity.CHANNEL_ID)
                .SetContentTitle("Your Pregnancy Countdown")
                .SetSmallIcon(Resource.Drawable.icons8_heart_64)
                .SetLargeIcon(BitmapFactory.DecodeResource(Resources, Resource.Drawable.icons8_heart_64))
                .SetContentText(message)
                .SetAutoCancel(true)
                .SetShowWhen(false)
                .SetContentIntent(intent)
                .Build();
        }

        private string GetDailyNotificationText(Settings settings)
        {
            var days = DaysLeftConverter.Convert(settings.DueDate);
            var actualBabyName = string.IsNullOrEmpty(settings.BabyName) ? "Baby" : settings.BabyName;
            return $"{actualBabyName} will arrive in just {days} days!";
        }

        private Settings GetSettings()
        {
            var settingsProvider = DependencyService.Get<ISettingsProvider>();
            if (settingsProvider != null)
                return settingsProvider.GetSettings();

            return new Settings
            {
                DueDate = Preferences.Get(nameof(Settings.DueDate), default(DateTime)),
                BabyName = Preferences.Get(nameof(Settings.BabyName), string.Empty),
                EnableNotifications = Preferences.Get(nameof(Settings.EnableNotifications), false)
            };
        }

        public override void OnNewToken(string p0)
        {
            Log.Debug(nameof(MessagingService), $"Received new FCM token {p0}");
            SendRegistrationToServer(p0);
        }

        private void SendRegistrationToServer(string token)
        {
            var hub = new NotificationHub(Constants.NotificationHubName, Constants.ListenConnectionString, this);
            var regId = hub.Register(token, $"deviceId:{SettingsProvider.GetDeviceId()}");
            Log.Debug(nameof(MessagingService), $"Successful registration of Id {regId}");
        }
    }
}