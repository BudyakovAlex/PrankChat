using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Google.Android.Material.Tabs;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Combiners;
using MvvmCross.DroidX;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.ViewModels.Subscriptions.Items;
using PrankChat.Mobile.Droid.Adapters;
using PrankChat.Mobile.Droid.Adapters.TemplateSelectors;
using PrankChat.Mobile.Droid.Adapters.ViewHolders.Subscriptions;
using PrankChat.Mobile.Droid.Controls;
using PrankChat.Mobile.Droid.Extensions;
using PrankChat.Mobile.Droid.LayoutManagers;
using PrankChat.Mobile.Droid.Views.Base;

namespace PrankChat.Mobile.Droid.Views.Subscriptions
{
    [MvxActivityPresentation]
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class SubscriptionsView : BaseView<SubscriptionsViewModel>, TabLayout.IOnTabSelectedListener
    {
        private TabLayout.Tab _subscribersTab;
        private TabLayout.Tab _subscriptionsTab;
        private EndlessRecyclerView _recyclerView;
        private TextView _titleTextView;
        private RecycleViewBindableAdapter _adapter;
        private MvxSwipeRefreshLayout _subscriptionsSwipeRefreshLayout;
        private View _emptyView;
        private TextView _emptyViewTitleTextView;

        protected override string TitleActionBar => Core.Localization.Resources.Subscriptions;

        protected override bool HasBackButton => true;

        private SubscriptionTabType _selectedTabType;
        public SubscriptionTabType SelectedTabType
        {
            get => _selectedTabType;
            set
            {
                _selectedTabType = value;
                switch (_selectedTabType)
                {
                    case SubscriptionTabType.Subscriptions:
                        _subscriptionsTab.Select();
                        break;

                    case SubscriptionTabType.Subscribers:
                        _subscribersTab.Select();
                        break;
                }
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle, Resource.Layout.activity_subscriptions);
            Window.SetBackgroundDrawableResource(Resource.Drawable.gradient_action_bar_background);
        }

        protected override void SetViewProperties()
        {
            base.SetViewProperties();
            InitializeEmptyView();

            _recyclerView = FindViewById<EndlessRecyclerView>(Resource.Id.recycler_view);
            var layoutManager = new SafeLinearLayoutManager(this, LinearLayoutManager.Vertical, false);
            _recyclerView.SetLayoutManager(layoutManager);

            _adapter = new RecycleViewBindableAdapter((IMvxAndroidBindingContext)BindingContext);
            _recyclerView.Adapter = _adapter;
            _recyclerView.ItemTemplateSelector = new TemplateSelector()
                .AddElement<SubscriptionItemViewModel, SubscriptionItemViewHolder>(Resource.Layout.cell_subscription);

            _titleTextView = FindViewById<TextView>(Resource.Id.toolbar_title);
            _subscriptionsSwipeRefreshLayout = FindViewById<MvxSwipeRefreshLayout>(Resource.Id.subscriptions_swipe_refresh_layout);
            var tabLayout = FindViewById<TabLayout>(Resource.Id.tab_layout);

            _subscribersTab = tabLayout.NewTab();
            _subscriptionsTab = tabLayout.NewTab();

            tabLayout.AddTab(_subscribersTab);
            tabLayout.AddTab(_subscriptionsTab);

            tabLayout.AddOnTabSelectedListener(this);
        }

        protected override void Bind()
        {
            base.Bind();

            using var bindingSet = this.CreateBindingSet<SubscriptionsView, SubscriptionsViewModel>();

            bindingSet.Bind(this).For(v => v.SelectedTabType).To(vm => vm.SelectedTabType);
            bindingSet.Bind(_titleTextView).For(v => v.Text).To(vm => vm.Title);
            bindingSet.Bind(_subscribersTab).For(v => v.BindTabText()).To(vm => vm.SubscribersTitle);
            bindingSet.Bind(_subscriptionsTab).For(v => v.BindTabText()).To(vm => vm.SubscriptionsTitle);
            bindingSet.Bind(_adapter).For(v => v.ItemsSource).To(vm => vm.Items);
            bindingSet.Bind(_recyclerView).For(v => v.LoadMoreItemsCommand).To(vm => vm.LoadMoreItemsCommand);
            bindingSet.Bind(_subscriptionsSwipeRefreshLayout).For(v => v.Refreshing).To(vm => vm.IsBusy);
            bindingSet.Bind(_subscriptionsSwipeRefreshLayout).For(v => v.RefreshCommand).To(vm => vm.LoadDataCommand);
            bindingSet.Bind(_emptyView)
               .For(v => v.BindVisible())
               .ByCombining(new MvxAndValueCombiner(),
                 vm => vm.IsEmpty,
                 vm => vm.IsNotBusy,
                 vm => vm.IsInitialized);
        }

        public void OnTabReselected(TabLayout.Tab tab)
        {
        }

        public void OnTabSelected(TabLayout.Tab tab)
        {
            var tabType = tab == _subscribersTab
                ? SubscriptionTabType.Subscribers
                : SubscriptionTabType.Subscriptions;

            ViewModel.SelectedTabType = tabType;
            _emptyViewTitleTextView.Text = tabType == SubscriptionTabType.Subscribers
                ? Core.Localization.Resources.SubscribersListIsEmpty
                : Core.Localization.Resources.SubscriptionsListIsEmpty;
        }

        public void OnTabUnselected(TabLayout.Tab tab)
        {
        }

        private void InitializeEmptyView()
        {
            _emptyView = FindViewById<View>(Resource.Id.empty_view);
            _emptyViewTitleTextView = _emptyView.FindViewById<TextView>(Resource.Id.title_text_view);
            _emptyViewTitleTextView.Text = Core.Localization.Resources.SubscribersListIsEmpty;
        }
    }
}
