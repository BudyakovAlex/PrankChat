using System;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.Timer;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items
{
    public class CompetitionItemViewModel : BaseItemViewModel, IDisposable
    {
        private MvxSubscriptionToken _timerTickMessageToken;
        private readonly IMvxMessenger _mvxMessenger;

        public string Id { get; }
        public string Title { get; }
        public string Description { get; }
        public int PrizePool { get; }
        public CompetitionPhase Phase { get; }
        public int LikesCount { get; }
        public DateTime VoteTerm { get; }
        public DateTime NewTerm { get; }

        private TimeSpan? _nextPhaseCountdown;
        public TimeSpan? NextPhaseCountdown
        {
            get => _nextPhaseCountdown;
            set
            {
                if (SetProperty(ref _nextPhaseCountdown, value))
                {
                    RaisePropertyChanged(nameof(NextPhaseCountdown));
                }
            }
        }

        public CompetitionItemViewModel(IMvxMessenger mvxMessenger,
                                        string id,
                                        string title,
                                        string description,
                                        DateTime newTerm,
                                        DateTime voteTerm,
                                        int prizePool,
                                        CompetitionPhase phase,
                                        int likesCount)
        {
            _mvxMessenger = mvxMessenger;
            VoteTerm = voteTerm;
            NewTerm = newTerm;

            Title = title;
            Description = description;
            PrizePool = prizePool;
            Phase = phase;
            LikesCount = likesCount;
            Id = id;

            Subscribe();
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

        private void OnTimerTick(TimerTickMessage message)
        {
            switch (Phase)
            {
                case CompetitionPhase.New when NewTerm < DateTime.UtcNow:
                    NextPhaseCountdown = DateTime.UtcNow - NewTerm;
                    break;
                case CompetitionPhase.Voting when VoteTerm < DateTime.UtcNow:
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
