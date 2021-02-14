using PrankChat.Mobile.Core.Models.Data;
using System.Threading.Tasks;

namespace PrankChat.Mobile.iOS.ApplicationServices.ExternalAuth.AppleSignIn
{
    public interface IAppleSignInService
    {
        Task<AppleAuth> LoginAsync();
    }
}
