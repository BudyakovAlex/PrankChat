using System.Threading.Tasks;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.Common;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Managers.Users;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.ViewModels.Common;
using PrankChat.Mobile.Core.ViewModels.Parameters;

namespace PrankChat.Mobile.Core.ViewModels.Subscriptions.Items
{
    public class SubscriptionsViewModel : PaginationViewModel<SubscriptionsNavigationParameter, bool, SubscriptionItemViewModel>
    {
        private readonly IUsersManager _usersManager;

        private int _userId;
        private bool _isUpdateNeeded;

        private Task _reloadTask;

        public SubscriptionsViewModel(IUsersManager usersManager) : base(Constants.Pagination.DefaultPaginationSize)
        {
            _usersManager = usersManager;

            LoadDataCommand = this.CreateCommand(LoadDataAsync);
        }

        protected override bool DefaultResult => _isUpdateNeeded; 

        public IMvxAsyncCommand LoadDataCommand { get; }

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

        public override void Prepare(SubscriptionsNavigationParameter parameter)
        {
            _userId = parameter.UserId;
            _selectedTabType = parameter.SubscriptionTabType;
            Title = parameter.UserName;
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

            SubscribersTitle = string.Format(Resources.SubscribersTemplate, getSubscribersTask.Result.TotalCount.ToCountString());
            SubscriptionsTitle = string.Format(Resources.SubscriptionTemplate, getSubscriptionsTask.Result.TotalCount.ToCountString());

            return SelectedTabType switch
            {
                SubscriptionTabType.Subscribers => getSubscribersTask.Result,
                SubscriptionTabType.Subscriptions => getSubscriptionsTask.Result,
                _ => new Pagination<User>(),
            };
        }

        private SubscriptionItemViewModel ProduceSubscriptionItemViewModel(User user) =>
            new SubscriptionItemViewModel(UserSessionProvider, user, LoadDataAsync);

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
    }
}