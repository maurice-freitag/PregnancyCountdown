using System;
using Android.App;
using Android.Content;
using Android.Support.V4.App;
using Java.Lang;
using PregnancyCountdown.Droid.Services;
using PregnancyCountdown.Services;

namespace PregnancyCountdown.Droid
{
    public class NotificationHandler : INotificationHandler
    {
        internal static readonly string DAYS_KEY = "count";
        internal static readonly string CHANNEL_ID = "pregnancy_countdown_notification";
        internal static readonly int NOTIFICATION_ID = 1810;
        private static Context NOTIFICATION_CONTEXT;
        private static string ALARM_SERVICE;

        internal static void SetNotificationContext(Context notificationContext, string alarmService)
        {
            NOTIFICATION_CONTEXT = notificationContext;
            ALARM_SERVICE = alarmService;
        }
        public void UpdateNotificationPreferences(bool enableNotifications)
        {
            var notificationManager = NotificationManagerCompat.From(NOTIFICATION_CONTEXT);
            notificationManager.CancelAll();

            var intent = new Intent(NOTIFICATION_CONTEXT, typeof(NotificationBroadcastReceiver));
            var pendingIntent = PendingIntent.GetBroadcast(NOTIFICATION_CONTEXT, NOTIFICATION_ID, intent, PendingIntentFlags.UpdateCurrent);
            var alarmManager = (AlarmManager)NOTIFICATION_CONTEXT.GetSystemService(ALARM_SERVICE);
            alarmManager.Cancel(pendingIntent);

            if (enableNotifications)
            {
                var interval = 5000;
                var referenceDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var totalMilliSeconds = (long)(DateTime.Now.AddSeconds(5).ToUniversalTime() - referenceDate).TotalMilliseconds;
                if (totalMilliSeconds < JavaSystem.CurrentTimeMillis())
                    totalMilliSeconds += interval;

                alarmManager.SetRepeating(AlarmType.RtcWakeup, totalMilliSeconds, interval, pendingIntent);
            }
        }
    }
}