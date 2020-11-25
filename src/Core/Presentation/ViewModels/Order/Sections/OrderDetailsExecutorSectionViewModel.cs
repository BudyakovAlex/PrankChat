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
    public class OrderDetailsExecutorSectionViewModel : BaseItemViewModel
    {
        private readonly ISettingsService _settingsService;
        private readonly INavigationService _navigationService;
        private readonly UserDataModel _executor;

        public OrderDetailsExecutorSectionViewModel(ISettingsService settingsService, INavigationService navigationService, UserDataModel executor)
        {
            _settingsService = settingsService;
            _navigationService = navigationService;
            _executor = executor;

            OpenExecutorProfileCommand = new MvxRestrictedAsyncCommand(OpenExecutorProfileAsync, restrictedCanExecute: () => settingsService.User != null, handleFunc: navigationService.ShowLoginView);
        }

        public string ExecutorPhotoUrl => _executor?.Avatar;

        public string ExecutorName => _executor?.Login;

        public string ExecutorShortName => ExecutorName.ToShortenName();

        public bool IsExecutorAvailable => _executor != null && _executor?.Id != _settingsService.User?.Id;

        public bool IsUserExecutor => _executor?.Id == _settingsService.User?.Id;

        public IMvxAsyncCommand OpenExecutorProfileCommand { get; }

        private Task OpenExecutorProfileAsync()
        {
            if (_executor?.Id is null ||
                _executor.Id == _settingsService.User.Id)
            {
                return Task.CompletedTask;
            }

            return _navigationService.ShowUserProfile(_executor.Id);
        }
    }
}