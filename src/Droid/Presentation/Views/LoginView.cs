using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using Google.Android.Material.TextField;
using MvvmCross.Platforms.Android.Binding;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.ViewModels.Registration;
using PrankChat.Mobile.Droid.Services.Callback;
using PrankChat.Mobile.Droid.Services.Callbacks;
using PrankChat.Mobile.Droid.Presentation.Views.Base;
using PrankChat.Mobile.Droid.Presenters.Attributes;

namespace PrankChat.Mobile.Droid.Presentation.Views
{
    [ClearStackActivityPresentation]
    [Activity(
        ScreenOrientation = ScreenOrientation.Portrait,
        Theme = "@style/Theme.PrankChat.Base")]
    public class LoginView : BaseView<LoginViewModel>
    {
        private TextInputEditText _emailEditText;
        private TextInputEditText _passwordEditText;
        private TextView _resetPasswordTextView;
        private Button _loginButton;
        private Button _demoButton;
        private ImageButton _vkLoginButton;
        private ImageButton _okLoginButton;
        private ImageButton _fbLoginButton;
        private ImageButton _gmailLoginButton;
        private TextView _createAccountTextView;
        private ProgressBar _progressBar;

        private bool _hasOldEmailTextSpace;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle, Resource.Layout.activity_login);

            Window.SetBackgroundDrawableResource(Resource.Drawable.gradient_background);
        }

        protected override void SetViewProperties()
        {
            base.SetViewProperties();

            var forgotPasswordButton = FindViewById<TextView>(Resource.Id.reset_password_button);
            forgotPasswordButton.PaintFlags |= PaintFlags.UnderlineText;

            var createAccountButton = FindViewById<TextView>(Resource.Id.create_account_button);
            createAccountButton.PaintFlags |= PaintFlags.UnderlineText;

            _emailEditText = FindViewById<TextInputEditText>(Resource.Id.email_text_input_text);
            _passwordEditText = FindViewById<TextInputEditText>(Resource.Id.password_text_input_text);
            _resetPasswordTextView = FindViewById<TextView>(Resource.Id.reset_password_button);
            _loginButton = FindViewById<Button>(Resource.Id.login_button);
            _demoButton = FindViewById<Button>(Resource.Id.demo_button);
            _vkLoginButton = FindViewById<ImageButton>(Resource.Id.vk_login_button);
            _okLoginButton = FindViewById<ImageButton>(Resource.Id.ok_login_button);
            _fbLoginButton = FindViewById<ImageButton>(Resource.Id.fb_login_button);
            _gmailLoginButton = FindViewById<ImageButton>(Resource.Id.gmail_login_button);
            _createAccountTextView = FindViewById<TextView>(Resource.Id.create_account_button);
            _progressBar = FindViewById<ProgressBar>(Resource.Id.progressBar);
        }

        protected override void Bind()
        {
            base.Bind();

            using var bindingSet = CreateBindingSet();

            bindingSet.Bind(_emailEditText).For(v => v.Text).To(vm => vm.EmailText);
            bindingSet.Bind(_passwordEditText).For(v => v.Text).To(vm => vm.PasswordText);
            bindingSet.Bind(_resetPasswordTextView).For(v => v.BindClick()).To(vm => vm.ResetPasswordCommand);
            bindingSet.Bind(_createAccountTextView).For(v => v.BindClick()).To(vm => vm.RegistrationCommand);
            bindingSet.Bind(_demoButton).For(v => v.BindClick()).To(vm => vm.ShowDemoModeCommand);
            bindingSet.Bind(_progressBar).For(v => v.BindVisible()).To(vm => vm.IsBusy);
            bindingSet.Bind(_loginButton).For(v => v.BindClick()).To(vm => vm.LoginCommand)
                      .CommandParameter(LoginType.UsernameAndPassword);
            bindingSet.Bind(_vkLoginButton).For(v => v.BindClick()).To(vm => vm.LoginCommand)
                      .CommandParameter(LoginType.Vk);
            bindingSet.Bind(_okLoginButton).For(v => v.BindClick()).To(vm => vm.LoginCommand)
                      .CommandParameter(LoginType.Ok);
            bindingSet.Bind(_fbLoginButton).For(v => v.BindClick()).To(vm => vm.LoginCommand)
                      .CommandParameter(LoginType.Facebook);
            bindingSet.Bind(_gmailLoginButton).For(v => v.BindClick()).To(vm => vm.LoginCommand)
                      .CommandParameter(LoginType.Gmail);
        }

        protected override void Subscription()
        {
            _emailEditText.BeforeTextChanged += EmailEditTextBeforeTextChanged;
            _emailEditText.TextChanged += EmailEditTextTextChanged;
        }

        protected override void Unsubscription()
        {
            _emailEditText.BeforeTextChanged -= EmailEditTextBeforeTextChanged;
            _emailEditText.TextChanged -= EmailEditTextTextChanged;
        }

        protected override async void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (FacebookCallback.Instance.OnActivityResult(requestCode, (int)resultCode, data))
            {
                return;
            }

            await VkontakteCallback.Instance.OnActivityResultAsync(requestCode, resultCode, data);
        }

        private void EmailEditTextBeforeTextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            _hasOldEmailTextSpace = _emailEditText?.Text?.Contains(" ") ?? false;
        }

        private void EmailEditTextTextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            if (!_hasOldEmailTextSpace)
            {
                return;
            }

            _emailEditText.SetSelection(_emailEditText.Text.Length);
        }
    }
}