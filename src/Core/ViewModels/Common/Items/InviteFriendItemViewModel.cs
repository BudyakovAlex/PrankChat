using MvvmCross.Commands;
using PrankChat.Mobile.Core.Common;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Providers.UserSession;
using PrankChat.Mobile.Core.ViewModels.Abstract;
using PrankChat.Mobile.Core.ViewModels.Invites;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.ViewModels.Common.Items
{
    public sealed class InviteFriendItemViewModel : BaseViewModel
    {
        private readonly IUserSessionProvider _userSessionProvider;

        public InviteFriendItemViewModel(IUserSessionProvider userSessionProvider)
        {
            _userSessionProvider = userSessionProvider;

            InviteFriendCommand = this.CreateCommand(InviteFriendAsync, () => CanInviteFriend);
        }

        public IMvxAsyncCommand InviteFriendCommand { get; }

        public bool CanInviteFriend => _userSessionProvider.User != null;

        private bool _hasBadge;
        public bool HasBadge
        {
            get => _hasBadge;
            private set => SetProperty(ref _hasBadge, value);
        }

        public void RefreshHasInviteFriendBadge()
        {
            var ticks = Preferences.Get(Constants.Keys.InviteFriendLastNavigationTicks, 0L);
            if (ticks == 0)
            {
                HasBadge = true;
                return;
            }

            var dateTime = new DateTime(ticks, DateTimeKind.Utc);
            var now = DateTime.UtcNow;
            var difference = now - dateTime;
            HasBadge = difference.TotalDays >= 1;
        }

        private async Task InviteFriendAsync()
        {
            Preferences.Set(Constants.Keys.InviteFriendLastNavigationTicks, DateTime.UtcNow.Ticks);
            await NavigationManager.NavigateAsync<InviteFriendViewModel>();
        }
    }
}
