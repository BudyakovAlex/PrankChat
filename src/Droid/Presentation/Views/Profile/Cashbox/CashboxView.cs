using System.ComponentModel;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Google.Android.Material.Tabs;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.ViewModels.Profile.Cashbox;
using PrankChat.Mobile.Droid.Presentation.Views.Base;

namespace PrankChat.Mobile.Droid.Presentation.Views.Profile.Cashbox
{
    [MvxActivityPresentation]
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class CashboxView : BaseView<CashboxViewModel>
    {
        private TabLayout _tabLayout;
        protected override string TitleActionBar => Core.Localization.Resources.CashboxView_Title;

        protected override bool HasBackButton => true;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle, Resource.Layout.cashbox_layout);

            if (bundle == null)
            {
                ViewModel.ShowContentCommand.Execute(null);
            }

            Window.SetBackgroundDrawableResource(Resource.Drawable.gradient_action_bar_background);

            _tabLayout = FindViewById<TabLayout>(Resource.Id.cashbox_tab_layout);
            _tabLayout.GetTabAt(0).SetText(Core.Localization.Resources.CashboxView_Fillup_Tab);
            _tabLayout.GetTabAt(1).SetText(Core.Localization.Resources.CashboxView_Withdrawal_Tab);

            _tabLayout.GetTabAt(ViewModel.SelectedPage).Select();
        }

        protected override void Subscription()
        {
            ViewModel.PropertyChanged += ViewModelOnPropertyChanged;
            _tabLayout.TabSelected += TabLayoutOnTabSelected;
        }

        protected override void Unsubscription()
        {
            ViewModel.PropertyChanged -= ViewModelOnPropertyChanged;
            _tabLayout.TabSelected -= TabLayoutOnTabSelected;
        }

        private void TabLayoutOnTabSelected(object sender, TabLayout.TabSelectedEventArgs e)
        {
            ViewModel.SelectedPage = e.Tab.Position;
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.SelectedPage))
            {
                _tabLayout.GetTabAt(ViewModel.SelectedPage).Select();
            }
        }
    }
}
