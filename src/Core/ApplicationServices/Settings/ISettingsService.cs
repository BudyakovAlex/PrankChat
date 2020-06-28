using System.Threading.Tasks;
using PrankChat.Mobile.Core.Models.Data;

namespace PrankChat.Mobile.Core.ApplicationServices.Settings
{
    public interface ISettingsService
    {
        UserDataModel User { get; set; }

        string PushToken { get; set; }

        bool IsPushTokenSend { get; set; }

        Task<string> GetAccessTokenAsync();

        Task SetAccessTokenAsync(string accessToken);

        bool IsDebugMode { get; }
    }
}
