using System;
using System.Globalization;
using MvvmCross.Converters;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Converters
{
    public class CompetitionPhaseToTermTitleConverter : MvxValueConverter<CompetitionPhase, string>
    {
        protected override string Convert(CompetitionPhase value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                CompetitionPhase.New => Resources.Competitions_New_Term,
                CompetitionPhase.Voting => Resources.Competitions_Voting_Term,
                CompetitionPhase.Finished => Resources.Competitions_Finished_Term,
                _ => string.Empty,
            };
        }
    }
}