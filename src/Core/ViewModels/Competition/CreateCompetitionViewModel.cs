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
        }

        public IMvxAsyncCommand CreateCommand { get; }
        public IMvxAsyncCommand ShowWalkthrouthCommand { get; }
        public IMvxAsyncCommand ShowWalkthrouthSecretCommand { get; }

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
                SetProperty(ref _collectionBidsFrom, value);
                RaisePropertiesChanged(nameof(CollectionBidsFromString));
            }
        }

        public string CollectionBidsFromString => _collectionBidsFrom?.ToShortDateString();

        private DateTime? _collectionBidsTo;
        public DateTime? CollectionBidsTo
        {
            get => _collectionBidsTo;
            set
            {
                SetProperty(ref _collectionBidsTo, value);
                RaisePropertiesChanged(nameof(CollectionBidsToString));
            }
        }

        public string CollectionBidsToString => _collectionBidsTo?.ToShortDateString();

        private DateTime? _votingFrom;
        public DateTime? VotingFrom
        {
            get => _votingFrom;
            set
            {
                SetProperty(ref _votingFrom, value);
                RaisePropertiesChanged(nameof(VotingFromString));
            }
        }

        public string VotingFromString => _votingFrom?.ToShortDateString();

        private DateTime? _votingTo;
        public DateTime? VotingTo
        {
            get => _votingTo;
            set
            {
                SetProperty(ref _votingTo, value);
                RaisePropertiesChanged(nameof(VotingToString));
            }
        }

        public string VotingToString => _votingTo?.ToShortDateString();

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
    }
}

