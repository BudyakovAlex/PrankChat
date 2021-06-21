using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Views.Base;

namespace PrankChat.Mobile.iOS.Presentation.Views.Registration
{
    [MvxModalPresentation(WrapInNavigationController = false)]
    public partial class RegistrationThirdStepView : BaseTransparentBarView<RegistrationThirdStepViewModel>
    {
		protected override void SetupBinding()
		{
			var set = this.CreateBindingSet<RegistrationThirdStepView, RegistrationThirdStepViewModel>();
			set.Bind(finishRegistrationButton).To(vm => vm.FinishRegistrationCommand);
			set.Apply();
		}

		protected override void SetupControls()
		{
            Title = Resources.RegistrationView_StepThree_Title;

            congratsTitleLabel.Text = Resources.RegistrationView_Congrats_Title;
            congratsTitleLabel.TextColor = Theme.Color.White;
            congratsTitleLabel.Font = Theme.Font.RegularFontOfSize(14);

            confirmationDescriptionLabel.Text = Resources.RegistrationView_Confirmation_Label;
            confirmationDescriptionLabel.TextColor = Theme.Color.White;
            confirmationDescriptionLabel.Font = Theme.Font.RegularFontOfSize(14);

            finishRegistrationButton.SetLightStyle(Resources.RegistrationView_GoToFeed_Button);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.NavigationItem.LeftBarButtonItem = null;
        }
    }
}

