﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Platforms;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication.Items;
using PrankChat.Mobile.Core.Presentation.ViewModels.Shared;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Publication
{
    public class PublicationsViewModel : BaseViewModel, IVideoListViewModel
    {
        private readonly IPlatformService _platformService;
        private readonly IVideoPlayerService _videoPlayerService;
        private readonly ISettingsService _settingsService;
        private readonly IMvxLog _mvxLog;
        private readonly IMvxMessenger _mvxMessenger;
        private readonly Dictionary<DateFilterType, string> _dateFilterTypeTitleMap;

        private PublicationType _selectedPublicationType;
        public PublicationType SelectedPublicationType
        {
            get => _selectedPublicationType;
            set
            {
                if (SetProperty(ref _selectedPublicationType, value))
                {
                    RefreshDataCommand.Execute();
                }
            }
        }

        public PaginationViewModel Pagination { get; }

        public MvxInteraction ItemsChangedInteraction { get; }

        private string _activeFilterName;
        public string ActiveFilterName
        {
            get => _activeFilterName;
            set => SetProperty(ref _activeFilterName, value);
        }

        private DateFilterType _activeFilter;
        public DateFilterType ActiveFilter
        {
            get => _activeFilter;
            set
            {
                _activeFilter = value;
                if (_dateFilterTypeTitleMap.TryGetValue(_activeFilter, out var activeFilterName))
                {
                    ActiveFilterName = activeFilterName;
                }
            }
        }

        private int _currentlyPlayingItem;
        public int CurrentlyPlayingItem
        {
            get => _currentlyPlayingItem;
            set => SetProperty(ref _currentlyPlayingItem, value);
        }

        public MvxObservableCollection<PublicationItemViewModel> Items { get; } = new MvxObservableCollection<PublicationItemViewModel>();

        public MvxAsyncCommand OpenFilterCommand { get; }

        public MvxAsyncCommand RefreshDataCommand { get; }

        public PublicationsViewModel(INavigationService navigationService,
                                     IDialogService dialogService,
                                     IApiService apiService,
                                     IPlatformService platformService,
                                     IVideoPlayerService videoPlayerService,
                                     ISettingsService settingsService,
                                     IMvxLog mvxLog,
                                     IErrorHandleService errorHandleService,
                                     IMvxMessenger mvxMessenger)
            : base(navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
            _platformService = platformService;
            _videoPlayerService = videoPlayerService;
            _settingsService = settingsService;
            _mvxLog = mvxLog;
            _mvxMessenger = mvxMessenger;

            _dateFilterTypeTitleMap = new Dictionary<DateFilterType, string>
            {
                { DateFilterType.Day, Resources.Publication_Tab_Filter_Day },
                { DateFilterType.Week, Resources.Publication_Tab_Filter_Week },
                { DateFilterType.Month, Resources.Publication_Tab_Filter_Month },
                { DateFilterType.Quarter, Resources.Publication_Tab_Filter_Quarter },
                { DateFilterType.HalfYear, Resources.Publication_Tab_Filter_HalfYear },
            };

            ItemsChangedInteraction = new MvxInteraction();
            Pagination = new PaginationViewModel(OnLoadPublicationsAsync, Constants.Pagination.DefaultPaginationSize);

            OpenFilterCommand = new MvxAsyncCommand(OnOpenFilterAsync);
            RefreshDataCommand = new MvxAsyncCommand(RefreshDataAsync);
        }

        public override Task Initialize()
        {
            ActiveFilter = DateFilterType.Month;
            return Pagination.LoadMoreItemsCommand.ExecuteAsync();
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

        public override void ViewDestroy(bool viewFinishing = true)
        {
            foreach (var publicationItemViewModel in Items)
            {
                publicationItemViewModel.Dispose();
            }

            base.ViewDestroy(viewFinishing);
        }

        private Task RefreshDataAsync()
        {
            Pagination.Reset();
            return Pagination.LoadMoreItemsCommand.ExecuteAsync();
        }

        private async Task OnOpenFilterAsync(CancellationToken arg)
        {
            var parameters = _dateFilterTypeTitleMap.Values.ToArray();
            var selectedFilterName = await DialogService.ShowMenuDialogAsync(parameters, Resources.Cancel);

            if (string.IsNullOrWhiteSpace(selectedFilterName) || selectedFilterName == Resources.Cancel)
            {
                return;
            }

            ActiveFilter = _dateFilterTypeTitleMap.FirstOrDefault(x => x.Value == selectedFilterName).Key;
            await RefreshDataCommand.ExecuteAsync();
        }

        private async Task<int> OnLoadPublicationsAsync(int page, int pageSize)
        {
            try
            {
                IsBusy = true;

                PaginationModel<VideoDataModel> pageContainer = null;
                switch (SelectedPublicationType)
                {
                    case PublicationType.Popular:
                        pageContainer = await ApiService.GetPopularVideoFeedAsync(ActiveFilter, page, pageSize);
                        break;
                        
                    case PublicationType.Actual:
                        pageContainer = await ApiService.GetActualVideoFeedAsync(ActiveFilter, page, pageSize);
                       
                        break;

                    case PublicationType.MyVideosOfCreatedOrders:
                        if (!IsUserSessionInitialized)
                        {
                            await NavigationService.ShowLoginView();
                            return 0;
                        }

                        pageContainer = await ApiService.GetMyVideoFeedAsync(_settingsService.User.Id, SelectedPublicationType, page, pageSize, ActiveFilter);
                        break;
                }

                return SetVideoList(pageContainer, page);
            }
            catch (Exception ex)
            {
                ErrorHandleService.HandleException(ex);
                ErrorHandleService.LogError(this, "Error on publication list loading.");
                return 0;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private int SetVideoList(PaginationModel<VideoDataModel> videoBundle, int page)
        {
            Pagination.SetTotalItemsCount(videoBundle.TotalCount);
            var publicationViewModels = videoBundle.Items.Select(publication =>
                new PublicationItemViewModel(NavigationService,
                                             DialogService,
                                             _platformService,
                                             Mvx.IoCProvider.Resolve<IVideoPlayerService>(),
                                             ApiService,
                                             ErrorHandleService,
                                             _mvxMessenger,
                                             _settingsService,
                                             publication.User?.Name,
                                             publication.User?.Avatar,
                                             publication.Id,
                                             publication.Title,
                                             publication.Description,
                                             publication.StreamUri,
                                             publication.ViewsCount,
                                             publication.CreatedAt.DateTime,
                                             publication.LikesCount,
                                             publication.ShareUri,
                                             publication.IsLiked)).ToList();

            if (page > 1)
            {
                Items.AddRange(publicationViewModels);
            }
            else
            {
                Items.SwitchTo(publicationViewModels);
            }

            ItemsChangedInteraction.Raise();
            return publicationViewModels.Count;
        }
    }
}