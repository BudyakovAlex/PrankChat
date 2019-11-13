using System;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Registration
{
    public class RegistrationViewModel : BaseViewModel
    {
        public MvxAsyncCommand ShowSecondStepCommand
        {
            get
            {
                return new MvxAsyncCommand(() => NavigationService.ShowRegistrationSecondStepView());
            }
        }

        public RegistrationViewModel(INavigationService navigationService) : base(navigationService)
        {
        }
    }
}
