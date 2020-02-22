using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Commands;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Rating.Items
{
    public class RatingItemViewModel : BaseItemViewModel
    {
        private readonly INavigationService _navigatiobService;

        private DateTime? _arbitrationFinishAt;
        private int _orderId;

        public string OrderTitle { get; }

        public string ProfilePhotoUrl { get; }

        public string TimeText => (_arbitrationFinishAt - DateTime.UtcNow)?.ToTimeWithSpaceString();

        public string CustomerShortName { get; }

        public string PriceText { get; }

        public int Likes { get; }

        public int Dislikes { get; }

        public IMvxAsyncCommand OpenDetailsOrderCommand { get; }

        public RatingItemViewModel(INavigationService navigatiobService,
                                   bool isUserSessionInitialized,
                                   int orderId,
                                   string orderTitle,
                                   string customerPhotoUrl,
                                   string customerName,
                                   double? priceText,
                                   int likes,
                                   int dislikes,
                                   DateTime? arbitrationFinishAt)
        {
            _navigatiobService = navigatiobService;
            OrderTitle = orderTitle;
            ProfilePhotoUrl = customerPhotoUrl;
            PriceText = priceText.ToPriceString();
            Likes = likes;
            Dislikes = dislikes;
            CustomerShortName = customerName.ToShortenName();

            _arbitrationFinishAt = arbitrationFinishAt;
            _orderId = orderId;

            OpenDetailsOrderCommand = new MvxRestrictedAsyncCommand(OnOpenDetailsOrderAsync, restrictedExecute: () => isUserSessionInitialized, handleFunc: _navigatiobService.ShowLoginView);
        }

        private Task OnOpenDetailsOrderAsync()
        {
            return _navigatiobService.ShowDetailsOrderView(_orderId);
        }
    }
}
