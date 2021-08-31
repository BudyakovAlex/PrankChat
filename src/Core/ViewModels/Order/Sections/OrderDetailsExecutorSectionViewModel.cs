using MvvmCross.Commands;
using PrankChat.Mobile.Core.Commands;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.ViewModels.Order.Sections.Abstract;
using PrankChat.Mobile.Core.ViewModels.Profile;
using PrankChat.Mobile.Core.ViewModels.Registration;
using PrankChat.Mobile.Core.Providers.UserSession;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.ViewModels.Order.Sections
{
    public class OrderDetailsExecutorSectionViewModel : BaseOrderDetailsSectionViewModel
    {
        private readonly IUserSessionProvider _userSessionProvider;

        public OrderDetailsExecutorSectionViewModel(IUserSessionProvider userSessionProvider)
        {
            _userSessionProvider = userSessionProvider;

            OpenExecutorProfileCommand = new MvxRestrictedAsyncCommand(
                OpenExecutorProfileAsync,
                restrictedCanExecute: () => userSessionProvider.User != null,
                handleFunc: NavigationManager.NavigateAsync<LoginViewModel>);
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

            if (!Connectivity.NetworkAccess.HasConnection())
            {
                return Task.CompletedTask;
            }

            return NavigationManager.NavigateAsync<UserProfileViewModel, int, bool>(Order.Executor.Id);
        }
    }
}