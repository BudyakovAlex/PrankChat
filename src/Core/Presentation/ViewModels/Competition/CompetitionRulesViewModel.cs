using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Competition
{
    public class CompetitionRulesViewModel : BaseViewModel, IMvxViewModel<string>
    {
        public CompetitionRulesViewModel(INavigationService navigationService,
                                         IErrorHandleService errorHandleService,
                                         IApiService apiService,
                                         IDialogService dialogService,
                                         ISettingsService settingsService) : base(navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
        }

        public string HtmlContent { get; private set; }

        public void Prepare(string parameter)
        {
            HtmlContent = parameter;
        }
    }
}