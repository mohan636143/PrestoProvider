using System;
using Provider.Infrastructure;

namespace Provider.ViewModels
{
    public class ProfilePageViewModel : ViewModelBase
    {

        private string _name ;
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
				OnPropertyChanged("Name");
			}
		}

        private string _leadTime;
        public string LeadTime
        {
            get
            {
                return _leadTime;
            }
            set
            {
                _leadTime = value;
                OnPropertyChanged("LeadTime");
            }
        }

        private string _status;
		public string Status
		{
			get
			{
				return _status;
			}
			set
			{
				_status = value;
				OnPropertyChanged("Status");
			}
		}

		private string _distance;
		public string Distance
		{
			get
			{
				return _distance;
			}
			set
			{
				_distance = value;
				OnPropertyChanged("Distance");
			}
		}

        public ProfilePageViewModel()
        {
            Name = "Jon Snow";
            AboutMe = "Cooking soups has always beem my passion and I do them exceptionally well. I use quality organic secret ingredients and slow cook to deliver you the best soup in the world";
            LeadTime = "4";
            Status = "Online";
            Distance = "5";
        }
    }
}
