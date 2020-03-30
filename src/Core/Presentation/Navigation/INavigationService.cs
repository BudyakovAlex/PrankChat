using System.Threading.Tasks;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.Navigation.Results;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.Navigation
{
    public interface INavigationService
    {
        Task AppStart();

        Task ShowLoginView();

        Task ShowPasswordRecoveryView();

        Task ShowFinishPasswordRecoveryView();

        Task ShowRegistrationView();

        Task ShowRegistrationSecondStepView(string email);

        Task ShowRegistrationThirdStepView();

        Task ShowMainView();

        Task ShowMainViewContent();

        Task ShowCommentsView();

        Task ShowNotificationView();

        Task ShowSearchView();

        Task<OrderDetailsResult> ShowOrderDetailsView(int orderId);

        Task ShowFullScreenVideoView(FullScreenVideoParameter fullScreenVideoParameter);

        Task ShowDetailsPublicationView();

        Task ShowCompetitionDetailsView(CompetitionDataModel competition);

        Task ShowCompetitionPrizePoolView(CompetitionDataModel competition);

        Task ShowCompetitionRulesView(string content);

        Task ShowWithdrawalView();

        Task ShowRefillView();

        Task ShowCashboxContent();

        Task<ImageCropPathResult> ShowImageCropView(string filePath);

        Task Logout();

        Task<bool> ShowUpdateProfileView();

        Task<bool> CloseView(BaseViewModel viewModel);

        Task ShowProfileUser(int idUser);

        Task ShowWebView(string url);

        #region Dialogs

        Task ShowShareDialog(ShareDialogParameter parameter);

        Task<ArrayDialogResult> ShowArrayDialog(ArrayDialogParameter parameter);

        #endregion
    }
}
