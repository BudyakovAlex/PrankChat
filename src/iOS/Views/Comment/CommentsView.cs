using Foundation;
using MvvmCross.Base;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Combiners;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross.ViewModels;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.ViewModels.Comment;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Common;
using PrankChat.Mobile.iOS.Controls;
using PrankChat.Mobile.iOS.SourcesAndDelegates;
using PrankChat.Mobile.iOS.Views.Base;
using System;
using UIKit;

namespace PrankChat.Mobile.iOS.Views.Comment
{
    [MvxModalPresentation(WrapInNavigationController = true)]
	public partial class CommentsView : BaseViewController<CommentsViewModel>
	{
        private MvxUIRefreshControl _refreshControl;
        private CommentsTableSource _commentTableSource;
        private EmptyView _emptyView;

        public override bool CanHandleKeyboardNotifications => true;

        private MvxInteraction<int> _scrollInteraction;
        public MvxInteraction<int> ScrollInteraction
        {
            get => _scrollInteraction;
            set
            {
                if (_scrollInteraction != null)
                {
                    _scrollInteraction.Requested -= OnInteractionRequested;
                }

                _scrollInteraction = value;

                if (_scrollInteraction != null)
                {
                    _scrollInteraction.Requested += OnInteractionRequested;
                }
            }
        }

        protected override void Bind()
		{
            using var bindingSet = this.CreateBindingSet<CommentsView, CommentsViewModel>();

            bindingSet.Bind(_commentTableSource).To(vm => vm.Items);
            bindingSet.Bind(sendButton).To(vm => vm.SendCommentCommand);
            bindingSet.Bind(commentTextView).To(vm => vm.Comment);
            bindingSet.Bind(_refreshControl).For(v => v.IsRefreshing).To(vm => vm.IsBusy);
            bindingSet.Bind(this).For(v => v.ScrollInteraction).To(vm => vm.ScrollInteraction);
            bindingSet.Bind(_refreshControl).For(v => v.RefreshCommand).To(vm => vm.ReloadItemsCommand);
            bindingSet.Bind(profileImageView)
                .For(v => v.ImagePath)
                .To(vm => vm.ProfilePhotoUrl)
                .Mode(MvxBindingMode.OneTime);
            bindingSet.Bind(profileImageView)
                .For(v => v.PlaceholderText)
                .To(vm => vm.ProfileShortName)
                .Mode(MvxBindingMode.OneTime);
            bindingSet.Bind(_emptyView)
                .For(v => v.BindVisible())
                .ByCombining(new MvxAndValueCombiner(),
                  vm => vm.IsEmpty,
                  vm => vm.IsNotBusy,
                  vm => vm.IsInitialized);
        }

		protected override void SetupControls()
		{
            Title = Resources.Comments;
            InitializeTableView();
            CreateEmptyView();

            commentTextView.SetBorderlessStyle(string.Empty, Theme.Color.CommentBorder);

            commentViewSeparatorView.BackgroundColor = Theme.Color.Separator;
        }

        private void InitializeTableView()
        {
            _commentTableSource = new CommentsTableSource(tableView);
            tableView.Source = _commentTableSource;

            tableView.RegisterNibForCellReuse(CommentItemCell.Nib, CommentItemCell.CellId);
            tableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag;
            tableView.AllowsSelection = false;

            _refreshControl = new MvxUIRefreshControl();
            tableView.RefreshControl = _refreshControl;
        }

        protected override void OnKeyboardChanged(bool visible, nfloat keyboardHeight)
        {
            base.OnKeyboardChanged(visible, keyboardHeight);

            var window = UIApplication.SharedApplication.KeyWindow;
            var bottomPadding = window.SafeAreaInsets.Bottom;
            editorBottomConstraint.Constant = visible ? -(keyboardHeight - bottomPadding) : 0;
            UIView.Animate(0.5, () => View.LayoutIfNeeded());
        }

        private void OnInteractionRequested(object sender, MvxValueEventArgs<int> e)
        {
            tableView.ScrollToRow(NSIndexPath.FromItemSection(e.Value, 0), UITableViewScrollPosition.Bottom, true);
        }

        private void CreateEmptyView()
        {
            _emptyView = EmptyView
                .Create(Resources.CommentsListIsEmpty, ImageNames.ImageEmptyState)
                .AttachToTableViewAsBackgroundView(tableView);
        }
    }
}