using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using PrankChat.Mobile.Droid.Presentation.Views.Base;

namespace PrankChat.Mobile.Droid.Presentation.Views.Registration
{
    [MvxFragmentPresentation(
        Tag = nameof(RegistrationSecondStepView),
        ActivityHostViewModelType = typeof(RegistrationViewModel),
        FragmentContentId = Resource.Id.container_layout,
        AddToBackStack = true)]
    [Register(nameof(RegistrationSecondStepView))]
    public class RegistrationSecondStepView : BaseFragment<RegistrationSecondStepViewModel>
    {
        protected override bool HasBackButton => true;

        protected override string TitleActionBar => Core.Presentation.Localization.Resources.RegistrationView_StepTwo_Title;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState, Resource.Layout.registration_second_step_layout);
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
