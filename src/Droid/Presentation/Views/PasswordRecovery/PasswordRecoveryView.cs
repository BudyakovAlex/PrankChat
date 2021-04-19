using Android.Runtime;
using MvvmCross.Platforms.Android.Presenters.Attributes;
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
        public PasswordRecoveryView() : base(Resource.Layout.fragment_password_recovery)
        {
        }

        protected override bool HasBackButton => true;
	}
}
