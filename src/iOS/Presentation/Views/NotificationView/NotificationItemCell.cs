using System;
using Foundation;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Binding.Views.Gestures;
using PrankChat.Mobile.Core.Presentation.ViewModels.Notification.Items;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Converters;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.NotificationView
{
    public partial class NotificationItemCell : BaseTableCell<NotificationItemCell, NotificationItemViewModel>
    {
        private string _profileName;
        public string ProfileName
        {
            get => _profileName;
            set
            {
                _profileName = value;
                SetDataToLabel();
            }
        }

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                SetDataToLabel();
            }
        }

        private string _descriptionText;
        public string DescriptionText
        {
            get => _descriptionText;
            set
            {
                _descriptionText = value;
                SetDataToLabel();
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

            profileNameAndDescriptionLabel.SetSmallTitleStyle();
            dateLabel.SetSmallSubtitleStyle();
            statusLabel.SetSmallSubtitleStyle();
        }

        protected override void SetBindings()
        {
            var set = this.CreateBindingSet<NotificationItemCell, NotificationItemViewModel>();

            set.Bind(profilePhotoImageView)
                .For(v => v.DownsampleWidth)
                .To(vm => vm.DownsampleWidth)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(profilePhotoImageView)
                .For(v => v.Transformations)
                .To(vm => vm.Transformations)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(profilePhotoImageView)
                .For(v => v.ImagePath)
                .To(vm => vm.ImageUrl)
                .WithConversion<PlaceholderImageConverter>()
                .Mode(MvxBindingMode.OneTime);

            set.Bind(profilePhotoImageView.Tap())
                .For(v => v.Command)
                .To(vm => vm.ShowUserProfileCommand);

            set.Bind(this)
                .For(v => v.ProfileName)
                .To(vm => vm.ProfileName)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(this)
                .For(v => v.Title)
                .To(vm => vm.Title)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(this)
                .For(v => v.DescriptionText)
                .To(vm => vm.Description)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(dateLabel)
                .To(vm => vm.DateText)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(statusLabel)
                .To(vm => vm.Status)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(profileShortNameLabel)
                .To(vm => vm.ProfileShortName)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(profileShortNameLabel)
                .For(v => v.BindHidden())
                .To(vm => vm.ImageUrl)
                .Mode(MvxBindingMode.OneTime);

            set.Apply();
        }

        private void SetDataToLabel()
        {
            string s = SetString("", _profileName);
            s = SetString(s, _title, "  ");
            int length = s.Length;
            s = SetString(s, _descriptionText, $"{Environment.NewLine}{Environment.NewLine}");

            var attributedString = new NSMutableAttributedString(s);
            attributedString.AddAttribute(UIStringAttributeKey.Font, UIFont.SystemFontOfSize(profileNameAndDescriptionLabel.Font.PointSize, UIFontWeight.Bold), new NSRange(0, _profileName?.Length ?? 0));
            if (s.Length > length)
                attributedString.AddAttribute(UIStringAttributeKey.Font, UIFont.SystemFontOfSize(6), new NSRange(length, 2));

            profileNameAndDescriptionLabel.AttributedText = attributedString;
        }

        private string SetString(string first, string second = null, string delimiter = "")
        {
            if (!string.IsNullOrWhiteSpace(second))
            {
                if (!string.IsNullOrWhiteSpace(first))
                    first += delimiter;
                first += second;
            }
            return first;
        }
    }
}

