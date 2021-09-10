using System;
using System.Globalization;
using MvvmCross.Converters;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.iOS.Converters
{
    public class CompetitionPhaseToHiddenConverter : MvxValueConverter<CompetitionPhase, bool>
    {
        protected override bool Convert(CompetitionPhase value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == CompetitionPhase.New;
        }
    }
}
