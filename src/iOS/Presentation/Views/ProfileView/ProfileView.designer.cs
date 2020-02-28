// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.ProfileView
{
    [Register("ProfileView")]
    partial class ProfileView
    {
        [Outlet]
        UIKit.UIScrollView rootScrollView { get; set; }

        [Outlet]
        UIKit.UITableView tableView { get; set; }

        [Outlet]
        FFImageLoading.Cross.MvxCachedImageView profileImageView { get; set; }

        [Outlet]
        FFImageLoading.Cross.MvxCachedImageView changeProfileImageView { get; set; }

        [Outlet]
        UIKit.UILabel profileShortNameLabel { get; set; }

        [Outlet]
        UIKit.UILabel profileDescriptionLabel { get; set; }

        [Outlet]
        UIKit.UILabel priceLabel { get; set; }

        [Outlet]
        UIKit.UIButton refillButton { get; set; }

        [Outlet]
        UIKit.UIButton withdrawalButton { get; set; }

        [Outlet]
        UIKit.UILabel ordersValueLabel { get; set; }

        [Outlet]
        UIKit.UILabel ordersTitleLabel { get; set; }

        [Outlet]
        UIKit.UILabel completedOrdersValueLabel { get; set; }

        [Outlet]
        UIKit.UILabel completedOrdersTitleLabel { get; set; }

        [Outlet]
        UIKit.UILabel subscribersValueLabel { get; set; }

        [Outlet]
        UIKit.UILabel subscribersTitleLabel { get; set; }

        [Outlet]
        UIKit.UILabel subscriptionsValueLabel { get; set; }

        [Outlet]
        UIKit.UILabel subscriptionsTitleLabel { get; set; }

        [Outlet]
        UIKit.UISegmentedControl segmentedControl { get; set; }

        void ReleaseDesignerOutlets()
        {
            if (rootScrollView != null)
            {
                rootScrollView.Dispose();
                rootScrollView = null;
            }
            if (tableView != null)
            {
                tableView.Dispose();
                tableView = null;
            }
            if (profileImageView != null)
            {
                profileImageView.Dispose();
                profileImageView = null;
            }
            if (changeProfileImageView != null)
            {
                changeProfileImageView.Dispose();
                changeProfileImageView = null;
            }
            if (profileShortNameLabel != null)
            {
                profileShortNameLabel.Dispose();
                profileShortNameLabel = null;
            }
            if (profileDescriptionLabel != null)
            {
                profileDescriptionLabel.Dispose();
                profileDescriptionLabel = null;
            }
            if (priceLabel != null)
            {
                priceLabel.Dispose();
                priceLabel = null;
            }
            if (refillButton != null)
            {
                refillButton.Dispose();
                refillButton = null;
            }
            if (withdrawalButton != null)
            {
                withdrawalButton.Dispose();
                withdrawalButton = null;
            }
            if (ordersValueLabel != null)
            {
                ordersValueLabel.Dispose();
                ordersValueLabel = null;
            }
            if (ordersTitleLabel != null)
            {
                ordersTitleLabel.Dispose();
                ordersTitleLabel = null;
            }
            if (completedOrdersTitleLabel != null)
            {
                completedOrdersTitleLabel.Dispose();
                completedOrdersTitleLabel = null;
            }
            if (completedOrdersValueLabel != null)
            {
                completedOrdersValueLabel.Dispose();
                completedOrdersValueLabel = null;
            }
            if (subscribersValueLabel != null)
            {
                subscribersValueLabel.Dispose();
                subscribersValueLabel = null;
            }
            if (subscribersTitleLabel != null)
            {
                subscribersTitleLabel.Dispose();
                subscribersTitleLabel = null;
            }
            if (subscriptionsValueLabel != null)
            {
                subscriptionsValueLabel.Dispose();
                subscriptionsValueLabel = null;
            }
            if (subscriptionsTitleLabel != null)
            {
                subscriptionsTitleLabel.Dispose();
                subscriptionsTitleLabel = null;
            }
            if (segmentedControl != null)
            {
                segmentedControl.Dispose();
                segmentedControl = null;
            }
        }
    }
}
