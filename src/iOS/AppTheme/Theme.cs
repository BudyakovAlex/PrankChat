using System;
using UIKit;

namespace PrankChat.Mobile.iOS.AppTheme
{
	public class Theme
	{
		public class Color
		{
			public static UIColor Accent => UIColor.FromRGB(109, 40, 190);
		}

		public class Font
		{
			public static UIFont BlackOfSize(nfloat fontSize)
			{
				return UIFont.FromName("Roboto-Black", fontSize);
			}

			public static UIFont BoldOfSize(nfloat fontSize)
			{
				return UIFont.FromName("Roboto-Bold", fontSize);
			}

			public static UIFont LightOfSize(nfloat fontSize)
			{
				return UIFont.FromName("Roboto-Light", fontSize);
			}

			public static UIFont MediumOfSize(nfloat fontSize)
			{
				return UIFont.FromName("Roboto-Medium", fontSize);
			}

			public static UIFont RegularFontOfSize(nfloat fontSize)
			{
				return UIFont.FromName("Roboto-Regular", fontSize);
			}

			public static UIFont ThinFontOfSize(nfloat fontSize)
			{
				return UIFont.FromName("Roboto-Thin", fontSize);
			}
		}
	}
}
