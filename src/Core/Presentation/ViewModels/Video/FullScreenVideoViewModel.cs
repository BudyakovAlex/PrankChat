﻿using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Shared.Abstract;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Video
{
    public class FullScreenVideoViewModel : LikeableViewModel, IMvxViewModel<FullScreenVideoParameter>
    {
        private string _shareLink;

        public FullScreenVideoViewModel(INavigationService navigationService,
                                        IErrorHandleService errorHandleService,
                                        IApiService apiService,
                                        IDialogService dialogService,
                                        ISettingsService settingsService)
            : base(navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
            ShareCommand = new MvxAsyncCommand(() => DialogService.ShowShareDialogAsync(_shareLink));
        }

        public MvxAsyncCommand ShareCommand { get; }

        private string _videoUrl;
        public string VideoUrl
        {
            get => _videoUrl;
            private set => SetProperty(ref _videoUrl, value);
        }

        private bool _isMuted;
        public bool IsMuted
        {
            get => _isMuted;
            set => SetProperty(ref _isMuted, value);
        }

        public string VideoName { get; private set; }

        public string Description { get; private set; }

        public string NumberOfLikesPresentation => NumberOfLikes.ToCountViewsString();

        public void Prepare(FullScreenVideoParameter parameter)
        {
            VideoId = parameter.VideoId;
            VideoUrl = parameter.VideoUrl;
            VideoName = parameter.VideoName;
            Description = parameter.Description;
            _shareLink = parameter.ShareLink;
        }
    }
}