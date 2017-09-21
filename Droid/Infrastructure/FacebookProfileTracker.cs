using System;
using Xamarin.Facebook;

namespace Provider.Droid.Infrastructure
{
	public class FacebookProfileTracker : Xamarin.Facebook.ProfileTracker
	{
		public event EventHandler<Profile> mOnProfileChanged;

		protected override void OnCurrentProfileChanged(Profile oldProfile, Profile currentProfile)
		{
			mOnProfileChanged?.Invoke(this, currentProfile);
		}
	}
}
