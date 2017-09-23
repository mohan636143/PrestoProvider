using System;
using System.Windows.Input;
using Provider.Infrastructure;
using Provider.Interface;
using Xamarin.Forms;
using Xamarin.Auth;
using Provider.Views;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Provider.Utility;

namespace Provider.ViewModels
{
	public class LoginPageViewModel : ViewModelBase
	{

		Account account;
		//AccountStore store;
        IGoogleAuth googleAuth;
        IFacebookAuth fbAuth;
        EventHandler<Account> handler;


		//private ICommand _googleLoginCommand;
        public ICommand GoogleLoginCommand { get; set; }
		//{
		//	get { return _googleLoginCommand; }
		//	set
		//	{
		//		_googleLoginCommand = value;
		//		OnPropertyChanged("GoogleLoginCommand");
		//	}
		//}

		//private ICommand _fbLoginCommand;
        public ICommand FbLoginCommand { get; set;}
		//{
		//	get { return _fbLoginCommand; }
		//	set
		//	{
		//		_fbLoginCommand = value;
		//		OnPropertyChanged("FbLoginCommand");
		//	}
		//}

		public LoginPageViewModel()
		{
            GoogleLoginCommand = new Command((obj) => HandleGoogelLogin());
            FbLoginCommand = new Command((obj) => OnFacebookSigin());
            //store = Xamarin.Auth.AccountStore.Create();
		}

        private void HandleGoogelLogin()
        {

				string clientId = null;
				string redirectUri = null;

				switch (Xamarin.Forms.Device.RuntimePlatform)
				{
					case Xamarin.Forms.Device.iOS:
						clientId = Constants.iOSClientId;
						redirectUri = Constants.iOSRedirectUrl;
						break;

					case Xamarin.Forms.Device.Android:
						clientId = Constants.AndroidClientId;
						redirectUri = Constants.AndroidRedirectUrl;
						break;
				}

				var authenticator = new OAuth2Authenticator(
					clientId,
					null,
					Constants.Scope,
					new Uri(Constants.AuthorizeUrl),
					new Uri(redirectUri),
					new Uri(Constants.AccessTokenUrl),
					null,
					true);

				authenticator.Completed += OnAuthCompleted;
				authenticator.Error += OnAuthError;

				AuthenticationState.Authenticator = authenticator;

				var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
				presenter.Login(authenticator);
		}

		async void OnAuthCompleted(object sender, AuthenticatorCompletedEventArgs e)
		{
			var authenticator = sender as OAuth2Authenticator;
			if (authenticator != null)
			{
				authenticator.Completed -= OnAuthCompleted;
				authenticator.Error -= OnAuthError;
			}

			Provider.Infrastructure.User user = null;
			if (e.IsAuthenticated)
			{
				// If the user is authenticated, request their basic user data from Google
				// UserInfoUrl = https://www.googleapis.com/oauth2/v2/userinfo
				var request = new OAuth2Request("GET", new Uri(Constants.UserInfoUrl), null, e.Account);
				var response = await request.GetResponseAsync();
				if (response != null)
				{
					// Deserialize the data and store it in the account store
					// The users email address will be used to identify data in SimpleDB
					string userJson = await response.GetResponseTextAsync();
					user = JsonConvert.DeserializeObject<Provider.Infrastructure.User>(userJson);
				}

				//if (account != null)
				//{
				//	store.Delete(account, Constants.AppName);
				//}
                //AccountStore.Create().Save(e.Account, Constants.GoogleAuth);
				//await store.SaveAsync(account = e.Account, Constants.AppName);
				//await DisplayAlert("Email address", user.Email, "OK");
				//await Navigation.PushAsync(new CustomerHomePage());
				user.access_token = e.Account.Properties["access_token"];
				user.refresh_token = e.Account.Properties["refresh_token"];
				Dictionary<string, string> userEnumerable = new Dictionary<string, string>();
                userEnumerable.Add(AccProperties.Id, user.Id);
                userEnumerable.Add(AccProperties.FirstName, user.GivenName);
                userEnumerable.Add(AccProperties.LastName, user.FamilyName);
                userEnumerable.Add(AccProperties.Email, user.Email);
				userEnumerable.Add(AccProperties.AccessToken, user.access_token);
                userEnumerable.Add(AccProperties.RefreshToken, user.refresh_token);
                userEnumerable.Add(AccProperties.PicUrl, user.Picture);

				account = new Account(Constants.GoogleAuth, userEnumerable as IDictionary<string, string>);

                AccountUtility.AddUserDatatoStore(account, AccTypes.Google);
				
			}
		}

		void OnAuthError(object sender, AuthenticatorErrorEventArgs e)
		{
			var authenticator = sender as OAuth2Authenticator;
			if (authenticator != null)
			{
				authenticator.Completed -= OnAuthCompleted;
				authenticator.Error -= OnAuthError;
			}

			Debug.WriteLine("Authentication error: " + e.Message);
		}


        public void OnFacebookSigin()
        {
            App.Current.MainPage.Navigation.PushModalAsync(new FacebookProfilePage());
        }
    }
}
