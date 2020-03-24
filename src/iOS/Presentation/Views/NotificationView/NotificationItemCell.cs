using System;
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
        private NSLayoutConstraint _leftAnchorTitleLabelConstraint;
        private NSLayoutConstraint _topAnchorDateCreateConstraint;

        public string ProfileName
        {
            get => profileNameLabel.Text;
            set
            {
                profileNameLabel.Text = value;
                UpdateConstraintStatus(value, _leftAnchorTitleLabelConstraint);
            }
        }

        public string NotificationDescription
        {
            get => descriptionLabel.Text;
            set
            {
                descriptionLabel.Text = value;
                UpdateConstraintStatus(value, _topAnchorDateCreateConstraint);
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

            profileNameLabel.SetSmallTitleStyle();
            descriptionLabel.SetSmallTitleStyle();
            dateLabel.SetSmallSubtitleStyle();
            titleLabel.SetSmallTitleStyle();

            _leftAnchorTitleLabelConstraint = titleLabel.LeadingAnchor.ConstraintEqualTo(profileNameLabel.TrailingAnchor);
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

            set.Bind(titleLabel)
                .To(vm => vm.Title)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(descriptionLabel)
                .To(vm => vm.Description)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(descriptionLabel)
                .For(v => v.BindVisibility())
                .To(vm => vm.Description)
                .WithConversion<MvxVisibilityValueConverter>();

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

            set.Apply();
        }

        private void UpdateConstraintStatus(string text, NSLayoutConstraint constraint)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                constraint.Active = true;
            }
            else
            {
                constraint.Active = false;
            }
        }
    }
}

