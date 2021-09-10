using System;
using System.Globalization;
using MvvmCross.Converters;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Converters
{
    public class CompetitionPhaseToActionButtonTitleConverter : MvxValueConverter<CompetitionPhase, string>
    {
        protected override string Convert(CompetitionPhase value, Type targetType, object parameter, CultureInfo culture) => value switch
        {
            CompetitionPhase.New => Resources.Participate,
            CompetitionPhase.Voting => Resources.Watch,
            CompetitionPhase.Finished => Resources.MoreDetails,
            _ => string.Empty,
        };   
    }
}