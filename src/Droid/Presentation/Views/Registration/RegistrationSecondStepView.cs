using Android.App;
using Android.OS;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using PrankChat.Mobile.Droid.Presentation.Views.Base;

namespace PrankChat.Mobile.Droid.Presentation.Views.Registration
{
    [MvxActivityPresentation]
    [Activity]
    public class RegistrationSecondStepView : BaseView<RegistrationSecondStepViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.registration_second_step_layout);
        }

		protected override void Subscription()
		{
		}

		protected override void Unsubscription()
		{
		}
	}
}
