using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.DroidX;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition;
using PrankChat.Mobile.Droid.Presentation.Adapters;
using PrankChat.Mobile.Droid.Presentation.Adapters.TemplateSelectors;
using PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Competitions;
using PrankChat.Mobile.Droid.Presentation.Views.Base;

namespace PrankChat.Mobile.Droid.Presentation.Views.Competitions
{
    [MvxTabLayoutPresentation(TabLayoutResourceId = Resource.Id.tabs, ViewPagerResourceId = Resource.Id.viewpager, ActivityHostViewModelType = typeof(MainViewModel))]
    [Register(nameof(CompetitionsView))]
    public class CompetitionsView : BaseTabFragment<CompetitionsViewModel>, IScrollableView
    {
        private RecycleViewBindableAdapter _adapter;
        private MvxSwipeRefreshLayout _refreshView;
        private MvxRecyclerView _competitionsRecyclerView;

        public RecyclerView RecyclerView => _competitionsRecyclerView;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.fragment_competitions, null);

            InitializeControls(view);
            DoBind();
            return view;
        }

        private void DoBind()
        {
            var bindingSet = this.CreateBindingSet<CompetitionsView, CompetitionsViewModel>();

            bindingSet.Bind(_adapter)
                      .For(v => v.ItemsSource)
                      .To(vm => vm.Items);

            bindingSet.Bind(_refreshView)
                      .For(v => v.Refreshing)
                      .To(vm => vm.IsBusy);

            bindingSet.Bind(_refreshView)
                      .For(v => v.RefreshCommand)
                      .To(vm => vm.LoadDataCommand);

            bindingSet.Apply();
        }

        private void InitializeControls(View view)
        {
            _refreshView = view.FindViewById<MvxSwipeRefreshLayout>(Resource.Id.swipe_refresh);

            _competitionsRecyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.competitions_recycler_view);
            var layoutManager = new LinearLayoutManager(Context, LinearLayoutManager.Vertical, false);
            _competitionsRecyclerView.SetLayoutManager(layoutManager);

            _adapter = new RecycleViewBindableAdapter((IMvxAndroidBindingContext)BindingContext);
            _competitionsRecyclerView.Adapter = _adapter;
            _competitionsRecyclerView.ItemTemplateSelector = new TemplateSelector()
                .AddElement<CompetitionsSectionViewModel, CompetitionsSectionViewHolder>(Resource.Layout.cell_competitions_section);
        }
    }
}