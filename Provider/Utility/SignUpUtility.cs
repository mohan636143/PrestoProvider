using System;
using Provider.Infrastructure;
using Xamarin.Auth;
using Provider.Views;
using System.Linq;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Provider.Utility
{
    public static class AccountUtility
    {
        public async static void AddUserDatatoStore(Account account, AccTypes accType)
        {
            if (accType == AccTypes.Google)
                App.Store.Save(account, Constants.GoogleAuth);
            else if (accType == AccTypes.Facebook)
                App.Store.Save(account, Constants.FacebookAuth);

            string email;
            account.Properties.TryGetValue("Email", out email);
            if(!string.IsNullOrEmpty(email))
            {
                ProviderLaunchPage nextPage;

                if(CheckUserExistence(email))
                {
                    nextPage = new ProviderLaunchPage() { Detail = new ProfilePage() ,BarBackgroundColor = Color.Black, BarTintColor = Color.White};
                }
                else
                {
                    nextPage = new ProviderLaunchPage() { Detail = new UserSignUpPage() , BarBackgroundColor = Color.Black , BarTintColor = Color.White };
                }
                if (accType == AccTypes.Facebook)
                    await App.Current.MainPage.Navigation.PopModalAsync();
                App.Current.MainPage = nextPage;
            }
        }

        public static bool CheckUserExistence(string Email)
        {
            bool isExistingUser = false;

            //Call the api to get the user existence

            return isExistingUser;
        }
    }
}
