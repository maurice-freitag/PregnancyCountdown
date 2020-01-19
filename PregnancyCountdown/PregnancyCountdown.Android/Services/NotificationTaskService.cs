
using Android.App;
using Android.Content;
using Android.OS;

namespace PregnancyCountdown.Droid.Services
{
    [IntentFilter(new[] { "net.gedoens.pregnancycountdown.NotificationTaskService" })]
    [Service(Name = "net.gedoens.pregnancycountdown.NotificationTaskService", Enabled = true)]
    public class NotificationTaskService : Service
    {
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            return StartCommandResult.NotSticky;
        }
    }
}