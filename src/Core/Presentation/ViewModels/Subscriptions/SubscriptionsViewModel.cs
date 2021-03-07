using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Managers.Users;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Common;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Subscriptions.Items
{
    public class SubscriptionsViewModel : PaginationViewModel, IMvxViewModel<SubscriptionsNavigationParameter, bool>
    {
        private readonly IUsersManager _usersManager;

        private int _userId;
        private bool _isUpdateNeeded;

        private Task _reloadTask;

        public SubscriptionsViewModel(IUsersManager usersManager) : base(Constants.Pagination.DefaultPaginationSize)
        {
            _usersManager = usersManager;

            Items = new MvxObservableCollection<SubscriptionItemViewModel>();
            CloseCompletionSource = new TaskCompletionSource<object>();
            LoadDataCommand = new MvxAsyncCommand(LoadDataAsync);
            ShowProfileCommand = new MvxAsyncCommand<SubscriptionItemViewModel>(ShowProfileAsync);
        }

        public MvxObservableCollection<SubscriptionItemViewModel> Items { get; }

        public IMvxAsyncCommand LoadDataCommand { get; }

        public IMvxAsyncCommand<SubscriptionItemViewModel> ShowProfileCommand { get; }

        public string Title { get; private set; }

        private SubscriptionTabType _selectedTabType;
        public SubscriptionTabType SelectedTabType
        {
            get => _selectedTabType;
            set
            {
                SetProperty(ref _selectedTabType, value);
                _ = DebounceRefreshDataAsync();
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

        private async Task DebounceRefreshDataAsync()
        {
            if (_reloadTask != null &&
                !_reloadTask.IsCompleted &&
                !_reloadTask.IsCanceled &&
                !_reloadTask.IsFaulted)
            {
                await _reloadTask;
            }

            Items.Clear();
            _reloadTask = ReloadItemsCommand.ExecuteAsync();
        }

        public void Prepare(SubscriptionsNavigationParameter parameter)
        {
            _userId = parameter.UserId;
            _selectedTabType = parameter.SubscriptionTabType;
            Title = parameter.UserName;
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            if (viewFinishing && CloseCompletionSource != null && !CloseCompletionSource.Task.IsCompleted && !CloseCompletionSource.Task.IsFaulted)
            {
                CloseCompletionSource?.SetResult(_isUpdateNeeded);
            }

            base.ViewDestroy(viewFinishing);
        }

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            await LoadMoreItemsCommand.ExecuteAsync();
        }

        protected override async Task<int> LoadMoreItemsAsync(int page = 1, int pageSize = Constants.Pagination.DefaultPaginationSize)
        {
            var items = await GetSubscriptionsAsync(page, pageSize);
            return SetList(items, page, ProduceSubscriptionItemViewModel, Items);
        }

        protected virtual async Task<Pagination<User>> GetSubscriptionsAsync(int page, int pageSize)
        {
            var getSubscriptionsTask = _usersManager.GetSubscriptionsAsync(_userId, page, pageSize);
            var getSubscribersTask = _usersManager.GetSubscribersAsync(_userId, page, pageSize);
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

            return new Pagination<User>();
        }

        private async Task ShowProfileAsync(SubscriptionItemViewModel viewModel)
        {
            if (!Connectivity.NetworkAccess.HasConnection())
            {
                return;
            }

            var shouldRefresh = await NavigationManager.NavigateAsync<UserProfileViewModel, int, bool>(viewModel.Id);
            if (!shouldRefresh)
            {
                return;
            }

            _isUpdateNeeded = shouldRefresh;
            await LoadDataCommand.ExecuteAsync();
        }

        private SubscriptionItemViewModel ProduceSubscriptionItemViewModel(User user)
        {
            return new SubscriptionItemViewModel(UserSessionProvider, user);
        }
    }
}