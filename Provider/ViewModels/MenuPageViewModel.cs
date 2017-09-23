using System;
using Provider.Infrastructure;
using System.Windows.Input;
using Xamarin.Forms;
using Provider.Views;
using System.Linq;
using Xamarin.Auth;

namespace Provider.ViewModels
{
    public class MenuPageViewModel : ViewModelBase
    {

        public ICommand LogOutCommand { get; set; }

		#region Properties

		private Account _storedAcc;
		public Account StoredAcc
		{
			get
			{
				return _storedAcc;
			}
			set
			{
				_storedAcc = value;
				OnPropertyChanged("StoredAcc");
			}
		}

		private ImageSource _profilePic;
		public ImageSource ProfilePic
		{
			get
			{
				return _profilePic;
			}
			set
			{
				_profilePic = value;
				OnPropertyChanged(("ProfilePic"));
			}
		}

		private string _name;
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
                _name = value;
				OnPropertyChanged("Name");
			}
		}

        #endregion

        #region Constructor

        public MenuPageViewModel()
        {
            LogOutCommand = new Command((obj) => ProcessLogOut());

			StoredAcc = App.Store.FindAccountsForService(Constants.GoogleAuth).FirstOrDefault();
			if (StoredAcc == null)
				StoredAcc = App.Store.FindAccountsForService(Constants.FacebookAuth).FirstOrDefault();
			UpdateData(StoredAcc);
		}

        #endregion

		#region Methods

		//Gets Data from stored auth account and updates the values to reflect in UI
		private void UpdateData(Account storedAcc)
		{
			//Variable to get the data from Auth Acc
			string tempData;

			//Pic
			storedAcc.Properties.TryGetValue(AccProperties.PicUrl, out tempData);
			ProfilePic = ImageSource.FromUri(new Uri(tempData));
			//First Name
			storedAcc.Properties.TryGetValue(AccProperties.FirstName, out tempData);
			Name = tempData + " ";
			//Last Name
			storedAcc.Properties.TryGetValue(AccProperties.LastName, out tempData);
			Name = tempData;
		}

		#endregion


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
