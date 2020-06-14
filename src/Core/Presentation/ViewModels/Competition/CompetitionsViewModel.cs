﻿using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Providers;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Competition
{
    public class CompetitionsViewModel : BaseViewModel
    {
        private readonly IMvxMessenger _mvxMessenger;
        private readonly IWalkthroughsProvider _walkthroughsProvider;
        private MvxSubscriptionToken _reloadItemsSubscriptionToken;

        public CompetitionsViewModel(IMvxMessenger mvxMessenger,
                                     INavigationService navigationService,
                                     IErrorHandleService errorHandleService,
                                     IApiService apiService,
                                     IDialogService dialogService,
                                     ISettingsService settingsService,
                                     IWalkthroughsProvider walkthroughsProvider) : base(navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
            _mvxMessenger = mvxMessenger;
            _walkthroughsProvider = walkthroughsProvider;

            LoadDataCommand = new MvxAsyncCommand(LoadDataAsync);
            ShowWalkthrouthCommand = new MvxAsyncCommand(ShowWalkthrouthAsync);
        }

        public IMvxAsyncCommand LoadDataCommand { get; }

        public IMvxAsyncCommand ShowWalkthrouthCommand { get; }

        public MvxObservableCollection<CompetitionsSectionViewModel> Items { get; set; } = new MvxObservableCollection<CompetitionsSectionViewModel>();

        public override Task Initialize()
        {
            return LoadDataCommand.ExecuteAsync();
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
                                                     .Select(group => new CompetitionsSectionViewModel(IsUserSessionInitialized, _mvxMessenger, NavigationService, group.Key, group.ToList()))
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
            _reloadItemsSubscriptionToken = _mvxMessenger.SubscribeOnMainThread<ReloadCompetitionsMessage>(OnReloadData);
        }

        private void Unsubscription()
        {
            if (_reloadItemsSubscriptionToken != null)
            {
                _mvxMessenger.Unsubscribe<ReloadCompetitionsMessage>(_reloadItemsSubscriptionToken);
                _reloadItemsSubscriptionToken.Dispose();
            }
        }

        private void OnReloadData(ReloadCompetitionsMessage msg)
        {
            LoadDataCommand.ExecuteAsync();
        }
    }
}