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
            switch (value)
            {
                case CompetitionPhase.Voting:
                    return Resource.Color.competition_vote_border;
                case CompetitionPhase.New:
                    return Resource.Color.competition_new_border;
                case CompetitionPhase.Finished:
                    return Resource.Color.competition_finished_border;
                default:
                    return 0;
            }
        }
    }
}