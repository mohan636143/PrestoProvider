using System;
using Provider.Infrastructure;
using System.Windows.Input;
using Xamarin.Forms;
using Provider.Views;

namespace Provider.ViewModels
{
    public class CodePageViewModel : ViewModelBase
    {
        public ICommand SubmitCodeCommand { get; set; }

        public CodePageViewModel()
        {
            SubmitCodeCommand = new Command(() => LoadNextPage());
        }

		private void LoadNextPage()
		{

            //App.SetPage(new RegSuccessPage());
            App.Current.MainPage = new RegSuccessPage();

		}
    }
}
