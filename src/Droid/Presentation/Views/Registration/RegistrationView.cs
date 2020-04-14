using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using PrankChat.Mobile.Droid.Presentation.Views.Base;

namespace PrankChat.Mobile.Droid.Presentation.Views.Registration
{
    [MvxActivityPresentation]
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class RegistrationView : BaseView<RegistrationViewModel>
    {
        protected override bool HasBackButton => true;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle, Resource.Layout.activity_registration);

            var textViewLogin = this.FindViewById<TextView>(Resource.Id.go_to_login_text_view);
            textViewLogin.PaintFlags |= Android.Graphics.PaintFlags.UnderlineText;
        }

		protected override void Subscription()
		{
		}

		protected override void Unsubscription()
		{
		}
	}
}
