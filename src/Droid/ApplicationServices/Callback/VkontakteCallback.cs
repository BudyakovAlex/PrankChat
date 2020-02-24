using System.Threading.Tasks;
using Android.App;
using Android.Content;
using VKontakte;

namespace PrankChat.Mobile.Droid.ApplicationServices.Callback
{
    public class VkontakteCallback
    {
        private VkontakteCallback()
        {
        }

        public static VkontakteCallback Instance { get; } = new VkontakteCallback();

        public TaskCompletionSource<string> CompletionSource { get; set; }

        public async Task OnActivityResultAsync(int requestCode, Result resultCode, Intent data)
        {
            var task = VKSdk.OnActivityResultAsync(requestCode, resultCode, data, out var isSuccededResult);
            if (!isSuccededResult)
            {
                CompletionSource?.TrySetResult(null);
            }

            try
            {
                var token = await task;
                CompletionSource?.TrySetResult(token.AccessToken);
            }
            catch (VKException)
            {
                CompletionSource?.TrySetResult(null);
            }
        }
    }
}