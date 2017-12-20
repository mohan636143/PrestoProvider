using System;
using System.IO;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Provider.UITests
{
    public class AppInitializer
    {
        public static IApp StartApp(Platform platform)
        {
            try
            {
                if (platform == Platform.Android)
                {
                    return ConfigureApp.Android.EnableLocalScreenshots().StartApp();
                }

                return ConfigureApp.iOS.EnableLocalScreenshots().StartApp();
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
