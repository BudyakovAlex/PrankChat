using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Competition
{
    public class CompetitionPrizePoolViewModel : BaseViewModel, IMvxViewModel<CompetitionDataModel>
    {
        private CompetitionDataModel _competition;

        public CompetitionPrizePoolViewModel()
        {
            Items = new MvxObservableCollection<CompetitionPrizePoolItemViewModel>();

            RefreshCommand = new MvxAsyncCommand(RefreshAsync);
        }

        public string PrizePool { get; private set; }

        public IMvxAsyncCommand RefreshCommand { get; }

        public MvxObservableCollection<CompetitionPrizePoolItemViewModel> Items { get; }

        public void Prepare(CompetitionDataModel parameter)
        {
            _competition = parameter;
            PrizePool = string.Format(Constants.Formats.MoneyFormat, parameter.PrizePool);
        }

        public override Task Initialize()
        {
            return RefreshAsync();
        }

        private async Task RefreshAsync()
        {
            IsBusy = true;

            var competitionRatings = _competition.GetPhase() == CompetitionPhase.Finished
                ? await ApiService.GetCompetitionResultsAsync(_competition.Id)
                : await ApiService.GetCompetitionRatingsAsync(_competition.Id);

            var items = ProducePrizePoolItems(competitionRatings);
            Items.SwitchTo(items);
            IsBusy = false;
        }

        private IEnumerable<CompetitionPrizePoolItemViewModel> ProducePrizePoolItems(List<CompetitionResultDataModel> results)
        {
            var isNewCompetition = _competition.GetPhase() == CompetitionPhase.New;
            var items = results.OrderBy(item => item.Place).Take(10).ToList();
            foreach (var item in items)
            {
                var ratingString = item?.Video?.LikesCount.ToString() ?? "-";
                var rating = isNewCompetition ? "-" : ratingString;
                var participant = isNewCompetition ? "-" : item?.User?.Login ?? "-";
                var position = item.Place >= 10 ? "#" : $"{item.Place}";
                var isMyPosition = item.User?.Id == SettingsService.User?.Id;

                yield return new CompetitionPrizePoolItemViewModel(rating,
                                                                   participant,
                                                                   position,
                                                                   string.Format(Constants.Formats.MoneyFormat, item.Prize),
                                                                   isMyPosition);
            }
        }
    }
}