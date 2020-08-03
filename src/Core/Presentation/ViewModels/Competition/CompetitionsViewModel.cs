using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
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
    public class CompetitionsViewModel : BaseViewModel
    {
        private readonly IWalkthroughsProvider _walkthroughsProvider;

        private MvxSubscriptionToken _reloadItemsSubscriptionToken;
        private MvxSubscriptionToken _tabChangedMessage;
        private MvxSubscriptionToken _enterForegroundMessage;

        public CompetitionsViewModel(IWalkthroughsProvider walkthroughsProvider)
        {
            _walkthroughsProvider = walkthroughsProvider;

            LoadDataCommand = new MvxAsyncCommand(LoadDataAsync);
            ShowWalkthrouthCommand = new MvxAsyncCommand(ShowWalkthrouthAsync);
        }

        public IMvxAsyncCommand LoadDataCommand { get; }

        public IMvxAsyncCommand ShowWalkthrouthCommand { get; }

        public MvxObservableCollection<CompetitionsSectionViewModel> Items { get; set; } = new MvxObservableCollection<CompetitionsSectionViewModel>();

        public override async Task Initialize()
        {
            await base.Initialize();
            await LoadDataCommand.ExecuteAsync();
        }

        public override void ViewCreated()
        {
            base.ViewCreated();
            Subscription();
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            Unsubscription();
            base.ViewDestroy(viewFinishing);
        }

        private Task ShowWalkthrouthAsync()
        {
            return _walkthroughsProvider.ShowWalthroughAsync<CompetitionsViewModel>();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                IsBusy = true;

                if (!Connectivity.NetworkAccess.HasConnection())
                {
                    if (DialogService.IsToastShown)
                    {
                        return;
                    }

                    DialogService.ShowToast(Resources.No_Intentet_Connection, ToastType.Negative);
                    return;
                }

                var competitionsPage = await ApiService.GetCompetitionsAsync(1, 100);

                var sections = competitionsPage.Items.GroupBy(competition => competition.GetPhase())
                                                     .Select(group => new CompetitionsSectionViewModel(IsUserSessionInitialized, Messenger, NavigationService, group.Key, group.ToList()))
                                                     .OrderBy(item => item.Phase)
                                                     .ToList();
                Items.SwitchTo(sections);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void Subscription()
        {
            _reloadItemsSubscriptionToken = Messenger.SubscribeOnMainThread<ReloadCompetitionsMessage>((msg) => LoadDataCommand?.Execute());
            _tabChangedMessage = Messenger.SubscribeOnMainThread<TabChangedMessage>(OnTabChangedMessage);
            _enterForegroundMessage = Messenger.SubscribeOnMainThread<EnterForegroundMessage>((msg) => LoadDataCommand?.Execute());

            SubscribeToNotificationsUpdates();
        }

        private void Unsubscription()
        {
            _reloadItemsSubscriptionToken?.Dispose();
            _tabChangedMessage?.Dispose();
            _enterForegroundMessage?.Dispose();

            UnsubscribeFromNotificationsUpdates();
        }

        private void OnTabChangedMessage(TabChangedMessage msg)
        {
            if (msg.TabType != MainTabType.Competitions)
            {
                return;
            }

            LoadDataCommand.Execute();
        }
    }
}