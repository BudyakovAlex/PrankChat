using System;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Registration
{
    public class RegistrationThirdStepViewModel : BaseViewModel
    {
        public MvxAsyncCommand FinishRegistrationCommand
        {
            get
            {
                return new MvxAsyncCommand(() => NavigationService.ShowMainView());
            }
        }

        public RegistrationThirdStepViewModel(INavigationService navigationService,
                                                IErrorHandleService errorHandleService,
                                                IApiService apiService,
                                                IDialogService dialogService)
            : base(navigationService, errorHandleService, apiService, dialogService)
        {
        }
    }
}
