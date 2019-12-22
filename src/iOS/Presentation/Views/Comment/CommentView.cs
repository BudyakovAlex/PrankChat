﻿using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Comment;
using PrankChat.Mobile.Core.Presentation.ViewModels.Publication;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Comment
{
	public partial class CommentView : BaseGradientBarView<CommentsViewModel>
	{
        public CommentTableSource CommentTableSource { get; private set; }

        protected override void SetupBinding()
		{
            var set = this.CreateBindingSet<CommentView, CommentsViewModel>();

            set.Bind(CommentTableSource)
                .To(vm => vm.Items);

            set.Bind(sendButton)
                .To(vm => vm.SendCommentCommand);

            set.Bind(commentTextView)
                .To(vm => vm.Comment);

            set.Apply();
        }

		protected override void SetupControls()
		{
            Title = Resources.CommentView_Title;
            InitializeTableView();
        }

        private void InitializeTableView()
        {
            CommentTableSource = new CommentTableSource(tableView);
            tableView.Source = CommentTableSource;
            tableView.RegisterNibForCellReuse(CommentItemCell.Nib, CommentItemCell.CellId);
            tableView.SetStyle();
            tableView.RowHeight = CommentItemCell.EstimatedHeight;
            tableView.UserInteractionEnabled = true;
            tableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag;

            tableView.AllowsSelection = false;
        }
    }
}

