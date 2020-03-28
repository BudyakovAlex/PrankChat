using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Mediaes;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Competition
{
    public class CompetitionDetailsViewModel : BaseViewModel, IMvxViewModel<CompetitionApiModel>
    {
        private readonly IMvxMessenger _mvxMessenger;
        private readonly IMediaService _mediaService;

        private CompetitionApiModel _competition;
        private CompetitionDetailsHeaderViewModel _header;

        public MvxObservableCollection<BaseItemViewModel> Items { get; }

        public CompetitionDetailsViewModel(IMvxMessenger mvxMessenger,
                                           INavigationService navigationService,
                                           IErrorHandleService errorHandleService,
                                           IApiService apiService,
                                           IMediaService mediaService,
                                           IDialogService dialogService,
                                           ISettingsService settingsService) : base(navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
            _mvxMessenger = mvxMessenger;
            _mediaService = mediaService;

            Items = new MvxObservableCollection<BaseItemViewModel>();
        }

        public void Prepare(CompetitionApiModel parameter)
        {
            _competition = parameter;
            _header = new CompetitionDetailsHeaderViewModel(_mvxMessenger,
                                                            NavigationService,
                                                            new MvxAsyncCommand(LoadVideoAsync),
                                                            parameter);
            Items.Add(_header);
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

        private async Task LoadVideoAsync()
        {
            try
            {
                IsBusy = true;

                var file = await _mediaService.PickVideoAsync();
                if (file == null)
                {
                    return;
                }

                var video = await ApiService.SendVideoAsync(_competition.Id, file.Path, _competition.Title, _competition.Description);
                if (video != null)
                {
                    _competition.HasLoadedVideo = true;
                    Items.Add(new CompetitionVideoViewModel(video.StreamUri,
                                                            video.User.Name,
                                                            video.User.Avatar,
                                                            video.LikesCount,
                                                            video.ViewsCount,
                                                            video.CreatedAt.UtcDateTime,
                                                            video.IsLiked,
                                                            video.User.Id == SettingsService.User.Id,
                                                            false));

                    await _header.RaisePropertyChanged(nameof(_header.CanLoadVideo));
                }
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}