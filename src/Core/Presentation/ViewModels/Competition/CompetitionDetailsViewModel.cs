﻿using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling.Messages;
using PrankChat.Mobile.Core.ApplicationServices.Mediaes;
using PrankChat.Mobile.Core.BusinessServices;
using PrankChat.Mobile.Core.Data.Enums;
using PrankChat.Mobile.Core.Exceptions;
using PrankChat.Mobile.Core.Exceptions.Network;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Managers.Competitions;
using PrankChat.Mobile.Core.Managers.Publications;
using PrankChat.Mobile.Core.Managers.Video;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.Messages;
using PrankChat.Mobile.Core.Presentation.Navigation.Parameters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Abstract;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition.Items;
using PrankChat.Mobile.Core.Presentation.ViewModels.Profile.Cashbox;
using PrankChat.Mobile.Core.Presentation.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Competition
{
    public class CompetitionDetailsViewModel : PaginationViewModel, IMvxViewModel<Models.Data.Competition, bool>
    {
        private readonly ICompetitionsManager _competitionsManager;
        private readonly IPublicationsManager _publicationsManager;
        private readonly IVideoManager _videoManager;
        private readonly IVideoPlayerService _videoPlayerService;
        private readonly IMediaService _mediaService;

        private Models.Data.Competition _competition;
        private CompetitionDetailsHeaderViewModel _header;

        private bool _isRefreshing;
        private bool _isReloadNeeded;

        private CancellationTokenSource _cancellationTokenSource;

        public CompetitionDetailsViewModel(ICompetitionsManager competitionsManager,
                                           IPublicationsManager publicationsManager,
                                           IVideoManager videoManager,
                                           IVideoPlayerService videoPlayerService,
                                           IMediaService mediaService) : base(Constants.Pagination.DefaultPaginationSize)
        {
            _competitionsManager = competitionsManager;
            _publicationsManager = publicationsManager;
            _videoManager = videoManager;
            _videoPlayerService = videoPlayerService;
            _mediaService = mediaService;

            Items = new MvxObservableCollection<BaseViewModel>();

            Messenger.SubscribeOnMainThread<ReloadCompetitionMessage>(OnReloadData).DisposeWith(Disposables);

            RefreshDataCommand = new MvxAsyncCommand(RefreshDataAsync);
            CancelUploadingCommand = new MvxCommand(() => _cancellationTokenSource?.Cancel());
        }

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

        public MvxObservableCollection<BaseViewModel> Items { get; }

        public IMvxAsyncCommand RefreshDataCommand { get; }

        public IMvxCommand CancelUploadingCommand { get; }

        public TaskCompletionSource<object> CloseCompletionSource { get; set; } = new TaskCompletionSource<object>();

        public void Prepare(Models.Data.Competition parameter)
        {
            _competition = parameter;
            _header = new CompetitionDetailsHeaderViewModel(
                IsUserSessionInitialized,
                new MvxAsyncCommand(ExecuteActionAsync),
                parameter);

            Items.Add(_header);
        }

        public override Task InitializeAsync()
        {
            return LoadMoreItemsCommand.ExecuteAsync();
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            if (viewFinishing && CloseCompletionSource != null && !CloseCompletionSource.Task.IsCompleted && !CloseCompletionSource.Task.IsFaulted)
            {
                CloseCompletionSource?.SetResult(_isReloadNeeded);
            }

            base.ViewDestroy(viewFinishing);
        }

        protected override async Task<int> LoadMoreItemsAsync(int page = 1, int pageSize = 20)
        {
            var pageContainer = await _competitionsManager.GetCompetitionVideosAsync(_competition.Id, page, pageSize);
            if (_competition.GetPhase() == CompetitionPhase.New)
            {
                var video = pageContainer.Items.FirstOrDefault(item => item.User.Id == UserSessionProvider.User.Id);
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

        protected override int SetList<TDataModel, TApiModel>(Pagination<TApiModel> pagination, int page, Func<TApiModel, TDataModel> produceItemViewModel, MvxObservableCollection<TDataModel> items)
        {
            SetTotalItemsCount(pagination?.TotalCount ?? 0);
            var orderViewModels = pagination?.Items?.Select(produceItemViewModel).ToList();

            items.AddRange(orderViewModels);
            return orderViewModels.Count;
        }

        private Task ExecuteActionAsync()
        {
            if (_competition is null)
            {
                return Task.CompletedTask;
            }

            if (_competition.Category.CheckIsPaidCompetitionOrder()
                && !_competition.IsPaidCompetitionMember)
            {
                return CompetitionJoinAsync();
            }

            return LoadVideoAsync();
        }

        private async Task CompetitionJoinAsync()
        {
            try
            {
                ErrorHandleService.SuspendServerErrorsHandling();
                var updatedCompetition = await _competitionsManager.CompetitionJoinAsync(_competition.Id);
                if (updatedCompetition != null)
                {
                    _competition = updatedCompetition;

                    _header = new CompetitionDetailsHeaderViewModel(
                        IsUserSessionInitialized,
                        new MvxAsyncCommand(ExecuteActionAsync),
                        _competition);

                    Items.ReplaceWith(new[] { _header });
                    await LoadMoreItemsAsync();
                    _isReloadNeeded = true;
                }
            }
            catch (NetworkException ex) when (ex.InnerException is ProblemDetailsException problemDetails && problemDetails?.CodeError == Constants.ErrorCodes.LowBalance)
            {
                await HandleLowBalanceExceptionAsync(ex);
            }
            catch (Exception ex) when(ex.InnerException is ProblemDetailsException problemDetails)
            {
                ErrorHandleService.ResumeServerErrorsHandling();
                Messenger.Publish(new ServerErrorMessage(this, problemDetails));
            }
            catch (Exception ex)
            {
                ErrorHandleService.ResumeServerErrorsHandling();
                Messenger.Publish(new ServerErrorMessage(this, ex));
            }
            finally
            {
                ErrorHandleService.ResumeServerErrorsHandling();
            }
        }

        private async Task HandleLowBalanceExceptionAsync(Exception exception)
        {
            var canRefil = await DialogService.ShowConfirmAsync(exception.Message, Resources.Attention, Resources.ProfileView_Refill, Resources.Cancel);
            if (!canRefil)
            {
                return;
            }

            var navigationParameter = new CashboxTypeNavigationParameter(CashboxType.Refill);
            var shouldRefresh = await NavigationManager.NavigateAsync<CashboxViewModel, CashboxTypeNavigationParameter, bool>(navigationParameter);
            if (!shouldRefresh)
            {
                return;
            }

            await CompetitionJoinAsync();
        }

        private void OnReloadData(ReloadCompetitionMessage obj)
        {
            _isReloadNeeded = true;
            RefreshDataCommand.Execute();
        }

        private async Task RefreshDataAsync()
        {
            try
            {
                IsRefreshing = true;
                Items.ReplaceWith(new[] { _header });
                await LoadMoreItemsAsync();
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        private List<FullScreenVideo> GetFullScreenVideos()
        {
            return Items.OfType<CompetitionVideoViewModel>()
                        .Select(item => item.GetFullScreenVideo())
                        .ToList();
        }

        private CompetitionVideoViewModel ProduceVideoItemViewModel(Models.Data.Video video)
        {
            return new CompetitionVideoViewModel(
                _publicationsManager,
                _videoPlayerService,
                UserSessionProvider,
                Logger,
                video,
                video.User.Id == UserSessionProvider.User.Id,
                _competition.GetPhase() == CompetitionPhase.Voting,
                GetFullScreenVideos);
        }

        private Task LoadVideoAsync()
        {
            try
            {
                return ExecutionStateWrapper.WrapAsync(async () =>
                {
                    var file = await _mediaService.PickVideoAsync();
                    if (file == null)
                    {
                        return;
                    }

                    UploadingProgress = 0;
                    UploadingProgressStringPresentation = "- / -";
                    IsUploading = true;

                    _cancellationTokenSource = new CancellationTokenSource();

                    var video = await _videoManager.SendVideoAsync(
                        _competition.Id,
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

                    _header.Competition.CanUploadVideo = false;
                    Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
                    {
                        video.User = UserSessionProvider.User;
                        Items.Insert(1, new CompetitionVideoViewModel(
                            _publicationsManager,
                            _videoPlayerService,
                            UserSessionProvider,
                            Logger,
                            video,
                            true,
                            false,
                            GetFullScreenVideos));
                    });

                    await _header.RaisePropertyChanged(nameof(_header.CanExecuteActionVideo));
                });
            }
            finally
            {
                _isReloadNeeded = true;
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
        }
    }
}