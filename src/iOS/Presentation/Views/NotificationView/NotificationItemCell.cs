using System;
using Foundation;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using PrankChat.Mobile.Core.Presentation.ViewModels.Notification.Items;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Converters;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.NotificationView
{
    public partial class NotificationItemCell : BaseTableCell<NotificationItemCell, NotificationItemViewModel>
    {
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
                .WithConversion<PlaceholderColorConverter>()
                .Mode(MvxBindingMode.OneTime);

            set.Bind(profileNameLabel)
                .To(vm => vm.ProfileName)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(descriptionLabel)
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
    }
}

