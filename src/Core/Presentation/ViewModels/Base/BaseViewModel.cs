using System.Collections.Generic;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Commands;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Base
{
    public abstract class BaseViewModel : MvxViewModel
    {
        #region Services

        public INavigationService NavigationService { get; }

        public IErrorHandleService ErrorHandleService { get; }

        public IApiService ApiService { get; }

        public IDialogService DialogService { get; }

        public ISettingsService SettingsService { get; }

        #endregion

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public bool IsUserSessionInitialized => SettingsService.User != null;

        public MvxAsyncCommand GoBackCommand
        {
            get
            {
                return new MvxAsyncCommand(() => NavigationService.CloseView(this));
            }
        }

        public MvxAsyncCommand ShowSearchCommand
        {
            get { return new MvxAsyncCommand(() => NavigationService.ShowSearchView()); }
        }

        public IMvxAsyncCommand ShowNotificationCommand { get; }

        public BaseViewModel(INavigationService navigationService,
                             IErrorHandleService errorHandleService,
                             IApiService apiService,
                             IDialogService dialogService,
                             ISettingsService settingsService)
        {
            NavigationService = navigationService;
            ErrorHandleService = errorHandleService;
            ApiService = apiService;
            DialogService = dialogService;
            DialogService = dialogService;
            SettingsService = settingsService;

            ShowNotificationCommand = new MvxRestrictedAsyncCommand(NavigationService.ShowNotificationView, restrictedCanExecute: () => IsUserSessionInitialized, handleFunc: NavigationService.ShowLoginView);
        }
    }
}
