using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.ViewModels;
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

        Task ShowMaintananceView(string url);

        Task ShowOnBoardingView();

        Task ShowMainViewContent();

        Task<int> ShowCommentsView(int videoId);

        Task ShowNotificationView();

        Task<bool> ShowSubscriptionsView(SubscriptionsNavigationParameter navigationParameter);

        Task ShowSearchView();

        Task<OrderDetailsResult> ShowOrderDetailsView(int orderId, List<FullScreenVideo> fullScreenVideos, int currentIndex);

        Task<bool> ShowFullScreenVideoView(FullScreenVideoParameter fullScreenVideoParameter);

        Task ShowDetailsPublicationView();

        Task<bool> ShowCompetitionDetailsView(Competition competition);

        Task ShowCompetitionPrizePoolView(Competition competition);

        Task ShowCompetitionRulesView(string content);

        Task<bool> ShowWithdrawalView();

        Task<bool> ShowRefillView();

        Task ShowCashboxContent();

        Task<ImageCropPathResult> ShowImageCropView(string filePath);

        Task Logout();

        Task<bool> ShowUpdateProfileView();

        Task<bool> CloseView(BasePageViewModel viewModel);

        Task<bool> CloseViewWithResult<TResult>(IMvxViewModelResult<TResult> viewModel, TResult result);

        Task<bool> ShowUserProfile(int userId);

        Task ShowWebView(string url);

        Task ShowWalthroughView(string title, string description);

        Task AppStartFromNotification(int orderId);

        #region Dialogs

        Task ShowShareDialog(ShareDialogParameter parameter);

        Task<ArrayDialogResult> ShowArrayDialog(ArrayDialogParameter parameter);

        #endregion
    }
}
