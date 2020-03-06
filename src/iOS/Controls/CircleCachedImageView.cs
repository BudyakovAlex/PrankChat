﻿using System;
using System.ComponentModel;
using CoreGraphics;
using FFImageLoading.Cross;
using Foundation;
using PrankChat.Mobile.iOS.AppTheme;
using UIKit;

namespace PrankChat.Mobile.iOS.Controls
{
    [Register("CircleCachedImageView"), DesignTimeVisible(true)]
    public class CircleCachedImageView : MvxCachedImageView
    {
        private UILabel _placeholderLabel;

        private string _placeholderText;
        public string PlaceholderText
        {
            get => _placeholderText;
            set
            {
                _placeholderText = value;
                SetPlaceholderText(_placeholderText);
            }
        }

        public CircleCachedImageView()
        {
            Initilize();
        }

        public CircleCachedImageView(IntPtr handle) : base(handle)
        {
            Initilize();
        }

        public CircleCachedImageView(CGRect frame) : base(frame)
        {
            Initilize();
        }

        public CircleCachedImageView(UILabel placeholderLabel)
        {
            _placeholderLabel = placeholderLabel;
        }

        public override void RemoveFromSuperview()
        {
            OnSuccess -= CircleCachedImageView_OnSuccess;
            base.RemoveFromSuperview();
        }

        private void Initilize()
        {
            DownsampleHeight = 50;
            DownsampleWidth = 50;

            BackgroundColor = Theme.Color.Accent;
            Layer.CornerRadius = Bounds.Height / 2;
            ContentMode = UIViewContentMode.ScaleAspectFill;

            InitilizePlaceholder();
            OnSuccess += CircleCachedImageView_OnSuccess;
        }

        private void CircleCachedImageView_OnSuccess(object sender, FFImageLoading.Args.SuccessEventArgs e)
        {
            InvokeOnMainThread(() =>
            {
                _placeholderLabel.Alpha = 0;
            });
        }

        private void InitilizePlaceholder()
        {
            _placeholderLabel = new UILabel
            {
                Font = UIFont.BoldSystemFontOfSize(16),
                TextColor = UIColor.White
            };
            AddSubview(_placeholderLabel);
        }

        private void SetPlaceholderText(string placeholder)
        {
            if (string.IsNullOrWhiteSpace(placeholder))
                return;

            _placeholderLabel.Text = placeholder;
            _placeholderLabel.SizeToFit();
            _placeholderLabel.Frame = new CGRect(Bounds.Width / 2 - _placeholderLabel.Frame.Size.Width / 2,
                                           Bounds.Height / 2 - _placeholderLabel.Frame.Size.Height / 2,
                                           _placeholderLabel.Frame.Size.Width,
                                           _placeholderLabel.Frame.Size.Height);
        }
    }
}
