﻿using System;
using UIKit;

namespace PrankChat.Mobile.iOS.AppTheme
{
    public class Theme
    {
        public class Color
        {
            public static UIColor Black => UIColor.FromRGB(0, 0, 0);
            public static UIColor White => UIColor.FromRGB(255, 255, 255);
            public static UIColor Accent => UIColor.FromRGBA(0.427f, 0.157f, 0.745f, 1);
            public static UIColor Inactive => UIColor.FromRGBA(0.129f, 0.129f, 0.129f, 1);
            public static UIColor GradientHeaderStart => UIColor.FromRGBA(0.231f, 0.553f, 0.929f, 1);
            public static UIColor GradientHeaderEnd => UIColor.FromRGBA(0.427f, 0.157f, 0.745f, 1);
            public static UIColor FilterText => UIColor.FromRGBA(0.129f, 0.129f, 0.129f, 1);
            public static UIColor Separator => UIColor.FromRGBA(0.965f, 0.965f, 0.965f, 1);
            public static UIColor Text => UIColor.FromRGBA(0.0f, 0.0f, 0.0f, 0.87f);
            public static UIColor Subtitle => UIColor.FromRGBA(0.62f, 0.62f, 0.62f, 1.0f);
            public static UIColor ButtonBorderPrimary => UIColor.FromRGBA(0.38f, 0.18f, 0.61f, 1.0f);
            public static UIColor TextFieldDarkBorder = UIColor.FromRGBA(0.76f, 0.76f, 0.76f, 1.0f);
            public static UIColor CommentBorder = UIColor.FromRGBA(0.0f, 0.0f, 0.0f, 0.24f);
        }

        public class Font
        {
            public static UIFont BlackOfSize(nfloat fontSize)
            {
                return UIFont.SystemFontOfSize(fontSize, UIFontWeight.Black);
            }

            public static UIFont BoldOfSize(nfloat fontSize)
            {
                return UIFont.SystemFontOfSize(fontSize, UIFontWeight.Bold);
            }

            public static UIFont LightOfSize(nfloat fontSize)
            {
                return UIFont.SystemFontOfSize(fontSize, UIFontWeight.Light);
            }

            public static UIFont MediumOfSize(nfloat fontSize)
            {
                return UIFont.SystemFontOfSize(fontSize, UIFontWeight.Medium);
            }

            public static UIFont RegularFontOfSize(nfloat fontSize)
            {
                return UIFont.SystemFontOfSize(fontSize, UIFontWeight.Regular);
            }

            public static UIFont ThinFontOfSize(nfloat fontSize)
            {
                return UIFont.SystemFontOfSize(fontSize, UIFontWeight.Thin);
            }
        }
    }
}
