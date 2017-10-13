using System;
using Provider.Infrastructure;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using Provider.Views;

namespace Provider.ViewModels
{
    public class ProfileStepOnePageViewModel : ViewModelBase
    {

        #region Properties

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

        #endregion

        #region Commands

        public ICommand NextCommand { get; set; }

        #endregion

        #region Constructor

        public ProfileStepOnePageViewModel()
        {

            CateringTimes = new List<string>() { "Select", "15 mins", "30 mins", "60 mins", "90 mins", "2 hours", "3 hours" };
            NextCommand = new Command(() => ProceedtoNextPage());
        }

        #endregion

        #region Methods

        private void ProceedtoNextPage()
        {
            App.SetPage(new ProfileStepTwoPage());
        }

        #endregion
    }
}
