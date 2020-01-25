using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.Exceptions;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Registration
{
    public class RegistrationViewModel : BaseViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly IErrorHandleService _errorHandleService;

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public MvxAsyncCommand ShowSecondStepCommand => new MvxAsyncCommand(OnShowSecondStepAsync);

        public RegistrationViewModel(INavigationService navigationService,
                                     IDialogService dialogService,
                                     IErrorHandleService errorHandleService) : base(navigationService)
        {
            _dialogService = dialogService;
            _errorHandleService = errorHandleService;
        }

        private Task OnShowSecondStepAsync()
        {
            if (!CheckValidation())
                return Task.CompletedTask;

            return NavigationService.ShowRegistrationSecondStepView(new RegistrationNavigationParameter(Email));
        }

        private bool CheckValidation()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                _errorHandleService.HandleException(new UserVisibleException("Имя не может быть пустым."));
                return false;
            }

            if (!Email.IsValidEmail())
            {
                _errorHandleService.HandleException(new UserVisibleException("Поле Email введено не правильно."));
                return false;
            }

            return true;
        }
    }
}
