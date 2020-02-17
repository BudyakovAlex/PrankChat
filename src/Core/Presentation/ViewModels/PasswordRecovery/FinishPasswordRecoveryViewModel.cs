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
    public class FinishPasswordRecoveryViewModel : BaseViewModel
    {
        private readonly ISettingsService _settingService;

        public MvxAsyncCommand FinishRecoveringPasswordCommand => new MvxAsyncCommand(OnFinishRecoverPassword);

        public FinishPasswordRecoveryViewModel(INavigationService navigationService,
                                                IErrorHandleService errorHandleService,
                                                IApiService apiService,
                                                IDialogService dialogService,
                                                ISettingsService settingsService)
            : base(navigationService, errorHandleService, apiService, dialogService)
        {
            _settingService = settingsService;
        }

        private Task OnFinishRecoverPassword()
        {
            if (_settingService.User == null)
                return NavigationService.ShowLoginView();
            return NavigationService.ShowMainView();
        }
    }
}
