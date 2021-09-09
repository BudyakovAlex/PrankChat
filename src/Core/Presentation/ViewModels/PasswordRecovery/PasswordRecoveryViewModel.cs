using MvvmCross.Commands;
using PrankChat.Mobile.Core.Exceptions.UserVisible.Validation;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Managers.Authorization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Abstract;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.PasswordRecovery
{
    public class PasswordRecoveryViewModel : BasePageViewModel
    {
        private readonly IAuthorizationManager _authorizationManager;

        public PasswordRecoveryViewModel(IAuthorizationManager authorizationManager)
        {
            _authorizationManager = authorizationManager;

            RecoverPasswordCommand = this.CreateCommand(RecoverPasswordAsync);
        }

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public MvxAsyncCommand RecoverPasswordCommand { get; }

        private async Task RecoverPasswordAsync()
        {
            if (!CheckValidation())
            {
                return;
            }

            var result = await _authorizationManager.RecoverPasswordAsync(Email);
            if (string.IsNullOrWhiteSpace(result?.Result))
            {
                return;
            }

            await NavigationManager.NavigateAsync<FinishPasswordRecoveryViewModel>();
        }

        private bool CheckValidation()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                ErrorHandleService.HandleException(new ValidationException(Resources.Email, ValidationErrorType.Empty));
                ErrorHandleService.LogError(this, "E-mail can't be empty.");
                return false;
            }

            if (!Email.IsValidEmail())
            {
                ErrorHandleService.HandleException(new ValidationException(Resources.Email, ValidationErrorType.Invalid));
                ErrorHandleService.LogError(this, "E-mail is invalid.");
                return false;
            }

            return true;
        }
    }
}
