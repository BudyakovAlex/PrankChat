using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using PrankChat.Mobile.Droid.ApplicationServices.Callback;
using PrankChat.Mobile.Droid.ApplicationServices.Callbacks;
using PrankChat.Mobile.Droid.Presentation.Views.Base;
using PrankChat.Mobile.Droid.Presenters.Attributes;

namespace PrankChat.Mobile.Droid.Presentation.Views
{
    [ClearStackActivityPresentation]
    [Activity(NoHistory = true)]
    public class LoginView : BaseView<LoginViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle, Resource.Layout.activity_login);

            var forgotPasswordButton = FindViewById<TextView>(Resource.Id.reset_password_button);
            forgotPasswordButton.PaintFlags = forgotPasswordButton.PaintFlags | PaintFlags.UnderlineText;

            var createAccountButton = FindViewById<TextView>(Resource.Id.create_account_button);
            createAccountButton.PaintFlags = createAccountButton.PaintFlags | PaintFlags.UnderlineText;
        }

        protected override void Subscription()
        {
        }

        protected override void Unsubscription()
        {
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            FacebookCallback.Instance.OnActivityResult(requestCode, (int)resultCode, data);
            VkontakteCallback.Instance.OnActivityResultAsync(requestCode, resultCode, data);
        }
    }
}