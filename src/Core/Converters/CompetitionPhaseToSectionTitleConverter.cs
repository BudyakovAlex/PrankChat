using System;
using System.Globalization;
using MvvmCross.Converters;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;

namespace PrankChat.Mobile.Core.Converters
{
    public class CompetitionPhaseToSectionTitleConverter : MvxValueConverter<CompetitionPhase, string>
    {
        protected override string Convert(CompetitionPhase value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case CompetitionPhase.New:
                    return Resources.Competitions_New;
                case CompetitionPhase.Voting:
                    return Resources.Competitions_Voting;
                case CompetitionPhase.Finished:
                    return Resources.Competitions_Finished;
                default:
                    return string.Empty;
            }
        }
    }
}