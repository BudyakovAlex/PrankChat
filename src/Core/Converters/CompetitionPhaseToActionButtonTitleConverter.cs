using System;
using System.Globalization;
using MvvmCross.Converters;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;

namespace PrankChat.Mobile.Core.Converters
{
    public class CompetitionPhaseToActionButtonTitleConverter : MvxValueConverter<CompetitionPhase, string>
    {
        protected override string Convert(CompetitionPhase value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case CompetitionPhase.New:
                    return Resources.Competitions_Participate;
                case CompetitionPhase.Voting:
                    return Resources.Competitions_See;
                case CompetitionPhase.Finished:
                    return Resources.Competitions_More;
                default:
                    return string.Empty;
            }
        }
    }
}