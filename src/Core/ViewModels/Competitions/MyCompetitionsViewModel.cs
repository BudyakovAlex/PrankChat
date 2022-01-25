using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.Common;
using PrankChat.Mobile.Core.Data.Enums;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Managers.Competitions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using PrankChat.Mobile.Core.ViewModels.Common;
using PrankChat.Mobile.Core.ViewModels.Competitions.Items;

namespace PrankChat.Mobile.Core.ViewModels.Competitions
{
    public class MyCompetitionsViewModel : PaginationViewModelResult<bool, CompetitionItemViewModel>
    {
        private readonly ICompetitionsManager _competitionsManager;

        private int _userId;
        private bool _isUpdateNeeded;

        private Task _reloadTask;

        public MyCompetitionsViewModel(ICompetitionsManager competitionsManager) : base(Constants.Pagination.DefaultPaginationSize)
        {
            _competitionsManager = competitionsManager;

            LoadDataCommand = this.CreateCommand(LoadDataAsync);
        }

        protected override bool DefaultResult => _isUpdateNeeded;

        public IMvxAsyncCommand LoadDataCommand { get; }

        public string Title { get; private set; }

        private CompetitionsTabType _selectedTabType;
        public CompetitionsTabType SelectedTabType
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

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            await LoadMoreItemsCommand.ExecuteAsync();
        }

        protected override async Task<int> LoadMoreItemsAsync(int page = 1, int pageSize = Constants.Pagination.DefaultPaginationSize)
        {
            var items = await GetSubscriptionsAsync(page, pageSize);
            return SetList(items, page, ProduceCompetitionItemViewModel, Items);
        }

        protected virtual async Task<Pagination<Competition>> GetSubscriptionsAsync(int page, int pageSize)
        {
            var getSubscriptionsTask = _competitionsManager.GetSubscriptionsAsync(_userId, page, pageSize);
            var getSubscribersTask = _competitionsManager.GetSubscribersAsync(_userId, page, pageSize);
            await Task.WhenAll(getSubscriptionsTask, getSubscribersTask);

            SubscribersTitle = string.Format(Resources.SubscribersTemplate, getSubscribersTask.Result.TotalCount.ToCountString());
            SubscriptionsTitle = string.Format(Resources.SubscriptionTemplate, getSubscriptionsTask.Result.TotalCount.ToCountString());

            return SelectedTabType switch
            {
                CompetitionsTabType.Ordered => getSubscribersTask.Result,
                CompetitionsTabType.InExecution => getSubscriptionsTask.Result,
                _ => new Pagination<User>(),
            };
        }

        private CompetitionItemViewModel ProduceCompetitionItemViewModel(Competition competition) =>
            new CompetitionItemViewModel(IsUserSessionInitialized, competition);

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
