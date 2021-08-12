using Android.Runtime;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.ViewModels.PasswordRecovery;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using PrankChat.Mobile.Droid.Presentation.Views.Base;

namespace PrankChat.Mobile.Droid.Presentation.Views.PasswordRecovery
{
    [MvxFragmentPresentation(
        Tag = nameof(FinishPasswordRecoveryView),
        ActivityHostViewModelType = typeof(LoginViewModel),
        FragmentContentId = Resource.Id.container_layout,
        AddToBackStack = true)]
    [Register(nameof(FinishPasswordRecoveryView))]
    public class FinishPasswordRecoveryView : BaseFragment<FinishPasswordRecoveryViewModel>
    {
        public FinishPasswordRecoveryView() : base(Resource.Layout.fragment_finish_password_recovery)
        {
        }

        protected override bool HasBackButton => false;

        protected override string TitleActionBar => Core.Localization.Resources.Password_Recovery_View_Title;
    }
}
