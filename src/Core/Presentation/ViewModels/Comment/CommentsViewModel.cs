using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.ApplicationServices.Dialogs;
using PrankChat.Mobile.Core.ApplicationServices.ErrorHandling;
using PrankChat.Mobile.Core.ApplicationServices.Network;
using PrankChat.Mobile.Core.ApplicationServices.Settings;
using PrankChat.Mobile.Core.Infrastructure;
using PrankChat.Mobile.Core.Infrastructure.Extensions;
using PrankChat.Mobile.Core.Models.Data;
using PrankChat.Mobile.Core.Models.Data.Shared;
using PrankChat.Mobile.Core.Presentation.Navigation;
using PrankChat.Mobile.Core.Presentation.ViewModels.Comment.Items;
using PrankChat.Mobile.Core.Presentation.ViewModels.Shared;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.Presentation.ViewModels.Comment
{
    public class CommentsViewModel : PaginationViewModel, IMvxViewModel<int, int>
    {
        private readonly IMvxLog _mvxLog;

        private int _videoId;
        private int _newCommentsCounter;

        public CommentsViewModel(INavigationService navigationService,
                                 IErrorHandleService errorHandleService,
                                 IApiService apiService,
                                 IDialogService dialogService,
                                 ISettingsService settingsService,
                                 IMvxLog mvxLog)
            : base(Constants.Pagination.DefaultPaginationSize, navigationService, errorHandleService, apiService, dialogService, settingsService)
        {
            Items = new MvxObservableCollection<CommentItemViewModel>();

            SendCommentCommand = new MvxAsyncCommand(SendCommentAsync, () => !string.IsNullOrWhiteSpace(Comment));
            ScrollInteraction = new MvxInteraction<int>();
            _mvxLog = mvxLog;
        }

        public MvxObservableCollection<CommentItemViewModel> Items { get; }

        public MvxInteraction<int> ScrollInteraction { get; }

        private string _comment;
        public string Comment
        {
            get => _comment;
            set
            {
                if (SetProperty(ref _comment, value))
                {
                    SendCommentCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string ProfilePhotoUrl => SettingsService.User?.Avatar;

        public string ProfileShortName => SettingsService.User?.Login?.ToShortenName();

        public IMvxAsyncCommand SendCommentCommand { get; }

        public TaskCompletionSource<object> CloseCompletionSource { get; set; } = new TaskCompletionSource<object>();

        public void Prepare(int parameter)
        {
            _videoId = parameter;
        }

        public override void Reset()
        {
            Items.Clear();
            base.Reset();
        }

        public override Task Initialize()
        {
            return LoadMoreItemsCommand.ExecuteAsync();
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            if (viewFinishing && CloseCompletionSource != null && !CloseCompletionSource.Task.IsCompleted && !CloseCompletionSource.Task.IsFaulted)
            {
                CloseCompletionSource?.SetResult(_newCommentsCounter);
            }

            base.ViewDestroy(viewFinishing);
        }

        protected override async Task<int> LoadMoreItemsAsync(int page = 1, int pageSize = 20)
        {
            var pageContainer = await ApiService.GetVideoCommentsAsync(_videoId, page, pageSize);

            var count = SetList(pageContainer, page, ProduceCommentItemViewModel, Items);
            return count;
        }

        private CommentItemViewModel ProduceCommentItemViewModel(CommentDataModel commentDataModel)
        {
            return new CommentItemViewModel(NavigationService, SettingsService, commentDataModel);
        }

        protected override int SetList<TDataModel, TApiModel>(PaginationModel<TApiModel> dataModel, int page, Func<TApiModel, TDataModel> produceItemViewModel, MvxObservableCollection<TDataModel> items)
        {
            SetTotalItemsCount(dataModel?.TotalCount ?? 0);
            _newCommentsCounter = (int)(dataModel?.TotalCount ?? 0);

            var orderViewModels = dataModel?.Items?.Select(produceItemViewModel).ToList();

            items.AddRange(orderViewModels);
            return orderViewModels.Count;
        }

        private async Task SendCommentAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Comment))
                {
                    return;
                }

                var comment = await ApiService.CommentVideoAsync(_videoId, Comment);
                if (comment is null)
                {
                    return;
                }

                comment.User = SettingsService.User;
                Items.Add(ProduceCommentItemViewModel(comment));
                _newCommentsCounter += 1;
                SetTotalItemsCount(_newCommentsCounter);

                ScrollInteraction.Raise(Items.Count - 1);

                Comment = string.Empty;
            }
            catch (Exception ex)
            {
                _mvxLog.ErrorException("Failed to send comments", ex);
            }
        }
    }
}