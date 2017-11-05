using System;
using Provider.Infrastructure;
using Xamarin.Auth;
using Provider.Utility;
using System.Linq;
using Xamarin.Forms;
using System.Windows.Input;
using Provider.Views;
using Provider.Models;
using Provider.Controls;
using Provider.Actions;

namespace Provider.ViewModels
{
    public class UserSignUpPageViewModel : ViewModelBase, GetProfileAction.IActionResponse
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
                isMailChanged = true;
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
                isMobileChanged = true;
                _mobile = value;
                OnPropertyChanged("Mobile");
            }
        }

        private string _age;
        public string Age
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

        //      private bool _enableNotify;
        //      public bool EnableNotify
        //      {
        //          get
        //          {
        //              return _enableNotify;
        //          }
        //          set
        //          {
        //              _enableNotify = value;
        //              OnPropertyChanged("EnableNotify");
        //          }
        //      }

        //      private string _aboutMe;
        //      public string AboutMe
        //      {
        //          get
        //          {
        //              return _aboutMe;
        //          }
        //          set
        //          {
        //              _aboutMe = value;
        //              OnPropertyChanged("AboutMe");
        //          }
        //      }

        //private bool _accessFriends;
        //public bool AccessFriends
        //{
        //	get
        //	{
        //		return _accessFriends;
        //	}
        //	set
        //	{
        //		_accessFriends = value;
        //		OnPropertyChanged("AccessFriends");
        //	}
        //}

        //private bool _addPosts;
        //public bool AddPosts
        //{
        //	get
        //	{
        //		return _addPosts;
        //	}
        //	set
        //	{
        //		_addPosts = value;
        //		OnPropertyChanged("AddPosts");
        //	}
        //}

        private bool _isFirstNameValid = true;
        public bool IsFirstNameValid
        {
            get
            {
                return _isFirstNameValid;
            }
            set
            {
                _isFirstNameValid = value;
                OnPropertyChanged("IsFirstNameValid");
            }
        }

        private bool _isSecNameValid = true;
        public bool IsSecNameValid
        {
            get
            {
                return _isSecNameValid;
            }
            set
            {
                _isSecNameValid = value;
                OnPropertyChanged("IsSecNameValid");
            }
        }

        private bool _isMailValid = true;
        public bool IsMailValid
        {
            get
            {
                if (!_isMailValid && isMailChanged)
                    EmailErrorText = "Please enter a valid e-mail.";
                return _isMailValid;
            }
            set
            {
                _isMailValid = value;
                OnPropertyChanged("IsMailValid");
                if (isMailChanged)
                    EmailErrorText = "Please enter a valid e-mail.";
            }
        }

        private bool _isMobileValid = true;
        public bool IsMobileValid
        {
            get
            {
                if (!_isMobileValid && isMobileChanged)
                    MobileErrorText = "Please enter a valid mobile number.";
                return _isMobileValid;
            }
            set
            {
                _isMobileValid = value;
                OnPropertyChanged("IsMobileValid");
                if (isMobileChanged)
                    MobileErrorText = "Please enter a valid mobile number.";
            }
        }

        private string _emailErrorText;
        public string EmailErrorText
        {
            get
            {
                return _emailErrorText;
            }
            set
            {
                _emailErrorText = value;
                OnPropertyChanged("EmailErrorText");
            }
        }

        private string _mobileErrorText;
        public string MobileErrorText
        {
            get
            {
                return _mobileErrorText;
            }
            set
            {
                _mobileErrorText = value;
                OnPropertyChanged("MobileErrorText");
            }
        }

        private ScrollPositions _scrollerPosition;
        public ScrollPositions ScrollerPosition
        {
            get
            {
                return _scrollerPosition;
            }
            set
            {
                _scrollerPosition = value;
                OnPropertyChanged("ScrollerPosition");
            }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }

        bool isMobileChanged = false;
        bool isMailChanged = false;

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

            //IsFirstNameValid = IsSecNameValid = IsMailValid = IsMobileValid = IsAgeValid = false;
            UpdateData(StoredAcc);
            StoredUserProfileData = new UserProfile();
            NextCommand = new Command((obj) => ValidateUserExistence());
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
            if (!string.IsNullOrEmpty(tempData))
                Email = tempData;
            else
            {
                IsMailValid = false;
            }
            //Mobile
            storedAcc.Properties.TryGetValue(AccProperties.Mobile, out tempData);
            if (!string.IsNullOrEmpty(tempData))
                Mobile = tempData;
            else
                IsMobileValid = false;
            //Location
            storedAcc.Properties.TryGetValue(AccProperties.Location, out tempData);
            Location = tempData;
        }



        //private void LoadNextPage()
        //{
        //    //UpdateProfileData(StoredUserProfileData);
        //    //NavigationPage navPage = ((App.Current.MainPage as DualMasterPage).DualMasterNavPage);
        //    //navPage.PushAsync(new CuisindAndDishesPage());

        //    //DualMasterPage mainPage = App.Current.MainPage as DualMasterPage;
        //    //if (mainPage != null)
        //    //mainPage.Detail = new CuisindAndDishesPage();

        //    UpdateSingleton();

        //    App.Current.MainPage = new ProviderLaunchPage()
        //    {
        //        Detail = new RegSuccessPage(),
        //        BarBackgroundColor = Color.White,
        //        BarTintColor = (Color)App.Current.Resources["PrestoGreyColor"],
        //        ShowLeftMasterNavButton = false
        //    };
        //}

        //private void UpdateProfileData(UserProfile profileData)
        //{
        //    profileData.ProfilePic = ProfilePic;
        //    profileData.FirstName = FirstName;
        //    profileData.LastName = LastName;
        //    profileData.Email = Email;
        //    profileData.Age = Age;
        //    profileData.Location = Location;
        //}

        void UpdateSingleton()
        {
            ProviderProfileModel userData = AppModel.AppDataInstance.ProviderData;

            userData.ID = Email;
            userData.Name = FirstName + " " + LastName;
            userData.Email = Email;
            userData.Age = Convert.ToInt32(Age);
            userData.MobNo = Mobile;
        }

        bool ValidateData()
        {
            isMobileChanged = isMailChanged = true;
            if (IsFirstNameValid && IsSecNameValid && IsMailValid && IsMobileValid)
            {
                //LoadNextPage();
                return true;
            }
            else
            {
                if (isMobileChanged)
                    ScrollerPosition = ScrollPositions.Bottom;
                return false;
                //App.Current.MainPage.DisplayAlert("Error","Please correct data and try again.","OK");
            }
        }

        void ValidateUserExistence()
        {
            bool isValid = ValidateData();
            if (isValid)
            {
                GetProfileAction action = new GetProfileAction(Email, this);
                action.Perform();
                IsBusy = true;
            }
        }

		async void HandleNavigation(bool userExists)
		{
            UpdateSingleton();
			ProviderLaunchPage nextPage;
			if (userExists)
			{
				//Currently using UserSignup Page.
				//Once the api is fully working we can go to profile page
				nextPage = new ProviderLaunchPage()
				{
                    Detail = new RegSuccessPage(),
					BarBackgroundColor = Color.White,
					BarTintColor = (Color)App.Current.Resources["PrestoGreyColor"],
					ShowLeftMasterNavButton = false
				};
			}
			else
			{
				nextPage = new ProviderLaunchPage()
				{
                    Detail = new RegSuccessPage(),
					BarBackgroundColor = Color.White,
					BarTintColor = (Color)App.Current.Resources["PrestoGreyColor"],
					ShowLeftMasterNavButton = false
				};
			}

			App.Current.MainPage = nextPage;
		}

		#endregion

		#region ActionRespose

		public void OnActionSuccess(ProviderProfileModel data, string actionIdentifier)
		{
            IsBusy = false;
			if (data.Msg != "NO RECORDS")
				HandleNavigation(true);
			else
				HandleNavigation(false);
		}

		public void OnActionError(string message, string actionIdentifier)
		{
            IsBusy = false;
			HandleNavigation(false);
		}

        #endregion

    }
}
