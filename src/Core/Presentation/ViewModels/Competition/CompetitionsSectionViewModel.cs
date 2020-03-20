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
        public CompetitionPhase Phase { get; }
        public List<CompetitionItemViewModel> Items { get; }

        public CompetitionsSectionViewModel(IMvxMessenger mvxMessenger,
                                            CompetitionPhase phase,
                                            List<CompetitionApiModel> competitions)
        {
            Phase = phase;
            Items = competitions.Select(competition => new CompetitionItemViewModel(mvxMessenger,
                                                                                    competition.Id,
                                                                                    competition.Title,
                                                                                    competition.Description,
                                                                                    competition.NewTerm,
                                                                                    competition.VoteTerm,
                                                                                    competition.PrizePool,
                                                                                    phase,
                                                                                    competition.LikesCount)).ToList();
        }
        
        public void Dispose()
        {
            Items.ForEach(x => x.Dispose());
        }
    }
}
