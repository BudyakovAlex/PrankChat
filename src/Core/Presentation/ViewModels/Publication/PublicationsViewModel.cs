﻿using FFImageLoading;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Common;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Extensions.DateFilter;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Managers.Publications;
using PrankChat.Mobile.Core.Managers.Video;
using PrankChat.Mobile.Core.Messages;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.ViewModels.Common;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication.Items;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using PrankChat.Mobile.Core.Wrappers;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Publication
{
    public class PublicationsViewModel : PaginationViewModel, IVideoListViewModel
    {
        private readonly IPublicationsManager _publicationsManager;
        private readonly IVideoManager _videoManager;

        private readonly ExecutionStateWrapper _refreshDataExecutionStateWrapper;

        public PublicationsViewModel(
            IPublicationsManager publicationsManager,
            IVideoManager videoManager)
            : base(Constants.Pagination.DefaultPaginationSize)
        {
            _publicationsManager = publicationsManager;
            _videoManager = videoManager;

            Items = new MvxObservableCollection<PublicationItemViewModel>();
            _refreshDataExecutionStateWrapper = new ExecutionStateWrapper();

            _refreshDataExecutionStateWrapper.SubscribeToEvent<ExecutionStateWrapper, bool>(
                (_, __) => RaisePropertyChanged(nameof(IsRefreshingData)),
                (wrapper, handler) => wrapper.IsBusyChanged += handler,
                (wrapper, handler) => wrapper.IsBusyChanged -= handler).DisposeWith(Disposables);

            ItemsChangedInteraction = new MvxInteraction();
            OpenFilterCommand = new MvxAsyncCommand(OnOpenFilterAsync);

            Messenger.SubscribeOnMainThread<ReloadPublicationsMessage>(_ => ReloadItems()).DisposeWith(Disposables);
        }

        private PublicationType _selectedPublicationType;
        public PublicationType SelectedPublicationType
        {
            get => _selectedPublicationType;
            set => SetProperty(ref _selectedPublicationType, value, () => _ = RefreshDataAsync());
        }

        public bool IsRefreshingData => _refreshDataExecutionStateWrapper.IsBusy;

        public MvxInteraction ItemsChangedInteraction { get; }

        public string ActiveFilterName => ActiveFilter.ToLocalizedString();

        private DateFilterType _activeFilter;
        public DateFilterType ActiveFilter
        {
            get => _activeFilter;
            set
            {
                _activeFilter = value;
                _ = RaisePropertyChanged(nameof(ActiveFilterName));
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
            _ = SafeExecutionWrapper.WrapAsync(() => _refreshDataExecutionStateWrapper.WrapAsync(() => LoadMoreItemsAsync()));
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
            var isExtracted = typeof(DateFilterType).TryExtractValues<DateFilterType>(out var enumValues);
            var dictionary = enumValues.ToDictionary(kv => kv.ToLocalizedString(), kv => kv);
            if (!isExtracted)
            {
                return;
            }

            var selectedFilterName = await DialogService.ShowMenuDialogAsync(dictionary.Keys.ToArray(), Resources.Cancel);
            if (string.IsNullOrWhiteSpace(selectedFilterName) ||
                selectedFilterName == Resources.Cancel)
            {
                return;
            }

            ActiveFilter = dictionary[selectedFilterName];
            await ReloadItemsCommand.ExecuteAsync();
        }

        private async Task RefreshDataAsync()
        {
            ShouldNotifyIsBusy = false;

            await _refreshDataExecutionStateWrapper.WrapAsync(ReloadItemsAsync);

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
                            await NavigationManager.NavigateAsync<LoginViewModel>();
                            return 0;
                        }

                        pageContainer = await _publicationsManager.GetMyVideoFeedAsync(page, pageSize, DateFilterType.HalfYear);
                        break;
                }

                var preloadImagesTasks = pageContainer.Items.Select(item => ImageService.Instance.LoadUrl(item.Poster).PreloadAsync()).ToArray();
                _ = Task.WhenAll(preloadImagesTasks);

                return SetList(pageContainer, page, ProducePublicationItemViewModel, Items);
            }
            catch (Exception ex)
            {
                ErrorHandleService.HandleException(ex);
                ErrorHandleService.LogError(this, "Error on publication list loading.");
                return 0;
            }
        }

        protected override int SetList<TDataModel, TApiModel>(Pagination<TApiModel> pagination, int page, Func<TApiModel, TDataModel> produceItemViewModel, MvxObservableCollection<TDataModel> items)
        {
            var count = base.SetList(pagination, page, produceItemViewModel, items);
            ItemsChangedInteraction.Raise();
            return count;
        }

        private PublicationItemViewModel ProducePublicationItemViewModel(Models.Data.Video publication)
        {
            return new PublicationItemViewModel(
                _videoManager,
                UserSessionProvider,
                publication,
                () => Items.ToArray());
        }

        private void ReloadItems()
        {
            Items.Clear();
            ReloadItemsCommand.Execute();
        }
    }
}