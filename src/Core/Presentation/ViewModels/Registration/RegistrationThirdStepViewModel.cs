using MvvmCross.Commands;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Registration
{
    public class RegistrationThirdStepViewModel : BasePageViewModel
    {
        public RegistrationThirdStepViewModel()
        {
            FinishRegistrationCommand = new MvxAsyncCommand(() => NavigationService.ShowMainView());
        }

        public MvxAsyncCommand FinishRegistrationCommand { get; }
    }
}
