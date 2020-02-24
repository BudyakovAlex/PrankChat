using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.ExternalAuth
{
    interface IExternalAuthService
    {
        void InitializeVkontakteAsync();
        Task<LoginResult> LoginWithVkontakteAsync();

        void InitializeFacebookAsync();
        Task<LoginResult> LoginWithFacebookAsync();
    }
}
