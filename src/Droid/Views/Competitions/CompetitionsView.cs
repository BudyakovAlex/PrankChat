using Android.Runtime;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.DroidX;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.ViewModels;
using PrankChat.Mobile.Core.ViewModels.Competition;
using PrankChat.Mobile.Droid.Adapters;
using PrankChat.Mobile.Droid.Adapters.TemplateSelectors;
using PrankChat.Mobile.Droid.Adapters.ViewHolders.Competitions;
using PrankChat.Mobile.Droid.Views.Base;

namespace PrankChat.Mobile.Droid.Views.Competitions
{
    [MvxTabLayoutPresentation(TabLayoutResourceId = Resource.Id.tabs, ViewPagerResourceId = Resource.Id.viewpager, ActivityHostViewModelType = typeof(MainViewModel))]
    [Register(nameof(CompetitionsView))]
    public class CompetitionsView : BaseRefreshableTabFragment<CompetitionsViewModel>, IScrollableView
    {
        private RecycleViewBindableAdapter _adapter;
        private MvxSwipeRefreshLayout _refreshView;
        private MvxRecyclerView _competitionsRecyclerView;

        public CompetitionsView() : base(Resource.Layout.fragment_competitions)
        {
        }

        public RecyclerView RecyclerView => _competitionsRecyclerView;

        protected override void RefreshData() =>
            ViewModel?.LoadDataCommand.Execute();

        protected override void SetViewProperties(View view)
        {
            base.SetViewProperties(view);

            _refreshView = view.FindViewById<MvxSwipeRefreshLayout>(Resource.Id.swipe_refresh);

            _competitionsRecyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.competitions_recycler_view);
            var layoutManager = new LinearLayoutManager(Context, LinearLayoutManager.Vertical, false);
            _competitionsRecyclerView.SetLayoutManager(layoutManager);

            _adapter = new RecycleViewBindableAdapter((IMvxAndroidBindingContext)BindingContext);
            _competitionsRecyclerView.Adapter = _adapter;
            _competitionsRecyclerView.ItemTemplateSelector = new TemplateSelector()
                .AddElement<CompetitionsSectionViewModel, CompetitionsSectionViewHolder>(Resource.Layout.cell_competitions_section);
        }

        protected override void Bind()
        {
            base.Bind();

            using var bindingSet = this.CreateBindingSet<CompetitionsView, CompetitionsViewModel>();

            bindingSet.Bind(_adapter).For(v => v.ItemsSource).To(vm => vm.Items);
            bindingSet.Bind(_refreshView).For(v => v.Refreshing).To(vm => vm.IsBusy);
            bindingSet.Bind(_refreshView).For(v => v.RefreshCommand).To(vm => vm.LoadDataCommand);
        }
    }
}