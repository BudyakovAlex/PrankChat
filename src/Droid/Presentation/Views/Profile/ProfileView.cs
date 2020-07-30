using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order.Items;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile;
using PrankChat.Mobile.Droid.Controls;
using PrankChat.Mobile.Droid.Presentation.Adapters;
using PrankChat.Mobile.Droid.Presentation.Adapters.TemplateSelectors;
using PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Orders;
using PrankChat.Mobile.Droid.Presentation.Views.Base;

namespace PrankChat.Mobile.Droid.Presentation.Views.Profile
{
    [MvxTabLayoutPresentation(TabLayoutResourceId = Resource.Id.tabs, ViewPagerResourceId = Resource.Id.viewpager, ActivityHostViewModelType = typeof(MainViewModel))]
    [Register(nameof(ProfileView))]
    public class ProfileView : BaseTabFragment<ProfileViewModel>, TabLayout.IOnTabSelectedListener, IScrollableView
    {
        private EndlessRecyclerView _endlessRecyclerView;
        private LinearLayoutManager _layoutManager;
        private RecycleViewBindableAdapter _adapter;

        protected override string TitleActionBar => Core.Presentation.Localization.Resources.Profile_Tab;

        public RecyclerView RecyclerView => _endlessRecyclerView;

        public ProfileView()
        {
            HasOptionsMenu = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.fragment_profile, null);

            InitializeControls(view);
            DoBind();

            var tabLayout = view.FindViewById<TabLayout>(Resource.Id.publication_type_tab_layout);
            tabLayout.AddOnTabSelectedListener(this);

            return view;
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

        private void InitializeControls(View view)
        {
            _endlessRecyclerView = view.FindViewById<EndlessRecyclerView>(Resource.Id.profile_publication_recycler_view);

            _layoutManager = new LinearLayoutManager(Context, LinearLayoutManager.Vertical, false);
            _endlessRecyclerView.SetLayoutManager(_layoutManager);
            _endlessRecyclerView.HasNextPage = true;

            _adapter = new RecycleViewBindableAdapter((IMvxAndroidBindingContext)BindingContext);
            _endlessRecyclerView.Adapter = _adapter;

            _endlessRecyclerView.ItemTemplateSelector = new TemplateSelector()
                .AddElement<OrderItemViewModel, OrderItemViewHolder>(Resource.Layout.cell_order);
        }

        private void DoBind()
        {
            var bindingSet = this.CreateBindingSet<ProfileView, ProfileViewModel>();

            bindingSet.Bind(_adapter)
                .For(v => v.ItemsSource)
                .To(vm => vm.Items);

            bindingSet.Bind(_endlessRecyclerView)
                .For(v => v.LoadMoreItemsCommand)
                .To(vm => vm.LoadMoreItemsCommand);

            bindingSet.Apply();
        }
    }
}
