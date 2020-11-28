using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Platforms
{
    //TODO: rename to platform provider
    public interface IPlatformService
    {
        Task ShareUrlAsync(string title, string url);

        Task CopyTextAsync(string text);
    }
}
