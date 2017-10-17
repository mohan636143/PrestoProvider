using System;
using Provider.Infrastructure;
using Xamarin.Auth;
using Provider.Utility;
using System.Linq;
using Xamarin.Forms;
using System.Windows.Input;
using Provider.Views;
using Provider.Models;

namespace Provider.ViewModels
{
    public class UserSignUpPageViewModel : ViewModelBase
    {

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

        private string _firstname;
        public string FirstName
        {
            get
            {
                return _firstname;
            }
            set
            {
                _firstname = value;
                OnPropertyChanged("FirstName");
            }
        }

		private string _lastname;
		public string LastName
		{
			get
			{
                return _lastname;
			}
			set
			{
                _lastname = value;
				OnPropertyChanged("LastName");
			}
		}

        private string _email;
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
                OnPropertyChanged("Email");
            }
        }

        private string _mobile;
        public string Mobile
        {
            get
            {
                return _mobile;
            }
            set
            {
                _mobile = value;
                OnPropertyChanged("Mobile");
            }
        }

        private int _age;
        public int Age
        {
            get
            {
                return _age;
            }
            set
            {
                _age = value;
                OnPropertyChanged("Age");
            }
        }

        private string _location;
        public string Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
                OnPropertyChanged("Location");
            }
        }

        public UserProfile StoredUserProfileData { get; set; }

        private bool _enableNotify;
        public bool EnableNotify
        {
            get
            {
                return _enableNotify;
            }
            set
            {
                _enableNotify = value;
                OnPropertyChanged("EnableNotify");
            }
        }

        private string _aboutMe;
        public string AboutMe
        {
            get
            {
                return _aboutMe;
            }
            set
            {
                _aboutMe = value;
                OnPropertyChanged("AboutMe");
            }
        }

		private bool _accessFriends;
		public bool AccessFriends
		{
			get
			{
				return _accessFriends;
			}
			set
			{
				_accessFriends = value;
				OnPropertyChanged("AccessFriends");
			}
		}

		private bool _addPosts;
		public bool AddPosts
		{
			get
			{
				return _addPosts;
			}
			set
			{
				_addPosts = value;
				OnPropertyChanged("AddPosts");
			}
		}

        #endregion

        #region Commands

        public ICommand NextCommand { get; set; }

        #endregion

        #region Constructor

        public UserSignUpPageViewModel()
        {
            StoredAcc = App.Store.FindAccountsForService(Constants.GoogleAuth).FirstOrDefault();
            if (StoredAcc == null)
                StoredAcc = App.Store.FindAccountsForService(Constants.FacebookAuth).FirstOrDefault();
            UpdateData(StoredAcc);
            StoredUserProfileData = new UserProfile();
            NextCommand = new Command((obj) => LoadNextPage());
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
            FirstName = tempData;
            //Last Name
            storedAcc.Properties.TryGetValue(AccProperties.LastName, out tempData);
            LastName = tempData;
            //Email
            storedAcc.Properties.TryGetValue(AccProperties.Email, out tempData);
            Email = tempData;
            //Mobile
            storedAcc.Properties.TryGetValue(AccProperties.Mobile, out tempData);
            Mobile = tempData;
            //Location
            storedAcc.Properties.TryGetValue(AccProperties.Location, out tempData);
            Location = tempData;
        }



		private void LoadNextPage()
		{
            UpdateProfileData(StoredUserProfileData);
            //NavigationPage navPage = ((App.Current.MainPage as DualMasterPage).DualMasterNavPage);
            //navPage.PushAsync(new CuisindAndDishesPage());

            //DualMasterPage mainPage = App.Current.MainPage as DualMasterPage;
            //if (mainPage != null)
            //mainPage.Detail = new CuisindAndDishesPage();

            UpdateSingleton();

            App.SetPage(new CodePage());

		}

        private void UpdateProfileData(UserProfile profileData)
        {
            profileData.ProfilePic = ProfilePic;
            profileData.FirstName = FirstName;
            profileData.LastName = LastName;
            profileData.Email = Email;
            profileData.Age = Age;
            profileData.Location = Location;
        }

        void UpdateSingleton()
		{
            ProviderProfileModel userData = AppModel.AppDataInstance.ProviderData;

			userData.ID = Email;
			userData.Name = FirstName + " " + LastName;
            userData.Email = Email;
            userData.Age = Age;
            userData.MobNo = Mobile;
        }

        #endregion

    }
}
