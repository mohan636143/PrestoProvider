using System;

using Provider.Infrastructure;
using System.Windows.Input;
using Xamarin.Forms;
using Provider.Utility;

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

        private UserProfile SavedProfile { get; set; }

        #endregion

        #region Commands

        public ICommand NextCommand { get; set; }

        #endregion

        #region Constructor

        public ProviderSkillsPageViewModel()
        {
            NextCommand = new Command((obj) => HandldeNext());
            SavedProfile = ProfileCreationUtility.ProfileData;
        }

        #endregion

        #region Methods

        private void HandldeNext()
        {
            UpdateProfileData();
            NavigationPage navPage = ((App.Current.MainPage as DualMasterPage).DualMasterNavPage);
            //navPage.PushAsync(new ProviderSkillsPage());
        }

        private void UpdateProfileData()
        {
            SavedProfile.Channel = YoutubeLink;
            SavedProfile.Description = AboutMe;
            SavedProfile.Skills = Skills;
            SavedProfile.Awards = Awards;

        }

        #endregion

    }
}
