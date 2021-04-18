using Android.OS;
using Android.Runtime;
using Android.Views;
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
        protected override bool HasBackButton => false;

        protected override string TitleActionBar => Core.Presentation.Localization.Resources.RegistrationView_StepThree_Title;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState, Resource.Layout.fragment_registration_third_step);
            return view;
        }
    }
}
