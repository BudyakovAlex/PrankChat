using System.Threading.Tasks;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.Common;
using PrankChat.Mobile.Core.Data.Enums;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Localization;
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

        private string _orderesTitle;
        public string OrderedTitle
        {
            get => _orderesTitle;
            set => SetProperty(ref _orderesTitle, value);
        }

        private string _executeTitle;
        public string ExecuteTitle
        {
            get => _executeTitle;
            set => SetProperty(ref _executeTitle, value);
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
            var getOrderdCompetitionsTask = _competitionsManager.GetMyOrderedCompetitionsAsync(page, pageSize);
            var getExecuteCompetitionsTask = _competitionsManager.GetMyExecuteCompetitionsAsync(page, pageSize);
            await Task.WhenAll(getOrderdCompetitionsTask, getExecuteCompetitionsTask);

            OrderedTitle = string.Format(Resources.Ordered, getExecuteCompetitionsTask.Result.TotalCount.ToCountString());
            ExecuteTitle = string.Format(Resources.Execute, getOrderdCompetitionsTask.Result.TotalCount.ToCountString());

            return SelectedTabType switch
            {
                CompetitionsTabType.Ordered => getExecuteCompetitionsTask.Result,
                CompetitionsTabType.InExecution => getOrderdCompetitionsTask.Result,
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
