﻿using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.Timer;
using PrankChat.Mobile.Core.Commands;
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

        private MvxSubscriptionToken _timerTickMessageToken;

        public CompetitionDataModel Competition { get; }

        public int Id => Competition.Id;

        public string Number => string.Format(Constants.Formats.NumberFormat, Competition.Id);

        public string Title => Competition.Title;

        public string Description => Competition.Description;

        public string HtmlContent => Competition.HtmlContent;

        public int PrizePool => Competition.PrizePool;

        public CompetitionPhase Phase => Competition.GetPhase();

        public int? LikesCount => Competition.LikesCount;

        public bool IsLikesUnavailable => Competition.CanUploadVideo;

        public DateTime? VoteTo => Competition.VoteTo;

        public DateTime UploadVideoTo => Competition.UploadVideoTo;

        public DateTime ActiveTo => Competition.ActiveTo;

        public DateTime CreatedAt => Competition.CreatedAt;

        public string Duration => $"{CreatedAt.ToString(Constants.Formats.DateTimeFormat)} - {ActiveTo.ToString(Constants.Formats.DateTimeFormat)}";

        public string ImageUrl => Competition.ImageUrl;

        public bool IsFinished => Phase == CompetitionPhase.Finished;

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

        public CompetitionItemViewModel(bool isUserSessionInitialized,
                                        IMvxMessenger mvxMessenger,
                                        INavigationService navigationService,
                                        CompetitionDataModel competition)
        {
            _mvxMessenger = mvxMessenger;
            _navigationService = navigationService;
            Competition = competition;

            Subscribe();
            ActionCommand = new MvxRestrictedAsyncCommand(ExecuteActionAsync, restrictedCanExecute: () => isUserSessionInitialized, handleFunc: _navigationService.ShowLoginView);
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
            return _navigationService.ShowCompetitionDetailsView(Competition);
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
