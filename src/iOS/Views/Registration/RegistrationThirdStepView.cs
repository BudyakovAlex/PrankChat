using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.ViewModels.Registration;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Views.Base;

namespace PrankChat.Mobile.iOS.Views.Registration
{
    [MvxModalPresentation(WrapInNavigationController = false)]
    public partial class RegistrationThirdStepView : BaseTransparentBarViewController<RegistrationThirdStepViewModel>
    {
		protected override void Bind()
		{
			using var bindingSet = this.CreateBindingSet<RegistrationThirdStepView, RegistrationThirdStepViewModel>();
			bindingSet.Bind(finishRegistrationButton).To(vm => vm.FinishRegistrationCommand);
		}

		protected override void SetupControls()
		{
            Title = Resources.StepThree;

            congratsTitleLabel.Text = Resources.Congratulations;
            congratsTitleLabel.TextColor = Theme.Color.White;
            congratsTitleLabel.Font = Theme.Font.RegularFontOfSize(14);

            confirmationDescriptionLabel.Text = Resources.ProfileEmailConfirmationSent;
            confirmationDescriptionLabel.TextColor = Theme.Color.White;
            confirmationDescriptionLabel.Font = Theme.Font.RegularFontOfSize(14);

            finishRegistrationButton.SetLightStyle(Resources.GoToFeed);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.NavigationItem.LeftBarButtonItem = null;
        }
    }
}

