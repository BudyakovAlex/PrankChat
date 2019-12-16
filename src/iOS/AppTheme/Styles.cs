using System;
using UIKit;
using CoreAnimation;
using CoreGraphics;
using Plugin.DeviceInfo;
using PrankChat.Mobile.iOS.Utils.Helpers;
using Foundation;

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

        public static void SetLightStyle(this UITextField textField)
        {
            textField.TextColor = Theme.Color.White;
            textField.BackgroundColor = UIColor.Clear;
            textField.TintColor = Theme.Color.White;
            textField.Layer.BorderColor = Theme.Color.White.CGColor;
            textField.Layer.BorderWidth = 1;
            textField.Layer.CornerRadius = 3;

            var placeholderAttributes = new UIStringAttributes
            {
                Font = Theme.Font.RegularFontOfSize(14),
                ForegroundColor = Theme.Color.White
            };

            textField.AttributedPlaceholder = new NSAttributedString(textField.Placeholder, placeholderAttributes);

            var paddingView = new UIView(new CGRect(0, 0, 14, textField.Frame.Height));
            textField.LeftView = paddingView;
            textField.LeftViewMode = UITextFieldViewMode.Always;
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
            backgroundLayer.Bounds = view.Bounds.Inset(-1f * view.Bounds.Size.Width, -1f * view.Bounds.Size.Height);
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

        public static void SetScreenTitleStyle(this UILabel label)
        {
            var attributes = new UIStringAttributes
            {
                Font = Theme.Font.MediumOfSize(14),
                ForegroundColor = Theme.Color.White
            };

            label.AttributedText = new NSAttributedString(label.Text, attributes);
        }

        public static void SetMainTitleStyle(this UILabel label, string text = null)
        {
            if (!string.IsNullOrEmpty(text))
                label.Text = text;

            label.SetMediumStyle(14, Theme.Color.Title);
        }

        public static void SetTitleStyle(this UILabel label, string text = null)
        {
            if (!string.IsNullOrEmpty(text))
                label.Text = text;

            label.SetRegularStyle(14, Theme.Color.Title);
        }

        public static void SetBoldTitleStyle(this UILabel label, string text = null)
        {
            if (!string.IsNullOrEmpty(text))
                label.Text = text;

            label.Font = Theme.Font.BoldOfSize(14);
            label.TextColor = Theme.Color.Title;
        }

        public static void SetSmallTitleStyle(this UILabel label, string text = null)
        {
            if (!string.IsNullOrEmpty(text))
                label.Text = text;

            label.SetMediumStyle(12, Theme.Color.Title);
        }

        public static void SetSmallSubtitleStyle(this UILabel label, string text = null)
        {
            if (!string.IsNullOrEmpty(text))
                label.Text = text;

            label.SetRegularStyle(12, Theme.Color.Subtitle);
        }

        public static void SetMediumStyle(this UILabel label, int size, UIColor color)
        {
            label.Font = Theme.Font.MediumOfSize(size);
            label.TextColor = color;
        }

        public static void SetRegularStyle(this UILabel label, int size, UIColor color)
        {
            label.Font = Theme.Font.RegularFontOfSize(size);
            label.TextColor = color;
        }

        public static void SetLinkStyle(this UIButton button, UIFont font)
        {
            var titleAttributes = new UIStringAttributes
            {
                Font = font,
                ForegroundColor = Theme.Color.White,
                UnderlineStyle = NSUnderlineStyle.Single,
                UnderlineColor = Theme.Color.White
            };

            var attributedTitle = new NSAttributedString(button.Title(UIControlState.Normal), titleAttributes);
            button.SetAttributedTitle(attributedTitle, UIControlState.Normal);
        }

        public static void SetLightStyle(this UIButton button, string title)
        {
            button.Layer.BorderColor = Theme.Color.ButtonBorderPrimary.CGColor;
            button.Layer.BorderWidth = 1;
            button.Layer.CornerRadius = 4;
            button.BackgroundColor = Theme.Color.White;

            var titleAttributes = new UIStringAttributes
            {
                Font = Theme.Font.MediumOfSize(14),
                ForegroundColor = Theme.Color.Accent
            };

            var attributedTitle = new NSAttributedString(title, titleAttributes);
            button.SetAttributedTitle(attributedTitle, UIControlState.Normal);
        }

        public static void SetDarkStyle(this UIButton button, string title)
        {
            button.Layer.BorderColor = Theme.Color.Accent.CGColor;
            button.Layer.BorderWidth = 1;
            button.Layer.CornerRadius = 4;
            button.BackgroundColor = Theme.Color.Accent;

            var titleAttributes = new UIStringAttributes
            {
                Font = Theme.Font.MediumOfSize(14),
                ForegroundColor = Theme.Color.White
            };

            var attributedTitle = new NSAttributedString(title, titleAttributes);
            button.SetAttributedTitle(attributedTitle, UIControlState.Normal);
        }

        public static void SetBorderlessStyle(this UIButton button, string title, UIColor borderColor = null)
        {
            if (borderColor != null)
            {
                button.Layer.BorderColor = borderColor.CGColor;
                button.Layer.BorderWidth = 1;
                button.Layer.CornerRadius = 4;
            }
            else
            {
                button.Layer.BorderColor = UIColor.Clear.CGColor;
            }

            button.BackgroundColor = UIColor.Clear;

            var titleAttributes = new UIStringAttributes
            {
                Font = Theme.Font.MediumOfSize(14),
                ForegroundColor = Theme.Color.Accent
            };

            var attributedTitle = new NSAttributedString(title, titleAttributes);
            button.SetAttributedTitle(attributedTitle, UIControlState.Normal);
        }

        public static void SetRadioInactiveStyle(this UIButton button, float padding = 8f)
        {
            button.SetImage(UIImage.FromBundle("ic_radio_button_inactive")
                .ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal), UIControlState.Normal);
        }

        public static void SetRadioActiveStyle(this UIButton button, float padding = 8f)
        {
            button.SetImage(UIImage.FromBundle("ic_radio_button_active")
                .ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal), UIControlState.Normal);
        }

        public static void SetRadioTitleStyle(this UIButton button)
        {
            var titleAttributes = new UIStringAttributes
            {
                Font = Theme.Font.RegularFontOfSize(14),
                ForegroundColor = Theme.Color.White,
            };

            var title = button.Title(UIControlState.Normal);
            title = title.ToLowerInvariant();
            var attributedTitle = new NSAttributedString(title, titleAttributes);
            button.SetAttributedTitle(attributedTitle, UIControlState.Normal);
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
