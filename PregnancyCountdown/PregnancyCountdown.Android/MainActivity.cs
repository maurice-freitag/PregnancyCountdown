using System;
using Android.App;
using Android.Content.PM;
using Android.Gms.Common;
using Android.OS;
using Android.Runtime;
using Android.Util;
using PregnancyCountdown.Services;
using Xamarin.Forms;

namespace PregnancyCountdown.Droid
{
    [Activity(Label = "PregnancyCountdown", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        internal static readonly string CHANNEL_ID = "net.gedoens.pregnancycountdown_notificationchannel";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            foreach (var key in Intent.Extras?.KeySet() ?? Array.Empty<string>())
            {
                if (!string.IsNullOrEmpty(key))
                    Log.Debug(nameof(MainActivity), "Key: {0} Value: {1}", key, Intent.Extras.GetString(key));
            }

            EnsurePlayServicesAvailable();
            CreateNotificationChannel();

            Forms.SetFlags("CollectionView_Experimental");
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            DependencyService.Register<ISettingsProvider, SettingsProvider>();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public bool EnsurePlayServicesAvailable()
        {
            var result = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (result != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(result))
                    Log.Debug(nameof(MainActivity), GoogleApiAvailability.Instance.GetErrorString(result));
                else
                {
                    Log.Debug(nameof(MainActivity), "This device is not supported.");
                    Finish();
                }
                return false;
            }

            Log.Debug(nameof(MainActivity), "Google Play Services is available.");
            return true;
        }

        public void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
                return;

            var channel = new NotificationChannel(CHANNEL_ID, CHANNEL_ID, NotificationImportance.Default)
            {
                Description = string.Empty
            };

            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }
    }
}