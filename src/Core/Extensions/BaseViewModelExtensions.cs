using MvvmCross.Commands;
using PrankChat.Mobile.Core.Commands;
using PrankChat.Mobile.Core.Presentation.ViewModels.Abstract;
using System;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Extensions
{
#nullable enable
    public static class BaseViewModelExtensions
    {
        public static MvxRestrictedAsyncCommand CreateRestrictedCommand(
            this BaseViewModel viewModel,
            Func<Task> execute,
            Func<bool>? canExecute = null,
            Func<bool>? restrictedCanExecute = null,
            Func<Task>? handleFunc = null,
            bool useIsBusyWrapper = true,
            bool notifyIsBusyChanged = true)
        {
            var wrappedExecute = WrapExecute(execute, viewModel, useIsBusyWrapper, notifyIsBusyChanged);
            return new MvxRestrictedAsyncCommand(wrappedExecute, canExecute, restrictedCanExecute: restrictedCanExecute, handleFunc: handleFunc);
        }

        public static MvxRestrictedCommand CreateRestrictedCommand(
            this BaseViewModel viewModel,
            Action execute,
            Func<bool>? canExecute = null,
            Func<bool>? restrictedCanExecute = null,
            Func<Task>? handleFunc = null,
            bool useIsBusyWrapper = true,
            bool notifyIsBusyChanged = true)
        {
            var wrappedExecute = WrapExecute(execute, viewModel, useIsBusyWrapper, notifyIsBusyChanged);
            return new MvxRestrictedCommand(wrappedExecute, canExecute, restrictedCanExecute, handleFunc);
        }

        public static MvxCommand CreateCommand(
            this BaseViewModel viewModel,
            Action execute,
            Func<bool>? canExecute = null,
            bool useIsBusyWrapper = true,
            bool notifyIsBusyChanged = true)
        {
            var wrappedExecute = WrapExecute(execute, viewModel, useIsBusyWrapper, notifyIsBusyChanged);
            return new MvxCommand(wrappedExecute, canExecute);
        }

        public static MvxCommand<T> CreateCommand<T>(
            this BaseViewModel viewModel,
            Action<T> execute,
            Func<T, bool>? canExecute = null,
            bool useIsBusyWrapper = true,
            bool notifyIsBusyChanged = true)
        {
            var wrappedExecute = WrapExecute(execute, viewModel, useIsBusyWrapper, notifyIsBusyChanged);
            return new MvxCommand<T>(wrappedExecute, canExecute);
        }

        public static MvxAsyncCommand CreateCommand(
            this BaseViewModel viewModel,
            Func<Task> execute,
            Func<bool>? canExecute = null,
            bool useIsBusyWrapper = true,
            bool notifyIsBusyChanged = true)
        {
            var wrappedExecute = WrapExecute(execute, viewModel, useIsBusyWrapper, notifyIsBusyChanged);
            return new MvxAsyncCommand(wrappedExecute, canExecute);
        }

        public static MvxAsyncCommand<T> CreateCommand<T>(
            this BaseViewModel viewModel,
            Func<T, Task> execute,
            Func<T, bool>? canExecute = null,
            bool useIsBusyWrapper = true,
            bool notifyIsBusyChanged = true)
        {
            var wrappedExecute = WrapExecute(execute, viewModel, useIsBusyWrapper, notifyIsBusyChanged);
            return new MvxAsyncCommand<T>(wrappedExecute, canExecute);
        }

        private static Action WrapExecute(
            Action execute,
            BaseViewModel viewModel,
            bool useIsBusyWrapper,
            bool notifyIsBusyChanged)
        {
            if (viewModel != null)
            {
                if (useIsBusyWrapper)
                {
                    var isBusyReference = execute;
                    execute = () => viewModel.ExecutionStateWrapper.Wrap(isBusyReference, notifyIsBusyChanged);
                }

                var safeExecutionReference = execute;
                execute = () => viewModel.SafeExecutionWrapper.Wrap(safeExecutionReference);
            }

            return execute;
        }

        private static Action<T> WrapExecute<T>(
            Action<T> execute,
            BaseViewModel viewModel,
            bool useIsBusyWrapper,
            bool notifyIsBusyChanged)
        {
            if (viewModel != null)
            {
                if (useIsBusyWrapper)
                {
                    var isBusyReference = execute;
                    execute = parameter => viewModel.ExecutionStateWrapper.Wrap(() => isBusyReference.Invoke(parameter), notifyIsBusyChanged);
                }

                var safeExecutionReference = execute;
                execute = parameter => viewModel.SafeExecutionWrapper.Wrap(() => safeExecutionReference.Invoke(parameter));
            }

            return execute;
        }

        private static Func<Task> WrapExecute(
            Func<Task> execute,
            BaseViewModel viewModel,
            bool useIsBusyWrapper,
            bool notifyIsBusyChanged)
        {
            if (viewModel != null)
            {
                if (useIsBusyWrapper)
                {
                    var isBusyReference = execute;
                    execute = () => viewModel.ExecutionStateWrapper.WrapAsync(isBusyReference, notifyIsBusyChanged);
                }

                var safeExecutionReference = execute;
                execute = () => viewModel.SafeExecutionWrapper.WrapAsync(safeExecutionReference);
            }

            return execute;
        }

        private static Func<T, Task> WrapExecute<T>(
            Func<T, Task> execute,
            BaseViewModel viewModel,
            bool useIsBusyWrapper,
            bool notifyIsBusyChanged)
        {
            if (viewModel != null)
            {
                if (useIsBusyWrapper)
                {
                    var isBusyReference = execute;
                    execute = parameter => viewModel.ExecutionStateWrapper.WrapAsync(() => isBusyReference.Invoke(parameter), notifyIsBusyChanged);
                }

                var safeExecutionReference = execute;
                execute = parameter => viewModel.SafeExecutionWrapper.WrapAsync(() => safeExecutionReference.Invoke(parameter));
            }

            return execute;
        }
    }
    #nullable disable
}