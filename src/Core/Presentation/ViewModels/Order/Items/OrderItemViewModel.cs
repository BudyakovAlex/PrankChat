using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Order.Items
{
    public class OrderItemViewModel : BaseItemViewModel
    {
        private readonly INavigationService _navigatiobService;
        private DateTime _orderTime;

        public string OrderTitle { get; }

        public string ProfilePhotoUrl { get; }

        public string TimeText => _orderTime.ToString("dd : hh : mm");

        public string PriceText { get; }

        public MvxAsyncCommand OpenDetailsOrderCommand => new MvxAsyncCommand(OnOpenDetailsOrderAsync);

        public OrderItemViewModel(INavigationService navigatiobService,
                                  string orderTitle,
                                  string profilePhotoUrl,
                                  string priceText,
                                  DateTime time)
        {
            _navigatiobService = navigatiobService;
            OrderTitle = orderTitle;
            ProfilePhotoUrl = profilePhotoUrl;
            PriceText = priceText;
            _orderTime = time;
        }

        private Task OnOpenDetailsOrderAsync()
        {
            return _navigatiobService.ShowDetailsOrderView();
        }
    }
}
