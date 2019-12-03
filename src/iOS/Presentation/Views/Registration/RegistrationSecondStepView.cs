using System;
using MvvmCross.Binding.BindingContext;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Registration
{
    public partial class RegistrationSecondStepView : BaseTransparentBarView<RegistrationSecondStepViewModel>
    {
		protected override void SetupBinding()
		{
			var set = this.CreateBindingSet<RegistrationSecondStepView, RegistrationSecondStepViewModel>();
			set.Bind(nextStepButton).To(vm => vm.ShowThirdStepCommand);
			set.Apply();
		}

		protected override void SetupControls()
		{
		}
    }
}

