﻿using Android.Runtime;
using Android.Views;
using Android.Widget;
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
        public RegistrationSecondStepView() : base(Resource.Layout.fragment_registration_second_step)
        {
        }

        protected override bool HasBackButton => true;

        protected override string TitleActionBar => Core.Presentation.Localization.Resources.RegistrationView_StepTwo_Title;

        protected override void SetViewProperties(View view)
        {
            base.SetViewProperties(view);

            var privacyLinkTextView = view.FindViewById<TextView>(Resource.Id.terms_link_text_view);
            privacyLinkTextView.PaintFlags = Android.Graphics.PaintFlags.UnderlineText;
        }
    }
}