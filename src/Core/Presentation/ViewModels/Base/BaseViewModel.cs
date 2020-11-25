﻿using MvvmCross;
using MvvmCross.Navigation;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Wrappers;
using System;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Base
{
    public abstract class BaseViewModel : MvxNotifyPropertyChanged
    {
        protected BaseViewModel()
        {
            Messenger = Mvx.IoCProvider.Resolve<IMvxMessenger>();

            ExecutionStateWrapper = new ExecutionStateWrapper();
            SafeExecutionWrapper = new SafeExecutionWrapper(OnExceptionHandledAsync);
        }

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

        public bool IsBusy => ExecutionStateWrapper.IsBusy;

        protected virtual SafeExecutionWrapper SafeExecutionWrapper { get; }

        protected INavigationService NavigationService { get; }

        protected IMvxMessenger Messenger { get; }

        protected virtual void OnIsBusyWrapperChanged()
        {
        }

        protected virtual Task OnExceptionHandledAsync(Exception exception)
        {
            return Task.CompletedTask;
        }
    }
}
