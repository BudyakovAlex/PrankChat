using MvvmCross.Commands;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Commands;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order.Sections.Abstract;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Order.Sections
{
    public class OrderDetailsCustomerSectionViewModel : BaseOrderDetailsSectionViewModel
    {
        private readonly ISettingsService _settingsService;
        private readonly INavigationService _navigationService;

        public OrderDetailsCustomerSectionViewModel(ISettingsService settingsService, INavigationService navigationService)
        {
            _settingsService = settingsService;
            _navigationService = navigationService;

            OpenCustomerProfileCommand = new MvxRestrictedAsyncCommand(OpenCustomerProfileAsync, restrictedCanExecute: () => settingsService.User != null, handleFunc: navigationService.ShowLoginView);
        }

        public string ProfilePhotoUrl => Order?.Customer?.Avatar;

        public string ProfileName => Order?.Customer?.Login;

        public string ProfileShortName => ProfileName?.ToShortenName();

        public bool IsUserCustomer =>  Order?.Customer?.Id == _settingsService.User?.Id;

        public IMvxAsyncCommand OpenCustomerProfileCommand { get; }

        private Task OpenCustomerProfileAsync()
        {
            if (Order?.Customer?.Id is null ||
                Order?.Customer.Id == _settingsService.User.Id)
            {
                return Task.CompletedTask;
            }

            return _navigationService.ShowUserProfile(Order.Customer.Id);
        }
    }
}