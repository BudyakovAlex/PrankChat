using MvvmCross.Plugin.Messenger;
using PrankChat.Mobile.Core.Common;
using PrankChat.Mobile.Core.Mappers;
using PrankChat.Mobile.Core.Messages;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using PrankChat.Mobile.Core.Providers.Permissions;
using PrankChat.Mobile.Core.Providers.Platform;
using PrankChat.Mobile.Core.Services.FileSystem;
using PrankChat.Mobile.Core.Services.Network.Http.Video;
using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PrankChat.Mobile.Core.Managers.Video
{
    public class VideoManager : IVideoManager
    {
        private readonly IPlatformPathsProvider _pathsProvider;
        private readonly IVideoService _videoService;
        private readonly IPermissionProvider _permissionProvider;
        private readonly IFileSystemService _fileSystemService;
        private readonly IMvxMessenger _mvxMessenger;

        public VideoManager(
            IPlatformPathsProvider pathsProvider,
            IVideoService videoService,
            IPermissionProvider permissionProvider,
            IFileSystemService fileSystemService,
            IMvxMessenger mvxMessenger)
        {
            _pathsProvider = pathsProvider;
            _videoService = videoService;
            _permissionProvider = permissionProvider;
            _fileSystemService = fileSystemService;
            _mvxMessenger = mvxMessenger;
        }

        public async Task<Models.Data.Video> SendVideoAsync(
            int orderId,
            string path,
            string title,
            string description,
            Action<double, double> onChangedProgressAction = null,
            CancellationToken cancellationToken = default)
        {
            var response = await _videoService.SendVideoAsync(orderId, path, title, description, onChangedProgressAction, cancellationToken);
            return response.Map();
        }

        public async Task<long?> IncrementVideoViewsAsync(int videoId)
        {
            var views = await _videoService.IncrementVideoViewsAsync(videoId);
            if (views.HasValue)
            {
                _mvxMessenger.Publish(new ViewCountMessage(this, videoId, views.Value));
            }

            return views;
        }

        public Task ComplainVideoAsync(int videoId, string title, string description)
        {
            return _videoService.ComplainVideoAsync(videoId, title, description);
        }

        public async Task<Comment> CommentVideoAsync(int videoId, string comment)
        {
            var response = await _videoService.CommentVideoAsync(videoId, comment);
            return response.Map();
        }

        public async Task<Pagination<Comment>> GetVideoCommentsAsync(int videoId, int page, int pageSize)
        {
            var response = await _videoService.GetVideoCommentsAsync(videoId, page, pageSize);
            return response.Map(item => item.Map());
        }

        public async Task<Models.Data.Video> SendLikeAsync(int videoId, bool isChecked, CancellationToken? cancellationToken = null)
        {
            var response = await _videoService.SendLikeAsync(videoId, isChecked, cancellationToken);
            return response.Map();
        }

        public async Task<Models.Data.Video> SendDislikeAsync(int videoId, bool isChecked, CancellationToken? cancellationToken = null)
        {
            var response = await _videoService.SendDislikeAsync(videoId, isChecked, cancellationToken);
            return response.Map();
        }

        public async Task<string> DownloadVideoAsync(string videoUrl, string videoName)
        {
            try
            {
                var isPermissionGranted = await _permissionProvider.RequestPermissionAsync<Permissions.StorageWrite>();
                if (!isPermissionGranted)
                {
                    return null;
                }

                var documentName = $"{videoName}_{DateTime.Now.ToString(Constants.Formats.DownloadVideoDateTimeFormat)}{Constants.File.DownloadVideoFormat}";
                var localPath = Path.Combine(_pathsProvider.DownloadsFolderPath, documentName);

                using var webClient = new WebClient();
                await webClient.DownloadFileTaskAsync(videoUrl, localPath);
                await _fileSystemService.StoreVideoFileToGalleryAsync(localPath);

                return localPath;
            }
            catch
            {
                return null;
            }
        }
    }
}