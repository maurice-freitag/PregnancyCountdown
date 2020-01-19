using Android.App;
using Android.Content;
using Android.Util;

namespace PregnancyCountdown.Droid.Services
{
    [IntentFilter(new string[] { Intent.ActionBootCompleted }, Priority = (int)IntentFilterPriority.HighPriority)]
    [BroadcastReceiver(Enabled = true)]
    public class BootBroadcastReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            Log.WriteLine(LogPriority.Debug, nameof(BootBroadcastReceiver), "boot receiver started");

            var serviceIntent = new Intent(context, typeof(BootReceiverService));
            context.StartService(serviceIntent);
        }
    }
}