﻿using System.Collections.Generic;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Base
{
    public abstract class BaseViewModel : MvxViewModel
    {
        #region Image

        public double DownsampleWidth { get; } = 100;

        public List<ITransformation> Transformations => new List<ITransformation> { new CircleTransformation() };

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

        public MvxAsyncCommand ShowNotificationCommand
        {
            get { return new MvxAsyncCommand(NavigationService.ShowNotificationView); }
        }

        public BaseViewModel(INavigationService navigationService,
                            IErrorHandleService errorHandleService,
                            IApiService apiService,
                            IDialogService dialogService)
        {
            NavigationService = navigationService;
            ErrorHandleService = errorHandleService;
            ApiService = apiService;
            DialogService = dialogService;
        }
    }
}