using System;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ApplicationServices.Platforms
{
    public interface IPlatformService
    {
        Task ShareUrlAsync(string title, string url);

        Task CopyTextAsync(string text);
    }
}
