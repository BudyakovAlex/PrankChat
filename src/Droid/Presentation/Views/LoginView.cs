using Android.App;
using Android.OS;
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
        }

        protected override void Subscription()
        {
        }

        protected override void Unsubscription()
        {
        }
    }
}
