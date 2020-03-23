using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Models.Api;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Competition
{
    public class CompetitionDetailsViewModel : BaseViewModel, IMvxViewModel<CompetitionApiModel>
    {
        private CompetitionApiModel _competition;

        public CompetitionDetailsViewModel(INavigationService navigationService,
                                           IErrorHandleService errorHandleService,
                                           IApiService apiService,
                                           IDialogService dialogService,
                                           ISettingsService settingsService) : base(navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
        }

        public void Prepare(CompetitionApiModel parameter)
        {
            _competition = parameter;
        }
    }
}