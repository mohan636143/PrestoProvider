
using System;
using System.Collections.Generic;
using Android.Runtime;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Xamarin.Facebook.Login.Widget;
using Xamarin.Facebook;
using Android.Support.V4.App;
using Provider.Droid.Infrastructure;
using Provider.Droid;

namespace Provider.Droid.Activities
{
    [Activity(Label = "FBLoginActivity")]
	public class FBLoginActivity : FragmentActivity, IFacebookCallback
	{
		private ICallbackManager mCallManager;
		LoginButton btnLogin;
		public static event EventHandler<Profile> OnAccDetailsReceived;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			//Android.App.Fragment FbFragment = new Android.App.Fragment();
			FacebookSdk.ApplicationId = "117832928935318";
			FacebookSdk.SdkInitialize(this.ApplicationContext);
			mCallManager = CallbackManagerFactory.Create();
			btnLogin = new LoginButton(this.ApplicationContext);
			btnLogin.SetReadPermissions(new List<string> { "user_friends", "public_profile" });
			btnLogin.RegisterCallback(mCallManager, this);
            //SetContentView(Resource.Layout.FbLogin);
			FacebookProfileTracker tracker = new FacebookProfileTracker();
			tracker.mOnProfileChanged += OnFacebookAccountDetailsReceived;
			tracker.StartTracking();
		}

		private void OnFacebookAccountDetailsReceived(object sender, Profile e)
		{

		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);
			mCallManager.OnActivityResult(requestCode, (int)resultCode, data);
		}

		protected override void OnStart()
		{
			base.OnStart();
		}

		public void OnCancel()
		{

		}

		public void OnError(FacebookException error)
		{

		}

		public void OnSuccess(Java.Lang.Object result)
		{

		}
	}
}
