using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Windows.Input;

namespace Provider.Views
{
    public partial class RegSuccessPage : ContentPage
    {
        public ICommand SubmitCodeCommand { get; set; }

        public RegSuccessPage()
        {
            InitializeComponent();
        }

        void Handle_Tapped(object sender, System.EventArgs e)
        {
            //App.SetPage(new ProfileStepOnePage());

            App.Current.MainPage = new ProviderLaunchPage()
            {
                Detail = new ProfileStepOnePage(),
                BarBackgroundColor = Color.White,
                BarTintColor = (Color)App.Current.Resources["PrestoGreyColor"]
            };
		}
    }
}
