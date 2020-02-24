using System;
using System.Threading.Tasks;
using MvvmCross.Commands;

namespace PrankChat.Mobile.Core.Commands
{
    public class MvxRestrictedAsyncCommand : MvxAsyncCommandBase, IMvxAsyncCommand
    {
        private readonly Func<Task> _execute;
        private readonly Func<bool> _canExecute;
        private readonly Func<bool> _restrictedCanExecute;
        private readonly Func<Task> _handleFunc;

        public MvxRestrictedAsyncCommand(Func<Task> execute,
                                         Func<bool> canExecute = null,
                                         bool allowConcurrentExecutions = false,
                                         Func<bool> restrictedCanExecute = null,
                                         Func<Task> handleFunc = null)
            : base(allowConcurrentExecutions)
        {
            _execute = execute;
            _canExecute = canExecute;
            _restrictedCanExecute = restrictedCanExecute;
            _handleFunc = handleFunc;
        }

        public Task ExecuteAsync(object parameter = null)
        {
            return ExecuteAsyncImpl(parameter);
        }

        protected override bool CanExecuteImpl(object parameter)
        {
            return _canExecute?.Invoke() ?? true;
        }

        protected override Task ExecuteAsyncImpl(object parameter)
        {
            if (_restrictedCanExecute?.Invoke() ?? true)
            {
                return _execute.Invoke();
            }

            if (_handleFunc is null)
            {
                return Task.CompletedTask;
            }

            return _handleFunc.Invoke();
        }
    }
}