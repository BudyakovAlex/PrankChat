using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views.Gestures;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Dialogs;
using PrankChat.Mobile.iOS.AppTheme;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Dialogs.Share
{
    public partial class ShareController : BaseDialog<ShareDialogViewModel>
    {
        protected override void Bind()
        {
            var setBind = this.CreateBindingSet<ShareController, ShareDialogViewModel>();

            setBind.Bind(copyLinkImageButton)
                .To(vm => vm.CopyLinkCommand);

            setBind.Bind(copyLinkTitleLabel.Tap())
                .For(v => v.Command)
                .To(vm => vm.CopyLinkCommand);

            setBind.Bind(shareInstagramImageButton)
                .To(vm => vm.ShareToInstagramCommand);

            setBind.Bind(shareInstagramTitleLabel.Tap())
                .For(v => v.Command)
                .To(vm => vm.ShareToInstagramCommand);

            setBind.Bind(cancelButton)
                .To(vm => vm.CloseCommand);

            setBind.Bind(shareButton)
                .To(vm => vm.ShareCommand);

            setBind.Apply();
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

