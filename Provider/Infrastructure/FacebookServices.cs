using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Auth;
using Provider.Utility;
using ModernHttpClient;

namespace Provider.Infrastructure
{

    public class FacebookServices
    {
        Account account;
        //AccountStore store;

        public async Task<FacebookProfileData> GetFacebookProfileAsync(string accessToken)
        {
            try
            {
                //store = AccountStore.Create();
                //account = store.FindAccountsForService(Constants.FacebookAuth).FirstOrDefault();

                var requestUrl = "https://graph.facebook.com/v2.10/me?fields=name,birthday,accounts,location,first_name,last_name,locale,picture,email,gender&access_token=" + accessToken;
                    //"https://graph.facebook.com/v2.10/me?fields=name,birthday,accounts,location,first_name,last_name,locale,picture,email,gender&access_token=" ;

                //"https://graph.facebook.com/v2.7/me/?fields=name,picture,work,website,religion,location,locale,link,cover,age_range,birthday,devices,email,first_name,last_name,gender,hometown,is_verified,languages&scope=email&access_token="
                //+ accessToken;

                HttpClient httpClient = new HttpClient(new NativeMessageHandler());

                var userJson = await httpClient.GetStringAsync(requestUrl);

                var facebookProfile = JsonConvert.DeserializeObject<FacebookProfileData>(userJson);

                await Task.Delay(1000);
                var url = facebookProfile.Picture.Data.Url;
                //facebookProfile.access_token = accessToken;

                Dictionary<string, string> userEnumerable = new Dictionary<string, string>();
                userEnumerable.Add(AccProperties.Id, facebookProfile.Id);
                userEnumerable.Add(AccProperties.FirstName, facebookProfile.First_Name);
                userEnumerable.Add(AccProperties.LastName, facebookProfile.Last_Name);
                if (string.IsNullOrEmpty(facebookProfile.Email))
                {
                    facebookProfile.Email = "NAN";
                }
                userEnumerable.Add(AccProperties.Email, facebookProfile.Email);
                userEnumerable.Add(AccProperties.AccessToken, accessToken);
                userEnumerable.Add(AccProperties.RefreshToken, accessToken);
                userEnumerable.Add(AccProperties.PicUrl, facebookProfile.Picture.Data.Url.ToString());

                //requestUrl = "https://graph.facebook.com/my_page_id?fields=" + facebookProfile.access_token + "&access_token=" + facebookProfile.access_token;
                //userJson = await httpClient.GetStringAsync(requestUrl);
                //facebookProfile = JsonConvert.DeserializeObject<FacebookProfile>(userJson);

                account = new Account(Constants.FacebookAuth, userEnumerable as IDictionary<string, string>);

                AccountUtility.Instance.AddUserDatatoStore(account, AccTypes.Facebook);
                return facebookProfile;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}

