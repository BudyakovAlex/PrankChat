using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Competition
{
    public class CompetitionsSectionViewModel : BaseItemViewModel, IDisposable
    {
        private readonly IMvxMessenger _mvxMessenger;

        public CompetitionPhase Phase { get; }

        public List<CompetitionItemViewModel> Items { get; }

        public bool HasNavigationControls => Items.Count > 1;

        public CompetitionsSectionViewModel(IMvxMessenger mvxMessenger,
                                            CompetitionPhase phase,
                                            List<CompetitionApiModel> competitions)
        {
            _mvxMessenger = mvxMessenger;
            Phase = phase;
            Items = competitions.Select(ProduceItemViewModel).ToList();
        }

        private CompetitionItemViewModel ProduceItemViewModel(CompetitionApiModel competition)
        {
            return new CompetitionItemViewModel(_mvxMessenger,
                                                competition.Id,
                                                competition.Title,
                                                competition.Description,
                                                competition.NewTerm,
                                                competition.VoteTerm,
                                                competition.PrizePool,
                                                Phase,
                                                competition.ImageUrl,
                                                competition.LikesCount);
        }

        public void Dispose()
        {
            Items.ForEach(item => item.Dispose());
        }
    }
}