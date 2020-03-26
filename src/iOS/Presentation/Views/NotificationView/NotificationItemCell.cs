﻿using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using CoreFoundation;
using Foundation;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Converters;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Binding.Views.Gestures;
using MvvmCross.Plugin.Visibility;
using PrankChat.Mobile.Core.Converters;
using PrankChat.Mobile.Core.Presentation.ViewModels.Notification.Items;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Converters;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.NotificationView
{
    public partial class NotificationItemCell : BaseTableCell<NotificationItemCell, NotificationItemViewModel>
    {
        private NSLayoutConstraint _topAnchorDateCreateConstraint;

        private string _profileName;
        public string ProfileName
        {
            get => _profileName;
            set
            {
                _profileName = value;
                UpdateTitleLabel();
            }
        }

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                UpdateTitleLabel();
            }
        }

        public string NotificationDescription
        {
            get => descriptionLabel.Text;
            set
            {
                descriptionLabel.Text = value;
                if (string.IsNullOrWhiteSpace(value))
                {
                    _topAnchorDateCreateConstraint.Active = true;
                }
                else
                {
                    _topAnchorDateCreateConstraint.Active = false;
                }
            }
        }

        static NotificationItemCell()
        {
            EstimatedHeight = 56;
        }

        protected NotificationItemCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        protected override void SetupControls()
        {
            base.SetupControls();

            descriptionLabel.SetSmallTitleStyle();
            dateLabel.SetSmallSubtitleStyle();
            profileNameAndTitleLabel.SetSmallTitleStyle();

            _topAnchorDateCreateConstraint = dateLabel.TopAnchor.ConstraintEqualTo(descriptionLabel.TopAnchor);
        }

        protected override void SetBindings()
        {
            var set = this.CreateBindingSet<NotificationItemCell, NotificationItemViewModel>();

            set.Bind(profilePhotoImageView)
                .For(v => v.ImagePath)
                .To(vm => vm.ImageUrl)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(profilePhotoImageView.Tap())
                .For(v => v.Command)
                .To(vm => vm.ShowUserProfileCommand);

            set.Bind(profilePhotoImageView)
                .For(v => v.PlaceholderText)
                .To(vm => vm.ProfileShortName)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(profileNameAndTitleLabel)
                .To(vm => vm.Title)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(dateLabel)
                .To(vm => vm.DateText)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(isReadedView)
                .For(v => v.BindVisibility())
                .To(vm => vm.IsDelivered)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(this)
                .For(v => v.ProfileName)
                .To(vm => vm.ProfileName)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(this)
                .For(v => v.NotificationDescription)
                .To(vm => vm.Description)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(this)
                .For(v => v.Title)
                .To(vm => vm.Title)
                .Mode(MvxBindingMode.OneTime);

            set.Apply();
        }

        private void UpdateTitleLabel()
        {
            var haveProfileName = !string.IsNullOrWhiteSpace(_profileName);
            var text = string.Join(haveProfileName ? "  " : "", _profileName, _title);
            var attributedString = new NSMutableAttributedString(text);
            profileNameAndTitleLabel.AttributedText = attributedString;
            var paragraphStyle = new NSMutableParagraphStyle();
            paragraphStyle.LineSpacing = 2.5f;
            attributedString.AddAttribute(UIStringAttributeKey.ParagraphStyle, paragraphStyle, new NSRange(0, text.Length));

            if (haveProfileName)
            {
                attributedString.AddAttribute(UIStringAttributeKey.Font, Theme.Font.BoldOfSize(Theme.Font.MediumFontSize), new NSRange(0, _profileName.Length));
            }

            profileNameAndTitleLabel.AttributedText = attributedString;
        }
    }
}

