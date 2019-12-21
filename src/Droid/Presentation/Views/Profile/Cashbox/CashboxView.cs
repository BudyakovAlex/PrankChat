using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile.Cashbox;
using PrankChat.Mobile.Droid.Presentation.Views.Base;

namespace PrankChat.Mobile.Droid.Presentation.Views.Profile.Cashbox
{
    [MvxActivityPresentation]
    [Activity]
    public class CashboxView : BaseView<CashboxViewModel>
    {
        protected override string TitleActionBar => Core.Presentation.Localization.Resources.CashboxView_Title;

        protected override bool HasBackButton => true;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle, Resource.Layout.cashbox_layout);

            if (bundle == null)
            {
                ViewModel.ShowContentCommand.Execute(null);
            }

            var tabLayout = FindViewById<TabLayout>(Resource.Id.cashbox_tab_layout);
            tabLayout.GetTabAt(0).SetText(Core.Presentation.Localization.Resources.CashboxView_Fillup_Tab);
            tabLayout.GetTabAt(1).SetText(Core.Presentation.Localization.Resources.CashboxView_Withdrawal_Tab);
        }

        protected override void Subscription()
        {
        }

        protected override void Unsubscription()
        {
        }
    }
}
