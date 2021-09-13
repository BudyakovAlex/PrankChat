using Android.Runtime;
using Android.Views;
using Android.Widget;
using Google.Android.Material.Button;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.ViewModels.Registration;
using PrankChat.Mobile.Droid.Views.Base;

namespace PrankChat.Mobile.Droid.Views.Registration
{
    [MvxFragmentPresentation(
        Tag = nameof(RegistrationSecondStepView),
        ActivityHostViewModelType = typeof(RegistrationViewModel),
        FragmentContentId = Resource.Id.container_layout,
        AddToBackStack = true)]
    [Register(nameof(RegistrationSecondStepView))]
    public class RegistrationSecondStepView : BaseFragment<RegistrationSecondStepViewModel>
    {
        private ProgressBar _progressBar;
        private MaterialButton _registrationButton;
        private CheckBox _adultCheckbox;
        private CheckBox _termsCheckbox;
        private TextView _birthdayTextView;
        private TextView _termsLinkTextView;
        private EditText _loginEditText;
        private EditText _nameEditText;
        private EditText _passwordEditText;
        private EditText _repeatPasswordEditText;
        private View _selectBirthdayContainerView;

        public RegistrationSecondStepView() : base(Resource.Layout.fragment_registration_second_step)
        {
        }

        protected override bool HasBackButton => true;

        protected override string TitleActionBar => Core.Localization.Resources.RegistrationView_StepTwo_Title;

        protected override void SetViewProperties(View view)
        {
            base.SetViewProperties(view);

            var privacyLinkTextView = view.FindViewById<TextView>(Resource.Id.terms_link_text_view);
            privacyLinkTextView.PaintFlags = Android.Graphics.PaintFlags.UnderlineText;

            _progressBar = view.FindViewById<ProgressBar>(Resource.Id.progressBar);
            _registrationButton = view.FindViewById<MaterialButton>(Resource.Id.registration_button);
            _adultCheckbox = view.FindViewById<CheckBox>(Resource.Id.adult_check_box);
            _termsCheckbox = view.FindViewById<CheckBox>(Resource.Id.terms_check_box);
            _birthdayTextView = view.FindViewById<TextView>(Resource.Id.birthday_text_view);
            _termsLinkTextView = view.FindViewById<TextView>(Resource.Id.terms_link_text_view);
            _loginEditText = view.FindViewById<EditText>(Resource.Id.login_edit_text);
            _nameEditText = view.FindViewById<EditText>(Resource.Id.name_edit_text);
            _passwordEditText = view.FindViewById<EditText>(Resource.Id.password_edit_text);
            _repeatPasswordEditText = view.FindViewById<EditText>(Resource.Id.repeat_password_edit_text);
            _selectBirthdayContainerView = view.FindViewById<View>(Resource.Id.select_birthday_container_view);
        }

        protected override void Bind()
        {
            base.Bind();

            using var bindingSet = CreateBindingSet();

            bindingSet.Bind(_adultCheckbox).For(v => v.Checked).To(vm => vm.IsAdultChecked);
            bindingSet.Bind(_termsCheckbox).For(v => v.Checked).To(vm => vm.IsPolicyChecked);
            bindingSet.Bind(_repeatPasswordEditText).For(v => v.Text).To(vm => vm.RepeatedPassword);
            bindingSet.Bind(_passwordEditText).For(v => v.Text).To(vm => vm.Password);
            bindingSet.Bind(_loginEditText).For(v => v.Text).To(vm => vm.Login);
            bindingSet.Bind(_nameEditText).For(v => v.Text).To(vm => vm.Name);
            bindingSet.Bind(_selectBirthdayContainerView).For(v => v.BindClick()).To(vm => vm.SelectBirthdayCommand);
            bindingSet.Bind(_birthdayTextView).For(v => v.Text).To(vm => vm.BirthdayText);
            bindingSet.Bind(_termsLinkTextView).For(v => v.BindClick()).To(vm => vm.ShowTermsAndRulesCommand);
            bindingSet.Bind(_registrationButton).For(v => v.BindClick()).To(vm => vm.UserRegistrationCommand);
            bindingSet.Bind(_progressBar).For(v => v.BindVisible()).To(vm => vm.IsBusy);
        }
    }
}