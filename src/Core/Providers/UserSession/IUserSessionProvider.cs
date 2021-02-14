using PrankChat.Mobile.Core.Models.Data;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Providers.UserSession
{
    public interface IUserSessionProvider
    {
        User User { get; set; }

        string PushToken { get; set; }

        bool IsPushTokenSend { get; set; }

        Task<string> GetAccessTokenAsync();

        Task SetAccessTokenAsync(string accessToken);

        bool IsDebugMode { get; }
    }
}
