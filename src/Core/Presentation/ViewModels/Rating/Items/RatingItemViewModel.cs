using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Rating.Items
{
    public class RatingItemViewModel : BaseItemViewModel
    {
        private readonly INavigationService _navigatiobService;
        private DateTime _orderTime;

        public string OrderTitle { get; }

        public string ProfilePhotoUrl { get; }

        public string TimeText => _orderTime.ToTimeWithSpaceString();

        public string PriceText { get; }

        public string Likes { get; }

        public string Dislikes { get; }

        public MvxAsyncCommand OpenDetailsOrderCommand => new MvxAsyncCommand(OnOpenDetailsOrderAsync);

        public RatingItemViewModel(INavigationService navigatiobService,
                                  string orderTitle,
                                  string profilePhotoUrl,
                                  double? priceText,
                                  DateTime time)
        {
            _navigatiobService = navigatiobService;
            OrderTitle = orderTitle;
            ProfilePhotoUrl = profilePhotoUrl;
            PriceText = priceText.ToPriceString();
            _orderTime = time;

            Likes = "1000";
            Dislikes = "100";
        }

        private Task OnOpenDetailsOrderAsync()
        {
            return _navigatiobService.ShowDetailsOrderView();
        }
    }
}
