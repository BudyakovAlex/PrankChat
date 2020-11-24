using MvvmCross.Commands;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Commands;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Order.Sections
{
    public class OrderDetailsCustomerSectionViewModel : BaseItemViewModel
    {
        private readonly ISettingsService _settingsService;
        private readonly INavigationService _navigationService;

        private readonly UserDataModel _customer;

        public OrderDetailsCustomerSectionViewModel(ISettingsService settingsService,
                                                    INavigationService navigationService,
                                                    UserDataModel customer)
        {
            _settingsService = settingsService;
            _navigationService = navigationService;
            _customer = customer;

            OpenCustomerProfileCommand = new MvxRestrictedAsyncCommand(OpenCustomerProfileAsync, restrictedCanExecute: () => settingsService.User != null, handleFunc: navigationService.ShowLoginView);
        }

        public string ProfilePhotoUrl => _customer?.Avatar;

        public string ProfileName => _customer?.Login;

        public string ProfileShortName => ProfileName?.ToShortenName();

        public bool IsUserCustomer => _customer?.Id == _settingsService.User?.Id;

        public IMvxAsyncCommand OpenCustomerProfileCommand { get; }

        private Task OpenCustomerProfileAsync()
        {
            if (_customer?.Id is null ||
                _customer.Id == _settingsService.User.Id)
            {
                return Task.CompletedTask;
            }

            return _navigationService.ShowUserProfile(_customer.Id);
        }
    }
}