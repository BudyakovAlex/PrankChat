using Android.Views;
using Android.Widget;
using FFImageLoading.Cross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Abstract;

namespace PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Competitions
{
    public class CompetitionItemViewHolder : CardViewHolder
    {
        private TextView _titleTextView;
        private TextView _descriptionTextView;
        private TextView _termTitle;
        private TextView _termTimerTextView;
        private TextView _daysTextView;
        private TextView _hoursTextView;
        private TextView _minutesTextView;
        private TextView _prizeTitleTextView;
        private TextView _prizeTextView;
        private TextView _numberTextView;
        private TextView _likesTextView;
        private ImageView _likesImageView;
        private MvxCachedImageView _placeholderImageView;
        private Button _actionButton;

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
            _prizeTitleTextView = view.FindViewById<TextView>(Resource.Id.prize_title_text_view);
            _prizeTextView = view.FindViewById<TextView>(Resource.Id.prize_text_view);
            _numberTextView = view.FindViewById<TextView>(Resource.Id.number_text_view);
            _likesTextView = view.FindViewById<TextView>(Resource.Id.likes_text_view);
            _likesImageView = view.FindViewById<ImageView>(Resource.Id.likes_image_view);
            _placeholderImageView = view.FindViewById<MvxCachedImageView>(Resource.Id.placeholder_image_view);
            _actionButton = view.FindViewById<Button>(Resource.Id.action_button);
        }

        public override void BindData()
        {
            base.BindData();

            var bindingSet = this.CreateBindingSet<CompetitionItemViewHolder, CompetitionItemViewModel>();
        }
    }
}