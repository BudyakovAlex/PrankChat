using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Managers.Search;
using PrankChat.Mobile.Core.Managers.Video;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.ViewModels.Abstract;
using PrankChat.Mobile.Core.Presentation.ViewModels.Common;
using PrankChat.Mobile.Core.Presentation.ViewModels.Common.Abstract;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order.Items;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order.Items.Abstract;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication.Items;
using PrankChat.Mobile.Core.Presentation.ViewModels.Search.Items;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels
{
    public class SearchViewModel : PaginationViewModel
    {
        private const int SearchDelay = 1000;

        private readonly ISearchManager _searchManager;
        private readonly IVideoManager _videoManager;

        public SearchViewModel(
            ISearchManager searchManager,
            IVideoManager videoManager) : base(Constants.Pagination.DefaultPaginationSize)
        {
            Items = new MvxObservableCollection<MvxNotifyPropertyChanged>();

            _searchManager = searchManager;
            _videoManager = videoManager;
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

            await ReloadItemsCommand.ExecuteAsync();
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

        private MvxNotifyPropertyChanged ProduceVideoViewModel(Models.Data.Video video)
        {
            return new PublicationItemViewModel(
                _videoManager,
                UserSessionProvider,
                video,
                GetPublicationsFullScreenVideos);
        }

        private BaseViewModel ProduceUserViewModel(User model) =>
            new ProfileSearchItemViewModel(UserSessionProvider, model);

        private BaseViewModel ProduceOrderViewModel(Models.Data.Order model) =>
            new OrderItemViewModel(
                _videoManager,
                UserSessionProvider,
                model,
                GetOrdersFullScreenVideos);

        private BaseVideoItemViewModel[] GetPublicationsFullScreenVideos() =>
            Items.OfType<BaseVideoItemViewModel>()
                 .ToArray();

        private BaseVideoItemViewModel[] GetOrdersFullScreenVideos() =>
           Items.OfType<BaseOrderItemViewModel>()
                .Where(order => order.VideoItemViewModel != null)
                .Select(order => order.VideoItemViewModel)
                .ToArray();
    }
}
