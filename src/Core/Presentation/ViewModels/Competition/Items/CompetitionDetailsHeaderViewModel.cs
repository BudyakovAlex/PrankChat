using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Navigation;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items
{
    public class CompetitionDetailsHeaderViewModel : CompetitionItemViewModel
    {
        public CompetitionDetailsHeaderViewModel(bool isUserSessionInitialized,
                                                 IMvxMessenger mvxMessenger,
                                                 INavigationService navigationService,
                                                 IMvxAsyncCommand actionCommand,
                                                 Models.Data.Competition competition)
            : base(isUserSessionInitialized, mvxMessenger, navigationService, competition)
        {
            _navigationService = navigationService;

            ActionCommand = actionCommand;
            OpenPrizePoolCommand = new MvxAsyncCommand(OpenPrizePoolAsync);
            OpenRulesCommand = new MvxAsyncCommand(OpenRulesAsync);
        }

        private readonly INavigationService _navigationService;

        public ICommand OpenPrizePoolCommand { get; set; }

        public new ICommand ActionCommand { get; set; }

        public ICommand OpenRulesCommand { get; set; }

        public ICommand JoinCommand { get; set; }

        public bool CanShowRules => !string.IsNullOrWhiteSpace(HtmlContent);

        public string ActionTitle => GetActionTitile();

        private string GetActionTitile()
        {
            if (CanJoinToPaidCompetition)
            {
                return Resources.Competition_Pay_For_Join;
            }

            return Resources.Competition_Load_Video;
        }

        private Task OpenRulesAsync()
        {
            return _navigationService.ShowCompetitionRulesView(HtmlContent);
        }

        private Task OpenPrizePoolAsync()
        {
            return _navigationService.ShowCompetitionPrizePoolView(Competition);
        }
    }
}