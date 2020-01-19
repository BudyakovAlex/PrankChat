using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Rating.Items
{
    public class RatingItemViewModel : BaseItemViewModel
    {
        private readonly INavigationService _navigatiobService;

        private DateTime _orderTime;
        private int _orderId;

        public string OrderTitle { get; }

        public string ProfilePhotoUrl { get; }

        public string TimeText => _orderTime.ToString("dd : hh : mm");

        public string PriceText { get; }

        public string Likes { get; }

        public string Dislikes { get; }

        public MvxAsyncCommand OpenDetailsOrderCommand => new MvxAsyncCommand(OnOpenDetailsOrderAsync);

        public RatingItemViewModel(INavigationService navigatiobService,
                                  int orderId,
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
            _orderId = orderId;

            Likes = "1000";
            Dislikes = "100";
        }

        private Task OnOpenDetailsOrderAsync()
        {
            var parameter = new OrderDetailsNavigationParameter(_orderId);
            return _navigatiobService.ShowDetailsOrderView(parameter);
        }
    }
}
