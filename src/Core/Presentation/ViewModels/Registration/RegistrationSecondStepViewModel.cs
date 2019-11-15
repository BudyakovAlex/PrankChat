using System;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Registration
{
    public class RegistrationSecondStepViewModel : BaseViewModel
    {
        public MvxAsyncCommand ShowThirdStepCommand
        {
            get
            {
                return new MvxAsyncCommand(() => NavigationService.ShowRegistrationThirdStepView());
            }
        }

        public RegistrationSecondStepViewModel(INavigationService navigationService) : base(navigationService)
        {
        }
    }
}
