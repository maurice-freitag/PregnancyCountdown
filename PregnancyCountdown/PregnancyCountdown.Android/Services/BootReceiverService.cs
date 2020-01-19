
using Android.App;
using Android.Content;
using Android.OS;
using Xamarin.Essentials;

namespace PregnancyCountdown.Droid.Services
{
    [Service(Name = "net.gedoens.pregnancycountdown.BootReceiverService", Enabled = true)]
    public class BootReceiverService : Service
    {
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            return StartCommandResult.NotSticky;
        }

        public override void OnCreate()
        {
            base.OnCreate();
            var notificationHandler = new NotificationHandler();
            var enableNotifications = Preferences.Get("EnableNotifications", false);
            notificationHandler.UpdateNotificationPreferences(enableNotifications);
        }
    }
}