using MvvmCross;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Wrappers;
using System;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Base
{
    public abstract class BaseViewModel : MvxNotifyPropertyChanged, IDisposable
    {
        private bool _isDisposed;

        protected BaseViewModel()
        {
            Disposables = new CompositeDisposable();
            ExecutionStateWrapper = new ExecutionStateWrapper();

            ExecutionStateWrapper.SubscribeToEvent<ExecutionStateWrapper, bool>(OnIsBusyChanged,
                                                                               (wrapper, handler) => wrapper.IsBusyChanged += handler,
                                                                               (wrapper, handler) => wrapper.IsBusyChanged -= handler).DisposeWith(Disposables);

            SafeExecutionWrapper = new SafeExecutionWrapper(OnExceptionHandledAsync);
        }

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

        protected INavigationService NavigationService => Mvx.IoCProvider.Resolve<INavigationService>();

        protected IMvxMessenger Messenger => Mvx.IoCProvider.Resolve<IMvxMessenger>();

        protected IDialogService DialogService => Mvx.IoCProvider.Resolve<IDialogService>();

        protected virtual void OnIsBusyWrapperChanged()
        {
        }

        protected virtual Task OnExceptionHandledAsync(Exception exception)
        {
            return Task.CompletedTask;
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
