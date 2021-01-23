using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.Timer;
using PrankChat.Mobile.Core.Commands;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items
{
    public class CompetitionItemViewModel : BaseViewModel, IDisposable
    {
        private readonly IMvxMessenger _mvxMessenger;
        private readonly INavigationService _navigationService;
        private readonly IDisposable _timerTickSubscription;

        public CompetitionItemViewModel(bool isUserSessionInitialized,
                                        IMvxMessenger mvxMessenger,
                                        INavigationService navigationService,
                                        CompetitionDataModel competition)
        {
            _mvxMessenger = mvxMessenger;
            _navigationService = navigationService;
            Competition = competition;

            _timerTickSubscription = _mvxMessenger.Subscribe<TimerTickMessage>(OnTimerTick, MvxReference.Strong).DisposeWith(Disposables);

            RefreshCountDownTimer();
            ActionCommand = new MvxRestrictedAsyncCommand(ExecuteActionAsync, restrictedCanExecute: () => isUserSessionInitialized, handleFunc: _navigationService.ShowLoginView);
        }

        public CompetitionDataModel Competition { get; }

        public int Id => Competition.Id;

        public string Number => string.Format(Constants.Formats.NumberFormat, Competition.Id);

        public string Title => Competition.Title;

        public string Description => Competition.Description;

        public string HtmlContent => Competition.HtmlContent;

        public int PrizePool => Competition.PrizePool;

        public CompetitionPhase Phase => Competition.GetPhase();

        public int? LikesCount => Competition.LikesCount;

        public bool CanUploadVideo => Phase == CompetitionPhase.New && Competition.CanUploadVideo;

        public bool CanJoinToPaidCompetition => Phase == CompetitionPhase.New && Competition.Category.CheckIsPaidCompetitionOrder() && !Competition.IsPaidCompetitionMember;

        public bool CanExecuteActionVideo => CanJoinToPaidCompetition || CanUploadVideo;

        public DateTime? VoteTo => Competition.VoteTo;

        public DateTime? UploadVideoTo => Competition.UploadVideoTo;

        public DateTime? ActiveTo => Competition.ActiveTo;

        public DateTime? CreatedAt => Competition.CreatedAt;

        public string Duration => GetDurationString();

        public string ImageUrl => Competition.ImageUrl;

        public bool IsFinished => Phase == CompetitionPhase.Finished;

        public OrderCategory Category => Competition.Category ?? OrderCategory.Competition;

        public bool IsNew => Phase == CompetitionPhase.New;

        public string LikesCountString => LikesCount.ToCountString();

        public string PrizePoolPresentation => string.Format(Constants.Formats.MoneyFormat, PrizePool);

        public string DaysText { get; } = Resources.Count_Days;

        public string HoursText { get; } = Resources.Count_Hours;

        public string MinutesText { get; } = Resources.Count_Minutes;

        private TimeSpan? _nextPhaseCountdown;
        public TimeSpan? NextPhaseCountdown
        {
            get => _nextPhaseCountdown;
            set => SetProperty(ref _nextPhaseCountdown, value);
        }

        public ICommand ActionCommand { get; }

        private async Task ExecuteActionAsync()
        {
            var shouldRefresh = await _navigationService.ShowCompetitionDetailsView(Competition);
            if (!shouldRefresh)
            {
                return;
            }

            _mvxMessenger.Publish(new ReloadCompetitionsMessage(this));
        }

        private void OnTimerTick(TimerTickMessage message)
        {
            RefreshCountDownTimer();
        }

        private void RefreshCountDownTimer()
        {
            switch (Phase)
            {
                case CompetitionPhase.New when UploadVideoTo > DateTime.Now:
                    NextPhaseCountdown = UploadVideoTo - DateTime.Now;
                    break;

                case CompetitionPhase.Voting when VoteTo > DateTime.Now:
                    NextPhaseCountdown = VoteTo - DateTime.Now;
                    break;

                default:
                    NextPhaseCountdown = TimeSpan.Zero;
                    Disposables.Remove(_timerTickSubscription);
                    _timerTickSubscription?.Dispose();
                    break;
            }
        }

        private string GetDurationString()
        {
            var createdAt = CreatedAt is null ? string.Empty : CreatedAt.Value.ToString(Constants.Formats.DateTimeFormat);
            var activeTo = ActiveTo is null ? string.Empty : ActiveTo.Value.ToString(Constants.Formats.DateTimeFormat);
            return $"{createdAt} - {activeTo}";
        }
    }
}