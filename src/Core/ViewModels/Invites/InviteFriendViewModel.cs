using MvvmCross.Commands;
using Newtonsoft.Json;
using PrankChat.Mobile.Core.Common;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Managers.Users;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Providers.Configuration;
using PrankChat.Mobile.Core.ViewModels.Abstract;
using System;
using System.Buffers.Text;
using System.IO;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ViewModels.Invites
{
    public sealed class InviteFriendViewModel : BasePageViewModel
    {
        private const int EarningsPercent = 10;

        private readonly IUsersManager _usersManager;
        private readonly IEnvironmentConfigurationProvider _environmentConfigurationProvider;

        public InviteFriendViewModel(IUsersManager usersManager, IEnvironmentConfigurationProvider environmentConfigurationProvider)
        {
            _usersManager = usersManager;
            _environmentConfigurationProvider = environmentConfigurationProvider;
            SendCommand = this.CreateCommand(SendAsync, () => !HasError && Email.IsNotNullNorEmpty());
        }

        public IMvxAsyncCommand SendCommand { get; }

        public AttributedText[] Description { get; } = CreateDescription();

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value, () =>
            {
                ErrorMessage = null;
                SendCommand.RaiseCanExecuteChanged();
            });
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value, () =>
            {
                RaisePropertyChanged(nameof(HasError));
                SendCommand.RaiseCanExecuteChanged();
            });
        }

        public bool HasError => ErrorMessage != null;

        private async Task SendAsync()
        {
            var response = await _usersManager.InviteFriendAsync(Email);
            if (!response.IsSuccessful)
            {
                ErrorMessage = response.Message;
                return;
            }

            var urlBody = JsonConvert.SerializeObject(new
            {
                login = UserSessionProvider.User.Login,
                email = Email
            });

            var url = Path.Combine(
                _environmentConfigurationProvider.Environment.SiteUrl,
                $"{RestConstants.InviteConfirm}{urlBody.ToBase64()}");

            var body = string.Format(Resources.InviteEmailBodyTemplate, url);
            await Xamarin.Essentials.Email.ComposeAsync(Resources.InviteEmailSubject, body, Email);

            var message = $"{Email} {Resources.InviteFriendSuccessfulMessage}";
            UserInteraction.ShowToast(message, ToastType.Positive);

            Email = null;
        }

        private static AttributedText[] CreateDescription()
        {
            return new AttributedText[]
            {
                new AttributedText(Resources.InviteFriendDescriptionPart1),
                new AttributedText($" {EarningsPercent}% ", textSize: 20, textColor: Colors.ActionPrimaryNormal),
                new AttributedText(Resources.InviteFriendDescriptionPart2)
            };
        }
    }
}
