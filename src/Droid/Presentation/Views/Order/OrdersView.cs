using Android.OS;
using Android.Runtime;
using Android.Views;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order;
using PrankChat.Mobile.Droid.Presentation.Views.Base;

namespace PrankChat.Mobile.Droid.Presentation.Views.Order
{
    [MvxTabLayoutPresentation(TabLayoutResourceId = Resource.Id.tabs, ViewPagerResourceId = Resource.Id.viewpager, Title = "Orders", ActivityHostViewModelType = typeof(MainViewModel))]
    [Register(nameof(OrdersView))]
    public class OrdersView : BaseTabFragment<OrdersViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.orders_layout, null);
            return view;
        }
    }
}
