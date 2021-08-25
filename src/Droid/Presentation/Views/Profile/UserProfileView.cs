using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using AndroidX.ConstraintLayout.Widget;
using AndroidX.Core.Content;
using AndroidX.RecyclerView.Widget;
using Google.Android.Material.Button;
using Google.Android.Material.Tabs;
using MvvmCross.Binding.BindingContext;
using MvvmCross.DroidX;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order.Items;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile;
using PrankChat.Mobile.Droid.Controls;
using PrankChat.Mobile.Droid.Presentation.Adapters;
using PrankChat.Mobile.Droid.Presentation.Adapters.TemplateSelectors;
using PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Orders;
using PrankChat.Mobile.Droid.Presentation.Views.Base;

namespace PrankChat.Mobile.Droid.Presentation.Views.Profile
{
    [MvxActivityPresentation]
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class UserProfileView : BaseView<UserProfileViewModel>, TabLayout.IOnTabSelectedListener
    {
        private MaterialButton _subscribeToUserButton;
        private EndlessRecyclerView _endlessRecyclerView;
        private LinearLayoutManager _layoutManager;
        private RecycleViewBindableAdapter _adapter;
        private MvxSwipeRefreshLayout _mvxSwipeRefreshLayout;
        private CircleCachedImageView _profilePhotoImageView;
        private TextView _profileNameTextView;
        private ConstraintLayout _subscribersViewConstraintLayout;
        private TextView _profileSubscribersValueTextView;
        private ConstraintLayout _subscriptionsViewConstraintLayout;
        private TextView _profileSubscriptionsValueTextView;
        private TextView _descriptionTextView;
        private bool _isSubscribed;

        public bool IsSubscribed
        {
            get => _isSubscribed;
            set
            {
                _isSubscribed = value;
                if (_isSubscribed)
                {
                    _subscribeToUserButton.Text = Core.Localization.Resources.OrderDetailsView_Unsubscribe_Button;
                    _subscribeToUserButton.SetBackgroundResource(Resource.Drawable.button_accent_background);
                    _subscribeToUserButton.SetTextColor(Color.White);
                    return;
                }

                _subscribeToUserButton.Text = Core.Localization.Resources.OrderDetailsView_Subscribe_Button;
                _subscribeToUserButton.SetBackgroundResource(Resource.Drawable.border_accent);
                var colorArgb = ContextCompat.GetColor(this, Resource.Color.accent);
                var color = new Color(colorArgb);
                _subscribeToUserButton.SetTextColor(color);
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle, Resource.Layout.activity_user_profile);
            Window.SetBackgroundDrawableResource(Resource.Drawable.gradient_action_bar_background);
        }

        protected override void SetViewProperties()
        {
            base.SetViewProperties();

            _subscribeToUserButton = FindViewById<MaterialButton>(Resource.Id.subscribe_button);
            _endlessRecyclerView = FindViewById<EndlessRecyclerView>(Resource.Id.profile_publication_recycler_view);

            _layoutManager = new LinearLayoutManager(this, LinearLayoutManager.Vertical, false);
            _endlessRecyclerView.SetLayoutManager(_layoutManager);
            _endlessRecyclerView.HasNextPage = true;

            _adapter = new RecycleViewBindableAdapter((IMvxAndroidBindingContext)BindingContext);
            _endlessRecyclerView.Adapter = _adapter;

            _endlessRecyclerView.ItemTemplateSelector = new TemplateSelector()
                .AddElement<OrderItemViewModel, OrderItemViewHolder>(Resource.Layout.cell_order);

            _mvxSwipeRefreshLayout = FindViewById<MvxSwipeRefreshLayout>(Resource.Id.order_swipe_refresh_layout);
            _profilePhotoImageView = FindViewById<CircleCachedImageView>(Resource.Id.profile_photo);
            _profileNameTextView = FindViewById<TextView>(Resource.Id.profile_name);
            _subscribeToUserButton = FindViewById<MaterialButton>(Resource.Id.subscribe_button);
            _subscribersViewConstraintLayout = FindViewById<ConstraintLayout>(Resource.Id.subscribers_view);
            _profileSubscribersValueTextView = FindViewById<TextView>(Resource.Id.profile_subscribers_value);
            _subscriptionsViewConstraintLayout = FindViewById<ConstraintLayout>(Resource.Id.subscriptions_view);
            _profileSubscriptionsValueTextView = FindViewById<TextView>(Resource.Id.profile_subscriptions_value);
            _descriptionTextView = FindViewById<TextView>(Resource.Id.description_text_view);

            var tabLayout = FindViewById<TabLayout>(Resource.Id.publication_type_tab_layout);
            tabLayout.AddOnTabSelectedListener(this);
        }

        public void OnTabReselected(TabLayout.Tab tab)
        {
        }

        public void OnTabSelected(TabLayout.Tab tab)
        {
            switch (tab.Position)
            {
                case 0:
                    ViewModel.SelectedOrderType = ProfileOrderType.MyOrdered;
                    break;

                case 1:
                    ViewModel.SelectedOrderType = ProfileOrderType.OrdersCompletedByMe;
                    break;
            }
        }

        public void OnTabUnselected(TabLayout.Tab tab)
        {
        }

        protected override void Bind()
        {
            base.Bind();

            using var bindingSet = this.CreateBindingSet<UserProfileView, UserProfileViewModel>();

            bindingSet.Bind(_adapter).For(v => v.ItemsSource).To(vm => vm.Items);
            bindingSet.Bind(this).For(v => v.IsSubscribed).To(vm => vm.IsSubscribed);
            bindingSet.Bind(_endlessRecyclerView).For(v => v.LoadMoreItemsCommand).To(vm => vm.LoadMoreItemsCommand);
            bindingSet.Bind(_mvxSwipeRefreshLayout).For(v => v.Refreshing).To(vm => vm.IsBusy);
            bindingSet.Bind(_mvxSwipeRefreshLayout).For(v => v.RefreshCommand).To(vm => vm.RefreshUserDataCommand);
            bindingSet.Bind(_profilePhotoImageView).For(v => v.ImagePath).To(vm => vm.ProfilePhotoUrl);
            bindingSet.Bind(_profilePhotoImageView).For(v => v.PlaceholderText).To(vm => vm.ProfileShortLogin);
            bindingSet.Bind(_profileNameTextView).For(v => v.Text).To(vm => vm.Login);
            bindingSet.Bind(_subscribeToUserButton).For(v => v.BindClick()).To(vm => vm.SubscribeCommand);
            bindingSet.Bind(_subscribersViewConstraintLayout).For(v => v.BindClick()).To(vm => vm.ShowSubscribersCommand);
            bindingSet.Bind(_profileSubscribersValueTextView).For(v => v.Text).To(vm => vm.SubscribersValue);
            bindingSet.Bind(_subscriptionsViewConstraintLayout).For(v => v.BindClick()).To(vm => vm.ShowSubscriptionsCommand);
            bindingSet.Bind(_profileSubscriptionsValueTextView).For(v => v.Text).To(vm => vm.SubscriptionsValue);
            bindingSet.Bind(_descriptionTextView).For(v => v.Text).To(vm => vm.Description);
            bindingSet.Bind(_descriptionTextView).For(v => v.BindVisible()).To(vm => vm.HasDescription);
        }
    }
}
