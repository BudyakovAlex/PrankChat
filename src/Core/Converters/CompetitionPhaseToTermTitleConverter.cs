using System;
using System.Globalization;
using MvvmCross.Converters;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Converters
{
    public class CompetitionPhaseToTermTitleConverter : MvxValueConverter<CompetitionPhase, string>
    {
        protected override string Convert(CompetitionPhase value, Type targetType, object parameter, CultureInfo culture) => value switch
        {
            CompetitionPhase.New => Resources.BeforeStartVoting,
            CompetitionPhase.Voting => Resources.UntilEndVoting,
            CompetitionPhase.Finished => Resources.CompetitionPeriod,
            _ => string.Empty,
        };
    }
}