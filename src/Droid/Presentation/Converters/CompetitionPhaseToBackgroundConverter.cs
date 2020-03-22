using System;
using System.Globalization;
using MvvmCross.Converters;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Droid.Presentation.Converters
{
    public class CompetitionPhaseToBackgroundConverter : MvxValueConverter<CompetitionPhase, int>
    {
        protected override int Convert(CompetitionPhase value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case CompetitionPhase.Voting:
                    return Resource.Color.competition_vote_background;
                case CompetitionPhase.New:
                    return Resource.Color.competition_new_background;
                case CompetitionPhase.Finished:
                    return Resource.Color.competition_finished_background;
                default:
                    return 0;
            }
        }
    }
}