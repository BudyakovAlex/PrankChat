using System;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using PrankChat.Mobile.Core.ViewModels.Competition.Items;
using PrankChat.Mobile.Droid.Adapters.ViewHolders.Abstract;
using PrankChat.Mobile.Droid.Bindings;
using PrankChat.Mobile.Droid.Converters;

namespace PrankChat.Mobile.Droid.Adapters.ViewHolders.Competitions
{
    public class CompetitionPrizePoolItemViewHolder : CardViewHolder
    {
        private View _parentView;
        private TextView _numberTextView;
        private TextView _userTextView;
        private TextView _votesTextView;
        private TextView _prizeTextView;

        public CompetitionPrizePoolItemViewHolder(View view, IMvxAndroidBindingContext context) : base(view, context)
        {
        }

        protected override void DoInit(View view)
        {
            base.DoInit(view);

            _parentView = view.FindViewById<View>(Resource.Id.parent_view);
            _numberTextView = view.FindViewById<TextView>(Resource.Id.number_text_view);
            _userTextView = view.FindViewById<TextView>(Resource.Id.participant_text_view);
            _votesTextView = view.FindViewById<TextView>(Resource.Id.votes_text_view);
            _prizeTextView = view.FindViewById<TextView>(Resource.Id.prize_text_view);
        }

        public override void BindData()
        {
            base.BindData();

            using var bindingSet = this.CreateBindingSet<CompetitionPrizePoolItemViewHolder, CompetitionPrizePoolItemViewModel>();

            bindingSet.Bind(_numberTextView).For(v => v.Text).To(vm => vm.Position);
            bindingSet.Bind(_userTextView).For(v => v.Text).To(vm => vm.Participant);
            bindingSet.Bind(_votesTextView).For(v => v.Text).To(vm => vm.Rating);
            bindingSet.Bind(_prizeTextView).For(v => v.Text).To(vm => vm.Prize);
            bindingSet.Bind(_parentView).For(BackgroundColorBinding.TargetBinding).To(vm => vm.IsMyPosition)
                .WithConversion(BoolToResourceConverter.Name, Tuple.Create(Resource.Color.dark_purple, Resource.Color.deep_purple));
        }
    }
}