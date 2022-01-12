using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using FFImageLoading.Cross;
using Google.Android.Material.Button;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using PrankChat.Mobile.Core.Common;
using PrankChat.Mobile.Core.Converters;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.ViewModels.Competitions.Items;
using PrankChat.Mobile.Droid.Adapters.ViewHolders.Abstract;
using PrankChat.Mobile.Droid.Controls;
using PrankChat.Mobile.Droid.Converters;

namespace PrankChat.Mobile.Droid.Adapters.ViewHolders.Competitions
{
    public class CompetitionDetailsHeaderViewHolder : CardViewHolder
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
        private TextView _numberTextView;
        private TextView _likesTextView;
        private TextView _durationTextView;
        private LinearLayout _termLinearLayout;
        private ImageView _likesImageView;
        private MvxCachedImageView _placeholderImageView;
        private CircleCachedImageView _customerImageView;
        private AppCompatButton _deleteButton;
        private AppCompatButton _statisticsButton;

        //TODO: add bindings for action buttons
        private MaterialButton _actionButton;
        private AppCompatButton _rulesButton;
        private AppCompatButton _resultsButton;
        private ImageView _paidFlagImageView;
        private ImageView _privateFlagImageView;

        public CompetitionDetailsHeaderViewHolder(View view, IMvxAndroidBindingContext context) : base(view, context)
        {
        }

        protected override void DoInit(View view)
        {
            base.DoInit(view);

            _paidFlagImageView = view.FindViewById<ImageView>(Resource.Id.paid_flag_image_view);
            _privateFlagImageView = view.FindViewById<ImageView>(Resource.Id.private_flag_image_view);
            _titleTextView = view.FindViewById<TextView>(Resource.Id.title_text_view);
            _descriptionTextView = view.FindViewById<TextView>(Resource.Id.description_text_view);
            _termTitle = view.FindViewById<TextView>(Resource.Id.term_title_text_view);
            _termTimerTextView = view.FindViewById<TextView>(Resource.Id.term_timer_text_view);
            _daysTextView = view.FindViewById<TextView>(Resource.Id.days_text_view);
            _hoursTextView = view.FindViewById<TextView>(Resource.Id.hours_text_view);
            _minutesTextView = view.FindViewById<TextView>(Resource.Id.minutes_text_view);
            _prizeTextView = view.FindViewById<TextView>(Resource.Id.prize_text_view);
            _numberTextView = view.FindViewById<TextView>(Resource.Id.number_text_view);
            _durationTextView = view.FindViewById<TextView>(Resource.Id.duration_text_view);
            _termLinearLayout = view.FindViewById<LinearLayout>(Resource.Id.term_linear_layout);
            _likesTextView = view.FindViewById<TextView>(Resource.Id.like_text_view);
            _likesImageView = view.FindViewById<ImageView>(Resource.Id.likes_image_view);
            _placeholderImageView = view.FindViewById<MvxCachedImageView>(Resource.Id.placeholder_image_view);
            _prizeTitleTextView = view.FindViewById<TextView>(Resource.Id.prize_title_text_view);
            _customerImageView = view.FindViewById<CircleCachedImageView>(Resource.Id.customer_image_view);
            _actionButton = view.FindViewById<MaterialButton>(Resource.Id.load_video_button);
            _rulesButton = view.FindViewById<AppCompatButton>(Resource.Id.rules_button);
            _resultsButton = view.FindViewById<AppCompatButton>(Resource.Id.results_button);
            _deleteButton = view.FindViewById<AppCompatButton>(Resource.Id.delete_button);
            _statisticsButton = view.FindViewById<AppCompatButton>(Resource.Id.statistics_button);

            _placeholderImageView.OnError += (s, e) => _placeholderImageView.SetBackgroundResource(Resource.Drawable.button_accent_background);
            _prizeTitleTextView.Text = Resources.TournamentPrizePool;
        }

        public override void BindData()
        {
            base.BindData();

            using var bindingSet = this.CreateBindingSet<CompetitionDetailsHeaderViewHolder, CompetitionDetailsHeaderViewModel>();

            bindingSet.Bind(_titleTextView).For(v => v.Text).To(vm => vm.Title);
            bindingSet.Bind(_descriptionTextView).For(v => v.Text).To(vm => vm.Description);
            bindingSet.Bind(_termTitle).For(v => v.Text).To(vm => vm.Phase)
                      .WithConversion<CompetitionPhaseToTermTitleConverter>();
            bindingSet.Bind(_prizeTextView).For(v => v.Text).To(vm => vm.PrizePoolPresentation);
            bindingSet.Bind(_numberTextView).For(v => v.Text).To(vm => vm.Number);
            bindingSet.Bind(_likesTextView).For(v => v.Text).To(vm => vm.LikesCountString);
            bindingSet.Bind(_numberTextView).For(v => v.BindHidden()).To(vm => vm.CanExecuteActionVideo);
            bindingSet.Bind(_likesImageView).For(v => v.BindHidden()).To(vm => vm.CanExecuteActionVideo);
            bindingSet.Bind(_likesTextView).For(v => v.BindHidden()).To(vm => vm.CanExecuteActionVideo);
            bindingSet.Bind(_termTimerTextView).For(v => v.Text).To(vm => vm.NextPhaseCountdown)
                      .WithConversion(StringFormatValueConverter.Name, Constants.Formats.DateWithSpace);
            bindingSet.Bind(_durationTextView).For(v => v.Text).To(vm => vm.Duration);
            bindingSet.Bind(_durationTextView).For(v => v.Visibility).To(vm => vm.IsFinished)
                      .WithConversion<BoolToGoneConverter>();
            bindingSet.Bind(_placeholderImageView).For(v => v.ImagePath).To(vm => vm.ImageUrl);
            bindingSet.Bind(_termLinearLayout).For(v => v.Visibility).To(vm => vm.IsFinished)
                      .WithConversion<BoolToGoneConverter>();
            bindingSet.Bind(_termTimerTextView).For(v => v.Visibility).To(vm => vm.IsFinished)
                      .WithConversion<BoolToGoneInvertedConverter>();
            bindingSet.Bind(_daysTextView).For(v => v.Visibility).To(vm => vm.IsFinished)
                      .WithConversion<BoolToGoneInvertedConverter>();
            bindingSet.Bind(_hoursTextView).For(v => v.Visibility).To(vm => vm.IsFinished)
                      .WithConversion<BoolToGoneInvertedConverter>();
            bindingSet.Bind(_minutesTextView).For(v => v.Visibility).To(vm => vm.IsFinished)
                      .WithConversion<BoolToGoneInvertedConverter>();
            bindingSet.Bind(_daysTextView).For(v => v.Text).To(vm => vm.DaysText);
            bindingSet.Bind(_hoursTextView).For(v => v.Text).To(vm => vm.HoursText);
            bindingSet.Bind(_minutesTextView).For(v => v.Text).To(vm => vm.MinutesText);
            bindingSet.Bind(_actionButton).For(v => v.Text).To(vm => vm.ActionTitle);
            bindingSet.Bind(_actionButton).For(v => v.BindClick()).To(vm => vm.ActionCommand);
            bindingSet.Bind(_actionButton).For(v => v.Visibility).To(vm => vm.CanExecuteActionVideo)
                      .WithConversion<BoolToGoneConverter>();
            bindingSet.Bind(_rulesButton).For(v => v.BindClick()).To(vm => vm.OpenRulesCommand);
            bindingSet.Bind(_rulesButton).For(v => v.BindVisible()).To(vm => vm.CanShowRules);
            bindingSet.Bind(_resultsButton).For(v => v.BindClick()).To(vm => vm.OpenPrizePoolCommand);
            bindingSet.Bind(_resultsButton).For(v => v.BindHidden()).To(vm => vm.IsModeration);
            bindingSet.Bind(_privateFlagImageView).For(v => v.BindVisible()).To(vm => vm.Category)
                      .WithConversion(new DelegateConverter<OrderCategory, bool>((category) => category == OrderCategory.PrivatePaidCompetition));
            bindingSet.Bind(_paidFlagImageView).For(v => v.BindVisible()).To(vm => vm.Category)
                      .WithConversion(new DelegateConverter<OrderCategory, bool>((category) => category == OrderCategory.PaidCompetition || category == OrderCategory.PrivatePaidCompetition));
            bindingSet.Bind(_customerImageView).For(v => v.ImagePath).To(vm => vm.CustomerAvatarUrl);
            bindingSet.Bind(_customerImageView).For(v => v.BindVisible()).To(vm => vm.IsCustomerAttached);
            bindingSet.Bind(_customerImageView).For(v => v.PlaceholderText).To(vm => vm.CustomerShortName);
            bindingSet.Bind(_deleteButton).For(v => v.BindClick()).To(vm => vm.DeleteCommand);
            bindingSet.Bind(_deleteButton).For(v => v.BindVisible()).To(vm => vm.CanDelete);
            bindingSet.Bind(_statisticsButton).For(v => v.BindClick()).To(vm => vm.OpenStatisticsCommand);
            bindingSet.Bind(_statisticsButton).For(v => v.BindVisible()).To(vm => vm.IsCompetitionOwner);
        }
    }
}