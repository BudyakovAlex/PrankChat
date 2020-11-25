using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.ApplicationServices.Timer;
using PrankChat.Mobile.Core.BusinessServices.Logger;
using PrankChat.Mobile.Core.Commands;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Presentation.Navigation;
using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Base
{
    public abstract class BasePageViewModel : BaseViewModel, IMvxViewModel
    {
        private const int RefreshAfterSeconds = 15;

        private int _timerThicksCount;
        private bool _isDisposed;

        public BasePageViewModel()
        {
            ShowNotificationCommand = new MvxRestrictedAsyncCommand(NavigationService.ShowNotificationView, restrictedCanExecute: () => IsUserSessionInitialized, handleFunc: NavigationService.ShowLoginView);
            ShowSearchCommand = new MvxAsyncCommand(NavigationService.ShowSearchView);
            GoBackCommand = new MvxAsyncCommand(GoBackAsync);

            Messenger.SubscribeOnMainThread<RefreshNotificationsMessage>(async (msg) => await NotificationBageViewModel.RefreshDataCommand.ExecuteAsync(null)).DisposeWith(Disposables);
            Messenger.Subscribe<TimerTickMessage>(OnTimerTick, MvxReference.Strong).DisposeWith(Disposables);
        }

        public event EventHandler<bool> AppearingChanged;

        public IErrorHandleService ErrorHandleService => Mvx.IoCProvider.Resolve<IErrorHandleService>();

        public IApiService ApiService => Mvx.IoCProvider.Resolve<IApiService>();

        public IDialogService DialogService => Mvx.IoCProvider.Resolve<IDialogService>();

        public ISettingsService SettingsService => Mvx.IoCProvider.Resolve<ISettingsService>();

        public ILogger Logger => Mvx.IoCProvider.Resolve<ILogger>();

        public INotificationBageViewModel NotificationBageViewModel => Mvx.IoCProvider.Resolve<INotificationBageViewModel>();

        public bool IsInitialized { get; private set; }

        public bool IsUserSessionInitialized => SettingsService.User != null;

        public MvxAsyncCommand GoBackCommand { get; }
       
        public MvxAsyncCommand ShowSearchCommand { get; }

        public IMvxAsyncCommand ShowNotificationCommand { get; }

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

        public CompositeDisposable Disposables { get; }

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

        private void OnTimerTick(TimerTickMessage msg)
        {
            _timerThicksCount++;
            if (_timerThicksCount >= RefreshAfterSeconds)
            {
                _timerThicksCount = 0;
                NotificationBageViewModel.RefreshDataCommand.ExecuteAsync(null).FireAndForget();
            }
        }

        private Task GoBackAsync()
        {
            return NavigationService.CloseView(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                System.Diagnostics.Debug.WriteLine($"Calling Dispose second time for {GetType().Name}. Ignoring");
                return;
            }

            if (disposing)
            {
                Disposables.Dispose();
            }

            _isDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);

        }
    }
}