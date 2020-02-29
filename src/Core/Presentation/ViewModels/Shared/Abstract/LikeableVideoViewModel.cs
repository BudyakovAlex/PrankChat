using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Commands;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Commands;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Shared.Abstract
{
    public abstract class LikeableViewModel : BaseViewModel
    {
        private CancellationTokenSource _cancellationSendingLikeTokenSource;

        public LikeableViewModel(INavigationService navigationService,
                                 IErrorHandleService errorHandleService,
                                 IApiService apiService,
                                 IDialogService dialogService,
                                 ISettingsService settingsService) : base(navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
            LikeCommand = new MvxRestrictedCommand(OnLike, restrictedExecute: () => IsUserSessionInitialized, handleFunc: NavigationService.ShowLoginView);
        }

        public IMvxCommand LikeCommand { get; }

        public int VideoId { get; set; }

        private bool _isLiked;
        public bool IsLiked
        {
            get => _isLiked;
            set => SetProperty(ref _isLiked, value);
        }

        protected long? NumberOfLikes { get; set; }

        protected virtual void OnLikeChanged()
        {
        }

        private void OnLike()
        {
            IsLiked = !IsLiked;

            var totalLikes = IsLiked ? NumberOfLikes + 1 : NumberOfLikes - 1;
            NumberOfLikes = totalLikes > 0 ? totalLikes : 0;

            OnLikeChanged();
            SendLikeAsync().FireAndForget();
        }

        private async Task SendLikeAsync()
        {
            _cancellationSendingLikeTokenSource?.Cancel();
            if (_cancellationSendingLikeTokenSource == null)
            {
                _cancellationSendingLikeTokenSource = new CancellationTokenSource();
            }

            try
            {
                await ApiService.SendLikeAsync(VideoId, IsLiked, _cancellationSendingLikeTokenSource.Token);
            }
            finally
            {
                _cancellationSendingLikeTokenSource?.Dispose();
                _cancellationSendingLikeTokenSource = null;
            }
        }
    }
}