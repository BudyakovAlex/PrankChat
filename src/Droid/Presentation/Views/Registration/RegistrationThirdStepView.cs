using Android.Runtime;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using PrankChat.Mobile.Droid.Presentation.Views.Base;

namespace PrankChat.Mobile.Droid.Presentation.Views.Registration
{
    [MvxFragmentPresentation(
        Tag = nameof(RegistrationThirdStepView),
        ActivityHostViewModelType = typeof(RegistrationViewModel),
        FragmentContentId = Resource.Id.container_layout,
        AddToBackStack = true)]
    [Register(nameof(RegistrationThirdStepView))]
    public class RegistrationThirdStepView : BaseFragment<RegistrationThirdStepViewModel>
    {
        public RegistrationThirdStepView() : base(Resource.Layout.fragment_registration_third_step)
        {
        }

        protected override bool HasBackButton => false;

        protected override string TitleActionBar => Core.Presentation.Localization.Resources.RegistrationView_StepThree_Title;
    }
}
