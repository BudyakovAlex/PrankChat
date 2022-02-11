using MvvmCross.Commands;
using PrankChat.Mobile.Core.Common;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Managers.Users;
using PrankChat.Mobile.Core.ViewModels.Abstract;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ViewModels.Invites
{
    public sealed class InviteFriendViewModel : BasePageViewModel
    {
        private const int EarningsPercent = 10;

        private readonly IUsersManager _usersManager;

        public InviteFriendViewModel(IUsersManager usersManager)
        {
            _usersManager = usersManager;

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
            await _usersManager.InviteFriendAsync(Email);
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
