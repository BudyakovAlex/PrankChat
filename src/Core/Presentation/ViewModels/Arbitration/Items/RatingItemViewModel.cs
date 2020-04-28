using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Commands;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Arbitration.Items
{
    public class ArbitrationItemViewModel : BaseItemViewModel
    {
        private readonly ISettingsService _settingsService;
        private readonly INavigationService _navigationService;

        private readonly int? _customerId;
        private readonly DateTime? _arbitrationFinishAt;
        private readonly int _orderId;

        public string OrderTitle { get; }

        public string ProfilePhotoUrl { get; }

        public string TimeText => (_arbitrationFinishAt - DateTime.UtcNow)?.ToTimeWithSpaceString();

        public string ProfileShortName { get; }

        public string PriceText { get; }

        private int _likes;
        public int Likes
        {
            get => _likes;
            set => SetProperty(ref _likes, value);
        }

        private int _dislikes;
        public int Dislikes
        {
            get => _dislikes;
            set => SetProperty(ref _dislikes, value);
        }

        public IMvxAsyncCommand OpenDetailsOrderCommand { get; }

        public OrderType OrderType => _settingsService.User?.Id == _customerId ? OrderType.MyOrder : OrderType.NotMyOrder;

        public ArbitrationItemViewModel(INavigationService navigatiobService,
                                        ISettingsService settingsService,
                                        bool isUserSessionInitialized,
                                        int orderId,
                                        string orderTitle,
                                        string customerPhotoUrl,
                                        string customerName,
                                        double? priceText,
                                        int likes,
                                        int dislikes,
                                        DateTime? arbitrationFinishAt,
                                        int? customerId)
        {
            _settingsService = settingsService;
            _navigationService = navigatiobService;
            OrderTitle = orderTitle;
            ProfilePhotoUrl = customerPhotoUrl;
            PriceText = priceText.ToPriceString();
            Likes = likes;
            Dislikes = dislikes;
            ProfileShortName = customerName.ToShortenName();

            _arbitrationFinishAt = arbitrationFinishAt;
            _customerId = customerId;
            _orderId = orderId;

            OpenDetailsOrderCommand = new MvxRestrictedAsyncCommand(OnOpenDetailsOrderAsync, restrictedCanExecute: () => isUserSessionInitialized, handleFunc: _navigationService.ShowLoginView);
        }

        private async Task OnOpenDetailsOrderAsync()
        {
            var result = await _navigationService.ShowOrderDetailsView(_orderId);
            if (result == null)
                return;

            Likes = result.PositiveArbitrationValuesCount ?? 0;
            Dislikes = result.NegativeArbitrationValuesCount ?? 0;
        }
    }
}
