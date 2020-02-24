using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.ExternalAuth
{
    public interface IExternalAuthService
    {
        void InitializeVkontakteAsync();  

        Task<string> LoginWithVkontakteAsync();
        
        void InitializeFacebookAsync();

        Task<string> LoginWithFacebookAsync();
    }
}
