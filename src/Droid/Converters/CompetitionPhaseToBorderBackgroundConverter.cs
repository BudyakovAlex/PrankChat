using System;
using System.Globalization;
using MvvmCross.Converters;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Droid.Converters
{
    public class CompetitionPhaseToBorderBackgroundConverter : MvxValueConverter<CompetitionPhase, int>
    {
        protected override int Convert(CompetitionPhase value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                CompetitionPhase.Voting => Resource.Color.competition_vote_border,
                CompetitionPhase.New => Resource.Color.competition_new_border,
                CompetitionPhase.Finished => Resource.Color.competition_finished_border,
                _ => 0,
            };
        }
    }
}