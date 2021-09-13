using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.ViewModels.Common;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Presentation.Views.Base;

namespace PrankChat.Mobile.iOS.Presentation.Views.Maintanance
{
    [MvxRootPresentation]
    public partial class MaintananceView : BaseTransparentBarView<MaintananceViewModel>
    {
        protected override void SetupControls()
        {
            base.SetupControls();

            downloadButton.SetLightStyle(Resources.DownloadUpdate);
            titleLabel.Font = Theme.Font.RegularFontOfSize(14);
            titleLabel.TextColor = Theme.Color.White;
            titleLabel.Text = Resources.ApplicationNewVersionReady;
        }

        protected override void Bind()
        {
            base.Bind();
            using var bindingSet = this.CreateBindingSet<MaintananceView, MaintananceViewModel>();

            bindingSet.Bind(downloadButton).To(vm => vm.OpenInBrowserCommand);
        }
    }
}

