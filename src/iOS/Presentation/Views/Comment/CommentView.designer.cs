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
	[Register ("CommentView")]
	partial class CommentView
    {
		[Outlet]
		UIKit.UITextView commentTextView { get; set; }

		[Outlet]
		UIKit.UIView commentView { get; set; }

		[Outlet]
		UIKit.UIButton sendButton { get; set; }

		[Outlet]
		UIKit.UITableView tableView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (tableView != null) {
				tableView.Dispose ();
				tableView = null;
			}

			if (commentView != null) {
				commentView.Dispose ();
				commentView = null;
			}

			if (commentTextView != null) {
				commentTextView.Dispose ();
				commentTextView = null;
			}

			if (sendButton != null) {
				sendButton.Dispose ();
				sendButton = null;
			}
		}
	}
}
