using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items
{
    public class CompetitionDetailsHeaderViewModel : CompetitionItemViewModel
    {
        private readonly INavigationService _navigationService;

        public ICommand OpenPrizePoolCommand { get; set; }

        public ICommand LoadVideoCommand { get; set; }

        public ICommand OpenRulesCommand { get; set; }

        public CompetitionDetailsHeaderViewModel(IMvxMessenger mvxMessenger,
                                                 INavigationService navigationService,
                                                 CompetitionApiModel competitionApiModel) : base(mvxMessenger, navigationService, competitionApiModel)
        {
            OpenPrizePoolCommand = new MvxAsyncCommand(OpenPrizePoolAsync);
            OpenRulesCommand = new MvxAsyncCommand(OpenRulesAsync);
            LoadVideoCommand = new MvxAsyncCommand(LoadVideoAsync);
            _navigationService = navigationService;
        }

        private async Task LoadVideoAsync()
        {
            //TODO: add logic here
        }

        private Task OpenRulesAsync()
        {
            return _navigationService.ShowCompetitionRulesView(HtmlContent);
        }

        private Task OpenPrizePoolAsync()
        {
            return _navigationService.ShowCompetitionPrizePoolView(Id);
        }
    }
}