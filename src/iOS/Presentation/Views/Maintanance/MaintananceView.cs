using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Localization;
using PrankChat.Mobile.Core.Presentation.ViewModels.Common;
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

            downloadButton.SetLightStyle(Resources.Download_Update);
            titleLabel.Font = Theme.Font.RegularFontOfSize(14);
            titleLabel.TextColor = Theme.Color.White;
            titleLabel.Text = Resources.Application_New_Version_Ready;
        }

        protected override void SetupBinding()
        {
            base.SetupBinding();

            var bindingSet = this.CreateBindingSet<MaintananceView, MaintananceViewModel>();

            bindingSet.Bind(downloadButton)
                      .To(vm => vm.OpenInBrowserCommand);

            bindingSet.Apply();
        }
    }
}

