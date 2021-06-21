﻿using Android.Runtime;
using Android.Views;
using Google.Android.Material.Button;
using MvvmCross.Platforms.Android.Binding;
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
        private MaterialButton _registrationButton;

        public RegistrationThirdStepView() : base(Resource.Layout.fragment_registration_third_step)
        {
        }

        protected override bool HasBackButton => false;

        protected override string TitleActionBar => Core.Presentation.Localization.Resources.RegistrationView_StepThree_Title;

        protected override void SetViewProperties(View view)
        {
            base.SetViewProperties(view);

            _registrationButton = view.FindViewById<MaterialButton>(Resource.Id.registration_button);
        }

        protected override void Bind()
        {
            base.Bind();

            using var bindingSet = CreateBindingSet();

            bindingSet.Bind(_registrationButton).For(v => v.BindClick()).To(vm => vm.FinishRegistrationCommand);
        }
    }
}
