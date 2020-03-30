using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Competition
{
    public class CompetitionsViewModel : BaseViewModel
    {
        private readonly IMvxMessenger _mvxMessenger;

        public CompetitionsViewModel(IMvxMessenger mvxMessenger,
                                     INavigationService navigationService,
                                     IErrorHandleService errorHandleService,
                                     IApiService apiService,
                                     IDialogService dialogService,
                                     ISettingsService settingsService) : base(navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
            _mvxMessenger = mvxMessenger;
        }

        public MvxObservableCollection<CompetitionsSectionViewModel> Items { get; set; } = new MvxObservableCollection<CompetitionsSectionViewModel>();

        public override async Task Initialize()
        {
            var competitionsPage = await ApiService.GetCompetitionsAsync(1, 100);

            var sections = competitionsPage.Items.GroupBy(competition => competition.GetPhase())
                                                 .Select(group => new CompetitionsSectionViewModel(_mvxMessenger, NavigationService, group.Key, group.ToList()))
                                                 .ToList();
            Items.SwitchTo(sections);
        }
    }
}