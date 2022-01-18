using System.Collections.Generic;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Binding.Views.Gestures;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Common;
using PrankChat.Mobile.Core.Converters;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.ViewModels.Competitions;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Views.Competition
{
    [MvxModalPresentation(WrapInNavigationController = true)]
    public partial class CreateCompetitionView : BaseViewController<CreateCompetitionViewModel>
    {
        private Dictionary<UITextField, string> _dictionaryEndOfTextFields;
        private Dictionary<UITextField, UITextPosition> _dictionaryPositionOfTextFields = new Dictionary<UITextField, UITextPosition>();

        public string ContestDescription
        {
            set
            {
                if (value is null)
                {
                    return;
                }

                DescriptionPlaceholdereLabel.Hidden = value.Length > 0;
                DescriptionTopPlacehodlerLabel.Hidden = !DescriptionPlaceholdereLabel.Hidden;
            }
        }

        protected override void SetupControls()
        {
            base.SetupControls();

            _dictionaryEndOfTextFields = new Dictionary<UITextField, string>()
            {
                { ParticipationFeeTextField, Resources.Currency },
                { PrizeFundTextField, Resources.Currency },
                { PerrcentFromContribution, Resources.Percent }
            };

            DescriptionTextView.TextContainer.MaximumNumberOfLines = 100;
            DescriptionContainerView.Layer.BorderColor = Theme.Color.TextFieldDarkBorder.CGColor;
            DescriptionContainerView.Layer.BorderWidth = 1f;
            DescriptionContainerView.Layer.CornerRadius = 3f;
            DescriptionPlaceholdereLabel.SetSmallSubtitleStyle(Resources.OrderDescription, 14);
            DescriptionTopPlacehodlerLabel.SetSmallSubtitleStyle(Resources.OrderDescription);
            DescriptionTopPlacehodlerLabel.Hidden = true;
            DescriptionTextView.SetTitleStyle(size: 14);
            DescriptionTextView.ContentInset = UIEdgeInsets.Zero;
            DescriptionTextView.AddGestureRecognizer(new UITapGestureRecognizer(() => DescriptionTextView.BecomeFirstResponder()));
            DescriptionTextView.ShouldChangeText = (textField, range, replacementString) =>
            {
                var newLength = textField.Text.Length + replacementString.Length - range.Length;
                return newLength <= Constants.Orders.DescriptionMaxLength;
            };

            Title = Resources.CreateContest;
            NameTextField.SetDarkStyle(Resources.CompetitionName);
            DescriptionPlaceholdereLabel.Text =
                DescriptionTopPlacehodlerLabel.Text = Resources.Description;
            CollectRequestsFromTextField.SetDarkStyle(Resources.CollectionBidsFrom);
            CollectOfRequestsToTextField.SetDarkStyle(Resources.CollectionBidsFor);
            VotingFromTextField.SetDarkStyle(Resources.VotingFrom);
            VotingToTextField.SetDarkStyle(Resources.VotingFor);
            PrizeFundTextField.SetDarkStyle(Resources.PrizePool, rightPadding: 14);
            ParticipationFeeTextField.SetDarkStyle(Resources.ParticipationFee, rightPadding: 14);
            PerrcentFromContribution.SetDarkStyle(Resources.PercentageContributionPrizePool, rightPadding: 14);
            SecretContestLabel.Text = Resources.SecretContest;
            SettingTableofParticipantLabel.Text = Resources.ConfiguringTheParticipantTable;
            CreateContestButton.SetDarkStyle(Resources.CreateContest);

            ParticipationFeeTextField.TextAlignment =
                PerrcentFromContribution.TextAlignment =
                PrizeFundTextField.TextAlignment = UITextAlignment.Right;
        }

        protected override void Bind()
        {
            using var bindingSet = CreateBindingSet();

            bindingSet.Bind(PrizeFundTextField).For(v => v.Text).To(vm => vm.PrizePool)
                .WithConversion<PriceConverter>();
            bindingSet.Bind(ParticipationFeeTextField).For(v => v.Text).To(vm => vm.ParticipationFee)
                .WithConversion<PriceConverter>();
            bindingSet.Bind(PerrcentFromContribution).For(v => v.Text).To(vm => vm.PercentParticipationFee)
                .WithConversion<PercentConverter>();
            bindingSet.Bind(CollectRequestsFromTextField).For(v => v.Text).To(vm => vm.CollectionBidsFrom)
                .WithConversion<StringFormatValueConverter>(Constants.Formats.DateTimeFormat);
            bindingSet.Bind(CollectRequestsFromTextField).For(v => v.BindTap()).To(vm => vm.SelectPeriodCollectionBidsFromCommand);
            bindingSet.Bind(CollectOfRequestsToTextField).For(v => v.Text).To(vm => vm.CollectionBidsTo)
                .WithConversion<StringFormatValueConverter>(Constants.Formats.DateTimeFormat);
            bindingSet.Bind(CollectOfRequestsToTextField).For(v => v.BindTap()).To(vm => vm.SelectPeriodCollectionBidsToCommand);
            bindingSet.Bind(VotingFromTextField).For(v => v.Text).To(vm => vm.CollectionBidsTo)
                .WithConversion<StringFormatValueConverter>(Constants.Formats.DateTimeFormat);
            bindingSet.Bind(VotingFromTextField).For(v => v.BindTap()).To(vm => vm.SelectPeriodCollectionBidsToCommand);
            bindingSet.Bind(VotingToTextField).For(v => v.Text).To(vm => vm.VotingTo)
                .WithConversion<StringFormatValueConverter>(Constants.Formats.DateTimeFormat);
            bindingSet.Bind(VotingToTextField).For(v => v.BindTap()).To(vm => vm.SelectPeriodVotingToCommand);
            bindingSet.Bind(NameTextField).For(v => v.Text).To(vm => vm.Name).TwoWay();
            bindingSet.Bind(DescriptionTextView).For(v => v.Text).To(vm => vm.Description).TwoWay();
            bindingSet.Bind(this).For(nameof(ContestDescription)).To(vm => vm.Description);
            bindingSet.Bind(CreateContestButton.Tap()).For(v => v.Command).To(vm => vm.CreateCommand);
            bindingSet.Bind(SecretContestButton).For(v => v.IsChecked).To(vm => vm.IsExecutorHidden).TwoWay();
            bindingSet.Bind(InfoImageView.Tap()).For(v => v.Command).To(vm => vm.ShowWalkthrouthSecretCommand);
            bindingSet.Bind(SettingTableOfParticipantView.Tap()).For(v => v.Command).To(vm => vm.ShowSettingTableParticipantsCommand);
        }

        protected override void RegisterKeyboardDismissResponders(List<UIView> views)
        {
            views.Add(ContentScrollView);
            views.Add(ContentStackView);
            views.Add(DescriptionTextView);

            base.RegisterKeyboardDismissResponders(views);
        }

        protected override void RegisterKeyboardDismissTextFields(List<UIView> viewList)
        {
            viewList.Add(NameTextField);
            viewList.Add(DescriptionTextView);
            viewList.Add(PrizeFundTextField);
            viewList.Add(ParticipationFeeTextField);
            viewList.Add(PerrcentFromContribution);

            DescriptionTextView.ScrollEnabled = false;

            base.RegisterKeyboardDismissTextFields(viewList);
        }

        protected override void Subscription()
        {
            base.Subscription();

            PrizeFundTextField.EditingChanged += OnRightTextAlignmentTextFieldEditingChanged;
            ParticipationFeeTextField.EditingChanged += OnRightTextAlignmentTextFieldEditingChanged;
            PerrcentFromContribution.EditingChanged += OnRightTextAlignmentTextFieldEditingChanged;
        }

        protected override void Unsubscription()
        {
            base.Unsubscription();

            PrizeFundTextField.EditingChanged -= OnRightTextAlignmentTextFieldEditingChanged;
            ParticipationFeeTextField.EditingChanged -= OnRightTextAlignmentTextFieldEditingChanged;
            PerrcentFromContribution.EditingChanged -= OnRightTextAlignmentTextFieldEditingChanged;
        }

        private void OnRightTextAlignmentTextFieldEditingChanged(object sender, System.EventArgs e)
        {
            if (!(sender is UITextField textField))
            {
                return;
            }

            var text = textField.Text;
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            if (!_dictionaryEndOfTextFields.TryGetValue(textField, out var endOfTextField))
            {
                return;
            }

            if (!text.EndsWith(endOfTextField))
            {
                return;
            }

            var position = textField.GetPosition(textField.EndOfDocument, -(endOfTextField.Length + 1));
            _dictionaryPositionOfTextFields.TryGetValue(textField, out var previousPosition);
            if (previousPosition == position)
            {
                return;
            }

            _dictionaryPositionOfTextFields.TryAdd(textField, position);
            textField.SelectedTextRange = textField.GetTextRange(position, position);
        }
    }
}

