﻿using Xamarin.Forms;
using Provider.Views;
using Provider.Themes;
using Xamarin.Auth;
using Provider.Infrastructure;
using System.Linq;

namespace Provider
{
    public partial class App : Application
    {
        public static AccountStore Store;
        public static Account LoginAccount;

        public App()
        {
            InitializeComponent();

            this.Resources = new DefaultTheme().Resources;
            Store = AccountStore.Create();

			//App.LoginAccount = App.Store.FindAccountsForService(Constants.GoogleAuth).FirstOrDefault();
			//if (App.LoginAccount == null)
			//{
			//	App.LoginAccount = App.Store.FindAccountsForService(Constants.FacebookAuth).FirstOrDefault();
            //   if(App.LoginAccount != null)
			//	App.Store.Delete(App.LoginAccount, Constants.FacebookAuth);
			//}
			//else
			//{
			//	App.Store.Delete(App.LoginAccount, Constants.GoogleAuth);
			//}


            LoginAccount = Store.FindAccountsForService(Constants.FacebookAuth).FirstOrDefault();
            if(LoginAccount == null)
                LoginAccount = Store.FindAccountsForService(Constants.GoogleAuth).FirstOrDefault();
            if (LoginAccount == null)
                MainPage = new LoginPage();
            else
                MainPage = new ProviderLaunchPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
