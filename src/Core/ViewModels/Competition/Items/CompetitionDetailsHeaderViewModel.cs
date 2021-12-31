using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Localization;

namespace PrankChat.Mobile.Core.ViewModels.Competition.Items
{
    public class CompetitionDetailsHeaderViewModel : CompetitionItemViewModel
    {
        public CompetitionDetailsHeaderViewModel(
            bool isUserSessionInitialized,
            IMvxAsyncCommand actionCommand,
            IMvxAsyncCommand deleteCommand,
            IMvxAsyncCommand openStatisticsCommand,
            Models.Data.Competition competition,
            bool isMyCompetition) : base(isUserSessionInitialized, competition)
        {
            ActionCommand = actionCommand;
            DeleteCommand = deleteCommand;
            OpenStatisticsCommand = openStatisticsCommand;
            CanChangeCompetition = isUserSessionInitialized && isMyCompetition;

            OpenPrizePoolCommand = this.CreateCommand(OpenPrizePoolAsync);
            OpenRulesCommand = this.CreateCommand(OpenRulesAsync);
        }

        public ICommand OpenPrizePoolCommand { get; }

        public new ICommand ActionCommand { get; }

        public ICommand OpenRulesCommand { get; }

        public ICommand DeleteCommand { get; }

        public ICommand OpenStatisticsCommand { get; }

        public bool CanChangeCompetition { get; }

        public bool CanShowRules => !string.IsNullOrWhiteSpace(HtmlContent);

        public string ActionTitle => GetActionTitile();

        private string GetActionTitile()
        {
            if (Competition.CanJoin && Competition.EntryTax.HasValue)
            {
                return Resources.PayForParticipation;
            }

            return Resources.UploadVideo;
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