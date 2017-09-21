using System;
using Android.App;
using Android.Content;
using Android.Gms.Auth.Api.SignIn;
using Provider.Droid.Activities;
using Provider.Droid.Services;
using Provider.Infrastructure;
using Provider.Interface;
using Xamarin.Auth;
using Xamarin.Forms;
[assembly: Dependency(typeof(GoogleAccAuth))]
namespace Provider.Droid.Services
{

    public class GoogleAccAuth : IGoogleAuth
    {
        Intent googleLoginIntent;
        Context currentContext;
        public event EventHandler<Account> OnLoginSucceded;
        EventHandler<Account> handler;


        public void GetAccDetails(AccTypes AuthType)
		{
			currentContext = Android.App.Application.Context;
            handler = (object sender, Account e) => { OnDetailsReceived(e); };
            GoogleLoginActivity.OnAccDetailsReceived -= handler;
            GoogleLoginActivity.OnAccDetailsReceived += handler;
            googleLoginIntent = new Intent(Android.App.Application.Context, typeof(GoogleLoginActivity));
            googleLoginIntent.AddFlags(ActivityFlags.NewTask);
            currentContext.StartActivity(googleLoginIntent);

            //currentContext.StartActivity(typeof(GoogleLoginActivity));

		}

        private void OnDetailsReceived(Account e)
		{
            handler = null;
            currentContext.StopService(googleLoginIntent);
			googleLoginIntent.UnregisterFromRuntime();
            //googleLoginIntent.on
            googleLoginIntent.Dispose();
			OnLoginSucceded?.Invoke(this, e);
		}
    }
}
