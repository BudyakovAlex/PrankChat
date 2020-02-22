using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Commands;

namespace PrankChat.Mobile.Core.Commands
{
    public class MvxRestrictedCommand : MvxCommandBase, IMvxCommand, ICommand
	{
		private readonly Func<bool> _canExecute;
        private readonly Func<bool> _restrictedExecute;
        private readonly Func<Task> _handleFunc;
        private readonly Action _execute;

		public MvxRestrictedCommand(Action execute,
                                    Func<bool> canExecute = null,
									Func<bool> restrictedExecute = null,
									Func<Task> handleFunc = null)
		{
			_execute = execute;
			_canExecute = canExecute;
            _restrictedExecute = restrictedExecute;
            _handleFunc = handleFunc;
		}

		public bool CanExecute(object parameter)
		{
			 return _canExecute?.Invoke() ?? true;
		}

		public bool CanExecute()
		{
			return CanExecute(null);
		}

		public void Execute(object parameter)
		{
			if (!CanExecute(parameter))
			{
				return;
			}

			if (_restrictedExecute?.Invoke() ?? true)
			{
			    _execute.Invoke();
				return;
			}

			if (_handleFunc is null)
			{
				return;
			}

		    _handleFunc.Invoke();
		}

		public void Execute()
		{
			Execute(null);
		}
	}
}
