using System;
using System.Windows.Input;
using UIKit;

namespace PrankChat.Mobile.iOS.Infrastructure.Helpers
{
    public static class NavigationItemHelper
    {
        public static UIBarButtonItem CreateBarButton(string imageName, ICommand command)
        {
            var imageView = UIImage.FromBundle(imageName).ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
            return new UIBarButtonItem(imageView, UIBarButtonItemStyle.Plain,
                (sender, e) => command?.Execute(null));
        }
    }
}
