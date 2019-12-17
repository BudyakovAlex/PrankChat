using System;
using System.Globalization;
using MvvmCross.Converters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile.Cashbox;
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
                    return value ? GetIcon("ic_payment_alphabank_selected") : GetIcon("ic_payment_alphabank");
                case PaymentType.Card:
                    return value ? GetIcon("ic_payment_cards_selected") : GetIcon("ic_payment_cards");
                case PaymentType.Phone:
                    return value ? GetIcon("ic_payment_phone_selected") : GetIcon("ic_payment_phone");
                case PaymentType.Qiwi:
                    return value ? GetIcon("ic_payment_qiwi_selected") : GetIcon("ic_payment_qiwi");
                case PaymentType.Sberbank:
                    return value ? GetIcon("ic_payment_sberbank_selected") : GetIcon("ic_payment_sberbank");
                case PaymentType.YandexMoney:
                    return value ? GetIcon("ic_payment_yandexmoney_selected") : GetIcon("ic_payment_yandexmoney");
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
