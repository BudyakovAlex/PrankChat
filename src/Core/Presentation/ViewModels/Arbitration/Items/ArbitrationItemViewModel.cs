using MvvmCross.Commands;
using PrankChat.Mobile.Core.Commands;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.Navigation.Results;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile;
using PrankChat.Mobile.Core.Providers.UserSession;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Arbitration.Items
{
    public class ArbitrationItemViewModel : BaseViewModel, IFullScreenVideoOwnerViewModel
    {
        private readonly IUserSessionProvider _userSessionProvider;
        private readonly INavigationService _navigationService;

        private readonly int? _customerId;
        private readonly ArbitrationOrder _order;
        private readonly Func<List<FullScreenVideo>> _getAllFullScreenVideoDataFunc;
        private readonly DateTime? _arbitrationFinishAt;
        private readonly int _orderId;

        public ArbitrationItemViewModel(INavigationService navigatiobService,
                                        IUserSessionProvider userSessionProvider,
                                        bool isUserSessionInitialized,
                                        int orderId,
                                        string orderTitle,
                                        string customerPhotoUrl,
                                        string customerName,
                                        double? priceText,
                                        int likes,
                                        int dislikes,
                                        DateTime? arbitrationFinishAt,
                                        int? customerId,
                                        ArbitrationOrder order,
                                        Func<List<FullScreenVideo>> getAllFullScreenVideoDataFunc)
        {
            _userSessionProvider = userSessionProvider;
            _navigationService = navigatiobService;
            OrderTitle = orderTitle;
            ProfilePhotoUrl = customerPhotoUrl;
            PriceText = priceText.ToPriceString();
            Likes = likes;
            Dislikes = dislikes;
            ProfileShortName = customerName.ToShortenName();

            _arbitrationFinishAt = arbitrationFinishAt;
            _customerId = customerId;
            _order = order;
            _getAllFullScreenVideoDataFunc = getAllFullScreenVideoDataFunc;
            _orderId = orderId;

            OpenDetailsOrderCommand = new MvxRestrictedAsyncCommand(OnOpenDetailsOrderAsync, restrictedCanExecute: () => isUserSessionInitialized, handleFunc: _navigationService.ShowLoginView);
            OpenUserProfileCommand = new MvxRestrictedAsyncCommand(OpenUserProfileAsync, restrictedCanExecute: () => _userSessionProvider.User != null, handleFunc: _navigationService.ShowLoginView);
        }

        public string OrderTitle { get; }

        public string ProfilePhotoUrl { get; }

        public string TimeText => (_arbitrationFinishAt - DateTime.UtcNow)?.ToTimeWithSpaceString();

        public string ProfileShortName { get; }

        public string PriceText { get; }

        public bool CanPlayVideo => _order?.Video != null;

        private int _likes;
        public int Likes
        {
            get => _likes;
            set => SetProperty(ref _likes, value);
        }

        private int _dislikes;
        public int Dislikes
        {
            get => _dislikes;
            set => SetProperty(ref _dislikes, value);
        }

        public IMvxAsyncCommand OpenDetailsOrderCommand { get; }

        public IMvxAsyncCommand OpenUserProfileCommand { get; }

        public OrderType OrderType => _userSessionProvider.User?.Id == _customerId
            ? OrderType.MyOrder
            : OrderType.NotMyOrder;

        public FullScreenVideo GetFullScreenVideo()
        {
            return new FullScreenVideo(
                _order.Customer.Id,
                _order.Customer.IsSubscribed,
                _order.Video.Id,
                _order.Video.StreamUri,
                _order.Title,
                _order.Description,
                _order.Video.ShareUri,
                _order.Customer.Avatar,
                _order.Customer?.Login?.ToShortenName(),
                _order.Video.LikesCount,
                _order.Video.DislikesCount,
                _order.Video.CommentsCount,
                _order.Video.IsLiked,
                _order.Video.IsDisliked,
                _order.Video.Poster);
        }

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

        private async Task OnOpenDetailsOrderAsync()
        {
            var items = _getAllFullScreenVideoDataFunc?.Invoke() ?? new List<FullScreenVideo> { GetFullScreenVideo() };
            var currentItem = items.FirstOrDefault(item => item.VideoId == _order.Video?.Id);
            var index = currentItem is null ? 0 : items.IndexOf(currentItem);

            var parameter = new OrderDetailsNavigationParameter(_orderId, items, index);
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
