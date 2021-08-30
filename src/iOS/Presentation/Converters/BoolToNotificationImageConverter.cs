using MvvmCross.Converters;
using PrankChat.Mobile.iOS.Providers;
using System;
using System.Globalization;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Converters
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