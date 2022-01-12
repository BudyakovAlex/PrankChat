﻿using System.Threading.Tasks;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Managers.Competitions;
using PrankChat.Mobile.Core.ViewModels.Abstract;

namespace PrankChat.Mobile.Core.ViewModels.Competitions
{
    public class CompetitonStatitsticsViewModel : BasePageViewModel<int>
    {
        private readonly ICompetitionsManager _competitionsManager;

        private int _competitionId;

        public CompetitonStatitsticsViewModel(ICompetitionsManager competitionsManager)
        {
            _competitionsManager = competitionsManager;
        }

        public string Profit { get; private set; }

        public string PercentageFromContribution { get; private set; }

        public string Contribution { get; private set; }

        public int Participants { get; private set; }

        public override void Prepare(int parameter)
        {
            _competitionId = parameter;
        }

        public override async Task InitializeAsync()
        {
            var statistics = await _competitionsManager.GetCompetitionStatisticsAsync(_competitionId);

            Profit = $"{statistics.Profit} {Resources.Currency.ToLower()}";
            PercentageFromContribution = $"{statistics.PercentageFromContribution}{Resources.Percent.ToLower()}";
            Contribution = $"{statistics.Contribution}{Resources.Currency.ToLower()}";
            Participants = statistics.Participants;

            _ = RaiseAllPropertiesChanged();
        }
    }
}
