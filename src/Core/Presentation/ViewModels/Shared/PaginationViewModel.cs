using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data.Shared;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Wrappers;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Shared
{
    public class PaginationViewModel : BasePageViewModel
    {
        private const int DefaultPageIndex = 1;

        private readonly int _paginationSize;

        private readonly ExecutionStateWrapper _loadMoreExecutionStateWrapper;

        public PaginationViewModel(int paginationSize)
        {
            _paginationSize = paginationSize;

            _loadMoreExecutionStateWrapper = new ExecutionStateWrapper();
            _loadMoreExecutionStateWrapper.SubscribeToEvent<ExecutionStateWrapper, bool>(OnIsBusyChanged,
                                                                                        (wrapper, handler) => wrapper.IsBusyChanged += handler,
                                                                                        (wrapper, handler) => wrapper.IsBusyChanged -= handler).DisposeWith(Disposables);

            LoadMoreItemsCommand = new MvxAsyncCommand(LoadMoreItemsInternalAsync, CanLoadMoreItems);
            ReloadItemsCommand = new MvxAsyncCommand(ReloadItemsAsync);
        }

        public override bool IsBusy => base.IsBusy || _loadMoreExecutionStateWrapper.IsBusy;

        public long TotalItemsCount { get; private set; }

        public int CurrentPaginationIndex { get; set; } = DefaultPageIndex;

        public long LoadedItemsCount { get; private set; }

        public bool HasNextPage => CanLoadMoreItems();

        public IMvxAsyncCommand LoadMoreItemsCommand { get; }

        public IMvxAsyncCommand ReloadItemsCommand { get; }

        protected virtual Task<int> LoadMoreItemsAsync(int page = 1, int pageSize = Constants.Pagination.DefaultPaginationSize)
        {
            return Task.FromResult(0);
        }

        protected virtual int SetList<TDataModel, TApiModel>(PaginationModel<TApiModel> dataModel, int page, Func<TApiModel, TDataModel> produceItemViewModel, MvxObservableCollection<TDataModel> items)
        {
            SetTotalItemsCount(dataModel.TotalCount);
            var viewModels = dataModel.Items.Select(produceItemViewModel).ToList();

            if (page > 1)
            {
                items.AddRange(viewModels);
            }
            else
            {
                items.SwitchTo(viewModels);
            }

            return viewModels.Count;
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
                    if (DialogService.IsToastShown)
                    {
                        return;
                    }

                    DialogService.ShowToast(Resources.No_Intentet_Connection, ToastType.Negative);
                    return;
                }

                var loadedItems = await LoadMoreItemsAsync(CurrentPaginationIndex, _paginationSize);

                ++CurrentPaginationIndex;
                LoadedItemsCount += loadedItems;

                await Task.WhenAll(RaisePropertyChanged(nameof(LoadedItemsCount)),
                                   RaisePropertyChanged(nameof(CurrentPaginationIndex)),
                                   RaisePropertyChanged(nameof(HasNextPage)));

                LoadMoreItemsCommand.RaiseCanExecuteChanged();
            }, awaitWhenBusy: true);
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
