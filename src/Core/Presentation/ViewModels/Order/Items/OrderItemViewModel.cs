using System;
using System.Threading.Tasks;
using MvvmCross.Commands;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Order.Items
{
    public class OrderItemViewModel : BaseItemViewModel
    {
        private DateTime _orderTime;

        public string OrderTitle { get; }

        public string ProfilePhotoUrl { get; }

        public string TimeText => $"{_orderTime.Day} : {_orderTime.Hour} : {_orderTime.Minute}";

        public string PriceText { get; }

        public MvxAsyncCommand OpenDetailsOrderCommand => new MvxAsyncCommand(OnOpenDetailsOrderAsync);

        public OrderItemViewModel()
        {
        }

        private Task OnOpenDetailsOrderAsync()
        {
        }
    }
}
