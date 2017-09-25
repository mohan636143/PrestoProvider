using System;

using Provider.Infrastructure;

namespace Provider.ViewModels
{
    public class ProviderSkillsPageViewModel : ViewModelBase
    {

        #region Properties

        private string _youtubeLink;
        public string YoutubeLink
        {
            get
            {
                return _youtubeLink;
            }
            set
            {
                _youtubeLink = value;
                OnPropertyChanged("YoutubeLink");
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

		private string _skills;
		public string Skills
		{
			get
			{
                return _skills;
			}
			set
			{
                _skills = value;
				OnPropertyChanged("Skills");
			}
		}

		private string _awards;
		public string Awards
		{
			get
			{
                return _awards;
			}
			set
			{
                _awards = value;
				OnPropertyChanged("Awards");
			}
		}

        #endregion

        public ProviderSkillsPageViewModel()
        {
        }
    }
}
