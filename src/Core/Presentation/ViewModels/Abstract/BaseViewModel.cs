using Microsoft.AppCenter.Crashes;
using MvvmCross;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Ioc;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Managers.Navigation;
using PrankChat.Mobile.Core.Plugins.Timer;
using PrankChat.Mobile.Core.Plugins.UserInteraction;
using PrankChat.Mobile.Core.Wrappers;
using System;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Abstract
{
    public abstract class BaseViewModel : MvxNotifyPropertyChanged, IDisposable
    {
        private bool _isDisposed;

        protected BaseViewModel()
        {
            Disposables = new CompositeDisposable();
            ExecutionStateWrapper = new ExecutionStateWrapper();

            ExecutionStateWrapper.SubscribeToEvent<ExecutionStateWrapper, bool>(
                OnIsBusyChanged,
                (wrapper, handler) => wrapper.IsBusyChanged += handler,
                (wrapper, handler) => wrapper.IsBusyChanged -= handler).DisposeWith(Disposables);

            SafeExecutionWrapper = new SafeExecutionWrapper(OnExceptionHandledAsync);
        }

        public ISystemTimer SystemTimer =>
            CompositionRoot.Container.Resolve<ISystemTimer>();

        public CompositeDisposable Disposables { get; }

        private ExecutionStateWrapper _executionStateWrapper;
        public ExecutionStateWrapper ExecutionStateWrapper
        {
            get => _executionStateWrapper;
            set
            {
                value.ThrowIfNull();
                SetProperty(ref _executionStateWrapper, value, OnIsBusyWrapperChanged);
            }
        }

        public virtual bool IsBusy => ExecutionStateWrapper.IsBusy;

        public virtual SafeExecutionWrapper SafeExecutionWrapper { get; }

        protected INavigationManager NavigationManager => Mvx.IoCProvider.Resolve<INavigationManager>();

        protected IMvxMessenger Messenger => Mvx.IoCProvider.Resolve<IMvxMessenger>();

        protected IUserInteraction UserInteraction => Mvx.IoCProvider.Resolve<IUserInteraction>();

        protected virtual void OnIsBusyWrapperChanged()
        {
        }

        protected virtual async Task OnExceptionHandledAsync(Exception exception)
        {
            await Task.Delay(500);

            UserInteraction.ShowToast(Resources.Error_Something_Went_Wrong_Message, Models.Enums.ToastType.Negative);
            Crashes.TrackError(exception);
        }

        protected virtual void OnIsBusyChanged(object sender, bool value)
        {
            RaisePropertyChanged(nameof(IsBusy));
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
