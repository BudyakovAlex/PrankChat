using MvvmCross.Commands;
using PrankChat.Mobile.Core.Commands;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Managers.Video;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.ViewModels.Common.Abstract;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order.Items.Abstract;
using PrankChat.Mobile.Core.Presentation.ViewModels.Parameters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using PrankChat.Mobile.Core.Providers.UserSession;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Arbitration.Items
{
    //TODO: make base order for arbitration and simple order
    public class ArbitrationOrderItemViewModel : BaseOrderItemViewModel
    {
        private readonly IUserSessionProvider _userSessionProvider;

        private readonly ArbitrationOrder _order;

        public ArbitrationOrderItemViewModel(
            IVideoManager videoManager,
            IUserSessionProvider userSessionProvider,
            ArbitrationOrder order,
            Func<BaseVideoItemViewModel[]> getAllFullScreenVideosFunc) : base(
                videoManager,
                userSessionProvider,
                order.Video,
                order.Customer?.Id,
                getAllFullScreenVideosFunc)
        {
            _order = order;

            _userSessionProvider = userSessionProvider;
            OrderTitle = order.Title;
            ProfilePhotoUrl = order?.Customer.Avatar;
            PriceText = order.Price.ToPriceString();
            Likes = order.Video?.LikesCount ?? 0;
            Dislikes = order.Video?.DislikesCount ?? 0;
            ProfileShortName = order.Customer?.Login.ToShortenName();

            OpenDetailsOrderCommand = new MvxRestrictedAsyncCommand(
                OpenDetailsOrderAsync,
                restrictedCanExecute: () => userSessionProvider.User != null,
                handleFunc: NavigationManager.NavigateAsync<LoginViewModel>);
        }

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

        public OrderType OrderType => _userSessionProvider.User?.Id == _order.Customer?.Id
            ? OrderType.MyOrder
            : OrderType.NotMyOrder;

        private Task OpenDetailsOrderAsync()
        {
            var items = GetFullScreenVideos();
            var currentItem = items.FirstOrDefault(item => item.VideoId == _order.Video?.Id);
            var index = currentItem is null ? 0 : items.IndexOfOrDefault(currentItem);

            var parameter = new OrderDetailsNavigationParameter(_order.Id, items, index);
            return NavigationManager.NavigateAsync<OrderDetailsViewModel, OrderDetailsNavigationParameter>(parameter);
        }
    }
}