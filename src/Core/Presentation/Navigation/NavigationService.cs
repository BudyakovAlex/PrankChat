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
using PrankChat.Mobile.Core.Presentation.ViewModels.Notification;
using PrankChat.Mobile.Core.Presentation.ViewModels.Comment;

namespace PrankChat.Mobile.Core.Presentation.Navigation
{
    public class NavigationService : INavigationService
    {
        private readonly IMvxNavigationService _mvxNavigationService;

        public NavigationService(IMvxNavigationService mvxNavigationService)
        {
            _mvxNavigationService = mvxNavigationService;
        }

        public Task AppStart()
        {
            return ShowLoginView();
            //return ShowMainView();
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

        public Task ShowRegistrationSecondStepView(RegistrationNavigationParameter parameter)
        {
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

        public Task ShowDetailsOrderView()
        {
            return _mvxNavigationService.Navigate<OrderDetailsViewModel>();
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
    }
}
