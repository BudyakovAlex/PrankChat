using MvvmCross.Converters;
using System;
using System.Globalization;

namespace PrankChat.Mobile.Core.Converters
{
    public class BoolToStateConverter<TTo> : MvxValueConverter<bool, TTo>
    {
        private readonly TTo _trueValue;
        private readonly TTo _falseValue;

        public BoolToStateConverter(TTo trueValue, TTo falseValue)
        {
            _trueValue = trueValue;
            _falseValue = falseValue;
        }

        protected override TTo Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            return value ? _trueValue : _falseValue;
        }
    }
}