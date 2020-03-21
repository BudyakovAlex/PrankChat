﻿using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items;
using PrankChat.Mobile.Droid.LayoutManagers;
using PrankChat.Mobile.Droid.Presentation.Adapters.TemplateSelectors;
using PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Abstract;
using PrankChat.Mobile.Droid.Utils.Helpers;

namespace PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Competitions
{
    public class CompetitionsSectionViewHolder : CardViewHolder
    {
        public TextView _titleTextView;
        public MvxRecyclerView _sectionRecyclerView;
        public ImageView _leftImageView;
        public ImageView _rightImageView;
        private RecycleViewBindableAdapter _adapter;

        public CompetitionsSectionViewHolder(View view, IMvxAndroidBindingContext context) : base(view, context)
        {
        }

        public override void BindData()
        {
            base.BindData();

            var bindingSet = this.CreateBindingSet<CompetitionsSectionViewHolder, CompetitionsSectionViewModel>();

            bindingSet.Bind(_sectionRecyclerView).For(v => v.ItemsSource).To(vm => vm.Items);
            bindingSet.Bind(_titleTextView).For(v => v.Text).To(vm => vm.Phase);

            bindingSet.Apply();
        }

        protected override void DoInit(View view)
        {
            base.DoInit(view);

            _titleTextView = view.FindViewById<TextView>(Resource.Id.title_text_view);
            _leftImageView = view.FindViewById<ImageView>(Resource.Id.left_navigation_image);
            _rightImageView = view.FindViewById<ImageView>(Resource.Id.right_navigation_image);

            InitRecyclerView(view);
        }

        private void InitRecyclerView(View view)
        {
            _sectionRecyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.section_recycler_view);

            var layoutManager = new CustomLinearLayoutManager(view.Context, LinearLayoutManager.Horizontal, false, 1)
            {
                Offset = DisplayUtils.DpToPx(20)
            };

            _sectionRecyclerView.SetLayoutManager(layoutManager);

            var adapter = new RecycleViewBindableAdapter((IMvxAndroidBindingContext)BindingContext);
            _sectionRecyclerView.Adapter = adapter;
            _sectionRecyclerView.ItemTemplateSelector = new TemplateSelector()
                .AddElement<CompetitionItemViewModel, CompetitionItemViewHolder>(Resource.Layout.cell_competition);

            var snapHelper = new PagerSnapHelper();
            snapHelper.AttachToRecyclerView(_sectionRecyclerView);
        }
    }
}