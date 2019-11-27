using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using PrankChat.Mobile.Droid.Presentation.Views.Base;

namespace PrankChat.Mobile.Droid.Presentation.Views
{
    [MvxActivityPresentation]
    [Activity]
    public class LoginView : BaseView<LoginViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle, Resource.Layout.login_layout);
            var forgotPasswordButton = FindViewById<TextView>(Resource.Id.reset_password_button);
            forgotPasswordButton.PaintFlags = forgotPasswordButton.PaintFlags | PaintFlags.UnderlineText;
            var createAccountButton = FindViewById<TextView>(Resource.Id.create_account_button);
            createAccountButton.PaintFlags = forgotPasswordButton.PaintFlags | PaintFlags.UnderlineText;
        }

        protected override void Subscription()
        {
        }

        protected override void Unsubscription()
        {
        }
    }
}
