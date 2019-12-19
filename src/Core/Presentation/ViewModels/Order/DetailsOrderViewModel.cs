using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Order
{
    public class OrderDetailsViewModel : BaseViewModel
    {
        #region Profile

        public string ProfilePhotoUrl { get; set; } = "https://ksassets.timeincuk.net/wp/uploads/sites/55/2019/04/GettyImages-1136749971-920x584.jpg";

        public string ProfileName { get; set; } = "Володя";

        #endregion

        #region Video

        public string VideoUrl { get; set; } = "https://ksassets.timeincuk.net/wp/uploads/sites/55/2019/04/GettyImages-1136749971-920x584.jpg";

        public string VideoName { get; set; } = "Скушать 1 кг томатов за 10 минут";

        public string VideoDetails { get; set; } = "Полное описание видео. Полное описание видео. Скушать 1 кг томатов за 10 минут";

        #endregion

        #region Executor

        public string ExecutorPhotoUrl { get; set; } = "https://ksassets.timeincuk.net/wp/uploads/sites/55/2019/04/GettyImages-1136749971-920x584.jpg";

        public string ExecutorName { get; set; } = "Евгений";

        public string StartOrderDate { get; set; } = DateTime.Now.ToShortDateString();

        #endregion

        #region PhotoSetting

        public List<ITransformation> Transformations => new List<ITransformation> { new CircleTransformation() };

        public double DownsampleWidth { get; } = 100;

        #endregion

        public string PriceValue { get; set; } = "10 000 ₽";

        public string TimeValue { get; set; } = "22 : 12 : 11";

        public MvxAsyncCommand TakeOrderCommand => new MvxAsyncCommand(OnTakeOrderAsync);

        public MvxAsyncCommand SubscribeTheOrderCommand => new MvxAsyncCommand(OnSubscribeOrderAsync);

        public MvxAsyncCommand UnsubscribeOrderCommand => new MvxAsyncCommand(OnUnsubscribeOrderAsync);

        public MvxAsyncCommand YesCommand => new MvxAsyncCommand(OnYesAsync);

        public MvxAsyncCommand NoCommand => new MvxAsyncCommand(OnNoAsync);

        public MvxAsyncCommand DownloadOrderCommand => new MvxAsyncCommand(OnDownloadOrderAsync);

        public MvxAsyncCommand ExecuteOrderCommand => new MvxAsyncCommand(OnExecuteOrderAsync);

        public MvxAsyncCommand CancelOrderCommand => new MvxAsyncCommand(OnCancelOrderAsync);

        public MvxAsyncCommand ArqueOrderCommand => new MvxAsyncCommand(OnArqueOrderAsync);

        public MvxAsyncCommand AcceptOrderCommand => new MvxAsyncCommand(OnAcceptOrderAsync);

        public OrderDetailsViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        private Task OnTakeOrderAsync()
        {
            return Task.CompletedTask;
        }

        private Task OnSubscribeOrderAsync()
        {
            return Task.CompletedTask;
        }

        private Task OnUnsubscribeOrderAsync()
        {
            return Task.CompletedTask;
        }

        private Task OnDownloadOrderAsync()
        {
            return Task.CompletedTask;
        }

        private Task OnArqueOrderAsync()
        {
            return Task.CompletedTask;
        }

        private Task OnAcceptOrderAsync()
        {
            return Task.CompletedTask;
        }

        private Task OnCancelOrderAsync()
        {
            return Task.CompletedTask;
        }

        private Task OnExecuteOrderAsync()
        {
            return Task.CompletedTask;
        }

        private Task OnYesAsync()
        {
            return Task.CompletedTask;
        }

        private Task OnNoAsync()
        {
            return Task.CompletedTask;
        }
    }
}
