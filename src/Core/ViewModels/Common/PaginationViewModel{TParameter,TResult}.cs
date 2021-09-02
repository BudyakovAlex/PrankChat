using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Common;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Models.Data.Shared;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.ViewModels.Abstract;
using PrankChat.Mobile.Core.Wrappers;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.ViewModels.Common
{
    class PaginationViewModel<TParameter, TResult> : BasePageViewModelResult<TResult>, IMvxViewModel<TParameter, TResult>
    {
        private const int DefaultPageIndex = 1;

        private readonly int _paginationSize;

        private readonly ExecutionStateWrapper _loadMoreExecutionStateWrapper;

        public PaginationViewModel(int paginationSize)
        {
            _paginationSize = paginationSize;

            _loadMoreExecutionStateWrapper = new ExecutionStateWrapper();
            _loadMoreExecutionStateWrapper.SubscribeToEvent<ExecutionStateWrapper, bool>(
                OnIsBusyChanged,
                (wrapper, handler) => wrapper.IsBusyChanged += handler,
                (wrapper, handler) => wrapper.IsBusyChanged -= handler).DisposeWith(Disposables);

            LoadMoreItemsCommand = this.CreateCommand(LoadMoreItemsInternalAsync, CanLoadMoreItems, useIsBusyWrapper: false);
            ReloadItemsCommand = this.CreateCommand(ReloadItemsAsync);
        }

        public override bool IsBusy => base.IsBusy || _loadMoreExecutionStateWrapper.IsBusy;

        public long TotalItemsCount { get; private set; }

        public int CurrentPaginationIndex { get; set; } = DefaultPageIndex;

        public long LoadedItemsCount { get; private set; }

        public bool HasNextPage => CanLoadMoreItems();

        public IMvxAsyncCommand LoadMoreItemsCommand { get; }

        public IMvxAsyncCommand ReloadItemsCommand { get; }

        protected bool ShouldNotifyIsBusy { get; set; } = true;

        protected virtual Task<int> LoadMoreItemsAsync(int page = 1, int pageSize = Constants.Pagination.DefaultPaginationSize)
        {
            return Task.FromResult(0);
        }

        protected virtual int SetList<TBussinessObject, TViewModel>(Pagination<TViewModel> pagination, int page, Func<TViewModel, TBussinessObject> produceItemViewModel, MvxObservableCollection<TBussinessObject> items)
        {
            SetTotalItemsCount(pagination.TotalCount);
            var viewModels = pagination.Items.Select(produceItemViewModel).ToList();

            if (page > 1)
            {
                items.AddRange(viewModels);
            }
            else
            {
                items.ReplaceWith(viewModels);
            }

            return viewModels.Count;
        }

        public virtual void Prepare(TParameter parameter)
        {

        }

        public void SetTotalItemsCount(long totalItemsCount)
        {
            TotalItemsCount = totalItemsCount;
            RaisePropertyChanged(nameof(TotalItemsCount));
        }

        public virtual void Reset()
        {
            TotalItemsCount = 0;
            CurrentPaginationIndex = DefaultPageIndex;
            LoadedItemsCount = 0;

            LoadMoreItemsCommand.RaiseCanExecuteChanged();
        }

        private Task LoadMoreItemsInternalAsync()
        {
            return _loadMoreExecutionStateWrapper.WrapAsync(async () =>
            {
                if (!Connectivity.NetworkAccess.HasConnection())
                {
                    if (UserInteraction.IsToastShown)
                    {
                        return;
                    }

                    UserInteraction.ShowToast(Resources.No_Intentet_Connection, ToastType.Negative);
                    return;
                }

                var loadedItems = await LoadMoreItemsAsync(CurrentPaginationIndex, _paginationSize);

                ++CurrentPaginationIndex;
                LoadedItemsCount += loadedItems;

                await RaisePropertiesChanged(nameof(LoadedItemsCount), nameof(CurrentPaginationIndex), nameof(HasNextPage));

                LoadMoreItemsCommand.RaiseCanExecuteChanged();
            }, ShouldNotifyIsBusy, true);
        }

        protected Task ReloadItemsAsync()
        {
            Reset();
            return LoadMoreItemsInternalAsync();
        }

        private bool CanLoadMoreItems()
        {
            if (CurrentPaginationIndex == DefaultPageIndex && TotalItemsCount == 0)
            {
                return true;
            }

            var currentPaginationCapacity = _paginationSize * (CurrentPaginationIndex - 1);
            return TotalItemsCount > currentPaginationCapacity;
        }
    }
}
