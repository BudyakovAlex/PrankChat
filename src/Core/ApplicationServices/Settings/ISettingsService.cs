using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Settings
{
    public interface ISettingsService
    {
        Task<string> GetAccessTokenAsync();

        Task SetAccessTokenAsync(string accessToken);
    }
}
