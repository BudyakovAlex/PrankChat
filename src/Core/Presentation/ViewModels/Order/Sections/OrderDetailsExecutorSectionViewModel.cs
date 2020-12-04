using MvvmCross.Commands;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Commands;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Order.Sections.Abstract;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Order.Sections
{
    public class OrderDetailsExecutorSectionViewModel : BaseOrderDetailsSectionViewModel
    {
        private readonly ISettingsService _settingsService;
        private readonly INavigationService _navigationService;

        public OrderDetailsExecutorSectionViewModel(ISettingsService settingsService, INavigationService navigationService)
        {
            _settingsService = settingsService;
            _navigationService = navigationService;

            OpenExecutorProfileCommand = new MvxRestrictedAsyncCommand(OpenExecutorProfileAsync, restrictedCanExecute: () => settingsService.User != null, handleFunc: navigationService.ShowLoginView);
        }

        public string ExecutorPhotoUrl => Order?.Executor?.Avatar;

        public string ExecutorName => Order?.Executor?.Login;

        public string ExecutorShortName => ExecutorName.ToShortenName();

        public bool IsExecutorAvailable => Order?.Executor != null && Order?.Executor?.Id != _settingsService.User?.Id;

        public bool IsUserExecutor => Order?.Executor?.Id == _settingsService.User?.Id;

        public IMvxAsyncCommand OpenExecutorProfileCommand { get; }

        private Task OpenExecutorProfileAsync()
        {
            if (Order?.Executor?.Id is null ||
                Order?.Executor.Id == _settingsService.User.Id)
            {
                return Task.CompletedTask;
            }

            return _navigationService.ShowUserProfile(Order.Executor.Id);
        }
    }
}