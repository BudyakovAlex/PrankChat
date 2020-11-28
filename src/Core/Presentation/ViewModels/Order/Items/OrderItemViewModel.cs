using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.ApplicationServices.Timer;
using PrankChat.Mobile.Core.Commands;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Order.Items
{
    public class OrderItemViewModel : BaseViewModel, IFullScreenVideoOwnerViewModel, IDisposable
    {
        private readonly INavigationService _navigationService;
        private readonly ISettingsService _settingsService;
        private readonly IMvxMessenger _mvxMessenger;

        private readonly OrderDataModel _orderDataModel;
        private readonly Func<List<FullScreenVideoDataModel>> _getAllFullScreenVideoDataFunc;

        private MvxSubscriptionToken _timerTickMessageToken;

        public OrderItemViewModel(INavigationService navigationService,
                                  ISettingsService settingsService,
                                  IMvxMessenger mvxMessenger,
                                  OrderDataModel orderDataModel,
                                  Func<List<FullScreenVideoDataModel>> getAllFullScreenVideoDataFunc)
        {
            _navigationService = navigationService;
            _settingsService = settingsService;
            _mvxMessenger = mvxMessenger;
            _orderDataModel = orderDataModel;
            _getAllFullScreenVideoDataFunc = getAllFullScreenVideoDataFunc;
            ElapsedTime = _orderDataModel.ActiveTo is null
                ? TimeSpan.FromHours(_orderDataModel.DurationInHours)
                : _orderDataModel.GetActiveOrderTime();

            Subscribe();
            OpenDetailsOrderCommand = new MvxRestrictedAsyncCommand(OnOpenDetailsOrderAsync, restrictedCanExecute: () => _settingsService.User != null, handleFunc: _navigationService.ShowLoginView);
            OpenUserProfileCommand = new MvxRestrictedAsyncCommand(OpenUserProfileAsync, restrictedCanExecute: () => _settingsService.User != null, handleFunc: _navigationService.ShowLoginView);
        }

        public int OrderId => _orderDataModel.Id;

        public string Title => _orderDataModel.Title;

        public string ProfilePhotoUrl => _orderDataModel.Customer?.Avatar;

        public string ProfileShortName => _orderDataModel.Customer?.Login.ToShortenName();

        public OrderType OrderType => _settingsService.User.GetOrderType(_orderDataModel.Customer?.Id, _orderDataModel?.Status ?? OrderStatusType.None);

        public OrderTagType OrderTagType => _settingsService.User.GetOrderTagType(_orderDataModel.Customer?.Id, _orderDataModel?.Status);

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
                    Unsubscribe();
                }
            }
        }

        public bool CanPlayVideo => _orderDataModel?.Video != null;

        public bool IsTimeAvailable => _elapsedTime.HasValue;

        public string TimeText => _elapsedTime?.ToTimeWithSpaceString();

        public string PriceText => _orderDataModel.Price.ToPriceString();

        public string StatusText => _orderDataModel.GetOrderStatusTitle(_settingsService?.User);

        // TODO: Update this code.
        public bool IsHiddenOrder => new Random().Next(0, 2) == 0;

        public IMvxAsyncCommand OpenDetailsOrderCommand { get; }

        public IMvxAsyncCommand OpenUserProfileCommand { get; }

        public FullScreenVideoDataModel GetFullScreenVideoDataModel()
        {
            return new FullScreenVideoDataModel(_orderDataModel.Customer.Id,
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Unsubscribe();
            }
        }

        private Task OpenUserProfileAsync()
        {
            if (_orderDataModel.Customer?.Id is null ||
                _orderDataModel.Customer.Id == _settingsService.User.Id)
            {
                return Task.CompletedTask;
            }

            return _navigationService.ShowUserProfile(_orderDataModel.Customer.Id);
        }

        private void Subscribe()
        {
            _timerTickMessageToken = _mvxMessenger.Subscribe<TimerTickMessage>(OnTimerTick, MvxReference.Strong);
        }

        private void Unsubscribe()
        {
            if (_timerTickMessageToken != null)
            {
                _mvxMessenger.Unsubscribe<TimerTickMessage>(_timerTickMessageToken);
                _timerTickMessageToken = null;
            }
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
                _mvxMessenger.Publish(new RemoveOrderMessage(this, OrderId, status));
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