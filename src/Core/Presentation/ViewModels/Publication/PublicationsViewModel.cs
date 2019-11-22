using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Navigation;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Publication
{
    public class PublicationsViewModel : BaseViewModel
    {
        private readonly IDialogService _dialogService;
        private string _activeFilterName;
        private PublicationType _selectedPublicationType;

        public PublicationsViewModel(
            INavigationService navigationService,
            IDialogService dialogService) : base(navigationService)
        {
            _dialogService = dialogService;
        }

        public PublicationType SelectedPublicationType
        {
            get => _selectedPublicationType;
            set => SetProperty(ref _selectedPublicationType, value);
        }

        public string ActiveFilterName
        {
            get => _activeFilterName;
            set => SetProperty(ref _activeFilterName, value);
        }

        public MvxAsyncCommand ShowNotificationCommand
        {
            get { return new MvxAsyncCommand(() => NavigationService.ShowNotificationView()); }
        }

        public MvxAsyncCommand OpenFilterCommand => new MvxAsyncCommand(OnOpenFilterCommand);

        public override Task Initialize()
        {
            base.Initialize();

            ActiveFilterName = Resources.Publication_Tab_Filter_Day;

            return Task.CompletedTask;
        }

        private async Task OnOpenFilterCommand(CancellationToken arg)
        {
            var selectedFilter = await _dialogService.ShowFilterSelectionAsync(new[]
            {
                Resources.Publication_Tab_Filter_Day,
                Resources.Publication_Tab_Filter_Week,
                Resources.Publication_Tab_Filter_Month,
                Resources.Publication_Tab_Filter_Quarter,
                Resources.Publication_Tab_Filter_HalfYear
            });

            if (string.IsNullOrWhiteSpace(selectedFilter) || selectedFilter == Resources.Cancel)
                return;

            ActiveFilterName = selectedFilter;
        }
    }
}
