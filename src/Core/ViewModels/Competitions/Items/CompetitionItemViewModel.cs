using System;
using System.Threading.Tasks;
using System.Windows.Input;
using PrankChat.Mobile.Core.Commands;
using PrankChat.Mobile.Core.Common;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Messages;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.ViewModels.Abstract;
using PrankChat.Mobile.Core.ViewModels.Registration;

namespace PrankChat.Mobile.Core.ViewModels.Competitions.Items
{
    public class CompetitionItemViewModel : BaseViewModel, IDisposable
    {
        private readonly IDisposable _timerTickSubscription;

        public CompetitionItemViewModel(bool isUserSessionInitialized, Models.Data.Competition competition)
        {
            Competition = competition;

            _timerTickSubscription = SystemTimer.SubscribeToEvent(
                OnTimerTick,
                (timer, handler) => timer.TimerElapsed += handler,
                (timer, handler) => timer.TimerElapsed -= handler).DisposeWith(Disposables);

            RefreshCountDownTimer();
            ActionCommand = new MvxRestrictedAsyncCommand(ExecuteActionAsync, restrictedCanExecute: () => isUserSessionInitialized, handleFunc: ShowLoginAsync);
        }

        public Models.Data.Competition Competition { get; }

        public int Id => Competition.Id;

        public string Number => string.Format(Constants.Formats.NumberFormat, Competition.Id);

        public string Title => Competition.Title;

        public string Description => Competition.Description;

        public string HtmlContent => Competition.HtmlContent;

        public int PrizePool => Competition.PrizePool;

        public CompetitionPhase Phase => Competition.GetPhase();

        public int? LikesCount => Competition.LikesCount;

        public bool CanUploadVideo => Phase == CompetitionPhase.New && Competition.CanUploadVideo;

        public bool CanJoinToPaidCompetition => Competition.CanJoin;

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

        public bool IsModeration => Phase == CompetitionPhase.Moderation;

        public string LikesCountString => LikesCount.ToCountString();

        public string PrizePoolPresentation => string.Format(Constants.Formats.MoneyFormat, PrizePool);

        public string DaysText { get; } = Resources.Days;

        public string HoursText { get; } = Resources.Hours;

        public string MinutesText { get; } = Resources.Minutes;

        public string ActionButtonTitle => GetAcitonButtonTitle();

        public string? CustomerAvatarUrl => Competition.Customer?.Avatar;

        public string? CustomerShortName => Competition.Customer?.Login.ToShortenName();

        public bool IsCustomerAttached => Competition.Customer != null;

        private TimeSpan? _nextPhaseCountdown;
        public TimeSpan? NextPhaseCountdown
        {
            get => _nextPhaseCountdown;
            set => SetProperty(ref _nextPhaseCountdown, value);
        }

        public ICommand ActionCommand { get; }

        private async Task ExecuteActionAsync()
        {
            var shouldRefresh = await NavigationManager.NavigateAsync<CompetitionDetailsViewModel, Models.Data.Competition, bool>(Competition);
            if (!shouldRefresh)
            {
                return;
            }

            Messenger.Publish(new ReloadCompetitionsMessage(this));
        }

        private void OnTimerTick(object _, EventArgs __)
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

        private Task ShowLoginAsync()
        {
            return NavigationManager.NavigateAsync<LoginViewModel>();
        }

        private string GetAcitonButtonTitle() => Phase switch
        {
            CompetitionPhase.New => CanJoinToPaidCompetition ? Resources.Participate : Resources.Watch,
            CompetitionPhase.Finished => Resources.MoreDetails,
            _ => Resources.Watch
        };
    }
}