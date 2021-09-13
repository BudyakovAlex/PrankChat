using System;
using System.Globalization;
using MvvmCross.Converters;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.iOS.Common;
using UIKit;

namespace PrankChat.Mobile.iOS.Converters
{
    public class BoolToUIImageConverter : MvxValueConverter<bool, UIImage>
    {
        protected override UIImage Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            var paymentType = (PaymentType)parameter;
            return paymentType switch
            {
                PaymentType.Alphabank => value ? GetIcon(ImageNames.IconPaymentAlphabankSelected) : GetIcon(ImageNames.IconPaymentAlphabank),
                PaymentType.Card => value ? GetIcon(ImageNames.IconPaymentСardsSelected) : GetIcon(ImageNames.IconPaymentСards),
                PaymentType.Phone => value ? GetIcon(ImageNames.IconPaymentPhoneSelected) : GetIcon(ImageNames.IconPaymentPhone),
                PaymentType.Qiwi => value ? GetIcon(ImageNames.IconPaymentQiwiSelected) : GetIcon(ImageNames.IconPaymentQiwi),
                PaymentType.Sberbank => value ? GetIcon(ImageNames.IconPaymentSberbankSelected) : GetIcon(ImageNames.IconPaymentSberbank),
                PaymentType.YandexMoney => value ? GetIcon(ImageNames.IconPaymentYandexmoneySelected) : GetIcon(ImageNames.IconPaymentYandexmoney),
                _ => new UIImage(),
            };
        }

        private UIImage GetIcon(string name)
        {
            return UIImage.FromBundle(name);
        }
    }
}
