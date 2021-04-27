﻿using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using Google.Android.Material.Button;
using Google.Android.Material.TextField;
using MvvmCross.Binding.Combiners;
using MvvmCross.DroidX;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Converters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile.Cashbox;
using PrankChat.Mobile.Droid.Presentation.Views.Base;

namespace PrankChat.Mobile.Droid.Presentation.Views.Profile.Cashbox
{
    [MvxTabLayoutPresentation(
        TabLayoutResourceId = Resource.Id.cashbox_tab_layout,
        ViewPagerResourceId = Resource.Id.cashbox_pager,
        ActivityHostViewModelType = typeof(CashboxViewModel))]
    [Register(nameof(WithdrawalView))]
    public class WithdrawalView : BaseFragment<WithdrawalViewModel>
    {
        private TextInputLayout _savedCreditCardTextInputLayout;
        private TextInputLayout _creditCardTextInputLayout;
        private TextInputLayout _nameTextInputLayout;
        private TextInputLayout _surnameTextInputLayout;
        private TextInputLayout _middleNameTextInputLayout;
        private TextInputLayout _nationalityTextInputLayout;
        private TextInputLayout _passportTextInputLayout;
        private TextInputEditText _costEditText;
        private TextInputEditText _cardEditText;
        private TextInputEditText _currentCardEditText;
        private TextInputEditText _creditCardEditText;
        private TextInputEditText _nameEditText;
        private TextInputEditText _surnameEditText;
        private TextInputEditText _middleNameEditText;
        private TextInputEditText _nationalityEditText;
        private MvxSwipeRefreshLayout _swipeRefreshLayout;
        private View _attachDocumentContainerView;
        private MaterialButton _attachDocumentButton;
        private MaterialButton _withdrawalButton;
        private MaterialButton _cancelWithdrawalButton;
        private View _pendingDocumentContainerView;
        private View _withdrawalContainerView;
        private View _pendingWithdrawalContainerView;
        private View _loadingOverlayView;
        private TextView _pendingWithdrawalCostTextView;
        private TextView _pendingWithdrawalDateTextView;
        private TextView _availableAmountTextView;
        private TextInputLayout _locationTextInputLayout;
        private TextInputEditText _passportEditText;
        private TextInputEditText _locationEditText;

        public WithdrawalView() : base(Resource.Layout.withdrawal_layout)
        {
        }

        protected override void SetViewProperties(View view)
        {
            base.SetViewProperties(view);

            _pendingWithdrawalCostTextView = view.FindViewById<TextView>(Resource.Id.pending_withdrawal_cost_value);
            _pendingWithdrawalDateTextView = view.FindViewById<TextView>(Resource.Id.pending_withdrawal_date_value);
            _availableAmountTextView = view.FindViewById<TextView>(Resource.Id.withdrawal_available_amount_text);
            _availableAmountTextView.PaintFlags |= PaintFlags.UnderlineText;

            _savedCreditCardTextInputLayout = view.FindViewById<TextInputLayout>(Resource.Id.saved_credit_card_text);
            _creditCardTextInputLayout = view.FindViewById<TextInputLayout>(Resource.Id.credit_card_text);

            _nameTextInputLayout = view.FindViewById<TextInputLayout>(Resource.Id.name_text);
            _surnameTextInputLayout = view.FindViewById<TextInputLayout>(Resource.Id.surname_text);
            _middleNameTextInputLayout = view.FindViewById<TextInputLayout>(Resource.Id.middle_name_text);
            _nationalityTextInputLayout = view.FindViewById<TextInputLayout>(Resource.Id.nationality_text);
            _passportTextInputLayout = view.FindViewById<TextInputLayout>(Resource.Id.passport_text);
            _locationTextInputLayout = view.FindViewById<TextInputLayout>(Resource.Id.location_text);

            _costEditText = view.FindViewById<TextInputEditText>(Resource.Id.credit_cost_text_input_edit_text);
            _cardEditText = view.FindViewById<TextInputEditText>(Resource.Id.credit_card_text_input_edit_text);
            _currentCardEditText = view.FindViewById<TextInputEditText>(Resource.Id.current_card_edit_text);
            _creditCardEditText = view.FindViewById<TextInputEditText>(Resource.Id.credit_card_text_input_edit_text);

            _nameEditText = view.FindViewById<TextInputEditText>(Resource.Id.name_edit_text);
            _surnameEditText = view.FindViewById<TextInputEditText>(Resource.Id.surname_edit_text);
            _middleNameEditText = view.FindViewById<TextInputEditText>(Resource.Id.middle_name_edit_text);
            _nationalityEditText = view.FindViewById<TextInputEditText>(Resource.Id.nationality_edit_text);
            _passportEditText = view.FindViewById<TextInputEditText>(Resource.Id.passport_edit_text);
            _locationEditText = view.FindViewById<TextInputEditText>(Resource.Id.location_edit_text);

            _swipeRefreshLayout = view.FindViewById<MvxSwipeRefreshLayout>(Resource.Id.swipe_refresh);
            _attachDocumentContainerView = view.FindViewById<View>(Resource.Id.attach_document_container_view);
            _attachDocumentButton = view.FindViewById<MaterialButton>(Resource.Id.attach_document_button);
            _withdrawalButton = view.FindViewById<MaterialButton>(Resource.Id.withdrawal_button);
            _cancelWithdrawalButton = view.FindViewById<MaterialButton>(Resource.Id.cancel_withdrawal_button);
            _pendingDocumentContainerView = view.FindViewById<View>(Resource.Id.pending_document_container_view);
            _withdrawalContainerView = view.FindViewById<View>(Resource.Id.withdrawal_container_view);
            _pendingWithdrawalContainerView = view.FindViewById<View>(Resource.Id.pending_withdrawal_container_view);
            _loadingOverlayView = view.FindViewById<View>(Resource.Id.loading_overlay);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Context.SetTheme(Resource.Style.Theme_PrankChat_Base_Dark);
            return base.OnCreateView(inflater, container, savedInstanceState);
        }

        protected override void Bind()
        {
            base.Bind();

            using var bindingSet = CreateBindingSet();

            bindingSet.Bind(_swipeRefreshLayout).For(v => v.Refreshing).To(vm => vm.IsUpdatingData);
            bindingSet.Bind(_swipeRefreshLayout).For(v => v.RefreshCommand).To(vm => vm.UpdateDataCommand);

            bindingSet.Bind(_attachDocumentContainerView).For(v => v.BindVisible()).To(vm => vm.IsAttachDocumentAvailable);
            bindingSet.Bind(_attachDocumentButton).For(v => v.BindClick()).To(vm => vm.AttachFileCommand);

            bindingSet.Bind(_pendingDocumentContainerView).For(v => v.BindVisible()).To(vm => vm.IsDocumentPending);
            bindingSet.Bind(_withdrawalContainerView).For(v => v.BindVisible()).To(vm => vm.IsWithdrawalAvailable);

            bindingSet.Bind(_currentCardEditText).For(v => v.Text).To(vm => vm.CurrentCardNumber);
            bindingSet.Bind(_currentCardEditText).For(v => v.BindClick()).To(vm => vm.OpenCardOptionsCommand);

            bindingSet.Bind(_savedCreditCardTextInputLayout).For(v => v.BindVisible()).To(vm => vm.IsPresavedWithdrawalAvailable);
            bindingSet.Bind(_creditCardTextInputLayout).For(v => v.BindHidden()).To(vm => vm.IsPresavedWithdrawalAvailable);

            bindingSet.Bind(_nameTextInputLayout).For(v => v.BindHidden()).ByCombining(
                new MvxAndValueCombiner(),
                vm => vm.IsPresavedWithdrawalAvailable,
                vm => vm.IsUserDataSaved);

            bindingSet.Bind(_surnameTextInputLayout).For(v => v.BindHidden()).ByCombining(
                new MvxAndValueCombiner(),
                vm => vm.IsPresavedWithdrawalAvailable,
                vm => vm.IsUserDataSaved);

            bindingSet.Bind(_middleNameTextInputLayout).For(v => v.BindHidden()).To(vm => vm.IsUserDataSaved);
            bindingSet.Bind(_passportTextInputLayout).For(v => v.BindHidden()).To(vm => vm.IsUserDataSaved);
            bindingSet.Bind(_nationalityTextInputLayout).For(v => v.BindHidden()).To(vm => vm.IsUserDataSaved);
            bindingSet.Bind(_locationTextInputLayout).For(v => v.BindHidden()).To(vm => vm.IsUserDataSaved);

            bindingSet.Bind(_creditCardEditText).For(v => v.Text).To(vm => vm.CardNumber);
            bindingSet.Bind(_nameEditText).For(v => v.Text).To(vm => vm.Name);
            bindingSet.Bind(_surnameEditText).For(v => v.Text).To(vm => vm.Surname);
            bindingSet.Bind(_costEditText).For(v => v.Text).To(vm => vm.Cost)
                      .WithConversion<PriceConverter>();

            bindingSet.Bind(_middleNameEditText).For(v => v.Text).To(vm => vm.MiddleName);
            bindingSet.Bind(_passportEditText).For(v => v.Text).To(vm => vm.Passport);
            bindingSet.Bind(_locationEditText).For(v => v.Text).To(vm => vm.Location);
            bindingSet.Bind(_nationalityEditText).For(v => v.Text).To(vm => vm.Nationality);

            bindingSet.Bind(_withdrawalButton).For(v => v.BindClick()).To(vm => vm.WithdrawCommand);
            bindingSet.Bind(_availableAmountTextView).For(v => v.Text).To(vm => vm.AvailableForWithdrawal);
            bindingSet.Bind(_pendingWithdrawalContainerView).For(v => v.BindVisible()).To(vm => vm.IsWithdrawalPending);
            bindingSet.Bind(_pendingWithdrawalDateTextView).For(v => v.Text).To(vm => vm.CreateAtWithdrawal);
            bindingSet.Bind(_pendingWithdrawalCostTextView).For(v => v.Text).To(vm => vm.AmountValue);

            bindingSet.Bind(_cancelWithdrawalButton).For(v => v.BindClick()).To(vm => vm.CancelWithdrawCommand);
            bindingSet.Bind(_loadingOverlayView).For(v => v.BindVisible()).To(vm => vm.IsBusy);
        }

        protected override void Subscription()
        {
            _costEditText.TextChanged += PriceEditTextOnTextChanged;
            _cardEditText.TextChanged += CardEditTextOnTextChanged;
        }

        protected override void Unsubscription()
        {
            _costEditText.TextChanged -= PriceEditTextOnTextChanged;
            _cardEditText.TextChanged -= CardEditTextOnTextChanged;
        }

        private void PriceEditTextOnTextChanged(object sender, TextChangedEventArgs e)
        {
            var text = e.Text.ToString();
            if (text.EndsWith(Core.Presentation.Localization.Resources.Currency))
            {
                _costEditText.SetSelection(text.Length - 2);
            }
        }

        private void CardEditTextOnTextChanged(object sender, TextChangedEventArgs e)
        {
            var text = e.Text.ToString();
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            _cardEditText.SetSelection(text.Length);
        }
    }
}
