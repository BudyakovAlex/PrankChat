using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Order
{
    public class DetailsOrderViewModel : BaseViewModel
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

        public MvxAsyncCommand TakeTheOrderCommand => new MvxAsyncCommand(OnTakeOrderAsync);

        public MvxAsyncCommand SubscribeTheOrderCommand => new MvxAsyncCommand(OnSubscribeTheOrderAsync);

        public MvxAsyncCommand UnsubscribeTheOrderCommand => new MvxAsyncCommand(OnUnsubscribeTheOrderAsync);

        public MvxAsyncCommand YesCommand => new MvxAsyncCommand(OnYesAsync);

        public MvxAsyncCommand NoCommand => new MvxAsyncCommand(OnNoAsync);

        public MvxAsyncCommand DownloadTheOrderCommand => new MvxAsyncCommand(OnDownloadTheOrderAsync);

        public MvxAsyncCommand ExecuteTheOrderCommand => new MvxAsyncCommand(OnExecuteTheOrderAsync);

        public MvxAsyncCommand CancelTheOrderCommand => new MvxAsyncCommand(OnCancelTheOrderAsync);

        public MvxAsyncCommand ArqueTheOrderCommand => new MvxAsyncCommand(OnArqueTheOrderAsync);

        public MvxAsyncCommand AcceptTheOrderCommand => new MvxAsyncCommand(OnAcceptTheOrderAsync);

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

        private Task OnDownloadTheOrderAsync()
        {
            return Task.CompletedTask;
        }

        private Task OnArqueTheOrderAsync()
        {
            return Task.CompletedTask;
        }

        private Task OnAcceptTheOrderAsync()
        {
            return Task.CompletedTask;
        }

        private Task OnCancelTheOrderAsync()
        {
            return Task.CompletedTask;
        }

        private Task OnExecuteTheOrderAsync()
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
