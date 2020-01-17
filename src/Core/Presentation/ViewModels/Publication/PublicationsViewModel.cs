using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Platforms;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication.Items;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Publication
{
    public class PublicationsViewModel : BaseViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly IApiService _apiService;
        private readonly IPlatformService _platformService;
        private readonly IMvxLog _mvxLog;

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

        public MvxAsyncCommand LoadPublicationsCommand => new MvxAsyncCommand(OnLoadPublicationsAsync);

        public MvxAsyncCommand<PublicationItemViewModel> SelectItemCommand => new MvxAsyncCommand<PublicationItemViewModel>((item) => NavigationService.ShowDetailsPublicationView());

        public PublicationsViewModel(
            INavigationService navigationService,
            IDialogService dialogService,
            IApiService apiService,
            IPlatformService platformService,
            IMvxLog mvxLog) : base(navigationService)
        {
            _dialogService = dialogService;
            _apiService = apiService;
            _platformService = platformService;
            _mvxLog = mvxLog;
        }

        public override Task Initialize()
        {
            base.Initialize();

            ActiveFilterName = Resources.Publication_Tab_Filter_Day;

            LoadPublicationsCommand.ExecuteAsync().FireAndForget();

            return Task.CompletedTask;
        }

        private async Task OnOpenFilterAsync(CancellationToken arg)
        {
            var selectedFilter = await _dialogService.ShowMenuDialogAsync(new[]
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

        private async Task OnLoadPublicationsAsync()
        {
            try
            {
                IsBusy = true;

                var videoBundle = await _apiService.GetVideoFeedAsync();

                if (videoBundle?.Data?.Count > 0)
                {
                    Items.Clear();
                }

                var publicationViewModels = videoBundle.Data.Select(x =>
                    new PublicationItemViewModel(
                        NavigationService,
                        _dialogService,
                        _platformService,
                        "Name one",
                        "https://images.pexels.com/photos/2092709/pexels-photo-2092709.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500",
                        x.Title,
                        "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/WeAreGoingOnBullrun.mp4",
                        x.ViewsCount,
                        new DateTime(2018, 4, 24),
                        x.RepostsCount,
                        x.ShareUri));

                //Items.AddRange(publicationViewModels);

                Items.Add(publicationViewModels.ToList()[0]);

                //Items.Add(publicationViewModels.ToList()[1]);
                //Items.Add(publicationViewModels.ToList()[2]);
                //Items.Add(publicationViewModels.ToList()[1]);
                //Items.Add(publicationViewModels.ToList()[2]);
            }
            catch (Exception ex)
            {
                _dialogService.ShowToast($"Exception with registration {ex.Message}");
                _mvxLog.ErrorException($"[{nameof(PublicationsViewModel)}]", ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
