using System;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using Plugin.DeviceInfo;
using PrankChat.Mobile.iOS.Controls;
using PrankChat.Mobile.iOS.Utils.Helpers;
using UIKit;

namespace PrankChat.Mobile.iOS.AppTheme
{
    public static class Styles
    {
        public static void SetNavigationBarStyle(this UINavigationBar navigationBar)
        {
            navigationBar.Translucent = false;
        }

        public static void SetStyle(this UISegmentedControl segmentedControl, params string[] segmentNames)
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
                searchBar.SearchTextField.TextColor = Theme.Color.Text;
            }
            searchBar.TintColor = Theme.Color.Text;
        }

        public static void SetLightStyle(
            this UITextField textField,
            string placeholder = null,
            UIImage leftImage = null,
            UIImage rightImage = null,
            float leftPadding = 14,
            float rightPadding = 0)
        {
            textField.TextColor = Theme.Color.White;
            textField.BackgroundColor = UIColor.Clear;
            textField.TintColor = Theme.Color.White;
            textField.Layer.BorderColor = Theme.Color.White.CGColor;

            var placeholderAttributes = new UIStringAttributes
            {
                Font = Theme.Font.RegularFontOfSize(14),
                ForegroundColor = Theme.Color.White
            };

            textField.SetStyle(placeholderAttributes, placeholder, leftImage, rightImage, leftPadding, rightPadding);
        }

        public static void SetDarkStyle(
            this UITextField textField,
            string placeholder = null,
            UIImage leftImage = null,
            UIImage rightImage = null,
            float leftPadding = 14,
            float rightPadding = 0)
        {
            textField.TextColor = Theme.Color.Text;
            textField.BackgroundColor = UIColor.Clear;
            textField.TintColor = Theme.Color.Text;
            textField.Layer.BorderColor = Theme.Color.TextFieldDarkBorder.CGColor;

            var placeholderAttributes = new UIStringAttributes
            {
                Font = Theme.Font.RegularFontOfSize(14),
                ForegroundColor = Theme.Color.Subtitle
            };

            textField.SetStyle(placeholderAttributes, placeholder, leftImage, rightImage, leftPadding, rightPadding);
        }

        public static void SetStyle(
            this PlaceholderTextView textView,
            UIStringAttributes placeholderAttributes,
            string placeholder = null)
        {
            textView.AttributedPlaceholder = new NSAttributedString(placeholder ?? string.Empty, placeholderAttributes);
            textView.TextContainerInset = new UIEdgeInsets(17, 14, 17, 14);
            textView.TextContainer.LineFragmentPadding = 0;
            textView.ScrollEnabled = true;
            textView.Editable = true;
            textView.Selectable = true;
            textView.Font = Theme.Font.RegularFontOfSize(14);
        }

        public static void SetLightStyle(
            this PlaceholderTextView textView,
            string placeholder = null)
        {
            textView.TextColor = Theme.Color.White;
            textView.BackgroundColor = UIColor.Clear;
            textView.TintColor = Theme.Color.White;
            textView.Layer.BorderColor = Theme.Color.White.CGColor;

            var placeholderAttributes = new UIStringAttributes
            {
                Font = Theme.Font.RegularFontOfSize(14),
                ForegroundColor = Theme.Color.White
            };

            textView.SetStyle(placeholderAttributes, placeholder);
        }

        public static void SetDarkStyle(
            this PlaceholderTextView textView,
            string placeholder = null)
        {
            textView.TextColor = Theme.Color.Text;
            textView.BackgroundColor = UIColor.Clear;
            textView.TintColor = Theme.Color.Text;
            textView.Layer.BorderColor = Theme.Color.TextFieldDarkBorder.CGColor;

            var placeholderAttributes = new UIStringAttributes
            {
                Font = Theme.Font.RegularFontOfSize(14),
                ForegroundColor = Theme.Color.Subtitle
            };

            textView.SetStyle(placeholderAttributes, placeholder);
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

        public static void SetVideoListStyle(this UITableView tableView, int? estimatedCellHeight = null)
        {
            tableView.SetStyle(estimatedCellHeight);
            tableView.AllowsSelection = false;
            tableView.UserInteractionEnabled = true;
            tableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.OnDrag;
            tableView.SeparatorColor = Theme.Color.Separator;
            tableView.SeparatorStyle = UITableViewCellSeparatorStyle.DoubleLineEtched;
        }

        public static void SetCornerRadius(this UIView innerView, float radius = 2f, CACornerMask maskedCorners = (CACornerMask.MaxXMaxYCorner | CACornerMask.MaxXMinYCorner | CACornerMask.MinXMaxYCorner | CACornerMask.MinXMinYCorner))
        {
            innerView.Layer.CornerRadius = radius;
            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                innerView.Layer.MaskedCorners = maskedCorners;
            }
        }

        public static void SetPreviewStyle(this UIView imageView)
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

            label.SetMediumStyle(14, Theme.Color.Text);
        }

        public static void SetTitleStyle(this UILabel label, string text = null)
        {
            if (!string.IsNullOrEmpty(text))
                label.Text = text;

            label.SetRegularStyle(14, Theme.Color.Text);
        }

        public static void SetBoldTitleStyle(this UILabel label, string text = null)
        {
            if (!string.IsNullOrEmpty(text))
                label.Text = text;

            label.Font = Theme.Font.BoldOfSize(14);
            label.TextColor = Theme.Color.Text;
        }

        public static void SetSmallTitleStyle(this UILabel label, string text = null, int size = Theme.Font.MediumFontSize)
        {
            if (!string.IsNullOrEmpty(text))
                label.Text = text;

            label.SetMediumStyle(size, Theme.Color.Text);
        }

        public static void SetSmallSubtitleStyle(this UILabel label, string text = null, int size = 12)
        {
            if (!string.IsNullOrEmpty(text))
                label.Text = text;

            label.SetRegularStyle(size, Theme.Color.Subtitle);
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

        public static void SetBorderlessStyle(this UIView view, string title = "", UIColor borderColor = null, UIColor textColor = null)
        {
            if (borderColor != null)
            {
                view.Layer.BorderColor = borderColor.CGColor;
                view.Layer.BorderWidth = 1;
                view.Layer.CornerRadius = 4;
            }
            else
            {
                view.Layer.BorderColor = UIColor.Clear.CGColor;
            }

            view.BackgroundColor = UIColor.Clear;

            if (view is UIButton button)
            {
                var titleAttributes = new UIStringAttributes
                {
                    Font = Theme.Font.MediumOfSize(14),
                    ForegroundColor = textColor ?? Theme.Color.Accent
                };

                var attributedTitle = new NSAttributedString(title, titleAttributes);
                button.SetAttributedTitle(attributedTitle, UIControlState.Normal);
            }
        }

        public static void SetSelectableImageStyle(this UIButton button, string normalImage, string selectedImage)
        {
            button.TintColor = UIColor.Clear;

            button.SetImage(UIImage.FromBundle(normalImage)
                .ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal), UIControlState.Normal);

            button.SetImage(UIImage.FromBundle(selectedImage)
                .ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal), UIControlState.Selected);
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

        private static UITextField SetStyle(
            this UITextField textField,
            UIStringAttributes placeholderAttributes,
            string placeholder = null,
            UIImage leftImage = null,
            UIImage rightImage = null,
            float leftPadding = 14,
            float rightPadding = 14)
        {

            textField.Layer.BorderWidth = 1;
            textField.Layer.CornerRadius = 3;

            textField.AttributedPlaceholder = new NSAttributedString(placeholder ?? string.Empty, placeholderAttributes);

            var leftPaddingView = new UIView(new CGRect(0, 0, leftPadding, textField.Frame.Height));
            textField.LeftView = leftPaddingView;
            textField.LeftViewMode = UITextFieldViewMode.Always;

            var rightPaddingView = new UIView(new CGRect(0, 0, rightPadding, textField.Frame.Height));
            textField.RightView = rightPaddingView;
            textField.RightViewMode = UITextFieldViewMode.Always;

            textField.TrySetRightImage(rightImage);
            textField.TrySetLeftImage(leftImage);

            return textField;
        }

        private static UITextField TrySetLeftImage(this UITextField textField, UIImage image, int leftPadding = 7, int rightPadding = 15)
        {
            if (image != null)
            {
                var imageView = new UIImageView(new CGRect(leftPadding, 0, image.Size.Width, image.Size.Height));
                imageView.Image = image;
                var imageContainer = new UIView(new CGRect(0, 0, leftPadding + image.Size.Width + rightPadding, image.Size.Height));
                imageContainer.ContentMode = UIViewContentMode.Left;
                imageContainer.AddSubview(imageView);
                textField.LeftView = imageContainer;
                textField.LeftViewMode = UITextFieldViewMode.Always;
            }

            return textField;
        }

        private static UITextField TrySetRightImage(this UITextField textField, UIImage image, int leftPadding = 10, int rightPadding = 16)
        {
            if (image != null)
            {
                var imageView = new UIImageView(new CGRect(leftPadding, 0, image.Size.Width, image.Size.Height));
                imageView.Image = image;
                var imageContainer = new UIView(new CGRect(0, 0, leftPadding + image.Size.Width + rightPadding, image.Size.Height));
                imageContainer.ContentMode = UIViewContentMode.Right;
                imageContainer.AddSubview(imageView);
                textField.RightView = imageContainer;
                textField.RightViewMode = UITextFieldViewMode.Always;
            }

            return textField;
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
