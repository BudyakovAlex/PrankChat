using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile.Cashbox;
using PrankChat.Mobile.Droid.Presentation.Views.Base;

namespace PrankChat.Mobile.Droid.Presentation.Views.Profile.Cashbox
{
    [MvxTabLayoutPresentation(TabLayoutResourceId = Resource.Id.cashbox_tab_layout, ViewPagerResourceId = Resource.Id.cashbox_pager, ActivityHostViewModelType = typeof(CashboxViewModel))]
    [Register(nameof(WithdrawalView))]
    public class WithdrawalView : BaseFragment<WithdrawalViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.withdrawal_layout, null);
            var availableAmountTextView = view.FindViewById<TextView>(Resource.Id.withdrawal_available_amount_text);
            availableAmountTextView.PaintFlags = availableAmountTextView.PaintFlags | PaintFlags.UnderlineText;
            return view;
        }

        protected override void Subscription()
        {
        }

        protected override void Unsubscription()
        {
        }
    }
}
