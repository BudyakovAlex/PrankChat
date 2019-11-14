using System;
using MvvmCross.Binding.BindingContext;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Registration
{
    public partial class RegistrationSecondStepView : BaseView<RegistrationSecondStepViewModel>
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetupBindings();
        }

        private void SetupBindings()
        {
            var set = this.CreateBindingSet<RegistrationSecondStepView, RegistrationSecondStepViewModel>();
            set.Bind(nextStepButton).To(vm => vm.ShowThirdStepCommand);
            set.Apply();
        }
    }
}

