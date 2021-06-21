using System;
using System.Globalization;
using MvvmCross.Converters;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Converters
{
    public class PublicationTypeConverter : MvxValueConverter<PublicationType, int>
    {
        protected override int Convert(PublicationType value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value;
        }

        protected override PublicationType ConvertBack(int value, Type targetType, object parameter, CultureInfo culture)
        {
            return (PublicationType)value;
        }
    }
}
