using System;
using Android.Content;
using AndroidX.Core.Content.Resources;
using PrankChat.Mobile.Core.Models.Enums;

namespace PrankChat.Mobile.Droid.Extensions
{
    public static class CompetitionPhaseExtensions
    {
        public static int GetPhaseTintColor(this CompetitionPhase phase, Context context) => phase switch
        {
            CompetitionPhase.New => ResourcesCompat.GetColor(context.Resources, Resource.Color.competition_new_border, null),
            CompetitionPhase.Voting => ResourcesCompat.GetColor(context.Resources, Resource.Color.competition_vote_border, null),
            CompetitionPhase.Finished => ResourcesCompat.GetColor(context.Resources, Resource.Color.competition_finished_border, null),
            CompetitionPhase.Moderation => ResourcesCompat.GetColor(context.Resources, Resource.Color.gray, null),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}