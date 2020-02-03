using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Rating.Items;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Rating
{
    public class RatingViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        private readonly IMvxLog _mvxLog;
        private readonly IDialogService _dialogService;

        public MvxObservableCollection<RatingItemViewModel> Items { get; } = new MvxObservableCollection<RatingItemViewModel>();

        private string _activeFilterName;
        public string ActiveFilterName
        {
            get => _activeFilterName;
            set => SetProperty(ref _activeFilterName, value);
        }

        public MvxAsyncCommand OpenFilterCommand => new MvxAsyncCommand(OnOpenFilterAsync);

        public RatingViewModel(INavigationService navigationService,
                               IDialogService dialogService,
                               IApiService apiService,
                               IMvxLog mvxLog) : base(navigationService)
        {
            _dialogService = dialogService;
            _apiService = apiService;
            _mvxLog = mvxLog;
        }

        public override Task Initialize()
        {
            ActiveFilterName = Resources.RateView_Filter_AllTasks;
            return LoadRatingOrders();
        }

        private async Task LoadRatingOrders()
        {
            try
            {
                IsBusy = true;

                var ratingOrders = await _apiService.GetRatingOrdersAsync();
                //var items = ratingOrders?.Select(o => new RatingItemViewModel(
                //                                        NavigationService,
                //                                        o.Id,
                //                                        o.Title,
                //                                        o.Customer?.Avatar,
                //                                        o.Price,
                //                                        o.CreatedAt.Value));

                //Items.AddRange(items);
            }
            catch (Exception ex)
            {
                _mvxLog.DebugException($"{nameof(RatingViewModel)}", ex);
                _dialogService.ShowToast("Can not load rating!");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OnOpenFilterAsync(CancellationToken arg)
        {
            var selectedFilter = await _dialogService.ShowMenuDialogAsync(new[]
            {
                Resources.RateView_Filter_AllTasks,
                Resources.RateView_Filter_MyTasks,
                Resources.RateView_Filter_NewTasks,
            });

            if (string.IsNullOrWhiteSpace(selectedFilter) || selectedFilter == Resources.Cancel)
                return;

            ActiveFilterName = selectedFilter;
        }
    }
}
