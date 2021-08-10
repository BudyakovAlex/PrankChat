using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using AndroidX.Core.Content;
using AndroidX.RecyclerView.Widget;
using Google.Android.Material.Button;
using Google.Android.Material.Tabs;
using MvvmCross.Binding.BindingContext;
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
        private MaterialButton _subscribeButton;
        private EndlessRecyclerView _endlessRecyclerView;
        private LinearLayoutManager _layoutManager;
        private RecycleViewBindableAdapter _adapter;

        private bool _isSubscribed;
        public bool IsSubscribed
        {
            get => _isSubscribed;
            set
            {
                _isSubscribed = value;
                if (_isSubscribed)
                {
                    _subscribeButton.Text = Core.Localization.Resources.OrderDetailsView_Unsubscribe_Button;
                    _subscribeButton.SetBackgroundResource(Resource.Drawable.button_accent_background);
                    _subscribeButton.SetTextColor(Color.White);
                    return;
                }

                _subscribeButton.Text = Core.Localization.Resources.OrderDetailsView_Subscribe_Button;
                _subscribeButton.SetBackgroundResource(Resource.Drawable.border_accent);
                var colorArgb = ContextCompat.GetColor(this, Resource.Color.accent);
                var color = new Color(colorArgb);
                _subscribeButton.SetTextColor(color);
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

            _subscribeButton = FindViewById<MaterialButton>(Resource.Id.subscribe_button);
            _endlessRecyclerView = FindViewById<EndlessRecyclerView>(Resource.Id.profile_publication_recycler_view);

            _layoutManager = new LinearLayoutManager(this, LinearLayoutManager.Vertical, false);
            _endlessRecyclerView.SetLayoutManager(_layoutManager);
            _endlessRecyclerView.HasNextPage = true;

            _adapter = new RecycleViewBindableAdapter((IMvxAndroidBindingContext) BindingContext);
            _endlessRecyclerView.Adapter = _adapter;

            _endlessRecyclerView.ItemTemplateSelector = new TemplateSelector()
                .AddElement<OrderItemViewModel, OrderItemViewHolder>(Resource.Layout.cell_order);

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

            var bindingSet = this.CreateBindingSet<UserProfileView, UserProfileViewModel>();

            bindingSet.Bind(_adapter)
                      .For(v => v.ItemsSource)
                      .To(vm => vm.Items);

            bindingSet.Bind(this)
                      .For(v => v.IsSubscribed)
                      .To(vm => vm.IsSubscribed);

            bindingSet.Bind(_endlessRecyclerView)
                      .For(v => v.LoadMoreItemsCommand)
                      .To(vm => vm.LoadMoreItemsCommand);

            bindingSet.Apply();
        }
    }
}
