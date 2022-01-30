using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Google.Android.Material.Tabs;
using MvvmCross.Binding.Combiners;
using MvvmCross.DroidX;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Data.Enums;
using PrankChat.Mobile.Core.ViewModels.Competitions;
using PrankChat.Mobile.Core.ViewModels.Competitions.Items;
using PrankChat.Mobile.Droid.Adapters;
using PrankChat.Mobile.Droid.Adapters.TemplateSelectors;
using PrankChat.Mobile.Droid.Adapters.ViewHolders.Competitions;
using PrankChat.Mobile.Droid.Controls;
using PrankChat.Mobile.Droid.Decorators;
using PrankChat.Mobile.Droid.Extensions;
using PrankChat.Mobile.Droid.LayoutManagers;
using PrankChat.Mobile.Droid.Utils.Helpers;
using PrankChat.Mobile.Droid.Views.Base;

namespace PrankChat.Mobile.Droid.Views.Competitions
{
    [MvxActivityPresentation]
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class MyCompetitionsView : BaseView<MyCompetitionsViewModel>, TabLayout.IOnTabSelectedListener
    {
        private TabLayout.Tab _inExecutionTab;
        private TabLayout.Tab _orderedTab;
        private EndlessRecyclerView _recyclerView;
        private TextView _titleTextView;
        private RecycleViewBindableAdapter _adapter;
        private MvxSwipeRefreshLayout _swipeRefreshLayout;
        private View _emptyView;
        private TextView _emptyViewTitleTextView;

        protected override string TitleActionBar => Core.Localization.Resources.Contests;

        protected override bool HasBackButton => true;

        private CompetitionsTabType _selectedTabType;
        public CompetitionsTabType SelectedTabType
        {
            get => _selectedTabType;
            set
            {
                _selectedTabType = value;
                switch (_selectedTabType)
                {
                    case CompetitionsTabType.Ordered:
                        _orderedTab.Select();
                        break;

                    case CompetitionsTabType.OnExecution:
                        _inExecutionTab.Select();
                        break;
                }
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle, Resource.Layout.activity_my_competitions);
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
            _recyclerView.AddItemDecoration(new BottomSpaceDecorator(this, DisplayUtils.DpToPx(6)));
            _recyclerView.ItemTemplateSelector = new TemplateSelector()
                .AddElement<CompetitionItemViewModel, CompetitionItemViewHolder>(Resource.Layout.cell_competition);

            _titleTextView = FindViewById<TextView>(Resource.Id.toolbar_title);
            _swipeRefreshLayout = FindViewById<MvxSwipeRefreshLayout>(Resource.Id.swipe_refresh_layout);
            var tabLayout = FindViewById<TabLayout>(Resource.Id.tab_layout);

            _inExecutionTab = tabLayout.NewTab();
            _orderedTab = tabLayout.NewTab();

            tabLayout.AddTab(_orderedTab);
            tabLayout.AddTab(_inExecutionTab);

            tabLayout.AddOnTabSelectedListener(this);
        }

        protected override void Bind()
        {
            base.Bind();

            using var bindingSet = CreateBindingSet();

            bindingSet.Bind(this).For(v => v.SelectedTabType).To(vm => vm.SelectedTabType);
            bindingSet.Bind(_titleTextView).For(v => v.Text).To(vm => vm.Title);
            bindingSet.Bind(_inExecutionTab).For(v => v.BindTabText()).To(vm => vm.OnExecutionTitle);
            bindingSet.Bind(_orderedTab).For(v => v.BindTabText()).To(vm => vm.OrderedTitle);
            bindingSet.Bind(_adapter).For(v => v.ItemsSource).To(vm => vm.Items);
            bindingSet.Bind(_recyclerView).For(v => v.LoadMoreItemsCommand).To(vm => vm.LoadMoreItemsCommand);
            bindingSet.Bind(_swipeRefreshLayout).For(v => v.Refreshing).To(vm => vm.IsBusy);
            bindingSet.Bind(_swipeRefreshLayout).For(v => v.RefreshCommand).To(vm => vm.LoadDataCommand);
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
            var tabType = tab == _inExecutionTab
                ? CompetitionsTabType.OnExecution
                : CompetitionsTabType.Ordered;

            ViewModel.SelectedTabType = tabType;
        }

        public void OnTabUnselected(TabLayout.Tab tab)
        {
        }

        private void InitializeEmptyView()
        {
            _emptyView = FindViewById<View>(Resource.Id.empty_view);
            _emptyViewTitleTextView = _emptyView.FindViewById<TextView>(Resource.Id.title_text_view);
            _emptyViewTitleTextView.Text = Core.Localization.Resources.CompetitionsListIsEmpty;
        }
    }
}
