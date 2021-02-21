using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Timer;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Managers.Competitions;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Providers;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Competition
{
    public class CompetitionsViewModel : BasePageViewModel
    {
        private readonly ICompetitionsManager _competitionsManager;
        private readonly IWalkthroughsProvider _walkthroughsProvider;

        public CompetitionsViewModel(ICompetitionsManager competitionsManager, IWalkthroughsProvider walkthroughsProvider)
        {
            _competitionsManager = competitionsManager;
            _walkthroughsProvider = walkthroughsProvider;

            LoadDataCommand = new MvxAsyncCommand(() => ExecutionStateWrapper.WrapAsync(LoadDataAsync, awaitWhenBusy: true));
            ShowWalkthrouthCommand = new MvxAsyncCommand(ShowWalkthrouthAsync);

            Messenger.SubscribeOnMainThread<ReloadCompetitionsMessage>((msg) => LoadDataCommand?.Execute()).DisposeWith(Disposables);
            Messenger.SubscribeOnMainThread<EnterForegroundMessage>((msg) => LoadDataCommand?.Execute()).DisposeWith(Disposables);

            Messenger.SubscribeOnMainThread<RefreshNotificationsMessage>(async (msg) => await NotificationBageViewModel.RefreshDataCommand.ExecuteAsync(null)).DisposeWith(Disposables);
            Messenger.Subscribe<TimerTickMessage>(OnTimerTick, MvxReference.Strong).DisposeWith(Disposables);
        }

        public IMvxAsyncCommand LoadDataCommand { get; }

        public IMvxAsyncCommand ShowWalkthrouthCommand { get; }

        public MvxObservableCollection<CompetitionsSectionViewModel> Items { get; set; } = new MvxObservableCollection<CompetitionsSectionViewModel>();

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
                if (DialogService.IsToastShown)
                {
                    return;
                }

                DialogService.ShowToast(Resources.No_Intentet_Connection, ToastType.Negative);
                return;
            }

            var competitionsPage = await _competitionsManager.GetCompetitionsAsync(1, 100);

            var sections = competitionsPage.Items.GroupBy(competition => competition.GetPhase())
                                                 .Select(group => new CompetitionsSectionViewModel(IsUserSessionInitialized, Messenger, NavigationService, group.Key, group.ToList()))
                                                 .OrderBy(item => item.Phase)
                                                 .ToList();
            Items.SwitchTo(sections);
        }
    }
}