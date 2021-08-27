using System;
using System.Globalization;
using MvvmCross.Converters;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.iOS.Providers;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Converters
{
    public class BoolToUIImageConverter : MvxValueConverter<bool, UIImage>
    {
        protected override UIImage Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            var paymentType = (PaymentType)parameter;
            switch (paymentType)
            {
                case PaymentType.Alphabank:
                    return value ? GetIcon(ImagePathProvider.IconPaymentAlphabankSelected) : GetIcon(ImagePathProvider.IconPaymentAlphabank);
                case PaymentType.Card:
                    return value ? GetIcon(ImagePathProvider.IconPaymentСardsSelected) : GetIcon(ImagePathProvider.IconPaymentСards);
                case PaymentType.Phone:
                    return value ? GetIcon(ImagePathProvider.IconPaymentPhoneSelected) : GetIcon(ImagePathProvider.IconPaymentPhone);
                case PaymentType.Qiwi:
                    return value ? GetIcon(ImagePathProvider.IconPaymentQiwiSelected) : GetIcon(ImagePathProvider.IconPaymentQiwi);
                case PaymentType.Sberbank:
                    return value ? GetIcon(ImagePathProvider.IconPaymentSberbankSelected) : GetIcon(ImagePathProvider.IconPaymentSberbank);
                case PaymentType.YandexMoney:
                    return value ? GetIcon(ImagePathProvider.IconPaymentYandexmoneySelected) : GetIcon(ImagePathProvider.IconPaymentYandexmoney);
                default:
                    return new UIImage();
            }
        }

        private UIImage GetIcon(string name)
        {
            return UIImage.FromBundle(name);
        }
    }
}
