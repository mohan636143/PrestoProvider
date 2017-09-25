using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Xamarin.Auth;

namespace Provider.Infrastructure
{
	[JsonObject]
	public class User
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("email")]
		public string Email { get; set; }

		[JsonProperty("verified_email")]
		public bool VerifiedEmail { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("given_name")]
		public string GivenName { get; set; }

		[JsonProperty("family_name")]
		public string FamilyName { get; set; }

		[JsonProperty("link")]
		public string Link { get; set; }

		[JsonProperty("picture")]
		public string Picture { get; set; }

		[JsonProperty("gender")]
		public string Gender { get; set; }

		public string access_token { get; set; }

		public string refresh_token { get; set; }

		public string mobile { get; set; }
	}
	public class UserDetails
	{
		public string TwitterId { get; set; }
		public string Name { get; set; }
		public string ScreenName { get; set; }
		public string Token { get; set; }
		public string TokenSecret { get; set; }
		public bool IsAuthenticated
		{
			get
			{
				return !string.IsNullOrWhiteSpace(Token);
			}
		}
	}
	public class AuthenticationState
	{
		public static OAuth2Authenticator Authenticator;
	}
	public static class Constants
	{
		public static string AppName = "Provider";
		public static string GoogleAuth = "ProviderGoogle";
		public static string FacebookAuth = "ProviderFacebook";


		// OAuth
		// For Google login, configure at https://console.developers.google.com/
		public static string iOSClientId = "681806121576-2s91or0v52qkqvp952cgt9466aoblcbs.apps.googleusercontent.com";
		public static string AndroidClientId = "681806121576-148gajscnb75s5ik1bj6jvpco5megus5.apps.googleusercontent.com";

		//For Facebook login
		public static string FacebookClientId = "1705068729532884";

		// These values do not need changing
		public static string Scope = "https://www.googleapis.com/auth/userinfo.email";
		public static string AuthorizeUrl = "https://accounts.google.com/o/oauth2/auth";
		public static string AccessTokenUrl = "https://www.googleapis.com/oauth2/v4/token";
		public static string UserInfoUrl = "https://www.googleapis.com/oauth2/v2/userinfo";

		// Set these to reversed iOS/Android client ids, with :/oauth2redirect appended
		public static string iOSRedirectUrl = "com.googleusercontent.apps.681806121576-2s91or0v52qkqvp952cgt9466aoblcbs:/oauthredirect";
		public static string AndroidRedirectUrl = "com.googleusercontent.apps.681806121576-148gajscnb75s5ik1bj6jvpco5megus5:/oauthredirect";

		//private static PropertyInfo GetProperty(TypeInfo typeInfo, string propertyName)
		//{
		//    var propertyInfo = typeInfo.GetDeclaredProperty(propertyName);
		//    if (propertyInfo == null && typeInfo.BaseType != null)
		//    {
		//        propertyInfo = GetProperty(typeInfo.BaseType.GetTypeInfo(), propertyName);
		//    }
		//    return propertyInfo;
		//}
	}

	//public class FacebookProfile
	//{
	//	public string Name { get; set; }
	//	public Picture Picture { get; set; }
	//	public string Locale { get; set; }
	//	public string Link { get; set; }
	//	public Cover Cover { get; set; }
	//	[JsonProperty("age_range")]
	//	public AgeRange AgeRange { get; set; }
	//	public Device[] Devices { get; set; }
	//	[JsonProperty("first_name")]
	//	public string FirstName { get; set; }
	//	[JsonProperty("last_name")]
	//	public string LastName { get; set; }
	//	public string Gender { get; set; }
	//	public bool IsVerified { get; set; }
	//	public string Id { get; set; }
	//	public string email { get; set; }
	//	public string mobile { get; set; }
	//	public string access_token { get; set; }
	//	public string refresh_token { get; set; }
	//}

	//public class Picture
	//{
	//	public Data Data { get; set; }
	//}

	//public class Data
	//{
	//	public bool IsSilhouette { get; set; }
	//	public string Url { get; set; }
	//}

	//public class Cover
	//{
	//	public string Id { get; set; }
	//	public int OffsetY { get; set; }
	//	public string Source { get; set; }
	//}

	//public class AgeRange
	//{
	//	public int Min { get; set; }
	//}

	//public class Device
	//{
	//	public string Os { get; set; }
	//}

	public class Datum
	{
		public string Access_Token { get; set; }
		public string Category { get; set; }
		public string Name { get; set; }
		public string Id { get; set; }
		public List<string> Perms { get; set; }
	}

	public class Cursors
	{
		public string Before { get; set; }
		public string After { get; set; }
	}

	public class Paging
	{
		public Cursors Cursors { get; set; }
	}

	public class Accounts
	{
		public List<Datum> Data { get; set; }
		public Paging Paging { get; set; }
	}

	public class Data
	{
		public bool Issilhouette { get; set; }
		public string Url { get; set; }
	}

	public class Picture
	{
		public Data Data { get; set; }
	}

	public class FacebookProfileData
	{
		public string Name { get; set; }
		public string Birthday { get; set; }
		public Accounts Accounts { get; set; }
		public string First_Name { get; set; }
		public string Last_Name { get; set; }
		public string Locale { get; set; }
		public Picture Picture { get; set; }
		public string Email { get; set; }
		public string Gender { get; set; }
		public string Id { get; set; }
	}
}
