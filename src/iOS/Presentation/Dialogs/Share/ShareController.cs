using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views.Gestures;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.ViewModels.Dialogs;
using PrankChat.Mobile.iOS.AppTheme;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Dialogs.Share
{
    public partial class ShareController : BaseDialog<ShareDialogViewModel>
    {
        protected override void Bind()
        {
            using var bindingSet = this.CreateBindingSet<ShareController, ShareDialogViewModel>();

            bindingSet.Bind(copyLinkImageButton).To(vm => vm.CopyLinkCommand);
            bindingSet.Bind(copyLinkTitleLabel.Tap()).For(v => v.Command).To(vm => vm.CopyLinkCommand);
            bindingSet.Bind(shareInstagramImageButton).To(vm => vm.ShareToInstagramCommand);
            bindingSet.Bind(shareInstagramTitleLabel.Tap()).For(v => v.Command).To(vm => vm.ShareToInstagramCommand);
            bindingSet.Bind(cancelButton).To(vm => vm.CloseCommand);
            bindingSet.Bind(shareButton).To(vm => vm.ShareCommand);
        }

        protected override void SetupControls()
        {
            shareTitleLabel.SetTitleStyle(Resources.ShareDialog_Title);

            shareButton.SetImage(UIImage.FromBundle("ic_share"), UIControlState.Normal);

            copyLinkImageButton.SetImage(UIImage.FromBundle("ic_share_copy").ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal), UIControlState.Normal);
            copyLinkTitleLabel.SetRegularStyle(10, Theme.Color.Black);
            copyLinkTitleLabel.Text = Resources.ShareDialog_CopyLink_Label;

            shareInstagramImageButton.SetImage(UIImage.FromBundle("ic_share_instagram").ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal), UIControlState.Normal);
            shareInstagramTitleLabel.SetRegularStyle(10, Theme.Color.Black);
            shareInstagramTitleLabel.Text = Resources.ShareDialog_ShareInstagram_Label;

            separatorView.BackgroundColor = Theme.Color.Separator;

            cancelButton.SetBorderlessStyle(Resources.Cancel);
        }
    }
}

