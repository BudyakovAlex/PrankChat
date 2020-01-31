using System.Threading.Tasks;
using MvvmCross.Navigation;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.ViewModels;
using PrankChat.Mobile.Core.Presentation.ViewModels.Notification;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order;
using PrankChat.Mobile.Core.Presentation.ViewModels.PasswordRecovery;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile.Cashbox;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication;
using PrankChat.Mobile.Core.Presentation.ViewModels.Rating;
using PrankChat.Mobile.Core.Presentation.ViewModels.Registration;
using PrankChat.Mobile.Core.Presentation.ViewModels.Comment;
using PrankChat.Mobile.Core.Presentation.ViewModels.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Presentation.Navigation.Results;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile;

namespace PrankChat.Mobile.Core.Presentation.Navigation
{
    public class NavigationService : INavigationService
    {
        private readonly IMvxNavigationService _mvxNavigationService;
        private readonly ISettingsService _settingsService;

        public NavigationService(IMvxNavigationService mvxNavigationService, ISettingsService settingsService)
        {
            _mvxNavigationService = mvxNavigationService;
            _settingsService = settingsService;
        }

        public async Task AppStart()
        {
            if (_settingsService.User == null)
                await ShowLoginView();
            else
                await ShowMainView();
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

        public Task ShowMainViewContent()
        {
            return Task.WhenAll(
                _mvxNavigationService.Navigate<PublicationsViewModel>(),
                _mvxNavigationService.Navigate<RatingViewModel>(),
                _mvxNavigationService.Navigate<CreateOrderViewModel>(),
                _mvxNavigationService.Navigate<OrdersViewModel>(),
                _mvxNavigationService.Navigate<ProfileViewModel>());
        }

        public Task ShowNotificationView()
        {
            return _mvxNavigationService.Navigate<NotificationViewModel>();
        }

        public Task ShowCommentsView()
        {
            return _mvxNavigationService.Navigate<CommentsViewModel>();
        }

        public Task ShowCashboxContent()
        {
            return Task.WhenAll(
                _mvxNavigationService.Navigate<RefillViewModel>(),
                           _mvxNavigationService.Navigate<WithdrawalViewModel>());
        }

        public Task<bool> CloseView(BaseViewModel viewModel)
        {
            return _mvxNavigationService.Close(viewModel);
        }

        public Task ShowSearchView()
        {
            return _mvxNavigationService.Navigate<SearchViewModel>();
        }

        public Task ShowDetailsOrderView(int orderId)
        {
            var parameter = new OrderDetailsNavigationParameter(orderId);
            return _mvxNavigationService.Navigate<OrderDetailsViewModel, OrderDetailsNavigationParameter>(parameter);
        }

        public Task ShowDetailsPublicationView()
        {
            return _mvxNavigationService.Navigate<PublicationDetailsViewModel>();
        }

        public Task ShowWithdrawalView()
        {
            var navigationParameter = new CashboxTypeNavigationParameter(CashboxTypeNavigationParameter.CashboxType.Withdrawal);
            return _mvxNavigationService.Navigate<CashboxViewModel, CashboxTypeNavigationParameter>(navigationParameter);
        }

        public Task ShowRefillView()
        {
            var navigationParameter = new CashboxTypeNavigationParameter(CashboxTypeNavigationParameter.CashboxType.Refill);
            return _mvxNavigationService.Navigate<CashboxViewModel, CashboxTypeNavigationParameter>(navigationParameter);
        }

        public Task ShowUpdateProfileView()
        {
            return _mvxNavigationService.Navigate<ProfileUpdateViewModel>();
        }

        public Task Logout()
        {
            return ShowLoginView();
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

        #endregion
    }
}
