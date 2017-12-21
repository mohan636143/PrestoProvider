﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Provider.Infrastructure;
using Xamarin.Forms;

namespace Provider.Droid
{

    [Activity(Label = "CustomUrlSchemeInterceptorActivity", NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
    [IntentFilter(
     new[] { Intent.ActionView },
     Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
     DataSchemes = new[] { "com.googleusercontent.apps.681806121576-148gajscnb75s5ik1bj6jvpco5megus5" },
     DataPath = "/oauth2redirect")]
    public class CustomUrlSchemeInterceptorActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Convert Android.Net.Url to Uri
            var uri = new Uri(Intent.Data.ToString());

            // Load redirectUrl page
            AuthenticationState.Authenticator.OnPageLoading(uri);

            Finish();
            //MessagingCenter.Send<CustomUrlSchemeInterceptorActivity, string>(this, "success", "LoginPageDone");
            //Android.Widget.ListView lv = new Android.Widget.ListView(this.BaseContext);
            //Android.Widget.ProgressBar pb = new Android.Widget.ProgressBar(this.BaseContext);
            //pb.Progress
        }
    }
}
