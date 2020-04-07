using System;
using System.Globalization;
using MvvmCross.Converters;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;

namespace PrankChat.Mobile.Core.Converters
{
    public class CompetitionPhaseToTermTitleConverter : MvxValueConverter<CompetitionPhase, string>
    {
        protected override string Convert(CompetitionPhase value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case CompetitionPhase.New:
                    return Resources.Competitions_New_Term;
                case CompetitionPhase.Voting:
                    return Resources.Competitions_Voting_Term;
                case CompetitionPhase.Finished:
                    return Resources.Competitions_Finished_Term;
                default:
                    return string.Empty;
            }
        }
    }
}