using System;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Registration
{
    public class RegistrationViewModel : BaseViewModel
    {
        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public MvxAsyncCommand ShowSecondStepCommand
        {
            get
            {
                return new MvxAsyncCommand(() => NavigationService.ShowRegistrationSecondStepView(new RegistrationNavigationParameter(Email)));
            }
        }

        public RegistrationViewModel(INavigationService navigationService) : base(navigationService)
        {
        }
    }
}
