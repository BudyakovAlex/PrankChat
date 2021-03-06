using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Core.Content;
using FFImageLoading.Cross;
using Google.Android.Material.Tabs;
using MvvmCross.Binding.BindingContext;
using PrankChat.Mobile.Core.ViewModels;
using PrankChat.Mobile.Droid.Controls;
using PrankChat.Mobile.Droid.Extensions;
using PrankChat.Mobile.Droid.Listeners;
using PrankChat.Mobile.Droid.Presenters.Attributes;
using PrankChat.Mobile.Droid.Utils.Helpers;
using PrankChat.Mobile.Droid.Views.Base;
using System.Linq;
using Localization = PrankChat.Mobile.Core.Localization.Resources;

namespace PrankChat.Mobile.Droid.Views
{
    [ClearStackActivityPresentation]
    [Activity(
        LaunchMode = LaunchMode.SingleTop,
        ScreenOrientation = ScreenOrientation.Portrait,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.KeyboardHidden)]
    public class MainView : BaseView<MainViewModel>
    {
        private readonly ViewOnTouchListener _tabViewOnTouchListener;

        private TabLayout _tabLayout;
        private ImageView _toolbarLogo;
        private TextView _toolbarTitle;

        private IMenuItem _searchMenuItem;
        private IMenuItem _infoMenuItem;
        private IMenuItem _notificationsMenuItem;
        private IMenuItem _inviteFriendMenuItem;

        public MainView()
        {
            _tabViewOnTouchListener = new ViewOnTouchListener(OnTabItemTouched);
        }

        private bool _hasInviteFriendBadge;
        public bool HasInviteFriendBadge
        {
            get => _hasInviteFriendBadge;
            set
            {
                var id = value
                    ? Resource.Drawable.ic_invite_friend_with_badge
                    : Resource.Drawable.ic_invite_friend;
                _inviteFriendMenuItem?.SetIcon(id);
                _hasInviteFriendBadge = value;
            }
        }

        private bool _hasUnreadNotifications;
        public bool HasUnreadNotifications
        {
            get => _hasUnreadNotifications;
            set
            {
                _hasUnreadNotifications = value;
                var iconId = _hasUnreadNotifications ? Resource.Drawable.ic_notification_with_bage : Resource.Drawable.ic_notification;
                _notificationsMenuItem?.SetIcon(iconId);
            }
        }

        private bool _canInviteFriend;
        public bool CanInviteFriend
        {
            get => _canInviteFriend;
            set
            {
                _canInviteFriend = value;
                UpdateMenuItemsVisibility();
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle, Resource.Layout.main_view_layout);
            ViewModel.LoadContentCommand.Execute(null);

            Window.SetBackgroundDrawableResource(Resource.Drawable.gradient_action_bar_background);

            var viewPager = FindViewById<ApplicationSwipeViewPager>(Resource.Id.viewpager);
            viewPager.OffscreenPageLimit = 5;
            _tabLayout = FindViewById<TabLayout>(Resource.Id.tabs);

            _toolbarLogo = FindViewById<ImageView>(Resource.Id.toolbar_logo);
            _toolbarTitle = FindViewById<TextView>(Resource.Id.toolbar_title);

            CreateTabs();
            RequestedOrientation = ScreenOrientation.Locked;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            menu.Clear();
            MenuInflater.Inflate(Resource.Menu.main_view_menu, menu);

            _searchMenuItem = menu.GetItem(0);
            _infoMenuItem = menu.GetItem(1);
            _notificationsMenuItem = menu.GetItem(2);
            _inviteFriendMenuItem = menu.GetItem(3);

            UpdateMenuItemsVisibility();

            var notificationIconId = _hasUnreadNotifications
                ? Resource.Drawable.ic_notification_with_bage
                : Resource.Drawable.ic_notification;

            _notificationsMenuItem.SetIcon(notificationIconId);

            var inviteIconId = _hasInviteFriendBadge
                ? Resource.Drawable.ic_invite_friend_with_badge
                : Resource.Drawable.ic_invite_friend;

            _inviteFriendMenuItem?.SetIcon(inviteIconId);

            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.notification_button:
                    ViewModel.ShowNotificationCommand.Execute(null);
                    return true;

                case Resource.Id.info_button:
                    ViewModel.ShowWalkthrouthCommand.Execute(_tabLayout.SelectedTabPosition);
                    return true;

                case Resource.Id.search_button:
                    ViewModel.ShowSearchCommand.Execute(null);
                    return true;

                case Resource.Id.invite_friend_button:
                    ViewModel.InviteFriendItemViewModel.InviteFriendCommand.Execute();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        protected override void Bind()
        {
            base.Bind();

            using var bindingSet = this.CreateBindingSet<MainView, MainViewModel>();

            bindingSet.Bind(this).For(v => v.HasUnreadNotifications).To(vm => vm.NotificationBadgeViewModel.HasUnreadNotifications);
            bindingSet.Bind(this).For(v => v.CanInviteFriend).To(vm => vm.InviteFriendItemViewModel.CanInviteFriend);
            bindingSet.Bind(this).For(nameof(HasInviteFriendBadge)).To(vm => vm.InviteFriendItemViewModel.HasBadge);
        }

        protected override void Subscription()
        {
            _tabLayout.TabSelected += OnTabLayoutTabSelected;
            _tabLayout.TabUnselected += OnTabLayoutTabUnselected;
            _tabLayout.TabReselected += OnTabLayoutTabReselected;
        }

        protected override void Unsubscription()
        {
            _tabLayout.TabSelected -= OnTabLayoutTabSelected;
            _tabLayout.TabUnselected -= OnTabLayoutTabUnselected;
            _tabLayout.TabReselected -= OnTabLayoutTabReselected;
        }

        private void CreateTabs()
        {
            _tabLayout.SetTabTextColors(Resource.Color.applicationBlack, Resource.Color.inactive);
            var inflater = LayoutInflater.FromContext(Application.Context);
            InitTab(0, Resource.Drawable.ic_home, Localization.Home, _tabLayout, inflater);
            InitTab(1, Resource.Drawable.ic_competitions, Localization.Contests, _tabLayout, inflater);
            InitCentralTab(Resource.Drawable.ic_create_order, _tabLayout, inflater);
            InitTab(3, Resource.Drawable.ic_orders, Localization.Orders, _tabLayout, inflater);
            InitTab(4, Resource.Drawable.ic_profile, Localization.Profile, _tabLayout, inflater);

            OnTabLayoutTabSelected(this, new TabLayout.TabSelectedEventArgs(_tabLayout.GetTabAt(0)));
        }

        private void RefreshTabData(int position)
        {
            var currentFragment = SupportFragmentManager.Fragments.ElementAtOrDefault(position);
            if (currentFragment is IScrollableView scrollableView && scrollableView.RecyclerView != null)
            {
                scrollableView.RecyclerView.Post(() => scrollableView.RecyclerView.ScrollToPosition(0));
            }

            if (currentFragment is IRefreshableView refreshableView)
            {
                refreshableView.RefreshData();
            }
        }

        private void OnTabLayoutTabReselected(object sender, TabLayout.TabReselectedEventArgs e) =>
            RefreshTabData(e.Tab.Position);

        private void OnTabLayoutTabSelected(object sender, TabLayout.TabSelectedEventArgs e)
        {
            if (e.Tab is null)
            {
                return;
            }

            this.HideKeyboard();

            _toolbarLogo.Visibility = e.Tab.Position != 2 ? ViewStates.Visible : ViewStates.Invisible;
            _toolbarTitle.Visibility = e.Tab.Position == 2 ? ViewStates.Visible : ViewStates.Invisible;
            _toolbarTitle.Text = e.Tab.Position == 2 ? Localization.CreateOrder : string.Empty;

            UpdateMenuItemsVisibility();

            var iconView = e.Tab.View.FindViewById<ImageView>(Resource.Id.tab_icon);
            var textView = e.Tab.View.FindViewById<TextView>(Resource.Id.tab_title);

            textView?.SetTextColor(ContextCompat.GetColorStateList(ApplicationContext, Resource.Color.applicationBlack));

            if (e.Tab.Position != 0 && e.Tab.Position != 3)
            {
                RefreshTabData(e.Tab.Position);
            }

            if (e.Tab.Position == 2)
            {
                ViewModel.ShowWalkthrouthIfNeedCommand?.Execute(_tabLayout.SelectedTabPosition);
                return;
            }

            var colorFilter = ContextCompat.GetColor(ApplicationContext, Resource.Color.accent);
            iconView.Drawable.SetColorFilter(ColorUtil.GetColorFromInteger(colorFilter), PorterDuff.Mode.SrcIn);
            ViewModel.ShowWalkthrouthIfNeedCommand?.Execute(_tabLayout.SelectedTabPosition);
        }

        private void OnTabLayoutTabUnselected(object sender, TabLayout.TabUnselectedEventArgs e)
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
            if (tab is null)
            {
                return;
            }

            tab.SetCustomView(tabView);
        }

        private void UpdateMenuItemsVisibility()
        {
            var selectedTabPosition = _tabLayout?.SelectedTabPosition;

            _searchMenuItem?.SetVisible(selectedTabPosition == 0);
            _infoMenuItem?.SetVisible(selectedTabPosition != 0);
            _inviteFriendMenuItem?.SetVisible(CanInviteFriend && selectedTabPosition == 0);
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
