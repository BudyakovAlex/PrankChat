using MvvmCross.Converters;
using System;
using System.Globalization;

namespace PrankChat.Mobile.Core.Converters
{
    public class DelegateConverter<TFrom, TTo> : MvxValueConverter<TFrom, TTo>
    {
        private readonly Func<TFrom, TTo> _convertFunc;

        public DelegateConverter(Func<TFrom, TTo> convertFunc)
        {
            _convertFunc = convertFunc;
        }

        protected override TTo Convert(TFrom value, Type targetType, object parameter, CultureInfo culture)
        {
            if (_convertFunc is null)
            {
                return default;
            }

            return _convertFunc.Invoke(value);
        }
    }
}
