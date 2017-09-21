
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Auth.Api;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common.Apis;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using Provider.Infrastructure;
using Xamarin.Auth;
using Android.App.Job;

namespace Provider.Droid.Activities
{
    [Activity(Label = "GoogleLoginActivity")]
	public class GoogleLoginActivity : FragmentActivity
	{

		Context currentContext;
		GoogleSignInOptions gso;
		GoogleApiClient mGoogleApiClient;
		Intent signInIntent;
        SignInAccount accDetails;

        public static event EventHandler<Account> OnAccDetailsReceived;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your application here
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
           
			base.OnActivityResult(requestCode, resultCode, data);
			GoogleSignInResult result = Auth.GoogleSignInApi.GetSignInResultFromIntent(data);
			//mGoogleApiClient.StopAutoManage(this);
            if (mGoogleApiClient.IsConnected)
			{
				mGoogleApiClient.UnregisterFromRuntime();
                Auth.GoogleSignInApi.SignOut(mGoogleApiClient);
                mGoogleApiClient.Disconnect();
                Auth.GoogleSignInApi.Dispose();
            }
            else
            {
                
            }
            if (result.IsSuccess)
                GetSignInResult(result);
            else
            {

            }
		}

		protected override void OnStart()
		{
            try
            {
                base.OnStart();
                currentContext = Android.App.Application.Context;

                gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn).RequestEmail().Build();
                mGoogleApiClient = new GoogleApiClient.Builder(currentContext).AddApi(Auth.GOOGLE_SIGN_IN_API, gso).Build();
                                                      //EnableAutoManage(this, (obj) => { HandleFailure(); }).Build();
                mGoogleApiClient.Connect();
                signInIntent = Auth.GoogleSignInApi.GetSignInIntent(mGoogleApiClient);
                StartActivityForResult(signInIntent, 1000);
            }
            catch(Exception ex)
            {
                mGoogleApiClient.StopAutoManage(this);
                mGoogleApiClient.Disconnect();
            }
		}


		void HandleFailure()
		{

		}

        private void GetSignInResult(GoogleSignInResult result)
        {
           
            Account acc = new Account();
            // Signed in successfully, show authenticated UI. 
            GoogleSignInAccount acct = result.SignInAccount;
            acc.Username = acct.DisplayName;
            acc.Properties.TryAdd("Email", acct.Email);
            acc.Properties.TryAdd("picurl", acct.PhotoUrl?.ToString());
            acc.Properties.TryAdd("accType", AccTypes.Google.ToString());
            Finish();
            FinishAndRemoveTask();
            FinishActivity(1000);
            OnAccDetailsReceived?.Invoke(this, acc);

        }
	}
}
