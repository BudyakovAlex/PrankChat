using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Mediaes;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Base;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items;
using PrankChat.Mobile.Core.Presentation.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Competition
{
    public class CompetitionDetailsViewModel : PaginationViewModel, IMvxViewModel<CompetitionDataModel, bool>
    {
        private readonly IMvxMessenger _mvxMessenger;
        private readonly IVideoPlayerService _videoPlayerService;
        private readonly IMediaService _mediaService;

        private CompetitionDataModel _competition;
        private CompetitionDetailsHeaderViewModel _header;
        private bool _isRefreshing;
        private MvxSubscriptionToken _reloadItemsSubscriptionToken;
        private bool _isReloadNeeded;

        private CancellationTokenSource _cancellationTokenSource;

        private bool _isUploading;
        public bool IsUploading
        {
            get => _isUploading;
            private set => SetProperty(ref _isUploading, value);
        }

        private float _uploadingProgress;
        public float UploadingProgress
        {
            get => _uploadingProgress;
            private set => SetProperty(ref _uploadingProgress, value);
        }

        private string _uploadingProgressStringPresentation;
        public string UploadingProgressStringPresentation
        {
            get => _uploadingProgressStringPresentation;
            private set => SetProperty(ref _uploadingProgressStringPresentation, value);
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }


        public MvxObservableCollection<BaseItemViewModel> Items { get; }

        public IMvxAsyncCommand RefreshDataCommand { get; }

        public IMvxCommand CancelUploadingCommand { get; }

        public TaskCompletionSource<object> CloseCompletionSource { get; set; } = new TaskCompletionSource<object>();

        public CompetitionDetailsViewModel(IMvxMessenger mvxMessenger,
                                           INavigationService navigationService,
                                           IVideoPlayerService videoPlayerService,
                                           IErrorHandleService errorHandleService,
                                           IApiService apiService,
                                           IMediaService mediaService,
                                           IDialogService dialogService,
                                           ISettingsService settingsService) : base(Constants.Pagination.DefaultPaginationSize, navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
            _mvxMessenger = mvxMessenger;
            _videoPlayerService = videoPlayerService;
            _mediaService = mediaService;

            Items = new MvxObservableCollection<BaseItemViewModel>();

            RefreshDataCommand = new MvxAsyncCommand(RefreshDataAsync);
            CancelUploadingCommand = new MvxCommand(() => _cancellationTokenSource?.Cancel());
        }

        public void Prepare(CompetitionDataModel parameter)
        {
            _competition = parameter;
            _header = new CompetitionDetailsHeaderViewModel(IsUserSessionInitialized,
                                                            _mvxMessenger,
                                                            NavigationService,
                                                            new MvxAsyncCommand(LoadVideoAsync),
                                                            parameter);
            Items.Add(_header);
        }

        public override Task Initialize()
        {
            return LoadMoreItemsCommand.ExecuteAsync();
        }

        public override void ViewCreated()
        {
            base.ViewCreated();
            Subscription();
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            Unsubscription();

            if (viewFinishing && CloseCompletionSource != null && !CloseCompletionSource.Task.IsCompleted && !CloseCompletionSource.Task.IsFaulted)
            {
                CloseCompletionSource?.SetResult(_isReloadNeeded);
            }

            base.ViewDestroy(viewFinishing);
        }

        private void Subscription()
        {
            _reloadItemsSubscriptionToken = _mvxMessenger.SubscribeOnMainThread<ReloadCompetitionMessage>(OnReloadData);
        }

        private void Unsubscription()
        {
            if (_reloadItemsSubscriptionToken != null)
            {
                _mvxMessenger.Unsubscribe<ReloadCompetitionMessage>(_reloadItemsSubscriptionToken);
                _reloadItemsSubscriptionToken.Dispose();
            }
        }

        private void OnReloadData(ReloadCompetitionMessage obj)
        {
            _isReloadNeeded = true;
            RefreshDataCommand.Execute();
        }

        protected override async Task<int> LoadMoreItemsAsync(int page = 1, int pageSize = 20)
        {
            var pageContainer = await ApiService.GetCompetitionVideosAsync(_competition.Id, page, pageSize);
            if (_competition.GetPhase() == CompetitionPhase.New)
            {
                var video = pageContainer.Items.FirstOrDefault(item => item.User.Id == SettingsService.User.Id);
                if (video is null)
                {
                    return 0;
                }

                Items.Add(ProduceVideoItemViewModel(video));
                return 1;
            }

            var count = SetList(pageContainer, page, ProduceVideoItemViewModel, Items);
            return count;
        }

        protected override int SetList<TDataModel, TApiModel>(PaginationModel<TApiModel> dataModel, int page, Func<TApiModel, TDataModel> produceItemViewModel, MvxObservableCollection<TDataModel> items)
        {
            SetTotalItemsCount(dataModel?.TotalCount ?? 0);
            var orderViewModels = dataModel?.Items?.Select(produceItemViewModel).ToList();

            items.AddRange(orderViewModels);
            return orderViewModels.Count;
        }

        private async Task RefreshDataAsync()
        {
            try
            {
                IsRefreshing = true;
                Items.ReplaceWith(new []{ _header });
                await LoadMoreItemsAsync();
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        private List<FullScreenVideoDataModel> GetFullScreenVideoDataModels()
        {
            return Items.OfType<CompetitionVideoViewModel>()
                        .Select(item => item.GetFullScreenVideoDataModel())
                        .ToList();
        }

        private CompetitionVideoViewModel ProduceVideoItemViewModel(VideoDataModel videoDataModel)
        {
            return new CompetitionVideoViewModel(ApiService,
                                                 _videoPlayerService,
                                                 NavigationService,
                                                 SettingsService,
                                                 _mvxMessenger,
                                                 Logger,
                                                 videoDataModel,
                                                 videoDataModel.User.Id == SettingsService.User.Id,
                                                 _competition.GetPhase() == CompetitionPhase.Voting,
                                                 GetFullScreenVideoDataModels);
        }

        private async Task LoadVideoAsync()
        {
            try
            {
                IsBusy = true;

                var file = await _mediaService.PickVideoAsync();
                if (file == null)
                {
                    return;
                }

                UploadingProgress = 0;
                UploadingProgressStringPresentation = "- / -";
                IsUploading = true;

                _cancellationTokenSource = new CancellationTokenSource();

                var video = await ApiService.SendVideoAsync(_competition.Id,
                                                            file.Path,
                                                            _competition.Title,
                                                            _competition.Description,
                                                            OnUploadingProgressChanged,
                                                            _cancellationTokenSource.Token);
                if (video == null && (!_cancellationTokenSource?.IsCancellationRequested ?? true))
                {
                    DialogService.ShowToast(Resources.Video_Failed_To_Upload, ToastType.Negative);
                    _cancellationTokenSource = null;
                    IsUploading = false;
                    return;
                }

                _cancellationTokenSource = null;
                IsUploading = false;
                IsBusy = true;

                _header.Competition.CanUploadVideo = false;
                Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
                {
                    video.User = SettingsService.User;
                    Items.Insert(1, new CompetitionVideoViewModel(ApiService,
                                                                  _videoPlayerService,
                                                                  NavigationService,
                                                                  SettingsService,
                                                                  _mvxMessenger,
                                                                  Logger,
                                                                  video,
                                                                  true,
                                                                  false,
                                                                  GetFullScreenVideoDataModels));
                });

                await _header.RaisePropertyChanged(nameof(_header.CanLoadVideo));
            }
            finally
            {
                _isReloadNeeded = true;
                IsBusy = false;
            }
        }

        private void OnUploadingProgressChanged(double progress, double size)
        {
            UploadingProgress = (float)(progress / size * 100);
            UploadingProgressStringPresentation = $"{((long)progress).ToFileSizePresentation()} / {((long)size).ToFileSizePresentation()}";

            if (UploadingProgress < 100)
            {
                return;
            }

            IsUploading = false;
            IsBusy = true;
        }
    }
}