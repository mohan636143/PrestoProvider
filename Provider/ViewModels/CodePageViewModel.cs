using System;
using Provider.Infrastructure;
using System.Windows.Input;
using Xamarin.Forms;
using Provider.Views;

namespace Provider.ViewModels
{
    public class CodePageViewModel : ViewModelBase
    {

        private string _code;
        public string Code
        {
            get
            {
                return _code;
            }
            set
            {
                _code = value;
                OnPropertyChanged("Code");
            }
        }

        public bool IsCodeValid { get; set; }

        public ICommand SubmitCodeCommand { get; set; }

        public CodePageViewModel()
        {
            IsCodeValid = false;
            SubmitCodeCommand = new Command(() => LoadNextPage());
        }

		private void LoadNextPage()
		{
            if (IsCodeValid)
                App.Current.MainPage = new ProviderLaunchPage() { Detail = new RegSuccessPage(), BarBackgroundColor = Color.Teal, BarTintColor = Color.White, ShowLeftMasterNavButton = false };
            else
                App.Current.MainPage.DisplayAlert("Error", "Please enter a valid 6 digit code", "OK");
		}
    }
}
