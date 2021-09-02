using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Common;
using PrankChat.Mobile.Core.Extensions;
using PrankChat.Mobile.Core.Managers.Video;
using PrankChat.Mobile.Core.Models.Data.Shared;
using PrankChat.Mobile.Core.ViewModels.Comment.Items;
using PrankChat.Mobile.Core.ViewModels.Common;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PrankChat.Mobile.Core.ViewModels.Comment
{
    public class CommentsViewModel : PaginationViewModel, IMvxViewModel<int, int>
    {
        private readonly IVideoManager _videoManager;
        private readonly IMvxLog _mvxLog;

        private int _videoId;
        private int _newCommentsCounter;

        public CommentsViewModel(IVideoManager videoManager, IMvxLog mvxLog) : base(Constants.Pagination.DefaultPaginationSize)
        {
            _videoManager = videoManager;
            _mvxLog = mvxLog;

            Items = new MvxObservableCollection<CommentItemViewModel>();

            SendCommentCommand = this.CreateCommand(SendCommentAsync, () => !string.IsNullOrWhiteSpace(Comment));
            ScrollInteraction = new MvxInteraction<int>();
        }

        public MvxObservableCollection<CommentItemViewModel> Items { get; }

        public MvxInteraction<int> ScrollInteraction { get; }

        private string _comment;
        public string Comment
        {
            get => _comment;
            set => SetProperty(ref _comment, value, () => SendCommentCommand.RaiseCanExecuteChanged());
        }

        public string ProfilePhotoUrl => UserSessionProvider.User?.Avatar;

        public string ProfileShortName => UserSessionProvider.User?.Login?.ToShortenName();

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

        public override Task InitializeAsync()
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
            var pageContainer = await _videoManager.GetVideoCommentsAsync(_videoId, page, pageSize);

            var count = SetList(pageContainer, page, ProduceCommentItemViewModel, Items);
            return count;
        }

        protected override int SetList<TDataModel, TApiModel>(
            Pagination<TApiModel> pagination,
            int page,
            Func<TApiModel, TDataModel> produceItemViewModel,
            MvxObservableCollection<TDataModel> items)
        {
            SetTotalItemsCount(pagination?.TotalCount ?? 0);
            _newCommentsCounter = (int)(pagination?.TotalCount ?? 0);

            var orderViewModels = pagination?.Items?.Select(produceItemViewModel).ToList();

            items.AddRange(orderViewModels);
            return orderViewModels.Count;
        }

        private CommentItemViewModel ProduceCommentItemViewModel(Models.Data.Comment comment)
        {
            return new CommentItemViewModel(UserSessionProvider, comment);
        }

        private Task SendCommentAsync()
        {
            return SafeExecutionWrapper.WrapAsync(async () =>
            {
                if (string.IsNullOrWhiteSpace(Comment))
                {
                    return;
                }

                var comment = await _videoManager.CommentVideoAsync(_videoId, Comment);
                if (comment is null)
                {
                    return;
                }

                comment.User = UserSessionProvider.User;
                Items.Add(ProduceCommentItemViewModel(comment));
                _newCommentsCounter += 1;
                SetTotalItemsCount(_newCommentsCounter);

                ScrollInteraction.Raise(Items.Count - 1);

                Comment = string.Empty;
            });
        }
    }
}