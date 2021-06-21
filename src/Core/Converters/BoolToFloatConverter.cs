using System;
using MvvmCross.Converters;

namespace PrankChat.Mobile.Core.Converters
{
    public class BoolToFloatConverter : MvxValueConverter
    {
        public const string Name = "BoolToFloat";

        public override object Convert(
            object value,
            Type targetType,
            object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (!(value is bool boolValue))
            {
                return 0;
            }

            if (!(parameter is Tuple<float, float> tuple))
            {
                return value;
            }

            return boolValue ? tuple.Item1 : tuple.Item2;
        }
    }
}
