using System;
using Foundation;
using Provider.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;

[assembly: ExportRenderer(typeof(Xamarin.Forms.WebView), typeof(CustomWebViewRenderer))]
namespace Provider.iOS.Renderers
{
    public class CustomWebViewRenderer : WebViewRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            try
            {
                base.OnElementChanged(e);



                NSHttpCookieStorage storage = NSHttpCookieStorage.SharedStorage;
                foreach (NSHttpCookie cookie in storage.Cookies)
                {
                    if (cookie.Domain == ".facebook.com")
                    {
                        storage.DeleteCookie(cookie);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
