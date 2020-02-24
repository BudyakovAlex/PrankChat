using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.PasswordRecovery
{
    public class PasswordRecoveryViewModel : BaseViewModel
    {
        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public MvxAsyncCommand RecoverPasswordCommand => new MvxAsyncCommand(OnRecoverPassword);

        public PasswordRecoveryViewModel(INavigationService navigationService,
                                         IErrorHandleService errorHandleService,
                                         IApiService apiService,
                                         IDialogService dialogService,
                                         ISettingsService settingsService)
            : base(navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
        }

        private Task OnRecoverPassword()
        {
            return NavigationService.ShowFinishPasswordRecoveryView();
        }
    }
}
