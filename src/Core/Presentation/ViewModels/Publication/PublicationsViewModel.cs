using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Platforms;
using PrankChat.Mobile.Core.ApplicationServices.Timer;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Managers.Publications;
using PrankChat.Mobile.Core.Managers.Video;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication.Items;
using PrankChat.Mobile.Core.Presentation.ViewModels.Shared;
using PrankChat.Mobile.Core.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Publication
{
    public class PublicationsViewModel : PaginationViewModel, IVideoListViewModel
    {
        private readonly IPublicationsManager _publicationsManager;
        private readonly IVideoManager _videoManager;
        private readonly IPlatformService _platformService;
        private readonly IVideoPlayerService _videoPlayerService;

        private readonly Dictionary<DateFilterType, string> _dateFilterTypeTitleMap;

        private ExecutionStateWrapper _refreshFilterExecutionStateWrapper;

        public PublicationsViewModel(IPublicationsManager publicationsManager,
                                     IVideoManager videoManager,
                                     IPlatformService platformService,
                                     IVideoPlayerService videoPlayerService) : base(Constants.Pagination.DefaultPaginationSize)
        {
            _publicationsManager = publicationsManager;
            _videoManager = videoManager;
            _platformService = platformService;
            _videoPlayerService = videoPlayerService;

            Items = new MvxObservableCollection<PublicationItemViewModel>();
            _refreshFilterExecutionStateWrapper = new ExecutionStateWrapper();

            _dateFilterTypeTitleMap = new Dictionary<DateFilterType, string>
            {
                { DateFilterType.Day, Resources.Publication_Tab_Filter_Day },
                { DateFilterType.Week, Resources.Publication_Tab_Filter_Week },
                { DateFilterType.Month, Resources.Publication_Tab_Filter_Month },
                { DateFilterType.Quarter, Resources.Publication_Tab_Filter_Quarter },
                { DateFilterType.HalfYear, Resources.Publication_Tab_Filter_HalfYear },
            };

            _refreshFilterExecutionStateWrapper.SubscribeToEvent<ExecutionStateWrapper, bool>((s, e) => RaisePropertyChanged(nameof(IsRefreshingFilter)),
                                                                                              (wrapper, handler) => wrapper.IsBusyChanged += handler,
                                                                                              (wrapper, handler) => wrapper.IsBusyChanged -= handler).DisposeWith(Disposables);

            ItemsChangedInteraction = new MvxInteraction();
            OpenFilterCommand = new MvxAsyncCommand(OnOpenFilterAsync);

            Messenger.SubscribeOnMainThread<ReloadPublicationsMessage>((msg) => OnReloadItems()).DisposeWith(Disposables);
            Messenger.SubscribeOnMainThread<EnterForegroundMessage>((msg) => OnReloadItems()).DisposeWith(Disposables);
            Messenger.SubscribeOnMainThread<RefreshNotificationsMessage>(async (msg) => await NotificationBageViewModel.RefreshDataCommand.ExecuteAsync(null)).DisposeWith(Disposables);
            Messenger.Subscribe<TimerTickMessage>(OnTimerTick, MvxReference.Strong).DisposeWith(Disposables);
        }

        private PublicationType _selectedPublicationType;
        public PublicationType SelectedPublicationType
        {
            get => _selectedPublicationType;
            set
            {
                SetProperty(ref _selectedPublicationType, value, () => _ = RefreshDataAsync());
            }
        }

        public bool IsRefreshingFilter => _refreshFilterExecutionStateWrapper.IsBusy;

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

        public MvxObservableCollection<PublicationItemViewModel> Items { get; }

        public IMvxAsyncCommand OpenFilterCommand { get; }

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            ActiveFilter = DateFilterType.HalfYear;
            await LoadMoreItemsCommand.ExecuteAsync();
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
        }

        private async Task OnOpenFilterAsync(CancellationToken arg)
        {
            var parameters = _dateFilterTypeTitleMap.Values.ToArray();
            var selectedFilterName = await DialogService.ShowMenuDialogAsync(parameters, Resources.Cancel);

            if (string.IsNullOrWhiteSpace(selectedFilterName) || selectedFilterName == Resources.Cancel)
            {
                return;
            }

            ActiveFilter = _dateFilterTypeTitleMap.FirstOrDefault(filter => filter.Value == selectedFilterName).Key;
            await ReloadItemsCommand.ExecuteAsync();
        }

        private async Task RefreshDataAsync()
        {
            ShouldNotifyIsBusy = false;

            await _refreshFilterExecutionStateWrapper.WrapAsync(ReloadItemsAsync);

            ShouldNotifyIsBusy = true;
        }

        protected async override Task<int> LoadMoreItemsAsync(int page = 1, int pageSize = 20)
        {
            try
            {
                Pagination<Models.Data.Video> pageContainer = null;
                switch (SelectedPublicationType)
                {
                    case PublicationType.Popular:
                        pageContainer = await _publicationsManager.GetPopularVideoFeedAsync(ActiveFilter, page, pageSize);
                        break;

                    case PublicationType.Actual:
                        pageContainer = await _publicationsManager.GetActualVideoFeedAsync(DateFilterType.HalfYear, page, pageSize);
                        break;

                    case PublicationType.MyVideosOfCreatedOrders:
                        if (!IsUserSessionInitialized)
                        {
                            await NavigationService.ShowLoginView();
                            return 0;
                        }

                        pageContainer = await _publicationsManager.GetMyVideoFeedAsync(page, pageSize, DateFilterType.HalfYear);
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
        }

        private PublicationItemViewModel ProducePublicationItemViewModel(Models.Data.Video publication)
        {
            return new PublicationItemViewModel(
                _publicationsManager,
                _videoManager,
                _platformService,
                _videoPlayerService,
                publication,
                GetFullScreenVidos);
        }

        protected override int SetList<TDataModel, TApiModel>(Pagination<TApiModel> pagination, int page, Func<TApiModel, TDataModel> produceItemViewModel, MvxObservableCollection<TDataModel> items)
        {
            var count = base.SetList(pagination, page, produceItemViewModel, items);
            ItemsChangedInteraction.Raise();
            return count;
        }

        private List<FullScreenVideo> GetFullScreenVidos()
        {
            return Items.Select(item => item.GetFullScreenVideo()).ToList();
        }

        private void OnReloadItems()
        {
            Items.Clear();
            ReloadItemsCommand.Execute();
        }
    }
}