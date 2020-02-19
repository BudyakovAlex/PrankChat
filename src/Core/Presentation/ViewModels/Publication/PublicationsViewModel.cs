using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
using PrankChat.Mobile.Core.Exceptions;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication.Items;

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
                SetProperty(ref _selectedPublicationType, value);
                LoadPublicationsCommand.ExecuteAsync().FireAndForget();
            }
        }

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

        public MvxAsyncCommand OpenFilterCommand => new MvxAsyncCommand(OnOpenFilterAsync);

        public MvxAsyncCommand LoadPublicationsCommand => new MvxAsyncCommand(OnLoadPublicationsAsync);

        public PublicationsViewModel(INavigationService navigationService,
                                    IDialogService dialogService,
                                    IApiService apiService,
                                    IPlatformService platformService,
                                    IVideoPlayerService videoPlayerService,
                                    ISettingsService settingsService,
                                    IMvxLog mvxLog,
                                    IErrorHandleService errorHandleService,
                                    IMvxMessenger mvxMessenger)
            : base(navigationService, errorHandleService, apiService, dialogService)
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
        }

        public override Task Initialize()
        {
            ActiveFilter = DateFilterType.Month;
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
            var parametres = _dateFilterTypeTitleMap.Values.ToArray();
            var selectedFilterName = await DialogService.ShowMenuDialogAsync(parametres, Resources.Cancel);

            if (string.IsNullOrWhiteSpace(selectedFilterName) || selectedFilterName == Resources.Cancel)
                return;

            ActiveFilter = _dateFilterTypeTitleMap.FirstOrDefault(x => x.Value == selectedFilterName).Key;
            await LoadPublicationsCommand.ExecuteAsync();
        }

        private async Task OnLoadPublicationsAsync()
        {
            try
            {
                IsBusy = true;

                switch (SelectedPublicationType)
                {
                    case PublicationType.Popular:
                        var videos = await ApiService.GetPopularVideoFeedAsync(ActiveFilter);
                        SetVideoList(videos);
                        break;

                    case PublicationType.Actual:
                        videos = await ApiService.GetActualVideoFeedAsync(ActiveFilter);
                        SetVideoList(videos);
                        break;

                    case PublicationType.MyVideosOfCreatedOrders:
                        videos = await ApiService.GetMyVideoFeedAsync(_settingsService.User.Id, SelectedPublicationType, ActiveFilter);
                        SetVideoList(videos);
                        break;
                }
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

        private void SetVideoList(List<VideoDataModel> videoBundle)
        {
            var publicationViewModels = videoBundle.Select(publication =>
                new PublicationItemViewModel(
                    NavigationService,
                    DialogService,
                    _platformService,
                    _videoPlayerService,
                    ApiService,
                    ErrorHandleService,
                    _mvxMessenger,
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

            Items.SwitchTo(publicationViewModels);
        }
    }
}
