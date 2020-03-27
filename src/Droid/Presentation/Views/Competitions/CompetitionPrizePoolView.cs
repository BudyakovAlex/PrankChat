﻿using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items;
using PrankChat.Mobile.Droid.Presentation.Adapters;
using PrankChat.Mobile.Droid.Presentation.Adapters.TemplateSelectors;
using PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Competitions;
using PrankChat.Mobile.Droid.Presentation.Views.Base;

namespace PrankChat.Mobile.Droid.Presentation.Views.Competitions
{
    [MvxActivityPresentation]
    [Activity]
    public class CompetitionPrizePoolView : BaseView<CompetitionPrizePoolViewModel>
    {
        private TextView _prizePoolTextView;
        private MvxRecyclerView _recyclerView;
        private RecycleViewBindableAdapter _adapter;

        protected override bool HasBackButton => true;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle, Resource.Layout.activity_competition_prize_pool);
        }

        protected override void SetViewProperties()
        {
            _prizePoolTextView = FindViewById<TextView>(Resource.Id.prize_pool_text_view);
            _recyclerView = FindViewById<MvxRecyclerView>(Resource.Id.recycler_view);

            var layoutManager = new LinearLayoutManager(this, LinearLayoutManager.Vertical, false);
            _recyclerView.SetLayoutManager(layoutManager);

            _adapter = new RecycleViewBindableAdapter((IMvxAndroidBindingContext)BindingContext);
            _recyclerView.Adapter = _adapter;
            _recyclerView.ItemTemplateSelector = new TemplateSelector()
                .AddElement<CompetitionPrizePoolItemViewModel, CompetitionPrizePoolItemViewHolder>(Resource.Layout.cell_competiton_prize_pool);
        }

        protected override void DoBind()
        {
            base.DoBind();

            var bindingSet = this.CreateBindingSet<CompetitionPrizePoolView, CompetitionPrizePoolViewModel>();

            bindingSet.Bind(_adapter)
                      .For(v => v.ItemsSource)
                      .To(vm => vm.Items);

            bindingSet.Bind(_prizePoolTextView)
                      .For(v => v.Text)
                      .To(vm => vm.PrizePool);

            bindingSet.Apply();
        }
    }
}
