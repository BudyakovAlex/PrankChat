using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Managers.Competitions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.ViewModels.Abstract;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Competition
{
    public class CompetitionPrizePoolViewModel : BasePageViewModel, IMvxViewModel<Models.Data.Competition>
    {
        private readonly ICompetitionsManager _competitionsManager;

        private Models.Data.Competition _competition;

        public CompetitionPrizePoolViewModel(ICompetitionsManager competitionsManager)
        {
            _competitionsManager = competitionsManager;
            Items = new MvxObservableCollection<CompetitionPrizePoolItemViewModel>();

            RefreshCommand = this.CreateCommand(RefreshAsync);
        }

        public string PrizePool { get; private set; }

        public IMvxAsyncCommand RefreshCommand { get; }

        public MvxObservableCollection<CompetitionPrizePoolItemViewModel> Items { get; }

        public void Prepare(Models.Data.Competition parameter)
        {
            _competition = parameter;
            PrizePool = string.Format(Constants.Formats.MoneyFormat, parameter.PrizePool);
        }

        public override Task InitializeAsync()
        {
            return RefreshAsync();
        }

        private async Task RefreshAsync()
        {
            var competitionRatings = _competition.GetPhase() == CompetitionPhase.Finished
                ? await _competitionsManager.GetCompetitionResultsAsync(_competition.Id)
                : await _competitionsManager.GetCompetitionRatingsAsync(_competition.Id);

            var items = ProducePrizePoolItems(competitionRatings);
            Items.SwitchTo(items);
        }

        private IEnumerable<CompetitionPrizePoolItemViewModel> ProducePrizePoolItems(List<CompetitionResult> results)
        {
            var isNewCompetition = _competition.GetPhase() == CompetitionPhase.New;
            var items = results.OrderBy(item => item.Place).Take(10).ToList();
            foreach (var item in items)
            {
                var ratingString = item?.Video?.LikesCount.ToString() ?? "-";
                var rating = isNewCompetition ? "-" : ratingString;
                var participant = isNewCompetition ? "-" : item?.User?.Login ?? "-";
                var position = item.Place >= 10 ? "#" : $"{item.Place}";
                var isMyPosition = item.User?.Id == UserSessionProvider.User?.Id;

                yield return new CompetitionPrizePoolItemViewModel(
                    rating,
                    participant,
                    position,
                    string.Format(Constants.Formats.MoneyFormat, item.Prize),
                    isMyPosition);
            }
        }
    }
}