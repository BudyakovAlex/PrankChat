using System;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Rating.Items;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Rating
{
    public class RatingViewModel : BaseViewModel
    {
        private readonly IDialogService _dialogService;

        public MvxObservableCollection<RatingItemViewModel> Items { get; } = new MvxObservableCollection<RatingItemViewModel>();

        private string _activeFilterName;
        public string ActiveFilterName
        {
            get => _activeFilterName;
            set => SetProperty(ref _activeFilterName, value);
        }

        public MvxAsyncCommand OpenFilterCommand => new MvxAsyncCommand(OnOpenFilterAsync);

        public RatingViewModel(INavigationService navigationService, IDialogService dialogService) : base(navigationService)
        {
            _dialogService = dialogService;

            Items.Add(new RatingItemViewModel(navigationService, "Подсесть к человеку в ТЦ и съесть его еду и сказать сальто де марто", "https://ksassets.timeincuk.net/wp/uploads/sites/55/2019/04/GettyImages-1136749971-920x584.jpg", "13 455 p", new DateTime(2019, 4, 22)));
            Items.Add(new RatingItemViewModel(navigationService, "Выпить бутылку воды без остановки", "https://ksassets.timeincuk.net/wp/uploads/sites/55/2019/04/GettyImages-1136749971-920x584.jpg", "995,55 p", new DateTime(2019, 11, 2)));
        }

        public override Task Initialize()
        {
            ActiveFilterName = Resources.RateView_Filter_AllTasks;
            return base.Initialize();
        }

        private async Task OnOpenFilterAsync(CancellationToken arg)
        {
            var selectedFilter = await _dialogService.ShowFilterSelectionAsync(new[]
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
