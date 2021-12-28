using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FFImageLoading;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Common;
using PrankChat.Mobile.Core.Data.Enums;
using PrankChat.Mobile.Core.Exceptions;
using PrankChat.Mobile.Core.Exceptions.Network;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Managers.Competitions;
using PrankChat.Mobile.Core.Managers.Media;
using PrankChat.Mobile.Core.Managers.Video;
using PrankChat.Mobile.Core.Messages;
using PrankChat.Mobile.Core.Models.Data.Shared;
using PrankChat.Mobile.Core.Models.Enums;
using PrankChat.Mobile.Core.Services.ErrorHandling.Messages;
using PrankChat.Mobile.Core.ViewModels.Abstract;
using PrankChat.Mobile.Core.ViewModels.Common;
using PrankChat.Mobile.Core.ViewModels.Common.Abstract;
using PrankChat.Mobile.Core.ViewModels.Competition.Items;
using PrankChat.Mobile.Core.ViewModels.Parameters;
using PrankChat.Mobile.Core.ViewModels.Profile.Cashbox;

namespace PrankChat.Mobile.Core.ViewModels.Competition
{
    public class CompetitionDetailsViewModel : PaginationViewModel<Models.Data.Competition, bool, BaseViewModel>
    {
        private readonly ICompetitionsManager _competitionsManager;
        private readonly IVideoManager _videoManager;
        private readonly IMediaManager _mediaManager;

        private Models.Data.Competition _competition;
        private CompetitionDetailsHeaderViewModel _header;

        private bool _isRefreshing;
        private bool _isReloadNeeded;

        private CancellationTokenSource _cancellationTokenSource;

        public CompetitionDetailsViewModel(
            ICompetitionsManager competitionsManager,
            IVideoManager videoManager,
            IMediaManager mediaManager) : base(Constants.Pagination.DefaultPaginationSize)
        {
            _competitionsManager = competitionsManager;
            _videoManager = videoManager;
            _mediaManager = mediaManager;

            Messenger.SubscribeOnMainThread<ReloadCompetitionMessage>(OnReloadData).DisposeWith(Disposables);

            RefreshDataCommand = this.CreateCommand(RefreshDataAsync);
            CancelUploadingCommand = this.CreateCommand(() => _cancellationTokenSource?.Cancel());
            OpenStatisticsCommand = this.CreateCommand(OpenStatisticsAsync);
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

        protected override bool DefaultResult => _isReloadNeeded;

        public IMvxAsyncCommand RefreshDataCommand { get; }

        public IMvxCommand CancelUploadingCommand { get; }

        public IMvxCommand OpenStatisticsCommand { get; }

        public override void Prepare(Models.Data.Competition parameter)
        {
            _competition = parameter;
            _header = new CompetitionDetailsHeaderViewModel(
                IsUserSessionInitialized,
                this.CreateCommand(ExecuteActionAsync),
                this.CreateCommand(DeleteAsync),
                parameter,
                _competition.Customer?.Id == UserSessionProvider.User?.Id);

            Items.Add(_header);
        }

        public override Task InitializeAsync()
        {
            return LoadMoreItemsCommand.ExecuteAsync();
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

            var preloadImagesTasks = pageContainer.Items.Select(item => ImageService.Instance.LoadUrl(item.Poster).PreloadAsync()).ToArray();
            _ = Task.WhenAll(preloadImagesTasks);

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
            var paymentMessage = string.Format(Resources.CompetionRulesTemplate, _competition.EntryTax);
            var isConfirmed = await UserInteraction.ShowConfirmAsync(paymentMessage, ok: Resources.Pay, cancel: Resources.Cancel);
            if (!isConfirmed)
            {
                return;
            }

            try
            {
                ErrorHandleService.SuspendServerErrorsHandling();
                var updatedCompetition = await _competitionsManager.CompetitionJoinAsync(_competition.Id);
                if (updatedCompetition != null)
                {
                    _competition = updatedCompetition;

                    _header = new CompetitionDetailsHeaderViewModel(
                        IsUserSessionInitialized,
                        this.CreateCommand(ExecuteActionAsync),
                        this.CreateCommand(DeleteAsync),
                        _competition,
                        _competition.Customer?.Id == UserSessionProvider.User?.Id);

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
            var canRefil = await UserInteraction.ShowConfirmAsync(exception.Message, Resources.Attention, Resources.Replenish, Resources.Cancel);
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
            // TODO
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

        private BaseVideoItemViewModel[] GetFullScreenVideos() =>
            Items.OfType<CompetitionVideoViewModel>().ToArray();

        private CompetitionVideoViewModel ProduceVideoItemViewModel(Models.Data.Video video)
        {
            return new CompetitionVideoViewModel(
                _videoManager,
                UserSessionProvider,
                video,
                video.User.Id == UserSessionProvider.User.Id,
                _competition.GetPhase() == CompetitionPhase.Voting,
                GetFullScreenVideos);
        }

        private async Task LoadVideoAsync()
        {
            try
            {
                var file = await _mediaManager.PickVideoAsync();
                if (file == null)
                {
                    return;
                }

                var fileInfo = new FileInfo(file.Path);
                if (fileInfo.Length > Constants.File.MaximumFileSizeInMegabytes)
                {
                    UserInteraction.ShowToast(Resources.VideoLimitOnUploading, ToastType.Negative);
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
                    UserInteraction.ShowToast(Resources.VideoFailedToUpload, ToastType.Negative);
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
                        _videoManager,
                        UserSessionProvider,
                        video,
                        true,
                        false,
                        GetFullScreenVideos));
                });

                await _header.RaisePropertyChanged(nameof(_header.CanExecuteActionVideo));
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

        private async Task DeleteAsync()
        {
            var shouldCancelOrder = await UserInteraction.ShowConfirmAsync(
                Resources.WantToCancelCompetition,
                Resources.Attention,
                Resources.Ok,
                Resources.Cancel);
            if (!shouldCancelOrder)
            {
                return;
            }

            await _competitionsManager.CancelCompetitionAsync(_competition.Id);
            _isReloadNeeded = true;
            await CloseAsync(false);
        }

        private Task OpenStatisticsAsync() =>
            NavigationManager.NavigateAsync<CompetitonStatitsticsViewModel, int>(_competition.Id);
    }
}