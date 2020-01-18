using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using PregnancyCountdown.Services;
using Xamarin.Forms;

namespace PregnancyCountdown.Droid
{
    [Activity(Label = "PregnancyCountdown", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            DependencyService.Register<ISettingsProvider, SettingsProvider>();
            DependencyService.Register<INotificationHandler, NotificationHandler>();

            SetupNotificationChannel(this, AlarmService);
        }

        private void SetupNotificationChannel(Context notificationContext, string alarmService)
        {
            NotificationHandler.SetNotificationContext(notificationContext, alarmService);
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            var name = "PregnancyCountdown.ChannelName";
            var description = "Notification channel for PregnanyCountdown";
            var channel = new NotificationChannel(NotificationHandler.CHANNEL_ID, name, NotificationImportance.Default)
            {
                Description = description,
                LockscreenVisibility = NotificationVisibility.Public
            };

            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}