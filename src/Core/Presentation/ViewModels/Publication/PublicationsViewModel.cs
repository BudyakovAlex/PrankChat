using System;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication.Items;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Publication
{
    public class PublicationsViewModel : BaseViewModel
    {
        private readonly IDialogService _dialogService;

        private PublicationType _selectedPublicationType;
        public PublicationType SelectedPublicationType
        {
            get => _selectedPublicationType;
            set => SetProperty(ref _selectedPublicationType, value);
        }

        private string _activeFilterName;
        public string ActiveFilterName
        {
            get => _activeFilterName;
            set => SetProperty(ref _activeFilterName, value);
        }

        public MvxObservableCollection<PublicationItemViewModel> Items { get; } = new MvxObservableCollection<PublicationItemViewModel>();

        public MvxAsyncCommand OpenFilterCommand => new MvxAsyncCommand(OnOpenFilterAsync);

        public MvxAsyncCommand<PublicationItemViewModel> SelectItemCommand => new MvxAsyncCommand<PublicationItemViewModel>((item) => NavigationService.ShowDetailsPublicationView());

        public PublicationsViewModel(
            INavigationService navigationService,
            IDialogService dialogService) : base(navigationService)
        {
            _dialogService = dialogService;
        }

        public override Task Initialize()
        {
            base.Initialize();

            ActiveFilterName = Resources.Publication_Tab_Filter_Day;

            InitializePublications();
            return Task.CompletedTask;
        }

        private async Task OnOpenFilterAsync(CancellationToken arg)
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

        private Task InitializePublications()
        {
            Items.Add(new PublicationItemViewModel("Name one",
                                                   "https://images.pexels.com/photos/2092709/pexels-photo-2092709.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500",
                                                   "Name video one",
                                                   "https://ksassets.timeincuk.net/wp/uploads/sites/55/2019/04/GettyImages-1136749971-920x584.jpg",
                                                   134,
                                                   new System.DateTime(2018, 4, 24),
                                                   245));

            Items.Add(new PublicationItemViewModel("Name two",
                                       "https://images.pexels.com/photos/2092709/pexels-photo-2092709.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500",
                                       "Name video two Name video two Name video two Name video two Name video two Name video two Name video two",
                                       "https://cdn.pixabay.com/photo/2016/11/30/09/27/hacker-1872291_960_720.jpg",
                                       134,
                                       new System.DateTime(2018, 4, 24),
                                       245));

            Items.Add(new PublicationItemViewModel("Name three",
                           "https://images.pexels.com/photos/2092709/pexels-photo-2092709.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500",
                           "Name video three",
                           "https://images.pexels.com/photos/326055/pexels-photo-326055.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500",
                           134,
                           new System.DateTime(2018, 4, 24),
                           245));

            return Task.CompletedTask;
        }
    }
}
