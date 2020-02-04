using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.Exceptions;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
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

        public MvxAsyncCommand ShowSecondStepCommand => new MvxAsyncCommand(OnShowSecondStepAsync);

        public RegistrationViewModel(INavigationService navigationService,
                                     IDialogService dialogService,
                                     IApiService apiService,
                                     IErrorHandleService errorHandleService) : base(navigationService, errorHandleService, apiService, dialogService)
        {
        }

        private Task OnShowSecondStepAsync()
        {
            if (!CheckValidation())
                return Task.CompletedTask;

            return NavigationService.ShowRegistrationSecondStepView(Email);
        }

        private bool CheckValidation()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                ErrorHandleService.HandleException(new UserVisibleException("Имя не может быть пустым."));
                return false;
            }

            if (!Email.IsValidEmail())
            {
                ErrorHandleService.HandleException(new UserVisibleException("Поле Email введено не правильно."));
                return false;
            }

            return true;
        }
    }
}
