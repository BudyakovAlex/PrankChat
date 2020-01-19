using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Order.Items
{
    public class OrderItemViewModel : BaseItemViewModel
    {
        private readonly INavigationService _navigatiobService;
        private readonly ISettingsService _settingsService;

        private TimeSpan? _orderTime;
        private OrderStatusType _status;
        private int _orderId;
        private int? _customerId;

        public string Title { get; }

        public string ProfilePhotoUrl { get; }

        public string TimeText => _orderTime?.ToString("dd' : 'hh' : 'mm");

        public string PriceText { get; }

        public string StatusText
        {
            get
            {
                if (_settingsService.User?.Id == _customerId)
                {
                    return "Мое задание";
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

        public OrderItemViewModel(INavigationService navigatiobService,
                                  ISettingsService settingsService,
                                  int orderId,
                                  string orderTitle,
                                  string profilePhotoUrl,
                                  long? price,
                                  TimeSpan? time,
                                  OrderStatusType status,
                                  int? customerId)
        {
            _navigatiobService = navigatiobService;
            _settingsService = settingsService;

            Title = orderTitle;
            ProfilePhotoUrl = profilePhotoUrl;
            PriceText = $"{price} P";
            _orderTime = time;
            _status = status;
            _orderId = orderId;
            _customerId = customerId;
        }

        private Task OnOpenDetailsOrderAsync()
        {
            var parameter = new OrderDetailsNavigationParameter(_orderId);
            return _navigatiobService.ShowDetailsOrderView(parameter);
        }
    }
}
