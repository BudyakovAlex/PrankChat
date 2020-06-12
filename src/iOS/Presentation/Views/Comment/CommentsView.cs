using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Comment;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Infrastructure;
using PrankChat.Mobile.iOS.Presentation.SourcesAndDelegates;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Comment
{
    [MvxModalPresentation(WrapInNavigationController = true)]
	public partial class CommentsView : BaseGradientBarView<CommentsViewModel>
	{
        private MvxUIRefreshControl _refreshControl;

        private CommentsTableSource _commentTableSource;

        protected override void SetupBinding()
		{
            var bindingSet = this.CreateBindingSet<CommentsView, CommentsViewModel>();

            bindingSet.Bind(_commentTableSource)
                      .To(vm => vm.Items);

            bindingSet.Bind(sendButton)
                      .To(vm => vm.SendCommentCommand);

            bindingSet.Bind(commentTextView)
                      .To(vm => vm.Comment);

            bindingSet.Bind(_refreshControl)
                      .For(v => v.IsRefreshing)
                      .To(vm => vm.IsBusy);

            bindingSet.Bind(_refreshControl)
                      .For(v => v.RefreshCommand)
                      .To(vm => vm.ReloadItemsCommand);

            bindingSet.Bind(profileImageView)
                      .For(v => v.ImagePath)
                      .To(vm => vm.ProfilePhotoUrl)
                      .Mode(MvxBindingMode.OneTime);

            bindingSet.Bind(profileImageView)
                      .For(v => v.PlaceholderText)
                      .To(vm => vm.ProfileShortName)
                      .Mode(MvxBindingMode.OneTime);

            bindingSet.Apply();
        }

		protected override void SetupControls()
		{
            Title = Resources.CommentView_Title;
            InitializeTableView();
            SetScrollKeyboard();

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

        private void SetScrollKeyboard()
        {
            Xamarin.IQKeyboardManager.SharedManager.ShouldResignOnTouchOutside = true;
            Xamarin.IQKeyboardManager.SharedManager.ShouldToolbarUsesTextFieldTintColor = true;
            Xamarin.IQKeyboardManager.SharedManager.KeyboardDistanceFromTextField = 10;
        }
    }
}