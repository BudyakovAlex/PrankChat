﻿using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.ApplicationServices.Timer;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Order.Items
{
    public class OrderItemViewModel : BaseItemViewModel, IDisposable
    {
        private readonly INavigationService _navigationService;
        private readonly ISettingsService _settingsService;
        private readonly IMvxMessenger _mvxMessenger;
        private DateTime? _activeTo;
        private OrderStatusType _status;
        private int _orderId;
        private int? _customerId;
        private TimeSpan? _elapsedTime;
        private MvxSubscriptionToken _timerTickMessageToken;

        public string Title { get; }

        public string ProfilePhotoUrl { get; }

        public TimeSpan? ElapsedTime
        {
            get => _elapsedTime;
            set
            {
                if(SetProperty(ref _elapsedTime, value))
                {
                    RaisePropertyChanged(nameof(TimeText));
                }
            }
        }

        public string TimeText => _elapsedTime?.ToString("dd' : 'hh' : 'mm");

        public string PriceText { get; }

        public string StatusText
        {
            get
            {
                if (_settingsService.User?.Id == _customerId)
                {
                    return Resources.OrderStatus_MyOrder;
                }

                switch (_status)
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
                        return Resources.OrderStatus_InWork;

                    case OrderStatusType.InArbitration:
                        return Resources.OrderStatus_InArbitration;

                    case OrderStatusType.ProcessCloseArbitration:
                        return Resources.OrderStatus_ProcessCloseArbitration;

                    case OrderStatusType.ClosedAfterArbitrationCustomerWin:
                        return Resources.OrderStatus_ClosedAfterArbitrationCustomerWin;

                    case OrderStatusType.ClosedAfterArbitrationExecutorWin:
                        return Resources.OrderStatus_ClosedAfterArbitrationExecutorWin;

                    case OrderStatusType.Finished:
                        return Resources.OrderStatus_Finished;

                    default:
                        return string.Empty;
                }
            }
        }

        public MvxAsyncCommand OpenDetailsOrderCommand => new MvxAsyncCommand(OnOpenDetailsOrderAsync);

        public OrderItemViewModel(INavigationService navigationService,
                                  ISettingsService settingsService,
                                  IMvxMessenger mvxMessenger,
                                  int orderId,
                                  string orderTitle,
                                  string profilePhotoUrl,
                                  double? price,
                                  DateTime? activeTo,
                                  OrderStatusType status,
                                  int? customerId)
        {
            _navigationService = navigationService;
            _settingsService = settingsService;
            _mvxMessenger = mvxMessenger;

            Title = orderTitle;
            ProfilePhotoUrl = profilePhotoUrl;
            PriceText = price.ToPriceString();
            _activeTo = activeTo;
            _status = status;
            _orderId = orderId;
            _customerId = customerId;

            _timerTickMessageToken = mvxMessenger.Subscribe<TimerTickMessage>(OnTimerTick, MvxReference.Strong);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && _timerTickMessageToken != null)
            {
                _mvxMessenger.Unsubscribe<TimerTickMessage>(_timerTickMessageToken);
                _timerTickMessageToken = null;
            }
        }

        private void OnTimerTick(TimerTickMessage message)
        {
            ElapsedTime = DateTime.Now - _activeTo?.ToLocalTime();
        }

        private Task OnOpenDetailsOrderAsync()
        {
            var parameter = new OrderDetailsNavigationParameter(_orderId);
            return _navigationService.ShowDetailsOrderView(parameter);
        }
    }
}
