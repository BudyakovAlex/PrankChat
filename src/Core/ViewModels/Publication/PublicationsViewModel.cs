using FFImageLoading;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Common;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Extensions.DateFilter;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Managers.Publications;
using PrankChat.Mobile.Core.Managers.Users;
using PrankChat.Mobile.Core.Managers.Video;
using PrankChat.Mobile.Core.Messages;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.ViewModels.Common;
using PrankChat.Mobile.Core.ViewModels.Common.Items;
using PrankChat.Mobile.Core.ViewModels.Publication.Items;
using PrankChat.Mobile.Core.ViewModels.Registration;
using PrankChat.Mobile.Core.ViewModels.Results;
using PrankChat.Mobile.Core.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ViewModels.Publication
{
    public class PublicationsViewModel : PaginationViewModel<PublicationItemViewModel>, IVideoListViewModel
    {
        private readonly IPublicationsManager _publicationsManager;
        private readonly IVideoManager _videoManager;
        private readonly IUsersManager _usersManager;

        private readonly ExecutionStateWrapper _refreshDataExecutionStateWrapper;

        public PublicationsViewModel(
            IPublicationsManager publicationsManager,
            IVideoManager videoManager,
            IUsersManager usersManager,
            InviteFriendItemViewModel inviteFriendItemViewModel)
            : base(Constants.Pagination.DefaultPaginationSize)
        {
            _publicationsManager = publicationsManager;
            _videoManager = videoManager;
            _usersManager = usersManager;
            InviteFriendItemViewModel = inviteFriendItemViewModel;

            _refreshDataExecutionStateWrapper = new ExecutionStateWrapper();

            _refreshDataExecutionStateWrapper.SubscribeToEvent<ExecutionStateWrapper, bool>(
                (_, __) => RaisePropertiesChanged(nameof(IsRefreshingData), nameof(IsNotRefreshingData)),
                (wrapper, handler) => wrapper.IsBusyChanged += handler,
                (wrapper, handler) => wrapper.IsBusyChanged -= handler).DisposeWith(Disposables);

            ItemsChangedInteraction = new MvxInteraction();
            OpenFilterCommand = new MvxAsyncCommand(OnOpenFilterAsync);

            Messenger.SubscribeOnMainThread<ReloadPublicationsMessage>(OnReloadPublications).DisposeWith(Disposables);
        }

        public IMvxAsyncCommand OpenFilterCommand { get; }

        public InviteFriendItemViewModel InviteFriendItemViewModel { get; }

        private PublicationType _selectedPublicationType;
        public PublicationType SelectedPublicationType
        {
            get => _selectedPublicationType;
            set => SetProperty(ref _selectedPublicationType, value, () => _ = RefreshDataAsync());
        }

        public bool IsRefreshingData => _refreshDataExecutionStateWrapper.IsBusy;

        public bool IsNotRefreshingData => !IsRefreshingData;

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

            var selectedFilterName = await UserInteraction.ShowMenuDialogAsync(dictionary.Keys.ToArray(), Resources.Cancel);
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
                () => Items.ToArray(),
                _usersManager);
        }

        private void OnReloadPublications(ReloadPublicationsMessage message)
        {
            if (message.UpdatedItemsDictionary != null && message.UpdatedItemsDictionary.Count != 0)
            {
                UpdateDataItemsWithoutReload(message.UpdatedItemsDictionary);
                return;
            }

            ReloadItems();
        }

        private void ReloadItems()
        {
            Items.Clear();
            ReloadItemsCommand.Execute();
        }

        private void UpdateDataItemsWithoutReload(Dictionary<int, FullScreenVideoResult> items)
        {
            var updatedItems = 0;
            foreach (var item in Items)
            {
                if (!items.TryGetValue(item.VideoId, out var value))
                {
                    continue;
                }

                item.NumberOfLikes = value.NumberOfLikes;
                item.NumberOfDislikes = value.NumberOfDislikes;
                item.NumberOfComments = value.NumberOfComments;
                updatedItems++;
                item.RaisePropertiesChanged(
                    nameof(item.NumberOfLikesText),
                    nameof(item.NumberOfDislikesText),
                    nameof(item.NumberOfCommentsPresentation));
                if (updatedItems == items.Count)
                {
                    break;
                }
            }
        }
    }
}