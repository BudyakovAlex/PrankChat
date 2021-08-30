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
                    return value ? GetIcon(ImageNames.IconPaymentAlphabankSelected) : GetIcon(ImageNames.IconPaymentAlphabank);
                case PaymentType.Card:
                    return value ? GetIcon(ImageNames.IconPaymentСardsSelected) : GetIcon(ImageNames.IconPaymentСards);
                case PaymentType.Phone:
                    return value ? GetIcon(ImageNames.IconPaymentPhoneSelected) : GetIcon(ImageNames.IconPaymentPhone);
                case PaymentType.Qiwi:
                    return value ? GetIcon(ImageNames.IconPaymentQiwiSelected) : GetIcon(ImageNames.IconPaymentQiwi);
                case PaymentType.Sberbank:
                    return value ? GetIcon(ImageNames.IconPaymentSberbankSelected) : GetIcon(ImageNames.IconPaymentSberbank);
                case PaymentType.YandexMoney:
                    return value ? GetIcon(ImageNames.IconPaymentYandexmoneySelected) : GetIcon(ImageNames.IconPaymentYandexmoney);
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
