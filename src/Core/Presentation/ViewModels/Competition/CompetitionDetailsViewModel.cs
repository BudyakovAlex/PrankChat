﻿using System;
using System.Threading.Tasks;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Competition
{
    public class CompetitionDetailsViewModel : BaseViewModel, IMvxViewModel<CompetitionApiModel>
    {
        private CompetitionApiModel _competition;
        private readonly IMvxMessenger _mvxMessenger;

        public MvxObservableCollection<BaseItemViewModel> Items { get; }

        public CompetitionDetailsViewModel(IMvxMessenger mvxMessenger,
                                           INavigationService navigationService,
                                           IErrorHandleService errorHandleService,
                                           IApiService apiService,
                                           IDialogService dialogService,
                                           ISettingsService settingsService) : base(navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
            Items = new MvxObservableCollection<BaseItemViewModel>();
            _mvxMessenger = mvxMessenger;
        }

        public void Prepare(CompetitionApiModel parameter)
        {
            _competition = parameter;
            var header = new CompetitionDetailsHeaderViewModel(_mvxMessenger,
                                                               NavigationService,
                                                               parameter);
            Items.Add(header);
        }

        public override async Task Initialize()
        {
            //TODO: add loading logic
            Items.AddRange(new[]
            {
                new CompetitionVideoViewModel("http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4",
                                              "Test user",
                                              "https://gravatar.com/avatar/7758426a877b824026e74b99d0223158?s=400&d=robohash&r=x",
                                              "100",
                                              "100k",
                                              DateTime.Now,
                                              true,
                                              false,
                                              true),

                new CompetitionVideoViewModel("http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4",
                                              "Test user",
                                              "https://gravatar.com/avatar/7758426a877b824026e74b99d0223158?s=400&d=robohash&r=x",
                                              "100",
                                              "100k",
                                              DateTime.Now,
                                              false,
                                              true,
                                              true),

                 new CompetitionVideoViewModel("http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4",
                                              "Test user",
                                              "https://gravatar.com/avatar/7758426a877b824026e74b99d0223158?s=400&d=robohash&r=x",
                                              "100",
                                              "100k",
                                              DateTime.Now,
                                              false,
                                              false,
                                              false)
            });
        }
    }
}