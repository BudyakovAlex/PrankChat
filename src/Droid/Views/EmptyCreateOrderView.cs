using Android.Runtime;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.ViewModels;
using PrankChat.Mobile.Droid.Views.Base;

namespace PrankChat.Mobile.Droid.Views
{
    [MvxTabLayoutPresentation(TabLayoutResourceId = Resource.Id.tabs, ViewPagerResourceId = Resource.Id.viewpager, ActivityHostViewModelType = typeof(MainViewModel))]
    [Register(nameof(EmptyCreateOrderView))]
    public class EmptyCreateOrderView : BaseFragment<EmptyCreateOrderViewModel>
    {
        public EmptyCreateOrderView() : base(Resource.Layout.fragment_empty_create_order)
        {
        }
    }
}
