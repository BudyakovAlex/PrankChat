using MvvmCross.Commands;
using PrankChat.Mobile.Core.Common;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.ViewModels.Abstract;
using PrankChat.Mobile.Core.ViewModels.Invites;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.ViewModels.Common.Items
{
    public sealed class InviteFriendItemViewModel : BaseViewModel
    {
        public InviteFriendItemViewModel()
        {
            InviteFriendCommand = this.CreateCommand(InviteFriendAsync);
        }

        public IMvxAsyncCommand InviteFriendCommand { get; }

        private bool _hasInviteFriendBadge;
        public bool HasInviteFriendBadge
        {
            get => _hasInviteFriendBadge;
            private set => SetProperty(ref _hasInviteFriendBadge, value);
        }

        public void UpdateHasInviteFriendBadgeIfNeeded()
        {
            if (HasInviteFriendBadge)
            {
                return;
            }

            var ticks = Preferences.Get(Constants.Keys.InviteFriendLastNavigationTicks, 0L);
            if (ticks == 0)
            {
                HasInviteFriendBadge = true;
                return;
            }

            var dateTime = new DateTime(ticks, DateTimeKind.Utc);
            var now = DateTime.UtcNow;
            var difference = now - dateTime;
            if (difference.TotalDays >= 1)
            {
                HasInviteFriendBadge = true;
            }
        }

        private async Task InviteFriendAsync()
        {
            Preferences.Set(Constants.Keys.InviteFriendLastNavigationTicks, DateTime.UtcNow.Ticks);
            await NavigationManager.NavigateAsync<InviteFriendViewModel>();
        }
    }
}
