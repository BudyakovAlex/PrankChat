using System;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.Common;
using PrankChat.Mobile.Core.Data.Enums;
using PrankChat.Mobile.Core.Data.Models.Competitions;
using PrankChat.Mobile.Core.Exceptions;
using PrankChat.Mobile.Core.Exceptions.Network;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Managers.Competitions;
using PrankChat.Mobile.Core.Messages;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Providers;
using PrankChat.Mobile.Core.Services.ErrorHandling.Messages;
using PrankChat.Mobile.Core.ViewModels.Abstract;
using PrankChat.Mobile.Core.ViewModels.Competitions.Items;
using PrankChat.Mobile.Core.ViewModels.Parameters;
using PrankChat.Mobile.Core.ViewModels.Profile;
using PrankChat.Mobile.Core.ViewModels.Profile.Cashbox;
using PrankChat.Mobile.Core.ViewModels.Results;
using PrankChat.Mobile.Core.ViewModels.Walthroughs;

namespace PrankChat.Mobile.Core.ViewModels.Competitions
{
    public class CreateCompetitionViewModel : BasePageViewModel
    {
        private readonly ICompetitionsManager _competitionsManager;
        private readonly IWalkthroughsProvider _walkthroughsProvider;

        private PlaceTableParticipantsItemViewModel[] _places;

        private bool _isExecuting;

        public CreateCompetitionViewModel(ICompetitionsManager competitionsManager, IWalkthroughsProvider walkthroughsProvider)
        {
            _competitionsManager = competitionsManager;
            _walkthroughsProvider = walkthroughsProvider;

            _places = Array.Empty<PlaceTableParticipantsItemViewModel>();

            ShowWalkthrouthCommand = this.CreateCommand(ShowWalkthrouthAsync);
            ShowWalkthrouthSecretCommand = this.CreateCommand(ShowWalkthrouthSecretAsync);
            CreateCommand = this.CreateCommand(CreateAsync);
            SelectPeriodCollectionBidsFromCommand = this.CreateCommand(SelectPeriodCollectionBidsFromAsync);
            SelectPeriodCollectionBidsToCommand = this.CreateCommand(SelectPeriodCollectionBidsToAsync);
            SelectPeriodVotingToCommand = this.CreateCommand(SelectPeriodVotingToAsync);
            ShowSettingTableParticipantsCommand = this.CreateCommand(ShowSettingTableParticipantsAsync);
        }

        public IMvxAsyncCommand CreateCommand { get; }
        public IMvxAsyncCommand ShowWalkthrouthCommand { get; }
        public IMvxAsyncCommand ShowWalkthrouthSecretCommand { get; }
        public IMvxAsyncCommand SelectPeriodCollectionBidsFromCommand { get; }
        public IMvxAsyncCommand SelectPeriodCollectionBidsToCommand { get; }
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
            set => SetProperty(ref _collectionBidsFrom, value);
        }

        private DateTime? _collectionBidsTo;
        public DateTime? CollectionBidsTo
        {
            get => _collectionBidsTo;
            set => SetProperty(ref _collectionBidsTo, value);
        }

        private DateTime? _votingTo;
        public DateTime? VotingTo
        {
            get => _votingTo;
            set => SetProperty(ref _votingTo, value);
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

        public override void ViewAppeared()
        {
            base.ViewAppeared();

            if (!_walkthroughsProvider.CheckCanShowOnFirstLoad<CreateCompetitionViewModel>())
            {
                return;
            }

            _ = _walkthroughsProvider.ShowWalthroughAsync<CreateCompetitionViewModel>();
        }

        private Task ShowWalkthrouthAsync() =>
            _walkthroughsProvider.ShowWalthroughAsync<CreateCompetitionViewModel>();

        private Task ShowWalkthrouthSecretAsync()
        {
            var parameters = new WalthroughNavigationParameter(Resources.SecretContest, Resources.WalkthrouthCreateOrderSecretDescription);
            return NavigationManager.NavigateAsync<WalthroughViewModel, WalthroughNavigationParameter>(parameters);
        }

        private async Task CreateAsync()
        {
            if (_isExecuting)
            {
                return;
            }

            _isExecuting = true;

            try
            {
                var canCreate = await UserInteraction.ShowConfirmAsync(Resources.CreateCompetitionMessage, Resources.Attention, Resources.Create, Resources.Cancel);
                if (!canCreate)
                {
                    return;
                }

                await SaveCompetitionAsync();
            }
            catch (NetworkException ex) when (ex.InnerException is ProblemDetailsException problemDetails && problemDetails?.CodeError == Constants.ErrorCodes.LowBalance)
            {
                await HandleLowBalanceExceptionAsync(ex);
            }
            catch (NetworkException ex) when (ex.InnerException is ProblemDetailsException problemDetails && problemDetails?.CodeError == Constants.ErrorCodes.Unauthorized)
            {
                await HandleUnauthorizedAsync(ex);
            }
            catch (Exception ex)
            {
                ErrorHandleService.ResumeServerErrorsHandling();
                Messenger.Publish(new ServerErrorMessage(this, ex));
            }
            finally
            {
                ErrorHandleService.ResumeServerErrorsHandling();
                _isExecuting = false;
            }
        }

        private async Task SaveCompetitionAsync()
        {
            var category = IsExecutorHidden
                ? OrderCategory.PrivatePaidCompetition
                : OrderCategory.PaidCompetition;

            var prizePool = _places
                .Where(place => place.Percent.HasValue)
                .OrderBy(place => place.Place)
                .Select(place => place.Percent.Value)
                .ToArray();

            var competitionCreationForm = new CompetitionCreationForm(
                PrizePool,
                Name,
                Description,
                CollectionBidsFrom,
                CollectionBidsTo,
                VotingTo,
                prizePool,
                category,
                ParticipationFee,
                PercentParticipationFee);

            ErrorHandleService.SuspendServerErrorsHandling();
            var newCompetition = await _competitionsManager.CreateCompetitionAsync(competitionCreationForm);
            if (newCompetition != null)
            {
                Messenger.Publish(new ReloadCompetitionsMessage(this));
                await NavigationManager.NavigateAsync<CompetitionDetailsViewModel, Competition, bool>(newCompetition);
                await NavigationManager.CloseAsync(this);
                SetDefaultData();
                return;
            }

            await UserInteraction.ShowAlertAsync(Resources.ErrorUnexpectedServer);
        }

        private void SetDefaultData()
        {
            Name = string.Empty;
            Description = string.Empty;
            IsExecutorHidden = false;
            CollectionBidsFrom = null;
            CollectionBidsTo = null;
            VotingTo = null;
            PrizePool = null;
            _places = Array.Empty<PlaceTableParticipantsItemViewModel>();
        }

        private async Task SelectPeriodCollectionBidsFromAsync()
        {
            CollectionBidsFrom = await SelectDateTimeAsync(CollectionBidsFrom);
            RefreshRangesIfNeed();
        }

        private async Task SelectPeriodCollectionBidsToAsync()
        {
            CollectionBidsTo = await SelectDateTimeAsync(CollectionBidsTo);
            RefreshRangesIfNeed();
        }

        private async Task SelectPeriodVotingToAsync()
        {
            VotingTo = await SelectDateTimeAsync(VotingTo);
            RefreshRangesIfNeed();
        }

        private async Task<DateTime?> SelectDateTimeAsync(DateTime? dateTime)
        {
            var datePromptConfig = new DatePromptConfig
            {
                iOSPickerStyle = iOSPickerStyle.Wheels,
                SelectedDate = dateTime
            };

            var selectedDateResult = await UserDialogs.Instance.DatePromptAsync(datePromptConfig);
            if (!selectedDateResult.Ok)
            {
                return dateTime;
            }

            var selectedDateTime = selectedDateResult.SelectedDate;

            var timePromptConfig = new TimePromptConfig
            {
                iOSPickerStyle = iOSPickerStyle.Wheels,
                SelectedTime = dateTime?.TimeOfDay
            };

            var selectedTimeResult = await UserDialogs.Instance.TimePromptAsync(timePromptConfig);
            if (!selectedTimeResult.Ok)
            {
                return dateTime;
            }

            return selectedDateTime.Date.Add(selectedTimeResult.SelectedTime);
        }

        public void RefreshRangesIfNeed()
        {
            if (CollectionBidsFrom.HasValue &&
                CollectionBidsTo.HasValue &&
                CollectionBidsFrom > CollectionBidsTo)
            {
                CollectionBidsTo = CollectionBidsFrom?.Add(TimeSpan.FromMinutes(5));
            }

            if (CollectionBidsTo.HasValue &&
                VotingTo.HasValue &&
                CollectionBidsTo > VotingTo)
            {
                VotingTo = CollectionBidsTo?.Add(TimeSpan.FromMinutes(5));
            }
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

        private async Task ShowSettingTableParticipantsAsync()
        {
            var navigationParameters = new SettingsTableParticipantsNavigationParameter(_places, PrizePool);
            _places = await NavigationManager.NavigateAsync<SettingsTableParticipantsViewModel, SettingsTableParticipantsNavigationParameter, PlaceTableParticipantsItemViewModel[]>(navigationParameters);
        }
    }
}