using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.Droid.Presentation.Views.Base;
using Localization = PrankChat.Mobile.Core.Presentation.Localization.Resources;

namespace PrankChat.Mobile.Droid.Presentation.Views
{
    [Activity(LaunchMode = LaunchMode.SingleTop)]
    public class MainView : BaseView<MainViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle, Resource.Layout.main_view_layout);

            if (bundle == null)
            {
                ViewModel.ShowContentCommand.Execute();
            }

            CreateTabs();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.main_view_menu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.notification_button:
                    ViewModel.ShowNotificationCommand.Execute();
                    return true;

                case Resource.Id.search_button:
                    ViewModel.ShowNotificationCommand.Execute();
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        private void CreateTabs()
        {
            var tabLayout = FindViewById<TabLayout>(Resource.Id.tabs);
            tabLayout.TabSelected += TabLayoutOnTabSelected;
            tabLayout.TabUnselected += TabLayoutOnTabUnselected;
            tabLayout.SetTabTextColors(Resource.Color.applicationBlack, Resource.Color.inactive);
            var inflater = LayoutInflater.FromContext(Application.Context);
            InitTab(0, Resource.Drawable.ic_home, Localization.Home_Tab, tabLayout, inflater);
            InitTab(1, Resource.Drawable.ic_rate, Localization.Rate_Tab, tabLayout, inflater);
            InitCentralTab(Resource.Drawable.ic_create_order, null, tabLayout, inflater);
            InitTab(3, Resource.Drawable.ic_orders, Localization.Orders_Tab, tabLayout, inflater);
            InitTab(4, Resource.Drawable.ic_profile, Localization.Profile_Tab, tabLayout, inflater);

            TabLayoutOnTabSelected(this, new TabLayout.TabSelectedEventArgs(tabLayout.GetTabAt(0)));
        }

        private void TabLayoutOnTabSelected(object sender, TabLayout.TabSelectedEventArgs e)
        {
            var iconView = e.Tab.View.FindViewById<ImageView>(Resource.Id.tab_icon);
            var textView = e.Tab.View.FindViewById<TextView>(Resource.Id.tab_title);

            textView?.SetTextColor(Resources.GetColor(Resource.Color.applicationBlack));

            if (e.Tab.Position == 4)
            {
                iconView.SetBackgroundResource(Resource.Drawable.gradient_oval);
                return;
            }

            if (e.Tab.Position == 2)
            {
                return;
            }

            iconView.Drawable.SetColorFilter(Resources.GetColor(Resource.Color.accent), PorterDuff.Mode.SrcIn);
        }

        private void TabLayoutOnTabUnselected(object sender, TabLayout.TabUnselectedEventArgs e)
        {
            var iconView = e.Tab.View.FindViewById<ImageView>(Resource.Id.tab_icon);
            var textView = e.Tab.View.FindViewById<TextView>(Resource.Id.tab_title);

            textView?.SetTextColor(Resources.GetColor(Resource.Color.inactive));

            if (e.Tab.Position == 4)
            {
                iconView.SetBackgroundResource(Color.Transparent);
                return;
            }

            if (e.Tab.Position == 2)
            {
                return;
            }

            iconView.Drawable.ClearColorFilter();
        }

        private void InitTab(int index, int iconResource, string title, TabLayout tabLayout, LayoutInflater inflater)
        {
            var tabView = inflater.Inflate(Resource.Layout.tab_button_layout, null);
            var textView = tabView.FindViewById<TextView>(Resource.Id.tab_title);
            var iconView = tabView.FindViewById<ImageView>(Resource.Id.tab_icon);
            textView.Text = title;
            iconView.SetImageResource(iconResource);
            var tab = tabLayout.GetTabAt(index);
            tab.SetCustomView(tabView);
        }

        private void InitCentralTab(int iconResource, string title, TabLayout tabLayout, LayoutInflater inflater)
        {
            var tabView = (ImageView)inflater.Inflate(Resource.Layout.central_tab_button_layout, null);
            tabView.SetImageResource(iconResource);
            var tab = tabLayout.GetTabAt(2);
            tab.SetCustomView(tabView);
        }
    }
}
