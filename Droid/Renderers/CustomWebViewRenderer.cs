using System;
using Android.Content;
using Android.OS;
using Android.Webkit;
using Provider.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AndroidWebView = Android.Webkit;

[assembly:ExportRenderer(typeof(Xamarin.Forms.WebView),typeof(CustomWebViewRenderer))]
namespace Provider.Droid.Renderers
{
    public class CustomWebViewRenderer : WebViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
		{
            try
            {
                base.OnElementChanged(e);
                AndroidWebView.WebView wV = Control as AndroidWebView.WebView;
                wV.ClearCache(true);
                wV.ClearHistory();
                wV.ClearFormData();
                ClearCookies();
            }
            catch (Exception ex)
            {

            }
        }

		public void ClearCookies()
		{

            if (Build.VERSION.SdkInt >= Build.VERSION_CODES.Lollipop)
			{
                CookieManager.Instance.RemoveAllCookies(null);
                CookieManager.Instance.Flush();
			}
			else
			{
                CookieSyncManager cookieSyncMngr = CookieSyncManager.CreateInstance(Android.App.Application.Context);
                cookieSyncMngr.StartSync();
                CookieManager cookieManager = CookieManager.Instance;
				cookieManager.RemoveAllCookie();
				cookieManager.RemoveSessionCookie();
				cookieSyncMngr.StopSync();
				cookieSyncMngr.Sync();
			}
		}

    }
}
