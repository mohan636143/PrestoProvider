using System;
namespace Provider.Infrastructure
{
	public static class ConfigData
	{

		//82534559216-i950lqgn6c50eghbassenkhhmg0tes0i.apps.googleusercontent.com : Android SigninClient ID
		//82534559216-93qaarvdgaio69hu81j9ub20tph6c1rb.apps.googleusercontent.com  : iOS SigninClient ID


		public static string AppName = "Share";
		public static string GoogleAuth = "ShareGoogle";
		public static string FacebookAuth = "ShareFacebook";


		// OAuth
		// For Google login, configure at https://console.developers.google.com/
		public static string iOSClientId = "82534559216-93qaarvdgaio69hu81j9ub20tph6c1rb.apps.googleusercontent.com";
		public static string AndroidClientId = "82534559216-i950lqgn6c50eghbassenkhhmg0tes0i.apps.googleusercontent.com";

		//For Facebook login
		public static string FacebookClientId = "413804695688278";

		// These values do not need changing
		public static string Scope = "https://www.googleapis.com/auth/userinfo.email";
		public static string AuthorizeUrl = "https://accounts.google.com/o/oauth2/auth";
		public static string AccessTokenUrl = "https://www.googleapis.com/oauth2/v4/token";
		public static string UserInfoUrl = "https://www.googleapis.com/oauth2/v2/userinfo";

		// Set these to reversed iOS/Android client ids, with :/oauth2redirect appended
		public static string iOSRedirectUrl = "com.googleusercontent.apps.82534559216-93qaarvdgaio69hu81j9ub20tph6c1rb:/oauthredirect";
		public static string AndroidRedirectUrl = "com.googleusercontent.apps.82534559216-i950lqgn6c50eghbassenkhhmg0tes0i:/oauthredirect";

		//public static OAuth2Authenticator Authenticator;
		//public static User CurrentUser;

	}
}
