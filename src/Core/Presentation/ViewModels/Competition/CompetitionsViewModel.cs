using MvvmCross.Commands;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Managers.Competitions;
using PrankChat.Mobile.Core.Messages;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.ViewModels.Abstract.Items;
using PrankChat.Mobile.Core.Providers;
using PrankChat.Mobile.Core.Wrappers;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Competition
{
    public class CompetitionsViewModel : BaseItemsPageViewModel<CompetitionsSectionViewModel>
    {
        private readonly ICompetitionsManager _competitionsManager;
        private readonly IWalkthroughsProvider _walkthroughsProvider;

        private ExecutionStateWrapper _loadDataStateWrapper;

        public CompetitionsViewModel(ICompetitionsManager competitionsManager, IWalkthroughsProvider walkthroughsProvider)
        {
            _competitionsManager = competitionsManager;
            _walkthroughsProvider = walkthroughsProvider;

            _loadDataStateWrapper = new ExecutionStateWrapper();
            _loadDataStateWrapper.SubscribeToEvent<ExecutionStateWrapper, bool>(
                OnIsBusyChanged,
                (wrapper, handler) => wrapper.IsBusyChanged += handler,
                (wrapper, handler) => wrapper.IsBusyChanged -= handler).DisposeWith(Disposables);

            LoadDataCommand = this.CreateCommand(() => _loadDataStateWrapper.WrapAsync(LoadDataAsync), useIsBusyWrapper : false);
            ShowWalkthrouthCommand = this.CreateCommand(ShowWalkthrouthAsync);

            Messenger.SubscribeOnMainThread<ReloadCompetitionsMessage>((msg) => LoadDataCommand?.Execute()).DisposeWith(Disposables);
        }

        public IMvxAsyncCommand LoadDataCommand { get; }

        public IMvxAsyncCommand ShowWalkthrouthCommand { get; }

        public override bool IsBusy => base.IsBusy || _loadDataStateWrapper.IsBusy;

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            _ = LoadDataCommand.ExecuteAsync();
        }

        private Task ShowWalkthrouthAsync()
        {
            return _walkthroughsProvider.ShowWalthroughAsync<CompetitionsViewModel>();
        }

        private async Task LoadDataAsync()
        {
            if (!Connectivity.NetworkAccess.HasConnection())
            {
                if (UserInteraction.IsToastShown)
                {
                    return;
                }

                UserInteraction.ShowToast(Resources.No_Intentet_Connection, ToastType.Negative);
                return;
            }

            var competitionsPage = await _competitionsManager.GetCompetitionsAsync(1, 100);

            var sections = competitionsPage.Items
                .GroupBy(competition => competition.GetPhase())
                .Select(group => new CompetitionsSectionViewModel(IsUserSessionInitialized, group.Key, group.ToList()))
                .OrderBy(item => item.Phase)
                .ToList();

            Items.SwitchTo(sections);
        }
    }
}