using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.Core.Presentation.ViewModels.PasswordRecovery;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using PrankChat.Mobile.Droid.Presentation.Views.Base;

namespace PrankChat.Mobile.Droid.Presentation.Views.PasswordRecovery
{
    [MvxFragmentPresentation(
        Tag = nameof(PasswordRecoveryView),
        ActivityHostViewModelType = typeof(LoginViewModel),
        FragmentContentId = Resource.Id.container_layout,
        AddToBackStack = true)]
    [Register(nameof(PasswordRecoveryView))]
    public class PasswordRecoveryView : BaseFragment<PasswordRecoveryViewModel>
    {
        protected override bool HasBackButton => true;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState, Resource.Layout.fragment_password_recovery);
            return view;
        }

        protected override void Subscription()
		{
		}

		protected override void Unsubscription()
		{
		}
	}
}
