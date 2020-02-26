using System.Threading.Tasks;
using Foundation;
using PrankChat.Mobile.iOS.Utils.Helpers;
using UIKit;
using VKontakte;
using VKontakte.Core;
using VKontakte.Views;

namespace PrankChat.Mobile.iOS.ApplicationServices.Callbacks
{
    public class VkontakteDelegate : NSObject, IVKSdkDelegate, IVKSdkUIDelegate
    {
        private VkontakteDelegate()
        {
        }

        public static VkontakteDelegate Instance { get; } = new VkontakteDelegate();

        public TaskCompletionSource<string> CompletionSource { get; set; }

        public void AccessAuthorizationFinished(VKAuthorizationResult result)
        {
            if (result?.Token == null)
            {
                CompletionSource?.TrySetResult(null);
                return;
            }

            CompletionSource?.TrySetResult(result.Token.AccessToken);
        }

        public void UserAuthorizationFailed()
        {
            CompletionSource?.TrySetResult(null);
        }

        public void ShouldPresentViewController(UIViewController controller)
        {
            var currentViewController = DeviceHelper.GetCurrentViewController();
            BeginInvokeOnMainThread(() => currentViewController?.PresentViewController(controller, true, null));
        }

        public void NeedCaptchaEnter(VKError captchaError)
        {
            var currentViewController = DeviceHelper.GetCurrentViewController();
            if (currentViewController is null)
            {
                return;
            }

            BeginInvokeOnMainThread(() => VKCaptchaViewController.Create(captchaError)
                                                                 .PresentIn(currentViewController));
        }
    }
}