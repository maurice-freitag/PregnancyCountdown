using System;
using Android.App;
using Android.Content;
using Android.Icu.Util;
using Android.Support.V4.App;
using PregnancyCountdown.Droid.Services;
using PregnancyCountdown.Services;

namespace PregnancyCountdown.Droid
{
    public class NotificationHandler : INotificationHandler
    {
        private static string actionName = "net.gedoens.LOCAL_NOTIFICATION";

        public void UpdateNotificationPreferences(bool enableNotifications)
        {
            if (enableNotifications)
            {
                var calendar = Calendar.GetInstance(Android.Icu.Util.TimeZone.Default);
                calendar.Set(CalendarField.HourOfDay, 19);
                calendar.Set(CalendarField.Minute, DateTime.Now.Minute + 1);

                var requestCode = Convert.ToInt32(new Random().Next(100000, 999999).ToString("D6"));
                var intent = new Intent(Application.Context, typeof(NotificationBroadcastReceiver))
                    .SetAction(actionName);
                var pendingIntent = PendingIntent.GetBroadcast(Application.Context, requestCode, intent, PendingIntentFlags.Immutable);

                var alarmManager = (AlarmManager)Application.Context.GetSystemService(Context.AlarmService);
                alarmManager.SetRepeating(AlarmType.RtcWakeup, calendar.TimeInMillis, 30000, pendingIntent);
            }
            else
            {
                var requestCode = Convert.ToInt32(new Random().Next(100000, 999999).ToString("D6"));
                var intent = new Intent(Application.Context, typeof(NotificationBroadcastReceiver))
                    .SetAction(actionName);
                var pendingIntent = PendingIntent.GetBroadcast(Application.Context, requestCode, intent, PendingIntentFlags.Immutable);

                var alarmManager = (AlarmManager)Application.Context.GetSystemService(Context.AlarmService);
                alarmManager.Cancel(pendingIntent);
                var notificationManager = NotificationManagerCompat.From(Application.Context);
                notificationManager.CancelAll();
            }
        }
    }
}