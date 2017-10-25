using System;
using System.Collections.Generic;
using System.Windows.Input;
using Provider.Infrastructure;
using Provider.Models;
using Provider.Views;
using Xamarin.Forms;
namespace Provider.ViewModels
{
    public class ProfileStepTwoPageViewModel : ViewModelBase
    {

        #region Properties

        private int _caterIndex;
        public int CaterIndex
        {
            get
            {
                return _caterIndex;
            }
            set
            {
                _caterIndex = value;
                OnPropertyChanged("CaterIndex");
            }
        }

		private List<string> _peopleCount;
		public List<string> PeopleCount
		{
			get
			{
				return _peopleCount;
			}
			set
			{
				_peopleCount = value;
				OnPropertyChanged("PeopleCount");
			}
		}


		private bool _openAllWeek;
		public bool OpenAllWeek
		{
			get
			{
				return _openAllWeek;
			}
			set
			{
				_openAllWeek = value;
				OnPropertyChanged("OpenAllWeek");
			}
		}

		private List<string> _timeslots;
		public List<string> TimeSlots
		{
			get
			{
				return _timeslots;
			}
			set
			{
				_timeslots = value;
				OnPropertyChanged("TimeSlots");
			}
		}

		private int _weekStartIndex;
		public int WeekStartIndex
		{
			get
			{
				return _weekStartIndex;
			}
			set
			{
				_weekStartIndex = value;
				OnPropertyChanged("WeekStartIndex");
			}
		}

		private int _weekEndIndex;
		public int WeekEndIndex
		{
			get
			{
				return _weekEndIndex;
			}
			set
			{
				_weekEndIndex = value;
				OnPropertyChanged("WeekEndIndex");
			}
		}

        #endregion

        #region Commands

        public ICommand NextCommand { get; set; }

		#endregion

		#region Constructor

		public ProfileStepTwoPageViewModel()
		{
            NextCommand = new Command(() => HandleNext());
            PeopleCount = new List<string>() {"Select", "5","10","15","20","25","50","100","200","500" };
            TimeSlots = new List<string>() { "Select", "6 AM", "7 AM", "8 AM", "9 AM", "10 AM", "11 AM", "12 PM", "1 PM", 
                                                       "2 PM", "3 PM", "4 PM", "5 PM", "6 PM", "7 PM", "8 PM", "9 PM" };
		}

		#endregion

		#region Methods

		private void HandleNext()
		{
            UpdateSingleton();
            App.SetPage(new ProfileStepThreePage());
		}

		private void UpdateSingleton()
		{
			ProviderProfileModel userData = AppModel.AppDataInstance.ProviderData;

            userData.CaterUpTo = CaterIndex != 0 ? Convert.ToInt16(PeopleCount[CaterIndex]) : 0;
            userData.IsAllWeek = OpenAllWeek;
            userData.BHourFrom = TimeSlots[WeekStartIndex];
            userData.BHourTo = TimeSlots[WeekEndIndex];
            //TODO : Add logic for sunday
		}

		#endregion
	}
}
