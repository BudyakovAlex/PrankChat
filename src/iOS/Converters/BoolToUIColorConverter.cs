using System;
using MvvmCross.Converters;
using UIKit;

namespace PrankChat.Mobile.iOS.Converters
{
    public class BoolToUIColorConverter : MvxValueConverter
    {
        public const string Name = "BoolToUIColor";

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is bool boolValue))
            {
                return UIColor.Clear;
            }

            if (!(parameter is Tuple<UIColor, UIColor> tuple))
            {
                return value;
            }

            return boolValue ? tuple.Item1 : tuple.Item2;
        }
    }
}
