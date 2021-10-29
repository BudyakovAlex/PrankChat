using System;
using System.Globalization;
using MvvmCross.Converters;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Droid.Converters
{
    public class BooleanToResourcePathConverter : MvxValueConverter<bool, string>
    {
        protected override string Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is PaymentType paymentType)
            {
                switch (paymentType)
                {
                    case PaymentType.Alphabank:
                        return value ? "ic_payment_alphabank_selected" : "ic_payment_alphabank";

                    case PaymentType.Card:
                        return value ? "ic_payment_cards_selected" : "ic_payment_cards";

                    case PaymentType.Phone:
                        return value ? "ic_payment_phone_selected" : "ic_payment_phone";

                    case PaymentType.Qiwi:
                        return value ? "ic_payment_qiwi_selected" : "ic_payment_qiwi";

                    case PaymentType.Sberbank:
                        return value ? "ic_payment_sberbank_selected" : "ic_payment_sberbank";

                    case PaymentType.YandexMoney:
                        return value ? "ic_payment_yandexmoney_selected" : "ic_payment_yandexmoney";

                    default:
                        return string.Empty;
                }
            }

            return value ? "bg_payment_selected" : "bg_payment";
        }
    }
}
