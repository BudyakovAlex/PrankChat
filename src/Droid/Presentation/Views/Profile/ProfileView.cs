using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile;
using PrankChat.Mobile.Droid.Presentation.Views.Base;

namespace PrankChat.Mobile.Droid.Presentation.Views.Profile
{
    [MvxTabLayoutPresentation(TabLayoutResourceId = Resource.Id.tabs, ViewPagerResourceId = Resource.Id.viewpager, ActivityHostViewModelType = typeof(MainViewModel))]
    [Register(nameof(ProfileView))]
    public class ProfileView : BaseTabFragment<ProfileViewModel>, TabLayout.IOnTabSelectedListener
    {
        protected override string TitleActionBar => Core.Presentation.Localization.Resources.Profile_Tab;

        public ProfileView()
        {
            HasOptionsMenu = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.profile_layout, null);

            var tabLayout = view.FindViewById<TabLayout>(Resource.Id.publication_type_tab_layout);
            tabLayout.AddOnTabSelectedListener(this);

            return view;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            menu.Clear();
            inflater.Inflate(Resource.Menu.profile_menu, menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.profile_menu_button:
                    ViewModel.ShowMenuCommand.Execute();
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        public void OnTabReselected(TabLayout.Tab tab)
        {
        }

        public void OnTabSelected(TabLayout.Tab tab)
        {
            switch (tab.Position)
            {
                case 0:
                    ViewModel.SelectedOrderType = ProfileOrderType.MyOrders;
                    break;
                case 1:
                    ViewModel.SelectedOrderType = ProfileOrderType.OrdersCompletedByMe;
                    break;
            }
        }

        public void OnTabUnselected(TabLayout.Tab tab)
        {
        }
    }
}
