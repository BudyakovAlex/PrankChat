using System;
using System.ComponentModel;
using CoreGraphics;
using Foundation;
using UIKit;

namespace PrankChat.Mobile.iOS.Controls
{
	[Register("CustomSegmentedControl"), DesignTimeVisible(true)]
	public class CustomSegmentedControl : UISegmentedControl
	{
		#region Constructions

		public CustomSegmentedControl()
		{
		}

		public CustomSegmentedControl(params object[] args) : base(args)
		{
		}

		public CustomSegmentedControl(params UIImage[] images) : base(images)
		{
		}

		public CustomSegmentedControl(params NSString[] strings) : base(strings)
		{
		}

		public CustomSegmentedControl(params string[] strings) : base(strings)
		{
		}

		public CustomSegmentedControl(NSCoder coder) : base(coder)
		{
		}

		public CustomSegmentedControl(NSArray items) : base(items)
		{
		}

		public CustomSegmentedControl(CGRect frame) : base(frame)
		{
		}

		protected CustomSegmentedControl(NSObjectFlag t) : base(t)
		{
		}

		protected internal CustomSegmentedControl(IntPtr handle) : base(handle)
		{
		}

		#endregion

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			this.Layer.CornerRadius = 0;
		}
	}
}
