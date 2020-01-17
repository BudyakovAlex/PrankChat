using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Order.Items
{
    public class OrderItemViewModel : BaseItemViewModel
    {
        private readonly INavigationService _navigatiobService;
        private readonly ISettingsService _settingsService;

        private TimeSpan _orderTime;
        private OrderStatusType _status;

        public string Title { get; }

        public string ProfilePhotoUrl { get; }

        public string TimeText => _orderTime.ToString("dd' : 'hh' : 'mm");

        public string PriceText { get; }

        public string StatusText
        {
            get
            {
                switch (_status)
                {
                    case OrderStatusType.New:
                        break;
                    case OrderStatusType.Rejected:
                        break;
                    case OrderStatusType.Cancelled:
                        break;
                    case OrderStatusType.Active:
                        break;
                    case OrderStatusType.InWork:
                        break;
                    case OrderStatusType.InArbitration:
                        break;
                    case OrderStatusType.ProcessCloseArbitration:
                        break;
                    case OrderStatusType.ClosedAfterArbitrationCustomerWin:
                        break;
                    case OrderStatusType.ClosedAfterArbitrationExecutorWin:
                        break;
                    case OrderStatusType.Finished:
                        break;
                    default:
                        break;
                }

                return "LOL";
            }
        }

        public MvxAsyncCommand OpenDetailsOrderCommand => new MvxAsyncCommand(OnOpenDetailsOrderAsync);

        public OrderItemViewModel(INavigationService navigatiobService,
                                  ISettingsService settingsService,
                                  string orderTitle,
                                  string profilePhotoUrl,
                                  long price,
                                  TimeSpan time,
                                  OrderStatusType status)
        {
            _navigatiobService = navigatiobService;
            _settingsService = settingsService;

            Title = orderTitle;
            ProfilePhotoUrl = profilePhotoUrl;
            PriceText = $"{price} P";
            _orderTime = time;
            _status = status;
        }

        private Task OnOpenDetailsOrderAsync()
        {
            return _navigatiobService.ShowDetailsOrderView();
        }
    }
}
