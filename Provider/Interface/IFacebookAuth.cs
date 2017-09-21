using System;
using Provider.Infrastructure;
using Xamarin.Auth;

namespace Provider.Interface
{
    public interface IFacebookAuth
    {
        void GetAccDetails(AccTypes AuthType);

        event EventHandler<Account> OnLoginSucceded;
    }
}
