using Android.App;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using PrankChat.Mobile.Core.Converters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items;
using PrankChat.Mobile.Droid.Decorators;
using PrankChat.Mobile.Droid.Extensions;
using PrankChat.Mobile.Droid.LayoutManagers;
using PrankChat.Mobile.Droid.Presentation.Adapters.TemplateSelectors;
using PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Abstract;
using PrankChat.Mobile.Droid.Presentation.Bindings;
using PrankChat.Mobile.Droid.Presentation.Converters;
using PrankChat.Mobile.Droid.Presentation.Listeners;

namespace PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Competitions
{
    public class CompetitionsSectionViewHolder : CardViewHolder, INestedCardViewHolder
    {
        public TextView _titleTextView;
        public MvxRecyclerView _sectionRecyclerView;
        public ImageView _leftImageView;
        public ImageView _rightImageView;
        private RecycleViewBindableAdapter _adapter;
        private CustomLinearLayoutManager _layoutManager;
        private View _leftDivider;
        private View _rightDivider;

        public int RecycledViewsVisibleCount => 5;

        public RecyclerView NestedRecyclerView => _sectionRecyclerView;

        public CompetitionsSectionViewHolder(View view, IMvxAndroidBindingContext context) : base(view, context)
        {
        }

        public override void BindData()
        {
            base.BindData();

            using var bindingSet = this.CreateBindingSet<CompetitionsSectionViewHolder, CompetitionsSectionViewModel>();

            bindingSet.Bind(_sectionRecyclerView).For(v => v.ItemsSource).To(vm => vm.Items);
            bindingSet.Bind(_titleTextView).For(v => v.Text).To(vm => vm.Phase)
                      .WithConversion<CompetitionPhaseToSectionTitleConverter>();
            bindingSet.Bind(_leftImageView).For(v => v.Visibility).To(vm => vm.HasNavigationControls)
                      .WithConversion<BoolToGoneConverter>();
            bindingSet.Bind(_rightImageView).For(v => v.Visibility).To(vm => vm.HasNavigationControls)
                      .WithConversion<BoolToGoneConverter>();
            bindingSet.Bind(_leftDivider).For(v => v.BindColor()).To(vm => vm.Phase)
                      .WithConversion<CompetitionPhaseToBorderBackgroundConverter>();
            bindingSet.Bind(_rightDivider).For(v => v.BindColor()).To(vm => vm.Phase)
                      .WithConversion<CompetitionPhaseToBorderBackgroundConverter>();
        }

        protected override void DoInit(View view)
        {
            base.DoInit(view);

            _leftDivider = view.FindViewById<View>(Resource.Id.first_divider);
            _rightDivider = view.FindViewById<View>(Resource.Id.second_divider);

            _titleTextView = view.FindViewById<TextView>(Resource.Id.title_text_view);
            _leftImageView = view.FindViewById<ImageView>(Resource.Id.left_navigation_image);
            _rightImageView = view.FindViewById<ImageView>(Resource.Id.right_navigation_image);

            var viewOnClickListener = new ViewOnClickListener(OnNavigationImageClicked);
            _leftImageView.SetOnClickListener(viewOnClickListener);
            _rightImageView.SetOnClickListener(viewOnClickListener);

            InitRecyclerView(view);
        }

        private void OnNavigationImageClicked(View view)
        {
            var position = _layoutManager.FindFirstCompletelyVisibleItemPosition();
            switch (view.Id)
            {
                case Resource.Id.left_navigation_image:
                    if (position == 0)
                    {
                        return;
                    }

                    _sectionRecyclerView.SmoothScrollToPosition(position - 1);
                    break;

                case Resource.Id.right_navigation_image:
                    if (position == _adapter.ItemCount - 1)
                    {
                        return;
                    }

                    _sectionRecyclerView.SmoothScrollToPosition(position + 1);
                    break;
            }
        }

        private void InitRecyclerView(View view)
        {
            _sectionRecyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.section_recycler_view);

            var offset = Application.Context.Resources.GetDimensionPixelSize(Resource.Dimension.card_offset);
            _layoutManager = new CustomLinearLayoutManager(view.Context, LinearLayoutManager.Horizontal, false, 1)
            {
                Offset = offset
            };

            _sectionRecyclerView.SetLayoutManager(_layoutManager);

            _adapter = new RecycleViewBindableAdapter((IMvxAndroidBindingContext)BindingContext);
            _sectionRecyclerView.Adapter = _adapter;
            _sectionRecyclerView.ItemTemplateSelector = new TemplateSelector()
                .AddElement<CompetitionItemViewModel, CompetitionItemViewHolder>(Resource.Layout.cell_competition);

            _sectionRecyclerView.AddItemDecoration(new PagerDecorator(Application.Context));
            var snapHelper = new PagerSnapHelper();
            snapHelper.AttachToRecyclerView(_sectionRecyclerView);
        }
    }
}