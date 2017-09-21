using System;
using Provider.Infrastructure;
using System.Windows.Input;
using Xamarin.Forms;
using Provider.Views;
using System.Linq;

namespace Provider.ViewModels
{
    public class MenuPageViewModel : ViewModelBase
    {

        public ICommand LogOutCommand { get; set; }

        public MenuPageViewModel()
        {
            LogOutCommand = new Command((obj) => ProcessLogOut());
        }


        void ProcessLogOut()
        {
			App.LoginAccount = App.Store.FindAccountsForService(Constants.GoogleAuth).FirstOrDefault();
			if (App.LoginAccount == null)
			{
				App.LoginAccount = App.Store.FindAccountsForService(Constants.FacebookAuth).FirstOrDefault();
                if (App.LoginAccount != null)
				App.Store.Delete(App.LoginAccount, Constants.FacebookAuth);
			}
			else
			{
				App.Store.Delete(App.LoginAccount, Constants.GoogleAuth);
			}
            App.Current.MainPage = new LoginPage();
        }
    }
}
