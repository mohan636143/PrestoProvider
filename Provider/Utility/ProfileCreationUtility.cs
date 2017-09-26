using System;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms;
namespace Provider.Utility
{
    public static class ProfileCreationUtility
    {
        public static UserProfile ProfileData{get;set;}
    }

    public class UserProfile
    {
        //Basic data
        public ImageSource ProfilePic { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public string Location { get; set; }
        //Skills Page
        public string Channel { get; set; }
        public string Description { get; set; }
        public string Skills { get; set; }
        public string Awards { get; set; }
    }
}
