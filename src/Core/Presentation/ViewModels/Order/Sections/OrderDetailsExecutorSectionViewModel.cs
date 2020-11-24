using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Order.Sections
{
    public class OrderDetailsExecutorSectionViewModel : BaseItemViewModel
    {
        private readonly ISettingsService _settingsService;

        private readonly UserDataModel _executor;

        public OrderDetailsExecutorSectionViewModel(ISettingsService settingsService, UserDataModel executor)
        {
            _settingsService = settingsService;
            _executor = executor;
        }

        public string ExecutorPhotoUrl => _executor?.Avatar;

        public string ExecutorName => _executor?.Login;

        public string ExecutorShortName => ExecutorName.ToShortenName();

        public bool IsExecutorAvailable => _executor != null && _executor?.Id != _settingsService.User?.Id;

        public bool IsUserExecutor => _executor?.Id == _settingsService.User?.Id;
    }
}