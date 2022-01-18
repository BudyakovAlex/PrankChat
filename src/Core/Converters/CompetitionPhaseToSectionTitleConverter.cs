﻿using System;
using System.Globalization;
using MvvmCross.Converters;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Core.Converters
{
    public class CompetitionPhaseToSectionTitleConverter : MvxValueConverter<CompetitionPhase, string>
    {
        protected override string Convert(CompetitionPhase value, Type targetType, object parameter, CultureInfo culture) => value switch
        {
            CompetitionPhase.New => Resources.New,
            CompetitionPhase.Voting => Resources.Voting,
            CompetitionPhase.Finished => Resources.Summary,
            CompetitionPhase.Moderation => Resources.Moderation,
            _ => string.Empty,
        };
    }
}