using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Data.Enums;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.Navigation.Results;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Presentation.ViewModels.Comment;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition;
using PrankChat.Mobile.Core.Presentation.ViewModels.Dialogs;
using PrankChat.Mobile.Core.Presentation.ViewModels.Notification;
using PrankChat.Mobile.Core.Presentation.ViewModels.Onboarding;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order;
using PrankChat.Mobile.Core.Presentation.ViewModels.PasswordRecovery;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile.Cashbox;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using PrankChat.Mobile.Core.Presentation.ViewModels.Subscriptions;
using PrankChat.Mobile.Core.Presentation.ViewModels.Video;
using PrankChat.Mobile.Core.Presentation.ViewModels.Walthroughs;
using PrankChat.Mobile.Core.Providers.UserSession;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.Presentation.Navigation
{
    //TODO: get rid of current service, use MvxNavigationService directly
    public class NavigationService : INavigationService
    {
        private readonly IMvxNavigationService _mvxNavigationService;
        private readonly IUserSessionProvider _userSessionProvider;

        private int? _orderId;

        public NavigationService(IMvxNavigationService mvxNavigationService,
                                 IUserSessionProvider userSessionProvider)
        {
            _mvxNavigationService = mvxNavigationService;
            _userSessionProvider = userSessionProvider;
        }

        public Task ShowMaintananceView(string url)
        {
            return _mvxNavigationService.Navigate<MaintananceViewModel, string>(url);
        }

        public Task ShowCashboxView()
        {
            return _mvxNavigationService.Navigate<CashboxViewModel>();
        }

        public Task ShowLoginView()
        {
            return _mvxNavigationService.Navigate<LoginViewModel>();
        }

        public Task ShowPasswordRecoveryView()
        {
            return _mvxNavigationService.Navigate<PasswordRecoveryViewModel>();
        }

        public Task ShowFinishPasswordRecoveryView()
        {
            return _mvxNavigationService.Navigate<FinishPasswordRecoveryViewModel>();
        }

        public Task<bool> ShowSubscriptionsView(SubscriptionsNavigationParameter navigationParameter)
        {
            return _mvxNavigationService.Navigate<SubscriptionsViewModel, SubscriptionsNavigationParameter, bool>(navigationParameter);
        }

        public Task ShowRegistrationView()
        {
            return _mvxNavigationService.Navigate<RegistrationViewModel>();
        }

        public Task ShowRegistrationSecondStepView(string email)
        {
            var parameter = new RegistrationNavigationParameter(email);
            return _mvxNavigationService.Navigate<RegistrationSecondStepViewModel, RegistrationNavigationParameter>(parameter);
        }

        public Task ShowRegistrationThirdStepView()
        {
            return _mvxNavigationService.Navigate<RegistrationThirdStepViewModel>();
        }

        public Task ShowMainView()
        {
            return _mvxNavigationService.Navigate<MainViewModel>();
        }

        public Task ShowOnBoardingView()
        {
            return _mvxNavigationService.Navigate<OnboardingViewModel>();
        }

        public Task ShowMainViewContent()
        {
            return Task.WhenAll(_mvxNavigationService.Navigate<PublicationsViewModel>(),
                                _mvxNavigationService.Navigate<CompetitionsViewModel>(),
                                _mvxNavigationService.Navigate<CreateOrderViewModel>(),
                                _mvxNavigationService.Navigate<OrdersViewModel>(),
                                _mvxNavigationService.Navigate<ProfileViewModel>());
        }

        public Task ShowNotificationView()
        {
            return _mvxNavigationService.Navigate<NotificationViewModel>();
        }

        public Task<int> ShowCommentsView(int videoId)
        {
            return _mvxNavigationService.Navigate<CommentsViewModel, int, int>(videoId);
        }

        public Task ShowCashboxContent()
        {
            return Task.WhenAll(_mvxNavigationService.Navigate<RefillViewModel>(), _mvxNavigationService.Navigate<WithdrawalViewModel>());
        }

        public Task<bool> CloseView(BasePageViewModel viewModel)
        {
            return _mvxNavigationService.Close(viewModel);
        }

        public Task ShowSearchView()
        {
            return _mvxNavigationService.Navigate<SearchViewModel>();
        }

        public Task<OrderDetailsResult> ShowOrderDetailsView(int orderId, List<FullScreenVideo> fullScreenVideos, int currentIndex)
        {
            var parameter = new OrderDetailsNavigationParameter(orderId, fullScreenVideos, currentIndex);
            return _mvxNavigationService.Navigate<OrderDetailsViewModel, OrderDetailsNavigationParameter, OrderDetailsResult>(parameter);
        }

        public Task<bool> ShowFullScreenVideoView(FullScreenVideoParameter fullScreenVideoParameter)
        {
            if (fullScreenVideoParameter.Videos.Count == 0)
            {
                return Task.FromResult(false);
            }

            return _mvxNavigationService.Navigate<FullScreenVideoViewModel, FullScreenVideoParameter, bool>(fullScreenVideoParameter);
        }

        public Task ShowDetailsPublicationView()
        {
            return _mvxNavigationService.Navigate<PublicationDetailsViewModel>();
        }

        public Task<bool> ShowWithdrawalView()
        {
            var navigationParameter = new CashboxTypeNavigationParameter(CashboxType.Withdrawal);
            return _mvxNavigationService.Navigate<CashboxViewModel, CashboxTypeNavigationParameter, bool>(navigationParameter);
        }

        public Task<bool> ShowRefillView()
        {
            var navigationParameter = new CashboxTypeNavigationParameter(CashboxType.Refill);
            return _mvxNavigationService.Navigate<CashboxViewModel, CashboxTypeNavigationParameter, bool>(navigationParameter);
        }

        public async Task<bool> ShowUpdateProfileView()
        {
            var result = await _mvxNavigationService.Navigate<ProfileUpdateViewModel, ProfileUpdateResult>();
            return (result?.IsProfileUpdated ?? false) || (result?.IsAvatarUpdated ?? false);
        }

        public Task Logout()
        {
            return ShowLoginView();
        }

        public Task ShowWebView(string url)
        {
            var parameter = new WebViewNavigationParameter(url);
            return _mvxNavigationService.Navigate<WebViewModel, WebViewNavigationParameter>(parameter);
        }

        public Task<bool> ShowUserProfile(int userId)
        {
            if (!Connectivity.NetworkAccess.HasConnection())
            {
                return Task.FromResult(false);
            }

            return _mvxNavigationService.Navigate<UserProfileViewModel, int, bool>(userId);
        }

        public Task<ImageCropPathResult> ShowImageCropView(string filePath)
        {
            var parameter = new ImagePathNavigationParameter(filePath);
            return _mvxNavigationService.Navigate<ImageCropViewModel, ImagePathNavigationParameter, ImageCropPathResult>(parameter);
        }

        public Task AppStartFromNotification(int orderId)
        {
            _orderId = orderId;
            if (_userSessionProvider.User != null)
            {
                _mvxNavigationService.AfterNavigate += NavigatenAfterMainViewByPushNotification;
                return ShowMainView();
            }

            return ShowLoginView();
        }

        private void NavigatenAfterMainViewByPushNotification(object sender, MvvmCross.Navigation.EventArguments.IMvxNavigateEventArgs e)
        {
            _mvxNavigationService.AfterNavigate -= NavigatenAfterMainViewByPushNotification;

            if (_orderId == null)
            {
                return;
            }

            if (e.ViewModel is MainViewModel mainViewModel)
            {
                ShowOrderDetailsView(_orderId.Value, null, 0).FireAndForget();
            }
        }

        #region Dialogs

        public Task ShowShareDialog(ShareDialogParameter parameter)
        {
            return _mvxNavigationService.Navigate<ShareDialogViewModel, ShareDialogParameter>(parameter);
        }

        public Task<ArrayDialogResult> ShowArrayDialog(ArrayDialogParameter parameter)
        {
            return _mvxNavigationService.Navigate<ArrayDialogViewModel, ArrayDialogParameter, ArrayDialogResult>(parameter);
        }

        public Task<bool> ShowCompetitionDetailsView(Competition competition)
        {
            return _mvxNavigationService.Navigate<CompetitionDetailsViewModel, Competition, bool>(competition);
        }

        public Task ShowCompetitionPrizePoolView(Competition competition)
        {
            return _mvxNavigationService.Navigate<CompetitionPrizePoolViewModel, Competition>(competition);
        }

        public Task ShowCompetitionRulesView(string content)
        {
            return _mvxNavigationService.Navigate<CompetitionRulesViewModel, string>(content);
        }

        public Task ShowWalthroughView(string title, string description)
        {
            var parameters = new WalthroughNavigationParameter(title, description);
            return _mvxNavigationService.Navigate<WalthroughViewModel, WalthroughNavigationParameter>(parameters);
        }

        public Task<bool> CloseViewWithResult<TResult>(IMvxViewModelResult<TResult> viewModel, TResult result)
        {
            return _mvxNavigationService.Close(viewModel, result);
        }

        #endregion
    }
}
