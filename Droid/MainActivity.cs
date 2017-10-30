using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Android;
using Android.Support.V4.App;

namespace Provider.Droid
{
    [Activity(Label = "Provider", Icon = "@drawable/Logo", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        int REQUEST_CODE_ASK_PERMISSIONS = 123;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            if ((int)Build.VERSION.SdkInt >= 23)
            {
                List<string> names = new List<string>();

                if (Android.Support.V4.Content.ContextCompat.CheckSelfPermission(this, Manifest.Permission.Camera) != (int)Permission.Granted)
                {
                    names.Add(Manifest.Permission.Camera);
                }
                if (Android.Support.V4.Content.ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) != (int)Permission.Granted)
                {
                    names.Add(Manifest.Permission.ReadExternalStorage);
                }

                if (Android.Support.V4.Content.ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) != (int)Permission.Granted)
                {
                    names.Add(Manifest.Permission.WriteExternalStorage);
                }
                if (names.Count > 0)
                {
                    ActivityCompat.RequestPermissions(this, names.ToArray(), REQUEST_CODE_ASK_PERMISSIONS);
                }
            }

			global::Xamarin.Forms.Forms.Init(this, bundle);
			global::Xamarin.Auth.Presenters.XamarinAndroid.AuthenticationConfiguration.Init(this, bundle);

			LoadApplication(new App());
        }
    }
}
