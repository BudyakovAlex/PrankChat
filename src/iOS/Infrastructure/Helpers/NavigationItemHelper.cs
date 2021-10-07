using System.Windows.Input;
using CoreGraphics;
using PrankChat.Mobile.iOS.Common;
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

        public static UIBarButtonItem CreateBarButton(string imageName, ICommand command, UIColor imageTintColor)
        {
            var barButtonItem = CreateBarButton(imageName, command);
            barButtonItem.Image = barButtonItem.Image.ApplyTintColor(imageTintColor);
            barButtonItem.TintColor = imageTintColor;
            return barButtonItem;
        }

        public static UIBarButtonItem CreateBarLogoButton()
        {
            var imageView = new UIButton(new CGRect(0, 0, 26, 30));
            imageView.SetImage(UIImage.FromBundle(ImageNames.IconLogo), UIControlState.Disabled);
            return new UIBarButtonItem(imageView)
            {
                Enabled = false
            };
        }
    }
}
