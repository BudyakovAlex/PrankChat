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
                                                 IMvxAsyncCommand actionCommand,
                                                 Models.Data.Competition competition)
            : base(isUserSessionInitialized, competition)
        {
            ActionCommand = actionCommand;
            OpenPrizePoolCommand = new MvxAsyncCommand(OpenPrizePoolAsync);
            OpenRulesCommand = new MvxAsyncCommand(OpenRulesAsync);
        }

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
            return NavigationManager.NavigateAsync<CompetitionRulesViewModel, string>(HtmlContent);
        }

        private Task OpenPrizePoolAsync()
        {
            return NavigationManager.NavigateAsync<CompetitionPrizePoolViewModel, Models.Data.Competition>(Competition);
        }
    }
}