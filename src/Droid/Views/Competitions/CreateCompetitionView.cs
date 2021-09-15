using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using Google.Android.Material.Button;
using Google.Android.Material.TextField;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Common;
using PrankChat.Mobile.Core.Converters;
using PrankChat.Mobile.Core.ViewModels;
using PrankChat.Mobile.Core.ViewModels.Competition;
using PrankChat.Mobile.Droid.Converters;
using PrankChat.Mobile.Droid.Views.Base;

namespace PrankChat.Mobile.Droid.Views.Competitions
{
    [MvxTabLayoutPresentation(
        TabLayoutResourceId = Resource.Id.tabs,
        ViewPagerResourceId = Resource.Id.viewpager,
        ActivityHostViewModelType = typeof(MainViewModel))]
    [Register(nameof(CreateCompetitionView))]
    public class CreateCompetitionView : BaseTabFragment<CreateCompetitionViewModel>
    {
        private TextInputEditText _nameEditText;
        private TextInputEditText _descriptionEditText;
        private TextInputEditText _collectionBidsFromEditText;
        private TextInputEditText _collectionBidsToEditText;
        private TextInputEditText _votingFromEditText;
        private TextInputEditText _votingToEditText;
        private TextInputEditText _prizePoolEditText;
        private TextInputEditText _participationFeeEditText;
        private TextInputEditText _percentParticipationFeeEditText;
        private CheckBox _createContestCheckBox;
        private ImageView _descriptionImageView;
        private MaterialButton _createContestButton;

        public CreateCompetitionView() : base(Resource.Layout.fragment_create_competition)
        {
            HasOptionsMenu = true;
        }

        protected override void SetViewProperties(View view)
        {
            base.SetViewProperties(view);

            _nameEditText = view.FindViewById<TextInputEditText>(Resource.Id.name_input_edit_text);
            _descriptionEditText = view.FindViewById<TextInputEditText>(Resource.Id.description_input_edit_text);
            _collectionBidsFromEditText = view.FindViewById<TextInputEditText>(Resource.Id.collection_bids_from_input_edit_text);
            _collectionBidsToEditText = view.FindViewById<TextInputEditText>(Resource.Id.collection_bids_to_input_edit_text);
            _votingFromEditText = view.FindViewById<TextInputEditText>(Resource.Id.voting_from_input_edit_text);
            _votingToEditText = view.FindViewById<TextInputEditText>(Resource.Id.voting_to_input_edit_text);
            _prizePoolEditText = view.FindViewById<TextInputEditText>(Resource.Id.prize_pool_input_edit_text);
            _participationFeeEditText = view.FindViewById<TextInputEditText>(Resource.Id.participation_fee_input_edit_text);
            _percentParticipationFeeEditText = view.FindViewById<TextInputEditText>(Resource.Id.percent_participation_fee_input_edit_text);
            _createContestCheckBox = view.FindViewById<CheckBox>(Resource.Id.create_contest_check_box);
            _descriptionImageView = view.FindViewById<ImageView>(Resource.Id.description_image_view);
            _createContestButton = view.FindViewById<MaterialButton>(Resource.Id.create_contest_button);
        }

        protected override void Bind()
        {
            base.Bind();

            using var bindingSet = CreateBindingSet();

            bindingSet.Bind(_nameEditText).For(v => v.Text).To(vm => vm.Name);
            bindingSet.Bind(_descriptionEditText).For(v => v.Text).To(vm => vm.Description);
            bindingSet.Bind(_collectionBidsFromEditText).For(v => v.Text).To(vm => vm.CollectionBidsFrom);
            bindingSet.Bind(_collectionBidsToEditText).For(v => v.Text).To(vm => vm.CollectionBidsTo);
            bindingSet.Bind(_votingFromEditText).For(v => v.Text).To(vm => vm.VotingFrom);
            bindingSet.Bind(_votingToEditText).For(v => v.Text).To(vm => vm.VotingTo);
            bindingSet.Bind(_prizePoolEditText).For(v => v.Text).To(vm => vm.PrizePool).WithConversion<PriceConverter>();
            bindingSet.Bind(_participationFeeEditText).For(v => v.Text).To(vm => vm.ParticipationFee).WithConversion<PriceConverter>();
            bindingSet.Bind(_percentParticipationFeeEditText).For(v => v.Text).To(vm => vm.PercentParticipationFee).WithConversion<PercentConvertor>();
            bindingSet.Bind(_createContestCheckBox).For(v => v.Checked).To(vm => vm.IsExecutorHidden);
            bindingSet.Bind(_descriptionImageView).For(v => v.BindClick()).To(vm => vm.ShowWalkthrouthSecretCommand);
            bindingSet.Bind(_createContestButton).For(v => v.BindClick()).To(vm => vm.CreateCommand);
        }

        protected override void Subscription()
        {
            _prizePoolEditText.TextChanged += PrizePoolEditTextOnTextChanged;
            _participationFeeEditText.TextChanged += ParticipationFeeEditTextOnTextChanged;
            _percentParticipationFeeEditText.TextChanged += PercentParticipationFeeEditTextOnTextChanged;
        }

        private void PercentParticipationFeeEditTextOnTextChanged(object sender, TextChangedEventArgs e)
        {
            var text = e.Text.ToString();
            if (text.EndsWith("%"))
            {
                _percentParticipationFeeEditText.SetSelection(text.Length - 2);
            }
        }

        private void ParticipationFeeEditTextOnTextChanged(object sender, TextChangedEventArgs e)
        {
            var text = e.Text.ToString();
            if (text.EndsWith(Core.Localization.Resources.Currency))
            {
                _participationFeeEditText.SetSelection(text.Length - 2);
            }
        }

        private void PrizePoolEditTextOnTextChanged(object sender, TextChangedEventArgs e)
        {
            var text = e.Text.ToString();
            if (text.EndsWith(Core.Localization.Resources.Currency))
            {
                _prizePoolEditText.SetSelection(text.Length - 2);
            }
        }

        protected override void Unsubscription()
        {
            _prizePoolEditText.TextChanged -= PrizePoolEditTextOnTextChanged;
            _participationFeeEditText.TextChanged -= ParticipationFeeEditTextOnTextChanged;
            _percentParticipationFeeEditText.TextChanged -= PercentParticipationFeeEditTextOnTextChanged;
        }
    }
}