using System.Threading.Tasks;
using Facebook.LoginKit;
using Foundation;

namespace PrankChat.Mobile.iOS.Services.Callbacks
{
    public class FacebookCallback
    {
        private FacebookCallback()
        {
        }

        public static FacebookCallback Instance { get; } = new FacebookCallback();

        public TaskCompletionSource<string> CompletionSource { get; set; }

        public void LoginManagerLoginHandler(LoginManagerLoginResult result, NSError error)
        {
            if (result.IsCancelled)
            {
                CompletionSource?.TrySetResult(null);
                return;
            }

            if (error != null)
            {
                CompletionSource?.TrySetResult(null);
                return;
            }

            CompletionSource?.TrySetResult(result.Token?.TokenString);
        }
    }
}