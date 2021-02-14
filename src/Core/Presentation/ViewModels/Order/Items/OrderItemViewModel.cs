using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.Timer;
using PrankChat.Mobile.Core.Commands;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Providers.UserSession;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Order.Items
{
    public class OrderItemViewModel : BaseViewModel, IFullScreenVideoOwnerViewModel, IDisposable
    {
        private readonly INavigationService _navigationService;
        private readonly IUserSessionProvider _userSessionProvider;

        private readonly Models.Data.Order _orderDataModel;
        private readonly Func<List<FullScreenVideo>> _getAllFullScreenVideoDataFunc;

        private IDisposable _timerTickMessageToken;

        public OrderItemViewModel(INavigationService navigationService,
                                  IUserSessionProvider userSessionProvider,
                                  Models.Data.Order orderDataModel,
                                  Func<List<FullScreenVideo>> getAllFullScreenVideoDataFunc)
        {
            _navigationService = navigationService;
            _userSessionProvider = userSessionProvider;
            _orderDataModel = orderDataModel;
            _getAllFullScreenVideoDataFunc = getAllFullScreenVideoDataFunc;

            ElapsedTime = _orderDataModel.ActiveTo is null
                ? TimeSpan.FromHours(_orderDataModel.DurationInHours)
                : _orderDataModel.GetActiveOrderTime();

            _timerTickMessageToken = Messenger.Subscribe<TimerTickMessage>(OnTimerTick, MvxReference.Strong).DisposeWith(Disposables);

            OpenDetailsOrderCommand = new MvxRestrictedAsyncCommand(OnOpenDetailsOrderAsync, restrictedCanExecute: () => _userSessionProvider.User != null, handleFunc: _navigationService.ShowLoginView);
            OpenUserProfileCommand = new MvxRestrictedAsyncCommand(OpenUserProfileAsync, restrictedCanExecute: () => _userSessionProvider.User != null, handleFunc: _navigationService.ShowLoginView);
        }

        public int OrderId => _orderDataModel.Id;

        public string Title => _orderDataModel.Title;

        public string ProfilePhotoUrl => _orderDataModel.Customer?.Avatar;

        public string ProfileShortName => _orderDataModel.Customer?.Login.ToShortenName();

        public OrderType OrderType => _userSessionProvider.User.GetOrderType(_orderDataModel.Customer?.Id, _orderDataModel?.Status ?? OrderStatusType.None);

        public OrderTagType OrderTagType => _userSessionProvider.User.GetOrderTagType(_orderDataModel.Customer?.Id, _orderDataModel?.Status);

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

        public bool CanPlayVideo => _orderDataModel?.Video != null;

        public bool IsTimeAvailable => _elapsedTime.HasValue;

        public string TimeText => _elapsedTime?.ToTimeWithSpaceString();

        public string PriceText => _orderDataModel.Price.ToPriceString();

        public string StatusText => _orderDataModel.GetOrderStatusTitle(_userSessionProvider?.User);

        public bool IsHiddenOrder => _orderDataModel?.OrderCategory == OrderCategory.Private;

        public IMvxAsyncCommand OpenDetailsOrderCommand { get; }

        public IMvxAsyncCommand OpenUserProfileCommand { get; }

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
                                                _orderDataModel.Customer.Login.ToShortenName(),
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

        private void OnTimerTick(TimerTickMessage message)
        {
            ElapsedTime = _orderDataModel.ActiveTo is null
               ? TimeSpan.FromHours(_orderDataModel.DurationInHours)
               : _orderDataModel.GetActiveOrderTime();
        }

        private async Task OnOpenDetailsOrderAsync()
        {
            // var items = _getAllFullScreenVideoDataFunc?.Invoke() ?? new List<FullScreenVideoDataModel> { GetFullScreenVideoDataModel() };
            // var currentItem = items.FirstOrDefault(item => item.VideoId == _orderDataModel.Video?.Id);
            // var index = currentItem is null ? 0 : items.IndexOf(currentItem);

            var result = await _navigationService.ShowOrderDetailsView(OrderId, null, -1);
            if (result == null)
            {
                return;
            }

            _orderDataModel.Status = result.Status ?? OrderStatusType.None;
            _orderDataModel.ActiveTo = result.ActiveTo;

            if (_orderDataModel.Status == OrderStatusType.Cancelled ||
                _orderDataModel.Status == OrderStatusType.Finished ||
                _orderDataModel.Status == OrderStatusType.InArbitration)
            {
                var status = _orderDataModel.Status.Value;
                Messenger.Publish(new RemoveOrderMessage(this, OrderId, status));
                return;
            }

            ElapsedTime = _orderDataModel.ActiveTo is null
              ? TimeSpan.FromHours(_orderDataModel.DurationInHours)
              : _orderDataModel.GetActiveOrderTime();

            await RaisePropertyChanged(nameof(StatusText));
            await RaisePropertyChanged(nameof(OrderType));
            await RaisePropertyChanged(nameof(OrderTagType));
        }
    }
}