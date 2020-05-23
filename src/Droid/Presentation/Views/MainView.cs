using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using FFImageLoading.Cross;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.Droid.Controls;
using PrankChat.Mobile.Droid.Presentation.Listeners;
using PrankChat.Mobile.Droid.Presentation.Views.Base;
using PrankChat.Mobile.Droid.Presenters.Attributes;
using PrankChat.Mobile.Droid.Utils.Helpers;
using Localization = PrankChat.Mobile.Core.Presentation.Localization.Resources;

namespace PrankChat.Mobile.Droid.Presentation.Views
{
    [ClearStackActivityPresentation]
    [Activity(LaunchMode = LaunchMode.SingleTop,
              Theme = "@style/Theme.PrankChat.Base.Dark",
              ScreenOrientation = ScreenOrientation.Portrait,
              ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.KeyboardHidden)]
    public class MainView : BaseView<MainViewModel>
    {
        private readonly ViewOnTouchListener _tabViewOnTouchListener;

        private TabLayout _tabLayout;
        private IMenuItem _infoMenuItem;

        public MainView()
        {
            _tabViewOnTouchListener = new ViewOnTouchListener(OnTabItemTouched);
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle, Resource.Layout.main_view_layout);
            ViewModel.ShowContentCommand.Execute();

            Window.SetBackgroundDrawableResource(Resource.Drawable.gradient_action_bar_background);

            var viewPager = FindViewById<ApplicationSwipeViewPager>(Resource.Id.viewpager);
            viewPager.OffscreenPageLimit = 5;
            _tabLayout = FindViewById<TabLayout>(Resource.Id.tabs);

            CreateTabs();
            RequestedOrientation = ScreenOrientation.Locked;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            menu.Clear();
            MenuInflater.Inflate(Resource.Menu.main_view_menu, menu);
            _infoMenuItem = menu.GetItem(0);
            _infoMenuItem?.SetVisible(_tabLayout?.SelectedTabPosition != 0);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.notification_button:
                    ViewModel.ShowNotificationCommand.Execute();
                    return true;
                case Resource.Id.info_button:
                    ViewModel.ShowWalkthrouthCommand?.Execute(_tabLayout.SelectedTabPosition);
                    return true;

                case Resource.Id.search_button:
                    ViewModel.ShowSearchCommand.Execute();
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        protected override void Subscription()
        {
            _tabLayout.TabSelected += TabLayoutOnTabSelected;
            _tabLayout.TabUnselected += TabLayoutOnTabUnselected;
        }

        protected override void Unsubscription()
        {
            _tabLayout.TabSelected -= TabLayoutOnTabSelected;
            _tabLayout.TabUnselected -= TabLayoutOnTabUnselected;
        }

        private void CreateTabs()
        {
            _tabLayout.SetTabTextColors(Resource.Color.applicationBlack, Resource.Color.inactive);
            var inflater = LayoutInflater.FromContext(Application.Context);
            InitTab(0, Resource.Drawable.ic_home, Localization.Home_Tab, _tabLayout, inflater);
            InitTab(1, Resource.Drawable.ic_competitions, Localization.Competitions_Tab, _tabLayout, inflater);
            InitCentralTab(Resource.Drawable.ic_create_order, _tabLayout, inflater);
            InitTab(3, Resource.Drawable.ic_orders, Localization.Orders_Tab, _tabLayout, inflater);
            InitTab(4, Resource.Drawable.ic_profile, Localization.Profile_Tab, _tabLayout, inflater);

            TabLayoutOnTabSelected(this, new TabLayout.TabSelectedEventArgs(_tabLayout.GetTabAt(0)));
        }

        private void TabLayoutOnTabSelected(object sender, TabLayout.TabSelectedEventArgs e)
        {
            if (e.Tab == null)
            {
                return;
            }

            _infoMenuItem?.SetVisible(e.Tab.Position != 0);
            var iconView = e.Tab.View.FindViewById<ImageView>(Resource.Id.tab_icon);
            var textView = e.Tab.View.FindViewById<TextView>(Resource.Id.tab_title);

            textView?.SetTextColor(ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.applicationBlack));

            if (e.Tab.Position == 2)
            {
                ViewModel.ShowWalkthrouthIfNeedCommand?.Execute(_tabLayout.SelectedTabPosition);
                return;
            }

            var colorFilter = ContextCompat.GetColor(ApplicationContext, Resource.Color.accent);
            iconView.Drawable.SetColorFilter(ColorUtil.GetColorFromInteger(colorFilter), PorterDuff.Mode.SrcIn);
            ViewModel.ShowWalkthrouthIfNeedCommand?.Execute(_tabLayout.SelectedTabPosition);
        }

        private void TabLayoutOnTabUnselected(object sender, TabLayout.TabUnselectedEventArgs e)
        {
            var iconView = e.Tab.View.FindViewById<ImageView>(Resource.Id.tab_icon);
            var textView = e.Tab.View.FindViewById<TextView>(Resource.Id.tab_title);

            textView?.SetTextColor(ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.inactive));

            if (e.Tab.Position == 2)
            {
                return;
            }

            iconView.Drawable.ClearColorFilter();
        }

        private void InitTab(int index, int iconResource, string title, TabLayout tabLayout, LayoutInflater inflater)
        {
            var tabView = inflater.Inflate(Resource.Layout.tab_button_layout, null);
            tabView.Tag = index;
            tabView.SetOnTouchListener(_tabViewOnTouchListener);

            var textView = tabView.FindViewById<TextView>(Resource.Id.tab_title);
            var iconView = tabView.FindViewById<MvxCachedImageView>(Resource.Id.tab_icon);

            textView.Text = title;
            iconView.SetImageResource(iconResource);
            var tab = tabLayout.GetTabAt(index);
            tab?.SetCustomView(tabView);
        }

        private void InitCentralTab(int iconResource, TabLayout tabLayout, LayoutInflater inflater)
        {
            var tabView = (ImageView)inflater.Inflate(Resource.Layout.central_tab_button_layout, null);
            tabView.Tag = 2;
            tabView.SetOnTouchListener(_tabViewOnTouchListener);

            tabView.SetImageResource(iconResource);
            var tab = tabLayout.GetTabAt(2);
            if (tab != null)
                tab.SetCustomView(tabView);
        }

        protected override void OnResume()
        {
            if (Resources.Configuration.Orientation != Android.Content.Res.Orientation.Portrait)
            {
                RequestedOrientation = ScreenOrientation.Portrait;
            }

            base.OnResume();
        }

        private bool OnTabItemTouched(View view, MotionEvent motionEvent)
        {
            if (view.Tag == null)
            {
                return false;
            }

            if (!int.TryParse(view.Tag.ToString(), out var index))
            {
                return false;
            }

            if (!ViewModel.CanSwitchTabs(index) &&
                motionEvent.Action != MotionEventActions.Up)
            {
                return true;
            }

            if (!ViewModel.CanSwitchTabs(index) &&
                motionEvent.Action == MotionEventActions.Up)
            {
                ViewModel.CheckDemoCommand.Execute(index);
                return true;
            }

            return false;
        }
    }
}
