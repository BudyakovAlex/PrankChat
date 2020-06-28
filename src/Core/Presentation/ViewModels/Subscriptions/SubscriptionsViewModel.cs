﻿using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Shared;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Subscriptions
{
    public class SubscriptionsViewModel : PaginationViewModel, IMvxViewModel<SubscriptionsNavigationParameter, bool>
    {
        private int _userId;
        private bool _isUpdateNeeded;
        private string _userName;

        public SubscriptionsViewModel(INavigationService navigationService,
                                      IErrorHandleService errorHandleService,
                                      IApiService apiService,
                                      IDialogService dialogService,
                                      ISettingsService settingsService)
            : base(Constants.Pagination.DefaultPaginationSize,
                   navigationService,
                   errorHandleService,
                   apiService,
                   dialogService,
                   settingsService)
        {
            Items = new MvxObservableCollection<SubscriptionItemViewModel>();
            CloseCompletionSource = new TaskCompletionSource<object>();
            LoadDataCommand = new MvxAsyncCommand(LoadDataAsync);
            ShowProfileCommand = new MvxAsyncCommand<SubscriptionItemViewModel>(ShowProfileAsync);
        }

        public MvxObservableCollection<SubscriptionItemViewModel> Items { get; }

        public IMvxAsyncCommand LoadDataCommand { get; }

        public IMvxAsyncCommand<SubscriptionItemViewModel> ShowProfileCommand { get; }

        public string Title => _userName;

        private SubscriptionTabType _selectedTabType;
        public SubscriptionTabType SelectedTabType
        {
            get => _selectedTabType;
            set
            {
                if (SetProperty(ref _selectedTabType, value))
                {
                    LoadDataCommand.Cancel();
                    LoadDataCommand.ExecuteAsync();
                }
            }
        }

        private string _subscribersTitle;
        public string SubscribersTitle
        {
            get => _subscribersTitle;
            set => SetProperty(ref _subscribersTitle, value);
        }

        private string _subscriptionsTitle;
        public string SubscriptionsTitle
        {
            get => _subscriptionsTitle;
            set => SetProperty(ref _subscriptionsTitle, value);
        }

        public TaskCompletionSource<object> CloseCompletionSource { get; set; }

        private Task LoadDataAsync()
        {
            Reset();
            Items.Clear();

            return LoadMoreItemsCommand.ExecuteAsync();
        }

        public void Prepare(SubscriptionsNavigationParameter parameter)
        {
            _userId = parameter.UserId;
            _selectedTabType = parameter.SubscriptionTabType;
            _userName = parameter.UserName;
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            if (viewFinishing && CloseCompletionSource != null && !CloseCompletionSource.Task.IsCompleted && !CloseCompletionSource.Task.IsFaulted)
            {
                CloseCompletionSource?.SetResult(_isUpdateNeeded);
            }

            base.ViewDestroy(viewFinishing);
        }

        public override async Task Initialize()
        {
            await base.Initialize();
            await LoadMoreItemsCommand.ExecuteAsync();
        }

        protected override async Task<int> LoadMoreItemsAsync(int page = 1, int pageSize = Constants.Pagination.DefaultPaginationSize)
        {
            var items = await GetSubscriptionsAsync(page, pageSize);
            return SetList(items, page, ProduceSubscriptionItemViewModel, Items);
        }

        protected virtual async Task<PaginationModel<UserDataModel>> GetSubscriptionsAsync(int page, int pageSize)
        {
            var getSubscriptionsTask = ApiService.GetSubscriptionsAsync(_userId, page, pageSize);
            var getSubscribersTask = ApiService.GetSubscribersAsync(_userId, page, pageSize);
            await Task.WhenAll(getSubscriptionsTask, getSubscribersTask);

            SubscribersTitle = string.Format(Resources.Subscribers_Title_Template, getSubscribersTask.Result.TotalCount.ToCountString());
            SubscriptionsTitle = string.Format(Resources.Subscription_Title_Template, getSubscriptionsTask.Result.TotalCount.ToCountString());

            switch (SelectedTabType)
            {
                case SubscriptionTabType.Subscribers:
                    return getSubscribersTask.Result;

                case SubscriptionTabType.Subscriptions:
                    return getSubscriptionsTask.Result;
            }

            return new PaginationModel<UserDataModel>();
        }

        private async Task ShowProfileAsync(SubscriptionItemViewModel viewModel)
        {
            var shouldRefresh = await NavigationService.ShowUserProfile(viewModel.Id);
            if (!shouldRefresh)
            {
                return;
            }

            await LoadDataCommand.ExecuteAsync();
        }

        private SubscriptionItemViewModel ProduceSubscriptionItemViewModel(UserDataModel userDataModel)
        {
            return new SubscriptionItemViewModel(userDataModel,
                                                 NavigationService,
                                                 ErrorHandleService,
                                                 ApiService,
                                                 DialogService,
                                                 SettingsService);
        }
    }
}