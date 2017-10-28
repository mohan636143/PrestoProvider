using System;
using Provider.Infrastructure;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using Provider.Views;
using System.Globalization;
using Provider.Models;

namespace Provider.ViewModels
{
    public class ProfileStepOnePageViewModel : ViewModelBase
    {

        #region Properties

        private string _kitchenName;
        public string KitchenName
        {
            get
            {
                return _kitchenName;
            }
            set
            {
                _kitchenName = value;
                OnPropertyChanged("KitchenName");
            }
        }

		private string _about;
		public string About
		{
			get
			{
				return _about;
			}
			set
			{
				_about = value;
				OnPropertyChanged("About");
			}
		}

		private string _channel;
		public string Channel
		{
			get
			{
				return _channel;
			}
			set
			{
				_channel = value;
				OnPropertyChanged("Channel");
			}
		}

		private bool _preOrder;
		public bool PreOrder
		{
			get
			{
				return _preOrder;
			}
			set
			{
				_preOrder = value;
				OnPropertyChanged("PreOrder");
			}
		}

		private int _selectedTime;
		public int SelectedTime
		{
			get
			{
				return _selectedTime;
			}
			set
			{
				_selectedTime = value;
				OnPropertyChanged("SelectedTime");
			}
		}

        private List<string> _cateringTimes;
        public List<string> CateringTimes
        {
            get
            {
                return _cateringTimes;
            }
            set
            {
                _cateringTimes = value;
                OnPropertyChanged("CateringTimes");
            }
        }

        public bool IsKitchenNameValid { get; set; }
        public bool IsChannelValid { get; set; }

        #endregion

        #region Commands

        public ICommand NextCommand { get; set; }

        #endregion

        #region Constructor

        public ProfileStepOnePageViewModel()
        {
            IsKitchenNameValid = IsChannelValid = false;
            CateringTimes = new List<string>() { "Select", "15 mins", "30 mins", "60 mins", "90 mins", "2 hours", "3 hours" };
            NextCommand = new Command(() => ProceedtoNextPage());
        }

        #endregion

        #region Methods

        private void ProceedtoNextPage()
        {
            if (ValidateData())
            {
                UpdateSingleton();
                App.SetPage(new ProfileStepTwoPage());
            }
            else
                App.Current.MainPage.DisplayAlert("Error", "Please correct data and try again.", "OK");
        }

        private void UpdateSingleton()
        {
            ProviderProfileModel userData = AppModel.AppDataInstance.ProviderData;

            userData.KitchenName = KitchenName;
            userData.Desc = About;
            userData.YouTubeCh = Channel;
            userData.IsPreOrder = PreOrder;
            userData.MinLeadTime = CateringTimes[SelectedTime];
        }

        bool ValidateData()
        {
            if (IsKitchenNameValid && IsChannelValid && SelectedTime > 0)
                return true;
            else
                return false;
        }

        #endregion
    }
}
