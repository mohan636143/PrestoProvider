using System;
using System.Windows.Input;
using Provider.Infrastructure;
using Provider.Views;
using Xamarin.Forms;
namespace Provider.ViewModels
{
    public class ProfileStepTwoPageViewModel : ViewModelBase
    {

		#region Commands

		public ICommand NextCommand { get; set; }

		#endregion

		#region Constructor

		public ProfileStepTwoPageViewModel()
		{
            NextCommand = new Command(() => HandleNext());
		}

		#endregion

		#region Methods

		private void HandleNext()
		{
            App.SetPage(new ProfileStepThreePage());
		}

		#endregion
	}
}
