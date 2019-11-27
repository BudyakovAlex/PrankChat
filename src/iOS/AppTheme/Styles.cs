﻿using System;
using UIKit;
using CoreAnimation;
using CoreGraphics;
using Plugin.DeviceInfo;
using PrankChat.Mobile.iOS.Utils.Helpers;

namespace PrankChat.Mobile.iOS.AppTheme
{
    public static class Styles
    {
        public static void SetNavigationBarStyle(this UINavigationBar navigationBar)
        {
            navigationBar.Translucent = false;
        }

        public static void SetPublicationSegmentedControlStyle(this UISegmentedControl segmentedControl, params string[] segmentNames)
        {
            segmentedControl.RemoveAllSegments();

            for (int i = 0; i < segmentNames.Length; i++)
            {
                segmentedControl.InsertSegment(segmentNames[i], i, false);
            }

            segmentedControl.SetTitleTextAttributes(
                new UITextAttributes
                {
                    TextColor = Theme.Color.Accent,
                    Font = Theme.Font.RegularFontOfSize(14),
                },
                UIControlState.Normal);

            segmentedControl.SetTitleTextAttributes(
                new UITextAttributes
                {
                    TextColor = Theme.Color.White,
                    Font = Theme.Font.RegularFontOfSize(14),
                },
                UIControlState.Selected);

            segmentedControl.TintColor = Theme.Color.Accent;
            segmentedControl.BackgroundColor = Theme.Color.White;
            segmentedControl.Layer.BorderColor = Theme.Color.Accent.CGColor;
            segmentedControl.Layer.BorderWidth = 1;

            if (CrossDeviceInfo.Current.VersionNumber > new Version(13, 0))
            {
                UIImage image(UIColor color)
                {
                    return UIImageUtil.ImageWithColor(color, segmentedControl.Frame.Size);
                }

                UIImage imageDivider(UIColor color)
                {
                    return UIImageUtil.ImageWithColor(color, new CGSize(1, segmentedControl.Frame.Height));
                }

                // Must set the background image for normal to something (even clear) else the rest won't work.
                segmentedControl.SetBackgroundImage(image(UIColor.Clear), UIControlState.Normal, UIBarMetrics.Default);
                segmentedControl.SetBackgroundImage(image(Theme.Color.Accent), UIControlState.Selected, UIBarMetrics.Default);
                segmentedControl.SetBackgroundImage(image(Theme.Color.Accent.ColorWithAlpha(0.2f)), UIControlState.Highlighted, UIBarMetrics.Default);
                segmentedControl.SetBackgroundImage(image(Theme.Color.Accent), UIControlState.Highlighted | UIControlState.Selected, UIBarMetrics.Default);
                segmentedControl.SetDividerImage(imageDivider(Theme.Color.Accent), UIControlState.Normal, UIControlState.Normal, UIBarMetrics.Default);
            }
        }

        public static void SetTabBarItemStyle(this UITabBarItem tabBarItem)
        {
            tabBarItem.ImageInsets = new UIEdgeInsets(0, 0, 0, 0);
            tabBarItem.TitlePositionAdjustment = new UIOffset(0, 0);
            var textAttributes = new UITextAttributes { Font = Theme.Font.RegularFontOfSize(12) };
            tabBarItem.SetTitleTextAttributes(textAttributes, UIControlState.Normal);
        }

        public static void SetCentralTabBarItemStyle(this UITabBarItem tabBarItem)
        {
            tabBarItem.Title = string.Empty;
            var padding = CrossDeviceInfo.Current.VersionNumber.Major <= 12 ? 6 : 0;
            tabBarItem.ImageInsets = new UIEdgeInsets(padding, 0, -padding, 0);
        }

        public static void SetTabBarStyle(this UITabBar tabBar)
        {
            tabBar.TintColor = Theme.Color.Accent;
            tabBar.UnselectedItemTintColor = Theme.Color.Inactive;
            tabBar.BarTintColor = Theme.Color.White;
        }

        public static void SetGradientStyle(this UINavigationBar navigationBar)
        {
            var statusBarHeight = UIApplication.SharedApplication.StatusBarFrame.Height;
            var fullHeight = navigationBar.Frame.Size.Width + statusBarHeight;
            var gradientContainer = new UIView();
            gradientContainer.Frame = new CGRect(navigationBar.Frame.Location, new CGSize(navigationBar.Frame.Size.Width, fullHeight));
            gradientContainer.ClipsToBounds = true;

            var gradient = new CAGradientLayer();
            gradient.Colors = new CGColor[]
            {
                Theme.Color.GradientHeaderStart.CGColor,
                Theme.Color.GradientHeaderEnd.CGColor
            };

            gradient.Locations = new Foundation.NSNumber[] { 0, 1 };
            gradient.StartPoint = new CGPoint(x: 0.25, y: 0.5);
            gradient.EndPoint = new CGPoint(x: 0.75, y: 0.5);
            gradient.Transform = CATransform3D.MakeFromAffine(new CGAffineTransform(-0.74f, 0.71f, -0.71f, -0.74f, 1.2f, 0.49f));
            gradient.Bounds = gradientContainer.Bounds.Inset(-0.5f * gradientContainer.Bounds.Size.Height, -0.5f * gradientContainer.Bounds.Size.Width);
            gradient.Position = gradientContainer.Center;
            gradientContainer.Layer.AddSublayer(gradient);
            navigationBar.SetBackgroundImage(GetNavigationBarBackgroundImage(gradientContainer), UIBarMetrics.Default);
            navigationBar.BarStyle = UIBarStyle.BlackTranslucent;
        }

        public static void SetTransparentStyle(this UINavigationBar navigationBar)
        {
            var statusBarHeight = UIApplication.SharedApplication.StatusBarFrame.Height;
            var fullHeight = navigationBar.Frame.Size.Width + statusBarHeight;
            var transparentContainer = new UIView();
            transparentContainer.Frame = new CGRect(navigationBar.Frame.Location, new CGSize(navigationBar.Frame.Size.Width, fullHeight));
            transparentContainer.ClipsToBounds = true;

            var transparentLayer = new CALayer();
            transparentLayer.BackgroundColor = UIColor.Clear.CGColor;

            transparentLayer.Position = transparentContainer.Center;
            transparentContainer.Layer.AddSublayer(transparentLayer);
            navigationBar.SetBackgroundImage(GetNavigationBarBackgroundImage(transparentContainer), UIBarMetrics.Default);
            navigationBar.BarStyle = UIBarStyle.BlackTranslucent;
            navigationBar.ShadowImage = new UIImage();
        }

        public static void SetStyle(this UISearchBar searchBar)
        {
            if (CrossDeviceInfo.Current.VersionNumber > new Version(13, 0))
            {
                searchBar.SearchTextField.BackgroundColor = Theme.Color.White;
                searchBar.SearchTextField.TextColor = Theme.Color.Title;
            }
            searchBar.TintColor = Theme.Color.Title;
        }

        public static void SetGradientBackground(this UIView view)
        {
            var containerView = new UIView(view.Frame);

            var backgroundLayer = new CAGradientLayer();

            backgroundLayer.Colors = new CGColor[] {
              new UIColor(0.231f, 0.553f, 0.929f, 1).CGColor,
              new UIColor(0.427f, 0.157f, 0.745f, 1).CGColor
            };

            backgroundLayer.Locations = new Foundation.NSNumber[] { 0, 1 };
            backgroundLayer.StartPoint = new CGPoint(x: 0.25, y: 0.5);
            backgroundLayer.EndPoint = new CGPoint(x: 0.75, y: 0.5);
            backgroundLayer.Transform = CATransform3D.MakeFromAffine(new CGAffineTransform(0, 1, -1, 0, 1, 0));
            backgroundLayer.Bounds = view.Bounds.Inset(-0.5f * view.Bounds.Size.Width, -0.5f * view.Bounds.Size.Height);
            backgroundLayer.Position = view.Center;
            containerView.Layer.AddSublayer(backgroundLayer);

            view.InsertSubview(containerView, 0);
        }

        /// <summary>
        /// Set style for UITableView.
        /// </summary>
        /// <param name="tableView"></param>
        /// <param name="estimatedCellHeight">
        /// If this parameter doesn't set, table
        /// will trying set height of rows based
        /// on cell content.
        /// </param>
        public static void SetStyle(this UITableView tableView, int? estimatedCellHeight = null)
        {
            if (estimatedCellHeight.HasValue)
            {
                tableView.EstimatedRowHeight = estimatedCellHeight.Value;
                tableView.RowHeight = UITableView.AutomaticDimension;
            }

            tableView.AllowsSelection = true;
            tableView.BackgroundColor = Theme.Color.White;
            tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            tableView.ContentInset = new UIEdgeInsets(1, 0, 10, 0);
        }

        public static void SetCornerRadius(this UIView innerView, float radius = 2f, CACornerMask maskedCorners = (CACornerMask.MaxXMaxYCorner | CACornerMask.MaxXMinYCorner | CACornerMask.MinXMaxYCorner | CACornerMask.MinXMinYCorner))
        {
            innerView.Layer.CornerRadius = radius;
            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                innerView.Layer.MaskedCorners = maskedCorners;
            }
        }

        public static void SetPreviewStyle(this UIImageView imageView)
        {
            imageView.BackgroundColor = UIColor.Clear;
            imageView.ContentMode = UIViewContentMode.ScaleAspectFill;
            imageView.SetCornerRadius(10);
        }

        public static void SetMainTitleStyle(this UILabel label)
        {
            label.Font = Theme.Font.MediumOfSize(14);
            label.TextColor = Theme.Color.Title;
        }

        public static void SetTitleStyle(this UILabel label)
        {
            label.Font = Theme.Font.RegularFontOfSize(14);
            label.TextColor = Theme.Color.Title;
        }

        public static void SetSmallTitleStyle(this UILabel label)
        {
            label.Font = Theme.Font.MediumOfSize(12);
            label.TextColor = Theme.Color.Title;
        }

        public static void SetSmallSubtitleStyle(this UILabel label)
        {
            label.Font = Theme.Font.RegularFontOfSize(12);
            label.TextColor = Theme.Color.Subtitle;
        }

        private static UIImage GetNavigationBarBackgroundImage(UIView gradientLayer)
        {
            UIImage resultImage = null;
            UIGraphics.BeginImageContext(gradientLayer.Frame.Size);
            var context = UIGraphics.GetCurrentContext();
            if (context != null)
            {
                gradientLayer.Layer.RenderInContext(context);
                resultImage = UIGraphics.GetImageFromCurrentImageContext();
            }

            UIGraphics.EndImageContext();
            return resultImage;
        }
    }
}
