using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Auth;

namespace Provider.Infrastructure
{

    public class FacebookServices
    {
        Account account;
        //AccountStore store;

        public async Task<FacebookProfile> GetFacebookProfileAsync(string accessToken)
        {
            //store = AccountStore.Create();
            //account = store.FindAccountsForService(Constants.FacebookAuth).FirstOrDefault();

            var requestUrl =
                "https://graph.facebook.com/v2.7/me/?fields=name,picture,work,website,religion,location,locale,link,cover,age_range,birthday,devices,email,first_name,last_name,gender,hometown,is_verified,languages&scope=email&access_token="
                + accessToken;

            var httpClient = new HttpClient();

            var userJson = await httpClient.GetStringAsync(requestUrl);

            var facebookProfile = JsonConvert.DeserializeObject<FacebookProfile>(userJson);

            await Task.Delay(1000);
            var url = facebookProfile.Picture.Data.Url;
            facebookProfile.access_token = accessToken;

            Dictionary<string, string> userEnumerable = new Dictionary<string, string>();
            userEnumerable.Add("id", facebookProfile.Id);
            userEnumerable.Add("firstName", facebookProfile.FirstName);
            userEnumerable.Add("lastName", facebookProfile.LastName);
            if (string.IsNullOrEmpty(facebookProfile.email))
            {
                facebookProfile.email = "NAN";
            }
            userEnumerable.Add("email", facebookProfile.email);
            userEnumerable.Add("accessToken", facebookProfile.access_token);
            userEnumerable.Add("refreshToken", facebookProfile.access_token);
            userEnumerable.Add("pictureUrl", facebookProfile.Picture.Data.Url.ToString());

            account = new Account(Constants.FacebookAuth, userEnumerable as IDictionary<string, string>);

            App.Store.Save(account, Constants.FacebookAuth);
            return facebookProfile;
        }
    }
}

