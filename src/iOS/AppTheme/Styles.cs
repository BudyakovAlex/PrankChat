using System;
using UIKit;
using CoreAnimation;
using CoreGraphics;

namespace PrankChat.Mobile.iOS.AppTheme
{
	public static class Styles
	{
		public static void SetNavigationBarStyle(this UINavigationBar navigationBar)
		{
			navigationBar.Translucent = false;
		}

		public static void SetSegmentedControlStyle(this UISegmentedControl segmentedControl, params string[] segmentNames)
		{
			segmentedControl.RemoveAllSegments();

			for (int i = 0; i < segmentNames.Length; i++)
			{
				segmentedControl.InsertSegment(segmentNames[i], i, false);
			}
			segmentedControl.SetTitleTextAttributes(
				new UITextAttributes
				{
					TextColor = UIColor.White,
					Font = Theme.Font.RegularFontOfSize(12)
				},
				UIControlState.Normal);
			segmentedControl.TintColor = Theme.Color.Accent;
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
            tabBarItem.ImageInsets = new UIEdgeInsets(0, 0, 0, 0);
        }

        public static void SetTabBarStyle(this UITabBar tabBar)
        {
            tabBar.TintColor = Theme.Color.Accent;
            tabBar.UnselectedItemTintColor = Theme.Color.Inactive;
            tabBar.BarTintColor = Theme.Color.White;
        }

        public static void SetStyle(this UINavigationBar navigationBar)
        {
            var gradientContainer = new UIView();
            gradientContainer.Frame = navigationBar.Frame;
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
            navigationBar.ShadowImage = new UIImage();
            navigationBar.BarStyle = UIBarStyle.BlackTranslucent;
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
