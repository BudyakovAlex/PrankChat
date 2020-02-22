using System.Collections.Generic;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using MvvmCross;
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
        #region Image

        public double DownsampleWidth { get; } = 100;

        public virtual List<ITransformation> Transformations => new List<ITransformation> { new CircleTransformation() };

        #endregion

        #region Services

        public INavigationService NavigationService { get; }

        public IErrorHandleService ErrorHandleService { get; }

        public IApiService ApiService { get; }

        public IDialogService DialogService { get; }

        #endregion

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public bool IsUserSessionInitialized { get; }

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
                             IDialogService dialogService)
        {
            NavigationService = navigationService;
            ErrorHandleService = errorHandleService;
            ApiService = apiService;
            DialogService = dialogService;

            var isSettingsResolved = Mvx.IoCProvider.TryResolve<ISettingsService>(out var settingsService);
            if (isSettingsResolved)
            {
                IsUserSessionInitialized = settingsService.User != null;
            }

            ShowNotificationCommand = new MvxRestrictedAsyncCommand(NavigationService.ShowNotificationView, restrictedCanExecute: () => IsUserSessionInitialized, handleFunc: NavigationService.ShowLoginView);
        }
    }
}
