using UIKit;

namespace PrankChat.Mobile.iOS.Utils.Helpers
{
    public static class DeviceHelper
    {
        public static UIViewController GetCurrentViewController()
        {
            var viewController = UIApplication.SharedApplication.KeyWindow.RootViewController;
            while (viewController.PresentedViewController != null)
            {
                viewController = viewController.PresentedViewController;
            }

            return viewController;
        }
    }
}