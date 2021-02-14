using MvvmCross.Commands;
using PrankChat.Mobile.Core.Commands;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order.Sections.Abstract;
using PrankChat.Mobile.Core.Providers.UserSession;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Order.Sections
{
    public class OrderDetailsExecutorSectionViewModel : BaseOrderDetailsSectionViewModel
    {
        private readonly IUserSessionProvider _userSessionProvider;
        private readonly INavigationService _navigationService;

        public OrderDetailsExecutorSectionViewModel(IUserSessionProvider userSessionProvider, INavigationService navigationService)
        {
            _userSessionProvider = userSessionProvider;
            _navigationService = navigationService;

            OpenExecutorProfileCommand = new MvxRestrictedAsyncCommand(OpenExecutorProfileAsync, restrictedCanExecute: () => userSessionProvider.User != null, handleFunc: navigationService.ShowLoginView);
        }

        public string ExecutorPhotoUrl => Order?.Executor?.Avatar;

        public string ExecutorName => Order?.Executor?.Login;

        public string ExecutorShortName => ExecutorName.ToShortenName();

        public bool IsExecutorAvailable => Order?.Executor != null && Order?.Executor?.Id != _userSessionProvider.User?.Id;

        public bool IsUserExecutor => Order?.Executor?.Id == _userSessionProvider.User?.Id;

        public IMvxAsyncCommand OpenExecutorProfileCommand { get; }

        private Task OpenExecutorProfileAsync()
        {
            if (Order?.Executor?.Id is null ||
                Order?.Executor.Id == _userSessionProvider.User.Id)
            {
                return Task.CompletedTask;
            }

            return _navigationService.ShowUserProfile(Order.Executor.Id);
        }
    }
}