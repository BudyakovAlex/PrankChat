using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items
{
    public class CompetitionDetailsHeaderViewModel : CompetitionItemViewModel
    {
        private readonly INavigationService _navigationService;

        private readonly CompetitionDataModel _competition;

        public ICommand OpenPrizePoolCommand { get; set; }

        public ICommand LoadVideoCommand { get; set; }

        public ICommand OpenRulesCommand { get; set; }

        private bool _canLoadVideo;
        public bool CanLoadVideo
        {
            get => _canLoadVideo;
            set => SetProperty(ref _canLoadVideo, value);
        }

        public bool CanShowRules => !string.IsNullOrWhiteSpace(HtmlContent);

        public CompetitionDetailsHeaderViewModel(bool isUserSessionInitialized,
                                                 IMvxMessenger mvxMessenger,
                                                 INavigationService navigationService,
                                                 IMvxAsyncCommand loadVideoCommand,
                                                 CompetitionDataModel competition) : base(isUserSessionInitialized, mvxMessenger, navigationService, competition)
        {
            _navigationService = navigationService;
            _competition = competition;

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
            return _navigationService.ShowCompetitionPrizePoolView(_competition);
        }
    }
}