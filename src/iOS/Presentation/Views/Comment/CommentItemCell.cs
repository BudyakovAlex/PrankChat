using System;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using PrankChat.Mobile.Core.Presentation.ViewModels.Comment.Items;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Views.Base;

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

            profileNameLabel.SetBoldTitleStyle();
            commentDateLabel.SetSmallSubtitleStyle();
            commentLabel.SetTitleStyle();
        }

        protected override void Bind()
        {
            using var bindingSet = this.CreateBindingSet<CommentItemCell, CommentItemViewModel>();

            bindingSet.Bind(profileImageView).For(v => v.ImagePath).To(vm => vm.ProfilePhotoUrl)
                      .Mode(MvxBindingMode.OneTime);
            bindingSet.Bind(profileImageView).For(v => v.BindTap()).To(vm => vm.OpenUserProfileCommand);
            bindingSet.Bind(profileNameLabel).To(vm => vm.ProfileName)
                      .Mode(MvxBindingMode.OneTime);
            bindingSet.Bind(commentDateLabel).To(vm => vm.DateText)
                      .Mode(MvxBindingMode.OneTime);
            bindingSet.Bind(commentLabel).To(vm => vm.Comment)
                      .Mode(MvxBindingMode.OneTime);
            bindingSet.Bind(profileImageView).For(v => v.ImagePath).To(vm => vm.ProfilePhotoUrl)
                      .Mode(MvxBindingMode.OneTime);
            bindingSet.Bind(profileImageView).For(v => v.PlaceholderText).To(vm => vm.ProfileShortName)
                      .Mode(MvxBindingMode.OneTime);
        }
    }
}

