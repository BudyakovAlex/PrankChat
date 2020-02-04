using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.Exceptions;
using PrankChat.Mobile.Core.Models.Data.FilterTypes;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Rating.Items;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Rating
{
    public class RatingViewModel : BaseViewModel
    {
        private readonly IMvxLog _mvxLog;

        public MvxObservableCollection<RatingItemViewModel> Items { get; } = new MvxObservableCollection<RatingItemViewModel>();

        private string _activeFilterName;
        public string ActiveFilterName
        {
            get => _activeFilterName;
            set => SetProperty(ref _activeFilterName, value);
        }

        private RatingOrderFilterType _activeFilter;
        public RatingOrderFilterType ActiveFilter
        {
            get => _activeFilter;
            set
            {
                _activeFilter = value;
                switch (_activeFilter)
                {
                    case RatingOrderFilterType.All:
                        ActiveFilterName = Resources.RateView_Filter_AllTasks;
                        break;

                    case RatingOrderFilterType.My:
                        ActiveFilterName = Resources.RateView_Filter_MyTasks;
                        break;

                    case RatingOrderFilterType.New:
                        ActiveFilterName = Resources.RateView_Filter_NewTasks;
                        break;
                }
            }
        }

        public MvxAsyncCommand OpenFilterCommand => new MvxAsyncCommand(OnOpenFilterAsync);

        public MvxAsyncCommand LoadRatingOrdersCommand => new MvxAsyncCommand(OnLoadRatingOrdersAsync);

        public RatingViewModel(INavigationService navigationService,
                               IDialogService dialogService,
                               IApiService apiService,
                               IMvxLog mvxLog,
                               IErrorHandleService errorHandleService)
            : base(navigationService, errorHandleService, apiService, dialogService)
        {
            _mvxLog = mvxLog;
        }

        public override Task Initialize()
        {
            ActiveFilter = RatingOrderFilterType.All;
            return LoadRatingOrdersCommand.ExecuteAsync();
        }

        private async Task OnLoadRatingOrdersAsync()
        {
            try
            {
                IsBusy = true;

                Items.Clear();

                var ratingOrders = await ApiService.GetRatingOrdersAsync(ActiveFilter);
                var items = ratingOrders?.Select(o => new RatingItemViewModel(
                                                            NavigationService,
                                                            o.Id,
                                                            o.Title,
                                                            o.Customer?.Avatar,
                                                            o.Price,
                                                            o.Likes,
                                                            o.Dislikes,
                                                            DateTime.Now));
                Items.AddRange(items);
            }
            catch (Exception ex)
            {
                _mvxLog.DebugException($"{nameof(RatingViewModel)}", ex);
                ErrorHandleService.HandleException(new UserVisibleException("Проблема с загрузкой оценок."));
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OnOpenFilterAsync(CancellationToken arg)
        {
            var selectedFilter = await DialogService.ShowMenuDialogAsync(new[]
            {
                Resources.RateView_Filter_AllTasks,
                Resources.RateView_Filter_MyTasks,
                Resources.RateView_Filter_NewTasks,
            }, Resources.Cancel);

            if (string.IsNullOrWhiteSpace(selectedFilter) || selectedFilter == Resources.Cancel)
                return;

            if (selectedFilter == Resources.RateView_Filter_AllTasks)
            {
                ActiveFilter = RatingOrderFilterType.All;
            }
            else if (selectedFilter == Resources.RateView_Filter_MyTasks)
            {
                ActiveFilter = RatingOrderFilterType.My;
            }
            else if (selectedFilter == Resources.RateView_Filter_NewTasks)
            {
                ActiveFilter = RatingOrderFilterType.New;
            }

            await LoadRatingOrdersCommand.ExecuteAsync();
        }
    }
}
