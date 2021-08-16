using System;
using CoreGraphics;
using UIKit;

namespace PrankChat.Mobile.iOS.Utils.Extensions
{
    public static class UIImageUtil
    {
        public static UIImage ImageWithColor(this UIColor color, CGSize size)
        {
            var rect = new CGRect(0, 0, size.Width, size.Height);
            UIGraphics.BeginImageContext(rect.Size);
            var context = UIGraphics.GetCurrentContext();
            context.SetFillColor(color.CGColor);
            context.FillRect(rect);
            var image = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return image;
        }
    }
}