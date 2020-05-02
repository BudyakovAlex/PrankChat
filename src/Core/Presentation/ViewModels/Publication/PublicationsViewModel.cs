using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Commands;
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
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication.Items;
using PrankChat.Mobile.Core.Presentation.ViewModels.Shared;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Publication
{
    public class PublicationsViewModel : PaginationViewModel, IVideoListViewModel
    {
        private readonly IPlatformService _platformService;
        private readonly IVideoPlayerService _videoPlayerService;
        private readonly ISettingsService _settingsService;
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
                    ReloadItemsCommand.Execute();
                }
            }
        }

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

        public MvxObservableCollection<PublicationItemViewModel> Items { get; } =
            new MvxObservableCollection<PublicationItemViewModel>();

        public MvxAsyncCommand OpenFilterCommand { get; }

        public PublicationsViewModel(INavigationService navigationService,
                                     IDialogService dialogService,
                                     IApiService apiService,
                                     IPlatformService platformService,
                                     IVideoPlayerService videoPlayerService,
                                     ISettingsService settingsService,
                                     IErrorHandleService errorHandleService,
                                     IMvxMessenger mvxMessenger)
            : base(Constants.Pagination.DefaultPaginationSize, navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
            _platformService = platformService;
            _videoPlayerService = videoPlayerService;
            _settingsService = settingsService;
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

            OpenFilterCommand = new MvxAsyncCommand(OnOpenFilterAsync);
        }

        public override Task Initialize()
        {
            ActiveFilter = DateFilterType.Month;
            return LoadMoreItemsCommand.ExecuteAsync();
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

        private async Task OnOpenFilterAsync(CancellationToken arg)
        {
            var parameters = _dateFilterTypeTitleMap.Values.ToArray();
            var selectedFilterName = await DialogService.ShowMenuDialogAsync(parameters, Resources.Cancel);

            if (string.IsNullOrWhiteSpace(selectedFilterName) || selectedFilterName == Resources.Cancel)
            {
                return;
            }

            ActiveFilter = _dateFilterTypeTitleMap.FirstOrDefault(x => x.Value == selectedFilterName).Key;
            await ReloadItemsCommand.ExecuteAsync();
        }

        protected override async Task<int> LoadMoreItemsAsync(int page = 1, int pageSize = 20)
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

                return SetList(pageContainer, page, ProducePublicationItemViewModel, Items);
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

        private PublicationItemViewModel ProducePublicationItemViewModel(VideoDataModel publication)
        {
            return new PublicationItemViewModel(NavigationService,
                                                DialogService,
                                                _platformService,
                                                _videoPlayerService,
                                                ApiService,
                                                ErrorHandleService,
                                                _mvxMessenger,
                                                _settingsService,
                                                publication.Poster,
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
                                                publication.IsLiked);
        }

        protected override int SetList<TDataModel, TApiModel>(PaginationModel<TApiModel> dataModel, int page, Func<TApiModel, TDataModel> produceItemViewModel, MvxObservableCollection<TDataModel> items)
        {
            var count = base.SetList(dataModel, page, produceItemViewModel, items);
            ItemsChangedInteraction.Raise();
            return count;
        }
    }
}
