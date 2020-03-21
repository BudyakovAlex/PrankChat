using Android.Views;
using Android.Widget;
using FFImageLoading.Cross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items;
using PrankChat.Mobile.Droid.Extensions;
using PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Abstract;
using PrankChat.Mobile.Droid.Utils.Helpers;

namespace PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Competitions
{
    public class CompetitionItemViewHolder : CardViewHolder
    {
        private const int CardCornerRadius = 15;

        private TextView _titleTextView;
        private TextView _descriptionTextView;
        private TextView _termTitle;
        private TextView _termTimerTextView;
        private TextView _daysTextView;
        private TextView _hoursTextView;
        private TextView _minutesTextView;
        private TextView _prizeTextView;
        private TextView _numberTextView;
        private TextView _likesTextView;
        private TextView _termFromTextView;
        private TextView _termToTextView;
        private LinearLayout _termLinearLayout;
        private ImageView _likesImageView;
        private MvxCachedImageView _placeholderImageView;
        private Button _actionButton;
        private FrameLayout _borderFrame;
        private FrameLayout _backgroundFrame;

        public CompetitionItemViewHolder(View view, IMvxAndroidBindingContext context) : base(view, context)
        {
        }

        protected override void DoInit(View view)
        {
            base.DoInit(view);

            _titleTextView = view.FindViewById<TextView>(Resource.Id.title_text_view);
            _descriptionTextView = view.FindViewById<TextView>(Resource.Id.description_text_view);
            _termTitle = view.FindViewById<TextView>(Resource.Id.term_title_text_view);
            _termTimerTextView = view.FindViewById<TextView>(Resource.Id.term_timer_text_view);
            _daysTextView = view.FindViewById<TextView>(Resource.Id.time_days_text_view);
            _hoursTextView = view.FindViewById<TextView>(Resource.Id.time_hours_text_view);
            _minutesTextView = view.FindViewById<TextView>(Resource.Id.time_minutes_text_view);
            _prizeTextView = view.FindViewById<TextView>(Resource.Id.prize_text_view);
            _numberTextView = view.FindViewById<TextView>(Resource.Id.number_text_view);
            _termFromTextView = view.FindViewById<TextView>(Resource.Id.term_from_text_view);
            _termToTextView = view.FindViewById<TextView>(Resource.Id.term_to_text_view);
            _termLinearLayout = view.FindViewById<LinearLayout>(Resource.Id.term_linear_layout);
            _likesTextView = view.FindViewById<TextView>(Resource.Id.likes_text_view);
            _likesImageView = view.FindViewById<ImageView>(Resource.Id.likes_image_view);
            _placeholderImageView = view.FindViewById<MvxCachedImageView>(Resource.Id.placeholder_image_view);
            _actionButton = view.FindViewById<Button>(Resource.Id.action_button);
            _borderFrame = view.FindViewById<FrameLayout>(Resource.Id.border_frame);
            _backgroundFrame = view.FindViewById<FrameLayout>(Resource.Id.background_frame);

            var cornersInPx = DisplayUtils.DpToPx(CardCornerRadius);
            _borderFrame.SetRoundedCorners(cornersInPx);
            _backgroundFrame.SetRoundedCorners(cornersInPx);
        }

        public override void BindData()
        {
            base.BindData();

            var bindingSet = this.CreateBindingSet<CompetitionItemViewHolder, CompetitionItemViewModel>();

            bindingSet.Bind(_titleTextView).For(v => v.Text).To(vm => vm.Title);
            bindingSet.Bind(_descriptionTextView).For(v => v.Text).To(vm => vm.Description);

            bindingSet.Bind(_termTimerTextView).For(v => v.Text).To(vm => vm.Description);
            bindingSet.Bind(_termTitle).For(v => v.Text).To(vm => vm.Description);

            bindingSet.Bind(_daysTextView).For(v => v.Text).To(vm => vm.NextPhaseCountdown);
            bindingSet.Bind(_hoursTextView).For(v => v.Text).To(vm => vm.NextPhaseCountdown);
            bindingSet.Bind(_minutesTextView).For(v => v.Text).To(vm => vm.NextPhaseCountdown);

            bindingSet.Bind(_prizeTextView).For(v => v.Text).To(vm => vm.PrizePool);
            bindingSet.Bind(_numberTextView).For(v => v.Text).To(vm => vm.Id);

            bindingSet.Bind(_likesTextView).For(v => v.Text).To(vm => vm.LikesCount);

            //bindingSet.Bind(_placeholderImageView).For(v => v.ImagePath).To(vm => vm.ImagePath);
            //bindingSet.Bind(_actionButton).For(v => v.Text).To(vm => vm.Phase).WithConversion();
        }
    }
}