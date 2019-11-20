using System;
using UIKit;

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
	}
}
