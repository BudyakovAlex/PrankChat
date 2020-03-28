using Android.Views;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order.Items;
using PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Abstract;

namespace PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Orders
{
    public class OrderItemViewHolder : CardViewHolder<OrderItemViewModel>
    {
        public OrderItemViewHolder(View view, IMvxAndroidBindingContext context) : base(view, context)
        {
        }
    }
}
