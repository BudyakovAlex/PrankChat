using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items;
using PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Abstract;

namespace PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Competitions
{
    public class CompetitionPrizePoolItemViewHolder : CardViewHolder
    {
        private TextView _numberTextView;
        private TextView _userTextView;
        private TextView _raitingTextView;

        public CompetitionPrizePoolItemViewHolder(View view, IMvxAndroidBindingContext context) : base(view, context)
        {
        }

        protected override void DoInit(View view)
        {
            base.DoInit(view);

            _numberTextView = view.FindViewById<TextView>(Resource.Id.number_text_view);
            _userTextView = view.FindViewById<TextView>(Resource.Id.participant_text_view);
            _raitingTextView = view.FindViewById<TextView>(Resource.Id.rating_text_view);
        }

        public override void BindData()
        {
            base.BindData();

            var bindingSet = this.CreateBindingSet<CompetitionPrizePoolItemViewHolder, CompetitionPrizePoolItemViewModel>();

            bindingSet.Bind(_numberTextView).For(v => v.Text).To(vm => vm.Position);
            bindingSet.Bind(_userTextView).For(v => v.Text).To(vm => vm.Participant);
            bindingSet.Bind(_raitingTextView).For(v => v.Text).To(vm => vm.Rating);

            bindingSet.Apply();
        }
    }
}