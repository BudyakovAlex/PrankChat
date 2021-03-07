using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.Timer;
using PrankChat.Mobile.Core.Commands;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.Navigation.Results;
using PrankChat.Mobile.Core.Presentation.ViewModels.Abstract;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using PrankChat.Mobile.Core.Providers.UserSession;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Order.Items
{
    public class OrderItemViewModel : BaseViewModel, IFullScreenVideoOwnerViewModel, IDisposable
    {
        private readonly IUserSessionProvider _userSessionProvider;

        private readonly Models.Data.Order _order;
        private readonly Func<List<FullScreenVideo>> _getAllFullScreenVideoDataFunc;

        private IDisposable _timerTickMessageToken;

        public OrderItemViewModel(
            IUserSessionProvider userSessionProvider,
            Models.Data.Order order,
            Func<List<FullScreenVideo>> getAllFullScreenVideoDataFunc)
        {
            _userSessionProvider = userSessionProvider;
            _order = order;
            _getAllFullScreenVideoDataFunc = getAllFullScreenVideoDataFunc;

            ElapsedTime = _order.ActiveTo is null
                ? TimeSpan.FromHours(_order.DurationInHours)
                : _order.GetActiveOrderTime();

            _timerTickMessageToken = Messenger.Subscribe<TimerTickMessage>(OnTimerTick, MvxReference.Strong).DisposeWith(Disposables);

            OpenDetailsOrderCommand = new MvxRestrictedAsyncCommand(
                OnOpenDetailsOrderAsync,
                restrictedCanExecute: () => _userSessionProvider.User != null,
                handleFunc: NavigationManager.NavigateAsync<LoginViewModel>);

            OpenUserProfileCommand = new MvxRestrictedAsyncCommand(
                OpenUserProfileAsync,
                restrictedCanExecute: () => _userSessionProvider.User != null,
                handleFunc: NavigationManager.NavigateAsync<LoginViewModel>);
        }

        public int OrderId => _order.Id;

        public string Title => _order.Title;

        public string ProfilePhotoUrl => _order.Customer?.Avatar;

        public string ProfileShortName => _order.Customer?.Login.ToShortenName();

        public OrderType OrderType => _userSessionProvider.User.GetOrderType(_order.Customer?.Id, _order?.Status ?? OrderStatusType.None);

        public OrderTagType OrderTagType => _userSessionProvider.User.GetOrderTagType(_order.Customer?.Id, _order?.Status);

        private TimeSpan? _elapsedTime;
        public TimeSpan? ElapsedTime
        {
            get => _elapsedTime;
            set
            {
                SetProperty(ref _elapsedTime, value);
                RaisePropertyChanged(nameof(TimeText));
                RaisePropertyChanged(nameof(IsTimeAvailable));

                if (!IsTimeAvailable)
                {
                    if (_timerTickMessageToken != null)
                    {
                        Disposables.Remove(_timerTickMessageToken);
                        _timerTickMessageToken.Dispose();
                        _timerTickMessageToken = null;
                    }
                }
            }
        }

        public bool CanPlayVideo => _order?.Video != null;

        public bool IsTimeAvailable => _elapsedTime.HasValue;

        public string TimeText => _elapsedTime?.ToTimeWithSpaceString();

        public string PriceText => _order.Price.ToPriceString();

        public string StatusText => _order.GetOrderStatusTitle(_userSessionProvider?.User);

        public bool IsHiddenOrder => _order?.OrderCategory == OrderCategory.Private;

        public IMvxAsyncCommand OpenDetailsOrderCommand { get; }

        public IMvxAsyncCommand OpenUserProfileCommand { get; }

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
                _order.Customer.Login.ToShortenName(),
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

        private void OnTimerTick(TimerTickMessage message)
        {
            ElapsedTime = _order.ActiveTo is null
               ? TimeSpan.FromHours(_order.DurationInHours)
               : _order.GetActiveOrderTime();
        }

        private async Task OnOpenDetailsOrderAsync()
        {
            // var items = _getAllFullScreenVideoDataFunc?.Invoke() ?? new List<FullScreenVideoDataModel> { GetFullScreenVideoDataModel() };
            // var currentItem = items.FirstOrDefault(item => item.VideoId == _orderDataModel.Video?.Id);
            // var index = currentItem is null ? 0 : items.IndexOf(currentItem);

            var parameter = new OrderDetailsNavigationParameter(OrderId, null, -1);
            var result = await NavigationManager.NavigateAsync<OrderDetailsViewModel, OrderDetailsNavigationParameter, OrderDetailsResult>(parameter);
            if (result == null)
            {
                return;
            }

            _order.Status = result.Status ?? OrderStatusType.None;
            _order.ActiveTo = result.ActiveTo;

            if (_order.Status == OrderStatusType.Cancelled ||
                _order.Status == OrderStatusType.Finished ||
                _order.Status == OrderStatusType.InArbitration)
            {
                var status = _order.Status.Value;
                Messenger.Publish(new RemoveOrderMessage(this, OrderId, status));
                return;
            }

            ElapsedTime = _order.ActiveTo is null
              ? TimeSpan.FromHours(_order.DurationInHours)
              : _order.GetActiveOrderTime();

            await RaisePropertyChanged(nameof(StatusText));
            await RaisePropertyChanged(nameof(OrderType));
            await RaisePropertyChanged(nameof(OrderTagType));
        }
    }
}