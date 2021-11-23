using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Services.ExternalAuth
{
    public interface IExternalAuthService
    {
        Task<string> LoginWithVkontakteAsync();
     
        Task<string> LoginWithFacebookAsync();

        void LogoutFromVkontakte();

        void LogoutFromFacebook();
    }
}