using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Providers;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Competition
{
    public class CompetitionsViewModel : BaseViewModel
    {
        private readonly IMvxMessenger _mvxMessenger;
        private readonly IWalkthroughsProvider _walkthroughsProvider;

        public CompetitionsViewModel(IMvxMessenger mvxMessenger,
                                     INavigationService navigationService,
                                     IErrorHandleService errorHandleService,
                                     IApiService apiService,
                                     IDialogService dialogService,
                                     ISettingsService settingsService,
                                     IWalkthroughsProvider walkthroughsProvider) : base(navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
            _mvxMessenger = mvxMessenger;
            _walkthroughsProvider = walkthroughsProvider;

            LoadDataCommand = new MvxAsyncCommand(LoadDataAsync);
            ShowWalkthrouthCommand = new MvxAsyncCommand(ShowWalkthrouthAsync);
        }

        public IMvxAsyncCommand LoadDataCommand { get; }

        public IMvxAsyncCommand ShowWalkthrouthCommand { get; }

        public MvxObservableCollection<CompetitionsSectionViewModel> Items { get; set; } = new MvxObservableCollection<CompetitionsSectionViewModel>();

        public override Task Initialize()
        {
            return LoadDataCommand.ExecuteAsync();
        }

        private Task ShowWalkthrouthAsync()
        {
            return _walkthroughsProvider.ShowWalthroughAsync<CompetitionsViewModel>();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                IsBusy = true;
                var competitionsPage = await ApiService.GetCompetitionsAsync(1, 100);

                var sections = competitionsPage.Items.GroupBy(competition => competition.GetPhase())
                                                     .Select(group => new CompetitionsSectionViewModel(IsUserSessionInitialized, _mvxMessenger, NavigationService, group.Key, group.ToList()))
                                                     .OrderBy(item => item.Phase)
                                                     .ToList();
                Items.SwitchTo(sections);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}