using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Rating.Items
{
    public class RatingItemViewModel : BaseItemViewModel
    {
        private readonly ISettingsService _settingsService;
        private readonly INavigationService _navigatiobService;
        private readonly OrderStatusType _status;
        private readonly int? _customerId;

        private DateTime? _arbitrationFinishAt;
        private int _orderId;

        public string OrderTitle { get; }

        public string ProfilePhotoUrl { get; }

        public string TimeText => (_arbitrationFinishAt - DateTime.UtcNow)?.ToTimeWithSpaceString();

        public string CustomerShortName { get; }

        public string PriceText { get; }

        public int Likes { get; }

        public int Dislikes { get; }

        public MvxAsyncCommand OpenDetailsOrderCommand => new MvxAsyncCommand(OnOpenDetailsOrderAsync);

        public OrderType OrderType
        {
            get
            {
                return _customerId == _settingsService.User.Id
                    ? _status == OrderStatusType.New
                        ? OrderType.MyOrderInModeration
                        : OrderType.MyOrder
                        : OrderType.NotMyOrder;
            }
        }

        public RatingItemViewModel(INavigationService navigatiobService,
                                   ISettingsService settingsService,
                                   int orderId,
                                   string orderTitle,
                                   string customerPhotoUrl,
                                   string customerName,
                                   double? priceText,
                                   int likes,
                                   int dislikes,
                                   DateTime? arbitrationFinishAt,
                                   OrderStatusType status,
                                   int? customerId)
        {
            _settingsService = settingsService;
            _navigatiobService = navigatiobService;
            OrderTitle = orderTitle;
            ProfilePhotoUrl = customerPhotoUrl;
            PriceText = priceText.ToPriceString();
            Likes = likes;
            Dislikes = dislikes;
            CustomerShortName = customerName.ToShortenName();

            _arbitrationFinishAt = arbitrationFinishAt;
            _status = status;
            _customerId = customerId;
            _orderId = orderId;
        }

        private Task OnOpenDetailsOrderAsync()
        {
            return _navigatiobService.ShowDetailsOrderView(_orderId);
        }
    }
}