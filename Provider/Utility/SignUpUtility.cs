using System;
using Provider.Infrastructure;
using Xamarin.Auth;
using Provider.Views;
using System.Linq;
using System.Collections.Generic;
using Xamarin.Forms;
using Provider.Actions;
using Provider.Models;

namespace Provider.Utility
{
    public class AccountUtility : GetProfileAction.IActionResponse
	{

        public static AccountUtility Instance
        {
            get
            {
                return new AccountUtility();
            }
        }

        public void AddUserDatatoStore(Account account, AccTypes accType)
		{
			if (accType == AccTypes.Google)
				App.Store.Save(account, Constants.GoogleAuth);
			else if (accType == AccTypes.Facebook)
				App.Store.Save(account, Constants.FacebookAuth);

			string email;
			account.Properties.TryGetValue("Email", out email);

            GetProfileAction action = new GetProfileAction(email,this);
            action.Perform();

		}

        async void HandleNavigation(bool userExists)
        {
            ProviderLaunchPage nextPage;
            if (userExists)
            {
                //Currently using UserSignup Page.
                //Once the api is fully working we can go to profile page
                nextPage = new ProviderLaunchPage()
                {
                    Detail = new UserSignUpPage(),
                    BarBackgroundColor = Color.FromHex("#343434"),
                    BarTintColor = Color.White,
                    ShowLeftMasterNavButton = false
                };
            }
            else
            {
                nextPage = new ProviderLaunchPage()
                {
                    Detail = new UserSignUpPage(),
                    BarBackgroundColor = Color.FromHex("#343434"),
                    BarTintColor = Color.White,
                    ShowLeftMasterNavButton = false
                };
            }

            await App.Current.MainPage.Navigation.PopModalAsync();
            App.Current.MainPage = nextPage;
        }

        public void OnActionSuccess(ProviderProfileModel data, string actionIdentifier)
        {
            if (data.Msg != "NO RECORDS")
                HandleNavigation(true);
            else
                HandleNavigation(false);
		}

		public void OnActionError(string message, string actionIdentifier)
		{
            HandleNavigation(false);
		}
	}
}
