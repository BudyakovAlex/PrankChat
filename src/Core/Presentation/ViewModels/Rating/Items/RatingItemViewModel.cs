using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Rating.Items
{
    public class RatingItemViewModel : BaseItemViewModel
    {
        private readonly INavigationService _navigatiobService;

        private DateTime _arbitrationFinishAt;
        private int _orderId;

        public string OrderTitle { get; }

        public string ProfilePhotoUrl { get; }

        public string TimeText => (_arbitrationFinishAt - DateTime.UtcNow).ToTimeWithSpaceString();

        public string PriceText { get; }

        public int Likes { get; }

        public int Dislikes { get; }

        public MvxAsyncCommand OpenDetailsOrderCommand => new MvxAsyncCommand(OnOpenDetailsOrderAsync);

        public RatingItemViewModel(INavigationService navigatiobService,
                                  int orderId,
                                  string orderTitle,
                                  string profilePhotoUrl,
                                  double? priceText,
                                  int likes,
                                  int dislikes,
                                  DateTime arbitrationFinishAt)
        {
            _navigatiobService = navigatiobService;
            OrderTitle = orderTitle;
            ProfilePhotoUrl = profilePhotoUrl;
            PriceText = priceText.ToPriceString();
            Likes = likes;
            Dislikes = dislikes;

            _arbitrationFinishAt = arbitrationFinishAt;
            _orderId = orderId;
        }

        private Task OnOpenDetailsOrderAsync()
        {
            return _navigatiobService.ShowDetailsOrderView(_orderId);
        }
    }
}
