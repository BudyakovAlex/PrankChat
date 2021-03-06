using MvvmCross.Commands;
using PrankChat.Mobile.Core.Commands;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order.Sections.Abstract;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile;
using PrankChat.Mobile.Core.Providers.UserSession;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Order.Sections
{
    public class OrderDetailsCustomerSectionViewModel : BaseOrderDetailsSectionViewModel
    {
        private readonly IUserSessionProvider _userSessionProvider;
        private readonly INavigationService _navigationService;

        public OrderDetailsCustomerSectionViewModel(IUserSessionProvider userSessionProvider, INavigationService navigationService)
        {
            _userSessionProvider = userSessionProvider;
            _navigationService = navigationService;

            OpenCustomerProfileCommand = new MvxRestrictedAsyncCommand(OpenCustomerProfileAsync, restrictedCanExecute: () => userSessionProvider.User != null, handleFunc: navigationService.ShowLoginView);
        }

        public string ProfilePhotoUrl => Order?.Customer?.Avatar;

        public string ProfileName => Order?.Customer?.Login;

        public string ProfileShortName => ProfileName?.ToShortenName();

        public bool IsUserCustomer =>  Order?.Customer?.Id == _userSessionProvider.User?.Id;

        public IMvxAsyncCommand OpenCustomerProfileCommand { get; }

        private Task OpenCustomerProfileAsync()
        {
            if (Order?.Customer?.Id is null ||
                Order?.Customer.Id == _userSessionProvider.User.Id)
            {
                return Task.CompletedTask;
            }

            if (!Connectivity.NetworkAccess.HasConnection())
            {
                return Task.CompletedTask;
            }

            return NavigationManager.NavigateAsync<UserProfileViewModel, int, bool>(Order.Customer.Id);
        }
    }
}