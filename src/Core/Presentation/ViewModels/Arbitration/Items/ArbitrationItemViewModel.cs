using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.Commands;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Providers.UserSession;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Arbitration.Items
{
    public class ArbitrationItemViewModel : BaseViewModel, IFullScreenVideoOwnerViewModel
    {
        private readonly IUserSessionProvider _userSessionProvider;
        private readonly INavigationService _navigationService;

        private readonly int? _customerId;
        private readonly ArbitrationOrder _orderDataModel;
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
                                        ArbitrationOrder orderDataModel,
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
            _orderDataModel = orderDataModel;
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

        public bool CanPlayVideo => _orderDataModel?.Video != null;

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

        public FullScreenVideo GetFullScreenVideoDataModel()
        {
            return new FullScreenVideo(_orderDataModel.Customer.Id,
                                                _orderDataModel.Customer.IsSubscribed,
                                                _orderDataModel.Video.Id,
                                                _orderDataModel.Video.StreamUri,
                                                _orderDataModel.Title,
                                                _orderDataModel.Description,
                                                _orderDataModel.Video.ShareUri,
                                                _orderDataModel.Customer.Avatar,
                                                _orderDataModel.Customer?.Login?.ToShortenName(),
                                                _orderDataModel.Video.LikesCount,
                                                _orderDataModel.Video.DislikesCount,
                                                _orderDataModel.Video.CommentsCount,
                                                _orderDataModel.Video.IsLiked,
                                                _orderDataModel.Video.IsDisliked,
                                                _orderDataModel.Video.Poster);
        }

        private Task OpenUserProfileAsync()
        {
            if (_orderDataModel.Customer?.Id is null ||
                _orderDataModel.Customer.Id == _userSessionProvider.User.Id)
            {
                return Task.CompletedTask;
            }

            return _navigationService.ShowUserProfile(_orderDataModel.Customer.Id);
        }

        private async Task OnOpenDetailsOrderAsync()
        {
            var items = _getAllFullScreenVideoDataFunc?.Invoke() ?? new List<FullScreenVideo> { GetFullScreenVideoDataModel() };
            var currentItem = items.FirstOrDefault(item => item.VideoId == _orderDataModel.Video?.Id);
            var index = currentItem is null ? 0 : items.IndexOf(currentItem);

            var result = await _navigationService.ShowOrderDetailsView(_orderId, items, index);
            if (result == null)
            {
                return;
            }

            Likes = result.PositiveArbitrationValuesCount ?? 0;
            Dislikes = result.NegativeArbitrationValuesCount ?? 0;
        }
    }
}
