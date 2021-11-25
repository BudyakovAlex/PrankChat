using MvvmCross.Commands;
using PrankChat.Mobile.Core.Common;
using PrankChat.Mobile.Core.Data.Enums;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Managers.Orders;
using PrankChat.Mobile.Core.ViewModels.Abstract;
using PrankChat.Mobile.Core.ViewModels.Parameters;
using PrankChat.Mobile.Core.ViewModels.Profile;
using PrankChat.Mobile.Core.ViewModels.Profile.Cashbox;
using PrankChat.Mobile.Core.ViewModels.Results;
using PrankChat.Mobile.Core.ViewModels.Walthroughs;
using PrankChat.Mobile.Core.Providers;
using PrankChat.Mobile.Core.Providers.Configuration;
using System;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ViewModels.Competition
{
    public class CreateCompetitionViewModel : BasePageViewModel
    {
        private readonly IOrdersManager _ordersManager;
        private readonly IWalkthroughsProvider _walkthroughsProvider;
        private readonly IEnvironmentConfigurationProvider _environmentConfigurationProvider;

        public CreateCompetitionViewModel(
            IOrdersManager ordersManager,
            IWalkthroughsProvider walkthroughsProvider,
            IEnvironmentConfigurationProvider environmentConfigurationProvider)
        {
            _ordersManager = ordersManager;
            _walkthroughsProvider = walkthroughsProvider;
            _environmentConfigurationProvider = environmentConfigurationProvider;

            ShowWalkthrouthCommand = this.CreateCommand(ShowWalkthrouthAsync);
            ShowWalkthrouthSecretCommand = this.CreateCommand(ShowWalkthrouthSecretAsync);
            CreateCommand = this.CreateCommand(CreateAsync);
            SelectPeriodCollectionBidsFromCommand = this.CreateCommand(SelectPeriodCollectionBidsFromAsync);
            SelectPeriodCollectionBidsToCommand = this.CreateCommand(SelectPeriodCollectionBidsToAsync);
            SelectPeriodVotingFromCommand = this.CreateCommand(SelectPeriodVotingFromAsync);
            SelectPeriodVotingToCommand = this.CreateCommand(SelectPeriodVotingToAsync);
            ShowSettingTableParticipantsCommand = this.CreateCommand(ShowSettingTableParticipantsAsync);
        }

        public IMvxAsyncCommand CreateCommand { get; }
        public IMvxAsyncCommand ShowWalkthrouthCommand { get; }
        public IMvxAsyncCommand ShowWalkthrouthSecretCommand { get; }
        public IMvxAsyncCommand SelectPeriodCollectionBidsFromCommand { get; }
        public IMvxAsyncCommand SelectPeriodCollectionBidsToCommand { get; }
        public IMvxAsyncCommand SelectPeriodVotingFromCommand { get; }
        public IMvxAsyncCommand SelectPeriodVotingToCommand { get; }
        public IMvxAsyncCommand ShowSettingTableParticipantsCommand { get; }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private DateTime? _collectionBidsFrom;
        public DateTime? CollectionBidsFrom
        {
            get => _collectionBidsFrom;
            set
            {
                if (value == null)
                {
                    return;
                }

                SetProperty(ref _collectionBidsFrom, value);
            }
        }

        private DateTime? _collectionBidsTo;
        public DateTime? CollectionBidsTo
        {
            get => _collectionBidsTo;
            set
            {
                if (value == null)
                {
                    return;
                }

                SetProperty(ref _collectionBidsTo, value);
            }
        }

        private DateTime? _votingFrom;
        public DateTime? VotingFrom
        {
            get => _votingFrom;
            set
            {
                if (value == null)
                {
                    return;
                }

                SetProperty(ref _votingFrom, value);
            }
        }

        private DateTime? _votingTo;
        public DateTime? VotingTo
        {
            get => _votingTo;
            set
            {
                if (value == null)
                {
                    return;
                }

                SetProperty(ref _votingTo, value);
            }
        }

        private double? _prizePool;
        public double? PrizePool
        {
            get => _prizePool;
            set => SetProperty(ref _prizePool, value);
        }

        private double? _participationFee;
        public double? ParticipationFee
        {
            get => _participationFee;
            set => SetProperty(ref _participationFee, value);
        }

        private double? _percentParticipationFee;
        public double? PercentParticipationFee
        {
            get => _percentParticipationFee;
            set => SetProperty(ref _percentParticipationFee, value);
        }

        private bool _isExecutorHidden;
        public bool IsExecutorHidden
        {
            get => _isExecutorHidden;
            set => SetProperty(ref _isExecutorHidden, value);
        }

        private Task ShowWalkthrouthAsync() =>
            _walkthroughsProvider.ShowWalthroughAsync<CreateCompetitionViewModel>();

        private Task ShowWalkthrouthSecretAsync()
        {
            var parameters = new WalthroughNavigationParameter(Resources.SecretOrder, Resources.WalkthrouthCreateOrderSecretDescription);
            return NavigationManager.NavigateAsync<WalthroughViewModel, WalthroughNavigationParameter>(parameters);
        }

        private Task CreateAsync()
        {
            return Task.CompletedTask;
        }

        private Task SaveOrderAsync()
        {
            return Task.CompletedTask;
        }

        private async Task SelectPeriodCollectionBidsFromAsync()
        {
            CollectionBidsFrom = await UserInteraction.ShowDateDialogAsync();
        }

        private async Task SelectPeriodVotingToAsync()
        {
            VotingTo = await UserInteraction.ShowDateDialogAsync();
        }

        private async Task SelectPeriodVotingFromAsync()
        {
            VotingFrom = await UserInteraction.ShowDateDialogAsync();
        }

        private async Task SelectPeriodCollectionBidsToAsync()
        {
            CollectionBidsTo = await UserInteraction.ShowDateDialogAsync();
        }

        private async Task HandleLowBalanceExceptionAsync(Exception exception)
        {
            var canRefil = await UserInteraction.ShowConfirmAsync(exception.Message, Resources.Attention, Resources.Replenish, Resources.Cancel);
            if (!canRefil)
            {
                return;
            }

            var navigationParameter = new CashboxTypeNavigationParameter(CashboxType.Refill);
            await NavigationManager.NavigateAsync<CashboxViewModel, CashboxTypeNavigationParameter, bool>(navigationParameter);
        }

        private async Task HandleUnauthorizedAsync(Exception exception)
        {
            var canGoProfile = await UserInteraction.ShowConfirmAsync(exception.Message, Resources.Attention, Resources.Ok, Resources.Cancel);
            if (!canGoProfile)
            {
                return;
            }

            await NavigationManager.NavigateAsync<ProfileUpdateViewModel, ProfileUpdateResult>();
        }

        private Task ShowPrivacyPolicyAsync() =>
            Xamarin.Essentials.Browser.OpenAsync(RestConstants.PolicyEndpoint);

        private async Task ShowSettingTableParticipantsAsync()
        {
            await NavigationManager.NavigateAsync<SettingsTableParticipantsViewModel>();
        }
    }
}