using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Support.V4.App;
using PregnancyCountdown.Services;
using Xamarin.Forms;

namespace PregnancyCountdown.Droid.Services
{
    [IntentFilter(new string[] { "android.intent.action.BOOT_COMPLETED" }, Priority = (int)IntentFilterPriority.LowPriority)]
    [BroadcastReceiver(Enabled = true, Label = "Local Notifications Broadcast Receiver")]
    public class NotificationBroadcastReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            var settingsProvider = DependencyService.Get<ISettingsProvider>();
            var settings = settingsProvider.GetSettings();

            var days = (int)Math.Max(Math.Ceiling((settings.DueDate - DateTime.Now).TotalDays), 0);
            var babyName = string.IsNullOrEmpty(settings.BabyName) ? "Baby" : settings.BabyName;

            var builder = new NotificationCompat.Builder(context, NotificationHandler.CHANNEL_ID)
                          .SetAutoCancel(true)
                          .SetContentTitle("Pregnancy Countdown")
                          .SetLargeIcon(BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.icons8_heart_64))
                          .SetSmallIcon(Resource.Drawable.icons8_heart_64)
                          .SetContentText($"{babyName} will arrive in just {days} days!");

            var notificationManager = NotificationManagerCompat.From(context);
            notificationManager.Notify(NotificationHandler.NOTIFICATION_ID, builder.Build());
        }
    }
}