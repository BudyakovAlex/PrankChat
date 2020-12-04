using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Platforms;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Managers.Publications;
using PrankChat.Mobile.Core.Managers.Search;
using PrankChat.Mobile.Core.Managers.Video;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order.Items;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication.Items;
using PrankChat.Mobile.Core.Presentation.ViewModels.Search.Items;
using PrankChat.Mobile.Core.Presentation.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels
{
    public class SearchViewModel : PaginationViewModel
    {
        private const int SearchDelay = 1000;

        private readonly ISearchManager _searchManager;
        private readonly IPublicationsManager _publicationsManager;
        private readonly IVideoManager _videoManager;
        private readonly IPlatformService _platformService;
        private readonly IVideoPlayerService _videoPlayerService;

        public SearchViewModel(ISearchManager searchManager,
                               IPublicationsManager publicationsManager,
                               IVideoManager videoManager,
                               IPlatformService platformService,
                               IVideoPlayerService videoPlayerService)
            : base(Constants.Pagination.DefaultPaginationSize)
        {
            Items = new MvxObservableCollection<MvxNotifyPropertyChanged>();

            _searchManager = searchManager;
            _publicationsManager = publicationsManager;
            _videoManager = videoManager;
            _platformService = platformService;
            _videoPlayerService = videoPlayerService;
        }

        public MvxObservableCollection<MvxNotifyPropertyChanged> Items { get; }

        private string _searchValue;
        public string SearchValue
        {
            get => _searchValue;
            set
            {
                SetProperty(ref _searchValue, value);
                _ = SearchAsync(SearchValue);
            }
        }

        private SearchTabType _searchTabType;
        public SearchTabType SearchTabType
        {
            get => _searchTabType;
            set
            {
                if (SetProperty(ref _searchTabType, value))
                {
                    Items.Clear();
                    _ = SearchAsync(SearchValue, true);
                }
            }
        }

        private async Task SearchAsync(string text, bool canSkipDelay = false)
        {
            if (text?.Length < 2)
            {
                return;
            }

            var buffer = text;
            if (!canSkipDelay)
            {
                await Task.Delay(SearchDelay);
            }

            if (buffer != _searchValue)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(_searchValue))
            {
                Items.Clear();
                return;
            }

            await ExecutionStateWrapper.WrapAsync(() => ReloadItemsCommand.ExecuteAsync());
        }

        protected async override Task<int> LoadMoreItemsAsync(int page = 1, int pageSize = 20)
        {
            try
            {
                switch (SearchTabType)
                {
                    case SearchTabType.Orders:
                        var ordersPaginationModel = await _searchManager.SearchOrdersAsync(SearchValue, page, pageSize);
                        return SetList(ordersPaginationModel, page, ProduceOrderViewModel, Items);
                    case SearchTabType.Users:
                        var usersPaginationModel = await _searchManager.SearchUsersAsync(SearchValue, page, pageSize);
                        return SetList(usersPaginationModel, page, ProduceUserViewModel, Items);
                    case SearchTabType.Videos:
                        var videosPaginationModel = await _searchManager.SearchVideosAsync(SearchValue, page, pageSize);
                        return SetList(videosPaginationModel, page, ProduceVideoViewModel, Items);
                }

                return 0;
            }
            catch (Exception ex)
            {
                ErrorHandleService.HandleException(ex);
                ErrorHandleService.LogError(this, "Search list loading error occured.");
                return 0;
            }
        }

        private MvxNotifyPropertyChanged ProduceVideoViewModel(VideoDataModel publication)
        {
            return new PublicationItemViewModel(_publicationsManager,
                                                _videoManager,
                                                _platformService,
                                                _videoPlayerService,
                                                publication,
                                                GetFullScreenVideoDataModels);
        }

        private BaseViewModel ProduceUserViewModel(UserDataModel model)
        {
            return new ProfileSearchItemViewModel(NavigationService, SettingsService, model);
        }

        private BaseViewModel ProduceOrderViewModel(OrderDataModel model)
        {
            return new OrderItemViewModel(NavigationService, SettingsService, Messenger, model, GetFullScreenVideoDataModels);
        }

        private List<FullScreenVideoDataModel> GetFullScreenVideoDataModels()
        {
            return Items.OfType<IFullScreenVideoOwnerViewModel>()
                        .Where(item => item.CanPlayVideo)
                        .Select(item => item.GetFullScreenVideoDataModel())
                        .ToList();
        }
    }
}
