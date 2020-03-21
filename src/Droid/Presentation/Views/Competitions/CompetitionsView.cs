using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Droid.Support.V7.RecyclerView;
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
    public class CompetitionsView : BaseTabFragment<CompetitionsViewModel>
    {
        private RecycleViewBindableAdapter _adapter;

        public CompetitionsView()
        {
        }

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

            bindingSet.Apply();
        }

        private void InitializeControls(View view)
        {
            var competitionsRecyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.competitions_recycler_view);
            var layoutManager = new LinearLayoutManager(Context, LinearLayoutManager.Vertical, false);
            competitionsRecyclerView.SetLayoutManager(layoutManager);

            _adapter = new RecycleViewBindableAdapter((IMvxAndroidBindingContext)BindingContext);
            competitionsRecyclerView.Adapter = _adapter;
            competitionsRecyclerView.ItemTemplateSelector = new TemplateSelector()
                .AddElement<CompetitionsSectionViewModel, CompetitionsSectionViewHolder>(Resource.Layout.cell_competitions_section);
        }
    }
}