using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Xamarin.Essentials;

namespace PregnancyCountdown.Droid.Services
{
    [IntentFilter(new[] { "ALARM_TRIGGERED", "net.gedoens.LOCAL_NOTIFICATION", Intent.ActionBootCompleted }, Categories = new[] { Intent.CategoryHome }, Priority = (int)IntentFilterPriority.HighPriority)]
    [BroadcastReceiver(Enabled = true, Name = "net.gedoens.pregnancycountdown.NotificationBroadcastReceiver", Label = "Local Notifications Broadcast Receiver")]
    public class NotificationBroadcastReceiver : BroadcastReceiver
    {
        private static readonly string CHANNEL_ID = "net.gedoens.pregnancycountdown.notifications";
        private static readonly int NOTIFICATION_ID = 1810;

        public override void OnReceive(Context context, Intent intent)
        {
            SetupNotificationChannel(context);
            var notification = BuildNotification(context);
            var notificationManager = NotificationManagerCompat.From(context);
            notificationManager.Notify(NOTIFICATION_ID, notification);
        }

        private Notification BuildNotification(Context context)
        {
            var dueDate = Preferences.Get("DueDate", default(DateTime));
            var babyName = Preferences.Get("BabyName", string.Empty);

            var days = (int)Math.Max(Math.Ceiling((dueDate - DateTime.Now).TotalDays), 0);
            var actualBabyName = string.IsNullOrEmpty(babyName) ? "Baby" : babyName;

            return new NotificationCompat.Builder(context, CHANNEL_ID)
                          .SetAutoCancel(true)
                          .SetContentTitle("Pregnancy Countdown")
                          .SetSmallIcon(Resource.Drawable.icons8_heart_64)
                          .SetContentText($"{actualBabyName} will arrive in just {days} days!")
                          .Build();
        }

        private void SetupNotificationChannel(Context context)
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            var name = "net.gedoens.pregnancycountdown.notifications";
            var description = "Notification channel for Pregnancy Countdown";
            var channel = new NotificationChannel(CHANNEL_ID, name, NotificationImportance.High)
            {
                Description = description,
                LockscreenVisibility = NotificationVisibility.Public
            };

            var notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }

    }
}