﻿using System;
using CoreGraphics;
using UIKit;

namespace PrankChat.Mobile.iOS.Extensions
{
    public static class UIImageExtensions
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

        public static UIImage ImageWithColor(this UIColor color, float width, float height)
        {
            return color.ImageWithColor(new CGSize(width, height));
        }
    }
}