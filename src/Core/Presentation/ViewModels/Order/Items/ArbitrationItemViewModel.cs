using MvvmCross.Commands;
using PrankChat.Mobile.Core.Commands;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Managers.Video;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.Navigation.Results;
using PrankChat.Mobile.Core.Presentation.ViewModels.Abstract;
using PrankChat.Mobile.Core.Presentation.ViewModels.Common.Abstract;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using PrankChat.Mobile.Core.Providers.UserSession;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Arbitration.Items
{
    public class ArbitrationItemViewModel : BaseViewModel
    {
        private readonly IUserSessionProvider _userSessionProvider;

        private readonly ArbitrationOrder _order;
        private readonly Func<BaseVideoItemViewModel[]> _getAllFullScreenVideosFunc;

        public ArbitrationItemViewModel(
            IVideoManager videoManager,
            IUserSessionProvider userSessionProvider,
            ArbitrationOrder order,
            Func<BaseVideoItemViewModel[]> getAllFullScreenVideosFunc)
        {
            _order = order;
            _getAllFullScreenVideosFunc = getAllFullScreenVideosFunc;

            _userSessionProvider = userSessionProvider;
            OrderTitle = order.Title;
            ProfilePhotoUrl = order?.Customer.Avatar;
            PriceText = order.Price.ToPriceString();
            Likes = order.Video?.LikesCount ?? 0;
            Dislikes = order.Video?.DislikesCount ?? 0;
            ProfileShortName = order.Customer?.Login.ToShortenName();

            if (_order.Video != null)
            {
                VideoItemViewModel = new OrderedVideoItemViewModel(
                    videoManager,
                    userSessionProvider,
                    order.Video);
            }

            OpenDetailsOrderCommand = new MvxRestrictedAsyncCommand(
                OpenDetailsOrderAsync,
                restrictedCanExecute: () => userSessionProvider.User != null,
                handleFunc: NavigationManager.NavigateAsync<LoginViewModel>);

            OpenUserProfileCommand = new MvxRestrictedAsyncCommand(
                OpenUserProfileAsync,
                restrictedCanExecute: () => _userSessionProvider.User != null,
                handleFunc: NavigationManager.NavigateAsync<LoginViewModel>);
        }

        public BaseVideoItemViewModel VideoItemViewModel { get; }

        public string OrderTitle { get; }

        public string ProfilePhotoUrl { get; }

        public string TimeText => (_order.ArbitrationFinishAt - DateTime.UtcNow)?.ToTimeWithSpaceString();

        public string ProfileShortName { get; }

        public string PriceText { get; }

        public bool CanPlayVideo => _order?.Video != null;

        private long _likes;
        public long Likes
        {
            get => _likes;
            set => SetProperty(ref _likes, value);
        }

        private long _dislikes;
        public long Dislikes
        {
            get => _dislikes;
            set => SetProperty(ref _dislikes, value);
        }

        public IMvxAsyncCommand OpenDetailsOrderCommand { get; }

        public IMvxAsyncCommand OpenUserProfileCommand { get; }

        public OrderType OrderType => _userSessionProvider.User?.Id == _order.Customer?.Id
            ? OrderType.MyOrder
            : OrderType.NotMyOrder;

        private Task OpenUserProfileAsync()
        {
            if (_order.Customer?.Id is null ||
                _order.Customer.Id == _userSessionProvider.User.Id)
            {
                return Task.CompletedTask;
            }

            if (!Connectivity.NetworkAccess.HasConnection())
            {
                return Task.CompletedTask;
            }

            return NavigationManager.NavigateAsync<UserProfileViewModel, int, bool>(_order.Customer.Id);
        }

        private BaseVideoItemViewModel[] GetFullScreenVideos()
        {
            if (_getAllFullScreenVideosFunc != null)
            {
                return _getAllFullScreenVideosFunc.Invoke();
            }

            return VideoItemViewModel is null
                ? Array.Empty<BaseVideoItemViewModel>()
                : new BaseVideoItemViewModel[] { VideoItemViewModel };
        }

        private async Task OpenDetailsOrderAsync()
        {
            var items = GetFullScreenVideos();
            var currentItem = items.FirstOrDefault(item => item.VideoId == _order.Video?.Id);
            var index = currentItem is null ? 0 : items.IndexOfOrDefault(currentItem);

            var parameter = new OrderDetailsNavigationParameter(_order.Id, items, index);
            var result = await NavigationManager.NavigateAsync<OrderDetailsViewModel, OrderDetailsNavigationParameter, OrderDetailsResult>(parameter);
            if (result == null)
            {
                return;
            }

            Likes = result.PositiveArbitrationValuesCount ?? 0;
            Dislikes = result.NegativeArbitrationValuesCount ?? 0;
        }
    }
}