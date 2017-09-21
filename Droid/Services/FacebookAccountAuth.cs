using System;
using Xamarin.Forms;
using Provider.Droid.Services;
using Provider.Interface;
using Provider.Infrastructure;
using Xamarin.Auth;
using Android.Content;
using Provider.Droid.Activities;
using Xamarin.Facebook;

[assembly: Dependency(typeof(FacebookAccountAuth))]
namespace Provider.Droid.Services
{
    public class FacebookAccountAuth : IFacebookAuth
    {
        public event EventHandler<Account> OnLoginSucceded;

        public void GetAccDetails(AccTypes AuthType)
        {
			Context currentContext = Android.App.Application.Context;
			FBLoginActivity.OnAccDetailsReceived += (sender, e) => OnDetailsReceived(e);
			Intent fbLoginIntent = new Intent(Android.App.Application.Context, typeof(FBLoginActivity));
			fbLoginIntent.AddFlags(ActivityFlags.NewTask);
			currentContext.StartActivity(fbLoginIntent);
        }

        private void OnDetailsReceived(Profile e)
        {
            
        }
    }
}