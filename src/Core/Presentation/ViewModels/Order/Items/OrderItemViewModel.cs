using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.ApplicationServices.Timer;
using PrankChat.Mobile.Core.Commands;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Order.Items
{
    public class OrderItemViewModel : BaseItemViewModel, IDisposable
    {
        private readonly INavigationService _navigationService;
        private readonly ISettingsService _settingsService;
        private readonly IMvxMessenger _mvxMessenger;

        private readonly OrderDataModel _orderDataModel;

        private MvxSubscriptionToken _timerTickMessageToken;

        public int OrderId => _orderDataModel.Id;

        public string Title => _orderDataModel.Title;

        public string ProfilePhotoUrl => _orderDataModel.Customer?.Avatar;

        public string ProfileShortName => _orderDataModel.Customer?.Name.ToShortenName();

        public OrderType OrderType => _settingsService.User.GetOrderType(_orderDataModel.Customer?.Id, _orderDataModel?.Status ?? OrderStatusType.None);

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

        public bool IsTimeAvailable => _elapsedTime > new TimeSpan();

        public string TimeText => _elapsedTime?.ToTimeWithSpaceString();

        public string PriceText => _orderDataModel.Price.ToPriceString();

        public string StatusText
        {
            get
            {
                switch (_orderDataModel.Status)
                {
                    case OrderStatusType.New:
                        return Resources.OrderStatus_New;

                    case OrderStatusType.Rejected:
                        return Resources.OrderStatus_Rejected;

                    case OrderStatusType.Cancelled:
                        return Resources.OrderStatus_Cancelled;

                    case OrderStatusType.Active:
                        return Resources.OrderStatus_Active;

                    case OrderStatusType.InWork:
                    case OrderStatusType.VideoWaitModeration:
                    case OrderStatusType.VideoInProcess:
                    case OrderStatusType.VideoProcessError:
                        return Resources.OrderStatus_InWork;

                    case OrderStatusType.InArbitration:
                        return Resources.OrderStatus_InArbitration;

                    case OrderStatusType.ProcessCloseArbitration:
                        return Resources.OrderStatus_ProcessCloseArbitration;

                    case OrderStatusType.ClosedAfterArbitrationCustomerWin:
                        return Resources.OrderStatus_ClosedAfterArbitrationCustomerWin;

                    case OrderStatusType.ClosedAfterArbitrationExecutorWin:
                        return Resources.OrderStatus_ClosedAfterArbitrationExecutorWin;

                    case OrderStatusType.WaitFinish:
                        return Resources.OrderStatus_WaitFinish;

                    case OrderStatusType.Finished:
                        return Resources.OrderStatus_Finished;

                    default:
                        return string.Empty;
                }
            }
        }

        public IMvxAsyncCommand OpenDetailsOrderCommand { get; }

        public OrderItemViewModel(INavigationService navigationService,
                                  ISettingsService settingsService,
                                  IMvxMessenger mvxMessenger,
                                  OrderDataModel orderDataModel)
        {
            _navigationService = navigationService;
            _settingsService = settingsService;
            _mvxMessenger = mvxMessenger;
            _orderDataModel = orderDataModel;

            ElapsedTime = _orderDataModel.ActiveTo is null
                ? TimeSpan.FromHours(_orderDataModel.DurationInHours)
                : _orderDataModel.GetActiveOrderTime();

            Subscribe();
            OpenDetailsOrderCommand = new MvxRestrictedAsyncCommand(OnOpenDetailsOrderAsync, restrictedCanExecute: () => _settingsService.User != null, handleFunc: _navigationService.ShowLoginView);
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
            var result = await _navigationService.ShowOrderDetailsView(OrderId);
            if (result == null)
                return;

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

            await RaisePropertyChanged(nameof(StatusText));
            await RaisePropertyChanged(nameof(OrderType));
        }
    }
}