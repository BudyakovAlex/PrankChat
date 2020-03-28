using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.Mediaes;
using PrankChat.Mobile.Core.ApplicationServices.Network;
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
                                                 IMvxAsyncCommand loadVideoCommand,
                                                 CompetitionApiModel competitionApiModel) : base(mvxMessenger, navigationService, competitionApiModel)
        {
            _navigationService = navigationService;

            LoadVideoCommand = loadVideoCommand;
            OpenPrizePoolCommand = new MvxAsyncCommand(OpenPrizePoolAsync);
            OpenRulesCommand = new MvxAsyncCommand(OpenRulesAsync);
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