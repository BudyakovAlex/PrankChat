using System;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Infrastructure.Extensions;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Shared
{
    public class PaginationViewModel : MvxViewModel
    {
        private const int DefaultPageIndex = 1;

        private readonly Func<int, int, Task<int>> _loadMoreItemsFunc;
        private readonly int _paginationSize;

        public PaginationViewModel(Func<int, int, Task<int>> loadMoreItemsFunc, int paginationSize)
        {
            loadMoreItemsFunc.ThrowIfNull();

             _loadMoreItemsFunc = loadMoreItemsFunc;
            _paginationSize = paginationSize;

            LoadMoreItemsCommand = new MvxAsyncCommand(LoadMoreItemsAsync, CanLoadMoreItems);
            CurrentPaginationIndex = DefaultPageIndex;
        }

        public MvxAsyncCommand LoadMoreItemsCommand { get; }

        public int TotalItemsCount { get; private set; }

        public int CurrentPaginationIndex { get; private set; }

        public int LoadedItemsCount { get; private set; }

        public void SetTotalItemsCount(int totalItemsCount)
        {
            TotalItemsCount = totalItemsCount;
            RaisePropertyChanged(nameof(TotalItemsCount));
        }

        public void Reset()
        {
            TotalItemsCount = 0;
            CurrentPaginationIndex = DefaultPageIndex;
            LoadedItemsCount = 0;

            LoadMoreItemsCommand.RaiseCanExecuteChanged();
        }

        public bool HasNextPage => CanLoadMoreItems();

        private async Task LoadMoreItemsAsync()
        {
            var loadedItems = await _loadMoreItemsFunc.Invoke(CurrentPaginationIndex, _paginationSize);

            ++CurrentPaginationIndex;
            LoadedItemsCount += loadedItems;

            await Task.WhenAll(RaisePropertyChanged(nameof(LoadedItemsCount)),
                               RaisePropertyChanged(nameof(CurrentPaginationIndex)),
                               RaisePropertyChanged(nameof(HasNextPage)));
            LoadMoreItemsCommand.RaiseCanExecuteChanged();
        }

        private bool CanLoadMoreItems()
        {
            if (CurrentPaginationIndex == DefaultPageIndex && TotalItemsCount == 0)
            {
                return true;
            }

            var currentPaginationCapacity = _paginationSize * CurrentPaginationIndex;
            return TotalItemsCount > currentPaginationCapacity;
        }
    }
}