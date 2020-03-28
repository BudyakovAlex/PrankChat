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

        public string Number => _competition.Number;

        public string Title => _competition.Title;

        public string Description => _competition.Description;

        public string HtmlContent => _competition.HtmlContent;

        public int PrizePool => _competition.PrizePool;

        public CompetitionPhase Phase => _competition.GetPhase();

        public int? LikesCount => _competition.LikesCount;

        public bool CanLoadVideo => Phase == CompetitionPhase.New && !_competition.HasLoadedVideo;

        public DateTime VoteTerm => _competition.VoteTerm;

        public DateTime NewTerm => _competition.NewTerm;

        public string Duration => $"{VoteTerm.ToString(Constants.Formats.DateTimeFormat)} - {NewTerm.ToString(Constants.Formats.DateTimeFormat)}";

        public string ImageUrl => _competition.ImageUrl;

        public bool IsFinished => Phase == CompetitionPhase.Finished;

        public string LikesCountString => LikesCount.ToCountString();

        public string PrizePoolPresentation => $"{PrizePool} ₽";

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
                case CompetitionPhase.New when NewTerm > DateTime.UtcNow:
                    NextPhaseCountdown = DateTime.UtcNow - NewTerm;
                    break;
                case CompetitionPhase.Voting when VoteTerm > DateTime.UtcNow:
                    NextPhaseCountdown = DateTime.UtcNow - VoteTerm;
                    break;
                default:
                    NextPhaseCountdown = TimeSpan.Zero;
                    Unsubsribe();
                    break;
            }
        }
    }
}
