using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Registration
{
    public class RegistrationViewModel : BaseViewModel
    {
        private readonly IDialogService _dialogService;

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public MvxAsyncCommand ShowSecondStepCommand => new MvxAsyncCommand(OnShowSecondStepAsync);

        public RegistrationViewModel(INavigationService navigationService, IDialogService dialogService) : base(navigationService)
        {
            _dialogService = dialogService;
        }

        private async Task OnShowSecondStepAsync()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                _dialogService.ShowToast("Email can not be empty!");
                return;
            }

            await NavigationService.ShowRegistrationSecondStepView(new RegistrationNavigationParameter(Email));
        }
    }
}
