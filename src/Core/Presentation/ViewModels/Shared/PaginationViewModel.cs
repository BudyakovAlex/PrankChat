using System;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Models.Data.Shared;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Shared
{
    public class PaginationViewModel : BaseViewModel
    {
        private const int DefaultPageIndex = 1;

        private readonly int _paginationSize;

        public PaginationViewModel(int paginationSize,
                                    INavigationService navigationService,
                                    IErrorHandleService errorHandleService,
                                    IApiService apiService,
                                    IDialogService dialogService,
                                    ISettingsService settingsService)
            : base(navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
            _paginationSize = paginationSize;

            LoadMoreItemsCommand = new MvxAsyncCommand(LoadMoreItemsInternalAsync, CanLoadMoreItems);
            ReloadItemsCommand = new MvxAsyncCommand(ReloadItemsAsync);
            CurrentPaginationIndex = DefaultPageIndex;
        }

        public MvxAsyncCommand LoadMoreItemsCommand { get; }

        public MvxAsyncCommand ReloadItemsCommand { get; }

        public long TotalItemsCount { get; private set; }

        public int CurrentPaginationIndex { get; private set; }

        public long LoadedItemsCount { get; private set; }

        public void SetTotalItemsCount(long totalItemsCount)
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

        protected virtual Task<int> LoadMoreItemsAsync(int page = 1, int pageSize = Constants.Pagination.DefaultPaginationSize)
        {
            return Task.FromResult(0);
        }

        public bool HasNextPage => CanLoadMoreItems();

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

        private async Task LoadMoreItemsInternalAsync()
        {
            try
            {
                IsBusy = true;

                var loadedItems = await LoadMoreItemsAsync(CurrentPaginationIndex, _paginationSize);

                ++CurrentPaginationIndex;
                LoadedItemsCount += loadedItems;

                await Task.WhenAll(RaisePropertyChanged(nameof(LoadedItemsCount)),
                                   RaisePropertyChanged(nameof(CurrentPaginationIndex)),
                                   RaisePropertyChanged(nameof(HasNextPage)));
                LoadMoreItemsCommand.RaiseCanExecuteChanged();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private Task ReloadItemsAsync()
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

            var currentPaginationCapacity = _paginationSize * CurrentPaginationIndex;
            return TotalItemsCount > currentPaginationCapacity;
        }
    }
}
