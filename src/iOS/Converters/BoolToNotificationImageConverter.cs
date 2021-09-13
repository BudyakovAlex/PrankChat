using MvvmCross.Converters;
using PrankChat.Mobile.iOS.Common;
using System;
using System.Globalization;
using UIKit;

namespace PrankChat.Mobile.iOS.Converters
{
    public class BoolToNotificationImageConverter : MvxValueConverter<bool, UIImage>
    {
        protected override UIImage Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            var imageName = value ? ImageNames.IconNotificationWithBage : ImageNames.IconNotification;
            return UIImage.FromBundle(imageName).ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
        }
    }
}