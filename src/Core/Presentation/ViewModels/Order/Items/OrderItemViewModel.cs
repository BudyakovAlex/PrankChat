using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Order.Items
{
    public class OrderItemViewModel : BaseItemViewModel
    {
        private readonly INavigationService _navigatiobService;

        private TimeSpan _orderTime;
        private OrderStatusType _status;

        public string Title { get; }

        public string ProfilePhotoUrl { get; }

        public string TimeText => _orderTime.ToString("dd' : 'hh' : 'mm");

        public string PriceText { get; }

        public MvxAsyncCommand OpenDetailsOrderCommand => new MvxAsyncCommand(OnOpenDetailsOrderAsync);

        public OrderItemViewModel(INavigationService navigatiobService,
                                  string orderTitle,
                                  string profilePhotoUrl,
                                  long price,
                                  TimeSpan time,
                                  OrderStatusType status)
        {
            _navigatiobService = navigatiobService;
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
