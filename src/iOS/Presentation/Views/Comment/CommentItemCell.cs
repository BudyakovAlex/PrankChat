using System;
using Foundation;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using PrankChat.Mobile.Core.Presentation.ViewModels.Comment.Items;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Comment
{
    public partial class CommentItemCell : BaseTableCell<CommentItemCell, CommentItemViewModel>
    {
        static CommentItemCell()
        {
            EstimatedHeight = 50;
        }

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
                .For(v => v.DownsampleWidth)
                .To(vm => vm.DownsampleWidth)
                .Mode(MvxBindingMode.OneTime);

            set.Bind(profileImageView)
                .For(v => v.Transformations)
                .To(vm => vm.Transformations)
                .Mode(MvxBindingMode.OneTime);

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

            set.Apply();
        }
    }
}

