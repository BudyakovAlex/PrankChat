using MvvmCross;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Timer;
using PrankChat.Mobile.Core.BusinessServices.Logger;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Presentation.ViewModels.Notification;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using PrankChat.Mobile.Core.Providers.UserSession;
using PrankChat.Mobile.Core.Wrappers;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Abstract
{
    public abstract class BasePageViewModel : BaseViewModel, IMvxViewModel
    {
        private const int RefreshAfterSeconds = 15;

        private int _timerThicksCount;

        public BasePageViewModel()
        {
            ShowNotificationCommand = this.CreateRestrictedCommand(
                ShowNotificationAsync,
                restrictedCanExecute: () => IsUserSessionInitialized,
                handleFunc: NavigationManager.NavigateAsync<LoginViewModel>);

            ShowSearchCommand = this.CreateCommand(ShowSearchAsync);
            CloseCommand = this.CreateCommand<bool?>(CloseAsync);
        }

        public event EventHandler<bool> AppearingChanged;

        public IErrorHandleService ErrorHandleService => Mvx.IoCProvider.Resolve<IErrorHandleService>();

        public IUserSessionProvider UserSessionProvider => Mvx.IoCProvider.Resolve<IUserSessionProvider>();

        public ILogger Logger => Mvx.IoCProvider.Resolve<ILogger>();

        public INotificationBageViewModel NotificationBageViewModel => Mvx.IoCProvider.Resolve<INotificationBageViewModel>();

        public bool IsInitialized { get; private set; }

        public bool IsUserSessionInitialized => UserSessionProvider.User != null;

        public ICommand CloseCommand { get; }
       
        public ICommand ShowSearchCommand { get; }

        public ICommand ShowNotificationCommand { get; }

        public virtual void ViewCreated()
        {
        }

        public virtual void ViewAppearing()
        {
            AppearingChanged?.Invoke(this, true);
        }

        public virtual void ViewAppeared()
        {
        }

        public virtual void ViewDisappearing()
        {
            AppearingChanged?.Invoke(this, false);
        }

        public virtual void ViewDisappeared()
        {
        }

        public virtual void ViewDestroy(bool viewFinishing = true)
        {
        }

        public virtual void OnIsInitializedChanged()
        {
        }

        Task IMvxViewModel.Initialize()
        {
            return ExecutionStateWrapper.WrapAsync(() => SafeExecutionWrapper.WrapAsync(async () =>
            {
                await InitializeAsync().ConfigureAwait(false);
                IsInitialized = true;
                await RaisePropertyChanged(nameof(IsInitialized)).ConfigureAwait(false);
                OnIsInitializedChanged();
            }));
        }

        MvxNotifyTask IMvxViewModel.InitializeTask { get; set; }

        public virtual void Prepare()
        {
        }

        public virtual Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        void IMvxViewModel.Start()
        {
        }

        void IMvxViewModel.Init(IMvxBundle parameters)
        {
        }

        void IMvxViewModel.SaveState(IMvxBundle state)
        {
        }

        void IMvxViewModel.ReloadState(IMvxBundle state)
        {
        }

        public virtual Task RaisePropertiesChanged(params string[] propertiesNames)
        {
            var raisePropertiesTasks = propertiesNames.Select(propertyName => RaisePropertyChanged(propertyName));
            return Task.WhenAll(raisePropertiesTasks);
        }

        protected void OnTimerTick(TimerTickMessage msg)
        {
            _timerThicksCount++;
            if (_timerThicksCount >= RefreshAfterSeconds)
            {
                _timerThicksCount = 0;
                NotificationBageViewModel.RefreshDataCommand.ExecuteAsync(null).FireAndForget();
            }
        }

        public virtual Task<bool> ConfirmPlatformCloseAsync()
        {
            return Task.FromResult(true);
        }

        protected virtual async Task CloseAsync(bool? isPlatform)
        {
            var isCloseDeclined = isPlatform == true && !await ConfirmPlatformCloseAsync();
            if (isCloseDeclined)
            {
                return;
            }

            await NavigationManager.CloseAsync(this);
        }

        private Task ShowNotificationAsync()
        {
            return NavigationManager.NavigateAsync<NotificationViewModel>();
        }

        private Task ShowSearchAsync()
        {
            return NavigationManager.NavigateAsync<SearchViewModel>();
        }
    }
}