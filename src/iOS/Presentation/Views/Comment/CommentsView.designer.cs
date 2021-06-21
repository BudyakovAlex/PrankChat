// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace PrankChat.Mobile.iOS.Presentation.Views.Comment
{
	[Register ("CommentsView")]
	partial class CommentsView
	{
		[Outlet]
		UIKit.UITextView commentTextView { get; set; }

		[Outlet]
		UIKit.UIView commentView { get; set; }

		[Outlet]
		UIKit.UIView commentViewSeparatorView { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint editorBottomConstraint { get; set; }

		[Outlet]
		PrankChat.Mobile.iOS.Controls.CircleCachedImageView profileImageView { get; set; }

		[Outlet]
		UIKit.UIButton sendButton { get; set; }

		[Outlet]
		UIKit.UITableView tableView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (editorBottomConstraint != null) {
				editorBottomConstraint.Dispose ();
				editorBottomConstraint = null;
			}

			if (commentTextView != null) {
				commentTextView.Dispose ();
				commentTextView = null;
			}

			if (commentView != null) {
				commentView.Dispose ();
				commentView = null;
			}

			if (commentViewSeparatorView != null) {
				commentViewSeparatorView.Dispose ();
				commentViewSeparatorView = null;
			}

			if (profileImageView != null) {
				profileImageView.Dispose ();
				profileImageView = null;
			}

			if (sendButton != null) {
				sendButton.Dispose ();
				sendButton = null;
			}

			if (tableView != null) {
				tableView.Dispose ();
				tableView = null;
			}
		}
	}
}
