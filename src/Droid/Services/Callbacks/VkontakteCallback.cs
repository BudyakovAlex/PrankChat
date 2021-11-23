using System.Threading.Tasks;
using Android.App;
using Android.Content;
using VKontakte;

namespace PrankChat.Mobile.Droid.Services.Callback
{
    public class VkontakteCallback
    {
        private VkontakteCallback()
        {
        }

        public static VkontakteCallback Instance { get; } = new VkontakteCallback();

        public TaskCompletionSource<string> CompletionSource { get; set; }

        public async Task<bool> OnActivityResultAsync(int requestCode, Result resultCode, Intent data)
        {
            try
            {
                var token = await VKSdk.OnActivityResultAsync(requestCode, resultCode, data, out var isSuccededResult);
                if (!isSuccededResult)
                {
                    CompletionSource?.TrySetResult(null);
                    return false;
                }

                CompletionSource?.TrySetResult(token?.AccessToken);
                return true;
            }
            catch (VKException)
            {
                CompletionSource?.TrySetResult(null);
                return false;
            }
        }
    }
}