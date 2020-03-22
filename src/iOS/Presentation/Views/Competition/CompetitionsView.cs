using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using PrankChat.Mobile.Core.Presentation.ViewModels.Competition;
using PrankChat.Mobile.iOS.AppTheme;
using PrankChat.Mobile.iOS.Infrastructure.Helpers;
using PrankChat.Mobile.iOS.Presentation.SourcesAndDelegates;
using PrankChat.Mobile.iOS.Presentation.Views.Base;
using UIKit;

namespace PrankChat.Mobile.iOS.Presentation.Views.Competition
{
    [MvxTabPresentation(TabName = "Competitions", TabIconName = "unselected", TabSelectedIconName = "selected", WrapInNavigationController = true)]
    public partial class CompetitionsView : BaseTabbedView<CompetitionsViewModel>
    {
        private TableViewSource _source;

        protected override void SetupControls()
        {
            base.SetupControls();

            NavigationController.NavigationBar.SetNavigationBarStyle();

            var logoButton = NavigationItemHelper.CreateBarButton("ic_logo", null);
            logoButton.Enabled = false;
            NavigationItem.LeftBarButtonItem = logoButton;

            _source = new TableViewSource(tableView)
                .Register<CompetitionsSectionViewModel>(CompetitionsSectionCell.Nib, CompetitionsSectionCell.CellId);

            tableView.Source = _source;
            tableView.ContentInset = new UIEdgeInsets(16f, 0, 0f, 0f);
        }

        protected override void SetupBinding()
        {
            base.SetupBinding();

            var bindingSet = this.CreateBindingSet<CompetitionsView, CompetitionsViewModel>();

            bindingSet.Bind(_source)
                      .For(v => v.ItemsSource)
                      .To(vm => vm.Items);

            bindingSet.Apply();
        }
    }
}
