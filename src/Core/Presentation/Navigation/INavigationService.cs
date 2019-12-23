using System;
using System.Threading.Tasks;
using PrankChat.Mobile.Core.Presentation.ViewModels;

namespace PrankChat.Mobile.Core.Presentation.Navigation
{
    public interface INavigationService
    {
        Task AppStart();

        Task ShowLoginView();

        Task ShowPasswordRecoveryView();

        Task ShowFinishPasswordRecoveryView();

        Task ShowRegistrationView();

        Task ShowRegistrationSecondStepView();

        Task ShowRegistrationThirdStepView();

        Task ShowMainView();

        Task ShowMainViewContent();

        Task ShowCommentsView();

        Task ShowNotificationView();

        Task ShowSearchView();

        Task ShowDetailsOrderView();

        Task ShowDetailsPublicationView();
        Task ShowWithdrawalView();

        Task ShowRefillView();

        Task ShowCashboxContent();

        Task<bool> CloseView(BaseViewModel viewModel);
    }
}
