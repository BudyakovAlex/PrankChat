﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Platforms;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.Exceptions;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication.Items;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Publication
{
    public class PublicationsViewModel : BaseViewModel, IVideoListViewModel
    {
        private readonly IPlatformService _platformService;
        private readonly IVideoPlayerService _videoPlayerService;
        private readonly ISettingsService _settingsService;
        private readonly IMvxLog _mvxLog;
        private readonly Dictionary<string, DateFilterType> _dateFilterTypeTitleMap;

        private PublicationType _selectedPublicationType;

        public PublicationType SelectedPublicationType
        {
            get => _selectedPublicationType;
            set
            {
                SetProperty(ref _selectedPublicationType, value);
                LoadPublicationsCommand.ExecuteAsync().FireAndForget();
            }
        }

        private string _activeFilterName;

        public string ActiveFilterName
        {
            get => _activeFilterName;
            set
            {
                SetProperty(ref _activeFilterName, value);
                LoadPublicationsCommand.ExecuteAsync().FireAndForget();
            }
        }

        private int _currentlyPlayingItem;

        public int CurrentlyPlayingItem
        {
            get => _currentlyPlayingItem;
            set => SetProperty(ref _currentlyPlayingItem, value);
        }

        public MvxObservableCollection<PublicationItemViewModel> Items { get; } = new MvxObservableCollection<PublicationItemViewModel>();

        public MvxAsyncCommand OpenFilterCommand => new MvxAsyncCommand(OnOpenFilterAsync);

        public MvxAsyncCommand LoadPublicationsCommand => new MvxAsyncCommand(OnLoadPublicationsAsync);

        public PublicationsViewModel(INavigationService navigationService,
                                    IDialogService dialogService,
                                    IApiService apiService,
                                    IPlatformService platformService,
                                    IVideoPlayerService videoPlayerService,
                                    ISettingsService settingsService,
                                    IMvxLog mvxLog,
                                    IErrorHandleService errorHandleService)
            : base(navigationService, errorHandleService, apiService, dialogService)
        {
            _platformService = platformService;
            _videoPlayerService = videoPlayerService;
            _settingsService = settingsService;
            _mvxLog = mvxLog;

            _dateFilterTypeTitleMap = new Dictionary<string, DateFilterType>
            {
                { Resources.Publication_Tab_Filter_Day, DateFilterType.Day },
                { Resources.Publication_Tab_Filter_Week, DateFilterType.Week},
                { Resources.Publication_Tab_Filter_Month, DateFilterType.Month },
                { Resources.Publication_Tab_Filter_Quarter, DateFilterType.Quarter },
                { Resources.Publication_Tab_Filter_HalfYear, DateFilterType.HalfYear },
            };
        }

        public override Task Initialize()
        {
            ActiveFilterName = Resources.Publication_Tab_Filter_Month;
            return LoadPublicationsCommand.ExecuteAsync();
        }

        public override void ViewDisappearing()
        {
            _videoPlayerService.Pause();

            base.ViewDisappearing();
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();

            _videoPlayerService.Play();
        }

        private async Task OnOpenFilterAsync(CancellationToken arg)
        {
            var selectedFilter = await DialogService.ShowMenuDialogAsync(new[]
            {
                Resources.Publication_Tab_Filter_Day,
                Resources.Publication_Tab_Filter_Week,
                Resources.Publication_Tab_Filter_Month,
                Resources.Publication_Tab_Filter_Quarter,
                Resources.Publication_Tab_Filter_HalfYear
            });

            if (string.IsNullOrWhiteSpace(selectedFilter) || selectedFilter == Resources.Cancel)
                return;

            ActiveFilterName = selectedFilter;
        }

        private async Task OnLoadPublicationsAsync()
        {
            try
            {
                IsBusy = true;

                _dateFilterTypeTitleMap.TryGetValue(ActiveFilterName, out var dateFilterType);

                VideoMetadataBundleDataModel videoBundle = null;

                switch (SelectedPublicationType)
                {
                    case PublicationType.Popular:
                        videoBundle = await ApiService.GetPopularVideoFeedAsync(dateFilterType);
                        break;

                    case PublicationType.Actual:
                        videoBundle = await ApiService.GetActualVideoFeedAsync(dateFilterType);
                        break;

                    case PublicationType.MyFeedComplete:
                        if (_settingsService.User != null)
                            videoBundle = await ApiService.GetMyVideoFeedAsync(_settingsService.User.Id, SelectedPublicationType, dateFilterType);
                        break;
                }

                SetVideoList(videoBundle);
            }
            catch (Exception ex)
            {
                ErrorHandleService.HandleException(new UserVisibleException("Проблема с загрузкой публикаций."));
                _mvxLog.ErrorException($"[{nameof(PublicationsViewModel)}]", ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void SetVideoList(VideoMetadataBundleDataModel videoBundle)
        {
            if (videoBundle.Data == null)
                return;

            var publicationViewModels = videoBundle.Data.Select(publication =>
                new PublicationItemViewModel(
                    NavigationService,
                    DialogService,
                    _platformService,
                    _videoPlayerService,
                    ApiService,
                    ErrorHandleService,
                    publication.User?.Name,
                    publication.User?.Avatar,
                    publication.Id,
                    publication.Title,
                    publication.StreamUri,
                    publication.ViewsCount,
                    publication.CreatedAt.DateTime,
                    publication.LikesCount,
                    publication.ShareUri,
                    publication.IsLiked));

            var list = publicationViewModels.ToList();

            Items.SwitchTo(publicationViewModels);
        }
    }
}
