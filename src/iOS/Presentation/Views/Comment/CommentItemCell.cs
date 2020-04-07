using System;
using Foundation;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using PrankChat.Mobile.Core.Presentation.ViewModels.Comment.Items;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Converters;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Comment
{
    public partial class CommentItemCell : BaseTableCell<CommentItemCell, CommentItemViewModel>
    {
        protected CommentItemCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        protected override void SetupControls()
        {
            base.SetupControls();

            profileNameLabel.SetMainTitleStyle();
            commentDateLabel.SetSmallSubtitleStyle();
        }

        protected override void SetBindings()
        {
            var set = this.CreateBindingSet<CommentItemCell, CommentItemViewModel>();

            set.Bind(profileImageView)
                .For(v => v.ImagePath)
                .To(vm => vm.ProfilePhotoUrl)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(profileNameLabel)
                .To(vm => vm.ProfileName)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(commentDateLabel)
                .To(vm => vm.DateText)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(profileShortName)
                .To(vm => vm.ProfileShortName)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(profileShortName)
                .For(v => v.BindHidden())
                .To(vm => vm.ProfilePhotoUrl)
                .Mode(MvxBindingMode.OneTime);

            set.Apply();
        }
    }
}

