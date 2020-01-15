using System;
using System.Globalization;
using MvvmCross.Converters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile.Cashbox;

namespace PrankChat.Mobile.Droid.Presentation.Converters
{
    public class BooleanToSoundResourceConverter : MvxValueConverter<bool, string>
    {
        protected override string Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            return value ? "ic_sound" : "ic_without_sound";
        }
    }
}
