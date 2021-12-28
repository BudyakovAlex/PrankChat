using System.Threading.Tasks;
using PrankChat.Mobile.Core.Managers.Competitions;
using PrankChat.Mobile.Core.ViewModels.Abstract;

namespace PrankChat.Mobile.Core.ViewModels.Competition
{
    public class CompetitonStatitsticsViewModel : BasePageViewModel<int>
    {
        private readonly ICompetitionsManager _competitionsManager;

        private int _competitionId;

        public CompetitonStatitsticsViewModel(ICompetitionsManager competitionsManager)
        {
            _competitionsManager = competitionsManager;
        }

        public decimal Profit { get; private set; }

        public int PercentageFromContribution { get; private set; }

        public decimal Contribution { get; private set; }

        public int Participants { get; private set; }

        public override void Prepare(int parameter)
        {
            _competitionId = parameter;
        }

        public override async Task InitializeAsync()
        {
            var statistics = await _competitionsManager.GetCompetitionStatisticsAsync(_competitionId);

            Profit = statistics.Profit;
            PercentageFromContribution = statistics.PercentageFromContribution;
            Contribution = statistics.Contribution;
            Participants = statistics.Participants;

            _ = RaiseAllPropertiesChanged();
        }
    }
}
