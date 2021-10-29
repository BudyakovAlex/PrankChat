using MvvmCross.Commands;
using PrankChat.Mobile.Core.Commands;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Managers.Video;
using PrankChat.Mobile.Core.ViewModels.Abstract;
using PrankChat.Mobile.Core.ViewModels.Arbitration.Items;
using PrankChat.Mobile.Core.ViewModels.Common.Abstract;
using PrankChat.Mobile.Core.ViewModels.Profile;
using PrankChat.Mobile.Core.ViewModels.Registration;
using PrankChat.Mobile.Core.Providers.UserSession;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.ViewModels.Order.Items.Abstract
{
    public class BaseOrderItemViewModel : BaseViewModel
    {
        private readonly IUserSessionProvider _userSessionProvider;

        private readonly Func<BaseVideoItemViewModel[]> _getAllFullScreenVideosFunc;

        private readonly int? _customerId;

        public BaseOrderItemViewModel(
            IVideoManager videoManager,
            IUserSessionProvider userSessionProvider,
            Models.Data.Video video,
            int? customerId,
            Func<BaseVideoItemViewModel[]> getAllFullScreenVideosFunc)
        {
            _userSessionProvider = userSessionProvider;
            _customerId = customerId;
            _getAllFullScreenVideosFunc = getAllFullScreenVideosFunc;

            if (video != null)
            {
                VideoItemViewModel = new OrderedVideoItemViewModel(
                    videoManager,
                    userSessionProvider,
                    video);
            }

            OpenUserProfileCommand = new MvxRestrictedAsyncCommand(
                OpenUserProfileAsync,
                restrictedCanExecute: () => _userSessionProvider.User != null,
                handleFunc: NavigationManager.NavigateAsync<LoginViewModel>);
        }

        public IMvxAsyncCommand OpenUserProfileCommand { get; }

        public OrderedVideoItemViewModel VideoItemViewModel { get; }

        protected virtual BaseVideoItemViewModel[] GetFullScreenVideos()
        {
            if (_getAllFullScreenVideosFunc != null)
            {
                return _getAllFullScreenVideosFunc.Invoke();
            }

            return VideoItemViewModel is null
                ? Array.Empty<BaseVideoItemViewModel>()
                : new BaseVideoItemViewModel[] { VideoItemViewModel };
        }

        protected virtual Task OpenUserProfileAsync()
        {
            if (_customerId is null ||
                _customerId == _userSessionProvider.User.Id)
            {
                return Task.CompletedTask;
            }

            if (!Connectivity.NetworkAccess.HasConnection())
            {
                return Task.CompletedTask;
            }

            return NavigationManager.NavigateAsync<UserProfileViewModel, int, bool>(_customerId.Value);
        }
    }
}