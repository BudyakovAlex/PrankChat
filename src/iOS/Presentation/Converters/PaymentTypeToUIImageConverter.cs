using System;
using System.Globalization;
using MvvmCross.Converters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile.Cashbox;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Converters
{
    public class PaymentTypeToUIImageConverter : MvxValueConverter<PaymentType, UIImage>
    {
        protected override UIImage Convert(PaymentType value, Type targetType, object parameter, CultureInfo culture)
        {
            switch(value)
            {
                case PaymentType.Alphabank:
                    return GetIcon("ic_payment_alphabank");
                case PaymentType.Card:
                    return GetIcon("ic_payment_cards");
                case PaymentType.Phone:
                    return GetIcon("ic_payment_phone");
                case PaymentType.Qiwi:
                    return GetIcon("ic_payment_qiwi");
                case PaymentType.Sberbank:
                    return GetIcon("ic_payment_sberbank");
                case PaymentType.YandexMoney:
                    return GetIcon("ic_payment_yandexmoney");
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
