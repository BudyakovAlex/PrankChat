using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using Google.Android.Material.TextField;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using PrankChat.Mobile.Droid.ApplicationServices.Callback;
using PrankChat.Mobile.Droid.ApplicationServices.Callbacks;
using PrankChat.Mobile.Droid.Presentation.Views.Base;
using PrankChat.Mobile.Droid.Presenters.Attributes;

namespace PrankChat.Mobile.Droid.Presentation.Views
{
    [ClearStackActivityPresentation]
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class LoginView : BaseView<LoginViewModel>
    {
        private TextInputEditText _emailEditText;
        private bool _hasOldEmailTextSpace;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle, Resource.Layout.activity_login);

            Window.SetBackgroundDrawableResource(Resource.Drawable.gradient_background);

            var forgotPasswordButton = FindViewById<TextView>(Resource.Id.reset_password_button);
            forgotPasswordButton.PaintFlags = forgotPasswordButton.PaintFlags | PaintFlags.UnderlineText;

            var createAccountButton = FindViewById<TextView>(Resource.Id.create_account_button);
            createAccountButton.PaintFlags = createAccountButton.PaintFlags | PaintFlags.UnderlineText;

            _emailEditText = FindViewById<TextInputEditText>(Resource.Id.email_text_input_text);
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
            if (_hasOldEmailTextSpace)
                _emailEditText.SetSelection(_emailEditText.Text.Length);
        }
    }
}