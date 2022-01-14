﻿using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Text;
using Android.Widget;
using AndroidX.ConstraintLayout.Widget;
using Google.Android.Material.Button;
using Google.Android.Material.TextField;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Converters;
using PrankChat.Mobile.Core.ViewModels.Competitions;
using PrankChat.Mobile.Droid.Converters;
using PrankChat.Mobile.Droid.Extensions;
using PrankChat.Mobile.Droid.Views.Base;
using System;

namespace PrankChat.Mobile.Droid.Views.Competitions
{
    [MvxActivityPresentation]
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class CreateCompetitionView : BaseView<CreateCompetitionViewModel>
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
        private ConstraintLayout _settingsTableParticipantsConstraintLayout;

        protected override bool HasBackButton => true;

        protected override string TitleActionBar => Core.Localization.Resources.CreateContest;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle, Resource.Layout.activity_create_competition);
        }

        protected override void SetViewProperties()
        {
            _nameEditText = FindViewById<TextInputEditText>(Resource.Id.name_input_edit_text);
            _descriptionEditText = FindViewById<TextInputEditText>(Resource.Id.description_input_edit_text);
            _collectionBidsFromEditText = FindViewById<TextInputEditText>(Resource.Id.collection_bids_from_input_edit_text);
            _collectionBidsToEditText = FindViewById<TextInputEditText>(Resource.Id.collection_bids_to_input_edit_text);
            _votingFromEditText = FindViewById<TextInputEditText>(Resource.Id.voting_from_input_edit_text);
            _votingToEditText = FindViewById<TextInputEditText>(Resource.Id.voting_to_input_edit_text);
            _prizePoolEditText = FindViewById<TextInputEditText>(Resource.Id.prize_pool_input_edit_text);
            _participationFeeEditText = FindViewById<TextInputEditText>(Resource.Id.participation_fee_input_edit_text);
            _percentParticipationFeeEditText = FindViewById<TextInputEditText>(Resource.Id.percent_participation_fee_input_edit_text);
            _createContestCheckBox = FindViewById<CheckBox>(Resource.Id.create_contest_check_box);
            _descriptionImageView = FindViewById<ImageView>(Resource.Id.description_image_view);
            _createContestButton = FindViewById<MaterialButton>(Resource.Id.create_contest_button);
            _settingsTableParticipantsConstraintLayout = FindViewById<ConstraintLayout>(Resource.Id.settings_table_participants_constraint_layout);
            var settingsTableParticipantsTextView = FindViewById<TextView>(Resource.Id.settings_table_participants_text_view);

            _collectionBidsFromEditText.InputType = InputTypes.Null;
            _collectionBidsToEditText.InputType = InputTypes.Null;
            _votingFromEditText.InputType = InputTypes.Null;
            _votingToEditText.InputType = InputTypes.Null;

            settingsTableParticipantsTextView.Text = Core.Localization.Resources.ConfiguringTheParticipantTable;

            _prizePoolEditText.SetTextChangeListener((sequence) => _prizePoolEditText.MoveCursorBeforeSymbol(Core.Localization.Resources.Currency, sequence));
            _participationFeeEditText.SetTextChangeListener((sequence) => _participationFeeEditText.MoveCursorBeforeSymbol(Core.Localization.Resources.Currency, sequence));
            _percentParticipationFeeEditText.SetTextChangeListener((sequence) => _percentParticipationFeeEditText.MoveCursorBeforeSymbol(Core.Localization.Resources.Percent, sequence));
        }

        protected override void Bind()
        {
            base.Bind();

            using var bindingSet = CreateBindingSet();

            bindingSet.Bind(_nameEditText).For(v => v.Text).To(vm => vm.Name);
            bindingSet.Bind(_descriptionEditText).For(v => v.Text).To(vm => vm.Description);
            bindingSet.Bind(_collectionBidsFromEditText).For(v => v.Text).To(vm => vm.CollectionBidsFrom)
                .WithConversion<DateTimeToStringConverter>();
            bindingSet.Bind(_collectionBidsFromEditText).For(v => v.BindTouch()).To(vm => vm.SelectPeriodCollectionBidsFromCommand);
            bindingSet.Bind(_collectionBidsToEditText).For(v => v.Text).To(vm => vm.CollectionBidsTo)
                .WithConversion<DateTimeToStringConverter>();
            bindingSet.Bind(_collectionBidsToEditText).For(v => v.BindTouch()).To(vm => vm.SelectPeriodCollectionBidsToCommand);
            bindingSet.Bind(_votingFromEditText).For(v => v.Text).To(vm => vm.CollectionBidsTo)
                .WithConversion<DateTimeToStringConverter>();
            bindingSet.Bind(_votingFromEditText).For(v => v.BindTouch()).To(vm => vm.SelectPeriodCollectionBidsToCommand);
            bindingSet.Bind(_votingToEditText).For(v => v.Text).To(vm => vm.VotingTo)
                .WithConversion<DateTimeToStringConverter>();
            bindingSet.Bind(_votingToEditText).For(v => v.BindTouch()).To(vm => vm.SelectPeriodVotingToCommand);
            bindingSet.Bind(_prizePoolEditText).For(v => v.Text).To(vm => vm.PrizePool)
                .WithConversion<PriceConverter>();
            bindingSet.Bind(_participationFeeEditText).For(v => v.Text).To(vm => vm.ParticipationFee)
                .WithConversion<PriceConverter>();
            bindingSet.Bind(_percentParticipationFeeEditText).For(v => v.Text).To(vm => vm.PercentParticipationFee)
                .WithConversion<PercentConvertor>();
            bindingSet.Bind(_createContestCheckBox).For(v => v.Checked).To(vm => vm.IsExecutorHidden);
            bindingSet.Bind(_descriptionImageView).For(v => v.BindClick()).To(vm => vm.ShowWalkthrouthSecretCommand);
            bindingSet.Bind(_createContestButton).For(v => v.BindClick()).To(vm => vm.CreateCommand);
            bindingSet.Bind(_settingsTableParticipantsConstraintLayout).For(v => v.BindClick()).To(vm => vm.ShowSettingTableParticipantsCommand);
        }
    }
}