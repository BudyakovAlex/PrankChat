using Android.Views;
using Android.Widget;
using FFImageLoading.Cross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Combiners;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using PrankChat.Mobile.Core.Converters;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items;
using PrankChat.Mobile.Droid.Extensions;
using PrankChat.Mobile.Droid.Presentation.Adapters.ViewHolders.Abstract;
using PrankChat.Mobile.Droid.Presentation.Bindings;
using PrankChat.Mobile.Droid.Presentation.Converters;
using PrankChat.Mobile.Droid.Utils.Helpers;

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
        private TextView _prizeTextView;
        private TextView _prizeTitleTextView;
        private View _thirdDividerView;
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
            _daysTextView = view.FindViewById<TextView>(Resource.Id.days_text_view);
            _hoursTextView = view.FindViewById<TextView>(Resource.Id.hours_text_view);
            _minutesTextView = view.FindViewById<TextView>(Resource.Id.minutes_text_view);
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
            _prizeTitleTextView = view.FindViewById<TextView>(Resource.Id.prize_title_text_view);
            _thirdDividerView = view.FindViewById<View>(Resource.Id.third_divider);
            _prizeTitleTextView.Text = Resources.Competitions_Prize_Pool;

            _borderFrame.SetRoundedCorners(DisplayUtils.DpToPx(15));
            _backgroundFrame.SetRoundedCorners(DisplayUtils.DpToPx(13));
        }

        public override void BindData()
        {
            base.BindData();

            var bindingSet = this.CreateBindingSet<CompetitionItemViewHolder, CompetitionItemViewModel>();

            bindingSet.Bind(_titleTextView)
                      .For(v => v.Text)
                      .To(vm => vm.Title);

            bindingSet.Bind(_descriptionTextView)
                      .For(v => v.Text)
                      .To(vm => vm.Description);

            bindingSet.Bind(_termTitle)
                      .For(v => v.Text)
                      .To(vm => vm.Phase)
                      .WithConversion<CompetitionPhaseToTermTitleConverter>();

            bindingSet.Bind(_prizeTextView)
                      .For(v => v.Text)
                      .To(vm => vm.PrizePoolPresentation);

            bindingSet.Bind(_numberTextView)
                      .For(v => v.Text)
                      .To(vm => vm.Number);

            bindingSet.Bind(_likesTextView)
                      .For(v => v.Text)
                      .To(vm => vm.LikesCountString);

            bindingSet.Bind(_numberTextView)
                      .For(v => v.BindHidden())
                      .To(vm => vm.IsLikesUnavailable);

            bindingSet.Bind(_likesImageView)
                      .For(v => v.BindHidden())
                      .To(vm => vm.IsLikesUnavailable);

            bindingSet.Bind(_likesTextView)
                      .For(v => v.BindHidden())
                      .To(vm => vm.IsLikesUnavailable);

            bindingSet.Bind(_thirdDividerView)
                      .For(v => v.BindHidden())
                      .To(vm => vm.IsLikesUnavailable);

            bindingSet.Bind(_termTimerTextView)
                      .For(v => v.Text)
                      .To(vm => vm.NextPhaseCountdown)
                      .WithConversion(StringFormatValueConverter.Name, Constants.Formats.DateWithSpace);

            bindingSet.Bind(_termFromTextView)
                      .For(v => v.Text)
                      .To(vm => vm.CreatedAt)
                      .WithConversion(StringFormatValueConverter.Name, Constants.Formats.DateTimeFormat);

            bindingSet.Bind(_termToTextView)
                      .For(v => v.Text)
                      .To(vm => vm.ActiveTo)
                      .WithConversion(StringFormatValueConverter.Name, Constants.Formats.DateTimeFormat);

            bindingSet.Bind(_borderFrame)
                      .For(BackgroundColorBinding.TargetBinding)
                      .To(vm => vm.Phase)
                      .WithConversion<CompetitionPhaseToBorderBackgroundConverter>();

            bindingSet.Bind(_backgroundFrame)
                      .For(BackgroundResourceBinding.TargetBinding)
                      .To(vm => vm.Phase)
                      .WithConversion<CompetitionPhaseToBackgroundConverter>();

            bindingSet.Bind(_placeholderImageView)
                      .For(v => v.ImagePath)
                      .To(vm => vm.ImageUrl);

            bindingSet.Bind(_actionButton)
                      .For(v => v.Text)
                      .To(vm => vm.Phase)
                      .WithConversion<CompetitionPhaseToActionButtonTitleConverter>();

            bindingSet.Bind(_actionButton)
                      .For(v => v.BindClick())
                      .To(vm => vm.ActionCommand);

            bindingSet.Bind(_termLinearLayout)
                      .For(v => v.Visibility)
                      .To(vm => vm.IsFinished)
                      .WithConversion<BoolToGoneConverter>();

            bindingSet.Bind(_termTimerTextView)
                      .For(v => v.Visibility)
                      .To(vm => vm.IsFinished)
                      .WithConversion<BoolToGoneInvertedConverter>();

            bindingSet.Bind(_daysTextView)
                      .For(v => v.Visibility)
                      .To(vm => vm.IsFinished)
                      .WithConversion<BoolToGoneInvertedConverter>();

            bindingSet.Bind(_hoursTextView)
                      .For(v => v.Visibility)
                      .To(vm => vm.IsFinished)
                      .WithConversion<BoolToGoneInvertedConverter>();

            bindingSet.Bind(_minutesTextView)
                      .For(v => v.Visibility)
                      .To(vm => vm.IsFinished)
                      .WithConversion<BoolToGoneInvertedConverter>();

            bindingSet.Bind(_daysTextView)
                      .For(v => v.Text)
                      .To(vm => vm.DaysText);

            bindingSet.Bind(_hoursTextView)
                      .For(v => v.Text)
                      .To(vm => vm.HoursText);

            bindingSet.Bind(_minutesTextView)
                      .For(v => v.Text)
                      .To(vm => vm.MinutesText);

            bindingSet.Apply();
        }
    }
}
