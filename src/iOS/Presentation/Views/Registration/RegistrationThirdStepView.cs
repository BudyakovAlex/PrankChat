using System;
using MvvmCross.Binding.BindingContext;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Registration
{
    public partial class RegistrationThirdStepView : BaseView<RegistrationThirdStepViewModel>
    {
		protected override void SetupBinding()
		{
			var set = this.CreateBindingSet<RegistrationThirdStepView, RegistrationThirdStepViewModel>();
			set.Bind(finishRegistrationButton).To(vm => vm.FinishRegistrationCommand);
			set.Apply();
		}

		protected override void SetupControls()
		{
		}
    }
}

