using System;
using System.Threading.Tasks;
using PrankChat.Mobile.Core.ApplicationServices.ExternalAuth;

namespace PrankChat.Mobile.iOS.ApplicationServices.ExternalAuth
{
    public class ExternalAuthService : IExternalAuthService
    {
        public Task<string> LoginWithFacebookAsync()
        {
            throw new NotImplementedException();
        }

        public Task<string> LoginWithVkontakteAsync()
        {
            throw new NotImplementedException();
        }

        public void LogoutFromFacebook()
        {
            throw new NotImplementedException();
        }

        public void LogoutFromVkontakte()
        {
            throw new NotImplementedException();
        }
    }
}