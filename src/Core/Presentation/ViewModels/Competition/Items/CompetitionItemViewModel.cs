using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.Timer;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items
{
    public class CompetitionItemViewModel : BaseItemViewModel, IDisposable
    {
        private readonly IMvxMessenger _mvxMessenger;
        private readonly INavigationService _navigationService;
        private readonly CompetitionDataModel _competition;

        private MvxSubscriptionToken _timerTickMessageToken;

        public int Id => _competition.Id;

        public string Number => string.Format(Constants.Formats.NumberFormat, _competition.Id);

        public string Title => _competition.Title;

        public string Description => _competition.Description;

        public string HtmlContent => _competition.HtmlContent;

        public int PrizePool => _competition.PrizePool;

        public CompetitionPhase Phase => _competition.GetPhase();

        public int? LikesCount => _competition.LikesCount;

        public DateTime VoteTo => _competition.VoteTo;

        public DateTime UploadVideoTo => _competition.UploadVideoTo;

        public DateTime ActiveTo => _competition.ActiveTo;

        public DateTime CreatedAt => _competition.CreatedAt;

        public string Duration => $"{CreatedAt.ToString(Constants.Formats.DateTimeFormat)} - {ActiveTo.ToString(Constants.Formats.DateTimeFormat)}";

        public string ImageUrl => _competition.ImageUrl;

        public bool IsFinished => Phase == CompetitionPhase.Finished;

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

        public CompetitionItemViewModel(IMvxMessenger mvxMessenger,
                                        INavigationService navigationService,
                                        CompetitionDataModel competition)
        {
            _mvxMessenger = mvxMessenger;
            _navigationService = navigationService;
            _competition = competition;

            Subscribe();
            ActionCommand = new MvxAsyncCommand(ExecuteActionAsync);
        }

        public void Dispose()
        {
            Unsubsribe();
        }

        private void Subscribe()
        {
            _timerTickMessageToken = _mvxMessenger.Subscribe<TimerTickMessage>(OnTimerTick, MvxReference.Strong);
        }

        public void Unsubsribe()
        {
            if (_timerTickMessageToken != null)
            {
                _mvxMessenger.Unsubscribe<TimerTickMessage>(_timerTickMessageToken);
                _timerTickMessageToken = null;
            }
        }

        private Task ExecuteActionAsync()
        {
            return _navigationService.ShowCompetitionDetailsView(_competition);
        }

        private void OnTimerTick(TimerTickMessage message)
        {
            switch (Phase)
            {
                case CompetitionPhase.New when UploadVideoTo > DateTime.UtcNow:
                    NextPhaseCountdown = UploadVideoTo - DateTime.UtcNow;
                    break;

                case CompetitionPhase.Voting when VoteTo > DateTime.UtcNow:
                    NextPhaseCountdown = VoteTo - DateTime.UtcNow;
                    break;
                default:
                    NextPhaseCountdown = TimeSpan.Zero;
                    Unsubsribe();
                    break;
            }
        }
    }
}
