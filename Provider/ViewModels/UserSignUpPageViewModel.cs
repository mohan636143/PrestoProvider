﻿using System;
using Provider.Infrastructure;
using Xamarin.Auth;
using Provider.Utility;
using System.Linq;
using Xamarin.Forms;

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

        #endregion

        #region Constructor

        public UserSignUpPageViewModel()
        {
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

        #endregion

    }
}