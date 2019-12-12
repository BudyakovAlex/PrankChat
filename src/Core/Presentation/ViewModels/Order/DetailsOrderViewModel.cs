using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Order
{
    public class DetailsOrderViewModel : BaseViewModel
    {
        public string ProfilePhotoUrl { get; set; } = "https://ksassets.timeincuk.net/wp/uploads/sites/55/2019/04/GettyImages-1136749971-920x584.jpg";

        public string ProfileName { get; set; } = "Володя";

        public string VideoName { get; set; } = "Скушать 1 кг томатов за 10 минут";

        public string VideoDetails { get; set; } = "Полное описание видео. Полное описание видео. Скушать 1 кг томатов за 10 минут";

        public string PriceText { get; set; } = "10 000 ₽";

        public string TimeText { get; set; } = "22 : 12 : 11";

        public MvxAsyncCommand TakeTheOrderCommand => new MvxAsyncCommand(OnTakeOrderAsync);

        public MvxAsyncCommand SubscribeTheOrderCommand => new MvxAsyncCommand(OnSubscribeTheOrderAsync);

        public MvxAsyncCommand UnsubscribeTheOrderCommand => new MvxAsyncCommand(OnUnsubscribeTheOrderAsync);

        public DetailsOrderViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        private Task OnTakeOrderAsync()
        {
            return Task.CompletedTask;
        }

        private Task OnSubscribeTheOrderAsync()
        {
            return Task.CompletedTask;
        }

        private Task OnUnsubscribeTheOrderAsync()
        {
            return Task.CompletedTask;
        }
    }
}
