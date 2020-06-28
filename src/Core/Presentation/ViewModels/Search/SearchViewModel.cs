using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Platforms;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Navigation;
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
        private const int SearchDelay = 1500;

        private readonly IPlatformService _platformService;
        private readonly IVideoPlayerService _videoPlayerService;

        public SearchViewModel(INavigationService navigationService,
                               IErrorHandleService errorHandleService,
                               IApiService apiService,
                               IDialogService dialogService,
                               ISettingsService settingsService,
                               IPlatformService platformService,
                               IVideoPlayerService videoPlayerService)
            : base(Constants.Pagination.DefaultPaginationSize, navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
            Items = new MvxObservableCollection<MvxNotifyPropertyChanged>();
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
                    _ = SearchAsync(SearchValue, true);
                }
            }
        }

        private async Task SearchAsync(string text, bool canSkipDelay = false)
        {
            if (text.Length < 2)
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

            IsBusy = true;

            await ReloadItemsCommand.ExecuteAsync();

            IsBusy = false;
        }

        protected override async Task<int> LoadMoreItemsAsync(int page = 1, int pageSize = 20)
        {
            try
            {
                IsBusy = true;

                switch (SearchTabType)
                {
                    case SearchTabType.Orders:
                        var ordersPaginationModel = await ApiService.SearchOrdersAsync(SearchValue, page, pageSize);
                        return SetList(ordersPaginationModel, page, ProduceOrderViewModel, Items);
                    case SearchTabType.Users:
                        var usersPaginationModel = await ApiService.SearchUsersAsync(SearchValue, page, pageSize);
                        return SetList(usersPaginationModel, page, ProduceUserViewModel, Items);
                    case SearchTabType.Videos:
                        var videosPaginationModel = await ApiService.SearchVideosAsync(SearchValue, page, pageSize);
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
            finally
            {
                IsBusy = false;
            }
        }

        private MvxNotifyPropertyChanged ProduceVideoViewModel(VideoDataModel publication)
        {
            return new PublicationItemViewModel(NavigationService,
                                                DialogService,
                                                _platformService,
                                                _videoPlayerService,
                                                ApiService,
                                                ErrorHandleService,
                                                Messenger,
                                                SettingsService,
                                                publication,
                                                GetFullScreenVideoDataModels);
        }

        private BaseItemViewModel ProduceUserViewModel(UserDataModel model)
        {
            return new ProfileSearchItemViewModel(NavigationService, SettingsService, model);
        }

        private BaseItemViewModel ProduceOrderViewModel(OrderDataModel model)
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
