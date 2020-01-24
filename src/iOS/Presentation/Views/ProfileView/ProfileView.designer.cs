// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using FFImageLoading.Cross;
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace PrankChat.Mobile.iOS.Presentation.Views.ProfileView
{
    [Register ("ProfileView")]
    partial class ProfileView
    {
        [Outlet]
        UIKit.UILabel completedOrdersTitleLabel { get; set; }


        [Outlet]
        UIKit.UILabel completedOrdersValueLabel { get; set; }


        [Outlet]
        UIKit.UILabel ordersTitleLabel { get; set; }


        [Outlet]
        UIKit.UILabel ordersValueLabel { get; set; }


        [Outlet]
        UIKit.UILabel priceLabel { get; set; }


        [Outlet]
        UIKit.UILabel profileDescriptionLabel { get; set; }


        [Outlet]
        MvxCachedImageView profileImageView { get; set; }


        [Outlet]
        UIKit.UIButton refillButton { get; set; }


        [Outlet]
        UIKit.UISegmentedControl segmentedControl { get; set; }


        [Outlet]
        UIKit.UILabel subscribersTitleLabel { get; set; }


        [Outlet]
        UIKit.UILabel subscribersValueLabel { get; set; }


        [Outlet]
        UIKit.UILabel subscriptionsTitleLabel { get; set; }


        [Outlet]
        UIKit.UILabel subscriptionsValueLabel { get; set; }


        [Outlet]
        UIKit.UITableView tableView { get; set; }


        [Outlet]
        UIKit.UIButton withdrawalButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIActivityIndicatorView progressBar { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (progressBar != null) {
                progressBar.Dispose ();
                progressBar = null;
            }
        }
    }
}