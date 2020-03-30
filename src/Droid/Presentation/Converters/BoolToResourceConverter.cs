using System;
using MvvmCross.Converters;

namespace PrankChat.Mobile.Droid.Presentation.Converters
{
    public class BoolToResourceConverter : MvxValueConverter
    {
        public const string Name = "BoolToResource";

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            if (parameter == null)
            {
                return value;
            }

            if (value is bool result &&
                parameter is Tuple<int, int> resources)
            {
                return result ? resources.Item1 : resources.Item2;
            }

            return null;
        }
    }
}
