using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.ConstraintLayout.Widget;
using AndroidX.RecyclerView.Widget;
using Google.Android.Material.Button;
using Google.Android.Material.Tabs;
using MvvmCross.Binding.BindingContext;
using MvvmCross.DroidX;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.ViewModels;
using PrankChat.Mobile.Core.ViewModels.Order.Items;
using PrankChat.Mobile.Core.ViewModels.Profile;
using PrankChat.Mobile.Droid.Controls;
using PrankChat.Mobile.Droid.Adapters;
using PrankChat.Mobile.Droid.Adapters.TemplateSelectors;
using PrankChat.Mobile.Droid.Adapters.ViewHolders.Orders;
using PrankChat.Mobile.Droid.Converters;
using PrankChat.Mobile.Droid.Views.Base;

namespace PrankChat.Mobile.Droid.Views.Profile
{
    [MvxTabLayoutPresentation(TabLayoutResourceId = Resource.Id.tabs, ViewPagerResourceId = Resource.Id.viewpager, ActivityHostViewModelType = typeof(MainViewModel))]
    [Register(nameof(ProfileView))]
    public class ProfileView : BaseRefreshableTabFragment<ProfileViewModel>, TabLayout.IOnTabSelectedListener, IScrollableView
    {
        private EndlessRecyclerView _endlessRecyclerView;
        private LinearLayoutManager _layoutManager;
        private RecycleViewBindableAdapter _adapter;
        private CircleCachedImageView _profileCircleCachedImageView;
        private TextView _profileNameTextView;
        private TextView _profilePriceTextView;
        private TextView _profileSubscribersValueTextView;
        private TextView _profileSubscriptionsValueTextView;
        private TextView _descriptionTextView;
        private ConstraintLayout _subscriptionsViewConstraintLayout;
        private ConstraintLayout _subscribersViewConstraintLayout;
        private MaterialButton _profileRefillButton;
        private MaterialButton _profileWithdrawalButton;
        private MvxSwipeRefreshLayout _mvxSwipeRefreshLayout;

        protected override string TitleActionBar => Core.Localization.Resources.Profile;

        public RecyclerView RecyclerView => _endlessRecyclerView;

        public ProfileView() : base(Resource.Layout.fragment_profile)
        {
            HasOptionsMenu = true;
        }

        public void OnTabReselected(TabLayout.Tab tab)
        {
        }

        public void OnTabSelected(TabLayout.Tab tab)
        {
            RecyclerView.Post(() => RecyclerView.ScrollToPosition(0));

            ViewModel.SelectedOrderType = (ProfileOrderType)tab.Position;
        }

        public void OnTabUnselected(TabLayout.Tab tab)
        {
        }

        protected override void Bind()
        {
            base.Bind();

            using var bindingSet = this.CreateBindingSet<ProfileView, ProfileViewModel>();

            bindingSet.Bind(_adapter).For(v => v.ItemsSource).To(vm => vm.Items);
            bindingSet.Bind(_endlessRecyclerView).For(v => v.LoadMoreItemsCommand).To(vm => vm.LoadMoreItemsCommand);
            bindingSet.Bind(_profileCircleCachedImageView).For(v => v.ImagePath).To(vm => vm.ProfilePhotoUrl);
            bindingSet.Bind(_profileCircleCachedImageView).For(v => v.PlaceholderText).To(vm => vm.ProfileShortName).OneTime();
            bindingSet.Bind(_profileCircleCachedImageView).For(v => v.BindClick()).To(vm => vm.ShowUpdateProfileCommand);
            bindingSet.Bind(_profileNameTextView).For(v => v.Text).To(vm => vm.Login);
            bindingSet.Bind(_profilePriceTextView).For(v => v.Text).To(vm => vm.Price).OneWay();
            bindingSet.Bind(_profileSubscribersValueTextView).For(v => v.Text).To(vm => vm.SubscribersValue);
            bindingSet.Bind(_subscriptionsViewConstraintLayout).For(v => v.BindClick()).To(vm => vm.ShowSubscriptionsCommand);
            bindingSet.Bind(_profileSubscriptionsValueTextView).For(v => v.BindClick()).To(vm => vm.ShowSubscriptionsCommand);
            bindingSet.Bind(_profileSubscriptionsValueTextView).For(v => v.Text).To(vm => vm.SubscriptionsValue);
            bindingSet.Bind(_profileRefillButton).For(v => v.BindClick()).To(vm => vm.ShowRefillCommand);
            bindingSet.Bind(_profileWithdrawalButton).For(v => v.BindClick()).To(vm => vm.ShowWithdrawalCommand);
            bindingSet.Bind(_subscribersViewConstraintLayout).For(v => v.BindClick()).To(vm => vm.ShowSubscribersCommand);
            bindingSet.Bind(_descriptionTextView).For(v => v.BindClick()).To(vm => vm.Description);
            bindingSet.Bind(_descriptionTextView).For(v => v.Visibility).To(vm => vm.HasDescription)
                .WithConversion<BoolToGoneConverter>();
            bindingSet.Bind(_mvxSwipeRefreshLayout).For(v => v.Refreshing).To(vm => vm.IsBusy);
            bindingSet.Bind(_mvxSwipeRefreshLayout).For(v => v.RefreshCommand).To(vm => vm.LoadProfileCommand);
        }

        protected override void SetViewProperties(View view)
        {
            base.SetViewProperties(view);

            _endlessRecyclerView = view.FindViewById<EndlessRecyclerView>(Resource.Id.profile_publication_recycler_view);

            _profileCircleCachedImageView = view.FindViewById<CircleCachedImageView>(Resource.Id.profile_photo);
            _profileNameTextView = view.FindViewById<TextView>(Resource.Id.profile_name);
            _profilePriceTextView = view.FindViewById<TextView>(Resource.Id.profile_price);
            _profileSubscribersValueTextView = view.FindViewById<TextView>(Resource.Id.profile_subscribers_value);
            _subscriptionsViewConstraintLayout = view.FindViewById<ConstraintLayout>(Resource.Id.subscriptions_view);
            _profileSubscriptionsValueTextView = view.FindViewById<TextView>(Resource.Id.profile_subscriptions_value);
            _profileRefillButton = view.FindViewById<MaterialButton>(Resource.Id.profile_refill_button);
            _profileWithdrawalButton = view.FindViewById<MaterialButton>(Resource.Id.profile_withdrawal_button);
            _subscribersViewConstraintLayout = view.FindViewById<ConstraintLayout>(Resource.Id.subscribers_view);
            _descriptionTextView = view.FindViewById<TextView>(Resource.Id.description_text_view);
            _mvxSwipeRefreshLayout = view.FindViewById<MvxSwipeRefreshLayout>(Resource.Id.swipe_refresh_layout);

            _layoutManager = new LinearLayoutManager(Context, LinearLayoutManager.Vertical, false);
            _endlessRecyclerView.SetLayoutManager(_layoutManager);
            _endlessRecyclerView.HasNextPage = true;

            _adapter = new RecycleViewBindableAdapter((IMvxAndroidBindingContext)BindingContext);
            _endlessRecyclerView.Adapter = _adapter;

            _endlessRecyclerView.ItemTemplateSelector = new TemplateSelector()
                .AddElement<OrderItemViewModel, OrderItemViewHolder>(Resource.Layout.cell_order);

            var tabLayout = view.FindViewById<TabLayout>(Resource.Id.publication_type_tab_layout);
            tabLayout.AddOnTabSelectedListener(this);
        }

        protected override void RefreshData() =>
            ViewModel?.LoadProfileCommand.Execute();
    }
}
