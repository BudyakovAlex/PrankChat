using PrankChat.Mobile.Core.Infrastructure.Extensions;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Wrappers
{
    public sealed class ExecutionStateWrapper : INotifyPropertyChanged
    {
        private readonly SemaphoreSlim _isExecutionSemaphore;

        private int _currentActionsCount;

        public ExecutionStateWrapper()
        {
            _isExecutionSemaphore = new SemaphoreSlim(1, 1);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<bool> IsBusyChanged;

        public bool IsBusy { get; private set; }

        public Task WrapAsync(Func<Task> action, bool notifyIsBusyChanged = true, bool awaitWhenBusy = false)
        {
            if (IsBusy && !awaitWhenBusy)
            {
                return Task.CompletedTask;
            }

            ++_currentActionsCount;

            return _isExecutionSemaphore.WrapAsync(async () =>
            {
                try
                {
                    SetIsBusy(true, notifyIsBusyChanged);
                    await action.Invoke();
                }
                finally
                {
                    --_currentActionsCount;
                    if (_currentActionsCount == 0)
                    {
                        SetIsBusy(false, notifyIsBusyChanged);
                    }
                }
            });
        }

        public Task WrapAsync(Action action, bool notifyIsBusyChanged = true, bool awaitWhenBusy = false)
        {
            if (IsBusy && !awaitWhenBusy)
            {
                return Task.CompletedTask;
            }

            ++_currentActionsCount;

            return _isExecutionSemaphore.WrapAsync(() =>
            {
                try
                {
                    SetIsBusy(true, notifyIsBusyChanged);
                    action?.Invoke();
                }
                finally
                {
                    --_currentActionsCount;
                    if (_currentActionsCount == 0)
                    {
                        SetIsBusy(false, notifyIsBusyChanged);
                    }
                }
            });
        }

        public void Wrap(Action action, bool notifyIsBusyChanged = true)
        {
            if (IsBusy)
            {
                return;
            }

            ++_currentActionsCount;

            try
            {
                SetIsBusy(true, notifyIsBusyChanged);
                action.Invoke();
            }
            finally
            {
                --_currentActionsCount;
                if (_currentActionsCount == 0)
                {
                    SetIsBusy(false, notifyIsBusyChanged);
                }
            }
        }

        private void SetIsBusy(bool value, bool notifyIsBusyChanged)
        {
            IsBusy = value;
            if (notifyIsBusyChanged)
            {
                IsBusyChanged?.Invoke(this, value);
                RaisePropertyChanged(nameof(IsBusy));
            }
        }

        private void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}