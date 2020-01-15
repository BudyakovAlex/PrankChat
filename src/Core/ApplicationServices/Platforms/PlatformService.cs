using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.ApplicationServices.Platforms
{
    public class PlatformService : IPlatformService
    {
        public Task CopyTextAsync(string text)
        {
            return Clipboard.SetTextAsync(text);
        }

        public Task ShareUrlAsync(string title, string url)
        {
            return Share.RequestAsync(new ShareTextRequest
            {
                Uri = url,
                Title = title
            });
        }
    }
}
