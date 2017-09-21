using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Provider.Infrastructure;
using Xamarin.Auth;
using Xamarin.Forms;

namespace Provider.Views
{
	   public partial class FacebookProfilePage : ContentPage
	{
	  Account account;
	  AccountStore store;

	  public FacebookProfilePage()
	  {
	      InitializeComponent();

	      store = AccountStore.Create();
	      account = store.FindAccountsForService(Constants.FacebookAuth).FirstOrDefault();

	      var apiRequest = "https://www.facebook.com/v2.10/dialog/oauth?client_id="
	                 + Constants.FacebookClientId
	                 + "&response_type=token&redirect_uri=https://www.facebook.com/connect/login_success.html";

	      var webview = new WebView
	      {
	          Source = apiRequest,
	          HeightRequest = 1
	      };

	      webview.Navigated += WebViewOnNavigated;

	      Content = webview;

	  }

	  private async void WebViewOnNavigated(object sender, WebNavigatedEventArgs e)
	  {
	      if (e.Url.Contains("access_token"))
	      {
	          var accessToken = ExtractAccessTokenFromUrl(e.Url);

	          if (accessToken != "")
	          {

	              await SetFacebookUserProfileAsync(accessToken);

	              //Content = MainStackLayout;

	              //await Navigation.PushAsync(new CustomerHomePage());
	          }
	      }
	  }

	  private string ExtractAccessTokenFromUrl(string url)
	  {
	      if (url.Contains("access_token") && url.Contains("&expires_in="))
	      {
	          var at = url.Replace("https://www.facebook.com/connect/login_success.html#access_token=", "");

	          var accessToken = at.Remove(at.IndexOf("&expires_in=", StringComparison.Ordinal));

	          return accessToken;
	      }

	      return string.Empty;
	  }
	  public async Task SetFacebookUserProfileAsync(string accessToken)
	  {
	      var facebookServices = new FacebookServices();

	        FacebookProfile facebookProfile = await facebookServices.GetFacebookProfileAsync(accessToken);
            Application.Current.MainPage.Navigation.PopModalAsync();
            Application.Current.MainPage = new ProviderLaunchPage();
	      
	  }
	}
}
